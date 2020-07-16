using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	internal class BrowserObjectListener
	{ 
		public event Action<string> NotificationReceived;
		 
		public void Notify(string listenerName)
		{
			Action<string> notificationReceived = this.NotificationReceived;
			if (notificationReceived == null)
			{
				return;
			}
			notificationReceived(listenerName);
		}
	}
}
