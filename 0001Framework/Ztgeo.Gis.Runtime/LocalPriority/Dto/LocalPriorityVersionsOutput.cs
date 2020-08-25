using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority.Dto
{
    /// <summary>
    /// 服务器上版本号返回数据
    /// </summary>
    public class LocalPriorityVersionsOutput
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 根据key获取
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 实体类型名称
        /// </summary>
        public string EntityName { get; set; }
    }
}
