using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefException
{
	public class UnhandledExceptionEventArgs
	{ 
		public UnhandledExceptionEventArgs(Exception e)
		{
			this.Exception = e;
		}
		 
		public Exception Exception { get; private set; }
		 
		public bool Handled { get; set; }
	}
}
