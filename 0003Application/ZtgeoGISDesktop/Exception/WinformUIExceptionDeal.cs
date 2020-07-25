using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Runtime;

namespace ZtgeoGISDesktop
{
    public class WinformUIExceptionDeal: UIExceptionDealBase
    { 
        public WinformUIExceptionDeal(string message) : base(message)
        {
        }

        public WinformUIExceptionDeal(string message, Exception innerException) : base(message, innerException)
        {

        }
        public WinformUIExceptionDeal(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public override void DealException(ExceptionType exType,string logCode)
        {
            string showText = "程序处理过程中出现异常！Code:" + logCode;
            DevExpress.XtraEditors.XtraMessageBox.Show(showText, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }
    }
}
