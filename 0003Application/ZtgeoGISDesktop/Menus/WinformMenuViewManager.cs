using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Util;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.Actions;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Utils;
using ZtgeoGISDesktop.Core.Menu;

namespace ZtgeoGISDesktop.Core.Menus
{
    /// <summary>
    /// DevExpress 中实现的WinformMenu
    /// </summary>
    public class WinformMenuViewManager : IWinformMenuViewManager
    {
        private readonly IWinformMenuManager winformMenuManager;
        private readonly IMainForm mainForm;
        private readonly IRepository<MenuOrder> menuOrderRepository;
        private readonly IocManager iocManager;

        public WinformMenuViewManager(IWinformMenuManager _winformMenuManager,
            IMainForm _mainForm,
            IRepository<MenuOrder> _menuOrderRepository,
            IocManager _iocmanager 
            ) {
            winformMenuManager = _winformMenuManager;
            mainForm = _mainForm;
            menuOrderRepository = _menuOrderRepository;
            iocManager = _iocmanager;
        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitializeMenus()
        {
            RibbonControl menuContainer = (RibbonControl)mainForm.MenuContainerControl;
             
            IReadOnlyList<WinformMenu> winformMenus = winformMenuManager.GetAllMenus();
            if (winformMenus!=null &&winformMenus.Count > 0) {
                // var orderSettings = this.GetMenuOrderSettings();
                addMenusPages(winformMenus, menuContainer);
            }
        }

        public IList<MenuOrderSetting> GetMenuOrderSettings() {
            //return null;
            var menuOrders= menuOrderRepository.GetAll().ToList<MenuOrder>(); 
            IList<MenuOrderSetting> ret = new List<MenuOrderSetting>();
            IReadOnlyList<WinformMenu> winformMenus = winformMenuManager.GetAllMenus();
            foreach (WinformMenu menu in winformMenus)
            {
                string key = string.Empty;
                string desc = string.Empty;
                GetMenuKey(menu, ref key, ref desc);
                string meunKey="";
                string menuDescription = "";
                if (!key.IsEmpty()) {
                    meunKey = key + "->" + menu.Name; 
                } 
                else {
                    meunKey = menu.Name;
                }
                if (!desc.IsEmpty())
                {
                    menuDescription = desc + "->" + menu.DisplayName;
                }
                else
                {
                    menuDescription = menu.DisplayName;
                }
                var ordered = menuOrders.FirstOrDefault(m => m.MenuKey.Equals(meunKey));
                if(ordered!=null)
                    menu.Order = ordered.Order==null ?999 : (int)ordered.Order;
                ret.Add(new MenuOrderSetting
                {
                    MenuId = menu.Name,
                    MenuName = menu.DisplayName,
                    MenuDescription = menuDescription,
                    MenuKey = meunKey,
                    ParentMenuKey = key,
                    Order = ordered == null ? null : (int?)ordered.Order
                }); ;
            }
            return ret;
        }

        private void GetMenuKey(WinformMenu gettedMenu,ref string menuKey,ref string desc) {
            if (gettedMenu.Parent != null)
            {
                if (!menuKey.IsEmpty())
                    menuKey = gettedMenu.Parent.Name + "->" + menuKey;
                else
                    menuKey = gettedMenu.Parent.Name;
                if (!desc.IsEmpty())
                    desc = gettedMenu.Parent.DisplayName + "->" + desc;
                else
                    desc = gettedMenu.Parent.DisplayName;
                GetMenuKey( gettedMenu.Parent, ref menuKey, ref desc);
            }
            //else {
            //    menuKey = gettedMenu.Name;
            //    parentMenuKey = string.Empty;
            //}
        }

        private IList<WinformMenu> getOrderedMenus(IList<WinformMenu> winformMenus, IList<MenuOrderSetting> orderSettings) {
            if (orderSettings == null || orderSettings.Count == 0)
            {
                return winformMenus;
            }
            else {
                var SettedWinformMenus= winformMenus.Where(m => orderSettings.Any(orderSetting => orderSetting.MenuName.Equals(m.Name)));
                var orderedSettedWinformMenus= SettedWinformMenus.OrderBy(m => m.Order);
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
                menuContainer.SelectPage(menuContainer.Pages[0]);
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
                        SetSubItemImage4Default(button, subItem);
                        pageGroup.ItemLinks.Add(subItem); 
                    }
                    else {
                        BarButtonItem newButton = new BarButtonItem
                        {
                            Name = button.Name,
                            Caption = button.DisplayName
                        };
                        SetSubItemImage4Default(button, newButton);
                        if (button.MenuActionType != null) {
                            newButton.ItemClick += (object sender, ItemClickEventArgs e) =>
                            {
                                IMenuAction winformAction = iocManager.Resolve(button.MenuActionType.Type) as IMenuAction;
                                winformAction.SenderMenu = button; 
                                winformAction.Excute(); 
                            }; 
                        }
                        pageGroup.ItemLinks.Add(newButton);
                    }
                }
            }
        }
        /// <summary>
        /// 设置菜单的图标
        /// </summary>
        private void SetSubItemImage4Default(WinformMenu menuDefining, BarItem subItem) {
            if (menuDefining.DefaultEnable)
            {
                if (menuDefining.Icon != null)
                {
                    if (menuDefining.Icon.Width >= 32)
                        subItem.ImageOptions.LargeImage = menuDefining.Icon;
                    else
                    {
                        subItem.ImageOptions.Image = menuDefining.Icon;
                    }
                }
            }
            else {
                if (menuDefining.DisIcon != null)
                {
                    if (menuDefining.DisIcon.Width >= 32)
                        subItem.ImageOptions.LargeImage = menuDefining.DisIcon;
                    else
                    {
                        subItem.ImageOptions.Image = menuDefining.DisIcon;
                    }
                }
                else {
                    if (menuDefining.Icon != null) {
                        if (menuDefining.Icon.Width >= 32)
                            subItem.ImageOptions.LargeImage = ImageHelp.ExColorDepth(menuDefining.Icon);
                        else
                        {
                            subItem.ImageOptions.Image = ImageHelp.ExColorDepth(menuDefining.Icon);
                        }
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
            RibbonControl menuContainer = (RibbonControl)mainForm.MenuContainerControl;
            if (menuContainer.Pages.Count > 0) {
                menuContainer.Pages.Clear();
            }
            IReadOnlyList<WinformMenu> winformMenus = winformMenuManager.GetAllMenus();
            if (winformMenus != null && winformMenus.Count > 0)
            {
                // var orderSettings = this.GetMenuOrderSettings();
                addMenusPages(winformMenus, menuContainer);
            }
        }
        /// <summary>
        /// 设置菜单状态
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menuStatus"></param>
        public void SetMenuStatus(WinformMenu menu, MenuStatus menuStatus) {
            if (menu.MenuType == MenuType.Page)
            {
                if (menu.UIObject is RibbonPage)
                {
                    RibbonPage page = (RibbonPage)menu.UIObject;
                    switch (menuStatus)
                    {
                        case MenuStatus.Available:
                            page.Visible = true;
                            break;
                        case MenuStatus.Disable: // page 设置disable。那么所有的子条目将都设置为disable 
                            page.Visible = true;
                            break;
                        case MenuStatus.Hidden:
                            page.Visible = false;
                            break;
                    }
                }
            }
            else if (menu.MenuType == MenuType.Group)
            {
                if (menu.UIObject is RibbonPageGroup)
                {
                    RibbonPageGroup group = (RibbonPageGroup)menu.UIObject;
                    switch (menuStatus)
                    {
                        case MenuStatus.Available:
                            group.Enabled = true;
                            group.Visible = true;
                            break;
                        case MenuStatus.Disable: // 
                            group.Enabled = false;
                            group.Visible = true;
                            break;
                        case MenuStatus.Hidden:
                            group.Enabled = false;
                            group.Visible = false;
                            break;
                    }
                }
            }
            else if (menu.MenuType == MenuType.Navigation)
            {
                if (menu.UIObject is BarSubItem)
                {
                    BarSubItem barSubItem = menu.UIObject as BarSubItem;
                    switch (menuStatus)
                    {
                        case MenuStatus.Available:
                            barSubItem.Enabled = true;
                            break;
                        case MenuStatus.Disable:
                            barSubItem.Enabled = false;
                            break;
                        case MenuStatus.Hidden:
                            barSubItem.Enabled = false;
                            break;
                    }
                }
                else if (menu.UIObject is BarButtonItem)
                {
                    BarButtonItem barButtonItem = menu.UIObject as BarButtonItem;
                    switch (menuStatus)
                    {
                        case MenuStatus.Available:
                            barButtonItem.Enabled = true;
                            break;
                        case MenuStatus.Disable:
                            barButtonItem.Enabled = false;
                            break;
                        case MenuStatus.Hidden:
                            barButtonItem.Enabled = false;
                            break;
                    }
                }
            } 
            if (menu.Children != null && menu.Children.Count > 0)
            {
                SetMenuStatus(menu.Children, MenuStatus.Disable);
            }
        }
        /// <summary>
        /// 设置菜单状态
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="menuStatus"></param>
        public void SetMenuStatus(IEnumerable<WinformMenu> menus, MenuStatus menuStatus) {
            foreach (var menu in menus) {
                SetMenuStatus(menu, menuStatus);
            }
        }
    }
}
