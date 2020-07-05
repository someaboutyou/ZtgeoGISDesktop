using Abp.Application.Features;
using Abp.Authorization;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public interface IMenuDefinitionContext
    {
        WinformMenu CreateMenu(
           string name,
           string displayName = null,
           string description = null,
           Permission permission = null,
           MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
           Dictionary<string, object> properties = null
           );

        WinformMenu GetPermissionOrNull(string name);
         
        void RemovePermission(string name);
    }
}
