using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder.BinderInterceptor;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    /// <summary>
    /// 从app到JS 代理代理基类
    /// </summary>
    public abstract class App2JSAdapterApiBase: IApp2JSAdapterApi
    {
        public IJSContextProvider JsCtx { get; protected set; }
         
        public virtual string JSBindObjectName { get;protected set; }

        /// <summary>
        /// 绑定JSCtx
        /// </summary>
        /// <param name="jsCtx"></param>
        [DisAdapter]
        public virtual void BindCtx4App2Js(IJSContextProvider jsCtx)
        {
            this.JsCtx = jsCtx;
            string jsObjectName = JSBindObjectName.IsEmpty() ? this.GetType().Name : JSBindObjectName; 
            jsCtx.ExecuteScriptFunction(getJsFunctionString(jsObjectName), Array.Empty<object>());
        }

        private string getJsFunctionString(string jsObjectName)
        {
            //return "(function() { window." + jsObjectName + " = CommonAPI.ViewAdapterWrapper(window.__" + jsObjectName + ",{trackCodes: true,}); })";
            return "(function(){ require([\"./js/App2JSAdapterApi/" + jsObjectName + ".js\"])})";
        }
    }
}
