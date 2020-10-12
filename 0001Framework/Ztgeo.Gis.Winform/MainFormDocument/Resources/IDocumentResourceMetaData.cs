using System;
using System.Drawing;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    /// <summary>
    /// 文档资源元数据
    /// </summary>
    public interface IDocumentResourceMetaData: IResourceMetaData
    {
        /// <summary>
        /// 该元数据对应的资源类。 例如打开资源时，可以根据resource 判断怎么打开
        /// </summary>
        Type TypeOfDocumentResource { get; } 
    }
}
