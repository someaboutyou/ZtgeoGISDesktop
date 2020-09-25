using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormStatusBar
{
    /// <summary>
    /// 状态显示类型
    /// </summary>
    public enum StatusShowType
    {
        /// <summary>
        /// 状态栏显示文字信息
        /// </summary>
        Msg,  
        /// <summary>
        /// 状态栏显示进度条
        /// </summary>
        ProcessBar,
        /// <summary>
        /// 状态栏显示进度条（不带具体进度）
        /// </summary>
        ProcessBarWithoutProcess
    }
}
