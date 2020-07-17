using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.WebViewControl.CefException;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class CefRequestHandler : IRequestHandler
	{
		public CefRequestHandler(WebView webView)
		{
			this.OwnerWebView = webView;
		}

		bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
		{
			return false;
		}

		bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
		{
			return false;
		}

		bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
		{
			return false;
		}

		void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
		{
		}

		void IRequestHandler.OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
		{
		}

		bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			return false;
		}

		bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
		{
			if (this.OwnerWebView.ProxyAuthentication != null)
			{
				callback.Continue(this.OwnerWebView.ProxyAuthentication.UserName, this.OwnerWebView.ProxyAuthentication.Password);
			}
			return true;
		}

		IResponseFilter IRequestHandler.GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="browserControl"></param>
		/// <param name="browser"></param>
		/// <param name="frame"></param>
		/// <param name="request"></param>
		/// <param name="userGesture"></param>
		/// <param name="isRedirect"></param>
		/// <returns>false 会继续请求</returns>
		bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			if (this.OwnerWebView.FilterRequest(request))
			{
				return false;
			}
			if (this.OwnerWebView.IsHistoryDisabled && (request.TransitionType & TransitionType.ForwardBack) == TransitionType.ForwardBack)
			{
				return true;
			}
			return this.OwnerWebView.DoBeforeNavigate(request, this.OwnerWebView.GetRequestUrl(request)); 
		}

		CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
		{
			return CefReturnValue.Continue;
		}

		bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
		{
			if (this.OwnerWebView.IgnoreCertificateErrors)
			{
				callback.Continue(true);
				return true;
			}
			return false;
		}

		void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
		{
		}

		void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
		{
			this.OwnerWebView.DoRenderProcessCrashed(); 
			string reason = "";
			bool wasKilled = false;
			switch (status)
			{
				case CefTerminationStatus.AbnormalTermination:
					reason = "terminated with an unknown reason";
					break;
				case CefTerminationStatus.ProcessWasKilled:
					reason = "was killed";
					wasKilled = true;
					break;
				case CefTerminationStatus.ProcessCrashed:
					reason = "crashed";
					break;
			}
			this.OwnerWebView.ExecuteWithAsyncErrorHandling(delegate
			{
				throw new RenderProcessTerminatedException("WebView render process " + reason, wasKilled);
			});
		}

		bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
		{
			return false;
		}

		void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
		{
		}

		bool IRequestHandler.CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
		{
			return true;
		}

		bool IRequestHandler.CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
		{
			return true;
		}
		 

		private readonly WebView OwnerWebView;
	}

}
