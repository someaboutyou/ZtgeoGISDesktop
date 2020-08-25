using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Runtime.LocalPriority.Proxy
{
    /// <summary>
    /// powered by jzw
    /// </summary>
    public interface ILocalPriorityProxy:Abp.Dependency.ITransientDependency
    {
        Task<string> GetRemoteVersionDatas();

        Task<string> GetRemoteVersionDatas(string EntityName);

        Task<TLocalPriority> GetRemoteDataAsync<TLocalPriority>(int id) where TLocalPriority : ILocalPriority;
        TLocalPriority GetRemoteData<TLocalPriority>(int id) where TLocalPriority : ILocalPriority;

        Task<TLocalPriority> GetRemoteDataAsync<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>;
        TLocalPriority GetRemoteData<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>;

        Task<IList<TLocalPriority>> GetRemoteDatasAsync<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority;
        IList<TLocalPriority> GetRemoteDatas<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority;

        Task<IList<TLocalPriority>> GetRemoteDatasAsync<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>;
        IList<TLocalPriority> GetRemoteDatas<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>;
    }
}
