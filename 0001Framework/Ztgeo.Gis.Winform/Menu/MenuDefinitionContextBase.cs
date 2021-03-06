﻿using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public abstract class MenuDefinitionContextBase :IMenuDefinitionContext
    {
        protected readonly MenuDictionary Menus;

        protected MenuDefinitionContextBase() {
            Menus = new MenuDictionary();
        }

        public WinformMenu CreateMenu(string name,
            MenuType menuType,
            string displayName = null,
            string description = null,
            string permission = null,
            Image icon =null, 
            Image disIcon =null,
            int order=0,
            bool isDefaultEnable =true,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            Dictionary<string, object> properties = null)
        {
            if (Menus.ContainsKey(name))
            {
                //throw new AbpException("There is already a permission with name: " + name);
                return Menus[name];
            }
            var menu = new WinformMenu(name, menuType, displayName, description, permission, icon, disIcon, order, isDefaultEnable, multiTenancySides, properties);
            Menus[menu.Name] = menu;
            return menu;
        }
         

        public virtual WinformMenu GetMenuOrNull(string name)
        {
            return Menus.GetOrDefault(name);
        }

        public virtual void RemoveMenu(string name)
        {
            Menus.Remove(name);
        }
    }
}