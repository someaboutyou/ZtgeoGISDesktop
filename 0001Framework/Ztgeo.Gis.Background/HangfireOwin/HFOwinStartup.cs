using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Background.HangfireOwin
{
    public class HFOwinStartup
    {
        public void Configuration(IAppBuilder app) {
            var options = new DashboardOptions { AppPath=null};
            app.UseHangfireDashboard("/hangfire", options);
        }
    }
}
