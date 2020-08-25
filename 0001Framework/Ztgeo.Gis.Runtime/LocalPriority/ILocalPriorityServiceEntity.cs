using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    /// <summary>
    /// see ILocalPriorityClientEntity
    /// </summary>
    public interface ILocalPriorityServiceEntity : ILocalPriority
    {
        
    }

    public interface ILocalPriorityServiceEntity<TPrimaryKey> : ILocalPriority<TPrimaryKey>
    {

    }
}
