using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid.JsBinder;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Winform.Menu;
using ZtgeoGISDesktop.Core.Menu;

namespace ZtgeoGISDesktop.Menus
{
    public class MenuSettingJs2AppAdapterApi: Js2AppAdapterApiBase
    {
        private readonly IRepository<MenuOrder> menuOrderRepository;
        private readonly IWinformMenuViewManager winformMenuViewManager;
        public MenuSettingJs2AppAdapterApi(IRepository<MenuOrder> _menuOrderRepository,
            IWinformMenuViewManager _winformMenuViewManager
            ) {
            menuOrderRepository = _menuOrderRepository;
            winformMenuViewManager = _winformMenuViewManager;
        }
        public Control HostControl { get; set; }
        public override string AppBindObjectName { get; protected set; } = "MenuSettingJs2AppAdapterApi";
        /// <summary>
        /// 代理js 传来的方法，
        /// </summary>
        /// <param name="message"></param>
        public void onSaveMenuSetting(string menuSettingStr)
        {
            try
            {
                List<MenuOrderSetting> menuOrderSettings = JsonConvert.DeserializeObject<List<MenuOrderSetting>>(menuSettingStr);
                menuOrderRepository.Delete(mo => true);
                menuOrderSettings.ForEach(mos =>
                {
                    menuOrderRepository.Insert(new MenuOrder { MenuKey = mos.MenuKey, Order = mos.Order });
                });
                winformMenuViewManager.RefeshMenu();
                this.onClose();
            }catch(Exception e)
            { 
                EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(e, false) }); 
            }
        }

        public void onClose() {
            if (this.HostControl != null) {
                Form form= this.HostControl.FindForm();
                if (form != null) {
                    form.Close();
                } 
            }
        }
    }
}
