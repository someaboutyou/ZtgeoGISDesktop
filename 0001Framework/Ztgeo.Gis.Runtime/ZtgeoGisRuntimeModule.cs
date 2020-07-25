using Abp.Events.Bus;
using Abp.Modules;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Bootstrapper;

namespace Ztgeo.Gis.Runtime
{
    public class ZtgeoGisRuntimeModule:AbpModule
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
            IocManager.Resolve<ILicenseInitializer>().InitializeApplication(
               new esriLicenseProductCode[] { 
                   esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB 
               },
               new esriLicenseExtensionCode[] {
                    esriLicenseExtensionCode.esriLicenseExtensionCodeGeoStats,
                    esriLicenseExtensionCode.esriLicenseExtensionCode3DAnalyst,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeSchematics,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeMLE,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeDataInteroperability,
                    esriLicenseExtensionCode.esriLicenseExtensionCodeTracking });
        }
    }
}
