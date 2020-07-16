using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public interface IExecutionEngine
	{ 
		T EvaluateMethod<T>(IViewModule module, string functionName, params object[] args);
		 
		void ExecuteMethod(IViewModule module, string functionName, params object[] args);
	}
}
