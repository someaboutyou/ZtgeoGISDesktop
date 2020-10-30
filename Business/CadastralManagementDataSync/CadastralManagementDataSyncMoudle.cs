using Abp.Dependency;
using Abp.Modules;
using CadastralManagementDataSync.Menus;
using CadastralManagementDataSync.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform;
using Ztgeo.Gis.Winform.Configuration;
using Ztgeo.Gis.Winform.Resources;

namespace CadastralManagementDataSync
{
    [DependsOn(typeof(ZtgeoGisWinformMoudle),
        typeof(ZtgeoGisExtensionMoudle))]
    public class CadastralManagementDataSyncMoudle : AbpModule
    {
        public override void PreInitialize()
        { 
            Configuration.Modules.WinformMenus().Providers.Add(typeof(DataSyncMenuProvider));
            IocManager.Register<SyncDataResource>(DependencyLifeStyle.Transient);
            IocManager.Register<SyncDataResourceMetaData>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Resolve<IResourceMetaDataProvider>().DocumentResourceMetaDataProviders.Add(typeof(SyncDataResourceMetaData)); //添加CAD文件元数据 
        }

        public override void PostInitialize()
        {

        }
    }
}
