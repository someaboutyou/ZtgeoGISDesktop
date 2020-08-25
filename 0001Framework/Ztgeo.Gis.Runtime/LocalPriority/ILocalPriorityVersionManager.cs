using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.LocalPriority.Dto;

namespace Ztgeo.Gis.Runtime.LocalPriority
{
    /// <summary>
    /// powered by jzw, 20200820
    /// 本地优先管理类
    /// </summary>
    public interface ILocalPriorityVersionManager: IDomainService
    {
        /// <summary>
        /// 生成实体的Key string
        /// </summary>
        /// <typeparam name="TLocalPriority"></typeparam>
        /// <returns></returns>
        string GenerateKeyString<TLocalPriority>(TLocalPriority entity) where TLocalPriority : ILocalPriority; 
        /// <summary>
        /// 生成实体的Key string
        /// </summary>
        /// <typeparam name="TLocalPriority"></typeparam>
        /// <returns></returns>
        string GenerateKeyString<TLocalPriority, TPrimaryKey>(TLocalPriority entity) where TLocalPriority : ILocalPriority<TPrimaryKey>;

        string GetIdFromKey(string key);
        /// <summary>
        /// 获取远程的版本号集合,并将其写入缓存。isrefresh 是否刷新，重新从缓存获取
        /// </summary>
        /// <returns></returns>
        Task GetRemoteVersionsOutput();
        /// <summary>
        /// 获取远程版本号集合
        /// </summary>
        /// <typeparam name="TLocalPriority"></typeparam>
        /// <returns></returns>
        Task GetRemoteVersionsOutput<TLocalPriority>(bool isrefresh = false) where TLocalPriority : ILocalPriority;

        TLocalPriority GetRemoteEntity<TLocalPriority>(int id) where TLocalPriority : ILocalPriority;
        Task<TLocalPriority> GetRemoteEntityAsync<TLocalPriority>(int id) where TLocalPriority : ILocalPriority;
        
        Task<TLocalPriority> GetRemoteEntityAsync<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>;
        TLocalPriority GetRemoteEntity<TLocalPriority, TPrimaryKey>(TPrimaryKey id) where TLocalPriority : ILocalPriority<TPrimaryKey>;

        Task<IList<TLocalPriority>> GetRemoteEntitysAsync<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority;
        IList<TLocalPriority> GetRemoteEntitys<TLocalPriority>(IList<int> ids) where TLocalPriority : ILocalPriority;
        Task<IList<TLocalPriority>> GetRemoteEntitysAsync<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>;
        IList<TLocalPriority> GetRemoteEntitys<TLocalPriority, TPrimaryKey>(IList<TPrimaryKey> ids) where TLocalPriority : ILocalPriority<TPrimaryKey>;
        /// <summary>
        /// 从远程返回的版本数据中获得所有的某个实体类型的所有id。
        /// 通过所有服务器上的Ids,检查
        /// </summary>
        /// <returns></returns>
        IList<LocalPriorityVersionsOutput> GetAllRomoteDatasFromVersionData(string entityName);
    }
}
