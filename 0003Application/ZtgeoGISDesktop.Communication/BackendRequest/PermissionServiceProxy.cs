using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.Authorization.Permissions;

namespace ZtgeoGISDesktop.Communication.BackendRequest
{
    public interface IPermissionServiceProxy : ISingletonDependency
    {
        IList<FlatPermissionWithLevelDto> GetAllPressions();
        Task<IList<FlatPermissionWithLevelDto>> GetAllPressionsAsync();
    }
    public class PermissionServiceProxy : ServiceProxyBase, IPermissionServiceProxy
    {
        public PermissionServiceProxy (IocManager iocManager):base(iocManager, "api/services/app/Permission") { 
        
        }
        public IList<FlatPermissionWithLevelDto> GetAllPressions() {
            return _restService.Post<List<FlatPermissionWithLevelDto>>(this.GetRequestUri("GetAllPermissions"), null);
        }

        public async Task<IList<FlatPermissionWithLevelDto>> GetAllPressionsAsync()
        {
            return await _restService.PostAsync<List<FlatPermissionWithLevelDto>>(this.GetRequestUri("GetAllPermissions"), null);
        }
    }
}
