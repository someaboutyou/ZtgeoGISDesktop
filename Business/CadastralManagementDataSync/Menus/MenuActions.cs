using Abp.Dependency;
using Abp.Events.Bus;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.DataOperation.Model;
using CadastralManagementDataSync.DBOperation;
using CadastralManagementDataSync.Forms;
using CadastralManagementDataSync.Setting;
using Castle.Core.Logging;
using DevExpress.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid;
using Ztgeo.Gis.Hybrid.FormIO;
using Ztgeo.Gis.Runtime;
using Ztgeo.Utils;
using ZtgeoGISDesktop.Winform.Share;
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace CadastralManagementDataSync.Menus
{
    public static class MenuActions
    {
        public static void DataSyncSettingClick(IocManager iocManager, IFormIOSchemeManager formIOSchemeManager, IDataSyncSettingsManager dataSyncSettingManager, ILogger logger) {
            DialogHybirdForm<FormIOControl> dialog = new DialogHybirdForm<FormIOControl>(iocManager, typeof(ZtgeoGisHybridMoudle).Assembly, new string[] {
                        "WebViews","FormIO","FormIoWebView.html"
                    });
            dialog.Size = new Size(1260, 760);
            dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            string component= formIOSchemeManager.GetFormIOSchemeById(1);
            string data = dataSyncSettingManager.GetDataSyncSettings();
            ((FormIOControl)dialog.hybridControl).SetFormIOComponentAndData(component, data);
            ((FormIOControl)dialog.hybridControl).OnSave = (control,submissionData)=> {
                try
                { 
                    dynamic subdata = JsonConvert.DeserializeObject<dynamic>(submissionData);
                    dataSyncSettingManager.SetDataSyncSettings(subdata);
                }
                catch (Exception ex) {
                    EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
                }
            };
            dialog.ShowDialog();
        }
        private static DataSyncConfig GetDataSyncConfig() {
            string configPath = System.Windows.Forms.Application.StartupPath + "/DataOperation/DataCaptureConfig.json";
            DataSyncConfig config = null;
            using (System.IO.StreamReader file = System.IO.File.OpenText(configPath))
            {
                string json = file.ReadToEnd();
                config = JsonConvert.DeserializeObject<DataSyncConfig>(json);
            }
            return config;
        }
        public static void DataSyncOperationClick(IocManager iocManager, DataSyncDirection dataSyncDirection, DataCapture dataCapture, DataSyncOperator dataSyncOperator,ILogger logger) {
            try
            { 
                DataSyncConfig config = GetDataSyncConfig();
                if (config != null)
                {
                    WaitDialogForm sdf = new WaitDialogForm("提示", "正在同步数据......");  
                    dataSyncOperator.SyncData(config, dataSyncDirection);
                    dataCapture.CaptureDirtyFromDBAndSave(config, dataSyncDirection);
                    MessageBox.Show(dataSyncDirection== DataSyncDirection.InnerDataSync? "内网数据同步完成" : "外网数据同步完成");
                    sdf.Close();
                }
                else {
                    logger.Warn("未找到数据同步的配置。/DataOperation/DataCaptureConfig.json");
                }
            }
            catch (Exception ex) {
                EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
            }
        }

        public static void DoDBTriggerOperationCilck(IocManager iocManager, DataSyncDirection dataSyncDirection, TriggerOperation triggerOperation, ILogger logger) {
            try
            {
                DataSyncConfig config = GetDataSyncConfig();
                if (config != null)
                {
                    string result = triggerOperation.DoDBTriggerOperation(config, dataSyncDirection);
                    TextShowDialog textShowDialog = new TextShowDialog();
                    textShowDialog.SetText(result);
                    textShowDialog.ShowDialog();
                }
                else
                {
                    logger.Warn("未找到数据同步的配置。/DataOperation/DataCaptureConfig.json");
                }
            }
            catch (Exception ex) {
                EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
            }
        }
    }
}
