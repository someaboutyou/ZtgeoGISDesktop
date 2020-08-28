using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Permissions;

namespace Ztgeo.Gis.Runtime.Authorization
{
    public interface IAuthorizationManager:Abp.Dependency.ITransientDependency
    {
        /// <summary>
        /// 登录 认证
        /// </summary>
        /// <param name="authenticateModel"></param>
        /// <param name="isLogining">是否正在登录</param>
        /// <returns></returns>
        AuthenticateResultModel Authorization(AuthenticateModel authenticateModel,bool isLogining);
        Task<AuthenticateResultModel> AuthorizationAsync(AuthenticateModel authenticateModel, bool isLogining);
        /// <summary>
        /// 获得所有权限
        /// </summary>
        /// <returns></returns>
        IEnumerable<FlatPermissionWithLevelDto> GetAllPermissions();
        Task<IEnumerable<FlatPermissionWithLevelDto>> GetAllPermissionsAsync();
    }
}
