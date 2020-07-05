using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztgeo.Gis.Winform.Configuration;

namespace Ztgeo.Gis.Winform.Menu
{
    public class WinformMenuManager : MenuDefinitionContextBase, IWinformMenuManager, ISingletonDependency
    {
        public IAbpSession AbpSession { get; set; } 
        private readonly IIocManager _iocManager;
        private readonly IPermissionChecker _permissionChecker;
        private readonly WinformMenuConfiguration _winformMenuConfiguration; 
        public WinformMenuManager(IIocManager iocManager,
            WinformMenuConfiguration winformMenuConfiguration,
            IPermissionChecker permissionChecker) {
            _iocManager = iocManager;
            _winformMenuConfiguration = winformMenuConfiguration;
            _permissionChecker = permissionChecker;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual void Initialize()
        {
            foreach (var providerType in _winformMenuConfiguration.Providers)
            {
                using (var provider = _iocManager.ResolveAsDisposable<MenuProvider>(providerType))
                {
                    provider.Object.SetMenus(this);
                }
            } 
            Menus.AddAllMenus();
        }

        public virtual IReadOnlyList<WinformMenu> GetAllMenus(bool tenancyFilter = true)
        {
            return Menus.Values
                .WhereIf(tenancyFilter, m => m.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                .Where(m =>
                    string.IsNullOrEmpty(m.Permission) ||
                    AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                    (!string.IsNullOrEmpty(m.Permission) && _permissionChecker.IsGranted(m.Permission))
                ).ToImmutableList();
        }

        public virtual IReadOnlyList<WinformMenu> GetAllMenus(MultiTenancySides multiTenancySides)
        { 
            return Menus.Values
                .Where(m => m.MultiTenancySides.HasFlag(multiTenancySides))
                .Where(m =>
                    string.IsNullOrEmpty(m.Permission) ||
                    AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                    (m.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                     multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                    (!string.IsNullOrEmpty(m.Permission) && _permissionChecker.IsGranted(m.Permission))
                ).ToImmutableList();
        }

        public virtual WinformMenu GetMenu(string name)
        {
            var menu = Menus.GetOrDefault(name);
            if (menu == null)
            {
                throw new AbpException("There is no menu with name: " + name);
            }

            return menu;
        }

        public virtual WinformMenu GetMenuOrNull(string name)
        {
            var menu = Menus.GetOrDefault(name); 
            return menu;
        }

        private int? GetCurrentTenantId()
        { 
            return AbpSession.TenantId;
        }
    }
}
