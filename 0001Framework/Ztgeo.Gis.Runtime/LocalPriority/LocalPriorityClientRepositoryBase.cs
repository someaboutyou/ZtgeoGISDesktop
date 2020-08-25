using Abp.EntityFramework.Repositories;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Abp.EntityFramework;
using System.Security.Policy;
using Abp;
using System.Linq.Expressions;
using Ztgeo.Gis.Runtime.Configuration;
using Ztgeo.Gis.Runtime.LocalPriority.Dto;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    public abstract class LocalPriorityClientRepositoryBase<TDbContext, TLocalPriorityClientEntity, TPrimaryKey> :
        EfRepositoryBase<TDbContext, TLocalPriorityClientEntity, TPrimaryKey>,
        ILocalPriorityClientRepository<TLocalPriorityClientEntity, TPrimaryKey>
        where TLocalPriorityClientEntity : class, ILocalPriorityClientEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        private readonly ILocalPriorityVersionManager localPriorityVersionManager;
        private readonly RuntimeSetting runtimeSetting;
        public LocalPriorityClientRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider,
            ILocalPriorityVersionManager _localPriorityVersionManager,
            RuntimeSetting _runtimeSetting 
            ) : base(dbContextProvider) {
            localPriorityVersionManager = _localPriorityVersionManager;
            runtimeSetting = _runtimeSetting;
        }

        public override TLocalPriorityClientEntity Get(TPrimaryKey id) 
        {
            if (base.Count( CreateEqualityExpressionForId(id) ) == 0 && runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless)
            {
                TLocalPriorityClientEntity entity= localPriorityVersionManager.GetRemoteEntity<TLocalPriorityClientEntity, TPrimaryKey>(id);
                return Insert(entity);
            }
            else
            { 
                return base.Get(id);
            }
        }
        public override async Task<TLocalPriorityClientEntity> GetAsync(TPrimaryKey id) {
            if(runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless && await base.CountAsync(e => e.Id.Equals(id)) == 0) {
                TLocalPriorityClientEntity entity = await localPriorityVersionManager.GetRemoteEntityAsync<TLocalPriorityClientEntity, TPrimaryKey>(id);
                return await InsertAsync(entity);
            }else {
                return await base.GetAsync(id);
            }
            
        }

        public override List<TLocalPriorityClientEntity> GetAllList()
        {
            List<TLocalPriorityClientEntity> getFromLocalDatas = base.GetAllList();
            if(runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless)
            {
                IList<LocalPriorityVersionsOutput> versionDatas = localPriorityVersionManager.GetAllRomoteDatasFromVersionData(typeof(TLocalPriorityClientEntity).Name);
                IList<TPrimaryKey> wantRegetIds = new List<TPrimaryKey>();
                foreach (var versionData in versionDatas)
                {
                    string idString = localPriorityVersionManager.GetIdFromKey(versionData.Key);
                    TPrimaryKey id = (TPrimaryKey)Convert.ChangeType(idString, typeof(TPrimaryKey));
                    if (!getFromLocalDatas.Any(d => d.Id.Equals(id)))
                    {
                        wantRegetIds.Add(id);
                    }
                }
                if (wantRegetIds.Count > 0)
                {
                    var remoteDatas = localPriorityVersionManager.GetRemoteEntitys<TLocalPriorityClientEntity, TPrimaryKey>(wantRegetIds);
                    if (remoteDatas != null && remoteDatas.Count > 0)
                    {
                        getFromLocalDatas.AddRange(remoteDatas);
                    }
                }
            } 
            return getFromLocalDatas; 
        }

 
        public override async Task<List<TLocalPriorityClientEntity>> GetAllListAsync()
        {
            List<TLocalPriorityClientEntity> getFromLocalDatas =await base.GetAllListAsync();
            if (runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless)
            {
                IList<LocalPriorityVersionsOutput> versionDatas = localPriorityVersionManager.GetAllRomoteDatasFromVersionData(typeof(TLocalPriorityClientEntity).Name);
                IList<TPrimaryKey> wantRegetIds = new List<TPrimaryKey>();
                foreach (var versionData in versionDatas)
                {
                    string idString = localPriorityVersionManager.GetIdFromKey(versionData.Key);
                    TPrimaryKey id = (TPrimaryKey)Convert.ChangeType(idString, typeof(TPrimaryKey));
                    if (!getFromLocalDatas.Any(d => d.Id.Equals(id)))
                    {
                        wantRegetIds.Add(id);
                    }
                }
                if (wantRegetIds.Count > 0)
                {
                    var remoteDatas = await localPriorityVersionManager.GetRemoteEntitysAsync<TLocalPriorityClientEntity, TPrimaryKey>(wantRegetIds);
                    if (remoteDatas != null && remoteDatas.Count > 0)
                    {
                        getFromLocalDatas.AddRange(remoteDatas);
                    }
                }
            } 
            return getFromLocalDatas;
        }

 
 
        public override TLocalPriorityClientEntity FirstOrDefault(TPrimaryKey id)
        {
            TLocalPriorityClientEntity e = base.FirstOrDefault(id);
            if (runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless && e == null) {
                TLocalPriorityClientEntity entity = localPriorityVersionManager.GetRemoteEntity<TLocalPriorityClientEntity, TPrimaryKey>(id);
                return Insert(entity);
            }
            return e;
        }
 
        public override async Task<TLocalPriorityClientEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            TLocalPriorityClientEntity e = await base.FirstOrDefaultAsync(id);
            if (runtimeSetting.RuntimeStatus != RuntimeStatus.Serverless && e == null)
            {
                TLocalPriorityClientEntity entity = await localPriorityVersionManager.GetRemoteEntityAsync<TLocalPriorityClientEntity, TPrimaryKey>(id);
                return Insert(entity);
            }
            return e;
        } 
    }

    public abstract class LocalPriorityClientRepositoryBase<TDbContext, TLocalPriorityClientEntity> :
        LocalPriorityClientRepositoryBase<TDbContext, TLocalPriorityClientEntity, int>,
        ILocalPriorityClientRepository<TLocalPriorityClientEntity>
        where TLocalPriorityClientEntity : class, ILocalPriorityClientEntity<int>
        where TDbContext : DbContext
    {
        public LocalPriorityClientRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider,
            ILocalPriorityVersionManager _localPriorityVersionManager,
            RuntimeSetting _runtimeSetting
            ) : base(dbContextProvider, _localPriorityVersionManager, _runtimeSetting)
        {

        }
    }


}
