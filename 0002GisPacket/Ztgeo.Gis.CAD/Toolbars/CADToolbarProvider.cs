﻿using Abp.Dependency;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.CAD.Controls;
using Ztgeo.Gis.Winform.ABPForm;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.ToolBar;
using Ztgeo.Utils;

namespace Ztgeo.Gis.CAD.Toolbars
{
    /// <summary>
    /// CAD tool 按钮
    /// 
    /// </summary>
    public class CADToolbarProvider : ToolbarProvider
    {
        private readonly IocManager iocManager;
        private readonly IWinformToolbarViewManager winformToolbarViewManager;
        private readonly IWinformToolbarManager winformToolbarManager;
        private readonly ICADToolbarControl cadToolbarControl;

        public CADToolbarProvider(IocManager _iocmanager,
            IWinformToolbarViewManager _winformToolbarViewManager,
            IWinformToolbarManager _winformToolbarManager,
            ICADToolbarControl _cadToolbarControl
            ) {
            iocManager = _iocmanager;
            winformToolbarViewManager = _winformToolbarViewManager;
            winformToolbarManager = _winformToolbarManager;
            cadToolbarControl = _cadToolbarControl;
        }
        public override void SetToolbars(IToolbarDefinitionContext context)
        {
            var group = context.CreateToolbarGroup(CADToolbarNames.CADToolGroup, "CAD_ToolGroup", null,MultiTenancySides.Tenant|MultiTenancySides.Host, false);
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ZoomIn,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ZoomIn16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ZoomIn_dis16.png"),false,"zoom in",group,null, MultiTenancySides.Host | MultiTenancySides.Tenant, (e) => {
                    IDocument document= iocManager.Resolve<IDocumentManager>().GetActiveDocument;
                    if (document!=null && document is ICADViewDocument) {
                        ((ICADViewDocument)document).ZoomIn();
                    }
                } 
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ZoomOut,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ZoomOut16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.ZoomOut_dis16.png"), false, "zoom out", group,null, MultiTenancySides.Host | MultiTenancySides.Tenant, (e) => {
                    IDocument document = iocManager.Resolve<IDocumentManager>().GetActiveDocument;
                    if (document != null && document is ICADViewDocument)
                    {
                        ((ICADViewDocument)document).ZoomOut();
                    }
                }
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.Pan,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.pan16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.pan_dis16.png"), true, "pan", group, null, MultiTenancySides.Host | MultiTenancySides.Tenant, (e) => {
                    if (e.IsActive)
                    {  //设置其他的为非激活状态 to do
                        winformToolbarViewManager.SetToolbarStatus(e, Winform.Menu.MenuStatus.Available);
                    }
                    else
                    {
                        winformToolbarViewManager.SetToolbarStatus(e, Winform.Menu.MenuStatus.Active);
                        //与selectmodeltoolbar 排他
                        cadToolbarControl.SetPanActive();
                    }
                }
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.ShowLayerManager,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.layer16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.layer_dis16.png"), false, "layer manager", group
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.FitDrawing,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.fitDraw16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.fitdraw_dis16.png"), false, "fit drawing", group, null, MultiTenancySides.Host | MultiTenancySides.Tenant,
                (e) => {
                    IDocument document = iocManager.Resolve<IDocumentManager>().GetActiveDocument;
                    if (document is ICADViewDocument) {
                        ICADViewDocument cadViewDocument = (ICADViewDocument)document;
                        cadViewDocument.ResetScaling();
                    }
                }
            ));
            group.AddToolbar(new WinformToolbar(
                CADToolbarNames.SelectionModel,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.select16.png"),
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "Ztgeo.Gis.CAD.Icons.select_dis16.png"), false, "select entities", group,null, MultiTenancySides.Host | MultiTenancySides.Tenant,
                (e) => {
                    if (e.IsActive)
                    {  //设置其他的为非激活状态 to do
                        winformToolbarViewManager.SetToolbarStatus(e, Winform.Menu.MenuStatus.Available);
                    }
                    else
                    {
                        winformToolbarViewManager.SetToolbarStatus(e, Winform.Menu.MenuStatus.Active);
                        //与selectmodeltoolbar 排他
                        cadToolbarControl.SetSelectorActive();
                    }
                }
            ));

        } 
    }

    public static class CADToolbarNames {
        public const string CADToolGroup = "CAD_ToolGroup";
        public const string ZoomIn = "CAD_ZoomIn";
        public const string ZoomOut = "CAD_ZoomOut";
        public const string Pan = "CAD_Pan";
        public const string ShowLayerManager = "CAD_ShowLayerManager"; 
        public const string FitDrawing = "CAD_FitDrawing";
        public const string SelectionModel = "CAD_SelectionModel"; 
    }
}
