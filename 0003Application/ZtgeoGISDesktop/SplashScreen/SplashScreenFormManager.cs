using Ztgeo.Gis.Winform.ABPForm;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.SplashScreen
{
    public class SplashScreenFormManager : ISplashScreenFormManager
    {
        void ISplashScreenFormManager.CloseSplashScreenForm()
        {
            if (SplashScreenManager.Default != null)
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        void ISplashScreenFormManager.ShowSplashScreenForm()
        {
            SplashScreenManager.ShowForm(null, typeof(SplashScreenMain), true, true, false, 1000);
        }
    }
}
