using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4A RID: 3402
	[NullableContext(1)]
	[Nullable(0)]
	public class CardAfflictedEntry : CombatHistoryEntry
	{
		// Token: 0x170025D0 RID: 9680
		// (get) Token: 0x060089F1 RID: 35313 RVA: 0x002E37FA File Offset: 0x002E19FA
		public CardModel Card { get; }

		// Token: 0x170025D1 RID: 9681
		// (get) Token: 0x060089F2 RID: 35314 RVA: 0x002E3802 File Offset: 0x002E1A02
		public AfflictionModel Affliction { get; }

		// Token: 0x170025D2 RID: 9682
		// (get) Token: 0x060089F3 RID: 35315 RVA: 0x002E380A File Offset: 0x002E1A0A
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " afflicted " + this.Card.Id.Entry;
			}
		}

		// Token: 0x060089F4 RID: 35316 RVA: 0x002E3840 File Offset: 0x002E1A40
		public CardAfflictedEntry(CardModel card, AfflictionModel affliction, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Card = card;
			this.Affliction = affliction;
		}
	}
}
