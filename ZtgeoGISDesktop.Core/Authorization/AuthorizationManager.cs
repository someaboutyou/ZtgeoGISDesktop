using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Core.Authorization.EventDatas;
using ZtgeoGISDesktop.Core.Communication;
using ZtgeoGISDesktop.Core.Share.AjaxModels.TokenAuth;

namespace ZtgeoGISDesktop.Core.Authorization
{
    /// <summary>
    /// 系统授权管理
    /// </summary>
    public class AuthorizationManager : IAuthorizationManager
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
        public bool Authorization(AuthenticateModel authenticateModel) {
            AuthenticateResultModel authenticateResultModel = tokenAuthServiceProxy.Authenticate(authenticateModel);
            if (authenticateResultModel.ShouldResetPassword) {  //重设密码
                EventBus.Trigger<ResetPasswordEventData>(new ResetPasswordEventData
                {
                    UserNameOrEmailAddress = authenticateModel.UserNameOrEmailAddress,
                    UserId = authenticateResultModel.UserId
                });
                return false;
            }
            return true;
        }
    }
}
