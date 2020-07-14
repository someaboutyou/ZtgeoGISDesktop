using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ztgeo.WebViewControl
{
    public class WebView: UserControl, IDisposable
    {
        protected virtual string GetRequestUrl(IRequest request)
        {
            return request.Url;
        }
    }
}
