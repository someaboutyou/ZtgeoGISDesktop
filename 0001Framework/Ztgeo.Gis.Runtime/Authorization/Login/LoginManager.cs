using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Permissions;
using Ztgeo.Gis.Runtime.Configuration;

namespace Ztgeo.Gis.Runtime.Authorization.Login
{
    public class LoginManager:DomainService, ILoginManager
    {
        private readonly IAuthorizationManager authorizationManager;
        private readonly RuntimeSetting runtimeSetting;
        private readonly IAbpSession abpSession;
        public LoginManager(IAuthorizationManager _authorizationManager, RuntimeSetting _runtimeSetting) {
            authorizationManager = _authorizationManager;
            runtimeSetting = _runtimeSetting;
        }
        public AuthenticateResultModel Login(AuthenticateModel authenticateModel,bool isServerless) {
            if (!isServerless)
            {
                AuthenticateResultModel authenticateResultModel = authorizationManager.Authorization(authenticateModel, true);
                if (authenticateResultModel != null && !authenticateResultModel.AccessToken.IsNullOrEmpty())
                {
                    LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                    IEnumerable<FlatPermissionWithLevelDto> premissions = authorizationManager.GetAllPermissions();
                    LoginInfoCache.SetPermissions(premissions);
                    runtimeSetting.RuntimeStatus = RuntimeStatus.Common;
                    Logger.Info("login success!");
                    return authenticateResultModel;
                }
                Logger.Warn("login fail!" + JsonConvert.SerializeObject(authenticateResultModel));
                return authenticateResultModel;
            }
            else { //脱机登录 
                runtimeSetting.RuntimeStatus = RuntimeStatus.Serverless;
                var authenticateResultModel= new AuthenticateResultModel { AccessToken = "Serverless", UserId= ServerlessConfig.ServerlessUserId };
                LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                return authenticateResultModel;
            }
        } 

        public async Task<AuthenticateResultModel> LoginAsync(AuthenticateModel authenticateModel, bool isServerless)
        {
            if (!isServerless)
            {
                AuthenticateResultModel authenticateResultModel = await authorizationManager.AuthorizationAsync(authenticateModel, true);
                if (!authenticateResultModel.AccessToken.IsNullOrEmpty())
                {
                    LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                    IEnumerable<FlatPermissionWithLevelDto> premissions = await authorizationManager.GetAllPermissionsAsync();
                    LoginInfoCache.SetPermissions(premissions);
                    Logger.Info("login success!");
                    return authenticateResultModel;
                }
                Logger.Warn("login fail!" + JsonConvert.SerializeObject(authenticateResultModel));
                return authenticateResultModel;
            }
            else
            { //脱机登录 
                runtimeSetting.RuntimeStatus = RuntimeStatus.Serverless;
                var authenticateResultModel = new AuthenticateResultModel { AccessToken = "Serverless" };
                LoginInfoCache.SetAuthenticateModelAndAuthenticateResultModel(authenticateModel, authenticateResultModel);
                return authenticateResultModel;
            }
        }

        public bool IsLogined() {
            return LoginInfoCache.IsLogined;
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
