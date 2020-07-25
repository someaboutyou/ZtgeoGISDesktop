using Abp.Dependency;
using System;
using System.Runtime.Serialization;
using Ztgeo.Gis.Share;

namespace Ztgeo.Gis.Runtime
{
    /// <summary>
    /// 基本UI级线程异常处理
    /// </summary>
    public abstract class ExceptionDealBase : Exception, ITransientDependency
    {
        public ExceptionDealBase():base() { 
        }
        public ExceptionDealBase(string message) : base(message) { 
        }

        public ExceptionDealBase(string message, Exception innerException) : base(message, innerException) { 
        
        } 
        public ExceptionDealBase(SerializationInfo info, StreamingContext context) : base(info, context) { 
        
        }
        /// <summary>
        /// 如何处理异常信息
        /// </summary>
        public virtual void DealException(ExceptionType exType, string logCode) {
            return;
        }
    }
}
