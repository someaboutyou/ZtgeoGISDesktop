using Abp.Dependency;
using Abp.Resources.Embedded;
using Castle.MicroKernel.Util;
using DevExpress.XtraPrinting.Native;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Utils;
using ZtgeoGISDesktop.Forms;
using ZtgeoGISDesktop.Winform.Share;
using ZtgeoGISDesktop.Winform.Share.Forms;

namespace ZtgeoGISDesktop.Menus
{
    public class MainMenuProvider : MenuProvider
    {
        private IocManager iocManager;
        public MainMenuProvider(IocManager _iocManager) {
            this.iocManager = _iocManager;
        }
        public override void SetMenus(IMenuDefinitionContext context)
        { 
            var settingPageMenu = context.CreateMenu(MainFormMenuNames.SettingPageMenu, MenuType.Page, "设置", "", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(),"ZtgeoGISDesktop.Icons.Setting.png"));
            var systemSettingGroup= settingPageMenu.CreateChildMenu(MainFormMenuNames.SystemSettingGroupMenu, MenuType.Group, "系统设置");
            var uiDesign = systemSettingGroup.CreateChildMenu(MainFormMenuNames.SystemSettingGroup_UiDesign, MenuType.Button, "界面设置", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.UIDesign.png"),menuEvent:m=> { 
                    
                }); 
            var menuOrder = systemSettingGroup.CreateChildMenu(MainFormMenuNames.SystemSettingGroup_MenuOrderSetting, MenuType.Button, "菜单设置", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.MenuOrderSetting.png"), menuEvent: m =>
                {
                    DialogHybirdForm<MenuSettingControl> dialog = new DialogHybirdForm<MenuSettingControl>(iocManager, typeof(ZtgeoGISDesktopMoudle).Assembly, new string[] {
                        "WebViews","MenuSetting","index.html"
                    }); 
                    dialog.Size = new Size(1260, 560);
                    dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    dialog.StartPosition = FormStartPosition.CenterScreen;
                    dialog.ShowDialog();
                });

            var filePageMenu = context.CreateMenu(MainFormMenuNames.FilePageMenu, MenuType.Page, "文件", "", null,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.file16.png"));
        } 
    }

    public static class MainFormMenuNames{
        public const string FilePageMenu = "MainForm_FilePage";

        public const string SettingPageMenu = "MainForm_SettingPage";  // for page
        public const string SystemSettingGroupMenu = "MainForm_SettingPage_SystemSettingGroup"; // for group
        public const string SystemSettingGroup_UiDesign = "MainForm_SettingPage_SystemSettingGroup_UiDesign"; // for UI
        public const string SystemSettingGroup_Backend = "MainForm_SettingPage_SystemSettingGroup_Backend"; // for UI
        public const string SystemSettingGroup_MenuOrderSetting = "MainForm_SettingPage_SystemSettingGroup_MenuOrderSetting"; // Menu Order

    }
}
