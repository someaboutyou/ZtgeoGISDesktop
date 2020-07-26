using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ztgeo.Gis.Share;

namespace Ztgeo.Gis.Runtime 
{
    public class UIExceptionEventData : EventData
    {
        public ThreadExceptionEventArgs ThreadExceptionEventArgs { get; set; }

        public ExceptionType ExceptionType { get; set; }
    }
}
