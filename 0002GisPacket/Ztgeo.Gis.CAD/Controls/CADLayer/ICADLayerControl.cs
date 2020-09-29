using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormLayer;

namespace Ztgeo.Gis.CAD.Controls.CADLayer
{
    public interface ICADLayerControl : ILayerControl, ITransientDependency
    {


    }
}
