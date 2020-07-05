using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication.Configuration
{
    public interface IHttpInterceptConfiguration
    {
        /// <summary>
        /// 请求之前
        /// 可以在请求之前，加上Token
        /// </summary>
        Action<IHttp> OnBeforeRequest { get; set; }
        /// <summary>
        /// 请求之后
        /// </summary>
        Func<string, string> OnAfterRequest { get; set; }
    }
}
