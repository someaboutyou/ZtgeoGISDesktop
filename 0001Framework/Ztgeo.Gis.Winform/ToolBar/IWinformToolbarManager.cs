using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public interface IWinformToolbarManager
    {
        WinformToolbarGroup GetToolbarGroup(string groupName);
        WinformToolbarGroup GetToolbarGroupOrNull(string groupName);
        IReadOnlyList<WinformToolbarGroup> GetAllToolbarGroups(bool tenancyFilter = true);
        IReadOnlyList<WinformToolbarGroup> GetAllToolbarGroups(MultiTenancySides multiTenancySides);

        IList<WinformToolbar> GetToolbarsByGroups(WinformToolbarGroup group, bool tenancyFilter = true);
        IList<WinformToolbar> GetToolbarsByGroups(string groupName, bool tenancyFilter = true);
        IList<WinformToolbar> GetToolbarsByGroups(string groupName, MultiTenancySides multiTenancySides);

        WinformToolbar GetToolbarByProfixedName(string profixedName);
    }
}
