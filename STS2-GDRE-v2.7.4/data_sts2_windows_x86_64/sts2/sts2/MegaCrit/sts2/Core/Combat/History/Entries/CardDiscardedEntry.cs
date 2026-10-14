using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4B RID: 3403
	[NullableContext(1)]
	[Nullable(0)]
	public class CardDiscardedEntry : CombatHistoryEntry
	{
		// Token: 0x170025D3 RID: 9683
		// (get) Token: 0x060089F5 RID: 35317 RVA: 0x002E3868 File Offset: 0x002E1A68
		public CardModel Card { get; }

		// Token: 0x170025D4 RID: 9684
		// (get) Token: 0x060089F6 RID: 35318 RVA: 0x002E3870 File Offset: 0x002E1A70
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " discarded " + this.Card.Id.Entry;
			}
		}

		// Token: 0x060089F7 RID: 35319 RVA: 0x002E38A6 File Offset: 0x002E1AA6
		public CardDiscardedEntry(CardModel card, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Card = card;
		}
	}
}
