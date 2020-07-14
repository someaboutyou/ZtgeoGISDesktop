using Abp.Application.Features;
using Abp.Authorization;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
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
            bool isPage,
            bool isGroup,
            bool isNavigation, 
            string displayName = null,
            string description = null,
            string permission = null,
            Image icon = null,
            int order =0,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
            Dictionary<string,object> properties =null,
            Action<WinformMenu> menuEvent=null
         ) {
            Name = name;
            IsPage = isPage;
            IsGroup= isGroup;
            IsNavigation = isNavigation;
            DisplayName = displayName;
            Properties = properties;
            Description = description;
            Permission = permission;
            Icon = icon;
            Order = order;
            MultiTenancySides = multiTenancySides; 

            _children = new List<WinformMenu>();

        }
        public WinformMenu Parent { get; private set; } 
        public MultiTenancySides MultiTenancySides { get; set; } 
        public string Permission { get; set; }
        public string Name { get; }
        public string DisplayName { get; set; } 
        public string Description { get; set; }  
        public int Order { get; set; }
        public Dictionary<string, object> Properties { get; }
        /// <summary>
        /// 图标
        /// </summary>
        public Image Icon { get; set; }
        /// <summary>
        /// 菜单导航分组
        /// </summary>
        public bool IsPage { get; set; }
        /// <summary>
        /// 是否时分组信息
        /// </summary>
        public bool IsGroup { get; set; }
        /// <summary>
        /// 是否是导航按钮 
        /// </summary>
        public bool IsNavigation { get; set; }
         /// <summary>
         /// 界面对象
         /// </summary>
        public object UIObject { get; set; }

        public Action<WinformMenu> MenuEvent { get; set; }
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
              bool isPage,
              bool isGroup,
              bool isNavigation,
              string displayName = null,
              string description = null,
              string permission = null,
              Image icon =null,
              int order = 0,
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
              Dictionary<string, object> properties = null
         ) {
            var menu = new WinformMenu(name, isPage, isGroup, isNavigation, displayName, description, permission, icon, order, multiTenancySides, properties);
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
