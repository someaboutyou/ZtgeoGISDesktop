using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Hybrid.WebView
{
    /// <summary>
    /// 该类库集中了html 页面内容
    /// </summary>
    [DependsOn()]
    public class ZtgeoGISDesktopHybridWebViewModule:AbpModule
    {
        public override void PreInitialize()
        { 
        }

        public override void Initialize()
        {
            //IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
