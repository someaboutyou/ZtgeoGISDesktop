using Abp.Modules;
using Ztgeo.Gis.Winform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Communication;
using Ztgeo.Gis.Winform.Configuration;
using ZtgeoGISDesktop.Menus;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Hybrid;
using ZtgeoGISDesktop.Core;
using ZtgeoGISDesktop.EntityFramework;
using ZtgeoGISDesktop.Toolbars;
using CadastralManagementDataSync;
using Abp.Dependency;
using Castle.Core.Logging;
using Ztgeo.Gis.CAD;

namespace ZtgeoGISDesktop
{
    [DependsOn(typeof(ZtgeoGisWinformMoudle)
        ,typeof(ZtgeoGISDesktopCommunicationMoudle)
        ,typeof(ZtgeoGisRuntimeModule)
        ,typeof(ZtgeoGisHybridMoudle)
        ,typeof(ZtgeoGISDesktopCoreMoudle)
        ,typeof(ZtgeoGISDesktopEntityFrameworkMoudle)
        ,typeof(CadastralManagementDataSyncMoudle)
        ,typeof(ZtgeoGisCADModule))]
    public class ZtgeoGISDesktopMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.WinformMenus().Providers.Add(typeof(MainMenuProvider));
            Configuration.Modules.WinformToolbars().Providers.Add(typeof(MainToolbarProvider)); 
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<IExceptionDeal, DefaultExceptionDeal>();
        }

        public override void PostInitialize()
        {

        }
    }
}
