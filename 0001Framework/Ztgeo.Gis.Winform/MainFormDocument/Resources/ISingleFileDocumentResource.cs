using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    public interface ISingleFileDocumentResource : IDocumentResource
    {
        /// <summary>
        /// 单文件全路径
        /// </summary>
        string FilePath { get; }
        /// <summary>
        /// 拓展名
        /// </summary>
        string ExtensionName { get; }
        /// <summary>
        /// 单文件资源元数据
        /// </summary>
        ISingleFileDocumentResourceMetaData SingleFileDocumentResourceMetaData { get; }

    }
}
