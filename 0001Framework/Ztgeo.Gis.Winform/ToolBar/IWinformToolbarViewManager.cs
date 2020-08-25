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
    public interface IWinformToolbarViewManager : IApplicationService
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

        void SetToolbarStatus(WinformToolbar toolbar, MenuStatus menuStatus);
    }
}
