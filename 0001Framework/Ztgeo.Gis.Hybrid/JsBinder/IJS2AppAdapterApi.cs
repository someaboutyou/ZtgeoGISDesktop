using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public interface IJS2AppAdapterApi 
    {
        IJSContextProvider JsCtx { get; set; }
    }
}
