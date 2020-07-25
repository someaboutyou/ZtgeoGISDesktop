using Abp;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Configuration;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Winform
{
    [DependsOn(typeof(AbpKernelModule))]
    public class ZtgeoGisWinformMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<WinformMenuConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {  
            IocManager.Resolve<WinformMenuManager>().Initialize(); //是否可以在runtime 的hook 里面统一初始化。
            IocManager.Resolve<IWinformMenuViewManager>().InitializeMenus();
        }
    }
}
