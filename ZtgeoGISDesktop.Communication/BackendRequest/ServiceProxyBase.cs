using Abp.Configuration;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication;

namespace ZtgeoGISDesktop.Communication.BackendRequest
{
    public abstract class ServiceProxyBase
    {
        private readonly IRESTServices _restService;

        private readonly ISettingManager _settingManager;

        private string _controlUrl;
        public ServiceProxyBase(IocManager iocManager, string controlUrl)
        {
            _restService = iocManager.Resolve<IRESTServices>();
            _settingManager = iocManager.Resolve<ISettingManager>();
            _controlUrl = controlUrl;
        }

        protected Uri GetRequestUri(string actionName) {
            string scheme = _settingManager.GetSettingValue(BackendSettingNames.Scheme);
            string host= _settingManager.GetSettingValue(BackendSettingNames.Host);
            string port = _settingManager.GetSettingValue(BackendSettingNames.Port);
            string urlstring = scheme + "://" + host + ":" + port + "/" + _controlUrl + "/" + actionName;
            return new Uri(urlstring);
        }
    }
}
