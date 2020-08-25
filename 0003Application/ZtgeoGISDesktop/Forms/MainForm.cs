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
using DevExpress.XtraBars;
using Ztgeo.Gis.Runtime.Authorization.Login;
using Ztgeo.Gis.Hybrid.FormIO;

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm, IMainForm 
    { 
        public IocManager IocManager { get; set; }
        public Control MenuContainerControl
        {
            get
            {
                return menuContainerControl;
            }
        }
         
        public Object ToolBarManager
        {
            get
            {
                return this.mainFormBarManager;
            }
        }

        public Control StandaloneBarDockControl {
            get {
                return this.MainFormStandaloneBarDockControl;
            }
        }
         
        public MainForm(IocManager iocManager, IFormIOSchemeManager formIOSchemeManager)
        {
            IocManager = iocManager;
            var loginManager= iocManager.Resolve<ILoginManager>();
            if (!loginManager.IsLogined()) {
                LoginForm.ShowDialog(iocManager, formIOSchemeManager);
            }
        } 
        public void StartInitializeComponent()
        {
            InitializeComponent(); 
        }  

        
    }
}