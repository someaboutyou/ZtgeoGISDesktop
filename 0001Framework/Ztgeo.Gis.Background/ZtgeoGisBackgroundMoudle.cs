using Abp;
using Abp.Hangfire;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Background.Configuration;
using Ztgeo.Gis.Background.HangfireOwin;

namespace Ztgeo.Gis.Background
{
    /// <summary>
    /// powered by JZW
    /// 该模块是后台工作的模块，默认是采用hangfire 进行
    /// </summary>
    [DependsOn(typeof(AbpKernelModule),typeof(AbpHangfireModule))]
    public class ZtgeoGisBackgroundMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IHangfireDashboardConfiguration, HangfireDashboardConfiguration>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Resolve<HFServerManager>().Initialize();
        }

        public override void PostInitialize()
        {

        }

        public override void Shutdown()
        {
            base.Shutdown();
            IocManager.Resolve<HFServerManager>().Dispose();
        }
    }
}
