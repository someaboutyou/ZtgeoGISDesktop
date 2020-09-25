using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.Context
{
    /// <summary>
    /// power by JZW 2020.09.21
    /// 宿主环境变量，从启动参数里面获得.
    /// Winform启动时获取数据
    /// </summary>
    public static class HostingEnvironment
    {
        /// <summary>
        /// 运行环境名
        /// </summary>
        public static string EnvironmentName { get; set; }
        /// <summary>
        /// 是否开发环境
        /// </summary>
        public static bool IsDevelopment { get; set; }
        /// <summary>
        /// 获得环境变量信息
        /// </summary>
        /// <param name="args"></param>
        public static void GetHostingEnvironmentInfo(string[] args) {
            foreach (string arg in args) {
                if (arg.IndexOf("=") > 0) {
                    string[] argSplited = arg.Split('=');
                    if (argSplited[0].ToUpper().Equals("EnvironmentName".ToUpper())) {
                        HostingEnvironment.EnvironmentName = argSplited[1];
                    }
                    if (argSplited[0].ToUpper().Equals("IsDevelopment".ToUpper()))
                    {
                        if (argSplited[1].ToUpper().Equals("TRUE") || argSplited[1].ToUpper().Equals("T")) {
                            IsDevelopment = true;
                        }
                    }
                }
            }
        }
    }
}
