using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CCB RID: 3275
	[Flags]
	public enum UnplayableReason
	{
		// Token: 0x04002E0E RID: 11790
		None = 0,
		/// <summary>
		/// The card has the Unplayable keyword, or its UnplayableThisTurn flag is set to true.
		/// </summary>
		// Token: 0x04002E0F RID: 11791
		HasUnplayableKeyword = 2,
		/// <summary>
		/// Something like Normality is blocking the card from being played.
		/// </summary>
		// Token: 0x04002E10 RID: 11792
		BlockedByHook = 4,
		/// <summary>
		/// The card itself has a built-in reason that is blocking its play, like how Grand Finale can't be played unless
		/// your draw pile is empty.
		/// </summary>
		// Token: 0x04002E11 RID: 11793
		BlockedByCardLogic = 8,
		/// <summary>
		/// You don't have enough energy to play the card.
		/// </summary>
		// Token: 0x04002E12 RID: 11794
		EnergyCostTooHigh = 16,
		/// <summary>
		/// You don't have enough stars to play the card.
		/// </summary>
		// Token: 0x04002E13 RID: 11795
		StarCostTooHigh = 32,
		/// <summary>
		/// You have no living allies you can target.
		/// </summary>
		// Token: 0x04002E14 RID: 11796
		NoLivingAllies = 64
	}
}
