using Abp.Dependency;
using Abp.Events.Bus;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using ZtgeoGISDesktop.Communication.BackendRequest;
using ZtgeoGISDesktop.Communication.Share.Authorization;
using ZtgeoGISDesktop.Core.Authorization;
using ZtgeoGISDesktop.Core.Authorization.EventDatas;
using ZtgeoGISDesktop.Share.AjaxModels.TokenAuth;
using ZtgeoGISDesktop.Share.Authorization;

namespace ZtgeoGISDesktop.Communication.InterceptEvent
{
    /// <summary>
    /// 请求之后拦截
    /// </summary>
    public static class AfterRequestInterceptor
    {
        public static IocManager iocManager { private get; set; }
        /// <summary>
        /// 请求之后进行拦截。
        /// 因为后端返回的数据是经过统一封装的。所以需要进行统一的异常处理和数据Model统一处理
        /// </summary>
        /// <returns></returns>
        public static void Interceptor(IRestResponse restResponse) {
            
             
        }


        private static string PraseJsonAndGetResultString(string originalJson) {
            // todo
            
            return originalJson;
        }
 
        /// <summary>
        /// 处理授权异常错误
        /// </summary>
        /// <returns></returns>
        private static bool handleUnAuthorizedRequest(IRestResponse restResponse) {
            var eventBus = iocManager.Resolve<IEventBus>();
            if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (LoginInfoCache.AuthenticateModel != null
                    && string.IsNullOrEmpty(LoginInfoCache.AuthenticateModel.UserNameOrEmailAddress)
                     && string.IsNullOrEmpty(LoginInfoCache.AuthenticateModel.Password)
                    )
                { // 从缓存中重新认证
                    var authorizationManager = iocManager.Resolve<IAuthorizationManager>();
                    if (!authorizationManager.Authorization(LoginInfoCache.AuthenticateModel))
                    { 
                        restResponse = null;
                        return false;
                    }
                    //认证后重新发送请求数据 
                }
            }
            else if (restResponse.StatusCode == HttpStatusCode.Forbidden) {
                eventBus.Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData());
                restResponse = null;
            }
            else if (restResponse.StatusCode == HttpStatusCode.NotFound)
            {
                eventBus.Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData());
                restResponse = null;
            }

            return true;
        }

    }
}
