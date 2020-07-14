using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Core.Share.Constants
{
    public static class HttpHeaderConstants
    {
        public static  string AuthTokenKeyName { get; private set; } = "Abp.AuthToken";
        public static string MutitenancyTenantIdKeyName { get; private set; } = "Abp.TenantId";


    }
}
