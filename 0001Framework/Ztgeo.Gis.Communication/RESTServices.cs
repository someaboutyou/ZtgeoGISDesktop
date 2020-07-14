using Abp.Dependency;
using Castle.Core.Logging;
using Castle.MicroKernel.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Ztgeo.Gis.Communication.Configuration;

namespace Ztgeo.Gis.Communication
{
    public class RESTServices : IRESTServices  
    { 
        public ILogger Logger { get; set; }

        private HttpInterceptConfiguration _httpInterceptConfiguration;
        public RESTServices(HttpInterceptConfiguration httpInterceptConfiguration)
        {
            _httpInterceptConfiguration = httpInterceptConfiguration;
            Logger = NullLogger.Instance;
        }
        public string Get(Uri url,
            bool isRequestIntercept=true,
            bool isResponseIntercept=true, 
            int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            string responseContent= Request(Method.GET, url, isRequestIntercept, isResponseIntercept, timeout, null, null, additionalHeaders); 
            return responseContent; 
        }

        public OutModel Get<OutModel>(Uri url,
            bool isRequestIntercept = true,
            bool isResponseIntercept = true,
            int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            string responseContent = Request(Method.GET, url, isRequestIntercept, isResponseIntercept, timeout, null, null, additionalHeaders);
            return JsonConvert.DeserializeObject<OutModel>(responseContent);
        }

        public string Post(Uri uri,
            string requestContent,
            bool isRequestIntercept = true,
            bool isResponseIntercept = true,int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            string responseContent = Request(Method.POST, uri, isRequestIntercept, isResponseIntercept, timeout, requestContent, null, additionalHeaders); 
            return responseContent; 
        }

        public OutModel Post<OutModel>(Uri uri, object inputModel,
            bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null) {
            string responseContent = Request(Method.POST, uri, isRequestIntercept, isResponseIntercept, timeout, JsonConvert.SerializeObject(inputModel), null, additionalHeaders); 
            return JsonConvert.DeserializeObject<OutModel>(responseContent); 
        }

        private string Request(Method method, Uri uri,
            bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, string requestContent = null, string contentType = null,
        IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            var client = new RestClient(uri);
            client.Timeout =  timeout ;
            var request = new RestRequest(Method.POST);
            var context = new CommunicationContext { 
                Method=method, Uri=uri,IsRequestIntercept= isRequestIntercept,IsResponseIntercept=isResponseIntercept,
                TimeOut=timeout,RequestContent=requestContent,ContentType=contentType, AdditionalHeaders=additionalHeaders
            };
            if (this._httpInterceptConfiguration.OnBeforeRequest != null&& isRequestIntercept)
            {
                request.OnBeforeRequest = (IHttp http) => {
                    this._httpInterceptConfiguration.OnBeforeRequest(context, http);
                }; 
            }
            request.AddHeader("Content-Type", contentType ?? "application/json");
            Logger.Info(string.Format("Http Request: \r\n{0}", requestContent));  
            if (!string.IsNullOrEmpty(contentType))
                request.AddParameter(contentType ?? "application/json", requestContent, ParameterType.RequestBody);
            if (additionalHeaders != null)
            {
                Logger.Info(string.Format("Http Request Header: \r\n{0}", JsonConvert.SerializeObject(additionalHeaders)));
                foreach (KeyValuePair<string,string> additionHeader in additionalHeaders)
                {
                    request.AddHeader(additionHeader.Key, additionHeader.Value);
                }
            }
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage) || response.ErrorException != null)
            {
                Logger.Error(response.ErrorMessage, response.ErrorException); 
            }
            if (this._httpInterceptConfiguration.OnAfterRequest != null && isResponseIntercept)
            { //异常处理，和正常返回处理 
                Logger.Info(string.Format("Http Response: \r\n{0}", response.Content));
                if (this._httpInterceptConfiguration.OnAfterRequest(context, response))
                {
                    return response.Content;
                }
                else {
                    Logger.Error("Response filter error");
                    return string.Empty;
                }
                
            }
            else { 
                Logger.Info(string.Format("Http Response: \r\n{0}", response.Content));
                return response.Content;
            }  
        }

        public IRestResponse GetResponse(Method method, Uri uri,
            bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, string requestContent = null, string contentType = null,
        IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            var client = new RestClient(uri);
            client.Timeout = timeout;
            var request = new RestRequest(Method.POST);
            var context = new CommunicationContext
            {
                Method = method,
                Uri = uri,
                IsRequestIntercept = isRequestIntercept,
                IsResponseIntercept = isResponseIntercept,
                TimeOut = timeout,
                RequestContent = requestContent,
                ContentType = contentType,
                AdditionalHeaders = additionalHeaders
            };
            if (this._httpInterceptConfiguration.OnBeforeRequest != null && isRequestIntercept)
            {
                request.OnBeforeRequest =(IHttp http)=> {
                    this._httpInterceptConfiguration.OnBeforeRequest(context, http);
                } ;
            }
            request.AddHeader("Content-Type", contentType ?? "application/json");
            Logger.Info(string.Format("Http Request: \r\n{0}", requestContent));
            if (!string.IsNullOrEmpty(contentType))
                request.AddParameter(contentType ?? "application/json", requestContent, ParameterType.RequestBody);
            if (additionalHeaders != null)
            {
                Logger.Info(string.Format("Http Request Header: \r\n{0}", JsonConvert.SerializeObject(additionalHeaders)));
                foreach (KeyValuePair<string, string> additionHeader in additionalHeaders)
                {
                    request.AddHeader(additionHeader.Key, additionHeader.Value);
                }
            }
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage) || response.ErrorException != null)
            {
                Logger.Error(response.ErrorMessage, response.ErrorException);
            }
            return response;
        }

        public IRestResponse GetResponse(CommunicationContext communicationContext) {
            return GetResponse(communicationContext.Method, communicationContext.Uri,
            communicationContext.IsRequestIntercept,
            communicationContext.IsResponseIntercept, communicationContext.TimeOut, communicationContext.RequestContent, communicationContext.ContentType,
            communicationContext.AdditionalHeaders);
        }
    }
}
