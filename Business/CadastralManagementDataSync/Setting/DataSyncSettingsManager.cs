using Abp.Configuration;
using Abp.Domain.Services;
using Abp.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension;

namespace CadastralManagementDataSync.Setting
{
    public interface IDataSyncSettingsManager : IDomainService{
        string GetDataSyncSettings();
        void SetDataSyncSettings(dynamic data);
    }
    public class DataSyncSettingsManager: IDataSyncSettingsManager
    { 
        private readonly DataSyncSettings dataSyncSettings;
        private readonly ISettingManager settingManager;
        public DataSyncSettingsManager(DataSyncSettings _dataSyncSettings,ISettingManager _settingManager) {
            dataSyncSettings = _dataSyncSettings;
            settingManager = _settingManager;
        }

        public string GetDataSyncSettings() {
            return JsonConvert.SerializeObject(new
            {
                InnerSyncSetting = new
                {
                    Address = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "InnerSyncSetting_Address"))?? dataSyncSettings.InnerSyncSetting_Address,
                    Port = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "InnerSyncSetting_Port")) ?? dataSyncSettings.InnerSyncSetting_Port,
                    SID = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "InnerSyncSetting_SID")) ?? dataSyncSettings.InnerSyncSetting_SID,
                    User = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "InnerSyncSetting_User")) ?? dataSyncSettings.InnerSyncSetting_User,
                    Password = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "InnerSyncSetting_Password")) ?? dataSyncSettings.InnerSyncSetting_Password
                },
                OutNetworkSyncSetting = new
                {
                    Address = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "OutNetworkSyncSetting_Address"))?? dataSyncSettings.OutNetworkSyncSetting_Address,
                    Port = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "OutNetworkSyncSetting_Port"))?? dataSyncSettings.OutNetworkSyncSetting_Port,
                    SID = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "OutNetworkSyncSetting_SID")) ?? dataSyncSettings.OutNetworkSyncSetting_SID,
                    User = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "OutNetworkSyncSetting_User")) ?? dataSyncSettings.OutNetworkSyncSetting_User,
                    Password = settingManager.GetSettingValueForApplication(AutoSettingsUtils.CreateSettingName(typeof(DataSyncSettings), "OutNetworkSyncSetting_Password")) ?? dataSyncSettings.OutNetworkSyncSetting_Password
                }
            });
        }

        public void SetDataSyncSettings(dynamic data) {
            dataSyncSettings.InnerSyncSetting_Address = data.data.InnerSyncSetting.Address;
            dataSyncSettings.InnerSyncSetting_Password = data.data.InnerSyncSetting.Password;
            dataSyncSettings.InnerSyncSetting_Port = data.data.InnerSyncSetting.Port;
            dataSyncSettings.InnerSyncSetting_SID = data.data.InnerSyncSetting.SID;
            dataSyncSettings.InnerSyncSetting_User = data.data.InnerSyncSetting.User; 
            dataSyncSettings.OutNetworkSyncSetting_Address = data.data.OutNetworkSyncSetting.Address;
            dataSyncSettings.OutNetworkSyncSetting_Password = data.data.OutNetworkSyncSetting.Password;
            dataSyncSettings.OutNetworkSyncSetting_Port = data.data.OutNetworkSyncSetting.Port;
            dataSyncSettings.OutNetworkSyncSetting_SID = data.data.OutNetworkSyncSetting.SID;
            dataSyncSettings.OutNetworkSyncSetting_User = data.data.OutNetworkSyncSetting.User;
        }
    }
}
