using System;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// The rarity used by Badges in the Run Summary screen.
	/// The rarity is based on how hard they are to get and reward bonus points the harder they are.
	/// Used for tiebreakers amongst players who have a tied score in the Leaderboards.
	/// </summary>
	// Token: 0x02000B5B RID: 2907
	public enum BadgeRarity
	{
		// Token: 0x040028FA RID: 10490
		None,
		// Token: 0x040028FB RID: 10491
		Bronze,
		// Token: 0x040028FC RID: 10492
		Silver,
		// Token: 0x040028FD RID: 10493
		Gold
	}
}
