using System;

namespace MegaCrit.Sts2.Core.Entities.TreasureRelicPicking
{
	// Token: 0x02000C5F RID: 3167
	public enum RelicPickingResultType
	{
		/// <summary>
		/// A single player voted for the relic, and that player received the relic.
		/// </summary>
		// Token: 0x04002BC2 RID: 11202
		OnlyOnePlayerVoted,
		/// <summary>
		/// Multiple players voted for the relic. The details of the fight are in the fight member.
		/// </summary>
		// Token: 0x04002BC3 RID: 11203
		FoughtOver,
		/// <summary>
		/// No player voted for the relic. The player that received it was a loser of a fight.
		/// </summary>
		// Token: 0x04002BC4 RID: 11204
		ConsolationPrize,
		/// <summary>
		/// No player voted for the relic, and no player got the relic because they skipped over it.
		/// </summary>
		// Token: 0x04002BC5 RID: 11205
		Skipped
	}
}
