using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Entities.TreasureRelicPicking
{
	// Token: 0x02000C5B RID: 3163
	[NullableContext(1)]
	[Nullable(0)]
	public class RelicPickingFight
	{
		// Token: 0x04002BB6 RID: 11190
		public List<Player> playersInvolved = new List<Player>();

		// Token: 0x04002BB7 RID: 11191
		public List<RelicPickingFightRound> rounds = new List<RelicPickingFightRound>();
	}
}
