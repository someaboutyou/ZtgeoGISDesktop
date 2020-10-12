using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Configuration;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.CAD.Menus;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.Configuration;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;

namespace Ztgeo.Gis.CAD
{
    /// <summary>
    /// power by JZW
    /// 该模块中定义了CAD的显示控件，和按钮事件
    /// </summary>
    public class ZtgeoGisCADModule:AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.WinformMenus().Providers.Add(typeof(CadBaseMenuProvider));
            Configuration.Modules.WinformToolbars().Providers.Add(typeof(CADToolbarProvider));
            IocManager.RegisterIfNot<ICadImportConfiguration, CadImportConfiguration>();
            IocManager.Register<CADViewSingleFileDocumentResource>(DependencyLifeStyle.Transient);
            IocManager.Register<CADViewSingleFileDocumentResourceMetaData>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Resolve<IDocumentResourceProvider>().MetaDataProviders.Add(typeof(CADViewSingleFileDocumentResourceMetaData)); //添加CAD文件元数据

        }
        public override void PostInitialize()
        { 

        }
    }
}
