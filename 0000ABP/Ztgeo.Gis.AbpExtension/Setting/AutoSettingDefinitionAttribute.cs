using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Setting
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoSettingDefinitionAttribute : Attribute
    {
        public object DefaultValue { get; private set; }

        public bool IsVisibleToClients { get; private set; }

        public SettingScopes Scopes { get; private set; }

        public AutoSettingDefinitionAttribute(object defaultValue, bool isVisibleToClients = true, SettingScopes scopes = SettingScopes.Application)
        {
            this.DefaultValue = defaultValue;
            this.IsVisibleToClients = isVisibleToClients;
            this.Scopes = scopes;
        }
    }
}
