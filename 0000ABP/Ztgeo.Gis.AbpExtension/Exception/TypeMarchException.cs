using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Exception
{
    /// <summary>
    /// 类型不匹配
    /// </summary>
    public class TypeMarchException :System.Exception    {
        public TypeMarchException() : base() { 
        
        }

        public TypeMarchException(string message) : base(message) { 
        
        }

        public TypeMarchException(string message, System.Exception innerException) : base(message, innerException) { 
        
        }
    }
}
