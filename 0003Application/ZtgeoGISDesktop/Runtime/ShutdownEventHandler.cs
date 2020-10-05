using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Context;

namespace ZtgeoGISDesktop.Runtime
{
    /// <summary>
    /// 程序关闭执行事件
    /// </summary>
    public class ShutdownEventHandler : IEventHandler<AppShutdownEventData>, ITransientDependency
    {
        public ILogger Logger { get; set; }
         

        public ShutdownEventHandler( )
        {
            Logger = NullLogger.Instance; 
        }

        public void HandleEvent(AppShutdownEventData eventData)
        {
            Logger.Info("程序进程退出..."); 
            System.Environment.Exit(0);
        }
     }
}
