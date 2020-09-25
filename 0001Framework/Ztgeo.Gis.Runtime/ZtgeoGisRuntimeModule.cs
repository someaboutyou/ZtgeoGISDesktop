using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization;
using Ztgeo.Gis.Runtime.Configuration;
using Ztgeo.Gis.Runtime.Context;

namespace Ztgeo.Gis.Runtime
{
    public class ZtgeoGisRuntimeModule:AbpModule
    {
        private IConfigurationRoot _appConfiguration;
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IAbpSession, ZtgeoAbpSession>();
            IocManager.Register<ProductInfo>(); 
            //获得启动参数
            HostingEnvironment.GetHostingEnvironmentInfo(Environment.GetCommandLineArgs());
            _appConfiguration = AppConfigurations.Get();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()); 
            var productInfo = IocManager.Resolve<ProductInfo>();
            productInfo.Initialize(_appConfiguration);
        }

        public override void PostInitialize()
        {
            //productInfo.SupportBy=Configuration.
        }
    }
}
