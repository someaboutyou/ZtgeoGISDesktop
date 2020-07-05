using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Menu
{
    public abstract class MenuProvider : ITransientDependency
    {
        public abstract void SetMenus(IMenuDefinitionContext context);
    }
}
