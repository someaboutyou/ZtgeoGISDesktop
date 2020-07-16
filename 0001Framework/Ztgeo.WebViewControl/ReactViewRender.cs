using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Ztgeo.WebViewControl.CefSharpMe;

namespace Ztgeo.WebViewControl
{
	internal class ReactViewRender : UserControl, IReactView, IDisposable, IExecutionEngine
	{ 
		public static bool UseEnhancedRenderingEngine { get; set; } = true;
		 
		public ReactViewRender(bool preloadWebView)
		{
			this.userCallingAssembly = WebView.GetUserCallingMethod(false).ReflectedType.Assembly;
			this.webView = new InternalWebView(this, preloadWebView)
			{
				DisableBuiltinContextMenus = true,
				IgnoreMissingResources = false
			};
			this.webView.AttachListener("Ready", delegate
			{
				this.IsReady = true;
			}, false);
			this.webView.Navigated += this.OnWebViewNavigated;
			this.webView.Disposed += this.OnWebViewDisposed;
			this.webView.BeforeResourceLoad += this.OnWebViewBeforeResourceLoad;
			this.webView.Dock = DockStyle.Fill;
			this.Controls.Add(this.webView);
			//base.Content = this.webView;
			string[] value = new string[]
			{
				ReactViewRender.UseEnhancedRenderingEngine ? "1" : "0",
				ReactViewRender.LibrariesPath,
				"__Modules__",
				"__WebviewListener__",
				"Ready"
			};
			this.webView.LoadResource(new ResourceUrl(typeof(ReactViewRender).Assembly, new string[]
			{
				ReactViewRender.DefaultUrl + "?" + string.Join("&", value)
			}));
		}
		 
		private void OnWebViewDisposed()
		{
			this.Dispose();
		}
		 
		public new void Dispose()
		{
			FileSystemWatcher fileSystemWatcher = this.fileSystemWatcher;
			if (fileSystemWatcher != null)
			{
				fileSystemWatcher.Dispose();
			}
			this.webView.Dispose();
		}
		 
		public event Action Ready
		{
			add
			{
				this.readyEventListener = this.webView.AttachListener("Ready", value, true);
			}
			remove
			{
				if (this.readyEventListener != null)
				{
					this.webView.DetachListener(this.readyEventListener);
				}
			}
		}
		 
		public event Action<CefException.UnhandledExceptionEventArgs> UnhandledAsyncException
		{
			add
			{
				this.webView.UnhandledAsyncException += value;
			}
			remove
			{
				this.webView.UnhandledAsyncException -= value;
			}
		}
		 
		public event Func<string, Stream> CustomResourceRequested;
		 
		public void LoadComponent(IViewModule component)
		{
			this.component = component;
			if (this.pageLoaded)
			{
				this.InternalLoadComponent();
			}
		}
		 
		private void InternalLoadComponent()
		{
			string text = ReactViewRender.NormalizeUrl(this.component.JavascriptSource);
			string[] array = text.Split(new string[]
			{
				"/"
			}, StringSplitOptions.None);
			int num = (array.Length >= 2) ? 2 : 1;
			string str = this.ToFullUrl(string.Join("/", array.Take(array.Length - num))) + "/";
			List<string> list = new List<string>
			{
				ReactViewRender.Quote(str),
				ReactViewRender.Array(new string[]
				{
					ReactViewRender.Quote(this.component.NativeObjectName),
					ReactViewRender.Quote(this.component.Name),
					ReactViewRender.Quote(text)
				})
			};
			if (this.DefaultStyleSheet != null)
			{
				list.Add(ReactViewRender.Quote(ReactViewRender.NormalizeUrl(this.ToFullUrl(this.DefaultStyleSheet.ToString()))));
			}
			else
			{
				list.Add("null");
			}
			list.Add(ReactViewRender.AsBoolean(this.enableDebugMode));
			list.Add(ReactViewRender.Quote(this.cacheInvalidationTimestamp));
			this.webView.RegisterJavascriptObject(this.component.NativeObjectName, this.component.CreateNativeObject(), null, null, false);
			IViewModule[] array2 = this.Plugins;
			if (array2 != null && array2.Length != 0)
			{
				IViewModule[] array3 = (from p in this.Plugins
										where !string.IsNullOrEmpty(p.NativeObjectName)
										select p).ToArray<IViewModule>();
				list.Add(ReactViewRender.Array(from m in array3
											   select ReactViewRender.Array(new string[]
											   {
					ReactViewRender.Quote(m.Name),
					ReactViewRender.Quote(m.NativeObjectName)
											   })));
				foreach (IViewModule viewModule in array3)
				{
					this.webView.RegisterJavascriptObject(viewModule.NativeObjectName, viewModule.CreateNativeObject(), null, null, false);
				}
				list.Add(ReactViewRender.Object(from m in this.Plugins
												select new KeyValuePair<string, string>(ReactViewRender.Quote(m.Name), ReactViewRender.Quote(ReactViewRender.NormalizeUrl(this.ToFullUrl(m.JavascriptSource))))));
			}
			this.ExecuteDeferredScriptFunction("load", list.ToArray());
			this.componentLoaded = true;
		}
		 
