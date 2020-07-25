using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public interface ITrackableJSObject : IJSObject
    {
		bool IsTracked { get; set; }
		 
		string TrackingCode { get; }
		 
		string ModulePath { get; }
		 
		string ConstructorFunction { get; }
	}
}
