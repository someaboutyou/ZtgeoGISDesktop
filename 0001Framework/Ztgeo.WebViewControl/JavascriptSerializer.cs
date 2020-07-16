using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public static class JavascriptSerializer
	{ 
		public static string Serialize(IEnumerable<string> arr)
		{
			return "[" + string.Join(",", from si in arr
										  select JavascriptSerializer.Serialize(si)) + "]";
		}
		 
		public static string Serialize(IEnumerable<IJavascriptObject> arr)
		{
			return JavascriptSerializer.Serialize(from o in arr
												  select o.ToJavascriptObject());
		}
		 
		public static string Serialize(IEnumerable<Dictionary<string, object>> arr)
		{
			return "[" + string.Join(",", from o in arr
										  select JavascriptSerializer.Serialize(o)) + "]";
		}
		 
		public static string SerializeJavascriptObject(object o)
		{
			if (o == null)
			{
				return "null";
			}
			if (o is ValueType)
			{
				return o.ToString().ToLowerInvariant();
			}
			if (o is string)
			{
				return JavascriptSerializer.Serialize((string)o);
			}
			if (o is Dictionary<string, object>)
			{
				return JavascriptSerializer.Serialize((Dictionary<string, object>)o);
			}
			if (o is IJavascriptObject)
			{
				return JavascriptSerializer.Serialize((IJavascriptObject)o);
			}
			if (o is IJavascriptEnumValue)
			{
				return ((IJavascriptEnumValue)o).ToJavascriptEnumValue();
			}
			if (o is IEnumerable<string>)
			{
				return JavascriptSerializer.Serialize((IEnumerable<string>)o);
			}
			if (o is IEnumerable<Dictionary<string, object>>)
			{
				return JavascriptSerializer.Serialize((IEnumerable<Dictionary<string, object>>)o);
			}
			if (o is IEnumerable<IJavascriptObject>)
			{
				return JavascriptSerializer.Serialize((IEnumerable<IJavascriptObject>)o);
			}
			throw new ArgumentException("unexpected argument type: " + o.GetType().FullName);
		}
		 
		public static string Serialize(IJavascriptObject o)
		{
			return JavascriptSerializer.Serialize(o.ToJavascriptObject());
		}
		 
		public static string Serialize(Dictionary<string, object> o)
		{
			return "{" + string.Join(",", from kvp in o
										  select JavascriptSerializer.Serialize(kvp.Key) + ":" + JavascriptSerializer.SerializeJavascriptObject(kvp.Value)) + "}";
		}
		 
		public static string Serialize(string str)
		{
			if (str != null)
			{
				return "\"" + Regex.Escape(str).Replace("\"", "\\\"") + "\"";
			}
			return "null";
		}
		 
		public static string Serialize(bool boolean)
		{
			return boolean.ToString().ToLowerInvariant();
		}
	}
}
