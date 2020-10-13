 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Winform.MainFormDocument.Resources
{
    /// <summary>
    /// 文档资源接口
    /// </summary>
    public interface IDocumentResource:IResource
    {
        /// <summary>
        /// 资源对应的DocmentControl
        /// </summary>
        Type DocumentControlType { get; }
        /// <summary>
        /// 打开资源
        /// </summary>
        void Open();
        /// <summary>
        /// 保存资源
        /// </summary>
        void Save();
    }
}
