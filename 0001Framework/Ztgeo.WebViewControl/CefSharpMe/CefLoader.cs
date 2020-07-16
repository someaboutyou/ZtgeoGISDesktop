using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal static class CefLoader
	{ 
		public static void RegisterCefSharpAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CefLoader.Resolver;
		}
		 
		public static void UnRegisterCefSharpAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= CefLoader.Resolver;
		}
		 
		public static string GetBrowserSubProcessPath()
		{
			string text = Path.Combine(CefLoader.GetBaseArchitectureSpecificPath(), "CefSharp.BrowserSubprocess.exe");
			if (!File.Exists(text))
			{
				throw new FileNotFoundException("Unable to locate", text);
			}
			return text;
		}
		 
		private static Assembly Resolver(object sender, ResolveEventArgs args)
		{
			if (!args.Name.StartsWith("CefSharp"))
			{
				return null;
			}
			string path = args.Name.Split(new char[]
			{
				','
			}, 2)[0] + ".dll";
			string text = Path.Combine(CefLoader.GetBaseArchitectureSpecificPath(), path);
			if (!File.Exists(text))
			{
				throw new FileNotFoundException("Unable to locate", text);
			}
			return Assembly.LoadFile(text);
		}
		 
		private static string GetBasePath()
		{
			return Path.GetDirectoryName(typeof(CefLoader).Assembly.Location);
		}
		 
		private static string GetBaseArchitectureSpecificPath()
		{
			return Path.Combine(CefLoader.GetBasePath(), Environment.Is64BitProcess ? "x64" : "x86");
		}
	}
}
