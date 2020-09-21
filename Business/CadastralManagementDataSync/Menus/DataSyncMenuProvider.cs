using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Threading.BackgroundWorkers;
using CadastralManagementDataSync.DataOperation;
using CadastralManagementDataSync.DBOperation;
using CadastralManagementDataSync.Setting;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private readonly DataSyncOperator dataSyncOperator;
        private readonly DataCapture dataCapture;
        private readonly TriggerOperation triggerOperation;
        public ILogger Logger { get; set; }
        public DataSyncMenuProvider(IocManager _iocManager,
            IFormIOSchemeManager _formIOSchemeManager,
            IDataSyncSettingsManager _dataSyncSettingManager,
            DataSyncOperator _dataSyncOperator,
            DataCapture _dataCapture,
            TriggerOperation _triggerOperation
            ) {
            iocManager = _iocManager;
            formIOSchemeManager = _formIOSchemeManager;
            dataSyncSettingManager = _dataSyncSettingManager;
            dataSyncOperator = _dataSyncOperator;
            dataCapture = _dataCapture;
            triggerOperation = _triggerOperation;
            Logger = NullLogger.Instance;
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
                    MenuActions.DataSyncSettingClick(iocManager, formIOSchemeManager, dataSyncSettingManager, Logger);
                }
            );

            var DataSyncPageMenu = context.CreateMenu(DataSyncMenuNames.DataSyncPageMenu, MenuType.Page, "数据同步", "", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.DataSync16.png"));
            var DataSynvSqlCreateMenu = DataSyncPageMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageSqlCreateGroupMenu, MenuType.Group, "数据同步");
            DataSynvSqlCreateMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageSqlCreateGroupInnerDataInitMenu, MenuType.Button,"内网数据库初始化", "内网数据库初始化",null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.innerDB32.png"), 0, MultiTenancySides.Host | MultiTenancySides.Tenant, null,
                m => {
                    MenuActions.DoDBTriggerOperationCilck(iocManager, DataSyncDirection.InnerDataSync, triggerOperation, Logger);
                }
                );
            DataSynvSqlCreateMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageSqlCreateGroupOuterDataInitMenu, MenuType.Button, "外网数据库初始化", "外网数据库初始化", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.outDB32.png"), 0, MultiTenancySides.Host | MultiTenancySides.Tenant, null,
                m => {
                    MenuActions.DoDBTriggerOperationCilck(iocManager, DataSyncDirection.OuterDataSync, triggerOperation, Logger);
                }
                );
             
            var DataSynvDoDataSyncMenu = DataSyncPageMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageDoDataSyncGroupMenu, MenuType.Group, "同步操作");
            DataSynvDoDataSyncMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageDoDataSyncGroupInnerDoDataSyncMenu, MenuType.Button, "内网数据同步", "内网数据和本地数据同步", null,
                AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.DataOut32.png"), 0, MultiTenancySides.Host | MultiTenancySides.Tenant, null,
                m =>
                {
                    MenuActions.DataSyncOperationClick(iocManager, DataSyncDirection.InnerDataSync, dataCapture, dataSyncOperator, Logger);
                }
            ); 
            DataSynvDoDataSyncMenu.CreateChildMenu(DataSyncMenuNames.DataSyncPageDoDataSyncGroupOuterDoDataSyncMenu, MenuType.Button, "外网数据同步", "外网数据和本地数据同步", null,
                 AssemblyResource.GetResourceImage(Assembly.GetExecutingAssembly(), "CadastralManagementDataSync.Icons.DataIn32.png"), 0, MultiTenancySides.Host | MultiTenancySides.Tenant, null,
                                 m =>
                                 {
                                     MenuActions.DataSyncOperationClick(iocManager, DataSyncDirection.OuterDataSync, dataCapture, dataSyncOperator, Logger);
                                 }
                 );
        }
    }

    public static class DataSyncMenuNames {
        public const string SettingPageMenu = "MainForm_SettingPage"; // for page
        public const string SystemSettingGroupMenu = "MainForm_SettingPage_SystemSettingGroup"; // for group
        public const string SystemSettingGroup_DataSync= "MainForm_SettingPage_SystemSettingGroup_DataSync"; //数据同步设置

        public const string DataSyncPageMenu = "MainForm_DataSyncPage"; // for datasync
          public const string DataSyncPageSqlCreateGroupMenu = "MainForm_DataSyncPage_DataSyncPageSqlCreateGroup"; // 数据库初始化
            public const string DataSyncPageSqlCreateGroupInnerDataInitMenu = "MainForm_DataSyncPage_DataSyncPageSqlCreate_InnerDataInit"; // 内网数据初始化
            public const string DataSyncPageSqlCreateGroupOuterDataInitMenu = "MainForm_DataSyncPage_DataSyncPageSqlCreate_OuterDataInit"; // 外网数据初始化
          public const string DataSyncPageDoDataSyncGroupMenu = "MainForm_DataSyncPage_DataSyncPageDoDataSyncGroup"; // 同步操作
            public const string DataSyncPageDoDataSyncGroupInnerDoDataSyncMenu = "MainForm_DataSyncPage_DataSyncPageDoDataSync_InnerDoDataSync"; // 内网数据同步
            public const string DataSyncPageDoDataSyncGroupOuterDoDataSyncMenu = "MainForm_DataSyncPage_DataSyncPageDoDataSync_OutDoDataSync"; // 外网数据同步
    } 
}
