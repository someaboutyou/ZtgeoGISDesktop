using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;

namespace ZtgeoGISDesktop.Resources
{

    public class ResourcePanel : XtraUserControl
    {

        public ResourcePanel() : base()
        { 
            InitializeComponent(); 
            CalcImageSize();
        }
        void CalcImageSize()
        {
            Item.ImageSize = new Size(16, 16);
            if (ScaleHelper.ScaleFactor.Width < 1.5)
                return;
            Item.ImageSize = ScaleHelper.ScaleSize(Item.ImageSize);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeLocalResourceTreeList();
        }
        #region localresource
        /// <summary>
        /// 初始化本地资源树
        /// </summary>
        private void InitializeLocalResourceTreeList() { 
            navigationTreeList.VirtualTreeGetChildNodes += OnNavigationTreeListGetChildNodes;
            navigationTreeList.VirtualTreeGetCellValue += OnNavigationTreeListGetCellValue;
            navigationTreeList.FocusedNodeChanged += OnNavigationTreeListFocusedNodeChanged;
            navigationTreeList.CustomDrawNodeImages += OnTreeListCustomDrawNodeImages;
            navigationTreeList.GetSelectImage += OnTreeListGetStateImage;
            navigationTreeList.DataSource = new RootItem();
           navigationTreeList.ForceInitialize();
            navigationTreeList.Nodes[0].Expand();
            if (navigationTreeList.Nodes[0].Nodes.Count > 2)
            {
                navigationTreeList.Nodes[0].Nodes[1].Expand();
                navigationTreeList.FocusedNode = navigationTreeList.Nodes[0].Nodes[1];
            }
            else
                navigationTreeList.FocusedNode = navigationTreeList.Nodes[0];
        }
        void OnNavigationTreeListGetCellValue(object sender, VirtualTreeGetCellValueInfo e)
        {
            e.CellData = ((Item)e.Node).DisplayName;
        }
        void OnNavigationTreeListGetChildNodes(object sender, VirtualTreeGetChildNodesInfo e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            e.Children = ((Item)e.Node).GetDirectories();
            Cursor.Current = current;
        }
        //</navigationTreeList>
        void OnNavigationTreeListFocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (navigationTreeList.FocusedNode == null)
                return;
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            //searchControl.ClearFilter();
            //searchControl.Properties.NullValuePrompt = "Search " + navigationTreeList.FocusedNode.GetDisplayText(0);
            Item _item = (Item)navigationTreeList.GetRow(navigationTreeList.FocusedNode.Id);
            //breadCrumbEdit.Properties.RootGlyph = _item.Image;
            //treeList1.DataSource = _item.GetFilesSystemInfo();
            Cursor.Current = current;
        }
        void OnTreeListCustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            TreeList tree = (TreeList)sender;
            IFileImage _item = (IFileImage)tree.GetRow(e.Node.Id);
            if (_item.Image == null)
                return;
            e.Cache.DrawImage(_item.Image, e.SelectImageLocation);
            e.Handled = true;
        }
        void OnTreeListGetStateImage(object sender, GetSelectImageEventArgs e)
        {
            e.NodeImageIndex = 0;
        }
        void OnTreeListMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            //TreeListHitInfo hitInfo = treeList1.CalcHitInfo(e.Location);
            //if(hitInfo.Node == null)
            //return;
            //TreeListNode pressedNode = hitInfo.Node;
            //CustomFileInfo fileInfo = (CustomFileInfo)treeList1.GetRow(pressedNode.Id);
            //if(fileInfo.Type == FileType.File)
            //    FileSystemHelper.ShellExecuteFileInfo(fileInfo.FullName, ShellExecuteInfoFileType.Open);
            //else
            //    navigationTreeList.FocusedNode = navigationTreeList.FocusedNode.Nodes[pressedNode.Id];
        }  
        void OnBreadCrumbEditPathChanged(object sender, BreadCrumbPathChangedEventArgs e)
        {
            UpdateButtonsState();
        }  

        void UpdateButtonsState()
        {
            //forwardButton.Enabled = breadCrumbEdit.CanGoForward;
            //backButton.Enabled = breadCrumbEdit.CanGoBack;
            //upButton.Enabled = breadCrumbEdit.CanGoUp;
        }
        void OnRecentButtonCheckedChanged(object sender, EventArgs e)
        {

        }
        void OnMouseEnterButton(object sender, EventArgs e)
        {
            SimpleButton button = (SimpleButton)sender;
            button.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        }
        void OnMouseLeaveButton(object sender, EventArgs e)
        {
            SimpleButton button = (SimpleButton)sender;
            button.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.navigationTreeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.LocalResourceTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.DBResourceTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.WebTabPage = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.navigationTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.LocalResourceTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationTreeList
            // 
            this.navigationTreeList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navigationTreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.navigationTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationTreeList.Location = new System.Drawing.Point(0, 0);
            this.navigationTreeList.Name = "navigationTreeList";
            this.navigationTreeList.OptionsBehavior.Editable = false;
            this.navigationTreeList.OptionsFind.AllowFindPanel = false;
            this.navigationTreeList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.navigationTreeList.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
            this.navigationTreeList.OptionsView.ShowColumns = false;
            this.navigationTreeList.OptionsView.ShowHorzLines = false;
            this.navigationTreeList.OptionsView.ShowIndentAsRowStyle = true;
            this.navigationTreeList.OptionsView.ShowIndicator = false;
            this.navigationTreeList.OptionsView.ShowVertLines = false;
            this.navigationTreeList.RowHeight = 22;
            this.navigationTreeList.SelectImageList = this.svgImageCollection1;
            this.navigationTreeList.Size = new System.Drawing.Size(317, 702);
            this.navigationTreeList.TabIndex = 1;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.FieldName = "treeListColumn1";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("electronics_desktopmac", "image://svgimages/icon builder/electronics_desktopmac.svg");
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.LocalResourceTabPage;
            this.xtraTabControl1.Size = new System.Drawing.Size(323, 731);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.LocalResourceTabPage,
            this.DBResourceTabPage,
            this.WebTabPage});
            // 
            // LocalResourceTabPage
            // 
            this.LocalResourceTabPage.Controls.Add(this.navigationTreeList);
            this.LocalResourceTabPage.Name = "LocalResourceTabPage";
            this.LocalResourceTabPage.Size = new System.Drawing.Size(317, 702);
            this.LocalResourceTabPage.Text = "本地资源";
            // 
            // DBResourceTabPage
            // 
            this.DBResourceTabPage.Name = "DBResourceTabPage";
            this.DBResourceTabPage.Size = new System.Drawing.Size(294, 271);
            this.DBResourceTabPage.Text = "远程数据库资源";
            // 
            // WebTabPage
            // 
            this.WebTabPage.Name = "WebTabPage";
            this.WebTabPage.Size = new System.Drawing.Size(0, 0);
            this.WebTabPage.Text = "Web资源";
            // 
            // ResourcePanel
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "ResourcePanel";
            this.Size = new System.Drawing.Size(323, 731);
            ((System.ComponentModel.ISupportInitialize)(this.navigationTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.LocalResourceTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage LocalResourceTabPage;
        private DevExpress.XtraTab.XtraTabPage DBResourceTabPage;
        private DevExpress.XtraTab.XtraTabPage WebTabPage;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        #endregion
        private TreeList navigationTreeList;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }

}