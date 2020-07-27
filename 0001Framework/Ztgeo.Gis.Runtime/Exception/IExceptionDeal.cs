using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Share;

namespace Ztgeo.Gis.Runtime
{
    public interface IExceptionDeal 
    {
        void DealException(ExceptionType exType, string logCode);
    }
}
