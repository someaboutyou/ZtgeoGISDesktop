using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Share.Constants
{
    public  static class Abp
    {
        public static  string AuthTokenKeyName { get; private set; } = "Abp.AuthToken";
        public static string MutitenancyTenantIdKeyName { get; private set; } = "Abp.TenantId";


    }
}
