using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 多个文件夹资源。一个主文件夹和其他附带文件夹。
    /// 例如文件夹组织为， 1 1.1 1.2 。 找到文件夹为1 判断有其它文件夹为1.1，1.2 Identified 返回true 1.1 1.2 为其他文件夹
    /// </summary>
    public interface IMultiFolderResourceMetaData:IResourceMetaData
    {
        /// <summary>
        /// 鉴别这个这么多文件路径里面是不是多文件夹资源。但是还是根据某个具体的文件夹去判别的
        /// </summary>
        /// <param name="folderPaths"></param>
        /// <returns></returns>
        bool Identified(string identifiedFolderPath);
        /// <summary>
        /// 找到其他文件夹
        /// </summary>
        /// <param name="identifiedFolderPath"></param>
        /// <returns></returns>
        IList<string> FindOtherFolders(string identifiedFolderPath);

        IList<IMultiFolderResourceMetaData> FindMultiFolderResourceInDirectory(IList<string> fatherDirectoryPath);
    }
}
