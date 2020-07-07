using Abp.Dependency;
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
    }
}
