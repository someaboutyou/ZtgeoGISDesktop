using CefSharp.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	internal class LambdaMethodInterceptor : IMethodInterceptor
	{ 
		public LambdaMethodInterceptor(Func<Func<object>, object> interceptCall)
		{
			this.interceptCall = interceptCall;
		}
		 
		object IMethodInterceptor.Intercept(Func<object> originalMethod, string methodName)
		{
			return this.interceptCall(originalMethod);
		}
		 
		private readonly Func<Func<object>, object> interceptCall;
	}
}
