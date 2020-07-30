using Abp.Resources.Embedded;
using Castle.MicroKernel.Util;
using DevExpress.XtraPrinting.Native;
using System.Drawing;
using System.IO;
using System.Reflection;  
using Ztgeo.Gis.Winform.Menu;

namespace ZtgeoGISDesktop.Menus
{
    public class MainMenuProvider : MenuProvider
    {
        public MainMenuProvider() {
             
        }
        public override void SetMenus(IMenuDefinitionContext context)
        { 
            var settingPageMenu = context.CreateMenu(MainFormMenuNames.SettingPageMenu, MenuType.Page, "设置", "", null,
                GetResourceImage("ZtgeoGISDesktop.Icons.Setting.png"));
            var systemSettingGroup= settingPageMenu.CreateChildMenu(MainFormMenuNames.SystemSettingGroupMenu, MenuType.Group, "系统设置");
            var uiDesign = systemSettingGroup.CreateChildMenu(MainFormMenuNames.SystemSettingGroup_UiDesign, MenuType.Navigation, "界面设置", "", null
                , GetResourceImage("ZtgeoGISDesktop.Icons.UIDesign.png"));
        }

        private Image GetResourceImage(string name) {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (stream != null)
            {
                return Image.FromStream(stream);
            }
            else {
                return null;
            }
        } 
    }

    public static class MainFormMenuNames{
        public const string SettingPageMenu = "MainForm_SettingPage";  // for page
        public const string SystemSettingGroupMenu = "MainForm_SettingPage_SystemSettingGroup"; // for group
        public const string SystemSettingGroup_UiDesign = "MainForm_SettingPage_SystemSettingGroup_UiDesign"; // for UI

    }
}
