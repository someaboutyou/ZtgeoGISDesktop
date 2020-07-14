using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication.Configuration
{
    public static class HttpInterceptConfigurationExtensions
    {
        public static HttpInterceptConfiguration HttpInterceptEvents(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("HttpInterceptConfiguration",
               () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<HttpInterceptConfiguration>());
        }
    }
}
