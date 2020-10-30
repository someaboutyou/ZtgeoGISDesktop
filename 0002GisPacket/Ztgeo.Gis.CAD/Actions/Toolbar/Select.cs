using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.CAD.Actions.Toolbar
{
    public class Select :IToolbarAction
    {
        private readonly IWinformToolbarViewManager WinformToolbarViewManager;
        private readonly ICADToolbarManager CADToolbarControl;
        public Select(IWinformToolbarViewManager winformToolbarViewManager,
                ICADToolbarManager cadToolbarControl
            ) {
            WinformToolbarViewManager = winformToolbarViewManager;
            CADToolbarControl = cadToolbarControl;
        }

        public WinformToolbar WinformToolbar { private get; set; }

        public void Excute() {
            if (WinformToolbar.IsActive)
            {  //设置其他的为非激活状态 to do
                WinformToolbarViewManager.SetToolbarStatus(WinformToolbar, Winform.Menu.MenuStatus.Available);
            }
            else
            {
                WinformToolbarViewManager.SetToolbarStatus(WinformToolbar, Winform.Menu.MenuStatus.Active);
                //与selectmodeltoolbar 排他
                CADToolbarControl.SetSelectorActive();
            }
        }
    }
}
