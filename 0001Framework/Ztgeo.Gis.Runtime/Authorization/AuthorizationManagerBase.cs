using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Permissions;

namespace Ztgeo.Gis.Runtime.Authorization
{
    public class DefaultAuthorizationManager  : IAuthorizationManager
    { 
        public ILogger Logger { get; set; }
        public DefaultAuthorizationManager() {
            Logger = NullLogger.Instance;
        }
        public AuthenticateResultModel Authorization(AuthenticateModel authenticateModel,bool isLogining) {
            Logger.Warn("IAuthorizationManager was not inheritance.未实现IAuthorizationManager");
            return new AuthenticateResultModel { };
        }

        public IList<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            Logger.Warn("IAuthorizationManager was not inheritance.未实现IAuthorizationManager");
            return new List<FlatPermissionWithLevelDto>();
        }

        public async Task<AuthenticateResultModel> AuthorizationAsync(AuthenticateModel authenticateModel, bool isLogining) {
            Logger.Warn("IAuthorizationManager was not inheritance.未实现IAuthorizationManager");
            return await Task.Run(()=>new AuthenticateResultModel { });
        }

        public async Task<IList<FlatPermissionWithLevelDto>> GetAllPermissionsAsync() {
            Logger.Warn("IAuthorizationManager was not inheritance.未实现IAuthorizationManager");
            return await Task.Run(() => new List<FlatPermissionWithLevelDto> { });
        }
    }
}
