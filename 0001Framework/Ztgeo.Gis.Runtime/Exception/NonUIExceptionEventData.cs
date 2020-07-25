using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime
{
    public class NonUIExceptionEventData:EventData
    {
        public UnhandledExceptionEventArgs UnhandledExceptionEventArgs { get; set; }

        public ExceptionType ExceptionType { get; set; }
    }
}