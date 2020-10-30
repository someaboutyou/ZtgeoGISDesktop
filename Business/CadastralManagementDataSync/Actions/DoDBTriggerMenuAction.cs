using Abp.Dependency;
using Abp.Events.Bus;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.DataOperation.Model;
using CadastralManagementDataSync.DBOperation;
using CadastralManagementDataSync.Forms;
using CadastralManagementDataSync.Menus;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;

namespace CadastralManagementDataSync.Actions
{
    public class DoDBTriggerMenuAction : IMenuAction
    {
        private readonly IocManager iocManager;
        private readonly TriggerOperation triggerOperation;
        public ILogger Logger { get; set; }
        public DoDBTriggerMenuAction(IocManager _iocManager, TriggerOperation _triggerOperation ) {
            iocManager = _iocManager;
            triggerOperation = _triggerOperation;
            Logger = NullLogger.Instance;
        }
        public WinformMenu SenderMenu { set; private get; }

        public void Excute()
        {
            try
            {
                DataSyncConfig config = DataSyncConfig.GetDataSyncConfig();
                if (config != null)
                {
                    DataSyncDirection dataSyncDirection =
                       SenderMenu.Name.Equals(DataSyncMenuNames.DataSyncPageSqlCreateGroupInnerDataInitMenu) ? DataSyncDirection.InnerDataSync : DataSyncDirection.OuterDataSync;
                    string result = triggerOperation.DoDBTriggerOperation(config, dataSyncDirection);
                    TextShowDialog textShowDialog = new TextShowDialog();
                    textShowDialog.SetText(result);
                    textShowDialog.ShowDialog();
                }
                else
                {
                    Logger.Warn("未找到数据同步的配置。/DataOperation/DataCaptureConfig.json");
                }
            }
            catch (Exception ex)
            {
                EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
            }
        }
    }
}
