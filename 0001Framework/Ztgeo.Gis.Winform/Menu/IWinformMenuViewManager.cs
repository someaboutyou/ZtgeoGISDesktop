using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    /// <summary>
    /// winform 菜单显示
    /// </summary>
    public interface IWinformMenuViewManager : ISingletonDependency
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

        void SetMenuStatus(WinformMenu menu, MenuStatus menuStatus);
    }
}
