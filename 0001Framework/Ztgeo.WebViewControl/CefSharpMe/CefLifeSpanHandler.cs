using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
    internal class CefLifeSpanHandler : ILifeSpanHandler
	{
		public CefLifeSpanHandler(WebView webView)
		{
			this.OwnerWebView = webView;
		}

		public event Action<string> PopupOpening;

		void ILifeSpanHandler.OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
		{
		}

		bool ILifeSpanHandler.OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
		{
			newBrowser = null;
			if (targetUrl.StartsWith("chrome-devtools:", StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}
			if (Uri.IsWellFormedUriString(targetUrl, UriKind.RelativeOrAbsolute))
			{
				Uri uri = new Uri(targetUrl);
				if (!uri.IsAbsoluteUri)
				{
					targetUrl = new Uri(new Uri(frame.Url), uri).AbsoluteUri;
				}
				try
				{
					if (this.PopupOpening != null)
					{
						this.PopupOpening(targetUrl);
					}
					else
					{
						Process.Start(targetUrl);
					}
				}
				catch
				{
					try
					{
						Process.Start("explorer.exe", "\"" + targetUrl + "\"");
					}
					catch
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
		{
		}

		bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
		{
			return false;
		}

		private readonly WebView OwnerWebView;
	}
}
