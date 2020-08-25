using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.Winform.Configuration
{
    public class WinformToolbarConfiguration: IWinformToolbarConfiguration
    {
        public ITypeList<ToolbarProvider> Providers { get; }

        public bool IsEnabled { get; set; }

        public WinformToolbarConfiguration() {
            Providers = new TypeList<ToolbarProvider>();
            IsEnabled = true;
        }
    }
}
