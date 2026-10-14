using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000821 RID: 2081
	[NullableContext(1)]
	[Nullable(0)]
	public class Specialized : ModifierModel
	{
		// Token: 0x06006754 RID: 26452 RVA: 0x002724B4 File Offset: 0x002706B4
		public override Func<Task> GenerateNeowOption(EventModel eventModel)
		{
			return () => Specialized.ObtainCards(eventModel.Owner, eventModel.Rng);
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x002724DC File Offset: 0x002706DC
		private static async Task ObtainCards(Player player, Rng rng)
		{
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), null).WithFlags(CardCreationFlags.NoRarityModification);
			CardModel card = CardFactory.CreateForReward(player, 1, cardCreationOptions).First<CardCreationResult>().Card;
			for (int i = 0; i < 5; i++)
			{
				CardModel cardModel = player.RunState.CloneCard(card);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				results.Add(cardPileAddResult2);
			}
			CardCmd.PreviewCardPileAdd(results, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.CustomScaledWait(0.6f, 1.2f, false, default(CancellationToken));
		}
	}
}
