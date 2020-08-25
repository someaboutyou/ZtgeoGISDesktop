using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{ 
    public class MenuOrderSetting
    {
        public string MenuId { get; set; }
        /// <summary>
        /// MenuName
        /// </summary>
        public string MenuName { get; set; }

        public string MenuKey { get; set; }

        public string ParentMenuKey { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string MenuDescription { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Order { get; set; }
    }
}
