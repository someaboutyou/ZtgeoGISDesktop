using Abp;
using Abp.Dependency;
using DevExpress.XtraBars;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;

namespace ZtgeoGISDesktop.Toolbars
{
    public class WinformToolbarViewManager : IWinformToolbarViewManager
    {
        private readonly IWinformToolbarManager winformToolbarManager;
        private readonly IMainForm mainForm;
        private readonly IocManager iocManager;
        private const string toolbarDisableSuffixStr = "_DisableImage";
        public WinformToolbarViewManager(
            IocManager _iocManager,
            IWinformToolbarManager _winformToolbarManager,
            IMainForm _mainForm
            ) {
            winformToolbarManager = _winformToolbarManager;
            mainForm = _mainForm;
        }
        public void InitialzeToolbars()
        {
            BarManager barManager = mainForm.ToolBarManager as BarManager;
            if (barManager != null) {
                ((System.ComponentModel.ISupportInitialize)(barManager)).BeginInit();
                var groups= winformToolbarManager.GetAllToolbarGroups();
                var standaloneBarDockControl = mainForm.StandaloneBarDockControl as StandaloneBarDockControl;
                ImageList imageList = new ImageList();
                barManager.Images = imageList;
                foreach (var group in groups)
                {
                    var newBar = CreateBarByGroup(group, standaloneBarDockControl);
                    IList<WinformToolbar> toolbars=  winformToolbarManager.GetToolbarsByGroups(group);
                    foreach (var toolbar in toolbars) {
                        imageList.Images.Add(toolbar.PrefixedName,toolbar.Icon);
                        
                        BarButtonItem buttonItem = new BarButtonItem();
                        buttonItem.Caption = toolbar.Tip;
                        buttonItem.Name = toolbar.Name; 
                        buttonItem.ImageIndex = imageList.Images.IndexOfKey(toolbar.PrefixedName);
                        if (toolbar.DisableIcon != null)
                        {
                            imageList.Images.Add(toolbar.PrefixedName + toolbarDisableSuffixStr, toolbar.DisableIcon);
                            buttonItem.ImageIndexDisabled = imageList.Images.IndexOfKey(toolbar.PrefixedName + toolbarDisableSuffixStr);
                        }
                        toolbar.UIObject = buttonItem; 
                        buttonItem.Enabled = toolbar.DefaultEnable; 
                        buttonItem.ItemClick += (sender, e) => {
                            //toolbar.ToolbarEvent?.Invoke(toolbar); 
                            if (toolbar.ToolbarAction != null) {
                                var toolActiono= iocManager.Resolve(toolbar.ToolbarAction);
                                if (toolActiono is IToolbarAction) {
                                    var toolAction = (IToolbarAction)toolActiono;
                                    toolAction.WinformToolbar = toolbar;
                                    toolAction.Excute();
                                }
                            }
                        }; 
                        barManager.Items.Add(buttonItem);
                        newBar.LinksPersistInfo.Add(new LinkPersistInfo(buttonItem));
                    } 
                    barManager.Bars.Add(newBar);
                }
                ((System.ComponentModel.ISupportInitialize)(barManager)).EndInit();
            }
            else
            {
                throw new AbpException("No Bar Manager was found");
            }
        }

        private Bar CreateBarByGroup(WinformToolbarGroup group, StandaloneBarDockControl standaloneBarDockControl) {
            Bar bar = new Bar();
            bar.BarName = group.Name;
            bar.DockCol = 0;
            bar.DockRow = 0;
            bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            bar.StandaloneBarDockControl = standaloneBarDockControl;
            bar.Text = group.Text;
            group.UIObject = bar;
            if (group.IsDefaultShow)
            {
                bar.Visible = true;
            }
            else {
                bar.Visible = false;
            }
            return bar;
        }

        //public void RefeshToolbar()
        //{
        //    throw new NotImplementedException();
        //}

        //public void RefeshToolbar(string groupName)
        //{
        //    IList<WinformToolbar> toolbars= winformToolbarManager.GetToolbarsByGroups(groupName);
        //    if (toolbars != null && toolbars.Count > 0) {
                
        //    }
        //}

        public void SetToolbarGroupStatus(WinformToolbarGroup toolbarGroup, MenuStatus menuStatus)
        {
            if (menuStatus == MenuStatus.Available)
            {
                ((Bar)(toolbarGroup.UIObject)).Visible = true;
            }
            else if (menuStatus == MenuStatus.Hidden)
            {
                ((Bar)(toolbarGroup.UIObject)).Visible = false;
            }
            else if (menuStatus == MenuStatus.Disable)
            {
                if (toolbarGroup.ToolBars != null && toolbarGroup.ToolBars.Count > 0)
                {
                    foreach (var tool in toolbarGroup.ToolBars)
                    {
                        ((BarButtonItem)tool.UIObject).Enabled = false;
                    }
                }
            }
        }

        public void SetToolbarStatus(WinformToolbar toolbar, MenuStatus menuStatus)
        {
            if (menuStatus == MenuStatus.Available)
            {
                ((BarButtonItem)toolbar.UIObject).Enabled = true;
                ((BarButtonItem)toolbar.UIObject).ButtonStyle = BarButtonStyle.Default;
                ((BarButtonItem)toolbar.UIObject).Down = false;
                ((BarButtonItem)toolbar.UIObject).Visibility = BarItemVisibility.Always;
                toolbar.IsActive = false;
            }
            else if (menuStatus == MenuStatus.Active)
            {
                 
                ((BarButtonItem)toolbar.UIObject).Enabled = true;
                ((BarButtonItem)toolbar.UIObject).ButtonStyle = BarButtonStyle.Check;
                ((BarButtonItem)toolbar.UIObject).Down = true;
                ((BarButtonItem)toolbar.UIObject).Visibility = BarItemVisibility.Always;
                toolbar.IsActive = true;
            }
            else if (menuStatus == MenuStatus.Hidden) {
                ((BarButtonItem)toolbar.UIObject).Visibility = BarItemVisibility.Never;
                toolbar.IsActive = false;
            }
            else if (menuStatus == MenuStatus.Disable)
            {
                ((BarButtonItem)toolbar.UIObject).Enabled = false;
                ((BarButtonItem)toolbar.UIObject).ButtonStyle = BarButtonStyle.Default;
                ((BarButtonItem)toolbar.UIObject).Visibility = BarItemVisibility.Always;
                toolbar.IsActive = false;
            }
        }

        public void SetToolbarsStatus(IList<WinformToolbar> toolbars, MenuStatus menuStatus)
        {
            foreach (WinformToolbar tb in toolbars) {
                SetToolbarStatus(tb, menuStatus);
            }
        }

        public void ClearToolbars() {
            var groups = winformToolbarManager.GetAllToolbarGroups();
            if (groups != null && groups.Count > 0) {
                foreach (var group in groups) {
                    this.SetToolbarGroupStatus(group, MenuStatus.Hidden);
                }
            }
        }
    }
}
