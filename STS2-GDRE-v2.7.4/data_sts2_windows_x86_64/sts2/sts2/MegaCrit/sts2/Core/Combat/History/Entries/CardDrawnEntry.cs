using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D4C RID: 3404
	[NullableContext(1)]
	[Nullable(0)]
	public class CardDrawnEntry : CombatHistoryEntry
	{
		// Token: 0x170025D5 RID: 9685
		// (get) Token: 0x060089F8 RID: 35320 RVA: 0x002E38C6 File Offset: 0x002E1AC6
		public CardModel Card { get; }

		// Token: 0x170025D6 RID: 9686
		// (get) Token: 0x060089F9 RID: 35321 RVA: 0x002E38CE File Offset: 0x002E1ACE
		public bool FromHandDraw { get; }

		// Token: 0x170025D7 RID: 9687
		// (get) Token: 0x060089FA RID: 35322 RVA: 0x002E38D6 File Offset: 0x002E1AD6
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " discarded " + this.Card.Id.Entry;
			}
		}

		// Token: 0x060089FB RID: 35323 RVA: 0x002E390C File Offset: 0x002E1B0C
		public CardDrawnEntry(CardModel card, int roundNumber, CombatSide currentSide, bool fromHandDraw, CombatHistory history, IEnumerable<Player> players)
			: base(card.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Card = card;
			this.FromHandDraw = fromHandDraw;
		}
	}
}
