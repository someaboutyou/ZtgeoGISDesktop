using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.MainFormProperty;

namespace Ztgeo.Gis.CAD.Controls.CADProperty
{
    public interface ICADPropertiesControl:IPropertiesControl, Abp.Dependency.ITransientDependency
    {
        void SetSelectObjectNull();
    }
}
