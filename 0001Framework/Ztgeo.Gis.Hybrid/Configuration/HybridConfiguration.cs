using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.Configuration
{
    public class HybridConfiguration: IHybridConfiguration
    {
        public bool DisableWebViewExecutionTimeouts { get; set; } = false;

        public bool ShowDeveloperTools { get; set; } = true;

        public string CustomWebViewResourcePath { get; set; }
    }
}
