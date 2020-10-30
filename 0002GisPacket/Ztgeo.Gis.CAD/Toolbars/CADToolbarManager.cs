using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.CAD.Toolbars
{
    public interface ICADToolbarManager : Abp.Dependency.ITransientDependency {
        void CADFileOpen();
        /// <summary>
        /// 设置pan 激活时，选中按钮不能是激活状态
        /// </summary>
        void SetPanActive();
        /// <summary>
        /// 设置选中按钮激活时，pan 按钮不能是激活状态
        /// </summary>
        void SetSelectorActive();
    }
    public class CADToolbarManager : ICADToolbarManager
    {
        private readonly IWinformToolbarViewManager winformToolbarViewManager;
        private readonly IWinformToolbarManager toolbarManager;
        public CADToolbarManager(IWinformToolbarViewManager _winformToolbarViewManager, IWinformToolbarManager _toolbarManager) {
            winformToolbarViewManager = _winformToolbarViewManager;
            toolbarManager = _toolbarManager;
        }
        /// <summary>
        /// CAD文件打开时的Toolbar操作
        /// </summary>
        public void CADFileOpen() {
            var toolbarGroup = toolbarManager.GetToolbarGroupOrNull(CADToolbarNames.CADToolGroup);
            if (toolbarGroup != null)
            {
                winformToolbarViewManager.ClearToolbars();
                winformToolbarViewManager.SetToolbarGroupStatus(toolbarGroup, MenuStatus.Available);
                var bars = toolbarManager.GetToolbarsByGroups(toolbarGroup);
                winformToolbarViewManager.SetToolbarsStatus(bars, MenuStatus.Available);
            }
        }

        public void SetPanActive() {
            var selectModelToolbar = toolbarManager.GetToolbarByProfixedName(CADToolbarNames.CADToolGroup + WinformToolbar.NameSplitKey + CADToolbarNames.SelectionModel);
            if (selectModelToolbar.IsActive)
                winformToolbarViewManager.SetToolbarStatus(selectModelToolbar, Winform.Menu.MenuStatus.Available);
        }

        public void SetSelectorActive() {
            var panToolbar = toolbarManager.GetToolbarByProfixedName(CADToolbarNames.CADToolGroup + WinformToolbar.NameSplitKey + CADToolbarNames.Pan);
            if(panToolbar.IsActive)
                winformToolbarViewManager.SetToolbarStatus(panToolbar, Winform.Menu.MenuStatus.Available);
        }
    }
}
