using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormLayer;
using Ztgeo.Gis.Winform.MainFormProperty;

namespace Ztgeo.Gis.Winform.MainFormDocument
{
    public interface IDocumentControl
    {
        IDocument Document { get; }
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        ILayerControl LayerControl { get; }

        IPropertiesControl PropertiesControl { get; }
    }
}
