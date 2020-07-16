using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{

	public interface IViewModule
	{ 
		string JavascriptSource { get; }
		 
		string NativeObjectName { get; }
		 
		string Name { get; }
		 
		string Source { get; }
		 
		object CreateNativeObject();
		 
		void Bind(IExecutionEngine engine);
		 
		IExecutionEngine ExecutionEngine { get; }
	}
}
