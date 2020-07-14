using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Core.Share.AjaxModels.TokenAuth;

namespace ZtgeoGISDesktop.Communication.Share.Authorization
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public static class LoginInfoCache
    {
        private static object _lockObject = new object();
        public static void SetAuthenticateModelAndAuthenticateResultModel(AuthenticateModel authenticateModel,
               AuthenticateResultModel authenticateResultModel
            ) {
            lock (_lockObject)
            {
                AuthenticateModel = authenticateModel;
                AuthenticateResultModel = authenticateResultModel;
            }
        }

        public static void SetPermissions(IList<string> permissions)
        {
            lock (_lockObject)
            {
                Permissions = permissions;
            }
        }

        public static void SetCultureInfo(string cultureInfo) {
            lock (_lockObject) {
                CultureInfo = cultureInfo;
            }
        }
        /// <summary>
        /// 登录请求
        /// </summary>
        public static AuthenticateModel AuthenticateModel { get;private set; }
        /// <summary>
        /// 登录返回
        /// </summary>
        public static AuthenticateResultModel AuthenticateResultModel { get;private set; }
        /// <summary>
        /// 权限字符串
        /// </summary>
        public static IList<string> Permissions { get; set; }

        public static string CultureInfo { get; set; }
         
    }
}
