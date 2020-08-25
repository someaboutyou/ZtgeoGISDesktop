using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.LocalPriority;
using Ztgeo.Gis.Runtime.LocalPriority.Proxy;

namespace ZtgeoGISDesktop.Communication.BackendRequest
{
    public class LocalPriorityProxy : ServiceProxyBase, ILocalPriorityProxy
    {
        public LocalPriorityProxy(IocManager iocManager) :base(iocManager, "api/LocalPriority")
        { 
        
        }
        public TLocalPriority GetRemoteData<TLocalPriority>(int id) where TLocalPriority : ILocalPriority
        { 
            return _restService.Post<TLocalPriority>(this.GetRequestUri("GetRemoteData"), new { Id=id,Type= typeof(TLocalPriority) });
        }

        public TLocalPriority GetRemoteData<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            return _restService.Post<TLocalPriority>(this.GetRequestUri("GetRemoteData"), new { Id = id, Type = typeof(TLocalPriority) });
        }

        public async Task<TLocalPriority> GetRemoteDataAsync<TLocalPriority>(int id) where TLocalPriority : ILocalPriority
        {
            return await _restService.PostAsync<TLocalPriority>(this.GetRequestUri("GetRemoteData"), new { Id = id, Type = typeof(TLocalPriority) });
        }

        public async Task<TLocalPriority> GetRemoteDataAsync<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            return await _restService.PostAsync<TLocalPriority>(this.GetRequestUri("GetRemoteData"), new { Id = id, Type = typeof(TLocalPriority) });
        }

        public IList<TLocalPriority> GetRemoteDatas<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority
        {
            throw new NotImplementedException();
        }

        public IList<TLocalPriority> GetRemoteDatas<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public Task<IList<TLocalPriority>> GetRemoteDatasAsync<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority
        {
            throw new NotImplementedException();
        }

        public Task<IList<TLocalPriority>> GetRemoteDatasAsync<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRemoteVersionDatas()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRemoteVersionDatas(string EntityName)
        {
            throw new NotImplementedException();
        }
    }
}
