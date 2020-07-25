using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Hybrid.JsBinder
{
    /// <summary>
    /// C#到js 适配器
    /// </summary>
    public interface IApp2JSAdapterApi: ITransientDependency
    {
        IJSContextProvider JsCtx { get; set; }

    }
}
