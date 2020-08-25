using Abp.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.HybridUserControl;
using Ztgeo.Gis.Winform.Menu;

namespace ZtgeoGISDesktop.Menus
{
    public class MenuSettingControl:BaseHybridControl<MenuSettingApp2JSAdapterApi, MenuSettingJs2AppAdapterApi>
    { 
        private readonly IWinformMenuViewManager winformMenuViewManager;
        public MenuSettingControl(IocManager iocManager,
            IWinformMenuViewManager _winformMenuViewManager
            ) : base(iocManager) {
            winformMenuViewManager = _winformMenuViewManager; 
            ShowSettingData();
            js2AppAdapterApi.HostControl = this;
        }
        /// <summary>
        /// 设置从而显示SettingData
        /// </summary>
        private void ShowSettingData() {
            IList<MenuOrderSetting> menuOrderSettings= winformMenuViewManager.GetMenuOrderSettings();
            string json = JsonConvert.SerializeObject(menuOrderSettings);
            app2JSAdapterApi.SetMenuSettingData(json);
        }

    }
}
