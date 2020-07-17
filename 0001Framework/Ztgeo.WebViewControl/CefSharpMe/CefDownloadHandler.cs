using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
    internal class CefDownloadHandler : IDownloadHandler
	{
		public CefDownloadHandler(WebView owner)
		{
			this.OwnerWebView = owner;
		}

		void IDownloadHandler.OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
		{
			if (!callback.IsDisposed)
			{
				try
				{
					callback.Continue(downloadItem.SuggestedFileName, true);
				}
				finally
				{
					if (callback != null)
					{
						callback.Dispose();
					}
				}
			}
		}

		void IDownloadHandler.OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
		{
			if (downloadItem.IsComplete)
			{
				this.OwnerWebView.DoDownloadCompleted(downloadItem.FullPath); 
			}
			else if (downloadItem.IsCancelled)
			{
				this.OwnerWebView.DoDownloadCancelled(downloadItem.FullPath);
			}
			else
			{
				this.OwnerWebView.DoDownloadProgressChanged(downloadItem.FullPath, downloadItem.ReceivedBytes, downloadItem.TotalBytes); 
			}
		}

		private readonly WebView OwnerWebView;
	}
}
