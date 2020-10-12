using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    /// <summary>
    /// web资源
    /// </summary>
    public interface IWebDocumentResource : IDocumentResource
    {
        string Url { get; }
        /// <summary>
        /// 主机名或者IP
        /// </summary>
        string HostOrIP { get; }
        /// <summary>
        /// 协议
        /// </summary>
        string Protocol { get;  }
        /// <summary>
        /// 端口号
        /// </summary>
        int Port { get; }
        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// http 请求时的Get、Post、Push...
        /// </summary>
        string HttpMethod { get; }
        /// <summary>
        /// 保存数据的Url
        /// </summary>
        string SaveUrl { get; }
        /// <summary>
        /// 是否可以保存
        /// </summary>
        bool CanSave { get; }

        IWebDocumentResourceMetaData WebDocumentResourceMetaData { get; }

    }
}
