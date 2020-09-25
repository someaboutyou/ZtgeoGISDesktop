using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Configuration;
using Ztgeo.Gis.CAD.Menus;
using Ztgeo.Gis.Winform.Configuration;

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
            IocManager.RegisterIfNot<ICadImportConfiguration, CadImportConfiguration>();
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
