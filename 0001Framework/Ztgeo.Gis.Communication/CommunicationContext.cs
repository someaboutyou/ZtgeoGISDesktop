using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Communication
{
    public class CommunicationContext
    {
        public Uri Uri { get; set; }
        public Method Method { get; set; }
        public bool IsRequestIntercept { get; set; }

        public bool IsResponseIntercept { get; set; }

        public int TimeOut { get; set; }

        public string RequestContent { get; set; }

        public string ContentType { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AdditionalHeaders { get; set; }
    }
}
