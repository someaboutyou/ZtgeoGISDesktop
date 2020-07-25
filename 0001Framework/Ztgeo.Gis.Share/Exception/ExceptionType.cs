using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Share 
{
	public enum ExceptionType
	{ 
		InsideCommand,
		 
		InsideServerCommand,
		 
		InsideUndo,
		 
		InsideRedo,
		 
		InsidePresenter,
		 
		InsidePresenterCritical,
		 
		InsideScriptInterpreter,
		 
		InsideView,
		 
		Terminal,
		 
		OpenError,
		 
		LazyLoadError
	}
}
