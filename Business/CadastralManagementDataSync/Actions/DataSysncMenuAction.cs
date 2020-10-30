using Abp.Dependency;
using Abp.Events.Bus;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.DataOperation.Model;
using CadastralManagementDataSync.Menus;
using CadastralManagementDataSync.Resource;
using Castle.Core.Logging;
using DevExpress.Utils;
using DevExpress.XtraEditors;
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
    public class DataSysncMenuAction:IMenuAction
    {
        private readonly IocManager iocManager; 
        private readonly DataCapture dataCapture;
        private readonly DataSyncOperator dataSyncOperator;
        public WinformMenu SenderMenu { set; private get; }
        public   ILogger Logger { get; set; } 
        public DataSysncMenuAction(IocManager _iocManager, DataCapture _dataCapture, DataSyncOperator _dataSyncOperator ) {
            iocManager = _iocManager; 
            dataCapture = _dataCapture;
            dataSyncOperator = _dataSyncOperator;
        }

        public void Excute()
        {
            WaitDialogForm sdf = null;
            try
            {
                DataSyncConfig config = DataSyncConfig.GetDataSyncConfig();
                if (config != null)
                {
                    DataSyncDirection dataSyncDirection =
                        SenderMenu.Name.Equals(DataSyncMenuNames.DataSyncPageDoDataSyncGroupInnerDoDataSyncMenu) ? DataSyncDirection.InnerDataSync : DataSyncDirection.OuterDataSync;
                    sdf = new WaitDialogForm("提示", "正在同步数据......");
                    dataSyncOperator.SyncData(config, dataSyncDirection); 
                    string filePath = dataCapture.CaptureDirtyFromDBAndSave(config, dataSyncDirection);
                    //打开数据
                    SyncDataResource syncDataResource = iocManager.Resolve<SyncDataResource>();
                    syncDataResource.FullName = filePath;
                    syncDataResource.Open();
                    XtraMessageBox.Show(dataSyncDirection == DataSyncDirection.InnerDataSync ? "内网数据同步完成" : "外网数据同步完成"); 
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
            finally {
                sdf.Close();
            }
        }
    }
}
