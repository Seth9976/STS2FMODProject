using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D58 RID: 3416
	[NullableContext(1)]
	[Nullable(0)]
	public class StarsModifiedEntry : CombatHistoryEntry
	{
		// Token: 0x06008A29 RID: 35369 RVA: 0x002E40FF File Offset: 0x002E22FF
		public StarsModifiedEntry(int amount, Player player, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(player.Creature, roundNumber, currentSide, history, players)
		{
		}

		// Token: 0x170025F8 RID: 9720
		// (get) Token: 0x06008A2A RID: 35370 RVA: 0x002E411B File Offset: 0x002E231B
		public int Amount { get; } = amount;

		// Token: 0x170025F9 RID: 9721
		// (get) Token: 0x06008A2B RID: 35371 RVA: 0x002E4124 File Offset: 0x002E2324
		public override string Description
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 3);
				defaultInterpolatedStringHandler.AppendFormatted(base.Actor.Player.Character.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted((this.Amount < 0) ? "lost" : "gained");
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Amount);
				defaultInterpolatedStringHandler.AppendLiteral(" star(s)");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
