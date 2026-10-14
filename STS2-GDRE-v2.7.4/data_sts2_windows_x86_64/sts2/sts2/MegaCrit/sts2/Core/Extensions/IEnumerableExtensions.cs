using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Extensions
{
	// Token: 0x02000C46 RID: 3142
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Gets a random selection of elements from an IEnumerable.
		/// </summary>
		/// <typeparam name="T">Type of elements in the IEnumerable.</typeparam>
		/// <param name="collection">IEnumerable to get random elements from.</param>
		/// <param name="count">Maximum number of elements that should be in the new IEnumerable.</param>
		/// <param name="rng">Random number generator to use for shuffling.</param>
		/// <returns>New IEnumerable with random elements.</returns>
		// Token: 0x06008192 RID: 33170 RVA: 0x002BF119 File Offset: 0x002BD319
		[NullableContext(1)]
		public static IEnumerable<T> TakeRandom<[Nullable(2)] T>(this IEnumerable<T> collection, int count, Rng rng)
		{
			return collection.ToList<T>().UnstableShuffle<T>(rng).Take<T>(count);
		}
	}
}
