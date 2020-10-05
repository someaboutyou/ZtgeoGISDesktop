using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public class WinformToolbar
    {
        /// <summary>
        /// tools 菜单
        /// </summary>
        /// <param name="name">tool Name</param>
        /// <param name="icon">可用时的图标</param>
        /// <param name="disableIcon">不可用时的图标（当未null时，用可用时的图标）</param>
        /// <param name="defaultEnable">默认状态下是否可用</param>
        /// <param name="tip">提示语</param>
        /// <param name="group">所属组</param>
        /// <param name="permission">权限</param>
        /// <param name="multiTenancySides">所属多租户Side,Host or Tenant</param>
        /// <param name="toolbarEvent">toolbarEvent</param>
        public WinformToolbar(
            string name,
            Image icon,
            Image disableIcon,
            bool defaultEnable,
            string tip,
            WinformToolbarGroup group, 
            string permission =null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            Action<WinformToolbar> toolbarEvent = null
            ) {
            this.Name = name;
            this.Icon = icon;
            this.DisableIcon = disableIcon;
            this.DefaultEnable = defaultEnable;
            this.Tip = tip;
            this.ToolbarEvent = toolbarEvent;
            this.WinformToolbarGroup = group;
            this.MultiTenancySides = multiTenancySides;
            this.Permission = permission; 
        }
        public static string NameSplitKey = "-->";

        public WinformToolbarGroup WinformToolbarGroup { get; set; }
        public string PrefixedName { get { return this.WinformToolbarGroup.Name + NameSplitKey + this.Name; } }
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public Image Icon { get; set; }
        /// <summary>
        /// 不可用状态下的图标
        /// </summary>
        public Image DisableIcon { get; set; }
        /// <summary>
        /// 提示字符
        /// </summary>
        public string Tip { get; set; } 
        /// <summary>
        /// 权限Name 
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// UI 上展示的实体
        /// </summary>
        public object UIObject { get; set; }
        /// <summary>
        /// 默认是否可用
        /// </summary>
        public bool DefaultEnable { get; set; }
        /// <summary>
        /// 是否是激活状态 默认时false
        /// </summary>
        public bool IsActive { get; set; } = false;
        public MultiTenancySides MultiTenancySides { get; set; }
        /// <summary>
        /// toolbar 点击事件
        /// </summary>
        public Action<WinformToolbar> ToolbarEvent { get; set; }

    }
}
