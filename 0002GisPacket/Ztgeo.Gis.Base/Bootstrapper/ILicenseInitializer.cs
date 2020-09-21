using Abp.Dependency;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Base.Bootstrapper
{
    public interface ILicenseInitializer : ISingletonDependency
    {
        bool InitializeApplication(esriLicenseProductCode[] productCodes, esriLicenseExtensionCode[] extensionLics);
    }
}
