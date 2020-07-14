using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication.Configuration;

namespace Ztgeo.Gis.Communication
{
    [DependsOn()]
    public class ZtgeoGisCommunicationMoudle: AbpModule
    {
        public override void PreInitialize()
        { 
            IocManager.Register<HttpInterceptConfiguration>();
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
