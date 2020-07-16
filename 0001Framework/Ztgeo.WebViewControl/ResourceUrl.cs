using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public class ResourceUrl
	{ 
		public ResourceUrl(params string[] path)
		{
			this.url = string.Join("/", path);
		}
		 
		public ResourceUrl(Assembly assembly, params string[] path) : this(path)
		{
			string name = assembly.GetName().Name;
			if (this.url.StartsWith("/"))
			{
				this.url = "assembly:" + name + ";" + this.url.Substring(1);
			}
			else
			{
				this.url = name + "/" + this.url;
			}
			this.url = ResourceUrl.BuildUrl("embedded", this.url);
		}
		 
		internal ResourceUrl(string scheme, string path)
		{
			this.url = ResourceUrl.BuildUrl(scheme, path);
		}
		 
		private static string BuildUrl(string scheme, string path)
		{
			return scheme + Uri.SchemeDelimiter + ResourceUrl.CombinePath("webview{0}", path);
		}
		 
		private static string CombinePath(string path1, string path2)
		{
			return path1 + (path1.EndsWith("/") ? "" : "/") + (path2.StartsWith("/") ? path2.Substring(1) : path2);
		}
		 
		public override string ToString()
		{
			return string.Format(this.url, "");
		}
		 
		private static bool ContainsAssemblyLocation(Uri url)
		{
			return url.Scheme == "embedded" && url.AbsolutePath.StartsWith("/assembly:");
		}
		 
		internal static string[] GetEmbeddedResourcePath(Uri resourceUrl)
		{
			if (ResourceUrl.ContainsAssemblyLocation(resourceUrl))
			{
				int num = resourceUrl.AbsolutePath.IndexOf(";");
				return resourceUrl.AbsolutePath.Substring(num + 1).Split(new string[]
				{
					"/"
				}, StringSplitOptions.None);
			}
			return (from p in resourceUrl.Segments.Skip(1)
					select p.Replace("/", "")).ToArray<string>();
		}
		 
		internal static string GetEmbeddedResourceAssemblyName(Uri resourceUrl)
		{
			if (ResourceUrl.ContainsAssemblyLocation(resourceUrl))
			{
				string text = resourceUrl.AbsolutePath.Substring("/assembly:".Length);
				int length = Math.Max(0, text.IndexOf(";"));
				return text.Substring(0, length);
			}
			if (resourceUrl.Segments.Length <= 1)
			{
				return string.Empty;
			}
			string text2 = resourceUrl.Segments[1];
			if (!text2.EndsWith("/"))
			{
				return text2;
			}
			return text2.Substring(0, text2.Length - "/".Length);
		}
		 
		internal string WithDomain(string domain)
		{
			return string.Format(this.url, "." + domain);
		}
		 
		public const string LocalScheme = "local";
		 
		internal const string EmbeddedScheme = "embedded";
		 
		internal const string CustomScheme = "custom";
		 
		internal const string PathSeparator = "/";
		 
		private const string AssemblyPathSeparator = ";";
		 
		private const string AssemblyPrefix = "assembly:";
		 
		private const string DefaultDomain = "webview{0}";
		 
		private readonly string url;
	}
}
