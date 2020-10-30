using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Menu;

namespace Ztgeo.Gis.Winform.Actions
{

    public interface IMenuAction: IWinformAction ,Abp.Dependency.ISingletonDependency
    {
        /// <summary>
        /// Sender
        /// </summary>
        WinformMenu SenderMenu { set; }
    }
}
