using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority
{ 
    public interface ILocalPriority : ILocalPriority<int>
    {

    }

    public interface ILocalPriority<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        int Version { get; set; } 
    }
}
