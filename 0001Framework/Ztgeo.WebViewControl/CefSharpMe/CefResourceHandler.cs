using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class CefResourceHandler : CefSharp.ResourceHandler
	{
		public CefResourceHandler(string redirectUrl) : base("text/html", null, false)
		{
			this.redirectUrl = redirectUrl;
		} 

		private readonly string redirectUrl;
	}
}
