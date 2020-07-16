using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.WebViewControl.CefSharpMe;

namespace Ztgeo.WebViewControl
{
	internal static class ModuleInitializer
	{ 
		internal static void Run()
		{
			CefLoader.RegisterCefSharpAssemblyResolver();
		}
	}
}
