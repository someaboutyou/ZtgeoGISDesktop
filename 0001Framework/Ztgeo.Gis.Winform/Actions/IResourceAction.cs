 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Resources;

namespace Ztgeo.Gis.Winform.Actions
{
    public interface IResourceAction : IWinformAction, Abp.Dependency.ISingletonDependency
    {
        IResource Resource { set; }
    }
}
