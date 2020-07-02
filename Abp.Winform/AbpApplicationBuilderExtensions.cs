using Abp.Modules;
using Ztgeo.Gis.Winform.ABPForm;
using Castle.MicroKernel.Util; 
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Abp;

namespace Ztgeo.Gis.Winform
{
    public static class AbpApplicationBuilderExtensions
    {
        public static Form UseAbp<TStartupModule>(ISplashScreenFormManager splashScreenFormManager = null,
            [CanBeNull] Action<AbpBootstrapperOptions> optionsAction = null)
            where TStartupModule : AbpModule
        {
            if (splashScreenFormManager != null) {
                splashScreenFormManager.ShowSplashScreenForm();
            } 
            var abpBootstrapper = AddAbpBootstrapper<TStartupModule>(optionsAction);
            abpBootstrapper.Initialize();
            IMainForm mainForm= abpBootstrapper.IocManager.Resolve<IMainForm>();
            mainForm.StartInitializeComponent();
            if (splashScreenFormManager != null)
            {
                splashScreenFormManager.CloseSplashScreenForm();
            }
            if (mainForm is Form) {
                return (Form)mainForm;
            }
            else
            {
                MessageBox.Show("未发现主窗体");
                return null;
            }
        }

        private static AbpBootstrapper AddAbpBootstrapper<TStartupModule>(Action<AbpBootstrapperOptions> optionsAction)
            where TStartupModule : AbpModule
        {
            var abpBootstrapper = AbpBootstrapper.Create<TStartupModule>(optionsAction);
            return abpBootstrapper;
        }
    }
}
