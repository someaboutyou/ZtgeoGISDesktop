using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Actions
{
    /// <summary>
    /// 右键列表item
    /// </summary>
    public interface IContextMenuItemAction : IWinformAction, Abp.Dependency.ISingletonDependency
    {
        Image ItemIcon { get; } //item 图标 

        object Sender { get; }
        /// <summary>
        /// 是否为分割线，作为分割线，不能执行事件
        /// </summary>
        bool IsSplitor { get; }
    }
}
