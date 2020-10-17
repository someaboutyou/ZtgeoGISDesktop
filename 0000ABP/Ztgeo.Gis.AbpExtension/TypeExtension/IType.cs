using System; 
using Ztgeo.Gis.AbpExtension.Exception;

namespace Ztgeo.Gis.AbpExtension
{
    /// <summary>
    /// 类型限制类 author jzw
    /// </summary>
    /// <typeparam name="TBaseType"></typeparam>
    public interface IType<in TBaseType>  
    {
        //Type ValueType<T>{ get; } where T:TBaseType
        Type Type { get; } 

        string TypeName { get; }
    }


    public class AbpType<TBaseType> : IType<TBaseType>
    {
        private Type _t;
        public AbpType(Type t) {
            if (typeof(TBaseType).IsAssignableFrom(t)) {
                throw new TypeMarchException(typeof(TBaseType).FullName+"和"+ t.FullName+ "类型不匹配");
            }
            _t = t;
        }
        public Type Type { get { return this._t; } }

        public string TypeName { get { return _t.FullName; } }

        
    }

    public class AbpType {
        public static AbpType<TBaseType> GetType<TBaseType>(Type t){
            return new AbpType<TBaseType>(t);
        }
    }
}
