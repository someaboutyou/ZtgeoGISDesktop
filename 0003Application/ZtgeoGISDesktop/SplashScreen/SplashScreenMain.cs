using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace ZtgeoGISDesktop.SplashScreen
{
    public partial class SplashScreenMain : DemoSplashScreen {
        int dotCount = 0;
        public SplashScreenMain() { 
            InitializeComponent();
            labelControl1.Text = string.Format("{0}{1}", labelControl1.Text, GetYearString()); 
            Timer tmr = new Timer();
            tmr.Interval = 400;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Start();
        }

        void tmr_Tick(object sender, EventArgs e) {
            if(++dotCount > 3) dotCount = 0;
            labelControl2.Text = string.Format("{1}{0}", GetDots(dotCount), "¿ªÊ¼");
        }

        string GetDots(int count) {
            string ret = string.Empty;
            for(int i = 0; i < count; i++) ret += ".";
            return ret;
        }
        int GetYearString() {
            int ret = DateTime.Now.Year;
            return (ret < 2012 ? 2012 : ret);
        }
    }
}