		private void OnWebViewNavigated(string obj)
		{
			this.IsReady = false;
			this.pageLoaded = true;
			if (this.component != null)
			{
				this.InternalLoadComponent();
			}
		}
		 
		public void ExecuteMethod(IViewModule module, string methodCall, params object[] args)
		{
			this.webView.ExecuteScriptFunctionWithSerializedParams("__Modules__." + module.Name + "." + methodCall, args);
		} 
		public T EvaluateMethod<T>(IViewModule module, string methodCall, params object[] args)
		{
			return this.webView.EvaluateScriptFunctionWithSerializedParams<T>("__Modules__." + module.Name + "." + methodCall, args);
		} 
		public ResourceUrl DefaultStyleSheet
		{
			get
			{
				return this.defaultStyleSheet;
			}
			set
			{
				if (this.componentLoaded)
				{
					throw new InvalidOperationException(string.Format("Cannot set {0} after component has been loaded", "DefaultStyleSheet"));
				}
				this.defaultStyleSheet = value;
			}
		} 
		public IViewModule[] Plugins
		{
			get
			{
				return this.plugins;
			}
			set
			{
				if (this.componentLoaded)
				{
					throw new InvalidOperationException(string.Format("Cannot set {0} after component has been loaded", "Plugins"));
				}
				IEnumerable<IViewModule> source = from p in value
												  where string.IsNullOrEmpty(p.JavascriptSource) || string.IsNullOrEmpty(p.Name)
												  select p;
				if (source.Any<IViewModule>())
				{
					string arg = source.First<IViewModule>().Name + "|" + source.First<IViewModule>().GetType().Name;
					throw new ArgumentException(string.Format("Plugin '{0}' is invalid", arg));
				}
				this.plugins = value;
				IViewModule[] array = this.plugins;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Bind(this);
				}
			}
		}
		 
		public T WithPlugin<T>()
		{
			return this.Plugins.OfType<T>().First<T>();
		}
		 
		public bool IsReady { get; private set; }

 		public void ShowDeveloperTools()
		{
			this.webView.ShowDeveloperTools();
		}

 		public void CloseDeveloperTools()
		{
			this.webView.CloseDeveloperTools();
		} 

		public bool EnableDebugMode
		{
			get
			{
				return this.enableDebugMode;
			}
			set
			{
				this.enableDebugMode = value;
				this.webView.AllowDeveloperTools = this.enableDebugMode;
				if (this.enableDebugMode)
				{
					this.webView.ResourceLoadFailed += this.ShowResourceLoadFailedMessage;
					return;
				}
				this.webView.ResourceLoadFailed -= this.ShowResourceLoadFailedMessage;
			}
		}

 		private void ShowResourceLoadFailedMessage(string url)
		{
			this.ShowErrorMessage("Failed to load resource '" + url + "'. Press F12 to open developer tools and see more details.");
		}

 		private void ShowErrorMessage(string msg)
		{
			msg = msg.Replace("\"", "\\\"");
			this.ExecuteDeferredScriptFunction("showErrorMessage", new string[]
			{
				ReactViewRender.Quote(msg)
			});
		}

 		private string ToFullUrl(string url)
		{
			if (url.Contains(Uri.SchemeDelimiter))
			{
				return url;
			}
			if (url.StartsWith("/"))
			{
				return new ResourceUrl("embedded", url).ToString();
			}
			return new ResourceUrl(this.userCallingAssembly, url).ToString();
		}

 		public void EnableHotReload(string baseLocation)
		{
			if (string.IsNullOrEmpty(baseLocation))
			{
				throw new InvalidOperationException("Hot reload does not work in release mode");
			}
			baseLocation = Path.GetDirectoryName(baseLocation);
			baseLocation = Path.GetFullPath(baseLocation + "\\..\\..");
			if (this.fileSystemWatcher != null)
			{
				this.fileSystemWatcher.Path = baseLocation;
				return;
			}
			this.fileSystemWatcher = new FileSystemWatcher(baseLocation);
			this.fileSystemWatcher.IncludeSubdirectories = true;
			this.fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
			this.fileSystemWatcher.EnableRaisingEvents = true;
			bool filesChanged = false;
			string[] fileExtensionsToWatch = new string[]
			{
				".js",
				".css"
			}; 
			this.fileSystemWatcher.Changed += delegate (object sender, FileSystemEventArgs eventArgs)
			{
				if (this.IsReady)
				{
					filesChanged = true; 
					this.webView.BeginInvoke(new Action(()=> {
						if (this.IsReady)
						{
							this.IsReady = false;
							this.cacheInvalidationTimestamp = DateTime.UtcNow.Ticks.ToString();
							this.webView.Reload(true);
						}
					}));
				}
			};
			this.webView.BeforeResourceLoad += delegate (CefSharpMe.ResourceHandler resourceHandler)
			{
				if (filesChanged)
				{
					Uri resourceUrl = new Uri(resourceHandler.Url);
					string path = Path.Combine(ResourceUrl.GetEmbeddedResourcePath(resourceUrl).Skip(1).ToArray<string>());
					if (fileExtensionsToWatch.Any((string e) => path.EndsWith(e)))
					{
						path = Path.Combine(this.fileSystemWatcher.Path, path);
						if (new FileInfo(path).Exists)
						{
							resourceHandler.RespondWith(path);
						}
					}
				}
			};
		}
		 
