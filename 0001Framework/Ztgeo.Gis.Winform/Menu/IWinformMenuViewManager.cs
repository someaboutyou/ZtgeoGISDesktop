using Abp.Application.Services;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    /// <summary>
    /// winform 菜单显示管理
    /// </summary>
    public interface IWinformMenuViewManager : IApplicationService
    {
        /// <summary>
        /// 初始化菜单
        /// </summary>
        void InitializeMenus();
        /// <summary>
        /// 刷新界面
        /// </summary>
        void RefeshMenu();
        /// <summary>
        /// 获得界面顺序设置
        /// </summary>
        /// <returns></returns>
        IList<MenuOrderSetting> GetMenuOrderSettings();
        /// <summary>
        /// 设置菜单状态，可用、不可用、隐藏
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menuStatus"></param>
        void SetMenuStatus(WinformMenu menu, MenuStatus menuStatus);
    }
}
