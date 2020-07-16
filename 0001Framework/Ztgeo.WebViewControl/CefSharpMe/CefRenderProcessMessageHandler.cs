using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
    internal class CefRenderProcessMessageHandler : IRenderProcessMessageHandler
	{
		public CefRenderProcessMessageHandler(WebView webView)
		{
			this.OwnerWebView = webView;
		}

		private static bool IgnoreEvent(IFrame frame)
		{
			return frame.Url.StartsWith("chrome-devtools:", StringComparison.InvariantCultureIgnoreCase);
		}

		public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
		{
			if (!CefRenderProcessMessageHandler.IgnoreEvent(frame))
			{
				Action javascriptContextCreated = this.OwnerWebView.JavascriptContextCreated;
				if (javascriptContextCreated != null)
				{
					this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
					{
						javascriptContextCreated();
					});
				}
			}
		}

		public void OnContextReleased(IWebBrowser browserControl, IBrowser browser, IFrame frame)
		{
			if (!CefRenderProcessMessageHandler.IgnoreEvent(frame))
			{
				Action javascriptContextReleased = this.OwnerWebView.JavascriptContextReleased;
				if (javascriptContextReleased != null)
				{
					this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
					{
						javascriptContextReleased();
					});
				}
			}
		}

		public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
		{
		}

		public void OnUncaughtException(IWebBrowser browserControl, IBrowser browser, IFrame frame, CefSharp.JavascriptException exception)
		{
			if (JavascriptExecutor.IsInternalException(exception.Message))
			{
				return;
			}
			JavascriptException e = new JavascriptException(exception.Message, exception.StackTrace);
			this.OwnerWebView.ForwardUnhandledAsyncException(e);
		}

		private readonly WebView OwnerWebView;
	}
}
