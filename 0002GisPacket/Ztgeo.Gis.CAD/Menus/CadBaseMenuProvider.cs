using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.Menu;
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
            var openFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.OpenCadFileMenu, MenuType.Button, "CAD打开", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OpenCad32.png"), menuEvent: m =>
                {
                    IDocumentManager documentManager = iocManager.Resolve<IDocumentManager>();
                    MenuActions.OpenCADFile(documentManager);
                });
            var saveFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.SaveCadFileMenu, MenuType.Button, "保存", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.Savecad32.png"), menuEvent: m =>
                {

                });
            var outputFile = fileGroupMenu.CreateChildMenu(CadBaseMenusNames.OutPutCad2ImageMenu, MenuType.Button, "导出图片", "", null
                , AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.OutputCAD2Img32.png"), menuEvent: m =>
                {

                }); 
        }
    }

    public static class CadBaseMenusNames {
        public const string CadPageMenu = "MainForm_CadPage";
        public const string CadFileGroupMenu = "MainForm_CadPage_FileGroup";
        public const string OpenCadFileMenu = "MainForm_CadPage_FileGroup_OpenFile";
        public const string SaveCadFileMenu = "MainForm_CadPage_FileGroup_SaveFile";
        public const string OutPutCad2ImageMenu = "MainForm_CadPage_FileGroup_OutPutCad2Image";

    }
}
