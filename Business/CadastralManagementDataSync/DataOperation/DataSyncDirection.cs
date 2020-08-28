using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastralManagementDataSync.DataOperation
{
    public enum DataSyncDirection
    {
        /// <summary>
        /// 内网数据同步
        /// </summary>
        InnerDataSync,
        /// <summary>
        /// 外网数据同步
        /// </summary>
        OuterDataSync
    }
}
