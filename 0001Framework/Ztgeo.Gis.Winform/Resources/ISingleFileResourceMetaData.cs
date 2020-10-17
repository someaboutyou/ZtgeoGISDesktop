using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface ISingleFileResourceMetaData : IResourceMetaData 
    {
        /// <summary>
        /// 选择时的filter ExtensionName
        /// </summary>
        List<string> SelectFilterExtensionName { get; }
        /// <summary>
        /// 鉴别单个文件是否是该资源
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool Identified(string filePath);
        /// <summary>
        /// 在文件夹下面查找对应的资源
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        IList<ISingleFileResource> FindSingleFileResourceInDirectory(string directoryPath);
    }
}
