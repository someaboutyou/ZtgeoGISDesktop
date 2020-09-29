using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ToolBar;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Toolbars
{
    public class CADToolbarProvider : ToolbarProvider
    {
        public override void SetToolbars(IToolbarDefinitionContext context)
        {
            var group = context.CreateToolbarGroup(CADToolbarNames.CADToolGroup, "CAD_ToolGroup", null);
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ZoomIn,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.ZoomIn16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.ZoomIn_dis16.png"),false,"zoom in",group,null , MultiTenancySides.Host | MultiTenancySides.Tenant,
                (e) => { 
                    
                }
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ZoomOut,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.ZoomOut16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.ZoomOut_dis16.png"), false, "zoom out", group
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ShowLayerManager,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.layer16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.layer_dis16.png"), false, "layer manager", group
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.FitDrawing,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.fitDraw16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.fitdraw_dis16.png"), false, "fit drawing", group, null, MultiTenancySides.Host | MultiTenancySides.Tenant,
                (e) => {
                    e.IsActive = !e.IsActive;
                }
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.SelectionModel,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.select16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.select_dis16.png"), false, "select entities", group
            ));

        } 
    }

    public static class CADToolbarNames {
        public const string CADToolGroup = "CAD_ToolGroup";
        public const string ZoomIn = "CAD_ZoomIn";
        public const string ZoomOut = "CAD_ZoomOut";
        public const string ShowLayerManager = "CAD_ShowLayerManager"; 
        public const string FitDrawing = "CAD_FitDrawing";
        public const string SelectionModel = "CAD_SelectionModel"; 
    }
}
