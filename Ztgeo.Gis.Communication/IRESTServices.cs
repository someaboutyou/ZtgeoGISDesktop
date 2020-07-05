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
        string Get(Uri url, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        OutModel Get<OutModel>(Uri url, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        string Post(Uri uri, string requestContent, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);

        OutModel Post<OutModel>(Uri uri, object inputModel, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null);
    }
}
