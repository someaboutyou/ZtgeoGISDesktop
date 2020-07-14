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

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm,IMainForm
    {
        private RibbonControl menuContainerControl;
        public Control MenuContainerControl { get {
                if (menuContainerControl == null) {
                    menuContainerControl = new DevExpress.XtraBars.Ribbon.RibbonControl(); 
                }
                return menuContainerControl;
        } }

        public MainForm()
        {
           
        }

        public void StartInitializeComponent() {
            InitializeComponent(); 
        }
        void navBarControl_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
        }
        void barButtonNavigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int barItemIndex = barSubItemNavigation.ItemLinks.IndexOf(e.Link);
        } 
    }
}