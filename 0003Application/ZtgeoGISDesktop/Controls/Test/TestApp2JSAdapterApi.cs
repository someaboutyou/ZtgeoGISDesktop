using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace ZtgeoGISDesktop.Test
{
    public class TestApp2JSAdapterApi: IApp2JSAdapterApi
    {
        public IJSContextProvider JsCtx { get; set; }

        public virtual void AlertMessage(string message) { }
         
    }
}
