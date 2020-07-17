using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class CefResourceHandlerFactory : IResourceHandlerFactory
	{

		private readonly WebView OwnerWebView;
		public CefResourceHandlerFactory(WebView webView)
		{
			this.OwnerWebView = webView;
		}

		public bool HasHandlers
		{
			get
			{
				return true;
			}
		} 

		public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
		{
			if (request.Url == this.OwnerWebView.DefaultLocalUrl)
			{
				if (this.OwnerWebView.htmlToLoad == null)
				{
					return null;
				}
				return CefSharp.ResourceHandler.FromString(this.OwnerWebView.htmlToLoad, Encoding.UTF8, true, "text/html");
			}
			else
			{
				if (this.OwnerWebView.FilterRequest(request))
				{
					return null;
				}
				ResourceHandler resourceHandler = new ResourceHandler(request, this.OwnerWebView.GetRequestUrl(request));
				Uri uri;
				if (Uri.TryCreate(resourceHandler.Url, UriKind.Absolute, out uri) && uri.Scheme == "embedded")
				{
					UriBuilder urlWithoutQuery = new UriBuilder(uri);
					if (uri.Query != "")
					{
						urlWithoutQuery.Query = "";
					}
					this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
					{
						this.OwnerWebView.LoadEmbeddedResource(resourceHandler, urlWithoutQuery.Uri);
					});
				}
				this.OwnerWebView.DoBeforeResourceLoadExecuteWithAsyncErrorHandling(resourceHandler); //运行加载之前执行事件
				if (resourceHandler.Handled)
				{
					return resourceHandler.Handler;
				}
				if (!this.OwnerWebView.IgnoreMissingResources && uri != null && uri.Scheme == "embedded")
				{
					this.OwnerWebView.DoResourceLoadFailed(request.Url);  //运行加载失败后执行事件
				}
				return null;
			}
		} 

	}
}
