using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	public sealed class ResourceHandler :  Request
	{
		internal ResourceHandler(IRequest request, string urlOverride) : base(request, urlOverride)
		{
		}

		public void RespondWith(string filename)
		{
			CefSharp.ResourceHandler resourceHandler = (CefSharp.ResourceHandler)CefSharp.ResourceHandler.FromFilePath(filename, CefSharp.ResourceHandler.GetMimeType(Path.GetExtension(filename)), false);
			resourceHandler.AutoDisposeStream = true;
			this.Handler = resourceHandler;
		}

		public void RespondWith(Stream stream, string extension)
		{
			this.Handler = CefSharp.ResourceHandler.FromStream(stream, CefSharp.ResourceHandler.GetMimeType(extension), false);
		}

		public void Redirect(string url)
		{
			this.Handler = new CefResourceHandler(url);
		}

		internal IResourceHandler Handler { get; private set; }

		public bool Handled
		{
			get
			{
				return this.Handler != null;
			}
		}

		public Stream Response
		{
			get
			{
				CefSharp.ResourceHandler resourceHandler = this.Handler as CefSharp.ResourceHandler;
				if (resourceHandler == null)
				{
					return null;
				}
				return resourceHandler.Stream;
			}
		}
	}
}
