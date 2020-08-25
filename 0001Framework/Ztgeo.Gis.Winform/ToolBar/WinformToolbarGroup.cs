using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    /// <summary>
    /// toolbar 组
    /// </summary>
    public class WinformToolbarGroup
    {
        public WinformToolbarGroup(
            string name,
            string text,
            string permission,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant
            ) {
            this.Name = name;
            this.Text = text;
            this.Permission = permission;
            this.MultiTenancySides = multiTenancySides;
        }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Permission { get; set; }
        public MultiTenancySides MultiTenancySides { get; set; }

        public IList<WinformToolbar> ToolBars { get; set; }

        public object UIObject { get; set; }

        public void AddToolbar(WinformToolbar toolbar) {
            if (this.ToolBars == null) {
                this.ToolBars = new List<WinformToolbar>();
            }
            this.ToolBars.Add(toolbar);
        } 


    }
}
