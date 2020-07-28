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
using ZtgeoGISDesktop.Test;
using Abp.Dependency;
using Ztgeo.Gis.Hybrid;

namespace ZtgeoGISDesktop.Forms
{
    public partial class DialogForm : DevExpress.XtraEditors.XtraForm
    {
        public DialogForm(IocManager iocManager)
        {
            InitializeComponent();
            var testHtmlControl = iocManager.Resolve<TestHtmlControl>();
            testHtmlControl.Dock = DockStyle.Fill;
            this.Controls.Add(testHtmlControl);
            testHtmlControl.LoadResource(typeof(ZtgeoGisHybridMoudle).Assembly, new string[]
            {
                "WebViews",
                "Common",
                string.Concat(new string[]
                {
                    "FormIoWebView", "", ".html?render=", "",
                    "&readOnly=",
                     "false"
                })
            });
        }
    }
}