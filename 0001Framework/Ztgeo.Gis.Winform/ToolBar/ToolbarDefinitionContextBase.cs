using Abp.MultiTenancy;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public abstract class ToolbarDefinitionContextBase:IToolbarDefinitionContext
    {
        public Dictionary<string, WinformToolbarGroup> ToolbarGroups;
        public ToolbarDefinitionContextBase() {
            ToolbarGroups = new Dictionary<string, WinformToolbarGroup>();
        }

        public WinformToolbarGroup CreateToolbarGroup(string name,string text,string permission, 
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant) {
            if (ToolbarGroups.ContainsKey(name))
            {
                return ToolbarGroups[name];
            }
            else {
                WinformToolbarGroup winformToolbarGroup = new WinformToolbarGroup( name, text, permission, multiTenancySides); 
                ToolbarGroups.Add(name, winformToolbarGroup);
                return winformToolbarGroup;
            }
        }

        public WinformToolbarGroup GetToolbarGroupOrNull(string groupName) {
            if (ToolbarGroups.ContainsKey(groupName))
            {
                return ToolbarGroups[groupName];
            }
            else {
                return null;
            }
        }

        public void RemoveToolbarGroup(string name) {
            ToolbarGroups.Remove(name);
        }
    }
}
