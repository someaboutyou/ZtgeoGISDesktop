using Abp;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension; 

namespace Ztgeo.Gis.AbpExtension
{
    [DependsOn(typeof(AbpKernelModule) )]
    public class ZtgeoGisExtensionMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<ISettingManager, Ztgeo.Gis.AbpExtension.SettingExtension.SettingManager>();
            AutoSettingsManager.Initialize(Configuration);
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
