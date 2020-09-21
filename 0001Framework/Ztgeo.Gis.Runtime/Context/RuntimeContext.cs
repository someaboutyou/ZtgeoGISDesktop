using Abp.Events.Bus;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.Context
{
    /// <summary>
    /// 程序运行上下文。
    /// 在此上下文中，管理着MainForm、后台运行程序和程序本身的一些状态。
    /// </summary>
    public class RuntimeContext:Abp.Dependency.ISingletonDependency
    { 
        private readonly IAbpModuleManager _abpModuleManager;
        public RuntimeContext(IAbpModuleManager abpModuleManager) {
            _abpModuleManager = abpModuleManager;
        }
        public void ShutdownImmediate() {
            _abpModuleManager.ShutdownModules();
            EventBus.Default.Trigger<AppShutdownEventData>(new AppShutdownEventData { });
        }
        public void Shutdown() { 
            // 判断有没有后台进程在工作
        }
    }
}
