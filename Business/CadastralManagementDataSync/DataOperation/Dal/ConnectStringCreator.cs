using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation.Dal
{
    public class ConnectStringCreator :ISingletonDependency
    {

        private readonly DataSyncSettings dataSyncSettings;

        public ConnectStringCreator(DataSyncSettings _dataSyncSettings) {
            dataSyncSettings = _dataSyncSettings;
        }

        public string GetOracleConstr(DataSyncDirection dataSyncDirection) {
            if (dataSyncDirection == DataSyncDirection.InnerDataSync)
            {
                return OracleHelper.GetConnStr(dataSyncSettings.InnerSyncSetting_Address,
                    dataSyncSettings.InnerSyncSetting_Port,
                    dataSyncSettings.InnerSyncSetting_SID,
                    dataSyncSettings.InnerSyncSetting_User,
                    dataSyncSettings.InnerSyncSetting_Password
                    );
            }
            else {
                return OracleHelper.GetConnStr(dataSyncSettings.OutNetworkSyncSetting_Address,
                   dataSyncSettings.OutNetworkSyncSetting_Port,
                   dataSyncSettings.OutNetworkSyncSetting_SID,
                   dataSyncSettings.OutNetworkSyncSetting_User,
                   dataSyncSettings.OutNetworkSyncSetting_Password
                   );
            }
        }
    }
}
