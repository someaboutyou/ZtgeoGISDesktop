using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Background.Configuration
{
    public interface IHangfireDashboardConfiguration
    {
        int DashboardPort { get; set; }
        string DashboardUrl { get; set; }
    }
    public class HangfireDashboardConfiguration: IHangfireDashboardConfiguration
    {
        /// <summary>
        /// 展示端口号, 默认9095
        /// </summary>
        public int DashboardPort { get; set; } = 9095;
        /// <summary>
        /// 查看地址
        /// </summary>
        public string DashboardUrl { get; set; } = "hangfire";
         
    }
}
