using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.CAD.Actions.Toolbar
{
    public class Pan : IToolbarAction
    {
        private readonly IDocumentManager DocumentManager;
        private readonly IWinformToolbarViewManager WinformToolbarViewManager;
        private readonly ICADToolbarControl CADToolbarControl;
        public Pan(IDocumentManager documentManager,
            IWinformToolbarViewManager winformToolbarViewManager,
            ICADToolbarControl cadToolbarControl
            )
        {
            DocumentManager = documentManager;
            WinformToolbarViewManager = winformToolbarViewManager;
            CADToolbarControl = cadToolbarControl;
        }
        public WinformToolbar WinformToolbar { private get; set; }

        public void Excute()
        {
            if (WinformToolbar.IsActive)
            { 
                WinformToolbarViewManager.SetToolbarStatus(WinformToolbar, Winform.Menu.MenuStatus.Available);
            }
            else
            {
                WinformToolbarViewManager.SetToolbarStatus(WinformToolbar, Winform.Menu.MenuStatus.Active); 
                CADToolbarControl.SetPanActive();
            }
        }
    }
}
