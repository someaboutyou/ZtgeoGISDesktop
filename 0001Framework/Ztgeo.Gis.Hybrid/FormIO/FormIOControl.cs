using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.HybridUserControl;

namespace Ztgeo.Gis.Hybrid.FormIO
{
    public class FormIOControl : BaseHybridControl<FormIOApp2JSAdapterApi, FormIOJs2AppAdapterApi>
    {
        public FormIOControl(IocManager iocManager
            ) : base(iocManager)
        {
            //SetFormIOComponentData();
            js2AppAdapterApi.Host = this;
        }

        public void SetFormIOComponentAndData(string component,string data) {
            this.app2JSAdapterApi.SetFormIOComponentAndData(component, data);
        }

        public Action<string> OnSave { get; set; }
         
    }
}
