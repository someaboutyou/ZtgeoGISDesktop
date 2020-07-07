using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Core.Authorization.Local
{
    /// <summary>
    /// 本地存储的认证信息
    /// </summary>
    public class LocalAuthInfo:Entity<int>
    {
        public string UserNameOrEmailAddress { get; set; } 
        public string TenantId { get; set; }
        public string AccessToken { get; set; }
        public int ExpireInSeconds { get; set; }

    }
}
