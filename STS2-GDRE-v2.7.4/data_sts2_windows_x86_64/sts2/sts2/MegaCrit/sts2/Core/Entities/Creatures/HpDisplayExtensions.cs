using System;

namespace MegaCrit.Sts2.Core.Entities.Creatures
{
	// Token: 0x02000CA4 RID: 3236
	public static class HpDisplayExtensions
	{
		/// <summary>
		/// Does this HP display indicate infinite HP?
		/// </summary>
		// Token: 0x0600846C RID: 33900 RVA: 0x002C9D68 File Offset: 0x002C7F68
		public static bool IsInfinite(this HpDisplay display)
		{
			return display - HpDisplay.InfiniteWithNumbers <= 1;
		}

		/// <summary>
		/// Does this HP display show Current/Max HP numbers?
		/// </summary>
		// Token: 0x0600846D RID: 33901 RVA: 0x002C9D84 File Offset: 0x002C7F84
		public static bool ShowsNumbers(this HpDisplay display)
		{
			return display <= HpDisplay.InfiniteWithNumbers;
		}
	}
}
