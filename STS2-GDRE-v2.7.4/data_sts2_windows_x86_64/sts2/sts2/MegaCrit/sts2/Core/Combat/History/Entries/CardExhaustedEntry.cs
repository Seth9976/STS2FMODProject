using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4D RID: 3405
	[NullableContext(1)]
	[Nullable(0)]
	public class CardExhaustedEntry : CombatHistoryEntry
	{
		// Token: 0x170025D8 RID: 9688
		// (get) Token: 0x060089FC RID: 35324 RVA: 0x002E3934 File Offset: 0x002E1B34
		public CardModel Card { get; }

		// Token: 0x170025D9 RID: 9689
		// (get) Token: 0x060089FD RID: 35325 RVA: 0x002E393C File Offset: 0x002E1B3C
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " exhausted " + this.Card.Id.Entry;
			}
		}

		// Token: 0x060089FE RID: 35326 RVA: 0x002E3972 File Offset: 0x002E1B72
		public CardExhaustedEntry(CardModel card, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Card = card;
		}
	}
}
