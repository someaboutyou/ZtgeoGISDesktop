using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Runtime.Hook
{
    /// <summary>
    /// 全局的Gis系统钩子函数，该钩子函数全局静态。 会触发一系列的GIS界面、数据修改事件。
    /// 由于是全局静态的，事件触发会加锁控制。
    /// </summary>
    public interface IGisApplicationHook :Abp.Dependency.ISingletonDependency
    {
        #region 菜单
        /// <summary>
        /// 菜单状态设置
        /// </summary>
        void SetMenuStatus(WinformMenu menu, MenuStatus menuStatus);
        #endregion

        #region toolbar

        #endregion

        #region TOC
        #endregion

        #region mapcontrol
        #endregion

    }
} 
