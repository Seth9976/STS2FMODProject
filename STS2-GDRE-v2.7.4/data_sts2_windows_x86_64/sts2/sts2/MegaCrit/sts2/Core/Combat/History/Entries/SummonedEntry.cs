using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D59 RID: 3417
	[NullableContext(1)]
	[Nullable(0)]
	public class SummonedEntry : CombatHistoryEntry
	{
		// Token: 0x06008A2C RID: 35372 RVA: 0x002E41B0 File Offset: 0x002E23B0
		public SummonedEntry(int amount, Player player, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(player.Creature, roundNumber, currentSide, history, players)
		{
		}

		// Token: 0x170025FA RID: 9722
		// (get) Token: 0x06008A2D RID: 35373 RVA: 0x002E41CC File Offset: 0x002E23CC
		public int Amount { get; } = amount;

		// Token: 0x170025FB RID: 9723
		// (get) Token: 0x06008A2E RID: 35374 RVA: 0x002E41D4 File Offset: 0x002E23D4
		public override string Description
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendFormatted(base.Actor.Player.Character.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" summoned ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Amount);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
