using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Winform.ABPForm
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
