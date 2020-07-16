using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public abstract class ViewModuleContainer : IViewModule
	{  
		protected virtual string JavascriptSource
		{
			get
			{
				return null;
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
		 
		protected virtual string Source
		{
			get
			{
				return null;
			}
		}
		 
		protected virtual object CreateNativeObject()
		{
			return null;
		}
		 
		string IViewModule.JavascriptSource
		{
			get
			{
				return this.JavascriptSource;
			}
		}
		 
		string IViewModule.NativeObjectName
		{
			get
			{
				return this.NativeObjectName;
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
		 
		object IViewModule.CreateNativeObject()
		{
			return this.CreateNativeObject();
		}
		 
		void IViewModule.Bind(IExecutionEngine engine)
		{
			this.engine = engine;
		}
		 
		protected IExecutionEngine ExecutionEngine
		{
			get
			{
				return this.engine;
			}
		}
		 
		IExecutionEngine IViewModule.ExecutionEngine
		{
			get
			{
				return this.ExecutionEngine;
			}
		} 
		private IExecutionEngine engine;
	}
}
