using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Login;

namespace Ztgeo.Gis.Runtime.Authorization
{
    public class ZtgeoAbpSession : AbpSessionBase
    {
        public ZtgeoAbpSession( 
            IMultiTenancyConfig multiTenancy, 
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base(
                  multiTenancy,
                  sessionOverrideScopeProvider)
        { 
        }
        public override long? UserId
        {
            get
            {
                if (LoginInfoCache.IsLogined)
                {
                    if (LoginInfoCache.AuthenticateResultModel != null)
                    {
                        return LoginInfoCache.AuthenticateResultModel.UserId;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        } 

        public override int? TenantId
        {
            get
            {
                return MultiTenancyConsts.DefaultTenantId;
            }
        } 

        public override long? ImpersonatorUserId { get;   }

        public override int? ImpersonatorTenantId { get;   }
         
    }
}
