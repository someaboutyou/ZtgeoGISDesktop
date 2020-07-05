using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication;
using Ztgeo.Gis.Communication.Configuration;
using ZtgeoGISDesktop.Communication.InterceptEvent;

namespace ZtgeoGISDesktop.Communication
{
    [DependsOn(typeof(ZtgeoGisCommunicationMoudle))]
    public class ZtgeoGISDesktopCommunicationMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.HttpInterceptEvents().OnAfterRequest = AfterRequestInterceptor.Interceptor;
            Configuration.Modules.HttpInterceptEvents().OnBeforeRequest =
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
