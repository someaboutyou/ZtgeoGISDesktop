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
    /// 可跟踪JsObject Get Set 拦截
    /// </summary>
    public class TrackableJsObjectInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            PostProceed(invocation);
        }

        private void PostProceed(IInvocation invocation)
        {
            if (invocation.Proxy is ITrackableJSObject)
            {
                var setFlag = "set_"; 
                var isSet = invocation.Method.Name.StartsWith(setFlag);
                //var isGet = invocation.Method.Name.StartsWith(getFlag);
                if (!isSet )
                    return;
                var pname = invocation.Method.Name.Replace(setFlag, "") ;
                if (pname.Equals("IsTracked")
                    || pname.Equals("JsCtx")
                    || pname.Equals("TrackingCode")
                    || pname.Equals("ModulePath")
                    || pname.Equals("ConstructorFunction")) {
                    return;
                }
                var trackableObject = (ITrackableJSObject)invocation.Proxy;
                trackableObject.JsCtx.MustBeSet();
                invocation.Proceed();
                trackableObject.JsCtx.NotifyPropertyChanged(trackableObject, pname, invocation.Arguments[0]);
            }
        }
    }
}
