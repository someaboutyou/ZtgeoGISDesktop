using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace WebViewControlTest
{
    public class TestApp2JSAdapterApi: IApp2JSAdapterApi
    {
        public IJSContextProvider JsCtx { get; set; }

        public string JSBindObjectName => throw new NotImplementedException();

        public virtual void AlertMessage(string message) { }

        public void BindCtx4App2Js(IJSContextProvider jsCtx)
        {
            throw new NotImplementedException();
        }
    }
}
