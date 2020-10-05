using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Configuration
{
    public static class ResourceConfigurationExtensions
    {
        public static ResourceConfiguration Resources(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("ResourcesConfiguration",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve< ResourceConfiguration>());
        }
    }
}
