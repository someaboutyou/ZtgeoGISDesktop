using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Share.Events
{
    public class WinFormMenuStatusChangeEventData: EventData
    {
        public WinformMenu WinFormMenu { get; set; }

        public MenuStatus MenuStatus { get; set; }
    }
}
