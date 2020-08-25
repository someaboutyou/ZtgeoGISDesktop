using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.Configuration;

namespace Ztgeo.Gis.Hybrid
{
    [DependsOn()]
    public class ZtgeoGisHybridMoudle:AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<HybridConfiguration>(); 
            HybridBootstrap.Initialize(Configuration);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            WebViewExtensions.HybridConfiguration = IocManager.Resolve<HybridConfiguration>();
        }

        public override void PostInitialize()
        {

        }
    }
}
