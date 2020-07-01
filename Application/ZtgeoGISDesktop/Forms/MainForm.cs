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
using Abp.Winform.ABPForm;

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm,IMainForm
    {
        public MainForm()
        {
            
        }
        void navBarControl_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            navigationFrame.SelectedPageIndex = navBarControl.Groups.IndexOf(e.Group);
        }
        void barButtonNavigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int barItemIndex = barSubItemNavigation.ItemLinks.IndexOf(e.Link);
            navBarControl.ActiveGroup = navBarControl.Groups[barItemIndex];
        }

        void IMainForm.StartInitializeComponent()
        {
            InitializeComponent();
        }

        void IMainForm.MenuInitialize()
        {
            throw new NotImplementedException();
        }
    }
}