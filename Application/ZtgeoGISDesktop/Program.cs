using Abp.Winform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms; 
using ZtgeoGISDesktop.SplashScreen;

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
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Utils.AppearanceObject.DefaultFont = new Font("Segoe UI", 8);
            //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");
            Form mainForm = AbpApplicationBuilderExtensions.UseAbp<ZtgeoGISDesktopMoudle>(new SplashScreenFormManager(), null);
            if (mainForm != null) { 
                Application.Run(mainForm);
            }
        }
    }
}
