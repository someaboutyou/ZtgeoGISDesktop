using Abp.Configuration;
using Abp.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.SettingExtension
{
    public class AutoSettingsProvider : SettingProvider
    {
        private readonly IAssemblyFinder _assemblyFinder;
        public AutoSettingsProvider(IAssemblyFinder assemblyFinder) {
            _assemblyFinder = assemblyFinder;
        }
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var settings = new List<SettingDefinition>();
             
            foreach (Assembly assembly in _assemblyFinder.GetAllAssemblies()) {
                var types = assembly
                                      .GetTypes()
                                      .Where(t => t.IsClass && typeof(ISettings).IsAssignableFrom(t));
                foreach (var type in types)
                {
                    var scopes = SettingScopes.All;
                    foreach (var p in type.GetProperties())
                    {
                        var key = AutoSettingsUtils.CreateSettingName(type, p.Name);
                        var isVisibleToClients = false;
                        var defaultValue = AutoSettingsUtils.GetDefaultValue(p.PropertyType);
                        var attr = p.GetCustomAttribute<AutoSettingDefinitionAttribute>();
                        if (attr != null)
                        {
                            scopes = attr.Scopes;
                            defaultValue = attr.DefaultValue;
                            isVisibleToClients = attr.IsVisibleToClients;
                        }
                        settings.Add(new SettingDefinition(
                               name: key,
                               defaultValue: defaultValue?.ToString(),
                               scopes: scopes,
                               isVisibleToClients: isVisibleToClients
                                ));
                    }
                }
            }
            
            //int io= types.Count();
            

            return settings;
        }
    }
}
