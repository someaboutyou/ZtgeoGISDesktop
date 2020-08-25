using Abp;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using Ztgeo.Gis.Communication;
using Ztgeo.Gis.Runtime.Authorization;
using Ztgeo.Gis.Runtime.Authorization.Permissions;
using ZtgeoGISDesktop.Core.Communication; 

namespace ZtgeoGISDesktop.Communication.BackendRequest
{ 
    public class TokenAuthServiceProxy : ServiceProxyBase,ITokenAuthServiceProxy
    {
        private readonly IPermissionServiceProxy permissionServiceProxy;
        public TokenAuthServiceProxy(IocManager iocManager, IPermissionServiceProxy _permissionServiceProxy) :base(iocManager, "api/TokenAuth") {
            permissionServiceProxy = _permissionServiceProxy;
        }
        public AuthenticateResultModel Authenticate(AuthenticateModel authenticateModel) {
            return _restService.Post<AuthenticateResultModel>(this.GetRequestUri("Authenticate"), authenticateModel, false, false); // false false 说明请求不拦截
        }

        public async Task<AuthenticateResultModel> AuthenticateAsync(AuthenticateModel authenticateModel)
        {
            return await _restService.PostAsync<AuthenticateResultModel>(this.GetRequestUri("Authenticate"), authenticateModel, false, false); // false false 说明请求不拦截
        }

        public IList<FlatPermissionWithLevelDto> GetAllPressions()
        {
            return permissionServiceProxy.GetAllPressions();
        }

        public async Task<IList<FlatPermissionWithLevelDto>> GetAllPressionsAsync()
        {
            return await permissionServiceProxy.GetAllPressionsAsync();
        }
    }
}
 