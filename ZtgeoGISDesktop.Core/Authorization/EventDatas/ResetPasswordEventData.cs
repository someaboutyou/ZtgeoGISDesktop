using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZtgeoGISDesktop.Core.Authorization.EventDatas
{
    /// <summary>
    /// 需要重置密码事件数据
    /// </summary>
    public class ResetPasswordEventData: EventData
    {
        public string UserNameOrEmailAddress { get; set; }
        public long UserId { get; set; } 
    }
}
