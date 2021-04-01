using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.OpenTK
{
    public class ZtgeoGisOpenTKModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Modules.WinformMenus().Providers.Add(typeof(CadBaseMenuProvider));
            //Configuration.Modules.WinformToolbars().Providers.Add(typeof(CADToolbarProvider));
            //IocManager.RegisterIfNot<ICadImportConfiguration, CadImportConfiguration>();
            //IocManager.Register<CADViewSingleFileDocumentResource>(DependencyLifeStyle.Transient);
            //IocManager.Register<CADViewSingleFileDocumentResourceMetaData>();
        }

        public override void Initialize()
        {
            //IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //IocManager.Resolve<IResourceMetaDataProvider>().DocumentResourceMetaDataProviders.Add(typeof(CADViewSingleFileDocumentResourceMetaData)); //添加CAD文件元数据

        }
        public override void PostInitialize()
        {

        }
    }
}
