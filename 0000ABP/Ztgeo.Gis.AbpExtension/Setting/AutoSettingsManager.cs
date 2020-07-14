using Abp.Configuration.Startup;
using Castle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Setting
{
    public class AutoSettingsManager
    {
        public static void Initialize(IAbpStartupConfiguration configuration)
        {
            configuration.IocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (typeof(ISettings).IsAssignableFrom(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AutoSettingsInterceptor)));
                }
            }; 
            //把自动属性的Provider注册
            configuration.Settings.Providers.Add<AutoSettingsProvider>();
        }
    }
}