		private void ExecuteDeferredScriptFunction(string functionName, params string[] args)
		{
			this.webView.ExecuteScript(string.Format("setTimeout(() => {0}({1}), 0)", functionName, string.Join(",", args)));
		}
		 
		private static string Quote(string str)
		{
			return "\"" + str + "\"";
		}
		 
		private static string AsBoolean(bool value)
		{
			if (!value)
			{
				return "false";
			}
			return "true";
		}
		 
		private static string Array(params string[] elements)
		{
			return "[" + string.Join(",", elements) + "]";
		}
		 
		private static string Array(IEnumerable<string> elements)
		{
			return ReactViewRender.Array(elements.ToArray<string>());
		}
		 
		private static string Object(IEnumerable<KeyValuePair<string, string>> properties)
		{
			return "{" + string.Join(",", from p in properties
										  select p.Key + ":" + p.Value) + "}";
		}

		private static string NormalizeUrl(string url)
		{
			if (url.EndsWith(".js"))
			{
				url = url.Substring(0, url.Length - ".js".Length);
			}
			return url.Replace("\\", "/");
		}

		private void OnWebViewBeforeResourceLoad(CefSharpMe.ResourceHandler resourceHandler)
		{
			Func<string, Stream> customResourceRequested = this.CustomResourceRequested;
			if (customResourceRequested != null && resourceHandler.Url.StartsWith("custom" + Uri.SchemeDelimiter))
			{
				Task<Stream> task = Task.Run<Stream>(() => customResourceRequested(resourceHandler.Url));
				task.Wait(ReactViewRender.CustomRequestTimeout);
				if (!task.IsCompleted)
				{
					throw new Exception(string.Format("Failed to fetch ({0}) within the alotted timeout", resourceHandler.Url));
				}
				resourceHandler.RespondWith(task.Result, "");
			}
		} 
		 
		private const string JavascriptNullConstant = "null";
		 
		private const string ModulesObjectName = "__Modules__";
		 
		private const string ReadyEventName = "Ready";
		 
		internal static TimeSpan CustomRequestTimeout = TimeSpan.FromSeconds(5.0);
		 
		private static readonly Assembly Assembly = typeof(ReactViewRender).Assembly;
		 
		private static readonly string BuiltinResourcesPath = "Resources/";
		 
		private static readonly string DefaultUrl = string.Format("{0}index.html", ReactViewRender.BuiltinResourcesPath);
		 
		private static readonly string LibrariesPath = new ResourceUrl(ReactViewRender.Assembly, new string[]
		{
			string.Format("{0}node_modules/", ReactViewRender.BuiltinResourcesPath)
		}).ToString();
		 
		private readonly WebView webView;
		 
		private Assembly userCallingAssembly;
		 
		private bool enableDebugMode;
		 
		private Listener readyEventListener;
		 
		private bool pageLoaded;
		 
		private bool componentLoaded;
		 
		private IViewModule component;
		 
		private ResourceUrl defaultStyleSheet;
		 
		private IViewModule[] plugins;
		 
		private FileSystemWatcher fileSystemWatcher;
		 
		private string cacheInvalidationTimestamp;
		 
		private class InternalWebView : WebView
		{ 
			public InternalWebView(ReactViewRender owner, bool preloadBrowser)
			{
				this.owner = owner;
				base.IsSecurityDisabled = true;
				if (preloadBrowser)
				{
					base.InitializeBrowser();
				}
			}

			public override string GetRequestUrl(IRequest request)
			{
				if (request.ResourceType == ResourceType.Script)
				{
					UriBuilder uriBuilder = new UriBuilder(request.Url);
					if (!uriBuilder.Path.EndsWith(".js"))
					{
						UriBuilder uriBuilder2 = uriBuilder;
						uriBuilder2.Path += ".js";
						return uriBuilder.ToString();
					}
				}
				return base.GetRequestUrl(request);
			}
			 
			private const string JavascriptExtension = ".js";
			 
			private readonly ReactViewRender owner;
		}
	}
}
