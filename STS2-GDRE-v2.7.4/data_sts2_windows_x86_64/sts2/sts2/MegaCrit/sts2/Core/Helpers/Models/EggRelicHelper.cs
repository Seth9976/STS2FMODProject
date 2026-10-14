using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Helpers.Models
{
	// Token: 0x02000C1B RID: 3099
	public static class EggRelicHelper
	{
		// Token: 0x06008000 RID: 32768 RVA: 0x002B76F8 File Offset: 0x002B58F8
		[NullableContext(1)]
		public static void UpgradeValidCards(List<CardCreationResult> cards, CardType cardType, RelicModel eggRelic)
		{
			foreach (CardCreationResult cardCreationResult in cards)
			{
				CardModel card = cardCreationResult.Card;
				if (card.Type == cardType && card.IsUpgradable)
				{
					CardModel cardModel = eggRelic.Owner.RunState.CloneCard(card);
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
					cardCreationResult.ModifyCard(cardModel, eggRelic);
				}
			}
		}
	}
}
