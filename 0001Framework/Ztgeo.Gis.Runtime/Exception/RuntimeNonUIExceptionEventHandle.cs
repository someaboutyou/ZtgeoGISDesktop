using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime
{
    public class RuntimeNonUIExceptionEventHandle : IEventHandler<NonUIExceptionEventData>
    {
        public ILogger Logger { get; set; }


        public RuntimeNonUIExceptionEventHandle()
        {
            Logger = NullLogger.Instance;
        }
        public void HandleEvent(NonUIExceptionEventData eventData)
        {
            if (eventData != null && eventData.UnhandledExceptionEventArgs != null)
            {
                var exception = eventData.UnhandledExceptionEventArgs.ExceptionObject;
                string errorCode = (Convert.ToDouble(DateTime.UtcNow.Ticks - 621355968000000000) / (10 * 1000 * 1000)).ToString();
                if (exception is ExceptionDealBase)
                {
                    Logger.Error("ErrorCode:" + errorCode + "。" + ((ExceptionDealBase)exception).Message, (Exception)exception);
                    ((ExceptionDealBase)exception).DealException(eventData.ExceptionType, errorCode);
                }
                else
                {
                    Logger.Error("ErrorCode:" + errorCode + "。" + ((ExceptionDealBase)exception).Message, (Exception)exception);
                }
            }
        }
    }
}
