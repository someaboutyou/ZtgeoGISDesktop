using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface IResourceMetaData
    {
        /// <summary>
        /// 针对资源显示的图标，例如dwg文件显示cad的图标，如shapefile 显示点线面图标
        /// </summary>
        Image Icon { get; }
        /// <summary>
        /// 资源的名称，如文件名，图层名等等 CAD 文件
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 资源存储类型
        /// </summary>
        ResourceStorageMode ResourceStorageMode { get; }
    }
}
