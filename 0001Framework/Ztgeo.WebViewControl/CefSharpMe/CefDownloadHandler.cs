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
				Action<string> downloadCompleted = this.OwnerWebView.DownloadCompleted;
				if (downloadCompleted != null)
				{
					this.OwnerWebView.AsyncExecuteInUI(delegate
					{
						downloadCompleted(downloadItem.FullPath);
					});
					return;
				}
			}
			else if (downloadItem.IsCancelled)
			{
				Action<string> downloadCancelled = this.OwnerWebView.DownloadCancelled;
				if (downloadCancelled != null)
				{
					this.OwnerWebView.AsyncExecuteInUI(delegate
					{
						downloadCancelled(downloadItem.FullPath);
					});
					return;
				}
			}
			else
			{
				Action<string, long, long> downloadProgressChanged = this.OwnerWebView.DownloadProgressChanged;
				if (downloadProgressChanged != null)
				{
					this.OwnerWebView.AsyncExecuteInUI(delegate
					{
						downloadProgressChanged(downloadItem.FullPath, downloadItem.ReceivedBytes, downloadItem.TotalBytes);
					});
				}
			}
		}

		private readonly WebView OwnerWebView;
	}
}
