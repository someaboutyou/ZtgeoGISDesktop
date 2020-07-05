using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Share.AjaxModels.TokenAuth;

namespace ZtgeoGISDesktop.Share.Authorization
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public static class LoginInfo
    {
        /// <summary>
        /// 登录请求
        /// </summary>
        public static AuthenticateModel AuthenticateModel { get; set; }
        /// <summary>
        /// 登录返回
        /// </summary>
        public static AuthenticateResultModel AuthenticateResultModel { get; set; }
        /// <summary>
        /// 权限字符串
        /// </summary>
        public static IList<string> Permissions { get; set; }

        public static string CultureInfo { get; set; }
         
    }
}
