using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Communication.Configuration
{
    public class CommunicationSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[] {
                new SettingDefinition(BackendSettingNames.Host,"localhost",isVisibleToClients:true),
                new SettingDefinition(BackendSettingNames.Port,"8033",isVisibleToClients:true)
            };
        }
    }
}
