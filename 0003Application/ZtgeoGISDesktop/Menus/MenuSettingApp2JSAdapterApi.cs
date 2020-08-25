using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace ZtgeoGISDesktop.Menus
{
    public class MenuSettingApp2JSAdapterApi : App2JSAdapterApiBase
    {
        public override string JSBindObjectName { get; protected set; } = "MenuSettingApp2JSAdapterApi";

        public virtual void SetMenuSettingData(string data) { }
    }
}
