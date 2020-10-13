using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
    public static class FileHelper
    {
        public static string GetOpenFileFilter(IList<string> extesionNames,bool isAllFiles) {
            string ret= string.Join("|", extesionNames.Select(n =>
            {
                n = n.Trim('.');
                return n + " (*." + n + ")|*." + n;
            }));
            if (isAllFiles) {
                ret += "|All files (*.*)|*.*";
            }
            return ret;
        }
    }
}
