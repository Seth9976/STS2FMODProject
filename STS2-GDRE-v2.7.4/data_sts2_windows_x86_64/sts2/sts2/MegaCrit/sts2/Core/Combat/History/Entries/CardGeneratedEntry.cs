using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4E RID: 3406
	[NullableContext(1)]
	[Nullable(0)]
	public class CardGeneratedEntry : CombatHistoryEntry
	{
		// Token: 0x170025DA RID: 9690
		// (get) Token: 0x060089FF RID: 35327 RVA: 0x002E3992 File Offset: 0x002E1B92
		public CardModel Card { get; }

		// Token: 0x170025DB RID: 9691
		// (get) Token: 0x06008A00 RID: 35328 RVA: 0x002E399A File Offset: 0x002E1B9A
		[Nullable(2)]
		public Player Creator
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025DC RID: 9692
		// (get) Token: 0x06008A01 RID: 35329 RVA: 0x002E39A2 File Offset: 0x002E1BA2
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " generated " + this.Card.Id.Entry + " during combat";
			}
		}

		// Token: 0x06008A02 RID: 35330 RVA: 0x002E39DD File Offset: 0x002E1BDD
		public CardGeneratedEntry(CardModel card, [Nullable(2)] Player creator, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Card = card;
			this.Creator = creator;
		}
	}
}
