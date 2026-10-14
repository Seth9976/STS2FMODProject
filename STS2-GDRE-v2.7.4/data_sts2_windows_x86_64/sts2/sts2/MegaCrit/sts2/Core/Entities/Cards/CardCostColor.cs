using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CA8 RID: 3240
	public enum CardCostColor
	{
		/// <summary>
		/// "Normal" cost. Colored white.
		/// </summary>
		// Token: 0x04002D71 RID: 11633
		Unmodified,
		/// <summary>
		/// The current cost is modified to above normal. Colored blue.
		/// </summary>
		// Token: 0x04002D72 RID: 11634
		Increased,
		/// <summary>
		/// The current cost is modified to at/below normal. Colored green.
		/// </summary>
		// Token: 0x04002D73 RID: 11635
		Decreased,
		/// <summary>
		/// The card cannot be played because the player does not have enough resources.
		/// </summary>
		// Token: 0x04002D74 RID: 11636
		InsufficientResources
	}
}
