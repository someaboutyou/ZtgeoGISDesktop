using Abp.Events.Bus;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.Configuration;
using Ztgeo.Gis.Hybrid.JsBinder; 
using Ztgeo.Gis.Share;
using Ztgeo.Utils;
using Ztgeo.WebViewControl;

namespace Ztgeo.Gis.Hybrid
{
	internal static class WebViewExtensions
	{ 
		internal static HybridConfiguration HybridConfiguration { get; set; }
		 
		internal static void SetupWebView<TApi>(this WebView webView, string editorName, bool readOnly, out IBindableJSContextProvider jsCtx, Func<IBindableJSContextProvider, TApi> newJsApi, out TApi jsApi, out CommonAPI jsCommonApi) where TApi : class
		{
			webView.SetupWebView(editorName, readOnly, false, out jsCtx, newJsApi, out jsApi, out jsCommonApi);
		}
		 
		internal static void SetupWebView<TApi>(this WebView webView, string editorName, bool readOnly, bool oldApi, out IBindableJSContextProvider jsCtx, Func<IBindableJSContextProvider, TApi> newJsApi, out TApi jsApi, out CommonAPI jsCommonApi)
		{
			webView.SetupWebView(out jsCtx, newJsApi, out jsApi, out jsCommonApi);
			webView.LoadResource(new ResourceUrl(typeof(WebViewExtensions).Assembly, new string[]
			{
				"WebViews",
				"Common",
				string.Concat(new string[]
				{
					"webview",
					oldApi ? "-old" : "",
					".html?render=",
					editorName,
					"&readOnly=",
					readOnly ? "true" : "false"
				})
			}));
		}
		 
		internal static void SetupWebView<TApi>(this WebView webView, out IBindableJSContextProvider jsCtx, Func<IBindableJSContextProvider, TApi> newJsApi, out TApi jsApi, out CommonAPI jsCommonApi)
		{
			webView.IsHistoryDisabled = true;
			webView.AllowDeveloperTools = true;
			webView.DisableBuiltinContextMenus = true;
			//webView.BeforeResourceLoad += WebViewExtensions.OnWebViewBeforeResourceLoad;
			webView.AddBeforeResourceLoadEvent(WebViewExtensions.OnWebViewBeforeResourceLoad);
			webView.DefaultScriptsExecutionTimeout = (HybridConfiguration.DisableWebViewExecutionTimeouts ? null : new TimeSpan?(WebViewExtensions.DefaultScriptExecutionTimeout));
			//webView.FindLogicalParent<IDisposable>().MustBeSet("The webview must belong to a IDisposable view and be disposed");
			if (HybridConfiguration.ShowDeveloperTools)
			{
				webView.ShowDeveloperTools();
			} 
			jsCtx = new DocumentReadyJSApi(webView);
			jsApi = newJsApi(jsCtx);
			jsCommonApi = new CommonAPI(jsCtx);
		} 
		private static void OnWebViewBeforeResourceLoad(WebViewControl.CefSharpMe.ResourceHandler resourceHandler)
		{
			Uri uri;
			if (Uri.TryCreate(resourceHandler.Url, UriKind.Absolute, out uri))
			{
				string text = string.Empty;
				string customWebViewResourcePath = HybridConfiguration.CustomWebViewResourcePath;
				if (!customWebViewResourcePath.IsEmpty())
				{
					text = Path.Combine(customWebViewResourcePath, uri.AbsolutePath.Substring(uri.AbsolutePath.IndexOf(";") + 1).Replace('/', '\\').TrimStart(new char[]
					{
						'\\'
					}));
					try
					{
						if (!File.Exists(text))
						{
							resourceHandler.Cancel();
							return;
						}
						resourceHandler.RespondWith(text);
					}
					catch
					{
						resourceHandler.Cancel();
						return;
					}
				}
				if (uri.AbsolutePath.EndsWith(".ico"))
				{
					double size = double.Parse(uri.Query.Substring(uri.Query.IndexOf("=") + 1));
					Stream manifestResourceStream = resourceHandler.Response;
					if (!text.IsEmpty())
					{
						manifestResourceStream = File.OpenRead(text);
					}
					using (MemoryStream ms = GetIconBitmap(manifestResourceStream, size)) { 
						resourceHandler.RespondWith(ms, "png");
					}
				}
			}
		}
		 
		private static MemoryStream GetIconBitmap(Stream manifestResourceStream, double size)
		{
			//return manifestResourceStream.ToIconBitmap(size).ToImageStream(WpfExtensions.ImageStreamTypes.Png, null);
			MemoryStream memoryStream = new MemoryStream();
			Icon icon = new Icon(manifestResourceStream);
			icon.ToBitmap().Save(memoryStream, ImageFormat.Png);
			return memoryStream;
		}



		public static void RegisterJavascriptObjectWithErrorHandling(this WebView webView, string variableName, object objectToBind, bool executeInUIThread = true)
		{
			webView.RegisterJavascriptObject(variableName, objectToBind,
				(Func<object> originalFunc) => SafeExecute<object>(ExceptionType.InsideView, null, originalFunc),
				null,
				executeInUIThread);
		}
		 
		private static TResult SafeExecute<TResult>(ExceptionType exceptionType, Func<Exception, TResult> onError, Func<TResult> function)
		{
			TResult result;
			try
			{
				result = function();
			}
			catch (Exception ex)
			{
				//if (!Runtime.Instance.OnException(ex, exceptionType, presenterContext))
				//{
				//	throw;
				//}
				result = onError(ex);
				throw new RegisterJavascriptObjectException(ex.Message,ex);
			}
			return result;
		}

		private static readonly TimeSpan DefaultScriptExecutionTimeout = TimeSpan.FromSeconds(120.0);
		 
	}
}
