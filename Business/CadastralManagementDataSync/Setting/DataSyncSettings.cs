using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension;

namespace CadastralManagementDataSync
{
    /// <summary>
    /// 数据同步设置
    /// </summary>
    [AutoMap(typeof(DataSyncSettings))]
    public class DataSyncSettings :ISettings
    {
        [AutoSettingDefinition("localhost")]
        public virtual string InnerSyncSetting_Address { get; set; } 

        [AutoSettingDefinition("1521")]
        public virtual string InnerSyncSetting_Port { get; set; }
        [AutoSettingDefinition("orcl")]
        public virtual string InnerSyncSetting_SID { get; set; }
        [AutoSettingDefinition("KJK")]
        public virtual string InnerSyncSetting_User { get; set; }
        [AutoSettingDefinition("KJK")]
        public virtual string InnerSyncSetting_Password { get; set; }

        [AutoSettingDefinition("localhost")]
        public virtual string OutNetworkSyncSetting_Address { get; set; }

        [AutoSettingDefinition("1521")]
        public virtual string OutNetworkSyncSetting_Port { get; set; }
        [AutoSettingDefinition("orcl")]
        public virtual string OutNetworkSyncSetting_SID { get; set; }
        [AutoSettingDefinition("KJK")]
        public virtual string OutNetworkSyncSetting_User { get; set; }
        [AutoSettingDefinition("KJK")]
        public virtual string OutNetworkSyncSetting_Password { get; set; }
    }
}
