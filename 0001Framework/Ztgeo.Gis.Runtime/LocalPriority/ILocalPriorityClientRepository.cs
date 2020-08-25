using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    public interface ILocalPriorityClientRepository<TLocalPriorityClientEntity> : IRepository<TLocalPriorityClientEntity>
        where TLocalPriorityClientEntity:class, IEntity<int>
    {
       
    }

    public interface ILocalPriorityClientRepository<TLocalPriorityClientEntity, TPrimaryKey> : IRepository<TLocalPriorityClientEntity, TPrimaryKey>
        where TLocalPriorityClientEntity : class, IEntity<TPrimaryKey>
    { 
    }
}
