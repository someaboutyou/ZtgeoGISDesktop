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
        public override string AppBindObjectName { get; protected set; } = "TestJs2AppAdapterApi";

        /// <summary>
        /// 代理js 传来的方法，注意第一个字母要求小写
        /// </summary>
        /// <param name="message"></param>
        public void onMessageBox(string message) {
            MessageBox.Show(message);
        }
         
    }
}
