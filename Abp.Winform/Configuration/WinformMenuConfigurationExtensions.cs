using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Configuration
{
    public static class WinformMenuConfigurationExtensions
    {
        public static WinformMenuConfiguration WinformMenus(this IModuleConfigurations moduleConfigurations) {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("WinformMenusConfiguration",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<WinformMenuConfiguration>());
        }
    }
}
