using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.MainFormStatusBar
{
    /// <summary>
    /// 状态信息
    /// </summary>
    public class StatusInfo
    {
        public string Message { get; set; }

        public StatusShowType StatusShowType { get; set; }

        public int MaxValue { get; set; }

        public int CurrentValue { get; set; }
    }
}
