using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public interface IJavascriptObject
	{ 
		Dictionary<string, object> ToJavascriptObject();
	}
}
