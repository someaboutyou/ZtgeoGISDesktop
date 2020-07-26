using Abp.Configuration.Startup; 

namespace Ztgeo.Gis.Hybrid.Configuration
{
    public static class HybridConfigurationExtensions
    {
        public static HybridConfiguration HybridConfigurations(this IModuleConfigurations moduleConfigurations) {
            return moduleConfigurations.AbpConfiguration.GetOrCreate("HybridConfiguration",
                ()=> moduleConfigurations.AbpConfiguration.IocManager.Resolve<HybridConfiguration>());
        }
    }
}
