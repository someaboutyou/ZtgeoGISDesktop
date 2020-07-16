using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	[DataContract]
	internal class JsError
	{
		[DataMember(Name = "stack")]
		public string Stack;

		[DataMember(Name = "name")]
		public string Name;

		[DataMember(Name = "message")]
		public string Message;
	}
}
