using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace ZtgeoGISDesktop.Test
{
    public class TestApp2JSAdapterApi: App2JSAdapterApiBase
    {
        public override string JSBindObjectName { get; protected set; } = "TestApp2JSAdapterApi";

        public virtual void AlertMessage(string message) { }
         
    }
}
