using Abp;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Communication; 
using ZtgeoGISDesktop.Core.Communication;
using ZtgeoGISDesktop.Core.Share.AjaxModels.TokenAuth;

namespace ZtgeoGISDesktop.Communication.BackendRequest
{ 
    public class TokenAuthServiceProxy : ServiceProxyBase,ITokenAuthServiceProxy
    {
        private readonly IRESTServices _restService; 
        
        public TokenAuthServiceProxy( IocManager iocManager):base(iocManager, "api/TokenAuth") { 

        }
        public AuthenticateResultModel Authenticate(AuthenticateModel authenticateModel) {
            return _restService.Post<AuthenticateResultModel>(this.GetRequestUri("Authenticate"), authenticateModel);
        }
    }
}
