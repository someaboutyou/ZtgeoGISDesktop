using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
    /// <summary>
    /// 界面上的文档管理，Document  
    /// </summary>
    public interface IDocumentManager : Abp.Dependency.ISingletonDependency
    {
        /// <summary>
        /// 添加一个文档
        /// </summary>
        /// <param name="cocument"></param>
        /// <returns></returns>
        IDocumentControl AddADocument<T>(string documentName) where T :IDocumentControl;
        /// <summary>
        /// 增加一个文档的子文档
        /// </summary>
        /// <param name="childDocument"></param>
        /// <param name="parentDocument"></param>
        /// <returns></returns>
        IDocumentControl AddAChildDocument(IDocument childDocument, IDocument parentDocument);
        /// <summary>
        /// 设置一个文档时激活状态
        /// </summary>
        /// <param name="document"></param>
        void SetDocumentControlActive(IDocumentControl documentContorl);
        /// <summary>
        /// 获得当前活动文档
        /// </summary>
        IDocument GetActiveDocument { get; }

        /// <summary>
        /// 获得当前活动文档控件
        /// </summary>
        IDocumentControl GetActiveDocumentControl { get; }

        IList<IDocumentControl> DocumentList { get; set; }

    }
}
