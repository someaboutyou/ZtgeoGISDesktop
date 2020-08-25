using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    public interface IJS2AppAdapterApi : ITransientDependency
    {
        IJSContextProvider JsCtx { get; }

        string AppBindObjectName { get; }

        void BindCtx4JS2App(IJSContextProvider jsCtx);
    }
}
