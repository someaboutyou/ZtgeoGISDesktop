using Abp.Configuration;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication;
using ZtgeoGISDesktop.Communication.Configuration;

namespace ZtgeoGISDesktop.Communication.BackendRequest
{
    public abstract class ServiceProxyBase
    {
        protected readonly IRESTServices _restService;

        private readonly CommunicationSettings _communicationSettings;

        private string _controlUrl;
        public ServiceProxyBase(IocManager iocManager, string controlUrl)
        {
            _restService = iocManager.Resolve<IRESTServices>();
            _communicationSettings = iocManager.Resolve<CommunicationSettings>();
            _controlUrl = controlUrl;
        }

        protected Uri GetRequestUri(string actionName) {
            string scheme = _communicationSettings.BackendScheme;
            string host= _communicationSettings.BackendHost;
            string port = _communicationSettings.BackendPort;
            string urlstring = scheme + "://" + host + ":" + port + "/" + _controlUrl + "/" + actionName;
            return new Uri(urlstring);
        }
    }
}
