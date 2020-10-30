using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 本地资源管理
    /// </summary>
    public interface ILocalResourceManager:Abp.Dependency.ITransientDependency
    {
        /// <summary>
        /// 获取某路径下面所有的文件资源
        /// </summary>
        /// <param name="directoryPath">被获取的路径</param>
        /// <param name="metaDataFilter">待查找的资源</param>
        /// <param name="restDirs">除去资源后剩余的文件夹路径</param>
        /// <param name="restFiles">出去资源后剩余的文件的路径</param>
        /// <returns>获取到的资源</returns>
        IList<IResource> GetLocalResource(string directoryPath, IList<IResourceMetaData> metaDataFilter, out IList<string> restDirs,out IList<string> restFiles);
        /// <summary>
        /// 获取所有系统内置的资源，没有过滤
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="restDirs"></param>
        /// <param name="restFiles"></param>
        /// <returns></returns>
        IList<IResource> GetAllLocalResource(string directoryPath, out IList<string> restDirs, out IList<string> restFiles);
    }
}
