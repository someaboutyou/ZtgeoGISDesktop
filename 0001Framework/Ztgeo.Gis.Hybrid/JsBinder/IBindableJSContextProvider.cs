using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public interface IBindableJSContextProvider : IJSContextProvider
    {
		void TrackObject(ITrackableJSObject jsObj); 
		ITrackableJSObject GetTrackedObject(string trackCode); 
		void NotifyPropertyChanged(ITrackableJSObject jsObj, string propName, object newValue); 
		void NotifyElementAdded(ITrackableJSObject jsObj, string collectionPropName, int position, object newElement); 
		void NotifyElementMoved(ITrackableJSObject jsObj, string collectionPropName, int oldPosition, int newPosition); 
		void NotifyElementRemoved(ITrackableJSObject jsObj, string collectionPropName, int position); 
		void NotifyElementReplaced(ITrackableJSObject jsObj, string collectionPropName, int position, object newElement); 
		bool IsReady { get; } 
		void ExecuteWhenReady(Action action);
	}
}
