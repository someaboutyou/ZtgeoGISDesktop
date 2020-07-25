using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
	[Serializable]
	public class Pair<T1, T2> : IPair
	{ 
		object IPair.First
		{
			get
			{
				return this.First;
			}
		}
		 
		object IPair.Second
		{
			get
			{
				return this.Second;
			}
		}
		 
		public T1 First { get; set; }
		 
		public T2 Second { get; set; }

 		public Pair()
		{
		}

 		public Pair(T1 first, T2 second)
		{
			this.First = first;
			this.Second = second;
		}

 		public override int GetHashCode()
		{
			int num;
			if (this.First != null)
			{
				T1 first = this.First;
				num = first.GetHashCode();
			}
			else
			{
				num = 0;
			}
			int num2;
			if (this.Second != null)
			{
				T2 second = this.Second;
				num2 = second.GetHashCode();
			}
			else
			{
				num2 = 0;
			}
			return num ^ num2;
		}

 		public override bool Equals(object obj)
		{
			Pair<T1, T2> pair = obj as Pair<T1, T2>;
			return this == pair;
		}

 		public static bool operator ==(Pair<T1, T2> pair1, Pair<T1, T2> pair2)
		{
			if (pair1 == null)
			{
				return pair2 == null;
			}
			if (pair2 == null)
			{
				return false;
			}
			if (pair1.First == null)
			{
				if (pair2.First != null)
				{
					return false;
				}
			}
			else
			{
				T1 first = pair1.First;
				if (!first.Equals(pair2.First))
				{
					return false;
				}
			}
			if (pair1.Second == null)
			{
				if (pair2.Second != null)
				{
					return false;
				}
			}
			else
			{
				T2 second = pair1.Second;
				if (!second.Equals(pair2.Second))
				{
					return false;
				}
			}
			return true;
		}

 		public static bool operator !=(Pair<T1, T2> pair1, Pair<T1, T2> pair2)
		{
			if (pair1 == null)
			{
				return pair2 != null;
			}
			if (pair2 == null)
			{
				return true;
			}
			if (pair1.First == null)
			{
				if (pair2.First != null)
				{
					return true;
				}
			}
			else
			{
				T1 first = pair1.First;
				if (!first.Equals(pair2.First))
				{
					return true;
				}
			}
			if (pair1.Second == null)
			{
				if (pair2.Second != null)
				{
					return true;
				}
			}
			else
			{
				T2 second = pair1.Second;
				if (!second.Equals(pair2.Second))
				{
					return true;
				}
			}
			return false;
		}

 		public override string ToString()
		{
			return string.Format("<{0},{1}>", this.First, this.Second);
		}
	}

	public static class Pair
	{ 
		public static Pair<T1, T2> Create<T1, T2>(T1 first, T2 second)
		{
			return new Pair<T1, T2>(first, second);
		}
	}
}
