using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// What algorithm should be used to apply a <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier" /> to a card's cost.
	/// </summary>
	// Token: 0x02000CC2 RID: 3266
	public enum LocalCostType
	{
		// Token: 0x04002DE9 RID: 11753
		None,
		/// <summary>
		/// The card's local cost should be reset to this specific value.
		/// </summary>
		// Token: 0x04002DEA RID: 11754
		Absolute,
		/// <summary>
		/// The card's current local cost should be offset by this amount.
		/// </summary>
		// Token: 0x04002DEB RID: 11755
		Relative
	}
}
