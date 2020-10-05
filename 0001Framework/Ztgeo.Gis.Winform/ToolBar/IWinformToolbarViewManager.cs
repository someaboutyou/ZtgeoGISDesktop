using Abp.Application.Services;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public interface IWinformToolbarViewManager : ITransientDependency
    {
        /// <summary>
        /// 初始化toolbar菜单
        /// </summary>
        void InitialzeToolbars();
        /// <summary>
        /// 刷新toolbar
        /// </summary>
        //void RefeshToolbar();
        /// <summary>
        /// 刷新 根据 group name
        /// </summary>
        /// <param name="groupName"></param>
        //void RefeshToolbar(string groupName);

        void SetToolbarGroupStatus(WinformToolbarGroup toolbarGroup, MenuStatus menuStatus);

        /// <summary>
        /// 批量设置toolbar状态
        /// </summary>
        /// <param name="toolbars"></param>
        /// <param name="menuStatus"></param>
        void SetToolbarsStatus(IList<WinformToolbar> toolbars,MenuStatus menuStatus);
        /// <summary>
        /// 设置toolbar状态
        /// </summary>
        /// <param name="toolbar"></param>
        /// <param name="menuStatus"></param>
        void SetToolbarStatus(WinformToolbar toolbar, MenuStatus menuStatus);
        /// <summary>
        /// 清除toolbar；将隐藏所有的Toolbars
        /// </summary>
        void ClearToolbars();
    }
}
