using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Utils;

namespace Ztgeo.Gis.Hybrid.JsBinder.BinderInterceptor
{
    /// <summary>
    /// autor jzw
    /// 主程序调用webview 方法拦截。直接调用jsctx里面的 ExecuteScriptFunction 方法。
    /// 通过拦截虚方法，可以实现直接调用
    /// </summary>
    public class App2JsAdapterInterceptor : IInterceptor
    { 
        private void PostProceed(IInvocation invocation) { 
            if (invocation.Method.GetCustomAttributes(typeof(DisAdapterAttribute), false).Length == 0) {
                if (invocation.Proxy is IApp2JSAdapterApi) {  //在app 上可以访问js 代码
                    var api = (IApp2JSAdapterApi)invocation.Proxy;
                    api.JsCtx.MustBeSet();
                    var jsCtx = api.JsCtx;
                    string jsApiObjectName = invocation.TargetType.Name;
                     jsCtx.ExecuteScriptFunction(jsApiObjectName+"."+ invocation.Method.Name, invocation.Arguments.Select(arg=> {
                         if (arg is IEnumerable<IJSObject>)
                         {
                             return jsCtx.EscapeArray((IEnumerable<IJSObject>)arg);
                         }
                         else if (arg is IEnumerable<Dictionary<string, object>>)
                         {
                             return jsCtx.EscapeArray((IEnumerable<Dictionary<string, object>>)arg);
                         }
                         else if (arg is IEnumerable<string>)
                         {
                             return jsCtx.EscapeArray((IEnumerable<string>)arg);
                         }
                         else if (arg is bool)
                         {
                             return jsCtx.EscapeBoolean((bool)arg);
                         }
                         else if (arg is IJSObject)
                         {
                             return jsCtx.EscapeJSObject((IJSObject)arg);
                         }
                         else if (arg is Dictionary<string, object>)
                         {
                             return jsCtx.EscapeJSObject((Dictionary<string, object>)arg);
                         }
                         else if (arg is string)
                         {
                             return jsCtx.EscapeString((string)arg);
                         }
                         else if (arg is object)
                         {
                             return jsCtx.EscapeJSObjectValue(arg);
                         }
                         else { 
                            return arg;
                         }
                     }).ToArray());
                }
            }   
        }
        
        void IInterceptor.Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            PostProceed(invocation);
        }
    }
}
