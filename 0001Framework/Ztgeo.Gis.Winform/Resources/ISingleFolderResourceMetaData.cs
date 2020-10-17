using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    public interface ISingleFolderResourceMetaData :IResourceMetaData
    {
        /// <summary>
        /// 鉴别单个文件夹啊是否是该资源
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        bool Identified(string folderPath);
        /// <summary>
        /// 在父文件夹下面寻找文件夹资源
        /// </summary>
        /// <param name="fatherDirectoryPath"></param>
        /// <returns></returns>
        IList<ISingleFolderResource> FindSingleFolderResourceInDirectory(string fatherDirectoryPath);
    }
}
