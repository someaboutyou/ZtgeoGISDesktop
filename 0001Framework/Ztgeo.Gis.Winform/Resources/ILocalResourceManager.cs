using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Resources
{
    /// <summary>
    /// 本地资源管理
    /// </summary>
    public interface ILocalResourceManager:Abp.Dependency.ITransientDependency
    {
        IList<IResource> 
    }
}
