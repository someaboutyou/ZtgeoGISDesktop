using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Setting
{
    public class AutoSettingsProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var settings = new List<SettingDefinition>();

            var types = this.GetType().Assembly
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

            return settings;
        }
    }
}
