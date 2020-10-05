using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public interface IToolbarDefinitionContext
    {
        /// <summary>
        /// 创建Toolbar 组
        /// </summary>
        /// <returns></returns>
        WinformToolbarGroup CreateToolbarGroup(string groupName,string text, string permission,
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, bool isDefaultShow =true);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        WinformToolbarGroup GetToolbarGroupOrNull(string groupName);

        void RemoveToolbarGroup(string name);
    }
}
