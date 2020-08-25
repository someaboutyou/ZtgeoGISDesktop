using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid.HybridUserControl;

namespace ZtgeoGISDesktop.Winform.Share.Forms
{
    public class DialogHybirdForm<TIHybridControl>:Form where TIHybridControl: IHybridControl
    {
        private readonly IocManager iocManager;
        public IHybridControl hybridControl { get; private set; }
        public DialogHybirdForm(IocManager _iocManager,Assembly assembly,string[] path) {
            iocManager = _iocManager;
            hybridControl = (IHybridControl)(iocManager.Resolve(typeof(TIHybridControl)));
            hybridControl.LoadResource(assembly, path);
        }
    }
}
