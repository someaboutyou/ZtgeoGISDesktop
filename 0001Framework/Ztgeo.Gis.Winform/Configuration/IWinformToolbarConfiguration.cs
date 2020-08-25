using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.ToolBar;

namespace Ztgeo.Gis.Winform.Configuration
{
    public interface IWinformToolbarConfiguration
    {
        ITypeList<ToolbarProvider> Providers { get; }

        bool IsEnabled { get; set; }
    }
}
