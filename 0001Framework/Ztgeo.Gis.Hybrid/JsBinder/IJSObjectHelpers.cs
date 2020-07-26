using System;
using System.Collections.Specialized;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
	internal static class IJSObjectHelpers
	{ 
		public static string MakeJSID(this IJSObject thisObj)
		{
			Type typeFromHandle = typeof(IJSObjectHelpers);
			lock (typeFromHandle)
			{
				IJSObjectHelpers.lastJsId += 1UL;
			}
			return thisObj.GetType().Name + "-" + IJSObjectHelpers.lastJsId;
		}
		 
		public static NotifyCollectionChangedEventHandler CreateOnCollectionChangedHandler(IBindableJSContextProvider jsCtx, ITrackableJSObject jsObj, string collectionPropName)
		{
			return delegate (object sender, NotifyCollectionChangedEventArgs e)
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						jsCtx.NotifyElementAdded(jsObj, collectionPropName, e.NewStartingIndex, e.NewItems[0]);
						return;
					case NotifyCollectionChangedAction.Remove:
						jsCtx.NotifyElementRemoved(jsObj, collectionPropName, e.OldStartingIndex);
						return;
					case NotifyCollectionChangedAction.Replace:
						jsCtx.NotifyElementReplaced(jsObj, collectionPropName, e.OldStartingIndex, e.NewItems[0]);
						return;
					case NotifyCollectionChangedAction.Move:
						jsCtx.NotifyElementMoved(jsObj, collectionPropName, e.OldStartingIndex, e.NewStartingIndex);
						return;
					case NotifyCollectionChangedAction.Reset:
						jsCtx.NotifyPropertyChanged(jsObj, collectionPropName, new IJSObject[0]);
						return;
					default:
						throw new InvalidOperationException("Unexpected type of collection update");
				}
			};
		}
		 
		private static ulong lastJsId;
	}
}
