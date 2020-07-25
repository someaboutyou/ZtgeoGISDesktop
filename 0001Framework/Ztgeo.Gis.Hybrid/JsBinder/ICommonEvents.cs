using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
	public interface ICommonEvents
	{ 
		void SelectionChanged();
		 
		void SendTelemetry(int type, string interfaceArea, string targetType, string label, string context, string relatedEvent, bool ctrlDown, bool altDown, bool shiftDown);
		 
		void SendKeyPressTelemetry(string interfaceArea, string targetType, string label, int keyCode, bool ctrlDown, bool altDown, bool shiftDown);
	}
}
