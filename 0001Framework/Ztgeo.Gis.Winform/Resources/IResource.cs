using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Actions;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 资源的积累。何为资源？ 资源可能是硬盘的上的一个文件，也可能是数据库里面的一个表格等。
    /// 资源包含图标表达，路径表达等。
    /// 继承该接口后，程序启动时，会将所有程序所能鉴别的资源注入ResourceProvider中
    /// Power by JZW
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// 全名称，一般从全路径中可以判断出资源的位置，例如文件资源的路径
        /// </summary>
        string FullName { get;  }
        /// <summary>
        /// 针对资源显示的图标，例如dwg文件显示cad的图标，如shapefile 显示点线面图标
        /// </summary>
        Image Icon { get;  }
        /// <summary>
        /// 资源的名称，如文件名，图层名等等
        /// </summary>
        string Name { get;  }
         /// <summary>
         /// 扩展名
         /// </summary>
        string ExtensionName { get;  }
        /// <summary>
        /// 鉴别resource类型。系统会将继承自IResource接口的类各自执行一次此方法，用于鉴别此资源所属何种资源
        /// 系统内置有各种文件资源、数据库资源等等。文件资源一般可以根据后缀名判断，数据库资源根据
        /// </summary>
        /// <returns></returns>
        bool IdentifiedResource();
        /// <summary>
        /// 为了提高鉴别效率，设置此order。
        /// </summary>
        int IdentifiedOrder { get; }
        /// <summary>
        /// 单击事件 
        /// </summary>
        IResourceClickAction ClickAction { get; }
        /// <summary>
        /// 双击事件
        /// </summary>
        IResourceDoubleClickAction DoubleClickAction { get; }
        /// <summary>
        /// 资源上的右键事件 
        /// </summary>
        IOrderedEnumerable<IContextMenuItemAction> ContextActions { get; }
    }
}
