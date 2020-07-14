using Abp.Dependency;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication
{
    public interface IRESTServices: ISingletonDependency
    {
        string Get(Uri url, bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        OutModel Get<OutModel>(Uri url, bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        string Post(Uri uri, string requestContent, bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        OutModel Post<OutModel>(Uri uri, object inputModel, bool isRequestIntercept = true,
            bool isResponseIntercept = true, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        IRestResponse GetResponse(Method method, Uri uri,
           bool isRequestIntercept = true,
           bool isResponseIntercept = true, int timeout = 30, string requestContent = null, string contentType = null,
       IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);
        IRestResponse GetResponse(CommunicationContext communicationContext);
    }
}
