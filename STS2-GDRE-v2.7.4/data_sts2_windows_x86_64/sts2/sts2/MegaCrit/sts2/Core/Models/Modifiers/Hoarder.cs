using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x0200081B RID: 2075
	[NullableContext(1)]
	[Nullable(0)]
	public class Hoarder : ModifierModel
	{
		// Token: 0x0600673E RID: 26430 RVA: 0x002720A0 File Offset: 0x002702A0
		public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			if (oldPileType == PileType.None)
			{
				CardPile pile = card.Pile;
				if (pile != null && pile.Type == PileType.Deck)
				{
					if (clonedBy == null)
					{
						if (!this._cardsToSkip.Remove(card))
						{
							for (int i = 0; i < 2; i++)
							{
								CardModel cardModel = card.Owner.RunState.CloneCard(card);
								this._cardsToSkip.Add(cardModel);
								CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, this, false);
								CardPileAddResult cardPileAddResult2 = cardPileAddResult;
								CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x002720FB File Offset: 0x002702FB
		public override bool ShouldAllowMerchantCardRemoval(Player player)
		{
			return false;
		}

		// Token: 0x04002750 RID: 10064
		private readonly HashSet<CardModel> _cardsToSkip = new HashSet<CardModel>();
	}
}
