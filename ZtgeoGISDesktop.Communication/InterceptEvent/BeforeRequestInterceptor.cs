using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Share.Authorization;

namespace ZtgeoGISDesktop.Communication.InterceptEvent
{
    public static class BeforeRequestInterceptor
    {
        /// <summary>
        /// 拦截请求，添加
        /// </summary>
        /// <param name="http"></param>
        public static void Interceptor(IHttp http)
        {
            normalizeRequestHeaders(http);
        }
        private static void normalizeRequestHeaders(IHttp http) {
            http.Headers.Add(new HttpHeader("Pragma", "no-cache"));
            http.Headers.Add(new HttpHeader("Cache-Control", "no-cache"));
            http.Headers.Add(new HttpHeader("Expires", "Sat, 01 Jan 2000 00:00:00 GMT"));
            addXRequestedWithHeader(http);
            addAuthorizationHeaders(http);
            addAspNetCoreCultureHeader(http);
            addAcceptLanguageHeader(http);
            addTenantIdHeader(http);
        }
        private static void addXRequestedWithHeader(IHttp http) {
            http.Headers.Add(new HttpHeader("X-Requested-With", "XMLHttpRequest"));
        }

        private static void addAuthorizationHeaders(IHttp http)
        {
            if(LoginInfo.AuthenticateResultModel!=null && !string.IsNullOrEmpty(LoginInfo.AuthenticateResultModel.AccessToken))
            {
                http.Headers.Add(new HttpHeader("Authorization", "Bearer " + LoginInfo.AuthenticateResultModel.AccessToken));
            }
        }
        private static void addAspNetCoreCultureHeader(IHttp http) {
            http.Headers.Add(new HttpHeader(".AspNetCore.Culture", LoginInfo.CultureInfo));
        }
        private static void addAcceptLanguageHeader(IHttp http) {
            http.Headers.Add(new HttpHeader("Accept-Language", LoginInfo.CultureInfo));
        }
        private static void addTenantIdHeader(IHttp http) {
            if (LoginInfo.AuthenticateModel != null && !string.IsNullOrEmpty(LoginInfo.AuthenticateModel.TenantId)) {
                http.Headers.Add(new HttpHeader(ZtgeoGISDesktop.Share.Constants.Abp.MutitenancyTenantIdKeyName, LoginInfo.AuthenticateModel.TenantId));
            }
        }
    }
}
