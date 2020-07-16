using CefSharp.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	internal class LambdaMethodBinder : IBinder
	{ 
		public LambdaMethodBinder(Func<object, Type, object> bind)
		{
			this.bind = bind;
		}
		 
		object IBinder.Bind(object obj, Type modelType)
		{
			return this.bind(obj, modelType);
		}
		 
		private readonly Func<object, Type, object> bind;
	}
}
