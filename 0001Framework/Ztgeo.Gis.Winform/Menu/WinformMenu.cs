﻿using Abp.Application.Features;
using Abp.Authorization;
using Abp.MultiTenancy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.AbpExtension;
using Ztgeo.Gis.Winform.Actions;

namespace Ztgeo.Gis.Winform.Menu
{
    /// <summary>
    /// winform 菜单Bean
    /// </summary>
    public class WinformMenu
    {
        public WinformMenu(
            string name,
            MenuType menuType,
            string displayName = null,
            string description = null,
            string permission = null,
            Image icon = null,
            Image disIcon =null,
            int order =0,
            bool defaultEnable=true,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
            Dictionary<string,object> properties =null,
            IType<IMenuAction> menuActionType = null
         ) {
            Name = name;
            MenuType = menuType;
            DisplayName = displayName;
            Properties = properties;
            Description = description;
            Permission = permission;
            Icon = icon;
            DisIcon = disIcon;
            Order = order;
            MultiTenancySides = multiTenancySides;
            MenuActionType = menuActionType;
            DefaultEnable = defaultEnable;
            IsEnable = defaultEnable;
            _children = new List<WinformMenu>(); 
        }
        public WinformMenu Parent { get; private set; } 
        public MultiTenancySides MultiTenancySides { get; set; } 
        public string Permission { get; set; }
        public string Name { get; }
        public string DisplayName { get; set; } 
        public string Description { get; set; }  
        public int Order { get; set; } 
        /// <summary>
        /// 默认为可用状态
        /// </summary>
        public bool DefaultEnable { get; set; }
        /// <summary>
        /// 是否为可用状态
        /// </summary>
        public bool IsEnable { get; set; }
        public Dictionary<string, object> Properties { get; }
        /// <summary>
        /// 图标
        /// </summary>
        public Image Icon { get; set; }
        /// <summary>
        /// 不可用时的图标
        /// </summary>
        public Image DisIcon { get; set; }

        public MenuType MenuType { get; set; }

        /// <summary>
        /// 菜单导航分组
        /// </summary>
        public bool IsPage { get {return this.MenuType == MenuType.Page; }   }
        /// <summary>
        /// 是否时分组信息
        /// </summary>
        public bool IsGroup { get { return this.MenuType == MenuType.Group; } }
        /// <summary>
        /// 是否是导航按钮 
        /// </summary>
        public bool IsNavigation { get { return this.MenuType == MenuType.Navigation; } }
        /// <summary>
        /// 界面对象
        /// </summary>
        public object UIObject { get; set; }

        public IType<IMenuAction> MenuActionType { get; set; }
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
              MenuType menuType,
              string displayName = null,
              string description = null,
              string permission = null,
              Image icon =null,
              Image disIcon =null,
              int order = 0,
              bool defaultEnable = true,
              MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, 
              Dictionary<string, object> properties = null,
              IType<IMenuAction> menuActionType =null
         ) {
            WinformMenu menu = null;
            if (_children.Any(c => c.Name.Equals(name)))
            {
                menu = _children.First(c => c.Name.Equals(name));
                menu.Parent = this;
                return menu;
            }
            else
            {
                menu = new WinformMenu(name, menuType, displayName, description, permission, icon,disIcon, order, defaultEnable, multiTenancySides, properties, menuActionType);
                _children.Add(menu);
                menu.Parent = this;
                return menu;
            }
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
