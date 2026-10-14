using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D51 RID: 3409
	[NullableContext(1)]
	[Nullable(0)]
	public class CreatureAttackedEntry : CombatHistoryEntry
	{
		// Token: 0x170025E2 RID: 9698
		// (get) Token: 0x06008A0A RID: 35338 RVA: 0x002E3BC7 File Offset: 0x002E1DC7
		public IReadOnlyList<DamageResult> DamageResults { get; }

		// Token: 0x170025E3 RID: 9699
		// (get) Token: 0x06008A0B RID: 35339 RVA: 0x002E3BCF File Offset: 0x002E1DCF
		public override string Description
		{
			get
			{
				return base.Actor.Name + " attacked";
			}
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x002E3BE6 File Offset: 0x002E1DE6
		public CreatureAttackedEntry(Creature attacker, IReadOnlyList<DamageResult> damageResults, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(attacker, roundNumber, currentSide, history, players)
		{
			this.DamageResults = damageResults;
		}
	}
}
