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
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraBars;

namespace ZtgeoGISDesktop.Forms
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();

        }
        XtraTabbedMdiManager mdiManager;
        private void XtraForm1_Load(object sender, EventArgs e)
        {
            BarManager barManager = new BarManager();
            barManager.Form = this;
            // Create a bar with a New button.
            barManager.BeginUpdate();
            Bar bar = new Bar(barManager, "My Bar");
            bar.DockStyle = BarDockStyle.Top;
            barManager.MainMenu = bar;
            BarItem barItem = new BarButtonItem(barManager, "New");
            barItem.ItemClick += new ItemClickEventHandler(barItem_ItemClick);
            bar.ItemLinks.Add(barItem);
            barManager.EndUpdate();
            // Create an XtraTabbedMdiManager that will manage MDI child windows.
            mdiManager = new XtraTabbedMdiManager(components);
            mdiManager.MdiParent = this;
            mdiManager.PageAdded += MdiManager_PageAdded;
            
            XtraForm2 f = new XtraForm2();
            f.Text = "Child Form " + (++ctr).ToString();
            f.MdiParent = this;
            f.Show();
        }
        private void MdiManager_PageAdded(object sender, MdiTabPageEventArgs e)
        {
            XtraMdiTabPage page = e.Page;
            page.Tooltip = "Tooltip for the page " + page.Text;
        }

        int ctr = 0;
        void barItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Create an MDI child form.
            XtraForm2 f = new XtraForm2();
            f.Text = "Child Form " + (++ctr).ToString();
            f.MdiParent = this;
            f.Show();
        }
    }
}