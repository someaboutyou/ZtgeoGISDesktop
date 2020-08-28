using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.Authorization.Login
{
    /// <summary>
    /// 登录管理
    /// </summary>
    public interface ILoginManager
    {
        /// <summary>
        /// 获取本地的登录信息
        /// </summary>
        AuthenticateResultModel Login(AuthenticateModel authenticateModel,bool isServerless);

        Task<AuthenticateResultModel> LoginAsync(AuthenticateModel authenticateModel, bool isServerless);
        /// <summary>
        /// 登出
        /// </summary>
        void Logout();
        Task LogoutAsync();
        /// <summary>
        /// 获取是否已经登录
        /// </summary>
        /// <returns></returns>
        bool IsLogined();
    }
}
