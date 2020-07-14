using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ztgeo.Gis.AbpExtension.Setting
{
    public static class ConvertHelper
    {
        #region = ChangeType =
        public static object ChangeType(object obj, Type conversionType)
        {
            return ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
        }
        public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
        {
            #region Nullable
            Type nullableType = Nullable.GetUnderlyingType(conversionType);
            if (nullableType != null)
            {
                if (obj == null)
                {
                    return null;
                }
                return Convert.ChangeType(obj, nullableType, provider);
            }
            #endregion
            if (typeof(System.Enum).IsAssignableFrom(conversionType))
            {
                return Enum.Parse(conversionType, obj.ToString());
            }
            return Convert.ChangeType(obj, conversionType, provider);
        }
        #endregion
    }
}
