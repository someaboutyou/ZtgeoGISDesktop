using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Winform.Configuration
{
    public class WinformMenuConfiguration: IWinformMenuConfiguration
    {
        public ITypeList<MenuProvider> Providers { get; }

        public bool IsEnabled { get; set; }

        public WinformMenuConfiguration()
        {
            Providers = new TypeList<MenuProvider>();
            IsEnabled = true;
        }
    }
}
