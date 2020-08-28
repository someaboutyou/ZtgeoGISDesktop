using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication.CommunicationException
{
    public class HttpRequestException : Exception
    {
        public HttpRequestException(IRestResponse restResponse):base() {
            RestResponse = restResponse;
        }
        public HttpRequestException(IRestResponse restResponse,string message) : base(message)
        {
            RestResponse = restResponse;
        }
        public HttpRequestException(IRestResponse restResponse, string message,Exception exception) : base(message, exception)
        {
            RestResponse = restResponse;
        } 
        public IRestResponse RestResponse { get;set;}
    }
}
