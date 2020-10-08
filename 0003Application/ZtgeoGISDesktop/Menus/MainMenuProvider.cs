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
            //filePage
            var filePageMenu = context.CreateMenu(MainFormMenuNames.FilePageMenu, MenuType.Page, "文件", "", null,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.file16.png"));
            //fileGroup
            var filePageGroup = filePageMenu.CreateChildMenu(MainFormMenuNames.FilePageGroupMenu, MenuType.Group, "文件");
            var fileNew = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FileNew, MenuType.Button, "新建", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.New.png"));
            var fileOpen = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FileOpen, MenuType.Button, "打开", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Open.png"));
            var fileSave = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FileSave, MenuType.Button, "保存", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Save.png"));
            var fileSaveAs = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FileSaveAs, MenuType.Button, "另存为", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.SaveAs.png"));
            var fileQuickPrint = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FileQuickPrint, MenuType.Button, "快速打印", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.QuickPrint.png"));
            var filePrint = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FilePrint, MenuType.Button, "打印", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Print.png"));
            var filePrintPreview = filePageGroup.CreateChildMenu(MainFormMenuNames.FilePageGroup_FilePrintPreview, MenuType.Button, "打印预览", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.PrintPreview32.png"),
               AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.PrintPreview_dis32.png")
               );
            //document info
            var documentInfoGroup = filePageMenu.CreateChildMenu(MainFormMenuNames.FilePageDocumentInfoGroupMenu, MenuType.Group, "文档");
            var documentInfo = documentInfoGroup.CreateChildMenu(MainFormMenuNames.FilePageDocumentInfoGroup_Document, MenuType.Button, "文档信息", "", null
               , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.DocumentInfo.png"));
            //lecense info
            var LicenseInfoGroup = filePageMenu.CreateChildMenu(MainFormMenuNames.LicenseGroupMenu, MenuType.Group, "授权");
            var licenseInfo = LicenseInfoGroup.CreateChildMenu(MainFormMenuNames.LicenseGroup_LicenseInfo, MenuType.Button, "授权信息", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.LicenseInfo.png"));
            var powerBy = LicenseInfoGroup.CreateChildMenu(MainFormMenuNames.LicenseGroup_PowerBy, MenuType.Button, "谴责声明", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.PowerBy.png"));
            //help
            var helpGroup = filePageMenu.CreateChildMenu(MainFormMenuNames.HelpGroupMenu, MenuType.Group, "帮助");
            var about= helpGroup.CreateChildMenu(MainFormMenuNames.HelpGroup_About, MenuType.Button, "关于", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.About.png"));
            var update = helpGroup.CreateChildMenu(MainFormMenuNames.HelpGroup_Update, MenuType.Button, "更新检查", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Update.png"));
            var helpDown = helpGroup.CreateChildMenu(MainFormMenuNames.HelpGroup_HelpDown, MenuType.Button, "下载帮助", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.HelpDown.png"));
            var feedBack = helpGroup.CreateChildMenu(MainFormMenuNames.HelpGroup_FeedBack, MenuType.Button, "反馈", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.FeedBack.png"));
            //setting page
            var settingPageMenu = context.CreateMenu(MainFormMenuNames.SettingPageMenu, MenuType.Page, "设置", "", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(),"ZtgeoGISDesktop.Icons.Setting.png"));
            //systemSettingGroup
            var systemSettingGroup= settingPageMenu.CreateChildMenu(MainFormMenuNames.SystemSettingGroupMenu, MenuType.Group, "系统设置");
            var uiDesign = systemSettingGroup.CreateChildMenu(MainFormMenuNames.SystemSettingGroup_UiDesign, MenuType.Button, "界面设置", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.UIDesign.png")); 
            var menuOrder = systemSettingGroup.CreateChildMenu(MainFormMenuNames.SystemSettingGroup_MenuOrderSetting, MenuType.Button, "菜单设置", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.MenuOrderSetting.png")
                //menuEvent: m =>
                //{
                //    DialogHybirdForm<MenuSettingControl> dialog = new DialogHybirdForm<MenuSettingControl>(iocManager, typeof(ZtgeoGISDesktopMoudle).Assembly, new string[] {
                //        "WebViews","MenuSetting","index.html"
                //    }); 
                //    dialog.Size = new Size(1260, 560);
                //    dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                //    dialog.StartPosition = FormStartPosition.CenterScreen;
                //    dialog.ShowDialog();
                //}
                ); 
        } 
    }

    public static class MainFormMenuNames{
        public const string FilePageMenu = "MainForm_FilePage";
        //file group
        public const string FilePageGroupMenu = "MainForm_FilePageGroup";
        public const string FilePageGroup_FileNew = "MainForm_FilePageGroup_FileNew"; // 新建
        public const string FilePageGroup_FileOpen = "MainForm_FilePageGroup_FileOpen"; // 打开
        public const string FilePageGroup_FileSave = "MainForm_FilePageGroup_FileSave"; // 保存
        public const string FilePageGroup_FileSaveAs = "MainForm_FilePageGroup_FileSaveAs"; // 另存为 
        public const string FilePageGroup_FileQuickPrint = "MainForm_FilePageGroup_FileQuickPrint"; // 快速打印
        public const string FilePageGroup_FilePrint = "MainForm_FilePageGroup_FilePrint"; // 快速打印
        public const string FilePageGroup_FilePrintPreview = "MainForm_FilePageGroup_FilePrintPreview"; // 打印预览
        //document info
        public const string FilePageDocumentInfoGroupMenu = "MainForm_DocumentInfoGroup";
        public const string FilePageDocumentInfoGroup_Document = "MainForm_DocumentInfoGroup_Document"; //文档信息
        //public const string FilePageDocumentInfoGroup_Document = "MainForm_DocumentInfoGroup_Document"; //文档信息
        public const string LicenseGroupMenu = "MainForm_LicenseGroup";
        public const string LicenseGroup_LicenseInfo = "MainForm_LicenseGroup_LicenseInfo"; //授权信息
        public const string LicenseGroup_PowerBy = "MainForm_LicenseGroup_PowerBy"; // 授权说明

        public const string HelpGroupMenu = "MainForm_HelpGroup";
        public const string HelpGroup_About = "MainForm_Help_About"; //关于
        public const string HelpGroup_Update = "MainForm_Help_Update"; //更新
        public const string HelpGroup_HelpDown = "MainForm_Help_HelpDown"; //更新
        public const string HelpGroup_FeedBack = "MainForm_Help_FeedBack"; //反馈

        public const string SettingPageMenu = "MainForm_SettingPage";  // for page
        public const string SystemSettingGroupMenu = "MainForm_SettingPage_SystemSettingGroup"; // for group
        public const string SystemSettingGroup_UiDesign = "MainForm_SettingPage_SystemSettingGroup_UiDesign"; // for UI
        public const string SystemSettingGroup_Backend = "MainForm_SettingPage_SystemSettingGroup_Backend"; // for UI
        public const string SystemSettingGroup_MenuOrderSetting = "MainForm_SettingPage_SystemSettingGroup_MenuOrderSetting"; // Menu Order

    }
}
