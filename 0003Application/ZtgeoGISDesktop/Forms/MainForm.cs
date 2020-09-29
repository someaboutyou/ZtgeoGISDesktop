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
using Ztgeo.Gis.Runtime.Context;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormStatusBar;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using System.Collections.Concurrent;
using CefSharp.WinForms.Internals;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils.Extensions;

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm, IMainForm 
    {
        private readonly IFormIOSchemeManager formIOSchemeManager;
        private readonly ProductInfo productInfo; 
        private BarItem StatusBarItem = new DevExpress.XtraBars.BarStaticItem();
        private ConcurrentDictionary<IDocumentControl, StatusInfo> documentStatuses = new ConcurrentDictionary<IDocumentControl, StatusInfo>(); //document状态
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
                return this.documentManagerDocking.StandaloneBarDockControl;
            }
        } 
        public IDocumentControl ActiveDocumentControl { get; private set; }
        public Control LayerPanel => throw new NotImplementedException();

        public Control PropertiesPanel { get { return this.documentManagerDocking.PropertiesControl; } }

        public Control ResourcesPanel => throw new NotImplementedException();

        public MainForm(IocManager iocManager, IFormIOSchemeManager _formIOSchemeManager, ProductInfo _productInfo)
        {
            IocManager = iocManager;
            formIOSchemeManager = _formIOSchemeManager;
            productInfo = _productInfo;
        } 
        public void StartInitializeComponent()
        {
            InitializeComponent(); 
            this.ribbonStatusBar.ItemLinks.Add(this.StatusBarItem);
            this.Text = productInfo.ProductName;
            var loginManager = IocManager.Resolve<ILoginManager>();
            if (!loginManager.IsLogined())
            {
                 LoginForm.ShowDialog(IocManager, formIOSchemeManager, loginManager);
            } 
        }
        /// <summary>
        /// 在中间的tab 页新添加一个文档
        /// </summary>
        /// <param name="documentControl"></param>
        /// <returns></returns>
        public IDocumentControl AddADocument(IDocumentControl documentControl,string name)
        {
            if (!(documentControl is Control)) {
                throw new WinformUIExceptionDeal("Type of " + documentControl.GetType().FullName +" is not a control.");
            }
            XtraUserControl child = new XtraUserControl();
            DocumentSettings DocumentSettings = new DocumentSettings();
            if (documentControl.Document != null)
            {
                DocumentSettings.Caption = documentControl.Document.DocumentName ?? name;
                if (documentControl.DocumentImage!=null)
                    DocumentSettings.Image = documentControl.DocumentImage;
            } 
            DocumentSettings.Attach(child, DocumentSettings); 
            ((Control)documentControl).Parent = child;
            ((Control)documentControl).Dock = DockStyle.Fill;
            ((Control)documentControl).GotFocus += (sender,e) => {
                this.ActiveDocumentControl = sender as IDocumentControl;
            };
            ((Control)documentControl).Visible = true;
            this.documentManagerDocking.TabbedView.AddDocument(child);
            if (documentControl.PropertiesControl != null)
            {
                Control propertiesControl = (Control)documentControl.PropertiesControl;
                propertiesControl.Dock = DockStyle.Fill;
                this.PropertiesPanel.AddControl(propertiesControl);
            }
            return documentControl;
        }

 

        public void SetStatusInfo(IDocumentControl documentControl, StatusInfo statusInfo)
        {
            if (this.documentStatuses.ContainsKey(documentControl))
            {
                documentStatuses[documentControl] = statusInfo;
            }
            else {
                documentStatuses.TryAdd(documentControl, statusInfo);
            }
            ShowActiveDocumentStauts();
        }

        public void ClearStatusInfo(IDocumentControl documentControl)
        {
            if (documentStatuses.ContainsKey(documentControl)) {
                StatusInfo statusInfo;
                documentStatuses.TryRemove(documentControl,out statusInfo);
            }
            ShowActiveDocumentStauts();
        }
        /// <summary>
        /// 查看当前文档状态
        /// </summary>
        private void ShowActiveDocumentStauts() {
            if (this.ActiveDocumentControl != null) {
                if (documentStatuses.ContainsKey(this.ActiveDocumentControl)) { 
                    ShowStauts(documentStatuses[this.ActiveDocumentControl]);
                }
            }
        }
        private void ShowStauts(StatusInfo statusInfo) { 
            switch (statusInfo.StatusShowType) {
                case StatusShowType.Msg:
                    StatusBarItem = new DevExpress.XtraBars.BarStaticItem();
                    StatusBarItem.Caption = statusInfo.Message;  
                    break;
                 case StatusShowType.ProcessBar:
                    StatusBarItem = new BarEditItem();
                    RepositoryItemProgressBar progressBarControl = new RepositoryItemProgressBar();
                    progressBarControl.Maximum = statusInfo.MaxValue;
                    progressBarControl.Minimum = 0;
                    progressBarControl.Step = statusInfo.CurrentValue;
                    ((BarEditItem)StatusBarItem).Edit = progressBarControl; 
                    break;
                case StatusShowType.ProcessBarWithoutProcess:
                    StatusBarItem = new BarEditItem();
                    RepositoryItemMarqueeProgressBar marqueeProgressBar = new RepositoryItemMarqueeProgressBar();
                    ((BarEditItem)StatusBarItem).Edit = marqueeProgressBar; 
                    break;
            }
            this.ribbonStatusBar.Refresh(); 
        }

    }
}