using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.CAD.Configuration
{
    public static class CadImportConfigurationExtension
    {
        public static ICadImportConfiguration CadImport(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("CadImportConfiguration",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<ICadImportConfiguration>());
        }
    }
     
}
