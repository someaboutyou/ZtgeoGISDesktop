using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public abstract class Js2AppAdapterApiBase : IJS2AppAdapterApi
    {
        public IJSContextProvider JsCtx { get ; set ; }

        public virtual void BindCtx(IJSContextProvider jsCtx) {
            this.JsCtx = jsCtx;
            string typeName=this.GetType().Name;
            jsCtx.BindVariable("__"+ typeName, this);
            jsCtx.ExecuteScriptFunction(getJsFunctionString(typeName), Array.Empty<object>());
        }

        private string getJsFunctionString(string typeName) {
            return "(function() { window." + typeName + " = CommonAPI.ViewAdapterWrapper(window.__" + typeName + ",{trackCodes: true,}); })" ;
        }
    }
}
