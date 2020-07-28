using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder.BinderInterceptor;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public abstract class Js2AppAdapterApiBase : IJS2AppAdapterApi
    {
        public IJSContextProvider JsCtx { get ;protected set ; }

        public virtual string AppBindObjectName { get; protected set; }
        [DisAdapter]
        public virtual void BindCtx4JS2App(IJSContextProvider jsCtx) {
            this.JsCtx = jsCtx;
            string appBindObjectName= AppBindObjectName.IsEmpty()? this.GetType().Name: AppBindObjectName;
            jsCtx.ExecuteScriptFunction(getJsFunctionString(appBindObjectName), Array.Empty<object>());
            jsCtx.BindVariable(appBindObjectName, this);
        }

        private string getJsFunctionString(string appBindObjectName)
        {
            //return "(function() { window." + appBindObjectName + " = CommonAPI.ViewAdapterWrapper(window.__" + appBindObjectName + ",{trackCodes: true,}); })";
            return "(function() { require([\"./js/Js2AppAdapterApi/" + appBindObjectName + ".js\"]); })";
        }
    }
}
