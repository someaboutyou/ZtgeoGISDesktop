using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Extensions;
using Castle.Core.Logging;
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
using Ztgeo.Gis.Communication;
using Ztgeo.Gis.Runtime.Authorization;
using Ztgeo.Gis.Runtime.Authorization.Login;
using ZtgeoGISDesktop.Communication.Ajax;
using ZtgeoGISDesktop.Communication.BackendRequest; 
using ZtgeoGISDesktop.Core.Authorization;
using ZtgeoGISDesktop.Core.Authorization.EventDatas; 

namespace ZtgeoGISDesktop.Communication.InterceptEvent
{
    /// <summary>
    /// 请求之后拦截
    /// </summary>
    public static class AfterRequestInterceptor
    {
        public static IIocManager IocManager { private get; set; }
        /// <summary>
        /// 请求之后进行拦截。
        /// 因为后端返回的数据是经过统一封装的。所以需要进行统一的异常处理和数据Model统一处理
        /// </summary>
        /// <returns>true 正常逻辑，false 流产</returns>
        public static bool Interceptor(CommunicationContext requestContext,IRestResponse restResponse) {
            if (handleUnAuthorizedRequest(requestContext,restResponse))
            {
                if (restResponse.ErrorException != null)
                {
                    GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData
                    {
                        Message = "Http request get a error.",
                        Details = restResponse.ErrorMessage
                    });
                    return false;
                }
                else
                {
                    return handleResponseContentRequest(restResponse);
                }
            }
            else {
                return false;
            }

        }


        private static bool handleResponseContentRequest(IRestResponse restResponse) {
            AjaxResponse ajaxResponse = JsonConvert.DeserializeObject<AjaxResponse>(restResponse.Content);
            if (ajaxResponse != null)
            {
                if (ajaxResponse.Success)
                {
                    try
                    {
                        string newResponseContent = handleResponseContentSuccess(ajaxResponse);
                        restResponse.Content = newResponseContent;
                        return true;
                    }
                    catch (Exception ex) {
                        GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData
                        {
                            Message = ajaxResponse.Error.Message,
                            Details = ajaxResponse.Error.Details
                        });
                        IocManager.Resolve<ILogger>();
                        return false;
                    }
                }
                else {
                    GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData
                    {
                        Message = ajaxResponse.Error.Message,
                        Details = ajaxResponse.Error.Details
                    });
                    return false;
                }
            }
            else {
                GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData
                {
                    Message = "None response content",
                    Details = "response content is empty" 
                }); 
                return false;
            }
        }

        private static string handleResponseContentSuccess(AjaxResponse ajaxResponse) {
            return JsonConvert.SerializeObject(ajaxResponse.Result);
        }
        private static int AuthorizeCount = 0;
        /// <summary>
        /// 处理授权异常错误
        /// </summary>
        /// <returns></returns>
        private static bool handleUnAuthorizedRequest(CommunicationContext context, IRestResponse restResponse) {
           
            if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (LoginInfoCache.AuthenticateModel != null
                    && !string.IsNullOrEmpty(LoginInfoCache.AuthenticateModel.UserNameOrEmailAddress)
                     && !string.IsNullOrEmpty(LoginInfoCache.AuthenticateModel.Password)
                    )
                { // 从缓存中重新认证
                    var authorizationManager = IocManager.Resolve<IAuthorizationManager>();
                    var authenticateResultModel = authorizationManager.Authorization(LoginInfoCache.AuthenticateModel, false);

                    if (authenticateResultModel!=null&&authenticateResultModel.AccessToken.IsNullOrEmpty())
                    { 
                        restResponse = null;
                        return false;
                    }
                    //认证后重新发送请求数据 
                    if (AuthorizeCount < 5)
                    {
                        var restService = IocManager.Resolve<IRESTServices>();
                        restResponse = restService.GetResponse(context);
                    }
                    else {
                        AuthorizeCount = 0;
                        GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData
                        {
                            Message = "You are not authorized!",
                            Details = "You are not allowed to perform this operation.and over try times"
                        });
                        restResponse = null; 
                        return false;
                    }
                }
            }
            else if (restResponse.StatusCode == HttpStatusCode.Forbidden) {
                GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData { 
                    Message= "You are not authorized!",
                    Details= "You are not allowed to perform this operation."
                });
                restResponse = null;
            }
            else if (restResponse.StatusCode == HttpStatusCode.NotFound)
            {
                GetEventBus().Trigger<HttpResponseErrorEventData>(new HttpResponseErrorEventData {
                    Message = "Resource not found!",
                    Details = "The resource requested could not be found on the server."
                });
                restResponse = null;
            } 
            return true;
        } 
        private static IEventBus GetEventBus()
        {
            var eventBus = IocManager.Resolve<IEventBus>();
            return eventBus;
        } 
    }
}
