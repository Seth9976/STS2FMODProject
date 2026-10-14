using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Extensions
{
	// Token: 0x02000C47 RID: 3143
	[NullableContext(1)]
	[Nullable(0)]
	public static class ListExtensions
	{
		/// <summary>
		/// Adds a random Shuffle method to any List.
		/// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
		///
		/// This shuffle is "stable", meaning the result is independent of the initial order of the list.
		/// If two lists containing the same items in different orders are shuffled, the results will always be the
		/// same, so long as the passed rng is the same.
		/// </summary>
		/// <typeparam name="T">Type of elements in the list.</typeparam>
		/// <param name="list">List to shuffle.</param>
		/// <param name="rng">Random number generator to use for sorting</param>
		/// <returns>Self.</returns>
		// Token: 0x06008193 RID: 33171 RVA: 0x002BF130 File Offset: 0x002BD330
		public static List<T> StableShuffle<[Nullable(0)] T>(this List<T> list, Rng rng) where T : IComparable<T>
		{
			List<T> list2 = list.ToList<T>();
			list2.Sort();
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = list2[i];
			}
			return list.UnstableShuffle<T>(rng);
		}

		/// <summary>
		/// Adds a random Shuffle method to any List.
		/// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
		///
		/// This shuffle is "unstable", meaning the result is dependent on the initial order of the list.
		/// If two lists containing the same items in different orders are shuffled, the results may be different even
		/// if the passed rng is the same.
		/// </summary>
		/// <typeparam name="T">Type of elements in the list.</typeparam>
		/// <param name="list">List to shuffle.</param>
		/// <param name="rng">Random number generator to use for sorting</param>
		/// <returns>Self.</returns>
		// Token: 0x06008194 RID: 33172 RVA: 0x002BF170 File Offset: 0x002BD370
		public static List<T> UnstableShuffle<[Nullable(2)] T>(this List<T> list, Rng rng)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int num = rng.NextInt(i + 1);
				int num2 = num;
				int num3 = i;
				T t = list[i];
				T t2 = list[num];
				list[num2] = t;
				list[num3] = t2;
			}
			return list;
		}

		/// <summary>
		/// Determines the index of a specific item in the IReadOnlyList.
		/// </summary>
		/// <param name="readOnlyList">The IReadOnlyList to locate the object in.</param>
		/// <param name="item">The object to locate in the IReadOnlyList.</param>
		/// <typeparam name="T">Type of elements in the list.</typeparam>
		/// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06008195 RID: 33173 RVA: 0x002BF1C8 File Offset: 0x002BD3C8
		public static int IndexOf<[Nullable(2)] T>(this IReadOnlyList<T> readOnlyList, T item)
		{
			IList<T> list = readOnlyList as IList<T>;
			if (list != null)
			{
				return list.IndexOf(item);
			}
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				if (EqualityComparer<T>.Default.Equals(readOnlyList[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Finds the first index of an item that matches the specified predicate in the IReadOnlyList.
		/// </summary>
		/// <param name="readOnlyList">The IReadOnlyList to locate the object in.</param>
		/// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">Type of elements in the list.</typeparam>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by match, if
		/// found; otherwise, -1.
		/// </returns>
		// Token: 0x06008196 RID: 33174 RVA: 0x002BF210 File Offset: 0x002BD410
		public static int FirstIndex<[Nullable(2)] T>(this IReadOnlyList<T> readOnlyList, Predicate<T> match)
		{
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				if (match(readOnlyList[i]))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
