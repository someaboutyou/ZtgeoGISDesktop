using Abp.Configuration.Startup;
using Castle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;
using Ztgeo.Gis.Hybrid.JsBinder.BinderInterceptor;

namespace Ztgeo.Gis.Hybrid
{
    public class HybridBootstrap
    {
        public static void Initialize(IAbpStartupConfiguration configuration) {
            //代理从C# 代码调用js 代码的方法
            configuration.IocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (typeof(IApp2JSAdapterApi).IsAssignableFrom(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(App2JsAdapterInterceptor)));
                }
            };
        }
    }
}
