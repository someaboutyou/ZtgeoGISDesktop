using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZtgeoGISDesktop.Core.Share.AjaxModels.TokenAuth;

namespace ZtgeoGISDesktop.Core.Authorization
{
    public interface IAuthorizationManager
    {
        bool Authorization(AuthenticateModel authenticateModel);
    }
}
