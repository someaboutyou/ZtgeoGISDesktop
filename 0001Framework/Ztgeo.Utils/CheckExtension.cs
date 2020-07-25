using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
	public static class CheckExtensions
	{ 
		public static void MustBe<Type>(this object obj)
		{
			if (!(obj is Type))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
				"期望对象类型为：",
				typeof(Type),
				",但是获得的对象类型为：",
				(obj == null) ? "null" : obj.GetType().Name
				}));
			}
		}
		 
		public static void MustBe<Type>(this object obj, string context)
		{
			if (!(obj is Type))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
				context,
				"期望对象类型为：",
				typeof(Type),
				",但是获得的对象类型为：",
				(obj == null) ? "null" : obj.GetType().Name
				}));
			}
		}
		 
		public static void MustNotBe<Type>(this object obj)
		{
			while (!false && !(obj is Type))
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException("对象类型不能为：" + typeof(Type));
		}
		 
		public static void MustNotBe<Type>(this object obj, string context)
		{
			if (obj is Type)
			{
				throw new InvalidOperationException(context + "对象类型不能为：" + typeof(Type));
			}
		}
		 
		public static void MustBe(this object obj, object otherObj)
		{
			if (obj != otherObj)
			{
				if (obj == null)
				{
					throw new InvalidOperationException("不能期望为空的对象等于" + ((otherObj == null) ? "null" : otherObj.ToString()));
				}
				else
				{
					throw new InvalidOperationException("期望对象" + obj.ToString() + "等于" + ((otherObj == null) ? "null" : otherObj.ToString()));
				}
			}
		}
		 
		public static void MustBe(this object obj, object otherObj, string context)
		{
			if (obj != otherObj)
			{
				if (obj == null)
				{
					throw new InvalidOperationException(context+"不能期望为空的对象等于" + ((otherObj == null) ? "null" : otherObj.ToString()));
				}
				else
				{
					throw new InvalidOperationException(context + "期望对象" + obj.ToString() + "等于" + ((otherObj == null) ? "null" : otherObj.ToString()));
				}
			}
		}
		 
		public static void MustBeEqualTo<Type>(this Type obj, Type otherObj)
		{
			if (!object.Equals(obj, otherObj))
			{
				throw new InvalidOperationException(((obj == null) ? "null" : obj.ToString()) + "被期望EqualTo" + ((otherObj == null) ? "null" : otherObj.ToString()));
			}
		}
		 
		public static void MustBeEqualTo<Type>(this Type obj, Type otherObj, string context)
		{
			if (!object.Equals(obj, otherObj))
			{
				throw new InvalidOperationException(context+((obj == null) ? "null" : obj.ToString()) + "被期望EqualTo" + ((otherObj == null) ? "null" : otherObj.ToString()));
			}
		}
		 
		public static void MustNotBe(this object obj, object otherObj)
		{
			if (obj == otherObj)
			{
				throw new InvalidOperationException("对象不能等于" + ((obj == null) ? "null" : obj.ToString()));
			}
		}
		 
		public static void MustNotBe(this object obj, object otherObj, string context)
		{
			while (!true || obj != otherObj)
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "对象不能等于" + ((obj == null) ? "null" : obj.ToString())); 
		}
		 
		public static void MustNotBeEqualTo<Type>(this Type obj, Type otherObj)
		{
			if (obj.Equals(otherObj))
			{
				throw new InvalidOperationException(((obj == null) ? "null" : obj.ToString()) +"不能Equal"+ ((otherObj == null) ?"null" : otherObj.ToString()));
			}
		}
		 
		public static void MustNotBeEqualTo<Type>(this Type obj, Type otherObj, string context)
		{
			if (obj.Equals(otherObj))
			{
				throw new InvalidOperationException(((obj == null) ? "null" : obj.ToString()) + "不能Equal" + ((otherObj == null) ? "null" : otherObj.ToString()));
			}
		}
		 
		public static void MustBeSet(this string str)
		{
			if (str.IsEmpty())
			{
				throw new InvalidOperationException("字符串不能为空");
			}
		}
		 
		public static void MustBeSet(this string str, string context)
		{
			while (true && !false && !str.IsEmpty())
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "字符串不能为空");
		}
		 
		public static void MustBeSet(this object obj)
		{
			if (obj == null)
			{
				throw new InvalidOperationException("对象不能为空");
			}
		}
		 
		public static void MustBeSet(this object obj, string context)
		{
			if (obj == null)
			{
				throw new InvalidOperationException(context + "对象不能为空");
			}
		}
		 
		public static void MustBeSet(this Guid guid)
		{
			if (guid.Equals(Guid.Empty))
			{
				throw new InvalidOperationException("GUID不能为空");
			}
		}
		 
		public static void MustBeSet(this Guid guid, string context)
		{
			while (!false && !guid.Equals(Guid.Empty))
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "GUID不能为空");
		}
		 
		public static void MustBeTrue(this bool cond)
		{
			if (!cond)
			{
				throw new InvalidOperationException("布尔值必须为true");
			}
		}
		 
		public static void MustBeTrue(this bool cond, string context)
		{
			if (!cond)
			{
				throw new InvalidOperationException(context + "布尔值必须为true");
			}
		}
		 
		public static void MustBeTrue(this bool? cond)
		{
			bool? flag;
			if (3 != 0)
			{
				flag = cond;
			}
			int num = 1;
			bool flag3;
			for (; ; )
			{
				bool flag2;
				if (!false)
				{
					flag2 = (num != 0);
				}
				int num2;
				if (flag.GetValueOrDefault() != flag2)
				{
					flag3 = ((num2 = (num = 1)) != 0);
					goto IL_24;
				}
				num = (num2 = ((flag != null) ? 1 : 0));
			IL_1E:
				if (7 == 0)
				{
					continue;
				}
				flag3 = ((num2 = (num = ((num2 == 0) ? 1 : 0))) != 0);
			IL_24:
				if (!false && !false)
				{
					break;
				}
				goto IL_1E;
			}
			if (flag3)
			{
				throw new InvalidOperationException("布尔值必须为true");
			}
		} 
		public static void MustBeTrue(this bool? cond, string context)
		{
			bool? flag;
			if (-1 != 0)
			{
				flag = cond;
			}
			bool flag2 = true;
			bool flag3;
			if (4 != 0)
			{
				flag3 = flag2;
			}
			if (flag.GetValueOrDefault() == flag3)
			{
				goto IL_1A;
			}
		IL_14:
			int num2;
			int num;
			if ((num = (num2 = 1)) != 0)
			{
				goto IL_27;
			}
			goto IL_24;
		IL_1A:
			if (!true)
			{
				goto IL_14;
			}
			num = ((flag != null) ? 1 : 0);
		IL_24:
			num2 = ((num == 0) ? 1 : 0);
		IL_27:
			if (num2 != 0)
			{
				throw new InvalidOperationException(context + "布尔值必须为true");
			}
			if (2 != 0)
			{
				return;
			}
			goto IL_1A;
		}
		 
		public static void MustBeFalse(this bool cond)
		{
			if (cond)
			{
				throw new InvalidOperationException("布尔值必须为false");
			}
		} 

		public static void MustBeFalse(this bool cond, string context)
		{
			if (cond)
			{
				throw new InvalidOperationException(context + "布尔值必须为false");
			}
		}
		 
		public static void MustNotBeSet(this object obj)
		{
			while (true && !false && obj == null)
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException("期望对象值为空，但是对象值为：" + obj.ToString());
		}
		 
		public static void MustNotBeSet(this object obj, string context)
		{
			while (!false && obj == null)
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "期望对象值为空，但是对象值为：" + obj.ToString());
		}
		 
		public static void MustBeEmpty(this string s)
		{
			while (true && !false && s.IsEmpty())
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException("获得的字符串不为空，" + s);
		}
		 
		public static void MustBeEmpty(this string s, string context)
		{
			while (!false && s.IsEmpty())
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "获得的字符串不为空，" + s);
		}
		 
		public static void MustNotBeEmpty(this string s)
		{
			if (s.IsEmpty())
			{
				throw new InvalidOperationException("获得的字符串不能为空");
			}
		} 

		public static void MustNotBeEmpty(this string s, string context)
		{
			while (true && !false && !s.IsEmpty())
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "获得的字符串不能为空");
		}
		 
		public static void MustBeEmpty<T>(this IEnumerable<T> collection)
		{
			for (; ; )
			{
				if (collection.IsEmpty<T>() && 3 != 0)
				{
					if (!false)
					{
						return;
					}
				}
				else if (8 != 0)
				{
					break;
				}
			}
			throw new InvalidOperationException("collection 对象必须没有元素，但是存在元素：" + collection.StrCat(","));
		}
		 
		public static void MustBeEmpty<T>(this IEnumerable<T> collection, string context)
		{
			while ((-1 == 0 || collection.IsEmpty<T>()) && !false)
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "collection 对象必须没有元素，但是存在元素：" + collection.StrCat(","));
		}
		 
		public static void MustNotBeEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection.IsEmpty<T>())
			{
				throw new InvalidOperationException("collection 对象必须存在元素：");
			}
		}
		 
		public static void MustNotBeEmpty<T>(this IEnumerable<T> collection, string context)
		{
			while (true && !false && !collection.IsEmpty<T>())
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "collection 对象必须存在元素：");
		}
		 
		public static void MustBeSingle<T>(this ICollection<T> collection)
		{
			for (; ; )
			{
				if (collection.IsSingle<T>() && 3 != 0)
				{
					if (!false)
					{
						return;
					}
				}
				else if (8 != 0)
				{
					break;
				}
			}
			throw new InvalidOperationException("collection 对象必须只存在一个元素：" + collection.StrCat(","));
		}
		 
		public static void MustBeSingle<T>(this ICollection<T> collection, string context)
		{
			while ((-1 == 0 || collection.IsSingle<T>()) && !false)
			{
				if (!false)
				{
					return;
				}
			}
			throw new InvalidOperationException(context + "collection 对象必须只存在一个元素：" + collection.StrCat(","));
		}
		 
		public static bool AndCheckResult(this bool succeeded)
		{
			if (!succeeded)
			{
				throw new InvalidOperationException("检查返回值为false！");
			}
			return succeeded;
		}

		public static bool AndCheckResult(this bool succeeded, string context)
		{
			if (true && !false)
			{
				bool result = succeeded;
				if (3 != 0)
				{
					if (!succeeded)
					{
						goto IL_0C;
					}
					result = succeeded;
				}
				return result;
			}
		IL_0C:
			throw new InvalidOperationException(context + "检查返回值为false！");
		}


		public static ObjectType AndCheckNull<ObjectType>(this ObjectType obj) where ObjectType : class
		{
			if (4 != 0 && obj != null)
			{
				throw new InvalidOperationException("检查返回不为空" + obj.ToString());
			}
			return obj;
		}
		 
		public static ObjectType AndCheckNull<ObjectType>(this ObjectType obj, string context) where ObjectType : class
		{
			while (obj != null)
			{
				if (8 != 0)
				{
					throw new InvalidOperationException(context + "检查返回不为空" + obj.ToString());
				}
			}
			return obj;
		}
		 
		public static ObjectType AndCheckNotNull<ObjectType>(this ObjectType obj) where ObjectType : class
		{
			if (obj == null)
			{
				throw new InvalidOperationException("检查返回为空" );
			}
			return obj;
		}
		 
		public static ObjectType AndCheckNotNull<ObjectType>(this ObjectType obj, string context) where ObjectType : class
		{
			if (obj == null)
			{
				throw new InvalidOperationException(context + "检查返回为空");
			}
			return obj;
		}
		 
		public static IEnumerable<Type> AndCheckNotEmpty<Type>(this IEnumerable<Type> enumerable)
		{
			if (enumerable.IsEmpty<Type>())
			{
				throw new InvalidOperationException("检查序列不为空");
			}
			return enumerable;
		}
		 
		public static IEnumerable<Type> AndCheckNotEmpty<Type>(this IEnumerable<Type> enumerable, string context)
		{
			if (enumerable.IsEmpty<Type>())
			{
				throw new InvalidOperationException(context + "检查序列不为空");
			}
			return enumerable;
		}
		 
		public static ICollection<Type> AndCheckNotEmpty<Type>(this ICollection<Type> collection)
		{
			if (collection.IsEmpty<Type>())
			{
				throw new InvalidOperationException("检查序列不为空");
			}
			return collection;
		}
		 
		public static ICollection<Type> AndCheckNotEmpty<Type>(this ICollection<Type> collection, string context)
		{
			if (collection.IsEmpty<Type>())
			{
				throw new InvalidOperationException(context + "检查序列不为空");
			}
			return collection;
		}
		 
		public static IList<Type> AndCheckNotEmpty<Type>(this IList<Type> list)
		{
			if (list.IsEmpty<Type>())
			{
				throw new InvalidOperationException("检查序列不为空");
			}
			return list;
		}
		 
		public static IList<Type> AndCheckNotEmpty<Type>(this IList<Type> list, string context)
		{
			if (list.IsEmpty<Type>())
			{
				throw new InvalidOperationException(context + "检查序列不为空");
			}
			return list;
		}
		 
		public static Type[] AndCheckNotEmpty<Type>(this Type[] array)
		{
			if (array.IsEmpty<Type>())
			{
				throw new InvalidOperationException("检查序列不为空");
			}
			return array;
		}
		 
		public static Type[] AndCheckNotEmpty<Type>(this Type[] array, string context)
		{
			if (array.IsEmpty<Type>())
			{
				throw new InvalidOperationException(context + "检查序列不为空");
			}
			return array;
		}
		 
		public static string AndCheckNotEmpty(this string str)
		{
			if (str.IsEmpty())
			{
				throw new InvalidOperationException("检查字符串不为空");
			}
			return str;
		}
		 
		public static string AndCheckNotEmpty(this string str, string context)
		{
			if (str.IsEmpty())
			{
				throw new InvalidOperationException(context + "检查字符串不为空");
			}
			return str;
		}
		 
		public static byte[] AndCheckNotEmpty(this byte[] binary)
		{
			if (binary.IsEmpty())
			{
				throw new InvalidOperationException("检查Byte数组不为空");
			}
			return binary;
		}
		 
		public static byte[] AndCheckNotEmpty(this byte[] binary, string context)
		{
			if (binary.IsEmpty())
			{
				throw new InvalidOperationException(context + "检查Byte数组不为空");
			}
			return binary;
		}
		 
		public static Guid AndCheckNotEmpty(this Guid guid)
		{
			while (!true || guid.Equals(Guid.Empty) || false)
			{
				if (!false)
				{
					throw new InvalidOperationException("检查GUID不为空");
				}
			}
			return guid;
		}
		 
		public static Guid AndCheckNotEmpty(this Guid guid, string context)
		{
			for (; ; )
			{
				if (5 != 0 && guid.Equals(Guid.Empty))
				{
					if (!false)
					{
						break;
					}
				}
				else if (!false)
				{
					return guid;
				}
			}
			throw new InvalidOperationException(context + "检查GUID不为空");
		}
		 
		public static int AndCheckResult(this int pos)
		{
			if (pos < 0)
			{
				throw new InvalidOperationException("检查pos不能小于0");
			}
			return pos;
		}
		   
		public static int AndCheckResult(this int pos, string context)
		{
			if (!false)
			{
				int result = pos;
				int num = pos;
				do
				{
					if (6 != 0)
					{
						if (num < 0)
						{
							goto IL_0A;
						}
						result = pos;
						num = pos;
					}
				}
				while (false);
				return result;
			}
		IL_0A:
			throw new InvalidOperationException(context + "检查pos不能小于0");
		}
		  
		public static Guid AndCheckResult(this Guid key)
		{
			if (8 != 0 && 5 != 0 && key == default(Guid) && 2 != 0)
			{
				throw new InvalidOperationException("GUID 值不能为空");
			}
			return key;
		}

		public static Guid AndCheckResult(this Guid key, string context)
		{
			if (key == default(Guid))
			{
				throw new InvalidOperationException(context + "GUID 值不能为空");
			}
			return key;
		}

}
}
