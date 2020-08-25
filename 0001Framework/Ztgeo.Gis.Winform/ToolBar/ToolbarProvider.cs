using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public abstract class ToolbarProvider : ITransientDependency
    {
        public abstract void SetToolbars(IToolbarDefinitionContext context);
    }
}
