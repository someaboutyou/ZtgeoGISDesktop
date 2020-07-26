using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.WebViewControl;

namespace WebViewControlTest
{
    public partial class Form1 : Form
    {
        private WebView webView;
        public Form1()
        {
            InitializeComponent();
            
            webView.LoadResource(new ResourceUrl(typeof(Form1).Assembly, "WebViews", "Common", "webview.html"));
            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                WebView.OnApplicationExit(null, null);
            };
        }

        //[System.Runtime.InteropServices.ComVisible(true)]
        public class JsAdapter {
            public void close() {
                if (MessageBox.Show("!") == DialogResult.OK) {
                    Process.GetCurrentProcess().Kill();
                } 
            }
        }
    }
}
