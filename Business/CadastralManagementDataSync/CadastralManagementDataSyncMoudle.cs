using Abp.Modules;
using CadastralManagementDataSync.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform;
using Ztgeo.Gis.Winform.Configuration;

namespace CadastralManagementDataSync
{
    [DependsOn(typeof(ZtgeoGisWinformMoudle),
        typeof(ZtgeoGisExtensionMoudle))]
    public class CadastralManagementDataSyncMoudle : AbpModule
    {
        public override void PreInitialize()
        { 
            Configuration.Modules.WinformMenus().Providers.Add(typeof(DataSyncMenuProvider));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()); 
        }

        public override void PostInitialize()
        {

        }
    }
}
