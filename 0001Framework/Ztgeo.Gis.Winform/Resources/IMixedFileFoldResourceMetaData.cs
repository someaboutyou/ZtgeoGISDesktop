using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 文件夹和文件混合的资源对象
    /// </summary>
    public interface IMixedFileFoldResourceMetaData: IResourceMetaData
    {
        /// <summary>
        /// 鉴别这个这么多文件路径里面是不是多文件资源。但是还是根据某个具体的文件去判别的
        /// </summary>
        /// <param name="identifiedFilePath">鉴别路径，主文件路径鉴别</param>
        /// <returns></returns>
        bool Identified(string identifiedFilePath);
        /// <summary>
        /// 找到其他文件 （除了主文件其他文件）
        /// </summary>
        /// <param name="identifiedFilePath"></param>
        /// <returns></returns>
        IList<string> FindOtherFiles(string identifiedFilePath);
        /// <summary>
        /// 找其他文件夹 
        /// </summary>
        /// <param name="identifiedFilePath"></param>
        /// <returns></returns>
        IList<string> FindOtherFolders(string identifiedFilePath);
    }
}
