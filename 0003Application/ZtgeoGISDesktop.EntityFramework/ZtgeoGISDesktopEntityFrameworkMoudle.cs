using Abp.EntityFramework;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Core;

namespace ZtgeoGISDesktop.EntityFramework
{
    [DependsOn(typeof(AbpEntityFrameworkModule))]
    public class ZtgeoGISDesktopEntityFrameworkMoudle:AbpModule
    {
        public override void PreInitialize()
        {
            //IocManager.Register<IZtgeoGISDesktopConnectStringConfigure, ZtgeoGISDesktopConnectStringConfigure>();
            Configuration.DefaultNameOrConnectionString = "Default";
            //Configuration.DefaultNameOrConnectionString = "Data Source=./db/desktopSqlite.db";
            Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted; 
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZtgeoGISDesktopEntityFrameworkMoudle).Assembly);
            
        }

        public override void PostInitialize()
        {

        }
    }
}
