using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication;
using ZtgeoGISDesktop.Communication.Share.Authorization;
using ZtgeoGISDesktop.Core.Share.Constants; 

namespace ZtgeoGISDesktop.Communication.InterceptEvent
{
    public static class BeforeRequestInterceptor
    {
        /// <summary>
        /// 拦截请求，添加
        /// </summary>
        /// <param name="http"></param>
        public static void Interceptor(CommunicationContext requestContext, IHttp http)
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
            if(LoginInfoCache.AuthenticateResultModel!=null && !string.IsNullOrEmpty(LoginInfoCache.AuthenticateResultModel.AccessToken))
            {
                http.Headers.Add(new HttpHeader("Authorization", "Bearer " + LoginInfoCache.AuthenticateResultModel.AccessToken));
            }
        }
        private static void addAspNetCoreCultureHeader(IHttp http) {
            http.Headers.Add(new HttpHeader(".AspNetCore.Culture", LoginInfoCache.CultureInfo));
        }
        private static void addAcceptLanguageHeader(IHttp http) {
            http.Headers.Add(new HttpHeader("Accept-Language", LoginInfoCache.CultureInfo));
        }
        private static void addTenantIdHeader(IHttp http) {
            if (LoginInfoCache.AuthenticateModel != null && !string.IsNullOrEmpty(LoginInfoCache.AuthenticateModel.TenantId)) {
                http.Headers.Add(new HttpHeader(HttpHeaderConstants.MutitenancyTenantIdKeyName, LoginInfoCache.AuthenticateModel.TenantId));
            }
        }
    }
}
