using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ztgeo.WebViewControl
{
	internal static class WindowsEventsListener
	{ 
		static WindowsEventsListener()
		{
			EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.UnloadedEvent, new RoutedEventHandler(WindowsEventsListener.OnWindowUnloaded), true);
		}
		 
		public static event Action<Window> WindowUnloaded;
		 
		private static void OnWindowUnloaded(object sender, EventArgs e)
		{
			Action<Window> windowUnloaded = WindowsEventsListener.WindowUnloaded;
			if (windowUnloaded == null)
			{
				return;
			}
			windowUnloaded((Window)sender);
		}
	}
}
