using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid.JsBinder;

namespace ZtgeoGISDesktop.Test
{
    public class TestJs2AppAdapterApi : Js2AppAdapterApiBase
    {
        public void OnMessageBox(string message) {
            MessageBox.Show(message);
        }
         
    }
}
