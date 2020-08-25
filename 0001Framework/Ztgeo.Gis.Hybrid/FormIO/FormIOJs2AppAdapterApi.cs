using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace Ztgeo.Gis.Hybrid.FormIO
{
    public class FormIOJs2AppAdapterApi: Js2AppAdapterApiBase
    {
        public override string AppBindObjectName { get; protected set; } = "FormIOJs2AppAdapterApi";

        public FormIOControl Host { get; set; }
        public virtual void OnSave(string SaveData) {
            Host.OnSave?.Invoke(SaveData);
        }
    }
}
