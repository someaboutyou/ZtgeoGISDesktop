using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Ztgeo.Gis.Winform.ABPForm;
using DevExpress.XtraBars.Ribbon;
using Abp.Dependency;
using ZtgeoGISDesktop.Test;
using ZtgeoGISDesktop.Hybrid.WebView;

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm,IMainForm
    { 
        public IocManager IocManager { get; set; }
        public Control MenuContainerControl
        {
            get
            {
                return menuContainerControl;
            }
        }

        public MainForm(IocManager iocManager)
        {
            IocManager = iocManager;
        }
        TestHtmlControl testHtmlControl;
        public void StartInitializeComponent()
        {
            InitializeComponent();
            testHtmlControl = IocManager.Resolve<TestHtmlControl>();
            testHtmlControl.Dock = DockStyle.Fill;
            mainSplitContainer.Panel2.Controls.Add(testHtmlControl);
            testHtmlControl.LoadResource(typeof(ZtgeoGISDesktopHybridWebViewModule).Assembly, new string[]
            {
                "WebViews",
                "Common",
                string.Concat(new string[]
                {
                    "webview", "", ".html?render=", "",
                    "&readOnly=",
                     "false"
                })
            });
        }
        void navBarControl_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
        }
        void barButtonNavigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int barItemIndex = barSubItemNavigation.ItemLinks.IndexOf(e.Link);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            testHtmlControl.JsAlert("JsAlert!");
        }
    }
}