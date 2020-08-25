using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension.SettingExtension;

namespace Ztgeo.Gis.Runtime.Configuration
{  
    public class RuntimeSetting: Abp.Dependency.ISingletonDependency
    {
        /// <summary>
        /// 运行时状态。
        /// </summary>
        public virtual RuntimeStatus RuntimeStatus { get; set; } = RuntimeStatus.Serverless;
    }
}
