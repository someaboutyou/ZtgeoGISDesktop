using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.Setting;

namespace Ztgeo.Gis.AbpExtension
{
    public class ZtgeoGisExtensionMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            AutoSettingsManager.Initialize(Configuration);
        }
    }
}
