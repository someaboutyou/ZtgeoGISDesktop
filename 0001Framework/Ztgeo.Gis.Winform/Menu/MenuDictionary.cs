using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public class MenuDictionary: Dictionary<string , WinformMenu>
    {
        public virtual void AddAllMenus() {
            foreach (var menu in Values.ToList()) {
                AddMenuRecursively(menu);
            }
        }

        private void AddMenuRecursively(WinformMenu menu) {
            WinformMenu existingMenu;
            if(TryGetValue(menu.Name,out existingMenu))
            {
                if (existingMenu != menu)
                {
                    throw new AbpInitializationException("Duplicate menu name detected for " + existingMenu.Name);
                }
            }
            else
            {
                this[menu.Name] = menu;
            }

            //Add child permissions (recursive call)
            foreach (var childMenu in menu.Children)
            {
                AddMenuRecursively(childMenu);
            }
        }
    }
}
