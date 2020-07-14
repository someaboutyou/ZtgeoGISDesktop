using Abp.Configuration;
using Abp.Runtime.Session;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Setting
{
    public class AutoSettingsInterceptor : IInterceptor
    {
        private readonly ISettingManager _settingManager;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        public IAbpSession AbpSession { get; set; }
        public AutoSettingsInterceptor(ISettingManager settingManager, ISettingDefinitionManager settingDefinitionManager)
        {
            this._settingManager = settingManager;
            this._settingDefinitionManager = settingDefinitionManager;
            this.AbpSession = NullAbpSession.Instance;
        }

        protected void PostProceed(IInvocation invocation)
        {
            var setFlag = "set_";
            var getFlag = "get_";

            var isSet = invocation.Method.Name.StartsWith(setFlag);
            var isGet = invocation.Method.Name.StartsWith(getFlag);
            //非属性方法不处理
            if (!isSet && !isGet)
                return;

            var pname = invocation.Method.Name.Replace(setFlag, "")
                                              .Replace(getFlag, "");
            var settingName = AutoSettingsUtils.CreateSettingName(invocation.TargetType, pname);
            //配置 设置
            if (isSet)
            {
                var setting = this._settingDefinitionManager.GetSettingDefinition(settingName);
                this.ChangeSettingValue(setting, invocation.Arguments[0]?.ToString());
            }
            //配置 获取
            else
            {
                var val = this._settingManager.GetSettingValue(settingName);
                invocation.ReturnValue = ConvertHelper.ChangeType(val, invocation.Method.ReturnType);
            }
        }
        protected void ChangeSettingValue(SettingDefinition settings, object value)
        {
            var val = value?.ToString();
            if (settings.Scopes.HasFlag(SettingScopes.User) && this.AbpSession.UserId.HasValue)
                this._settingManager.ChangeSettingForUser(this.AbpSession.ToUserIdentifier(), settings.Name, val);
            else if (settings.Scopes.HasFlag(SettingScopes.Tenant) && this.AbpSession.TenantId.HasValue)
                this._settingManager.ChangeSettingForTenant(this.AbpSession.TenantId.Value, settings.Name, val);
            else if (settings.Scopes.HasFlag(SettingScopes.Application))
                this._settingManager.ChangeSettingForApplication(settings.Name, val);
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            this.PostProceed(invocation);
        }
    }
}
