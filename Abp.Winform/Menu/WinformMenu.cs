using Abp.Application.Features;
using Abp.Authorization;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public class WinformMenu
    {
        public WinformMenu(
            string name,
            string displayName = null,
            string description =null,
            string permission = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
            Dictionary<string,object> properties =null
         ) {
            Name = name;
            DisplayName = displayName;
            Properties = properties;
            Description = description;
            Permission = permission;
            MultiTenancySides = multiTenancySides; 
            _children = new List<WinformMenu>();
        }
        public WinformMenu Parent { get; private set; } 
        public MultiTenancySides MultiTenancySides { get; set; } 
        public string Permission { get; set; }
        public string Name { get; }
        public string DisplayName { get; set; } 
        public string Description { get; set; }
        public Dictionary<string, object> Properties { get; }
         
        public object this[string key]
        {
            get => !Properties.ContainsKey(key) ? null : Properties[key];
            set
            {
                Properties[key] = value;
            }
        }
        public IReadOnlyList<WinformMenu> Children => _children.ToImmutableList();

        private readonly List<WinformMenu> _children;

        public WinformMenu CreateChildMenu(
              string name,
              string displayName = null,
              string description = null,
              string permission = null,
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
              Dictionary<string, object> properties = null
         ) {
            var menu = new WinformMenu(name, displayName, description, permission, multiTenancySides, properties);
            _children.Add(menu);
            return menu;
        }

        public void RemoveChildMenu(string name) {
            _children.RemoveAll(m => m.Name.Equals(name));
        }

        public override string ToString()
        {
            return string.Format("[Menu: {0}]", Name);
        }
    }
}
