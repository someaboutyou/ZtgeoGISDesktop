using Abp.Dependency;
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
    public class ZtgeoGISDesktopCoreMoudle: AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IAuthorizationManager, AuthorizationManager>();
            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSQLiteStorage(".\db\desktopSqlite.db");
            });
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZtgeoGISDesktopCoreMoudle).Assembly);
        }

        public override void PostInitialize()
        { 

        }
    }
}
