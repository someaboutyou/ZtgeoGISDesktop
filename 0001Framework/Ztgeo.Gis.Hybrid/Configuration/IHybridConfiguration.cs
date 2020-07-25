using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.Configuration
{
    public interface IHybridConfiguration
    {
        bool DisableWebViewExecutionTimeouts { get; set; }
        bool ShowDeveloperTools { get; set; }
        string CustomWebViewResourcePath { get; set; }
    }
}
