using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Actions
{
    public interface IResourceAction : IWinformAction, Abp.Dependency.ISingletonDependency
    {
        IResource Resource { get; }
    }
}
