using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.Configuration
{
    public class HybridConfiguration: IHybridConfiguration
    {
        public bool DisableWebViewExecutionTimeouts { get; set; }

        public bool ShowDeveloperTools { get; set; }

        public string CustomWebViewResourcePath { get; set; }
    }
}
