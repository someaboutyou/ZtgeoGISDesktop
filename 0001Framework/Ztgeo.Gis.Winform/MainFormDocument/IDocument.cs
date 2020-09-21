using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
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
        /// 保存
        /// </summary>
        void Save();
    }
}
