using Abp.Application.Services.Dto;
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
        IListResult<FlatPermissionWithLevelDto> GetAllPressions();
        Task<IListResult<FlatPermissionWithLevelDto>> GetAllPressionsAsync();
    }
    public class PermissionServiceProxy : ServiceProxyBase, IPermissionServiceProxy
    {
        public PermissionServiceProxy (IocManager iocManager):base(iocManager, "api/services/app/Permission") { 
        
        }
        public IListResult<FlatPermissionWithLevelDto> GetAllPressions() {
            return _restService.Get<ListResultDto<FlatPermissionWithLevelDto>>(this.GetRequestUri("GetAllPermissions"));
        }

        public async Task<IListResult<FlatPermissionWithLevelDto>> GetAllPressionsAsync()
        {
            return await _restService.GetAsync<ListResultDto<FlatPermissionWithLevelDto>>(this.GetRequestUri("GetAllPermissions"));
        }
    }
}
