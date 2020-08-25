using Abp.Domain.Services;
using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization;
using Ztgeo.Gis.Runtime.Authorization.Login;
using Ztgeo.Gis.Runtime.Authorization.Permissions;
using ZtgeoGISDesktop.Core.Authorization.EventDatas;
using ZtgeoGISDesktop.Core.Communication; 

namespace ZtgeoGISDesktop.Core.Authorization
{
    /// <summary>
    /// 系统授权管理
    /// </summary>
    public class AuthorizationManager :DomainService, IAuthorizationManager
    {
        private readonly ITokenAuthServiceProxy tokenAuthServiceProxy;
        public IEventBus EventBus { get; set; }
        public AuthorizationManager(ITokenAuthServiceProxy _tokenAuthServiceProxy) {
            tokenAuthServiceProxy = _tokenAuthServiceProxy;
            EventBus = NullEventBus.Instance;
        }
        /// <summary>
        /// 认证
        /// </summary>
        public AuthenticateResultModel Authorization(AuthenticateModel authenticateModel, bool isLogining)
        {
            AuthenticateResultModel authenticateResultModel = tokenAuthServiceProxy.Authenticate(authenticateModel);
            if (!isLogining) { //如果不是登录 
                
            }
            LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
            return authenticateResultModel;
        }

        public async Task<AuthenticateResultModel> AuthorizationAsync(AuthenticateModel authenticateModel, bool isLogining)
        {
            AuthenticateResultModel authenticateResultModel =await tokenAuthServiceProxy.AuthenticateAsync(authenticateModel);
            LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
            return authenticateResultModel;
        }

        public IList<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            return tokenAuthServiceProxy.GetAllPressions();
        }

        public async Task<IList<FlatPermissionWithLevelDto>> GetAllPermissionsAsync()
        {
            return await tokenAuthServiceProxy.GetAllPressionsAsync();
        }
    }
}
