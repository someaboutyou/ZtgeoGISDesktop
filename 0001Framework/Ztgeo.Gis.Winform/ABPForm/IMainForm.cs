using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Winform.MainFormDocument;
using Ztgeo.Gis.Winform.MainFormStatusBar;

namespace Ztgeo.Gis.Winform.ABPForm
{
    /// <summary>
    /// MainForm 
    /// </summary>
    public interface IMainForm : ISingletonDependency
    {
        IocManager IocManager { get; }
        #region 主界面容器
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
        /// 主界面 图层Panel
        /// </summary>
        Control LayerPanel { get; }
        /// <summary>
        /// 主界面 属性Panel
        /// </summary>
        Control PropertiesPanel { get; }
        /// <summary>
        /// 主界面 资源管理
        /// </summary>
        Control ResourcesPanel { get; }
        IDocumentControl ActiveDocumentControl { get; }
        #endregion

        #region 主界面方法
        /// <summary>
        /// 初始化
        /// </summary>
        void StartInitializeComponent();

        /// <summary>
        /// 在主文档界面添加一个文档
        /// </summary>
        /// <returns></returns>
        IDocumentControl AddADocument(IDocumentControl documentControl,string name);
        /// <summary>
        /// 手动激活一个文档
        /// </summary>
        /// <returns></returns>
        void ManualActiveADocumentControl(IDocumentControl documentControl);
        /// <summary>
        /// 在状态栏设置系统状态信息
        /// </summary>
        void SetStatusInfo(IDocumentControl documentControl,StatusInfo statusInfo);
        /// <summary>
        /// 清除状态栏信息
        /// </summary>
        /// <param name="documentControl"></param>
        void ClearStatusInfo(IDocumentControl documentControl);
        #endregion
    }
}
