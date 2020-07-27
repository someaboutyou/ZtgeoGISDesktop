using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Runtime;
using Ztgeo.Gis.Share;

namespace ZtgeoGISDesktop
{
    public class DefaultExceptionDeal: IExceptionDeal
    {
        public void DealException(ExceptionType exType, string logCode)
        {
            string showText = "程序处理过程中出现异常！Code:" + logCode;
            DevExpress.XtraEditors.XtraMessageBox.Show(showText, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }
    }
}
