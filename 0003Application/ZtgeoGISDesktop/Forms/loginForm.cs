using Abp.Dependency;
using Abp.Events.Bus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid;
using Ztgeo.Gis.Hybrid.FormIO;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Runtime.Authorization.Login;
using Ztgeo.Gis.Runtime.Context;
using Ztgeo.Utils;
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace ZtgeoGISDesktop.Forms
{
    public static class LoginForm
    {
        public static void ShowDialog(IocManager iocManager, IFormIOSchemeManager formIOSchemeManager,ILoginManager loginManager) {
            DialogHybirdForm<FormIOControl> dialog = new DialogHybirdForm<FormIOControl>(iocManager, typeof(ZtgeoGisHybridMoudle).Assembly, new string[] {
                        "WebViews","FormIO","FormIoWebView.html"
                    });
            dialog.Size = new Size(550, 520); 
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.ShowIcon = true;
            dialog.Icon = new Icon(AssemblyResource.GetResourceStream(typeof(ZtgeoGISDesktopMoudle).Assembly, "ZtgeoGISDesktop.Icons.login.ico"));
            string component = formIOSchemeManager.GetFormIOSchemeById(2);
            ((FormIOControl)dialog.hybridControl).SetFormIOComponentAndData(component, "{}");
            ((FormIOControl)dialog.hybridControl).OnSave = (control,submissionData) => {
                try
                {
                    dynamic subdata = JsonConvert.DeserializeObject<dynamic>(submissionData);
                    var authenticateResultModel= loginManager.Login(new Ztgeo.Gis.Runtime.Authorization.AuthenticateModel { UserNameOrEmailAddress = subdata.data.UserName, Password = subdata.data.Password }, (bool)subdata.data.IsServerless);
                    
                    if (authenticateResultModel != null && authenticateResultModel.ShouldResetPassword) {
                        MessageBox.Show("密码需要重置，请联系管理员");
                        return;
                    }
                    if (authenticateResultModel != null) {
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }

                }
                catch (Exception ex)
                {
                    EventBus.Default.Trigger(new NonUIExceptionEventData { UnhandledExceptionEventArgs = new UnhandledExceptionEventArgs(ex, false) });
                }
            };
            if (dialog.ShowDialog() == DialogResult.Cancel) { 
                iocManager.Resolve<RuntimeContext>().ShutdownImmediate();
                //System.Environment.Exit(0);
            }
        }
    }
}
