using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Core.Authorization.EventDatas
{
    public class HttpResponseErrorEventData : EventData
    {
        public string Messsage { get; set; }

        public string Details { get; set; }
    }
}
