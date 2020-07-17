using CefSharp;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class JavascriptExecutor : IDisposable
	{
		public JavascriptExecutor(WebView ownerWebView)
		{
			this.OwnerWebView = ownerWebView; 
			this.OwnerWebView.AddJavascriptContextCreatedEvent(this.OnJavascriptContextCreated);
			this.OwnerWebView.AddRenderProcessCrashedEvent( this.StopFlush);
		}

		private void OnJavascriptContextCreated()
		{ 
			this.OwnerWebView.RemoveJavascriptContextCreatedEvent(this.OnJavascriptContextCreated);
			Task.Factory.StartNew(new Action(this.FlushScripts), this.flushTaskCancelationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		private void StopFlush()
		{
			this.flushTaskCancelationToken.Cancel();
			if (this.flushRunning)
			{
				this.stoppedFlushHandle.WaitOne();
			}
			this.flushTaskCancelationToken.Dispose();
		}

		private void EnsureWebViewNotDisposing()
		{
			if (this.OwnerWebView.isDisposing)
			{
				throw new InvalidOperationException("Webview is disposing");
			}
		}

		private ScriptTask QueueScript(string script, TimeSpan? timeout = null, bool awaitable = false)
		{
			this.EnsureWebViewNotDisposing();
			 ScriptTask scriptTask = new  ScriptTask(script, timeout, awaitable);
			this.pendingScripts.Add(scriptTask);
			return scriptTask;
		}

		private void FlushScripts()
		{
			this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
			{
				try
				{
					this.flushRunning = true;
					while (!this.flushTaskCancelationToken.IsCancellationRequested)
					{
						this.InnerFlushScripts();
					}
				}
				catch (OperationCanceledException)
				{
				}
				finally
				{
					this.flushRunning = false;
					this.stoppedFlushHandle.Set();
				}
			});
		}

		private void InnerFlushScripts()
		{
			 ScriptTask scriptTask = null;
			List< ScriptTask> list = new List< ScriptTask>();
			 ScriptTask scriptTask2;
			for (; ; )
			{
				scriptTask2 = this.pendingScripts.Take(this.flushTaskCancelationToken.Token);
				if (scriptTask2.WaitHandle != null)
				{
					break;
				}
				list.Add(scriptTask2);
				if (this.pendingScripts.Count <= 0)
				{
					goto IL_42;
				}
			}
			scriptTask = scriptTask2;
		IL_42:
			if (list.Count > 0)
			{
				Task<JavascriptResponse> task = this.OwnerWebView.chromium.EvaluateScriptAsync( JavascriptExecutor.WrapScriptWithErrorHandling(string.Join(";" + Environment.NewLine, from s in list
					select s.Script)), this.OwnerWebView.DefaultScriptsExecutionTimeout);
				task.Wait(this.flushTaskCancelationToken.Token);
				JavascriptResponse response = task.Result;
				if (!response.Success)
				{
					this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
					{
						throw JavascriptExecutor.ParseResponseException(response);
					});
				}
			}
			if (scriptTask != null)
			{
				Task<JavascriptResponse> task2 = null;
				try
				{
					IWebBrowser chromium = this.OwnerWebView.chromium;
					string script = scriptTask.Script;
					TimeSpan? timeout = scriptTask.Timeout;
					task2 = chromium.EvaluateScriptAsync(script, (timeout != null) ? timeout : this.OwnerWebView.DefaultScriptsExecutionTimeout);
					task2.Wait(this.flushTaskCancelationToken.Token);
					scriptTask.Result = task2.Result;
				}
				catch (Exception exception)
				{
					if (task2 == null || !task2.IsCanceled)
					{
						scriptTask.Exception = exception;
					}
				}
				finally
				{
					scriptTask.WaitHandle.Set();
				}
			}
		}

		public T EvaluateScript<T>(string script, TimeSpan? timeout = null)
		{
			this.EnsureWebViewNotDisposing();
			string script2 = JavascriptExecutor.WrapScriptWithErrorHandling(script);
			 ScriptTask scriptTask = this.QueueScript(script2, timeout, true);
			if (!this.flushRunning)
			{
				if (!scriptTask.WaitHandle.WaitOne(timeout ?? TimeSpan.FromSeconds(15.0)) || scriptTask.Result == null)
				{
					throw new JavascriptException("Timeout", "Javascript engine is not initialized", null);
				}
			}
			else if (!scriptTask.WaitHandle.WaitOne() || scriptTask.Result == null)
			{
				throw new JavascriptException("Timeout", ((timeout != null) ? string.Format("More than {0}ms elapsed", timeout.Value.TotalMilliseconds) : "Timeout ocurred") + string.Format(" evaluating the script: '{0}'", script), null);
			}
			if (scriptTask.Exception != null)
			{
				throw scriptTask.Exception;
			}
			if (scriptTask.Result.Success)
			{
				return this.GetResult<T>(scriptTask.Result.Result);
			}
			throw JavascriptExecutor.ParseResponseException(scriptTask.Result);
		}

		public T EvaluateScriptFunction<T>(string functionName, bool serializeParams, params object[] args)
		{
			return this.EvaluateScript<T>( JavascriptExecutor.MakeScript(functionName, serializeParams, args), null);
		}

		public void ExecuteScriptFunction(string functionName, bool serializeParams, params object[] args)
		{
			this.QueueScript( JavascriptExecutor.MakeScript(functionName, serializeParams, args), null, false);
		}

		public void ExecuteScript(string script)
		{
			this.QueueScript(script, null, false);
		}

		private T GetResult<T>(object result)
		{
			Type typeFromHandle = typeof(T);
			if ( JavascriptExecutor.IsBasicType(typeFromHandle))
			{
				if (result == null)
				{
					return default(T);
				}
				return (T)((object)result);
			}
			else
			{
				if (result == null && typeFromHandle.IsArray)
				{
					return (T)((object)Array.CreateInstance(typeFromHandle.GetElementType(), 0));
				}
				return (T)((object)this.OwnerWebView.binder.Bind(result, typeFromHandle));
			}
		}

		public void Dispose()
		{
			this.StopFlush();
		}

		private static bool IsBasicType(Type type)
		{
			return type.IsPrimitive || type.IsEnum || type == typeof(string);
		}

		private static string MakeScript(string functionName, bool serializeParams, object[] args)
		{
			IEnumerable<string> values = from a in args
										 select JavascriptExecutor.Serialize(a, serializeParams);
			return functionName + "(" + string.Join(",", values) + ")";
		}

		private static string Serialize(object value, bool serializeValue)
		{
			if (value == null)
			{
				return "null";
			}
			if (serializeValue)
			{
				string value2;
				if ((value2 = (value as string)) != null)
				{
					return HttpUtility.JavaScriptStringEncode(value2, true);
				}
				IEnumerable source;
				if ((source = (value as IEnumerable)) != null)
				{
					return "[" + string.Join(",", from object v in source
												  select JavascriptExecutor.Serialize(v, serializeValue)) + "]";
				}
			}
			return value.ToString();
		}

		private static string WrapScriptWithErrorHandling(string script)
		{
			return "try {" + script + Environment.NewLine + "} catch (e) { throw JSON.stringify({ stack: e.stack, message: e.message, name: e.name }) + '|WebViewInternalException' }";
		}

		private static T DeserializeJSON<T>(string json)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(JsError));
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				result = (T)((object)dataContractJsonSerializer.ReadObject(memoryStream));
			}
			return result;
		}

		private static Exception ParseResponseException(JavascriptResponse response)
		{
			string text = response.Message;
			text = text.Substring(Math.Max(0, text.IndexOf("{")));
			text = text.Substring(0, text.LastIndexOf("}") + 1);
			if (!string.IsNullOrEmpty(text))
			{
				JsError jsError = null;
				try
				{
					jsError = JavascriptExecutor.DeserializeJSON<JsError>(text);
				}
				catch
				{
				}
				if (jsError != null)
				{
					jsError.Name = (jsError.Name ?? "");
					jsError.Message = (jsError.Message ?? "");
					jsError.Stack = (jsError.Stack ?? "");
					string[] array = jsError.Stack.Substring(Math.Min(jsError.Stack.Length, (jsError.Name + ": " + jsError.Message).Length)).Split(new char[]
					{
							'\n'
					}, StringSplitOptions.RemoveEmptyEntries);
					List<JavascriptStackFrame> list = new List<JavascriptStackFrame>();
					foreach (string input in array)
					{
						Match match = JavascriptExecutor.StackFrameRegex.Match(input);
						if (match.Success)
						{
							list.Add(new JavascriptStackFrame
							{
								FunctionName = match.Groups["method"].Value,
								SourceName = match.Groups["location"].Value,
								LineNumber = int.Parse(match.Groups["line"].Value),
								ColumnNumber = int.Parse(match.Groups["column"].Value)
							});
						}
					}
					return new JavascriptException(jsError.Name, jsError.Message, list.ToArray());
				}
			}
			return new JavascriptException("Javascript Error", response.Message, null);
		}

		internal static bool IsInternalException(string exceptionMessage)
		{
			return exceptionMessage.EndsWith("|WebViewInternalException");
		}

		private static readonly Regex StackFrameRegex = new Regex("at\\s*(?<method>.*?)\\s(?<location>[^\\s]+):(?<line>\\d+):(?<column>\\d+)", RegexOptions.Compiled);

		private const string InternalException = "|WebViewInternalException";

		private readonly WebView OwnerWebView;

		private readonly BlockingCollection<ScriptTask> pendingScripts = new BlockingCollection<ScriptTask>();

		private readonly CancellationTokenSource flushTaskCancelationToken = new CancellationTokenSource();

		private readonly ManualResetEvent stoppedFlushHandle = new ManualResetEvent(false);

		private volatile bool flushRunning;
	}
}
