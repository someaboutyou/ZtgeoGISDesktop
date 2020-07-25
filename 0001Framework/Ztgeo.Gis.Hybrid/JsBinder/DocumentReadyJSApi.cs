using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Utils;
using Ztgeo.WebViewControl;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
	public class DocumentReadyJSApi : IBindableJSContextProvider 
	{
		private const int TRACKS_BEFORE_GC = 50; 
		protected bool documentIsReady; 
		protected Dictionary<string, WeakReference> trackedObjects = new Dictionary<string, WeakReference>(); 
		private WebView webView; 
		private Action SetDocumentReady; 
		private Queue<Action> CallQueue = new Queue<Action>(); 
		private int trackCounts;

		public DocumentReadyJSApi(WebView webView)
		{
			this.webView = webView;
			this.SetDocumentReady = () => {
				this.ProcessCallQueue();
			};
			webView.RegisterJavascriptObjectWithErrorHandling("__setDocumentReady", this.SetDocumentReady, false);
		}

		protected virtual void ProcessCallQueue()
		{
			Queue<Action> callQueue = this.CallQueue;
			lock (callQueue)
			{
				while (this.CallQueue.Count > 0)
				{
					this.CallQueue.Dequeue()();
				}
				this.documentIsReady = true;
			}
		}
		 
		bool IBindableJSContextProvider.IsReady
		{
			get
			{
				return this.documentIsReady;
			}
		}
		
 		private string CallJSConstructor(ITrackableJSObject trackable)
		{
			return string.Concat(new string[]
			{
				"(CommonAPI.CreateViewObject(require('",
				trackable.ModulePath,
				"').",
				trackable.ConstructorFunction,
				", ",
				this.EscapeJSObject(trackable.ToJavascriptObject()),
				"))"
			});
		}

 		public void ExecuteWhenReady(Action action)
		{
			bool flag = false;
			if (!this.documentIsReady)
			{
				Queue<Action> callQueue = this.CallQueue;
				lock (callQueue)
				{
					if (!this.documentIsReady || !this.CallQueue.IsEmpty<Action>())
					{
						flag = true;
						this.CallQueue.Enqueue(action);
					}
				}
			}
			if (!flag)
			{
				action();
			}
		}

		public ITrackableJSObject GetTrackedObject(string trackCode)
		{
			if (trackCode == null)
			{
				return null;
			}
			return (ITrackableJSObject)this.trackedObjects[trackCode].Target;
		}

		public void TrackObject(ITrackableJSObject obj)
		{
			this.ExecuteScriptFunction("CommonAPI.TrackObject", new object[]
			{
				this.EscapeJSObject(obj)
			});
			this.trackedObjects[obj.TrackingCode] = new WeakReference(obj);
			obj.IsTracked = true;
			int num2;
			lock (this)
			{
				int num = this.trackCounts;
				this.trackCounts = num + 1;
				num2 = num;
			}
			if (num2 > TRACKS_BEFORE_GC)
			{
				this.GC_TrackedObjects();
			}
		} 
		private void GC_TrackedObjects()
		{
			lock (this)
			{
				foreach (KeyValuePair<string, WeakReference> keyValuePair in this.trackedObjects.AsEnumerable<KeyValuePair<string, WeakReference>>().ToList<KeyValuePair<string, WeakReference>>())
				{
					if (!keyValuePair.Value.IsAlive)
					{
						this.trackedObjects.Remove(keyValuePair.Key);
						this.ExecuteScriptFunction("CommonAPI.UntrackObject", new object[]
						{
							this.EscapeString(keyValuePair.Key)
						});
					}
				}
				this.trackCounts = 0;
			}
		}

		public void NotifyPropertyChanged(ITrackableJSObject jsObj, string propName, object propValue)
		{
			if (!jsObj.IsTracked)
			{
				this.TrackObject(jsObj);
			}
			this.ExecuteScriptFunction("CommonAPI.NotifyPropertyChanged", new object[]
			{
				this.EscapeString(jsObj.TrackingCode),
				this.EscapeString(propName),
				this.EscapeJSObjectValue(propValue)
			});
		}

		public void NotifyElementAdded(ITrackableJSObject jsObj, string collectionPropName, int position, object newElement)
		{
			jsObj.IsTracked.MustBeTrue();
			this.ExecuteScriptFunction("CommonAPI.NotifyElementAdded", new object[]
			{
				this.EscapeString(jsObj.TrackingCode),
				this.EscapeString(collectionPropName),
				this.EscapeJSObjectValue(position),
				this.EscapeJSObjectValue(newElement)
			});
		}

		public void NotifyElementRemoved(ITrackableJSObject jsObj, string collectionPropName, int position)
		{
			jsObj.IsTracked.MustBeTrue();
			this.ExecuteScriptFunction("CommonAPI.NotifyElementRemoved", new object[]
			{
				this.EscapeString(jsObj.TrackingCode),
				this.EscapeString(collectionPropName),
				this.EscapeJSObjectValue(position)
			});
		}

		public void NotifyElementMoved(ITrackableJSObject jsObj, string collectionPropName, int oldPosition, int newPosition)
		{
			jsObj.IsTracked.MustBeTrue();
			this.ExecuteScriptFunction("CommonAPI.NotifyElementMoved", new object[]
			{
				this.EscapeString(jsObj.TrackingCode),
				this.EscapeString(collectionPropName),
				this.EscapeJSObjectValue(oldPosition),
				this.EscapeJSObjectValue(newPosition)
			});
		}

		public void NotifyElementReplaced(ITrackableJSObject jsObj, string collectionPropName, int position, object newElement)
		{
			jsObj.IsTracked.MustBeTrue();
			this.ExecuteScriptFunction("CommonAPI.NotifyElementReplaced", new object[]
			{
				this.EscapeString(jsObj.TrackingCode),
				this.EscapeString(collectionPropName),
				this.EscapeJSObjectValue(position),
				this.EscapeJSObjectValue(newElement)
			});
		}

 		public virtual void ResolvePromise(int idx, object result)
		{
			this.ExecuteScriptFunction("CommonAPI.ResolvePromise", new object[]
			{
				idx,
				this.EscapeJSObjectValue(result)
			});
		}

 		public virtual void RejectPromise(int idx, string error)
		{
			this.ExecuteScriptFunction("CommonAPI.RejectPromise", new object[]
			{
				idx,
				this.EscapeString(error)
			});
		}

 		public string EscapeArray(IEnumerable<IJSObject> arr)
		{
			return "[" + (from o in arr
						  select this.EscapeJSObject(o)).StrCat(",") + "]";
		}

 		public string EscapeJSObject(IJSObject o)
		{
			ITrackableJSObject trackableJSObject = o as ITrackableJSObject;
			if (trackableJSObject != null)
			{
				if (trackableJSObject.IsTracked)
				{
					return "CommonAPI.GetTrackedObject(" + JavascriptSerializer.Serialize(trackableJSObject.TrackingCode) + ")";
				}
				if (trackableJSObject.ConstructorFunction != null)
				{
					return this.CallJSConstructor(trackableJSObject);
				}
			}
			return JavascriptSerializer.Serialize(o.ToJavascriptObject());
		}

 		public string EscapeArray(IEnumerable<Dictionary<string, object>> arr)
		{
			return JavascriptSerializer.Serialize(arr);
		}

 		public string EscapeArray(IEnumerable<string> arr)
		{
			return JavascriptSerializer.Serialize(arr);
		}

 		public string EscapeJSObject(Dictionary<string, object> o)
		{
			return "{" + (from kvp in o
						  select this.EscapeString(kvp.Key) + ":" + this.EscapeJSObjectValue(kvp.Value)).StrCat(",") + "}";
		}

 		public string EscapeJSObjectValue(object o)
		{
			IJSObject ijsobject = o as IJSObject;
			if (ijsobject != null)
			{
				return this.EscapeJSObject(ijsobject);
			}
			IEnumerable<IJSObject> enumerable = o as IEnumerable<IJSObject>;
			if (enumerable != null)
			{
				return this.EscapeArray(enumerable);
			}
			return JavascriptSerializer.SerializeJavascriptObject(o);
		}

 		public string EscapeString(string str)
		{
			return JavascriptSerializer.Serialize(str);
		}

 		public string EscapeBoolean(bool boolean)
		{
			return JavascriptSerializer.Serialize(boolean);
		}

 		public T EvaluateScriptFunction<T>(string functionName, params object[] args)
		{
			T result = default(T);
			this.ExecuteWhenReady(delegate
			{
				result = this.webView.EvaluateScriptFunction<T>(functionName, args.Select(delegate (object a)
				{
					if (a == null)
					{
						return "null";
					}
					return a.ToString();
				}).ToArray<string>());
			});
			return result;
		}

 		public void ExecuteScriptFunction(string functionName, params object[] args)
		{
			this.ExecuteWhenReady(delegate
			{
				this.webView.ExecuteScriptFunction(functionName, args.Select(delegate (object a)
				{
					if (a == null)
					{
						return "null";
					}
					return a.ToString();
				}).ToArray<string>());
			});
		}

 		public void BindVariable(string variableName, object objectToBind)
		{
			this.webView.RegisterJavascriptObjectWithErrorHandling(variableName, objectToBind, true);
		}


	}
}
