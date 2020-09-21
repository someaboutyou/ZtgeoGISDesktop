using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument;

namespace Ztgeo.Gis.Winform.MainFormProperty
{

    public interface IPropertiesControl
    {
        /// <summary>
        /// 当前活动的文档
        /// </summary>
        IDocument ActiveDocument { get; }
        /// <summary>
        /// 设置属性信息
        /// </summary>
        void SetPropertyInfos();
        /// <summary>
        /// 清理当前属性页数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 是否关闭。假如关闭了，不刷新
        /// </summary>
        bool IsClosed { get;  }
        /// <summary>
        /// 打开属性页
        /// </summary>
        void Show();
    }
}
