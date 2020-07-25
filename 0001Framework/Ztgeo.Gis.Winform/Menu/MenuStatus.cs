using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    /// <summary>
    /// 菜单状态
    /// </summary>
    public enum MenuStatus
    {
        /// <summary>
        /// 可用的,一般是正常状态
        /// </summary>
        Available,
        /// <summary>
        /// 不可用的，一般是不可点击状态
        /// </summary>
        Disable,
        /// <summary>
        /// 隐藏的
        /// </summary>
        Hidden 
    }
}
