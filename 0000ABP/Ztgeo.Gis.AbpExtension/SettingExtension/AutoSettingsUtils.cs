using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.SettingExtension
{
    public static class AutoSettingsUtils
    {
        public static string CreateSettingName(Type type, string propertyName)
        {
            return $"{type.Name}.{propertyName}";
        }

        public static object GetDefaultValue(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
