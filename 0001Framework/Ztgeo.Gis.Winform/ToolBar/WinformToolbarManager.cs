using Abp;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Configuration;

namespace Ztgeo.Gis.Winform.ToolBar
{
    public class WinformToolbarManager : ToolbarDefinitionContextBase, IWinformToolbarManager, ISingletonDependency
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IIocManager _iocManager;
        private readonly IPermissionChecker _permissionChecker;
        private readonly WinformToolbarConfiguration _winformToolbarConfiguration;
        public WinformToolbarManager(IIocManager iocManager,
            WinformToolbarConfiguration winformToolbarConfiguration,
            IPermissionChecker permissionChecker
            ) {
            _iocManager = iocManager;
            _permissionChecker = permissionChecker;
            _winformToolbarConfiguration = winformToolbarConfiguration;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual void Initialize() { 
            foreach(var providerType in _winformToolbarConfiguration.Providers)
            {
                using (var provider = _iocManager.ResolveAsDisposable<ToolbarProvider>(providerType)) {
                    provider.Object.SetToolbars(this);
                }
            } 
        }

        public IReadOnlyList<WinformToolbarGroup> GetAllToolbarGroups(bool tenancyFilter = true)
        {
            return this.ToolbarGroups.Values.WhereIf(tenancyFilter,t=>t.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                .Where(t=>string.IsNullOrEmpty(t.Permission) ||
                    AbpSession.MultiTenancySide ==MultiTenancySides.Host ||
                    (!string.IsNullOrEmpty(t.Permission) && _permissionChecker.IsGranted(t.Permission))).ToImmutableList();
        }

        public IReadOnlyList<WinformToolbarGroup> GetAllToolbarGroups(MultiTenancySides multiTenancySides)
        {
            return ToolbarGroups.Values
                .Where(t => t.MultiTenancySides.HasFlag(multiTenancySides))
                .Where(t =>
                    string.IsNullOrEmpty(t.Permission) ||
                    AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                    (t.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                     multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                    (!string.IsNullOrEmpty(t.Permission) && _permissionChecker.IsGranted(t.Permission))
                ).ToImmutableList();
        }
        /// <summary>
        /// 根据组名+toolbar名 获取 toolbar
        /// </summary>
        /// <param name="profixedName"></param>
        /// <returns></returns>
        public WinformToolbar GetToolbarByProfixedName(string profixedName)
        {
            if (profixedName.Contains(WinformToolbar.NameSplitKey)) {
                string[] names = profixedName.Split(new string[] { WinformToolbar.NameSplitKey }, StringSplitOptions.None);
                if (names.Length != 2) {
                    throw new AbpException("error profixed name: " + profixedName);
                }
                WinformToolbarGroup group = GetToolbarGroup(names[0]);
                var toolbar = group.ToolBars.FirstOrDefault(t => t.Name.Equals(names[1]));
                if (toolbar == null) {
                    throw new AbpException("There is no Toolbar with name: " + names[1] + " in group " + names[0]);
                }
                return toolbar;
            }
            else {
                throw new AbpException("error profixed name: " + profixedName);
            }
        }

        public WinformToolbarGroup GetToolbarGroup(string groupName)
        {
            var group= ToolbarGroups.GetOrDefault(groupName);
            if (group == null) {
                throw new AbpException("There is no Toolbar group with name: "+groupName);
            }
            return group;
        }

        public IList<WinformToolbar> GetToolbarsByGroups(string groupName, bool tenancyFilter = true)
        {
            var group = GetToolbarGroup(groupName);
            return GetToolbarsByGroups(group, tenancyFilter);
        }

        public IList<WinformToolbar> GetToolbarsByGroups(WinformToolbarGroup group, bool tenancyFilter = true) {
            return group.ToolBars.WhereIf(tenancyFilter, t => t.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
               .Where(t => string.IsNullOrEmpty(t.Permission) ||
                   AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                   (!string.IsNullOrEmpty(t.Permission) && _permissionChecker.IsGranted(t.Permission))).ToImmutableList();
        }

        public IList<WinformToolbar> GetToolbarsByGroups(string groupName, MultiTenancySides multiTenancySides)
        {
            var group = GetToolbarGroup(groupName);
            return group.ToolBars.Where(t => t.MultiTenancySides.HasFlag(multiTenancySides))
                .Where(t =>
                    string.IsNullOrEmpty(t.Permission) ||
                    AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                    (t.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                     multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                    (!string.IsNullOrEmpty(t.Permission) && _permissionChecker.IsGranted(t.Permission))
                ).ToImmutableList();
        }
    }
}
