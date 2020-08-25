using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Core
{
    public class ZtgeoGISDesktopCoreMoudle: AbpModule
    {
        public override void PreInitialize()
        {

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
