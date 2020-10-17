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
        /// 显示名称
        /// </summary>
        string Caption { get; set; }
        /// <summary>
        /// 元数据
        /// </summary>
        IResourceMetaData ResourceMetaData { get; } 
    }
}
