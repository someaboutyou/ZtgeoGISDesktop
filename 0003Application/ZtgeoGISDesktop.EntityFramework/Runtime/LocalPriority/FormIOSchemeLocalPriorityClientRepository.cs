using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Hybrid.FormIO;
using Ztgeo.Gis.Runtime.Configuration;
using Ztgeo.Gis.Runtime.LocalPriority;

namespace ZtgeoGISDesktop.EntityFramework.Runtime.LocalPriority
{
    public class FormIOSchemeLocalPriorityClientRepository :
        LocalPriorityClientRepository<SmartFormScheme>,
        IFormIOSchemeLocalPriorityClientRepository
    {
        public FormIOSchemeLocalPriorityClientRepository(
            IDbContextProvider<ZtgeoGISDesktopDbContext> dbContextProvider,
            ILocalPriorityVersionManager _localPriorityVersionManager,
            RuntimeSetting _runtimeSetting) : base(dbContextProvider, _localPriorityVersionManager, _runtimeSetting) { 
        
        }
    }
}
