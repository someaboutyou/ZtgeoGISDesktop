using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	public class Request
	{
		internal Request(IRequest request, string urlOverride)
		{
			this.CefRequest = request;
			this.UrlOverride = urlOverride;
		}

		public string Method
		{
			get
			{
				return this.CefRequest.Method;
			}
		}

		public string Url
		{
			get
			{
				return this.UrlOverride ?? this.CefRequest.Url;
			}
		}

		public virtual void Cancel()
		{
			this.Canceled = true;
		}

		public bool Canceled { get; private set; }

		private readonly IRequest CefRequest;

		private readonly string UrlOverride;
	}
}
