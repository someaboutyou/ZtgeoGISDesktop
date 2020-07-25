using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
	public static class CollectionsExtensions
	{ 
		[DebuggerNonUserCode]
		public static void Apply<Type>(this IEnumerable<Type> collection, Action<Type> action)
		{
			foreach (Type obj in collection)
			{
				action(obj);
			}
		}
		 
		[DebuggerNonUserCode]
		public static void Apply<Type>(this IEnumerable<Type> collection, Action<Type, int> action)
		{
			int num = 0;
			foreach (Type arg in collection)
			{
				action(arg, num);
				num++;
			}
		}
		 
		[DebuggerNonUserCode]
		public static IEnumerable<Type> Reject<Type>(this IEnumerable<Type> collection, Func<Type, bool> predicate)
		{
			return from x in collection
				   where !predicate(x)
				   select x;
		}
		 
		[DebuggerNonUserCode]
		public static bool IsOneOf<Type>(this Type obj, params Type[] collection)
		{
			return collection.Contains(obj);
		}

 		[DebuggerNonUserCode]
		public static bool IsOneOfReferentially<Type>(this Type obj, params Type[] collection)
		{
			return collection.Any((Type o) => o .Equals( obj));
		}

 		[DebuggerNonUserCode]
		public static T2 FirstNotNull<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector) where T2 : class
		{
			foreach (T1 arg in collection)
			{
				T2 t = selector(arg);
				if (t != null)
				{
					return t;
				}
			}
			return default(T2);
		}

 		[DebuggerNonUserCode]
		public static void AddRangeReplacing<K, T>(this IDictionary<K, T> dict, IEnumerable<KeyValuePair<K, T>> pairs)
		{
			foreach (KeyValuePair<K, T> keyValuePair in pairs.ToList<KeyValuePair<K, T>>())
			{
				dict[keyValuePair.Key] = keyValuePair.Value;
			}
		}

 		[DebuggerNonUserCode]
		public static void RemoveRange<K, T>(this IDictionary<K, T> dict, IEnumerable<K> keys)
		{
			foreach (K key in keys)
			{
				dict.Remove(key);
			}
		}

 		[DebuggerNonUserCode]
		public static void RemoveIfKeyMatches<K, T>(this IDictionary<K, T> dict, Func<K, bool> pred)
		{
			foreach (K key in dict.Keys.Where(pred).ToList<K>())
			{
				dict.Remove(key);
			}
		}

 		[DebuggerNonUserCode]
		public static T GetValueOrDefault<K, T>(this IDictionary<K, T> collection, K key)
		{
			return collection.GetValueOrDefault(key, default(T));
		}

 		[DebuggerNonUserCode]
		public static T GetValueOrDefault<K, T>(this IDictionary<K, T> collection, K key, T defaultValue)
		{
			T result;
			if (collection.TryGetValue(key, out result))
			{
				return result;
			}
			return defaultValue;
		}

 		[DebuggerNonUserCode]
		public static string GetValueOrEmpty<K>(this IDictionary<K, string> collection, K key)
		{
			return collection.GetValueOrDefault(key, string.Empty);
		}

 		[DebuggerNonUserCode]
		public static T Second<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(1);
		}

 		[DebuggerNonUserCode]
		public static T Third<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(2);
		}

 		[DebuggerNonUserCode]
		public static T Fourth<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(3);
		}

 		[DebuggerNonUserCode]
		public static T Fifth<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(4);
		}

 		[DebuggerNonUserCode]
		public static T RemoveLast<T>(this IList<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

 		[DebuggerNonUserCode]
		public static T RemoveFirst<T>(this IList<T> list)
		{
			T result = list[0];
			list.RemoveAt(0);
			return result;
		}

 		[DebuggerNonUserCode]
		public static bool RemoveFirst<T>(this IList<T> list, Predicate<T> predicate)
		{
			int num = 0;
			foreach (T obj in list)
			{
				if (predicate(obj))
				{
					list.RemoveAt(num);
					return true;
				}
				num++;
			}
			return false;
		}

 		[DebuggerNonUserCode]
		public static int RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
		{
			int num = 0;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (predicate(list[i]))
				{
					list.RemoveAt(i);
					num++;
				}
			}
			return num;
		}

 		[DebuggerNonUserCode]
		public static IEnumerable<T> ToEnumerable<T>(this T obj)
		{
			if (obj != null)
			{
				yield return obj;
			}
			yield break;
		}

 		[DebuggerNonUserCode]
		public static int IndexOf<Type>(this IEnumerable<Type> collection, Type item)
		{
			IList<Type> list = collection as IList<Type>;
			if (list != null)
			{
				return list.IndexOf(item);
			}
			using (IEnumerator<Type> enumerator = collection.GetEnumerator())
			{
				int num = 0;
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					if (type.Equals(item))
					{
						return num;
					}
					num++;
				}
			}
			return -1;
		}

 		[DebuggerNonUserCode]
		public static bool CountEquals<Type>(this IEnumerable<Type> collection, int countToCompare)
		{
			ICollection<Type> collection2 = collection as ICollection<Type>;
			if (collection2 != null)
			{
				return collection2.Count == countToCompare;
			}
			int num = 0;
			if (collection != null)
			{
				using (IEnumerator<Type> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
						if (num > countToCompare)
						{
							return false;
						}
					}
				}
			}
			return num == countToCompare;
		}

 		[DebuggerNonUserCode]
		public static bool CountBiggerThan<Type>(this IEnumerable<Type> collection, int countToCompare)
		{
			ICollection<Type> collection2 = collection as ICollection<Type>;
			if (collection2 != null)
			{
				return collection2.Count > countToCompare;
			}
			int num = 0;
			if (collection != null)
			{
				using (IEnumerator<Type> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
						if (num > countToCompare)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

 		[DebuggerNonUserCode]
		public static int IndexOf<Type>(this IEnumerable<Type> collection, Predicate<Type> match)
		{
			using (IEnumerator<Type> enumerator = collection.GetEnumerator())
			{
				int num = 0;
				while (enumerator.MoveNext())
				{
					if (match(enumerator.Current))
					{
						return num;
					}
					num++;
				}
			}
			return -1;
		}

 		[DebuggerNonUserCode]
		public static Pair<int, Type> GetIndexOfAndObject<Type>(this IEnumerable<Type> collection, Predicate<Type> match)
		{
			using (IEnumerator<Type> enumerator = collection.GetEnumerator())
			{
				int num = 0;
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					if (match(type))
					{
						return Pair.Create<int, Type>(num, type);
					}
					num++;
				}
			}
			return null;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmpty(this byte[] byteArray)
		{
			return byteArray == null || byteArray.Length == 0;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmpty<T>(this T[] array)
		{
			return array.Length == 0;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmpty<T>(this ICollection<T> collection)
		{
			return collection.Count == 0;
		}

 		[DebuggerNonUserCode]
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return collection == null || collection.IsEmpty<T>();
		}

 		[DebuggerNonUserCode]
		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			ICollection<T> collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			bool result;
			using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
			{
				result = !enumerator.MoveNext();
			}
			return result;
		}

 		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, string failMessage, Func<TSource, string> elementToString)
		{
			TSource result;
			try
			{
				result = source.SingleOrDefault(predicate);
			}
			catch (InvalidOperationException innerException)
			{
				throw new InvalidOperationException(failMessage + "Found " + (from elem in source.Where(predicate)
																			  select elementToString(elem)).StrCat(","), innerException);
			}
			return result;
		}

 		[DebuggerNonUserCode]
		public static Pair<IEnumerable<T>, IEnumerable<T>> Partition<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
		{
			return Pair.Create<IEnumerable<T>, IEnumerable<T>>(collection.Where(predicate), from elem in collection
																							where !predicate(elem)
																							select elem);
		}

 		[DebuggerNonUserCode]
		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

 		[DebuggerNonUserCode]
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || enumerable.IsEmpty<T>();
		}
		 
		[DebuggerNonUserCode]
		public static bool IsSingle<T>(this T[] array)
		{
			return array.Length == 1;
		}

 		[DebuggerNonUserCode]
		public static bool IsSingle<T>(this ICollection<T> collection)
		{
			return collection.Count == 1;
		}

 		[DebuggerNonUserCode]
		public static bool IsSingle<T>(this IEnumerable<T> enumerable)
		{
			ICollection<T> collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count == 1;
			}
			bool result;
			using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
			{
				result = (enumerator.MoveNext() && !enumerator.MoveNext());
			}
			return result;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmptyOrSingle<T>(this T[] array)
		{
			return array.Length == 0 || array.Length == 1;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmptyOrSingle<T>(this ICollection<T> collection)
		{
			return collection.Count == 0 || collection.Count == 1;
		}

 		[DebuggerNonUserCode]
		public static bool IsEmptyOrSingle<T>(this IEnumerable<T> enumerable)
		{
			ICollection<T> collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count == 0 || collection.Count == 1;
			}
			bool result;
			using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = true;
				}
				else
				{
					result = !enumerator.MoveNext();
				}
			}
			return result;
		}

 		[DebuggerNonUserCode]
		public static Type FirstIfSingleOrDefault<Type>(this IEnumerable<Type> collection)
		{
			using (IEnumerator<Type> enumerator = collection.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Type result = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return result;
					}
					return default(Type);
				}
			}
			return default(Type);
		}

 		[DebuggerNonUserCode]
		public static Type FirstIfSingleOrDefault<Type>(this IEnumerable<Type> collection, Predicate<Type> match)
		{
			using (IEnumerator<Type> enumerator = (from obj in collection
												   where match(obj)
												   select obj).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Type result = enumerator.Current;
					if (!enumerator.MoveNext())
					{
						return result;
					}
					return default(Type);
				}
			}
			return default(Type);
		}

 		[DebuggerNonUserCode]
		public static int AddRange<Type>(this ICollection<Type> collection, IEnumerable<Type> otherCollection)
		{
			int num = 0;
			HashSet<Type> hashSet = collection as HashSet<Type>;
			if (hashSet != null)
			{
				using (IEnumerator<Type> enumerator = otherCollection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Type item = enumerator.Current;
						if (hashSet.Add(item))
						{
							num++;
						}
					}
					return num;
				}
			}
			if (collection != null)
			{
				foreach (Type item2 in otherCollection)
				{
					collection.Add(item2);
					num++;
				}
			}
			return num;
		}

 		[DebuggerNonUserCode]
		public static void CopyTo<Type>(this IEnumerable<Type> collection, Type[] array)
		{
			int num = 0;
			foreach (Type type in collection)
			{
				array[num++] = type;
			}
		}

 		[DebuggerNonUserCode]
		public static void CopyTo<Type>(this IEnumerable<Type> collection, Type[] array, int arrayIndex)
		{
			int num = 0;
			foreach (Type type in collection)
			{
				array[arrayIndex + num++] = type;
			}
		}

 		[DebuggerNonUserCode]
		public static string StrCat<T>(this IEnumerable<T> source, string separator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (T t in source)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(separator);
				}
				stringBuilder.Append(t.ToString());
			}
			return stringBuilder.ToString();
		}

 		[DebuggerNonUserCode]
		public static string StrCat(this IEnumerable<string> source, string separator)
		{
			return string.Join(separator, source);
		}

 		[DebuggerNonUserCode]
		public static string StrCat(this IEnumerable<string> source, string separator, bool skipEmptyElements)
		{
			if (skipEmptyElements)
			{
				return (from e in source
						where !e.IsEmpty()
						select e).StrCat(separator);
			}
			return source.StrCat(separator);
		}

 		[DebuggerNonUserCode]
		public static string StrCat(this IList<string> source, string separator, string lastSeparator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < source.Count; i++)
			{
				if (i != 0)
				{
					if (i == source.Count - 1)
					{
						stringBuilder.Append(lastSeparator);
					}
					else if (i != 0)
					{
						stringBuilder.Append(separator);
					}
				}
				stringBuilder.Append(source[i] ?? "");
			}
			return stringBuilder.ToString();
		}

 		[DebuggerNonUserCode]
		public static IEnumerable<Type> Concat<Type>(this IEnumerable<Type> collection, params Type[] extraItems)
		{
			return collection.Concat(extraItems.AsEnumerable<Type>());
		}

 		[DebuggerNonUserCode]
		public static byte[] Concat(this byte[] a, byte[] b)
		{
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, array, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

 		[DebuggerNonUserCode]
		public static bool StartsWith<T>(this IEnumerable<T> firstCollection, IEnumerable<T> secondCollection)
		{
			using (IEnumerator<T> enumerator = secondCollection.GetEnumerator())
			{
				foreach (T t in firstCollection)
				{
					if (!enumerator.MoveNext())
					{
						return true;
					}
					if (!t.Equals(enumerator.Current))
					{
						return false;
					}
				}
			}
			return true;
		}

 		[DebuggerNonUserCode]
		private static void Move<Type>(this IList<Type> list, int from, int to)
		{
			Type item = list[from];
			list.RemoveAt(from);
			list.Insert(to, item);
		}

 		[DebuggerNonUserCode]
		public static void SetOrderTo<Type>(this IList<Type> list, IEnumerable<Type> orderList)
		{
			list.SetOrderTo(orderList, (Type a, Type b) => object.Equals(a, b));
		}

 		[DebuggerNonUserCode]
		public static void SetOrderTo<Type>(this IList<Type> list, IEnumerable<Type> orderList, Action<int, int> move)
		{
			list.SetOrderTo(orderList, (Type a, Type b) => object.Equals(a, b), move);
		}

 		[DebuggerNonUserCode]
		public static void SetOrderTo<Type1, Type2>(this IList<Type1> list, IEnumerable<Type2> orderList, Func<Type1, Type2, bool> equals)
		{
			list.SetOrderTo(orderList, equals, delegate (int from, int to)
			{
				list.Move(from, to);
			});
		}

 		[DebuggerNonUserCode]
		public static void SetOrderTo<Type1, Type2>(this IList<Type1> list, IEnumerable<Type2> orderList, Func<Type1, Type2, bool> equals, Action<int, int> move)
		{
			using (IEnumerator<Type2> enumerator = orderList.GetEnumerator())
			{
				enumerator.MoveNext();
				int i = 0;
				while (i < list.Count)
				{
					Type2 type = enumerator.Current;
					if (!equals(list[i], type))
					{
						int num = i;
						for (; ; )
						{
							num++;
							if (num >= list.Count)
							{
								break;
							}
							if (equals(list[num], type))
							{
								goto Block_6;
							}
						}
						throw new InvalidOperationException(("Object not found: " + type != null) ? type.ToString() : "null");
					Block_6:
						move(num, i);
					}
					i++;
					enumerator.MoveNext();
				}
			}
		}

 		[DebuggerNonUserCode]
		public static void QuickSort<T>(this IList<T> collection, Func<T, T, int> compare)
		{
			collection.QuickSort(compare, delegate (int i, int j)
			{
				T value = collection[i];
				collection[i] = collection[j];
				collection[j] = value;
			});
		}

 		[DebuggerNonUserCode]
		public static void QuickSort<T>(this IList<T> collection, Func<T, T, int> compare, Action<int, int> swap)
		{
			collection.QuickSort(0, collection.Count - 1, compare, swap);
		}

 		[DebuggerNonUserCode]
		private static void QuickSort<T>(this IList<T> collection, int i, int j, Func<T, T, int> compare, Action<int, int> swap)
		{
			if (i < j)
			{
				int num = CollectionsExtensions.Partition<T>(collection, i, j, compare, swap);
				collection.QuickSort(i, num, compare, swap);
				collection.QuickSort(num + 1, j, compare, swap);
			}
		}

 		[DebuggerNonUserCode]
		private static int Partition<T>(IList<T> collection, int p, int r, Func<T, T, int> compare, Action<int, int> swap)
		{
			T arg = collection[p];
			int num = p - 1;
			int num2 = r + 1;
			for (; ; )
			{
				num2--;
				if (compare(collection[num2], arg) <= 0)
				{
					do
					{
						num++;
					}
					while (compare(collection[num], arg) < 0);
					if (num >= num2)
					{
						break;
					}
					swap(num, num2);
				}
			}
			return num2;
		}

 		[DebuggerNonUserCode]
		public static Func<A, R> Memoize<A, R>(this Func<A, R> f)
		{
			Dictionary<A, R> map = new Dictionary<A, R>();
			return delegate (A a)
			{
				R r;
				if (map.TryGetValue(a, out r))
				{
					return r;
				}
				r = f(a);
				map.Add(a, r);
				return r;
			};
		} 
 		[DebuggerNonUserCode]
		public static IEnumerable<TSource> Distinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> comparer)
		{
			return source.Distinct(new CollectionsExtensions.DynamicEqualityComparer<TSource, TResult>(comparer));
		}

 		[DebuggerNonUserCode]
		public static IOrderedEnumerable<T> OrderBy<T, TT>(this IEnumerable<T> source, Func<T, TT> selector, Func<TT, TT, int> comparer)
		{
			return source.OrderBy((T obj) => selector(obj), new CollectionsExtensions.DynamicComparer<TT>(comparer));
		}

 		[DebuggerNonUserCode]
		public static IOrderedEnumerable<T> OrderByDescending<T, TT>(this IEnumerable<T> source, Func<T, TT> selector, Func<TT, TT, int> comparer)
		{
			return source.OrderByDescending((T obj) => selector(obj), new CollectionsExtensions.DynamicComparer<TT>(comparer));
		}

 		[DebuggerNonUserCode]
		public static IOrderedEnumerable<T> ThenBy<T, TT>(this IOrderedEnumerable<T> source, Func<T, TT> selector, Func<TT, TT, int> comparer)
		{
			return source.ThenBy((T obj) => selector(obj), new CollectionsExtensions.DynamicComparer<TT>(comparer));
		}

 		[DebuggerNonUserCode]
		public static IComparer<T> ToComparer<T>(this Func<T, T, int> comparer)
		{
			return new CollectionsExtensions.DynamicComparer<T>(comparer);
		}

 		[DebuggerNonUserCode]
		public static bool Contains<TSource, TResult>(this IEnumerable<TSource> source, TResult value, Func<TSource, TResult> selector)
		{
			foreach (TSource arg in source)
			{
				TResult tresult = selector(arg);
				if (tresult.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

 		[DebuggerNonUserCode]
		public static Expression<Func<T, R>> AsExpression<T, R>(Expression<Func<T, R>> f)
		{
			return f;
		}

 		[DebuggerNonUserCode]
		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic)
		{
			return dic.ToDictionary((KeyValuePair<TKey, TValue> kvp) => kvp.Key, (KeyValuePair<TKey, TValue> kvp) => kvp.Value);
		}

 		[DebuggerNonUserCode]
		public static Func<T, R> AsFunc<T, R>(Func<T, R> f)
		{
			return f;
		}

 		[DebuggerNonUserCode]
		public static bool ArrayEquals(this byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

 		[DebuggerNonUserCode]
		public static bool HasSameElements<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2)
		{
			bool result;
			using (IEnumerator<T> enumerator = seq1.GetEnumerator())
			{
				using (IEnumerator<T> enumerator2 = seq2.GetEnumerator())
				{
					int num = 0;
					int num2 = 0;
					while (enumerator.MoveNext())
					{
						num++;
						if (!enumerator2.MoveNext())
						{
							break;
						}
						num2++;
						T t = enumerator.Current;
						if (!t.Equals(enumerator2.Current))
						{
							return false;
						}
					}
					bool flag = !enumerator2.MoveNext();
					result = (num == num2 && flag);
				}
			}
			return result;
		}

 		[DebuggerNonUserCode]
		public static bool HasSameElementsIgnoreOrder<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2)
		{
			T[] array = seq2.ToArray<T>();
			int num = seq2.Count<T>();
			List<int> list = new List<int>();
			if (seq1.Count<T>() == num)
			{
				foreach (T t in seq1)
				{
					bool flag = true;
					for (int i = 0; i < num; i++)
					{
						if (array[i].Equals(t) && !list.Contains(i))
						{
							list.Add(i);
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

 		[DebuggerNonUserCode]
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> fromEnumerable)
		{
			HashSet<T> hashSet = fromEnumerable as HashSet<T>;
			if (hashSet != null)
			{
				return hashSet;
			}
			return new HashSet<T>(fromEnumerable);
		}

 		[DebuggerNonUserCode]
		public static Func<TKey, TValue> ToFunc<TKey, TValue>(this IDictionary<TKey, TValue> dict)
		{
			return (TKey key) => dict.GetValueOrDefault(key);
		}

 		[DebuggerNonUserCode]
		public static bool IsBetween(this int value, int start, int end)
		{
			return start <= value && value <= end;
		}
 
 		[DebuggerNonUserCode]
		public static ICollection<T> AsCollection<T>(this ICollection<T> collection)
		{
			return collection;
		}
 
		[DebuggerNonUserCode]
		public static TVal GetOrAdd<TKey, TVal>(this IDictionary<TKey, TVal> self, TKey key, Func<TVal> valGetter)
		{
			IDictionary<TKey, TVal> obj = self;
			TVal tval;
			lock (obj)
			{
				if (self.TryGetValue(key, out tval))
				{
					return tval;
				}
			}
			tval = valGetter();
			obj = self;
			TVal result;
			lock (obj)
			{
				self[key] = tval;
				result = tval;
			}
			return result;
		}

 		private static void CheckMoveIsPossible<T, U>(this List<T> list, Func<T, U> elemGetter, U elemToFind)
		{
			if (!list.Contains(elemToFind, elemGetter))
			{
				throw new InvalidOperationException(elemToFind + " does not exist in the list!");
			}
		}

 		private static void CheckMoveIsPossible<T>(this List<T> list, T elem)
		{
			if (!list.Contains(elem))
			{
				throw new InvalidOperationException(elem + " does not exist in the list!");
			}
		}

 		private static void CheckMoveIsPossible<T>(this List<T> list, T elem1, T marker)
		{
			list.CheckMoveIsPossible(elem1);
			list.CheckMoveIsPossible(marker);
		}

 		private static void CheckMoveIsPossible<T, U>(this List<T> list, Func<T, U> elemGetter, U elem1, U marker)
		{
			list.CheckMoveIsPossible(elemGetter, elem1);
			list.CheckMoveIsPossible(elemGetter, marker);
		}

 		public static void MoveAfter<T>(this List<T> list, T elem1, T marker)
		{
			list.CheckMoveIsPossible(elem1, marker);
			list.Remove(elem1);
			int num = list.IndexOf(marker);
			int index = Math.Min(list.Count, num + 1);
			list.Insert(index, elem1);
		}

 		public static void MoveAfter<T>(this List<T> list, IEnumerable<T> elems, T marker)
		{
			T marker2 = marker;
			foreach (T t in elems)
			{
				list.MoveAfter(t, marker2);
				marker2 = t;
			}
		}

 		public static void MoveAfter<T, U>(this List<T> list, Func<T, U> elemGetter, U elemToFind, U marker) where U : class
		{
			list.CheckMoveIsPossible(elemGetter, elemToFind, marker);
			Predicate<T> match = (T t) => elemGetter(t) == elemToFind;
			Predicate<T> match2 = (T t) => elemGetter(t) == marker;
			int from = list.IndexOf(match);
			int to = list.IndexOf(match2) + 1;
			list.Move(from, to);
		}

 		public static void MoveBefore<T>(this List<T> list, T elem1, T marker)
		{
			list.CheckMoveIsPossible(elem1, marker);
			list.Remove(elem1);
			int index = list.IndexOf(marker);
			list.Insert(index, elem1);
		}

 		public static void MoveBefore<T>(this List<T> list, IEnumerable<T> elems, T marker)
		{
			T marker2 = marker;
			foreach (T t in elems)
			{
				list.MoveBefore(t, marker2);
				marker2 = t;
			}
		}

 		public static void MoveBefore<T, U>(this List<T> list, Func<T, U> elemGetter, U elemToFind, U marker) where U : class
		{
			list.CheckMoveIsPossible(elemGetter, elemToFind, marker);
			Predicate<T> match = (T t) => elemGetter(t) == elemToFind;
			Predicate<T> match2 = (T t) => elemGetter(t) == marker;
			int from = list.IndexOf(match);
			int to = list.IndexOf(match2);
			list.Move(from, to);
		}

 		public static void MoveToBeginning<T, U>(this List<T> list, Func<T, U> elemGetter, U elemToFind) where U : class
		{
			list.CheckMoveIsPossible(elemGetter, elemToFind);
			Predicate<T> match = (T t) => elemGetter(t) == elemToFind;
			int from = list.IndexOf(match);
			list.Move(from, 0);
		}

 		public static void MoveToBeginning<T>(this List<T> list, T elem)
		{
			list.CheckMoveIsPossible(elem);
			list.Remove(elem);
			list.Insert(0, elem);
		}

 		public static void MoveToEnd<T>(this List<T> list, T elem)
		{
			list.CheckMoveIsPossible(elem);
			list.Remove(elem);
			list.Add(elem);
		}

 		public static void MoveToEnd<T>(this List<T> list, IEnumerable<T> elems)
		{
			elems.Apply(delegate (T el)
			{
				list.MoveToEnd(el);
			});
		}

 		[DebuggerNonUserCode]
		public static List<T> SafeToList<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable != null)
			{
				return enumerable.ToList<T>();
			}
			return new List<T>();
		}

 		[DebuggerNonUserCode]
		public static IList<T> ToList<T>(this IEnumerator<T> enumerator)
		{
			if (enumerator == null)
			{
				throw new ArgumentNullException();
			}
			List<T> list = new List<T>();
			while (enumerator.MoveNext())
			{
				T item = enumerator.Current;
				list.Add(item);
			}
			return list;
		}

 		private static class CreateDictionaryOf<Value>
		{
 			public static Dictionary<Key, Value> WithKey<Key>(Key prototype)
			{
				return new Dictionary<Key, Value>();
			}
		}

 		public sealed class DynamicComparer<T> : IComparer<T>
		{
 			public DynamicComparer(Func<T, T, int> comparer)
			{
				this.comparer = comparer;
			}

 			public int Compare(T a, T b)
			{
				return this.comparer(a, b);
			}

 			private readonly Func<T, T, int> comparer;
		}

 		private sealed class DynamicEqualityComparer<T, TResult> : IEqualityComparer<T>
		{
 			public DynamicEqualityComparer(Func<T, TResult> selector)
			{
				this._selector = selector;
			}

 			public bool Equals(T x, T y)
			{
				TResult tresult = this._selector(x);
				TResult tresult2 = this._selector(y);
				return tresult.Equals(tresult2);
			}

 			public int GetHashCode(T obj)
			{
				TResult tresult = this._selector(obj);
				return tresult.GetHashCode();
			}

 			private readonly Func<T, TResult> _selector;
		}
	}
}
