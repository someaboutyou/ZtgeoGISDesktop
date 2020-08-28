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
        private readonly IFormIOSchemeManager formIOSchemeManager;
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
         
        public MainForm(IocManager iocManager, IFormIOSchemeManager _formIOSchemeManager)
        {
            IocManager = iocManager;
            formIOSchemeManager = _formIOSchemeManager;
        } 
        public void StartInitializeComponent()
        {
            InitializeComponent();
            var loginManager = IocManager.Resolve<ILoginManager>();
            if (!loginManager.IsLogined())
            {
                 LoginForm.ShowDialog(IocManager, formIOSchemeManager, loginManager);
            } 
        }

        public Control AddADocument()
        {
            throw new NotImplementedException();
        }
    }
}