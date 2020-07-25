using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime 
{
    /// <summary>
    /// 处理UI线程异常
    /// </summary>
    public class RuntimeUIExceptionEventHandle : IEventHandler<UIExceptionEventData>, ITransientDependency
    {  
        public ILogger Logger { get; set; }
         

        public RuntimeUIExceptionEventHandle() { 
            Logger = NullLogger.Instance; 
        } 

        public void HandleEvent(UIExceptionEventData eventData) {
            if (eventData != null && eventData.ThreadExceptionEventArgs != null && eventData.ThreadExceptionEventArgs.Exception!=null) {
                var exception = eventData.ThreadExceptionEventArgs.Exception;
                string errorCode = (Convert.ToDouble(DateTime.UtcNow.Ticks - 621355968000000000) / (10 * 1000 * 1000)).ToString();
                if (exception is UIExceptionDealBase)
                { 
                    Logger.Error("ErrorCode:" + errorCode + "。" + exception.Message, exception);
                    ((UIExceptionDealBase)exception).DealException(eventData.ExceptionType, errorCode);
                }
                else {
                    Logger.Error("ErrorCode:" + errorCode + "。" + exception.Message, exception);
                }
            }
        }
    }
}
