using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.Winform.MainFormLayer
{
    public interface ILayerControl
    {
        IDocumentControl ActiveDocumentControl { get; set; }
        /// <summary>
        /// 当前活动的文档
        /// </summary>
        IDocument ActiveDocument { get; set; }
        /// <summary>
        /// 设置图层信息
        /// </summary>
        void SetLayerInfo();
        /// <summary>
        /// 清理当前图层数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 是否关闭着。假如关闭了，不刷新
        /// </summary>
        bool IsClosed { get; }
        /// <summary>
        /// 打开ILayerControl
        /// </summary>
        void Show();
    }
}
