using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl.CefSharpMe
{
	public class JavascriptException : Exception
	{
		internal JavascriptException(string message, JavascriptStackFrame[] stack = null) : base(message, null)
		{
			this.jsStack = (stack ?? new JavascriptStackFrame[0]);
		}

		internal JavascriptException(string name, string message, JavascriptStackFrame[] stack = null) : base((string.IsNullOrEmpty(name) ? "" : (name + ": ")) + message, null)
		{
			this.jsStack = (stack ?? new JavascriptStackFrame[0]);
		}

		public override string StackTrace
		{
			get
			{
				return string.Join(Environment.NewLine, this.jsStack.Select(new Func<JavascriptStackFrame, string>( JavascriptException.FormatStackFrame)).Concat(new string[]
				{
						base.StackTrace
				}));
			}
		}

		private static string FormatStackFrame(JavascriptStackFrame frame)
		{
			string arg = string.IsNullOrEmpty(frame.FunctionName) ? "<anonymous>" : frame.FunctionName;
			string arg2 = string.IsNullOrEmpty(frame.SourceName) ? "" : string.Format(" in {0}:line {1} {2}", frame.SourceName, frame.LineNumber, frame.ColumnNumber);
			return string.Format("   at {0}{1}", arg, arg2);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
					base.GetType().FullName,
					": ",
					this.Message,
					Environment.NewLine,
					this.StackTrace
			});
		}

		private readonly JavascriptStackFrame[] jsStack;
	}

}
