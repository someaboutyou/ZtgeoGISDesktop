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
        IocManager IocManager { get; }
        /// <summary>
        /// 菜单 容器
        /// </summary>
        Control MenuContainerControl { get; }
        /// <summary>
        /// tool bar 管理
        /// </summary>
        object ToolBarManager { get; }
        /// <summary>
        /// tool bar 容器管理 
        /// </summary>
        Control StandaloneBarDockControl { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        void StartInitializeComponent();
        /// <summary>
        /// 在主文档界面添加一个文档
        /// </summary>
        /// <returns></returns>
        Control AddADocument();

    }
}
