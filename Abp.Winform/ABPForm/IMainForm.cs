using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ztgeo.Gis.Winform.ABPForm
{
    /// <summary>
    /// MainForm 
    /// </summary>
    public interface IMainForm : ISingletonDependency
    {
        /// <summary>
        /// 初始化界面组件
        /// </summary>
        void StartInitializeComponent();
        /// <summary>
        /// 菜单
        /// </summary>
        void MenuInitialize();
    }
}
