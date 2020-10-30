﻿using System;
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
using Abp.Events.Bus;
using Ztgeo.Gis.Winform.Events;
using DevExpress.XtraEditors.Controls;
using ZtgeoGISDesktop.Resources;
using DevExpress.XtraBars.Docking2010.Views;

namespace ZtgeoGISDesktop.Forms
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm, IMainForm 
    {
        private readonly IFormIOSchemeManager formIOSchemeManager;
        private readonly ProductInfo productInfo; 
        //private BarItem StatusBarItem = new DevExpress.XtraBars.BarStaticItem();
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
        public Control LayerPanel { get { return this.documentManagerDocking.LayerControl; } }

        public Control PropertiesPanel { get { return this.documentManagerDocking.PropertiesControl; } }

        public Control ResourcesPanel { get { return this.documentManagerDocking.ResourceControl; } }

        public MainForm(IocManager iocManager, IFormIOSchemeManager _formIOSchemeManager, ProductInfo _productInfo)
        {
            IocManager = iocManager;
            formIOSchemeManager = _formIOSchemeManager;
            productInfo = _productInfo;
        } 
        public void StartInitializeComponent()
        {
            InitializeComponent();
            this.documentManagerDocking.TabbedView.DocumentClosing += this.ClosingDocument;
            this.documentManagerDocking.TabbedView.DocumentActivated += this.ActivatedDocument;
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
            //XtraUserControl child = new XtraUserControl();
            DocumentSettings DocumentSettings = new DocumentSettings();
            if (documentControl.Document != null)
            {
                DocumentSettings.Caption = documentControl.Document.DocumentName ?? name;
                if (documentControl.DocumentImage!=null)
                    DocumentSettings.Image = documentControl.DocumentImage;
            } 
            //DocumentSettings.Attach(child, DocumentSettings); 
            //((Control)documentControl).Parent = child;
            ((Control)documentControl).Dock = DockStyle.Fill;
            ((Control)documentControl).GotFocus += (sender,e) => {
                
            };
            ((Control)documentControl).Visible = true;
            DocumentSettings.Attach((Control)documentControl, DocumentSettings);
            this.documentManagerDocking.TabbedView.AddDocument((Control)documentControl); 
            return documentControl;
        }

 

        public void SetStatusInfo(IDocumentControl documentControl, StatusInfo statusInfo)
        {
            if (documentControl == null) {
                return;
            }
            if (this.documentStatuses.ContainsKey(documentControl))
            {
                documentStatuses[documentControl] = statusInfo;
            }
            else {
                documentStatuses.TryAdd(documentControl, statusInfo);
            }
            ShowDocumentStatus(documentControl);
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
        private void ShowDocumentStatus(IDocumentControl documentControl) {
            if (documentControl != null) {
                if (documentStatuses.ContainsKey(documentControl))
                {
                    ShowStauts(documentStatuses[documentControl]);
                }
            }
        }
        private void ShowStauts(StatusInfo statusInfo) {
            this.ribbonStatusBar.ItemLinks.Clear();
            BarItem StatusBarItem = null;
            if (statusInfo != null) {
                switch (statusInfo.StatusShowType)
                {
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
            } 
            if(StatusBarItem!=null)
                this.ribbonStatusBar.ItemLinks.Add(StatusBarItem);
            this.ribbonStatusBar.Refresh(); 
        } 
        /// <summary>
        /// 手动激活一个control
        /// </summary>
        /// <param name="documentControl"></param>
        public void ManualActiveADocumentControl(IDocumentControl documentControl) {
            var control = documentControl as Control;
            if (control != null) {
                this.documentManagerDocking.TabbedView.ActivateDocument(control);
            } 
        }
        private void ClosingDocument(object sender, DocumentCancelEventArgs e) {
            IDocumentManager documentManager= IocManager.Resolve<IDocumentManager>();
            documentManager.CloseADocumentControl(e.Document.Control as IDocumentControl);
        }

        private void ActivatedDocument(object sender, DocumentEventArgs e) {
            var tempActiveDocument = this.ActiveDocumentControl;
            this.ActiveDocumentControl = e.Document.Control as IDocumentControl;
            if (this.ActiveDocumentControl.PropertiesControl != null)
            {
                Control propertiesControl = (Control)this.ActiveDocumentControl.PropertiesControl;
                propertiesControl.Dock = DockStyle.Fill;
                if (this.PropertiesPanel.Controls.Count > 0)
                {
                    this.PropertiesPanel.Controls.Clear();
                }
                this.PropertiesPanel.AddControl(propertiesControl);
            }
            else
            { //为空隐藏
                this.PropertiesPanel.Controls.Clear(); 
            }
            if (this.ActiveDocumentControl.LayerControl != null) {
                Control layerControl = (Control)this.ActiveDocumentControl.LayerControl;
                layerControl.Dock = DockStyle.Fill;
                if (this.LayerPanel.Controls.Count > 0)
                {
                    this.LayerPanel.Controls.Clear();
                }
                this.LayerPanel.AddControl(layerControl);
            }
            else
            { //为空隐藏
                this.LayerPanel.Controls.Clear(); 
            }
            this.ActiveDocumentControl.Activated();
            EventBus.Default.Trigger(new DocumentActiveChangeEventData { ChangeFromDocumentControl = tempActiveDocument, ChangeToDocumentControl = ActiveDocumentControl });
        }
    }
}