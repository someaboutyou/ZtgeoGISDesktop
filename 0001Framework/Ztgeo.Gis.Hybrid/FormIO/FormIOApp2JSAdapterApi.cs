using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace Ztgeo.Gis.Hybrid.FormIO
{
    public class FormIOApp2JSAdapterApi : App2JSAdapterApiBase
    {
        public override string JSBindObjectName { get; protected set; } = "FormIOApp2JSAdapterApi";

        /// <summary>
        /// 设置FormIO展示组件数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetFormIOComponentAndData(string component,string data) { }
    }
}
