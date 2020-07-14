using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.ABPForm
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISplashScreenFormManager 
    {

        void ShowSplashScreenForm();

        void CloseSplashScreenForm();
    }
}
