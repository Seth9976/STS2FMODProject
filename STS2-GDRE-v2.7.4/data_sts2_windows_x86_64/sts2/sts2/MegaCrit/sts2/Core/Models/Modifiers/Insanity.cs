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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x0200081C RID: 2076
	[NullableContext(1)]
	[Nullable(0)]
	public class Insanity : ModifierModel
	{
		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x06006741 RID: 26433 RVA: 0x00272111 File Offset: 0x00270311
		public override bool ClearsPlayerDeck
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x00272114 File Offset: 0x00270314
		public override Func<Task> GenerateNeowOption(EventModel eventModel)
		{
			return () => Insanity.ObtainCards(eventModel.Owner, eventModel.Rng);
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x0027213C File Offset: 0x0027033C
		private static async Task ObtainCards(Player player, Rng rng)
		{
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			for (int i = 0; i < 30; i++)
			{
				CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), null).WithFlags(CardCreationFlags.NoRarityModification);
				CardModel card = CardFactory.CreateForReward(player, 1, cardCreationOptions).First<CardCreationResult>().Card;
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				results.Add(cardPileAddResult2);
			}
			foreach (CardPileAddResult cardPileAddResult3 in results)
			{
				CardCmd.PreviewCardPileAdd(cardPileAddResult3, 1.2f, CardPreviewStyle.MessyLayout);
				await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
			}
			List<CardPileAddResult>.Enumerator enumerator = default(List<CardPileAddResult>.Enumerator);
			await Cmd.CustomScaledWait(0.6f, 1.2f, false, default(CancellationToken));
			foreach (Player player2 in player.RunState.Players)
			{
				player2.RelicGrabBag.Remove<PandorasBox>();
			}
			player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
		}
	}
}
