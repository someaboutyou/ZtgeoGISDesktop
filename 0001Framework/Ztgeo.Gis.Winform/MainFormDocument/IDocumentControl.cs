﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormDocument.Resources;
using Ztgeo.Gis.Winform.MainFormLayer;
using Ztgeo.Gis.Winform.MainFormProperty;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
    public interface IDocumentControl
    {
        IDocument Document { get; }
        /// <summary>
        /// 文档图标
        /// </summary>
        Image DocumentImage { get;  }

        ILayerControl LayerControl { get; }

        IPropertiesControl PropertiesControl { get; }  
         
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        void Open(IDocumentResource documentResource);
        /// <summary>
        /// 不可用状态
        /// </summary>
        void Invalidate(); 
        void SetBusyCursor(); 
        void SetCommonCursor(); 
        bool Focus();
        /// <summary>
        /// 控件激活时触发事件
        /// </summary>
        void Activated();
    }
}
