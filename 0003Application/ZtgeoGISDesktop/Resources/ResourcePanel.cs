using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Abp.Collections;
using Abp.Dependency;
using Abp.Events.Bus.Factories;
using Abp.Reflection;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.Resources;
using Ztgeo.Utils;

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
            InitializeCheckResourceComboBox();
        }
        #region localresource author JZW
        /// <summary>
        /// 初始化本地资源树
        /// </summary>
        private void InitializeLocalResourceTreeList()
        {
            navigationTreeList.VirtualTreeGetChildNodes += OnNavigationTreeListGetChildNodes;
            navigationTreeList.BeforeExpand += OnNavigationTreeNodeBeforeExpand;
            navigationTreeList.VirtualTreeGetCellValue += OnNavigationTreeListGetCellValue;
            navigationTreeList.FocusedNodeChanged += OnNavigationTreeListFocusedNodeChanged;
            navigationTreeList.CustomDrawNodeImages += OnTreeListCustomDrawNodeImages;
            navigationTreeList.GetSelectImage += OnTreeListGetStateImage;
            navigationTreeList.PopupMenuShowing += OnTreeListPopMenuShow;
            navigationTreeList.MouseDoubleClick += OnTreeListDoubleClick;
            navigationTreeList.DataSource = new RootItem(); 
            navigationTreeList.ForceInitialize();
            //navigationTreeList.Nodes[0].Expand();
            //if (navigationTreeList.Nodes[0].Nodes.Count > 2)
            //{
            //    navigationTreeList.Nodes[0].Nodes[1].Expand();
            //    navigationTreeList.FocusedNode = navigationTreeList.Nodes[0].Nodes[1];
            //}
            //else
            //    navigationTreeList.FocusedNode = navigationTreeList.Nodes[0];
        }
        void OnNavigationTreeListGetCellValue(object sender, VirtualTreeGetCellValueInfo e)
        {
            e.CellData = ((Item)e.Node).DisplayName;
        }
        void OnNavigationTreeListGetChildNodes(object sender, VirtualTreeGetChildNodesInfo e)
        { 
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if(e.Children==null)
                e.Children = ((Item)e.Node).GetDirectories(checkedResourceMetaData);
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
        void OnNavigationTreeNodeBeforeExpand(object sender, BeforeExpandEventArgs e) {
            
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
        /// <summary>
        /// 资源树的右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTreeListPopMenuShow(object sender, PopupMenuShowingEventArgs e) {
            TreeList tree = sender as TreeList;
            TreeListHitInfo treeListHitInfo = tree.CalcHitInfo(e.Point);
            if (treeListHitInfo.HitInfoType == HitInfoType.Cell)
            {
                tree.SetFocusedNode(treeListHitInfo.Node);
            }
            if (tree.FocusedNode != null)
            {
                var item =navigationTreeList.GetRow(tree.FocusedNode.Id);
                if (item is ResourceItem)
                {
                    ResourceItem _item = item as ResourceItem;
                    popupMenu1.ClearLinks();
                    IResource resource = _item.Resource;
                    if (resource.ResourceMetaData != null
                        && resource.ResourceMetaData.ContextActionTypes != null
                        && resource.ResourceMetaData.ContextActionTypes.Count > 0)
                    {
                        foreach (Type itemActionType in resource.ResourceMetaData.ContextActionTypes)
                        {
                            IContextMenuItemAction contextMenuItemAction = IocManager.Instance.Resolve(itemActionType) as IContextMenuItemAction;
                            if (contextMenuItemAction.IsSplitor)
                            {
                                var link = popupMenu1.AddItem(new BarLargeButtonItem());
                                link.BeginGroup = true;
                            }
                            else
                            {

                                contextMenuItemAction.Sender = resource;
                                var buttonItem = new DevExpress.XtraBars.BarButtonItem(this.barManager1, contextMenuItemAction.Caption);
                                buttonItem.ImageOptions.Image = contextMenuItemAction.ItemIcon;
                                buttonItem.ItemClick += (itemSender, ee) =>
                                {
                                    contextMenuItemAction.Excute();
                                };

                                popupMenu1.AddItem(
                                       buttonItem
                                    );
                            }

                        }
                        Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                        popupMenu1.ShowPopup(p);
                    }
                }
                else if (item is DirectoryItem)
                {
                    popupMenu1.ClearLinks();
                    var barbutton = new DevExpress.XtraBars.BarButtonItem(this.barManager1, "打开资源文件目录");
                    barbutton.ImageOptions.Image = AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.ResourceManager16.png");
                    barbutton.ItemClick += (itemSender, ee) =>
                     {
                         System.Diagnostics.Process.Start("explorer.exe", ((DirectoryItem)item).FullName);
                     };
                    var barbutton2 = new DevExpress.XtraBars.BarButtonItem(this.barManager1, "刷新");
                    barbutton2.ImageOptions.Image = AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Refesh16.png");
                    barbutton2.ItemClick += (itemSender, ee) =>
                    {
                        //tree.FocusedNode.Collapse(); 
                        //tree.FocusedNode.HasChildren = true;
                        //tree.RefreshNode(tree.FocusedNode);
                        tree.RefreshDataSource();
                    };
                    popupMenu1.AddItem(barbutton); popupMenu1.AddItem(barbutton2);
                    Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                    popupMenu1.ShowPopup(p);
                }
                else if (item is FileItem) { 
                    
                } 
            }
        }
        /// <summary>
        /// 资源树的双击事件
        /// </summary>
        void OnTreeListDoubleClick(object sender, MouseEventArgs e) {
            TreeList tree = sender as TreeList;
            TreeListHitInfo treeListHitInfo = tree.CalcHitInfo(e.Location);
            if (treeListHitInfo.HitInfoType == HitInfoType.Cell)
            {
                tree.SetFocusedNode(treeListHitInfo.Node);
            }
            if (tree.FocusedNode != null)
            {
                var item = navigationTreeList.GetRow(tree.FocusedNode.Id);
                if (item is ResourceItem)
                {
                    ResourceItem _item = item as ResourceItem;
                    IResource resource = _item.Resource;
                    if (resource.ResourceMetaData != null
                        && resource.ResourceMetaData.DoubleClickResourceActionType != null)
                    {
                        IType<IResourceAction> resourceActionType = resource.ResourceMetaData.DoubleClickResourceActionType;
                        IResourceAction resourceAction = IocManager.Instance.Resolve(resourceActionType.Type) as IResourceAction;
                        resourceAction.Resource = resource;
                        resourceAction.Excute();
                    }
                }
                else if (item is FileItem) {
                    IResourceMetaDataProvider resourceMetaDataProvider = IocManager.Instance.Resolve<IResourceMetaDataProvider>();
                    ITypeList<IDocumentResourceMetaData> resourceMetaDatas = resourceMetaDataProvider.DocumentResourceMetaDataProviders;
                    if (resourceMetaDatas.Count > 0) {
                        foreach (var mt in resourceMetaDatas) {
                            var m = IocManager.Instance.Resolve(mt);
                            if (m is ISingleFileDocumentResourceMetaData) {
                                if (((ISingleFileDocumentResourceMetaData)m).Identified(((FileItem)item).FullName)){
                                    ISingleFileDocumentResource singleFileDocumentResource = IocManager.Instance.Resolve(((ISingleFileDocumentResourceMetaData)m).ResourceType.Type) as ISingleFileDocumentResource;
                                    singleFileDocumentResource.FullName = ((FileItem)item).FullName;
                                    singleFileDocumentResource.Open();
                                }
                            }
                        }
                    }
                }
            }
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

        #region resource checker author JZW
        /// <summary>
        /// 初始化可选择列表
        /// </summary>
        private void InitializeCheckResourceComboBox()
        {
            CheckedResourceCombox.EditValueChanged += (sender, e) =>
            {
                GetCheckedResourceMetaData(); 
                navigationTreeList.RefreshDataSource(); 
            };
            IResourceMetaDataProvider resourceMetaDataProvider = IocManager.Instance.Resolve<IResourceMetaDataProvider>();
            ITypeList<IResourceMetaData> resourceMetaDatas = resourceMetaDataProvider.AllResourceMetaDataProviders;
            if (resourceMetaDatas.Count > 0) {
                foreach (Type t in resourceMetaDatas) {
                    IResourceMetaData metaData = IocManager.Instance.Resolve(t) as IResourceMetaData;
                    CheckedResourceCombox.Properties.Items.Add(metaData,metaData.Name, CheckState.Unchecked, true);
                }
            } 
        }

        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private IList<IResourceMetaData> checkedResourceMetaData;
        /// <summary>
        /// 获得选中的MetaData
        /// </summary>
        /// <returns></returns>
        private void GetCheckedResourceMetaData() {
            var checkedItemString = CheckedResourceCombox.EditValue as string;
            if (string.IsNullOrEmpty(checkedItemString))
            {
                checkedResourceMetaData = null;
            }
            else
            {
                string[] checkTypes = checkedItemString.Split(',');
                ITypeFinder _typeFinder = IocManager.Instance.Resolve<ITypeFinder>();
                checkedResourceMetaData = new List<IResourceMetaData>();
                foreach (string typeName in checkTypes) {
                    if (!string.IsNullOrEmpty(typeName)) { 
                        var typess = _typeFinder.Find(t => t.FullName.Equals(typeName.Trim()));
                        if (typess.Length > 0) {
                            checkedResourceMetaData.Add(IocManager.Instance.Resolve(typess[0]) as IResourceMetaData);
                        }
                    }
                } 
            }
        }
         
        #endregion

        #region resource Panel component ,author JZW
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.navigationTreeList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.LocalResourceTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.CheckedResourceCombox = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.DBResourceTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.WebTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.navigationTreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.LocalResourceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheckedResourceCombox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // navigationTreeList
            // 
            this.navigationTreeList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navigationTreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.navigationTreeList.Location = new System.Drawing.Point(2, 26);
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
            this.navigationTreeList.Size = new System.Drawing.Size(313, 674);
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
            this.LocalResourceTabPage.Controls.Add(this.layoutControl1);
            this.LocalResourceTabPage.Name = "LocalResourceTabPage";
            this.LocalResourceTabPage.Size = new System.Drawing.Size(317, 702);
            this.LocalResourceTabPage.Text = "本地资源";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.CheckedResourceCombox);
            this.layoutControl1.Controls.Add(this.navigationTreeList);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(317, 702);
            this.layoutControl1.TabIndex = 3;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // CheckedResourceCombox
            // 
            this.CheckedResourceCombox.EditValue = "";
            this.CheckedResourceCombox.Location = new System.Drawing.Point(2, 2);
            this.CheckedResourceCombox.Name = "CheckedResourceCombox";
            this.CheckedResourceCombox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CheckedResourceCombox.Size = new System.Drawing.Size(313, 20);
            this.CheckedResourceCombox.StyleController = this.layoutControl1;
            this.CheckedResourceCombox.TabIndex = 2;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(317, 702);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.navigationTreeList;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(317, 678);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.CheckedResourceCombox;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(317, 24);
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // DBResourceTabPage
            // 
            this.DBResourceTabPage.Name = "DBResourceTabPage";
            this.DBResourceTabPage.Size = new System.Drawing.Size(317, 702);
            this.DBResourceTabPage.Text = "数据库资源";
            // 
            // WebTabPage
            // 
            this.WebTabPage.Name = "WebTabPage";
            this.WebTabPage.Size = new System.Drawing.Size(317, 702);
            this.WebTabPage.Text = "Web资源";
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barStaticItem1});
            this.barManager1.MaxItemId = 4;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(323, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 731);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(323, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 731);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(323, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 731);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 3;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // ResourcePanel
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ResourcePanel";
            this.Size = new System.Drawing.Size(323, 731);
            ((System.ComponentModel.ISupportInitialize)(this.navigationTreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.LocalResourceTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CheckedResourceCombox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage LocalResourceTabPage;
        private DevExpress.XtraTab.XtraTabPage DBResourceTabPage;
        private DevExpress.XtraTab.XtraTabPage WebTabPage;
        private CheckedComboBoxEdit CheckedResourceCombox;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;

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
        #endregion
    }

}