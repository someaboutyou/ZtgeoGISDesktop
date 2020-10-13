using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Actions;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.CAD.Toolbars;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Menus
{
    public class CadBaseMenuProvider : MenuProvider
    {
        private IocManager iocManager;

        public CadBaseMenuProvider(IocManager _iocManager) {
            this.iocManager = _iocManager;
        }

        public override void SetMenus(IMenuDefinitionContext context)
        {
            var cadPageMenu = context.CreateMenu(CadBaseMenusNames.CadPageMenu, MenuType.Page, "CAD", "", null,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.cad16.png"));
            var fileGroupMenu= cadPageMenu.CreateChildMenu(CadBaseMenusNames.CadFileGroupMenu, MenuType.Group, "文件");
            var openFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.OpenCadFileMenu, MenuType.Button, "文件打开", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OpenCad32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OpenCad_Dis32.png")
                , menuActionType:typeof(OpenCadFileAction)
                //,menuEvent: m =>
                //{
                //    IDocumentManager documentManager = iocManager.Resolve<IDocumentManager>();
                //    ICADToolbarControl toolbarControl = iocManager.Resolve<ICADToolbarControl>();
                //    MenuActions.OpenCADFile(documentManager, toolbarControl);
                //}
                );
            var openWebFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.OpenCadFromWebMenu, MenuType.Button, "Web打开", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.WebFile32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.WebFile_dis32.png")
                //,menuEvent: m =>
                //{
                //    IDocumentManager documentManager = iocManager.Resolve<IDocumentManager>();
                //    ICADToolbarControl toolbarControl = iocManager.Resolve<ICADToolbarControl>();
                //    MenuActions.OpenCADFile(documentManager, toolbarControl);
                //}
                );
            var saveFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.SaveCadFileMenu, MenuType.Button, "保存", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.Savecad32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.Savecad_Dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
            var saveDxfFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.SaveCadFileAsDxfMenu, MenuType.Button, "保存为DXF", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.SaveDxf32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.SaveDxf_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );

            var quickPrint = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.QuickPrintMenu, MenuType.Button, "快速打印", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.QuickPrint32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.QuickPrint_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                ); 
            var print = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.PrintMenu, MenuType.Button, "打印", "", null
                 , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.Print32.png")
                 , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.Print_dis32.png")
                 //, menuEvent: m =>
                 //{

                 //}
                 );
            var printPreview = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.PrintPreviewMenu, MenuType.Button, "打印预览", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.PrintPreview32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.PrintPreview_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );

            var outputGroupMenu = cadPageMenu.CreateChildMenu(CadBaseMenusNames.CadFileOutPutGroupMenu, MenuType.Group, "文件导出");
            var outputIamgeFile = outputGroupMenu.CreateChildMenu(CadBaseMenusNames.CadFileOutPutCad2ImageMenu, MenuType.Button, "导出图片", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OutputCAD2Img32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OutputCAD2Img_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
            var outputEmfFile = outputGroupMenu.CreateChildMenu(CadBaseMenusNames.CadFileOutPutCad2EmfMenu, MenuType.Button, "导出Emf", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OutputCAD2Emf32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OutputCAD2Emf_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
            var fileOperateGroupMenu = cadPageMenu.CreateChildMenu(CadBaseMenusNames.CadFileOperateGroupMenu, MenuType.Group, "文件操作");
            var mergeFile = fileOperateGroupMenu.CreateChildMenu(CadBaseMenusNames.CadFileOperate_MergeMenu, MenuType.Button, "文件合并", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.FileMerge32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.FileMerge_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
            var settingGroupMenu = cadPageMenu.CreateChildMenu(CadBaseMenusNames.CadFileViewSettingGroupMenu, MenuType.Group, "设置");
            var shxFontMenu = settingGroupMenu.CreateChildMenu(CadBaseMenusNames.CadFileShxFontsSettingMenu, MenuType.Button, "Shx Fonts", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ShxFont32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ShxFont_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
            var backgroundMenu = settingGroupMenu.CreateChildMenu(CadBaseMenusNames.CadFileBackgroudColorSettingMenu, MenuType.Button, "背景设置", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.background32.png")
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.background_dis32.png")
                //, menuEvent: m =>
                //{

                //}
                );
        }
    }

    public static class CadBaseMenusNames {
        public const string CadPageMenu = "MainForm_CadPage";
        public const string CadFileGroupMenu = "MainForm_CadPage_FileGroup"; // cad 文件
        public const string OpenCadFileMenu = "MainForm_CadPage_FileGroup_OpenFile"; // cad 文件打开
        public const string OpenCadFromWebMenu = "MainForm_CadPage_FileGroup_OpenFromWebFile";// 从Web 打开CAD文件
        public const string SaveCadFileMenu = "MainForm_CadPage_FileGroup_SaveFile"; // 保存文件
        public const string SaveCadFileAsDxfMenu = "MainForm_CadPage_FileGroup_SaveFileAsDxf"; // 保存文件为 dxf
        public const string QuickPrintMenu = "MainForm_CadPage_FileGroup_QuickPrintFile";// 快速打印文件
        public const string PrintMenu = "MainForm_CadPage_FileGroup_PrintFile"; // 打印文件
        public const string PrintPreviewMenu = "MainForm_CadPage_FileGroup_PrintPreviewFile"; // 打印预览


        public const string CadFileOutPutGroupMenu = "MainForm_CadPage_OutputGroup"; // cad 文件导出
        public const string CadFileOutPutCad2ImageMenu = "MainForm_CadPage_FileGroup_OutPutCad2Image"; // 导出图片
        public const string CadFileOutPutCad2EmfMenu = "MainForm_CadPage_FileGroup_OutPutCad2Emf"; // 导出emf

        public const string CadFileOperateGroupMenu = "MainForm_CadPage_FileOperateGroup"; // 文件操作
        public const string CadFileOperate_MergeMenu = "MainForm_CadPage_FileOperate_Merge"; // CAD 文件合并

        public const string CadFileViewSettingGroupMenu = "MainForm_CadPage_ViewSettingGroup"; // CAD文件设置
        public const string CadFileShxFontsSettingMenu = "MainForm_CadPage_ShxFontsSetting"; // shx fonts
        public const string CadFileBackgroudColorSettingMenu = "MainForm_CadPage_BackgroudColorSetting"; // 背景设置
    }
}
