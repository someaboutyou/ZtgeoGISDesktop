using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
    /// <summary>
    /// 文档接口
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// 文档资源
        /// </summary>
        IDocumentResource DocumentResource { get; }
        /// <summary>
        /// 文档类型的辨别码。 标识这个是什么文档。
        /// </summary>
        string TypeUniqueCode { get; }
        /// <summary>
        /// 是否是附属文档
        /// </summary>
        bool IsSubDocument { get; } 
        /// <summary>
        /// 文档所属控件
        /// </summary>
        IDocumentControl HostControl { get; } 
        /// <summary>
        /// 文档名称
        /// </summary>
        string DocumentName { get; } 
        /// <summary>
        /// 从资源上加载文档
        /// </summary>
        /// <param name="documentResource"></param>
        void LoadFromResource(IDocumentResource documentResource, params object[] otherParams);
        /// <summary>
        /// 是否已经加载资源
        /// </summary>
        /// <returns></returns>
        bool IsLoadedResource();
        /// <summary>
        /// 保存
        /// </summary>
        void Save();
        /// <summary>
        /// 关闭文档
        /// </summary>
        void Close();
    }
}
