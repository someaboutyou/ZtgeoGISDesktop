using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public interface IWinformMenuManager
    {
        WinformMenu GetMenu(string name);
        WinformMenu GetMenuOrNull(string name);
        IReadOnlyList<WinformMenu> GetAllMenus(bool tenancyFilter = true);
        IReadOnlyList<WinformMenu> GetAllMenus(MultiTenancySides multiTenancySides);
    }
}
