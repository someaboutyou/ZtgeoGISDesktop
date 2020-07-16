using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public class Listener
	{ 
		internal Listener(string eventName, Action<string> handler)
		{
			this.eventName = eventName;
			this.Handler = handler;
		}
		 
		internal Action<string> Handler { get; private set; }
		 
		public override string ToString()
		{
			return string.Format("({0}.notify('{1}'));", "__WebviewListener__", this.eventName);
		}
		 
		internal const string EventListenerObjName = "__WebviewListener__";
		 
		private readonly string eventName;
	}
}
