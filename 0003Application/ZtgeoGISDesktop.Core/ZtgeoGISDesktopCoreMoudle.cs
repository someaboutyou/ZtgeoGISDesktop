using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Hangfire;
using Hangfire.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization;
using ZtgeoGISDesktop.Core.Authorization; 

namespace ZtgeoGISDesktop.Core
{
    [DependsOn(typeof(AbpHangfireModule))]
    public class ZtgeoGISDesktopCoreMoudle: AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IAuthorizationManager, AuthorizationManager>();
            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSQLiteStorage("Default");  
            }); 
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZtgeoGISDesktopCoreMoudle).Assembly);
            //IocManager.Resolve<HFServerManager>().Initialize();
        }

        public override void PostInitialize()
        { 

        }

        public override void Shutdown()
        {
            base.Shutdown();
            //IocManager.Resolve<HFServerManager>().Dispose();
        }
    }
}
