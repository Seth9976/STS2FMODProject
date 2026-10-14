using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C0A RID: 3082
	[NullableContext(1)]
	[Nullable(0)]
	public static class HandLayoutHelper
	{
		/// <summary>
		/// Returns where <paramref name="target" /> belongs among <paramref name="presentCards" /> once both are ordered
		/// by <paramref name="pileOrder" />: the count of present cards (excluding the target itself) that precede the
		/// target in pile order.
		///
		/// Callers use this to turn a backend-pile position into a child index for a container that may be missing some
		/// of the pile's cards (selected, awaiting play, or being dragged). Each absent card that precedes the target
		/// inflates the raw pile index past the target's real slot; once enough precede it the index exceeds the live
		/// child count and Godot rejects the move with "Invalid new child index" (PRG-6847). The result cannot exceed
		/// the number of present cards, so when <paramref name="presentCards" /> is exactly the container's current
		/// children it is a valid child index by construction; correctness depends on the caller passing that set.
		/// </summary>
		/// <param name="pileOrder">The full backend pile order.</param>
		/// <param name="presentCards">The cards whose holders are currently in the container, in any order.</param>
		/// <param name="target">The card being (re)inserted.</param>
		/// <returns>The insert index, or -1 if the target is not in the pile (caller should skip the move).</returns>
		// Token: 0x06007FB9 RID: 32697 RVA: 0x002B5770 File Offset: 0x002B3970
		public static int GetInsertIndex<[Nullable(2)] T>(IReadOnlyList<T> pileOrder, IEnumerable<T> presentCards, T target)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int num = HandLayoutHelper.IndexOf<T>(pileOrder, target, @default);
			if (num < 0)
			{
				return -1;
			}
			int num2 = 0;
			foreach (T t in presentCards)
			{
				if (!@default.Equals(t, target))
				{
					int num3 = HandLayoutHelper.IndexOf<T>(pileOrder, t, @default);
					if (num3 >= 0 && num3 < num)
					{
						num2++;
					}
				}
			}
			return num2;
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x002B57F0 File Offset: 0x002B39F0
		private static int IndexOf<[Nullable(2)] T>(IReadOnlyList<T> list, T item, EqualityComparer<T> comparer)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (comparer.Equals(list[i], item))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
