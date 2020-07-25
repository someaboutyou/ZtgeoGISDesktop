using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Share.Events;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Runtime.Hook
{
    public class GisApplicationHook: IGisApplicationHook
    {
        private static object _lockObject = new object();
        private readonly IWinformMenuViewManager winformMenuViewManager;
        public GisApplicationHook(IWinformMenuViewManager _winformMenuViewManager) {
            winformMenuViewManager = _winformMenuViewManager;
        }
        public void SetMenuStatus(WinformMenu menu, MenuStatus menuStatus) {
            winformMenuViewManager.SetMenuStatus(menu, menuStatus);
            EventBus.Default.Trigger(new WinFormMenuStatusChangeEventData {
                WinFormMenu=menu,
                MenuStatus= menuStatus
            });
        }

    }
}
