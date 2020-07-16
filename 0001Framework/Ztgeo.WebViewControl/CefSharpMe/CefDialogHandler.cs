using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class CefDialogHandler : IDialogHandler
	{
		public CefDialogHandler(WebView webView)
		{
			this.OwnerWebView = webView;
		}

		bool IDialogHandler.OnFileDialog(IWebBrowser browserControl, IBrowser browser, CefFileDialogMode mode, CefFileDialogFlags flags, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
		{
			return this.OwnerWebView.DisableFileDialogs;
		}

		private readonly WebView OwnerWebView;
	}

}
