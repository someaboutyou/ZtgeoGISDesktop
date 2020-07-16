using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.WebViewControl
{
	public class SimpleViewModule : ViewModuleContainer
	{
 		public SimpleViewModule(string moduleName, ResourceUrl url)
		{
			this.moduleName = moduleName;
			this.source = url.ToString();
		}

  		protected override string JavascriptSource
		{
			get
			{
				return this.source;
			}
		}
		 
		protected override string ModuleName
		{
			get
			{
				return this.moduleName;
			}
		}
		 
		private readonly string source;
		 
		private readonly string moduleName;
	}
}
