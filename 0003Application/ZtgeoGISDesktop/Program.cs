using Ztgeo.Gis.Winform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms; 
using ZtgeoGISDesktop.SplashScreen;
using System.Threading;
using Abp.Dependency;
using ZtgeoGISDesktop.Forms;
using Ztgeo.Gis.Winform.ABPForm;
using Abp.Events.Bus;
using Ztgeo.Gis.Runtime;
using Castle.Facilities.Logging;
using Abp.Castle.Logging.Log4Net;
using Abp.Logging;
using Ztgeo.Gis.Runtime.Context;

namespace ZtgeoGISDesktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Utils.AppearanceObject.DefaultFont = new Font("Segoe UI", 8);
            //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();
            //加载IOC容器
            IIocManager iocManager = AbpApplicationBuilderExtensions.UseAbp<ZtgeoGISDesktopMoudle>(new SplashScreenFormManager(),
                optionAction =>
                {
                    optionAction.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
                });
            //初始化界面
            IMainForm mainForm = iocManager.Resolve<IMainForm>(); 
            if (mainForm != null)
            {
                #region 全局异常处理
                Application.ThreadException += (object sender, ThreadExceptionEventArgs e) =>
                {
                    EventBus.Default.Trigger<UIExceptionEventData>(new UIExceptionEventData { ThreadExceptionEventArgs = e });
                };
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((object sender, UnhandledExceptionEventArgs e) =>
                {
                    EventBus.Default.Trigger<NonUIExceptionEventData>(new NonUIExceptionEventData { UnhandledExceptionEventArgs = e });
                });
                #endregion
                Application.Run((Form)mainForm); 
            }
        }
    }
}
