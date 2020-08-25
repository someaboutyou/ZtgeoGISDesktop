using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Configuration
{
    public static class WinformToolbarConfigurationExtensions
    {
        public static WinformToolbarConfiguration WinformToolbars(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("WinformToolbarsConfiguration",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<WinformToolbarConfiguration>());
        }
    }
}
