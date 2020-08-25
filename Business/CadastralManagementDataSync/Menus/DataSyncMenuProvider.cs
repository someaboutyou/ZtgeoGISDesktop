using Abp.Dependency;
using Abp.MultiTenancy;
using CadastralManagementDataSync.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ztgeo.Gis.Hybrid;
using Ztgeo.Gis.Hybrid.FormIO;
using Ztgeo.Gis.Winform.Menu;
using Ztgeo.Utils;
using ZtgeoGISDesktop.Winform.Share;

namespace CadastralManagementDataSync.Menus
{ 
    /// <summary>
    /// 数据同步菜单
    /// </summary>
    public class DataSyncMenuProvider:MenuProvider
    {
        private readonly IocManager iocManager;
        private readonly IFormIOSchemeManager formIOSchemeManager;
        private readonly IDataSyncSettingsManager dataSyncSettingManager;
        public DataSyncMenuProvider(IocManager _iocManager,
            IFormIOSchemeManager _formIOSchemeManager,
            IDataSyncSettingsManager _dataSyncSettingManager
            ) {
            iocManager = _iocManager;
            formIOSchemeManager = _formIOSchemeManager;
            dataSyncSettingManager = _dataSyncSettingManager;
        }
        public override void SetMenus(IMenuDefinitionContext context)
        {
            var settingPageMenu = context.CreateMenu(DataSyncMenuNames.SettingPageMenu, MenuType.Page, "设置", "", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.Setting16.png"));
            var systemSettingGroup = settingPageMenu.CreateChildMenu(DataSyncMenuNames.SystemSettingGroupMenu, MenuType.Group, "系统设置");
            systemSettingGroup.CreateChildMenu(DataSyncMenuNames.SystemSettingGroup_DataSync, MenuType.Button, "数据同步设置", "数据同步设置", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.DataSync.png"),0, MultiTenancySides.Host| MultiTenancySides.Tenant,null,
                m =>
                {
                    MenuActions.DataSyncClick(iocManager, formIOSchemeManager, dataSyncSettingManager);
                }
            );
        }
    }

    public static class DataSyncMenuNames {
        public const string SettingPageMenu = "MainForm_SettingPage"; // for page
        public const string SystemSettingGroupMenu = "MainForm_SettingPage_SystemSettingGroup"; // for group
        public const string SystemSettingGroup_DataSync= "MainForm_SettingPage_SystemSettingGroup_DataSync"; //数据同步设置
    }
}
