using Abp.Runtime.Caching;
using Castle.MicroKernel.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.LocalPriority.Dto;
using Ztgeo.Gis.Runtime.LocalPriority.Proxy;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    public class LocalPriorityVersionManager : ILocalPriorityVersionManager
    {
        private const string CacheKey = "LocalPriorityVersions";
        private const string CacheKeyByEntityName = "LocalPriorityVersionsByEntityName";
        private readonly ICacheManager cacheManager; 
        private readonly ILocalPriorityProxy localPriorityVersionProxy;
        public LocalPriorityVersionManager(ILocalPriorityProxy _localPriorityVersionProxy,
            ICacheManager _cacheManager
            ) {
            localPriorityVersionProxy = _localPriorityVersionProxy;
            cacheManager = _cacheManager;
        }
        public string GenerateKeyString<TLocalPriority>(TLocalPriority entity) where TLocalPriority : ILocalPriority {
            return entity.GetType().Name + "__" + entity.Id;// + "__" + entity.Version;
        }

        public string GenerateKeyString<TLocalPriority, TPrimaryKey>(TLocalPriority entity) where TLocalPriority : ILocalPriority<TPrimaryKey>{
            return entity.GetType().Name + "__" + entity.Id;// + "__" + entity.Version;
        }

        public string GetIdFromKey(string key) {
            return key.Split(new string[] { "__" }, StringSplitOptions.None)[1];
        }
        public TLocalPriority GetRemoteEntity<TLocalPriority>(int id) where TLocalPriority : ILocalPriority
        {
            return localPriorityVersionProxy.GetRemoteData<TLocalPriority>(id);
        }
        public async Task<TLocalPriority> GetRemoteEntityAsync<TLocalPriority>(int id) where TLocalPriority : ILocalPriority
        {
            return await localPriorityVersionProxy.GetRemoteDataAsync<TLocalPriority>(id);
        }
        public  TLocalPriority GetRemoteEntity<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey> { 
            return localPriorityVersionProxy.GetRemoteData<TLocalPriority, TPrimaryKey>(id);
        }
        public async Task<TLocalPriority> GetRemoteEntityAsync<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            return await localPriorityVersionProxy.GetRemoteDataAsync<TLocalPriority, TPrimaryKey>(id);
        }
        public IList<TLocalPriority> GetRemoteEntitys <TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority
        {
            return localPriorityVersionProxy.GetRemoteDatas <TLocalPriority>(ids);
        }
        public async Task<IList<TLocalPriority>> GetRemoteEntitysAsync<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority
        {
            return await localPriorityVersionProxy.GetRemoteDatasAsync<TLocalPriority>(ids);
        }
        public async Task<IList<TLocalPriority>> GetRemoteEntitysAsync<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            return await localPriorityVersionProxy.GetRemoteDatasAsync<TLocalPriority, TPrimaryKey>(ids);
        }
        public  IList<TLocalPriority> GetRemoteEntitys<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            return localPriorityVersionProxy.GetRemoteDatas<TLocalPriority, TPrimaryKey>(ids);
        }
        public async Task GetRemoteVersionsOutput()
        {
            cacheManager.GetCache(CacheKey).Clear();
            string json = await localPriorityVersionProxy.GetRemoteVersionDatas();
            if (!string.IsNullOrEmpty(json))
            {
                IList<LocalPriorityVersionsOutput> datas = JsonConvert.DeserializeObject<IList<LocalPriorityVersionsOutput>>(json);
                if (datas.Count > 0)
                {
                    Dictionary<string, List<LocalPriorityVersionsOutput>> nameValues = new Dictionary<string, List<LocalPriorityVersionsOutput>>();
                    foreach (var data in datas)
                    {
                        cacheManager.GetCache(CacheKey).Set(data.Key, data);
                        if (!nameValues.ContainsKey(data.EntityName))
                        {
                            nameValues.Add(data.EntityName, new List<LocalPriorityVersionsOutput>());
                        }
                        nameValues[data.EntityName].Add(data);
                    }
                    foreach(var kv in nameValues)
                    { 
                        cacheManager.GetCache(CacheKeyByEntityName).Set(kv.Key, kv.Value);
                    }
                }
            }
        }
        

        public async Task GetRemoteVersionsOutput<TLocalPriority>(bool isrefresh = false) where TLocalPriority : ILocalPriority 
        {
            string entityName = typeof(TLocalPriority).Name;
            var o = cacheManager.GetCache(CacheKey).Get(entityName, n => null);
            if (o == null || isrefresh)
            {
                string json = await localPriorityVersionProxy.GetRemoteVersionDatas(entityName);
                if (!string.IsNullOrEmpty(json))
                {
                    IList<LocalPriorityVersionsOutput> datas = JsonConvert.DeserializeObject<IList<LocalPriorityVersionsOutput>>(json);
                    if (datas.Count > 0)
                    {
                        foreach (var data in datas)
                        {
                            cacheManager.GetCache(CacheKey).Set(data.Key, data);
                        }
                        cacheManager.GetCache(CacheKeyByEntityName).Set(entityName, datas);
                    }
                }
            }
        }
        
        public IList<LocalPriorityVersionsOutput> GetAllRomoteDatasFromVersionData(string entityType) {
            IList<LocalPriorityVersionsOutput> datas = cacheManager.GetCache(CacheKeyByEntityName).Get(entityType, name => null) as IList<LocalPriorityVersionsOutput>;
            return datas;
        }
    }
}
