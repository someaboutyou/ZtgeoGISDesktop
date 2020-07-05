using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Communication.InterceptEvent
{
    /// <summary>
    /// 请求之后拦截
    /// </summary>
    public static class AfterRequestInterceptor
    {
        /// <summary>
        /// 请求之后进行拦截。
        /// 因为后端返回的数据是经过统一封装的。所以需要进行统一的异常处理和数据Model统一处理
        /// </summary>
        /// <returns></returns>
        public static string Interceptor(string json) {




            return PraseJsonAndGetResultString(json);
        }


        private static string PraseJsonAndGetResultString(string originalJson) {
            // todo
            return originalJson;
        }
    }
}
