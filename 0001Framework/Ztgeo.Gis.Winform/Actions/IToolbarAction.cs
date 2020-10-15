using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.Winform.Actions
{
    public interface IToolbarAction:IWinformAction,Abp.Dependency.ISingletonDependency
    {
        WinformToolbar WinformToolbar { set; }
    }
}
