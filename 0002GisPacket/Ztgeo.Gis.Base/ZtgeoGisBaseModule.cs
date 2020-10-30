using Abp.Dependency;
using Abp.Modules;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Base.Bootstrapper;
using Ztgeo.Gis.Base.ShapeFile;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Base
{
    public class ZtgeoGisBaseModule:AbpModule
    {
        public override void PreInitialize()
        { 
            IocManager.Register<ShapeFileSingleFileResource>(DependencyLifeStyle.Transient);
            IocManager.Register<ShapeFileSingleFileResourceMetaData>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Resolve<IResourceMetaDataProvider>().ResourceMetaDataProviders.Add(typeof(ShapeFileSingleFileResourceMetaData)); //添加CAD文件元数据

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
