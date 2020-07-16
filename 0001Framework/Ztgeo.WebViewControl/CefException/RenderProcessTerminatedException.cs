using System; 

namespace Ztgeo.WebViewControl.CefException
{
	public class RenderProcessTerminatedException : Exception
	{ 
		internal RenderProcessTerminatedException(string message, bool wasKilled) : base(message)
		{
			this.WasKilled = wasKilled;
		} 
		public bool WasKilled { get; set; }
	}
}
