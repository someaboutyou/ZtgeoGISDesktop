using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Background.Configuration
{
    public static class HangfireDashboardConfigurationExtensions
    {
        public static HangfireDashboardConfiguration HangfireDashboard(this IModuleConfigurations moduleConfigurations) {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("HangfireDashboard",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<HangfireDashboardConfiguration>());
        }
    }
}
