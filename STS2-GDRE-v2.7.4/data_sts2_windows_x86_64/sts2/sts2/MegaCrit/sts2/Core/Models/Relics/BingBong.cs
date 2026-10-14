using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000502 RID: 1282
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BingBong : RelicModel
	{
		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06004F32 RID: 20274 RVA: 0x0023CE33 File Offset: 0x0023B033
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06004F33 RID: 20275 RVA: 0x0023CE36 File Offset: 0x0023B036
		private HashSet<CardModel> CardsToSkip
		{
			get
			{
				base.AssertMutable();
				if (this._cardsToSkip == null)
				{
					this._cardsToSkip = new HashSet<CardModel>();
				}
				return this._cardsToSkip;
			}
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x0023CE58 File Offset: 0x0023B058
		public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			CardPile pile = card.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				if (card.Owner == base.Owner)
				{
					if (clonedBy == null)
					{
						if (!this.CardsToSkip.Remove(card))
						{
							base.Flash();
							CardModel cardModel = base.Owner.RunState.CloneCard(card);
							this.CardsToSkip.Add(cardModel);
							CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, this, false);
							CardPileAddResult cardPileAddResult2 = cardPileAddResult;
							CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
						}
					}
				}
			}
		}

		// Token: 0x04002426 RID: 9254
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<CardModel> _cardsToSkip;
	}
}
