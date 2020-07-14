using Castle.MicroKernel.Util;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.Menu;

namespace ZtgeoGISDesktop.Menus
{
    public class WinformMenuViewManager : IWinformMenuViewManager
    {
        private readonly IWinformMenuManager winformMenuManager;
        private readonly IMainForm mainForm;

        public WinformMenuViewManager(IWinformMenuManager _winformMenuManager,
            IMainForm _mainForm
            ) {
            winformMenuManager = _winformMenuManager;
            mainForm = _mainForm;
        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitializeMenus()
        {
            RibbonControl menuContainer = (RibbonControl)mainForm.MenuContainerControl;
            RibbonPage defaultPage = new RibbonPage();
            menuContainer.Pages.Add(defaultPage);

            IReadOnlyList<WinformMenu> winformMenus = winformMenuManager.GetAllMenus();
            if (winformMenus!=null &&winformMenus.Count > 0) {
                // var orderSettings = this.GetMenuOrderSettings();
                addMenusPages(winformMenus, menuContainer);
            }
        }

        public IList<MenuOrderSetting> GetMenuOrderSettings() {
            return null;
        }

        private IList<WinformMenu> getOrderedMenus(IList<WinformMenu> winformMenus, IList<MenuOrderSetting> orderSettings) {
            if (orderSettings == null || orderSettings.Count == 0)
            {
                return winformMenus;
            }
            else {
                var SettedWinformMenus= winformMenus.Where(m => orderSettings.Any(orderSetting => orderSetting.MenuName.Equals(m.Name)));
                var orderedSettedWinformMenus= SettedWinformMenus.OrderBy(m => orderSettings.First(o => o.MenuName.Equals(m.Name)).Order);
                var UnSettedWinformMenus = winformMenus.Where(m => !orderSettings.Any(orderSetting => orderSetting.MenuName.Equals(m.Name))).OrderBy(m=>m.Order);
                return orderedSettedWinformMenus.Concat(UnSettedWinformMenus).ToList();
            }
        }

        /// <summary>
        /// 添加菜单页
        /// </summary>
        private void addMenusPages(IEnumerable<WinformMenu> allMenus, RibbonControl menuContainer) {
            var orderSettings= GetMenuOrderSettings();
            var pages= allMenus.Where(m => m.IsPage).ToList();
            if (pages.Count > 0) {
                IList<WinformMenu> pageMenus= getOrderedMenus(pages, orderSettings);
               foreach(WinformMenu pageMenu in pageMenus)
                {
                    RibbonPage ribbonPage = new RibbonPage {
                        Text= pageMenu.DisplayName,
                        Image = pageMenu.Icon,
                        Name = pageMenu.Name
                    };
                    addMenusGroups(pageMenu, ribbonPage, orderSettings);
                    menuContainer.Pages.Add(ribbonPage);

                }
            }
        } 
        private void addMenusGroups(WinformMenu pageMenu, RibbonPage ribbonPage, IList<MenuOrderSetting> menuOrderSettings) {
            if (pageMenu.Children!=null&& pageMenu.Children.Count>0) {
                var groupMenus = pageMenu.Children.Where(m => m.IsGroup);
                if (groupMenus != null && groupMenus.Count() > 0) {
                    groupMenus = getOrderedMenus(groupMenus.ToList(), menuOrderSettings);
                    foreach (WinformMenu groupMenu in groupMenus) {
                        RibbonPageGroup pageGroup = new RibbonPageGroup {
                            Text=groupMenu.DisplayName,
                            Name= groupMenu.Name 
                        };
                        addLinkItems(groupMenu, pageGroup, menuOrderSettings);
                        ribbonPage.Groups.Add(pageGroup);

                    }
                }
            }
        }
        private void addLinkItems(WinformMenu groupMenu, RibbonPageGroup pageGroup, IList<MenuOrderSetting> menuOrderSettings)
        {
            if (groupMenu.Children != null && groupMenu.Children.Count > 0)
            {
                var buts= getOrderedMenus(groupMenu.Children.ToList(), menuOrderSettings);
                foreach (WinformMenu button in buts) {
                    if (button.IsNavigation)
                    {
                        BarSubItem subItem = new BarSubItem
                        {
                            Name=button.Name,
                            Caption = button.DisplayName
                        };
                        if (button.Icon != null)
                            subItem.ImageOptions.LargeImage = button.Icon;
                        pageGroup.ItemLinks.Add(subItem);
                         //
                    }
                    else {
                        BarButtonItem newButton = new BarButtonItem
                        {
                            Name = button.Name,
                            Caption = button.DisplayName
                        };
                        if (button.Icon != null)
                            newButton.ImageOptions.LargeImage = button.Icon;
                        pageGroup.ItemLinks.Add(newButton);
                    }
                }
            }
        }

        private void addLinksPersistInfo(WinformMenu ButtonMenu, BarSubItem subItem, IList<MenuOrderSetting> menuOrderSettings) {
            if (ButtonMenu.Children != null && ButtonMenu.Children.Count > 0) {
                foreach (WinformMenu button in ButtonMenu.Children) {
                    if (button.IsNavigation)
                    {
                        BarSubItem subItem1 = new BarSubItem
                        {
                            Name = button.Name,
                            Caption = button.DisplayName
                        };
                        if (button.Icon != null)
                            subItem1.ImageOptions.Image = button.Icon;
                        subItem.LinksPersistInfo.Add(new LinkPersistInfo(subItem1));
                        if (button.Children != null && button.Children.Count > 0) {
                            addLinksPersistInfo(button, subItem1, menuOrderSettings);
                        }
                    }
                    else {
                        BarButtonItem buttonItem = new BarButtonItem
                        {
                            Name = button.Name,
                            Caption = button.DisplayName 
                        };
                        if (button.Icon != null)
                            buttonItem.ImageOptions.Image = button.Icon;
                        subItem.LinksPersistInfo.Add(new LinkPersistInfo(buttonItem));

                    }
                }
            }
        }

        public void RefeshMenu()
        {
             
        }
    }
}
