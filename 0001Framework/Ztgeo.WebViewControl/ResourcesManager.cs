using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace Ztgeo.WebViewControl
{
	public static class ResourcesManager
	{ 
		private static Stream InternalTryGetResource(string assemblyName, string defaultNamespace, IEnumerable<string> resourcePath, bool failOnMissingResource)
		{
			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == assemblyName);
			if (!(assembly == null))
			{
				return ResourcesManager.InternalTryGetResource(assembly, defaultNamespace, resourcePath, failOnMissingResource);
			}
			if (failOnMissingResource)
			{
				throw new InvalidOperationException("Assembly not found: " + assemblyName);
			}
			return null;
		}
		 
		private static string ComputeEmbeddedResourceName(string defaultNamespace, IEnumerable<string> resourcePath)
		{
			string[] array = new string[]
			{
				defaultNamespace
			}.Concat(resourcePath).ToArray<string>();
			for (int i = 0; i < array.Length - 1; i++)
			{
				array[i] = array[i].Replace('-', '_');
			}
			return string.Join(".", array);
		}
		 
		private static Stream InternalTryGetResource(Assembly assembly, string defaultNamespace, IEnumerable<string> resourcePath, bool failOnMissingResource)
		{
			string text = ResourcesManager.ComputeEmbeddedResourceName(defaultNamespace, resourcePath);
			Stream stream = assembly.GetManifestResourceStream(text);
			if (stream == null)
			{
				string name = assembly.GetName().Name;
				string arg = string.Join("/", resourcePath);
				try
				{
					StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(string.Format("/{0};component/{1}", name, arg), UriKind.Relative));
					stream = ((resourceStream != null) ? resourceStream.Stream : null);
				}
				catch (IOException)
				{
				}
			}
			if (failOnMissingResource && stream == null)
			{
				throw new InvalidOperationException("Resource not found: " + text);
			}
			return stream;
		}
		 
		public static Stream GetResourceWithFullPath(Assembly assembly, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assembly, resourcePath.First<string>(), resourcePath.Skip(1), true);
		}
		 
		public static Stream GetResource(Assembly assembly, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assembly, assembly.GetName().Name, resourcePath, true);
		}
		 
		public static Stream GetResource(string assemblyName, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assemblyName, assemblyName, resourcePath, true);
		}
		 
		public static Stream TryGetResource(Assembly assembly, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assembly, assembly.GetName().Name, resourcePath, false);
		}
		 
		public static Stream TryGetResource(string assemblyName, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assemblyName, assemblyName, resourcePath, false);
		}
		 
		public static Stream TryGetResourceWithFullPath(Assembly assembly, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assembly, resourcePath.First<string>(), resourcePath.Skip(1), false);
		}
		 
		public static Stream TryGetResourceWithFullPath(string assemblyName, IEnumerable<string> resourcePath)
		{
			return ResourcesManager.InternalTryGetResource(assemblyName, resourcePath.First<string>(), resourcePath.Skip(1), false);
		}
		 
		public static string GetMimeType(string resourceName)
		{
			return ResourceHandler.GetMimeType(Path.GetExtension(resourceName));
		}
	}
}
