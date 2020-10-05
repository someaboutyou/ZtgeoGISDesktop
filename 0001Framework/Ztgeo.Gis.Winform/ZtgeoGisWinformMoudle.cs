using Abp;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.Configuration;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.Winform
{
    [DependsOn(typeof(AbpKernelModule))]
    public class ZtgeoGisWinformMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<WinformMenuConfiguration>();
            IocManager.Register<WinformToolbarConfiguration>();
            IocManager.Register<ResourceConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<IMainForm>().StartInitializeComponent();
            IocManager.Resolve<WinformMenuManager>().Initialize(); //菜单初始化，会将provider中的所有菜单加载进来  thinking :::是否可以在runtime 的hook 里面统一初始化。
            IocManager.Resolve<WinformToolbarManager>().Initialize();// toolbar初始化，会将provider 中的所有toolbar 加载进来
            IocManager.Resolve<IWinformMenuViewManager>().InitializeMenus();
            IocManager.Resolve<IWinformToolbarViewManager>().InitialzeToolbars();
            IocManager.Resolve<ResourceConfiguration>().Resources=IocManager.Resolve<ResourceConfiguration>().Resources.OrderBy(r => r.IdentifiedOrder).ToList();
        }
    }
}
