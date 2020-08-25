using Abp.Dependency;
using Abp.Events.Bus;
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
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace ZtgeoGISDesktop.Forms
{
    public static class LoginForm
    {
        public static void ShowDialog(IocManager iocManager, IFormIOSchemeManager formIOSchemeManager) {
            DialogHybirdForm<FormIOControl> dialog = new DialogHybirdForm<FormIOControl>(iocManager, typeof(ZtgeoGisHybridMoudle).Assembly, new string[] {
                        "WebViews","FormIO","FormIoWebView.html"
                    });
            dialog.Size = new Size(1260, 760);
            dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            string component = formIOSchemeManager.GetFormIOSchemeById(2);
            ((FormIOControl)dialog.hybridControl).SetFormIOComponentAndData(component, "{}");
            ((FormIOControl)dialog.hybridControl).OnSave = (submissionData) => {
                try
                {
                    dynamic subdata = JsonConvert.DeserializeObject<dynamic>(submissionData); 
                }
                catch (Exception ex)
                {
                    EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
                }
            };
            if (dialog.ShowDialog()==DialogResult.Cancel) { 
                
            }
        }
    }
}
