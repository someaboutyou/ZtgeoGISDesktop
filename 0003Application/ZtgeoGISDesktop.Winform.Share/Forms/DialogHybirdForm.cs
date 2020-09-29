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
        private bool _isDisposed = false;

        private readonly IocManager iocManager;
        public IHybridControl hybridControl { get; private set; } 
        public DialogHybirdForm(IocManager _iocManager,Assembly assembly,string[] path) {
            iocManager = _iocManager;
            hybridControl = (IHybridControl)(iocManager.Resolve(typeof(TIHybridControl)));
            this.Controls.Add(hybridControl as Control);
            hybridControl.Dock = DockStyle.Fill;
            hybridControl.LoadResource(assembly, path); 
        }

        private void InitializeComponent()
        { 
            this.SuspendLayout();
            // 
            // DialogHybirdForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261); 
            this.Name = "DialogHybirdForm";
            this.ResumeLayout(false);

        } 
    }
}
