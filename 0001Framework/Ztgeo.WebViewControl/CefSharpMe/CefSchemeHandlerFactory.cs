using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class CefSchemeHandlerFactory : ISchemeHandlerFactory
	{
		public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
		{
			return null;
		}
	}
}



