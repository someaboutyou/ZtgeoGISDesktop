using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
	internal class CommonAPI
	{
		public CommonAPI(IJSContextProvider jsCtx)
		{
			this.jsCtx = jsCtx;
		}
		 
		public virtual ICommonEvents commonEvents
		{
			set
			{
				this.jsCtx.BindVariable("commonEvents", value);
			}
		}
		 
		public virtual void TrackObject(IJSObject obj)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.TrackObject", new object[]
			{
				this.jsCtx.EscapeJSObject(obj)
			});
		}
		 
		public virtual void UntrackObject(string trackCode)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.UntrackObject", new object[]
			{
				this.jsCtx.EscapeString(trackCode)
			});
		}
		 
		public virtual void GetTrackedObject(string trackingCode)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.GetTrackedObject", new object[]
			{
				this.jsCtx.EscapeString(trackingCode)
			});
		}
		 
		public virtual void NotifyPropertyChanged(string trackCode, string propName, IJSObject propValue)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.NotifyPropertyChanged", new object[]
			{
				this.jsCtx.EscapeString(trackCode),
				this.jsCtx.EscapeString(propName),
				this.jsCtx.EscapeJSObject(propValue)
			});
		}
		 
		public virtual void NotifyElementAdded(string trackCode, string collectionPropName, int position, IJSObject newElement)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.NotifyElementAdded", new object[]
			{
				this.jsCtx.EscapeString(trackCode),
				this.jsCtx.EscapeString(collectionPropName),
				position,
				this.jsCtx.EscapeJSObject(newElement)
			});
		}
		 
		public virtual void NotifyElementRemoved(string trackCode, string collectionPropName, int position)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.NotifyElementRemoved", new object[]
			{
				this.jsCtx.EscapeString(trackCode),
				this.jsCtx.EscapeString(collectionPropName),
				position
			});
		}
		 
		public virtual void NotifyElementMoved(string trackCode, string collectionPropName, int oldPosition, int newPosition)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.NotifyElementMoved", new object[]
			{
				this.jsCtx.EscapeString(trackCode),
				this.jsCtx.EscapeString(collectionPropName),
				oldPosition,
				newPosition
			});
		}
		 
		public virtual void NotifyElementReplaced(string trackCode, string collectionPropName, int position, IJSObject newElement)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.NotifyElementReplaced", new object[]
			{
				this.jsCtx.EscapeString(trackCode),
				this.jsCtx.EscapeString(collectionPropName),
				position,
				this.jsCtx.EscapeJSObject(newElement)
			});
		}
		 
		public virtual int[] GetScrollPosition()
		{
			return this.jsCtx.EvaluateScriptFunction<int[]>("CommonAPI.GetScrollPosition", Array.Empty<object>());
		}
		 
		public virtual string[] GetSelectedObjects()
		{
			return this.jsCtx.EvaluateScriptFunction<string[]>("CommonAPI.GetSelectedObjects", Array.Empty<object>());
		}
		 
		public virtual void SetSelectedObjects(IJSObject[] objects)
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.SetSelectedObjects", new object[]
			{
				this.jsCtx.EscapeArray(objects)
			});
		}
		 
		public virtual void SelectionChanged()
		{
			this.jsCtx.ExecuteScriptFunction("CommonAPI.SelectionChanged", Array.Empty<object>());
		}
		 
		private IJSContextProvider jsCtx;
	}
}
