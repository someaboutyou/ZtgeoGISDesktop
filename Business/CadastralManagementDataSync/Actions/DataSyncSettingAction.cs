using Abp.Dependency;
using Abp.Events.Bus;
using CadastralManagementDataSync.Setting;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid;
using Ztgeo.Gis.Hybrid.FormIO;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace CadastralManagementDataSync.Actions
{
    /// <summary>
    /// 同步设置操作设置
    /// </summary>
    public class DataSyncSettingAction : IMenuAction
    {
        private readonly IocManager iocManager;
        private readonly IFormIOSchemeManager formIOSchemeManager;
        private readonly IDataSyncSettingsManager dataSyncSettingsManager;
        public WinformMenu SenderMenu { set; private get; }
        public ILogger Logger { get; set; }
        public DataSyncSettingAction(IocManager _iocManager, 
            IFormIOSchemeManager _formIOSchemeManager, 
            IDataSyncSettingsManager _dataSyncSettingManager 
             ) {
            iocManager = _iocManager;
            formIOSchemeManager = _formIOSchemeManager;
            dataSyncSettingsManager = _dataSyncSettingManager;
            Logger = NullLogger.Instance;
        }
        public void Excute()
        {
            DialogHybirdForm<FormIOControl> dialog = new DialogHybirdForm<FormIOControl>(iocManager, typeof(ZtgeoGisHybridMoudle).Assembly, new string[] {
                        "WebViews","FormIO","FormIoWebView.html"
                    });
            dialog.Size = new Size(1260, 760);
            dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            string component = formIOSchemeManager.GetFormIOSchemeById(1);
            string data = dataSyncSettingsManager.GetDataSyncSettings();
            ((FormIOControl)dialog.hybridControl).SetFormIOComponentAndData(component, data);
            ((FormIOControl)dialog.hybridControl).OnSave = (control, submissionData) => {
                try
                {
                    dynamic subdata = JsonConvert.DeserializeObject<dynamic>(submissionData);
                    dataSyncSettingsManager.SetDataSyncSettings(subdata);
                }
                catch (Exception ex)
                {
                    EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
                }
            };
            dialog.ShowDialog();
        }
    }
}
