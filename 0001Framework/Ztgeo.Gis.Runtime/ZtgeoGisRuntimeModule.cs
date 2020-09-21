using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.Core.Logging; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization;
using Ztgeo.Gis.Runtime.Context;

namespace Ztgeo.Gis.Runtime
{
    public class ZtgeoGisRuntimeModule:AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IAbpSession, ZtgeoAbpSession>();
            IocManager.Register<ProductInfo>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }

        public override void PostInitialize()
        {
            var productInfo = IocManager.Resolve<ProductInfo>();
            //productInfo.SupportBy=Configuration.
        }
    }
}
