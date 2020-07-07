using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication.Configuration
{
    /// <summary>
    /// http 请求拦截配置
    /// </summary>
    public class HttpInterceptConfiguration : IHttpInterceptConfiguration
    {
        public Action<IHttp> OnBeforeRequest { get ; set  ; }
        public Action<IRestResponse> OnAfterRequest { get ; set  ; }
    }
}
