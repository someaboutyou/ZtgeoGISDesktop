using Abp;
using DevExpress.XtraBars;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;

namespace ZtgeoGISDesktop.Toolbars
{
    public class WinformToolbarViewManager : IWinformToolbarViewManager
    {
        private readonly IWinformToolbarManager winformToolbarManager;
        private readonly IMainForm mainForm;

        private const string toolbarDisableSuffixStr = "_DisableImage";
        public WinformToolbarViewManager(
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
                            toolbar.ToolbarEvent?.Invoke(toolbar);
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
            else if (menuStatus == MenuStatus.Disable) {
                if (toolbarGroup.ToolBars  != null && toolbarGroup.ToolBars.Count>0) { 
                    foreach(var tool in toolbarGroup.ToolBars)
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
            }
            else { 
                ((BarButtonItem)toolbar.UIObject).Enabled = false;
            }
        }
    }
}
