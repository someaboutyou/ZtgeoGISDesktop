using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface ISingleFileResource :IResource
    {
        /// <summary>
        /// 全名称，一般从全路径中可以判断出资源的位置，例如文件资源的路径
        /// </summary>
        string FullName { get; } 
        /// <summary>
        /// 扩展名
        /// </summary>
        string ExtensionName { get; }

    }
}
