using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000820 RID: 2080
	[NullableContext(1)]
	[Nullable(0)]
	public class SealedDeck : ModifierModel
	{
		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x0600674F RID: 26447 RVA: 0x002723E4 File Offset: 0x002705E4
		public override bool ClearsPlayerDeck
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x002723E8 File Offset: 0x002705E8
		public override Func<Task> GenerateNeowOption(EventModel eventModel)
		{
			return () => SealedDeck.ChooseCards(eventModel.Owner);
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x00272410 File Offset: 0x00270610
		private static async Task ChooseCards(Player player)
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, null).WithFlags(CardCreationFlags.NoUpgradeRoll | CardCreationFlags.ForceRarityOddsChange | CardCreationFlags.IsCardReward);
			IEnumerable<CardCreationResult> enumerable = CardFactory.CreateForReward(player, 30, cardCreationOptions).ToList<CardCreationResult>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(new LocString("modifiers", "SEALED_DECK.selectionPrompt"), 10);
			cardSelectorPrefs.Cancelable = false;
			cardSelectorPrefs.RequireManualConfirmation = true;
			Comparison<CardModel> comparison;
			if ((comparison = SealedDeck.<>O.<0>__CompareCards) == null)
			{
				comparison = (SealedDeck.<>O.<0>__CompareCards = new Comparison<CardModel>(SealedDeck.CompareCards));
			}
			cardSelectorPrefs.Comparison = comparison;
			CardSelectorPrefs cardSelectorPrefs2 = cardSelectorPrefs;
			IEnumerable<CardModel> enumerable2 = await CardSelectCmd.FromSimpleGridForRewards(new BlockingPlayerChoiceContext(), enumerable.ToList<CardCreationResult>(), player, cardSelectorPrefs2);
			List<CardModel> list = enumerable2.ToList<CardModel>();
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.Add(list, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.GridLayout);
			foreach (Player player2 in player.RunState.Players)
			{
				player2.RelicGrabBag.Remove<PandorasBox>();
			}
			player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x00272454 File Offset: 0x00270654
		private static int CompareCards(CardModel card1, CardModel card2)
		{
			if (card1.Rarity != card2.Rarity)
			{
				return card1.Rarity.CompareTo(card2.Rarity);
			}
			return string.Compare(card1.Title, card2.Title, LocManager.Instance.CultureInfo, CompareOptions.None);
		}

		// Token: 0x02001ED8 RID: 7896
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04007DE8 RID: 32232
			[Nullable(new byte[] { 0, 1 })]
			public static Comparison<CardModel> <0>__CompareCards;
		}
	}
}
