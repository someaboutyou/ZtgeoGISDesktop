using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
	public interface IJSContextProvider
	{ 
		string EscapeArray(IEnumerable<IJSObject> arr);
		 
		string EscapeArray(IEnumerable<Dictionary<string, object>> arr);
		 
		string EscapeArray(IEnumerable<string> arr);
		 
		string EscapeJSObject(IJSObject o);
		 
		string EscapeJSObject(Dictionary<string, object> o);
		 
		string EscapeJSObjectValue(object o);
		 
		string EscapeString(string str);
		 
		string EscapeBoolean(bool boolean);
		 
		T EvaluateScriptFunction<T>(string functionName, params object[] args);
		 
		void ExecuteScriptFunction(string functionName, params object[] args);
		 
		void BindVariable(string variableName, object objectToBind);
	}
}
