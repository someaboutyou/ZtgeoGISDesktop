using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
    /// <summary>
    /// 文档接口
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// 文档类型的辨别码。 标识这个是什么文档。
        /// </summary>
        string TypeUniqueCode { get; }
        /// <summary>
        /// 是否是附属文档
        /// </summary>
        bool IsSubDocument { get; }
        /// <summary>
        /// 父文档（当文档本身作为父文档时，此值为空）
        /// </summary>
        IDocument ParentDocument { get; }
        /// <summary>
        /// 文档所属控件
        /// </summary>
        IDocumentControl HostControl { get; }
        /// <summary>
        /// 拓展名
        /// </summary>
        string ExtensionName { get; }
        /// <summary>
        /// 文档名称
        /// </summary>
        string DocumentName { get; }
        /// <summary>
        /// 文档路径或者地址
        /// </summary>
        string FilePath { get; }
        /// <summary>
        /// 从文件加载文档
        /// </summary>
        /// <param name="path"></param>
        /// <param name="otherParams"></param>
        void LoadFromFile(string path,params object[] otherParams);
        /// <summary>
        /// 从流加载文档
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="otherParams"></param>
        void LoadFromStream(Stream stream, params object[] otherParams);
        /// <summary>
        /// 从web上加载文档
        /// </summary>
        /// <param name="url"></param>
        /// <param name="otherParams"></param>
        void LoadFromWeb(string url, params object[] otherParams);
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
