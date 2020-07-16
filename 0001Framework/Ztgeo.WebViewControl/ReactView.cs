using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ztgeo.WebViewControl
{
	public class ReactView : UserControl, IReactView, IDisposable, IViewModule
	{ 
		private static ReactViewRender CreateReactViewInstance()
		{
			ReactViewRender reactViewRender = ReactView.cachedView;
			ReactView.cachedView = null;
			new Thread(()=> {
				if (ReactView.cachedView == null)
				{
					ReactView.cachedView = new ReactViewRender(true);
				}
			}).Start();  
			return reactViewRender ?? new ReactViewRender(true);
		}
		 
		public ReactView(bool usePreloadedWebView = true)
		{
			if (usePreloadedWebView)
			{
				this.view = ReactView.CreateReactViewInstance();
			}
			else
			{
				this.view = new ReactViewRender(false);
			}
			this.Controls.Add(this.view);
			this.BeginInvoke(new Action(()=> {
				if (this.EnableHotReload) {
					this.view.EnableHotReload(this.Source);
				}
				this.view.LoadComponent(this);
			}));
			this.Focus();
			//base.SetResourceReference(FrameworkElement.StyleProperty, typeof(ReactView));
			//base.Content = this.view;
			//base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate
			//{
			//	if (this.EnableHotReload)
			//	{
			//		this.view.EnableHotReload(this.Source);
			//	}
			//	this.view.LoadComponent(this);
			//}));
			//FocusManager.SetIsFocusScope(this, true);
			//FocusManager.SetFocusedElement(this, this.view.FocusableElement);
		}
		 
		~ReactView()
		{
			this.Dispose();
		}
		 
		public new void Dispose()
		{
			this.view.Dispose();
			GC.SuppressFinalize(this);
		}
		 
		public ResourceUrl DefaultStyleSheet
		{
			get
			{
				return this.view.DefaultStyleSheet;
			}
			set
			{
				this.view.DefaultStyleSheet = value;
			}
		}
		 
		public IViewModule[] Plugins
		{
			get
			{
				return this.view.Plugins;
			}
			set
			{
				this.view.Plugins = value;
			}
		} 
		public T WithPlugin<T>()
		{
			return this.view.WithPlugin<T>();
		} 
		public bool EnableDebugMode
		{
			get
			{
				return this.view.EnableDebugMode;
			}
			set
			{
				this.view.EnableDebugMode = value;
			}
		}
 
		public bool EnableHotReload { get; set; }
		 
		public bool IsReady
		{
			get
			{
				return this.view.IsReady;
			}
		}
		 
		public event Action Ready
		{
			add
			{
				this.view.Ready += value;
			}
			remove
			{
				this.view.Ready -= value;
			}
		}
		 
		public event Action<CefException.UnhandledExceptionEventArgs> UnhandledAsyncException
		{
			add
			{
				this.view.UnhandledAsyncException += value;
			}
			remove
			{
				this.view.UnhandledAsyncException -= value;
			}
		}
		 
		public event Func<string, Stream> CustomResourceRequested
		{
			add
			{
				this.view.CustomResourceRequested += value;
			}
			remove
			{
				this.view.CustomResourceRequested -= value;
			}
		}
		 
		public void ShowDeveloperTools()
		{
			this.view.ShowDeveloperTools();
		}
		 
		public void CloseDeveloperTools()
		{
			this.view.CloseDeveloperTools();
		}
		 
		string IViewModule.JavascriptSource
		{
			get
			{
				return this.JavascriptSource;
			}
		}
		 
		protected virtual string JavascriptSource
		{
			get
			{
				return null;
			}
		}
		 
		string IViewModule.NativeObjectName
		{
			get
			{
				return this.NativeObjectName;
			}
		}
		 
		protected virtual string NativeObjectName
		{
			get
			{
				return null;
			}
		}
		 
		protected virtual string ModuleName
		{
			get
			{
				return null;
			}
		}
		 
		string IViewModule.Name
		{
			get
			{
				return this.ModuleName;
			}
		}
		 
		string IViewModule.Source
		{
			get
			{
				return this.Source;
			}
		}
		 
		protected virtual string Source
		{
			get
			{
				return null;
			}
		}
		 
		object IViewModule.CreateNativeObject()
		{
			return this.CreateNativeObject();
		}
		 
		protected virtual object CreateNativeObject()
		{
			return null;
		}
		 
		void IViewModule.Bind(IExecutionEngine engine)
		{
			throw new Exception("Cannot bind ReactView");
		}
		 
		IExecutionEngine IViewModule.ExecutionEngine
		{
			get
			{
				return this.ExecutionEngine;
			}
		}
		 
		protected IExecutionEngine ExecutionEngine
		{
			get
			{
				return this.view;
			}
		}
		 
		private static ReactViewRender cachedView;
		 
		private readonly ReactViewRender view;
	}
}
