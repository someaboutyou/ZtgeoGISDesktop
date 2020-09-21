using Abp.Hangfire.Configuration;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Background.Configuration;

namespace Ztgeo.Gis.Background.HangfireOwin
{
    public class HFServerManager : Abp.Dependency.ISingletonDependency, IDisposable
    {
        private IAbpHangfireConfiguration AbpHangfireConfiguration;
        private IHangfireDashboardConfiguration HangfireDashboardConfiguration;
        public HFServerManager(IAbpHangfireConfiguration _AbpHangfireConfiguration,
                IHangfireDashboardConfiguration _HangfireDashboardConfiguration
            ) {
            AbpHangfireConfiguration = _AbpHangfireConfiguration;
            HangfireDashboardConfiguration = _HangfireDashboardConfiguration;
        }
        private IDisposable _dashServer;
        /// <summary>
        /// 初始化Owin
        /// </summary>
        public void Initialize() {
            InitializeOwin();
        }

        private void InitializeOwin() {
            StartOptions options = new StartOptions();
            options.Urls.Add("http://localhost:"+ HangfireDashboardConfiguration.DashboardPort);
            _dashServer = WebApp.Start<HFOwinStartup>(options);
        }
        public void Dispose()
        {
            if(AbpHangfireConfiguration.Server != null) {
                AbpHangfireConfiguration.Server.Dispose();
            }
            this._dashServer.Dispose();
        }
         
    }
}
