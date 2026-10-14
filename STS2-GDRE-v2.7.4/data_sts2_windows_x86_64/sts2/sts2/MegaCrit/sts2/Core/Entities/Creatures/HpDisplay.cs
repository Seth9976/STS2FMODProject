using System;

namespace MegaCrit.Sts2.Core.Entities.Creatures
{
	// Token: 0x02000CA3 RID: 3235
	public enum HpDisplay
	{
		/// <summary>
		/// Normal HP display (red background with poison/doom overlays, Current/Max HP numbers).
		/// </summary>
		// Token: 0x04002D63 RID: 11619
		Normal,
		/// <summary>
		/// Purple "infinite" background with Current/Max HP numbers.
		/// </summary>
		// Token: 0x04002D64 RID: 11620
		InfiniteWithNumbers,
		/// <summary>
		/// Purple "infinite" background with infinity symbol instead of Current/Max HP numbers.
		/// </summary>
		// Token: 0x04002D65 RID: 11621
		InfiniteWithoutNumbers
	}
}
