using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.EntityFramework
{
    [DependsOn( )]
    public class ZtgeoGISDesktopEntityFrameworkMoudle:AbpModule
    {
        public override void PreInitialize()
        {
            //IocManager.Register<IZtgeoGISDesktopConnectStringConfigure, ZtgeoGISDesktopConnectStringConfigure>();
            Configuration.DefaultNameOrConnectionString = "Default";
            Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted; 
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
