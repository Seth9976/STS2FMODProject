using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000816 RID: 2070
	public class CursedRun : ModifierModel
	{
		// Token: 0x06006731 RID: 26417 RVA: 0x00271F10 File Offset: 0x00270110
		[NullableContext(1)]
		public override async Task AfterActEntered()
		{
			foreach (Player player in base.RunState.Players)
			{
				CardModel cardModel = base.RunState.Rng.Niche.NextItem<CardModel>(from c in ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
					where c.CanBeGeneratedByModifiers
					select c);
				CardModel cardModel2 = player.RunState.CreateCard(cardModel, player);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel2, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				if (LocalContext.IsMe(player))
				{
					CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
				}
				player = null;
			}
			IEnumerator<Player> enumerator = null;
		}
	}
}
