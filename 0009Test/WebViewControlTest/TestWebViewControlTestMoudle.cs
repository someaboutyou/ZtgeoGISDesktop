using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid;

namespace WebViewControlTest
{
    [DependsOn(typeof(ZtgeoGisHybridMoudle))]
    public class TestWebViewControlTestMoudle:AbpModule
    {
        public override void PreInitialize()
        { 
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
