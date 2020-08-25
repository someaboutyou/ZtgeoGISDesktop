using Abp.Domain.Services;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Runtime.LocalPriority;

namespace Ztgeo.Gis.Hybrid.FormIO
{
    public interface IFormIOSchemeManager {
        string GetFormIOSchemeById(int Id);
    }
    public class FormIOSchemeManager: IFormIOSchemeManager,IDomainService
    {
        private readonly IFormIOSchemeLocalPriorityClientRepository localPriorityClientRepository;
        public ILogger Logger { get; set; }
        public FormIOSchemeManager(IFormIOSchemeLocalPriorityClientRepository _localPriorityClientRepository) {
            localPriorityClientRepository = _localPriorityClientRepository;
            Logger = NullLogger.Instance;
        }

        public string GetFormIOSchemeById(int Id) {
           var entity= localPriorityClientRepository.Get(Id);
            if (entity != null) {
                return entity.Scheme;
            }
            Logger.Warn("Get Null FormIOScheme By Id:"+Id);
            return string.Empty;
        }
    }
}
