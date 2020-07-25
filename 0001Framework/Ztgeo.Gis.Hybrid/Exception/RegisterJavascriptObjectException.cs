using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid
{
    public class RegisterJavascriptObjectException:AbpException
    {
        public RegisterJavascriptObjectException() : base() { }

        public RegisterJavascriptObjectException(string message) : base(message) { }

        public RegisterJavascriptObjectException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { 
        }

        public RegisterJavascriptObjectException(string message, Exception innerException) : base() { }
    }
}
