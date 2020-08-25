using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Configuration;
using Ztgeo.Gis.Runtime.LocalPriority;

namespace ZtgeoGISDesktop.EntityFramework.Runtime.LocalPriority
{
    public class LocalPriorityClientRepository<TLocalPriorityClientEntity,TprimaryKey>
        : LocalPriorityClientRepositoryBase<ZtgeoGISDesktopDbContext, TLocalPriorityClientEntity, TprimaryKey>,
        ILocalPriorityClientRepository<TLocalPriorityClientEntity, TprimaryKey>
         where TLocalPriorityClientEntity : class, ILocalPriorityClientEntity<TprimaryKey>
    {
        public LocalPriorityClientRepository(
            IDbContextProvider<ZtgeoGISDesktopDbContext> dbContextProvider,
            ILocalPriorityVersionManager _localPriorityVersionManager,
            RuntimeSetting _runtimeSetting) : base(dbContextProvider, _localPriorityVersionManager, _runtimeSetting)
        {

        }
    }
    public class LocalPriorityClientRepository<TLocalPriorityClientEntity> 
        : LocalPriorityClientRepositoryBase<ZtgeoGISDesktopDbContext, TLocalPriorityClientEntity>,
        ILocalPriorityClientRepository<TLocalPriorityClientEntity>
         where TLocalPriorityClientEntity : class, ILocalPriorityClientEntity<int>
    {
        public LocalPriorityClientRepository(
            IDbContextProvider<ZtgeoGISDesktopDbContext> dbContextProvider,
            ILocalPriorityVersionManager _localPriorityVersionManager,
            RuntimeSetting _runtimeSetting) : base(dbContextProvider, _localPriorityVersionManager, _runtimeSetting) { 
        
        }
    }
}
