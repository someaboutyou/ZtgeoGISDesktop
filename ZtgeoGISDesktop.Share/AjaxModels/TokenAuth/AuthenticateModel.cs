using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Share.AjaxModels.TokenAuth
{
    public class AuthenticateModel
    {
        [Required] 
        public string UserNameOrEmailAddress { get; set; }

        [Required] 
        public string Password { get; set; }

        public string TwoFactorVerificationCode { get; set; }

        public bool RememberClient { get; set; }

        public string TwoFactorRememberClientToken { get; set; }

        public bool? SingleSignIn { get; set; }

        public string ReturnUrl { get; set; }

        public string TenantId { get; set; }
    }
}
