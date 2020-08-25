using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension;

namespace ZtgeoGISDesktop.Communication.Configuration
{
    [AutoMap(typeof(CommunicationSettings))]
    public class CommunicationSettings:ISettings
    {
        [AutoSettingDefinition("localhost")]
        public virtual string BackendHost { get; set; }
        [AutoSettingDefinition("8033")]
        public virtual string BackendPort { get; set; }
        [AutoSettingDefinition("http")]
        public virtual string BackendScheme { get; set; }
    }
}
