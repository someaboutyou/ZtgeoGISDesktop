using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	internal class ScriptTask
	{
		public ScriptTask(string script, TimeSpan? timeout = null, bool awaitable = false)
		{
			this.Script = script;
			if (awaitable)
			{
				this.WaitHandle = new ManualResetEvent(false);
			}
			this.Timeout = timeout;
		}

		public string Script { get; private set; }

		public ManualResetEvent WaitHandle { get; private set; }

		public JavascriptResponse Result { get; set; }

		public Exception Exception { get; set; }

		public TimeSpan? Timeout { get; set; }
	}
}
