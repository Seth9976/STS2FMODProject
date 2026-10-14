using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D53 RID: 3411
	[NullableContext(1)]
	[Nullable(0)]
	public class EnergySpentEntry : CombatHistoryEntry
	{
		// Token: 0x06008A14 RID: 35348 RVA: 0x002E3D31 File Offset: 0x002E1F31
		public EnergySpentEntry(int amount, Player player, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(player.Creature, roundNumber, currentSide, history, players)
		{
		}

		// Token: 0x170025E9 RID: 9705
		// (get) Token: 0x06008A15 RID: 35349 RVA: 0x002E3D4D File Offset: 0x002E1F4D
		public int Amount { get; } = amount;

		// Token: 0x170025EA RID: 9706
		// (get) Token: 0x06008A16 RID: 35350 RVA: 0x002E3D55 File Offset: 0x002E1F55
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " spent Amount energy";
			}
		}
	}
}
