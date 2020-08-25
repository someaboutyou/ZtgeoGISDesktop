using Abp.Domain.Services;
using Abp.Extensions;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Permissions;

namespace Ztgeo.Gis.Runtime.Authorization.Login
{
    public class LoginManager:DomainService, ILoginManager
    {
        private readonly IAuthorizationManager authorizationManager; 
        public LoginManager(IAuthorizationManager _authorizationManager) {
            authorizationManager = _authorizationManager;
        }
        public AuthenticateResultModel Login(AuthenticateModel authenticateModel) {
            AuthenticateResultModel authenticateResultModel = authorizationManager.Authorization(authenticateModel,true);
            if (!authenticateResultModel.AccessToken.IsNullOrEmpty()) {
                LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                IList<FlatPermissionWithLevelDto> premissions = authorizationManager.GetAllPermissions();
                LoginInfoCache.SetPermissions(premissions);
                Logger.Info("login success!");
                return authenticateResultModel;
            }
            Logger.Warn("login fail!"+JsonConvert.SerializeObject(authenticateResultModel));
            return authenticateResultModel;
        } 

        public async Task<AuthenticateResultModel> LoginAsync(AuthenticateModel authenticateModel)
        {
            AuthenticateResultModel authenticateResultModel =await authorizationManager.AuthorizationAsync(authenticateModel,true);
            if (!authenticateResultModel.AccessToken.IsNullOrEmpty())
            {
                LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                IList<FlatPermissionWithLevelDto> premissions = await authorizationManager.GetAllPermissionsAsync();
                LoginInfoCache.SetPermissions(premissions);
                Logger.Info("login success!");
                return authenticateResultModel;
            }
            Logger.Warn("login fail!" + JsonConvert.SerializeObject(authenticateResultModel));
            return authenticateResultModel;
        }

        public bool IsLogined() {
            return LoginInfoCache.AuthenticateModel != null && LoginInfoCache.AuthenticateResultModel != null;
        }

        public void Logout() {
            LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(null, null);
            LoginInfoCache.SetPermissions(null);
        }

        public async Task LogoutAsync() {
            await Task.Run(() => {
                LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(null, null);
                LoginInfoCache.SetPermissions(null);
            });
        }
    }
}
