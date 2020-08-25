using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ToolBar;
using Ztgeo.Utils;

namespace ZtgeoGISDesktop.Toolbars
{
    public class MainToolbarProvider : ToolbarProvider
    {
        public override void SetToolbars(IToolbarDefinitionContext context)
        {
            var group = context.CreateToolbarGroup(MainFormToolbarNames.MainToolsGroup, "MainTools", null);
            group.AddToolbar(new WinformToolbar(
                MainFormToolbarNames.SaveTool,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Save16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Save16_dis.png"),
                false,
                "保存",
                group
            ));
            group.AddToolbar(new WinformToolbar(
                MainFormToolbarNames.UndoTool,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Undo16.png"),
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Undo16_dis.png"),
                    false,
                 "撤销",
                 group
            ));
            group.AddToolbar(new WinformToolbar(
                MainFormToolbarNames.RedoTool,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Redo16.png"),
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "ZtgeoGISDesktop.Icons.Redo16_dis.png"),
                    false,
                 "重做",
                 group
            ));
        }
    }

    public static class MainFormToolbarNames {
        public const string MainToolsGroup = "MainForm_MainToolGroup";
        public const string SaveTool = "MainForm_MainToolGroup_Save";
        public const string RedoTool = "MainForm_MainToolGroup_Redo";
        public const string UndoTool = "MainForm_MainToolGroup_Undo";
    }
}
