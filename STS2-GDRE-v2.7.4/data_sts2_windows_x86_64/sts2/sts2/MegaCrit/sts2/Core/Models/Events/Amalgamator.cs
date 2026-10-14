using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000829 RID: 2089
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Amalgamator : EventModel
	{
		// Token: 0x0600676A RID: 26474 RVA: 0x002728DF File Offset: 0x00270ADF
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>(delegate(Player p)
			{
				if (p.Deck.Cards.Count<CardModel>((CardModel c) => Amalgamator.IsValid(CardTag.Strike, c)) >= 2)
				{
					return p.Deck.Cards.Count<CardModel>((CardModel c) => Amalgamator.IsValid(CardTag.Defend, c)) >= 2;
				}
				return false;
			});
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600676B RID: 26475 RVA: 0x0027290B File Offset: 0x00270B0B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Card1", ModelDb.Card<UltimateStrike>().Title),
					new StringVar("Card2", ModelDb.Card<UltimateDefend>().Title)
				});
			}
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x00272948 File Offset: 0x00270B48
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.CombineStrikes), base.InitialOptionKey("COMBINE_STRIKES"), HoverTipFactory.FromCardWithCardHoverTips<UltimateStrike>(false)),
				new EventOption(this, new Func<Task>(this.CombineDefends), base.InitialOptionKey("COMBINE_DEFENDS"), HoverTipFactory.FromCardWithCardHoverTips<UltimateDefend>(false))
			});
		}

		// Token: 0x0600676D RID: 26477 RVA: 0x002729AC File Offset: 0x00270BAC
		private async Task CombineStrikes()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, (CardModel c) => Amalgamator.IsValid(CardTag.Strike, c));
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("card_smith.mp3", 1f, PitchVariance.Small);
			}
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
			await Task.Delay(300);
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 != null)
			{
				instance2.Play("card_smith.mp3", 1f, PitchVariance.Small);
			}
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
			CardModel cardModel = base.Owner.RunState.CreateCard<UltimateStrike>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("AMALGAMATOR.pages.COMBINE_STRIKES.description"));
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x002729F0 File Offset: 0x00270BF0
		private async Task CombineDefends()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, (CardModel c) => Amalgamator.IsValid(CardTag.Defend, c));
			List<CardModel> list = enumerable.ToList<CardModel>();
			await CardPileCmd.RemoveFromDeck(list, true);
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("card_smith.mp3", 1f, PitchVariance.Small);
			}
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
			await Task.Delay(300);
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 != null)
			{
				instance2.Play("card_smith.mp3", 1f, PitchVariance.Small);
			}
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Strong);
			CardModel cardModel = base.Owner.RunState.CreateCard<UltimateDefend>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("AMALGAMATOR.pages.COMBINE_DEFENDS.description"));
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x00272A33 File Offset: 0x00270C33
		private static bool IsValid(CardTag tag, CardModel card)
		{
			return card.Tags.Contains(tag) && (card != null && card.Rarity == CardRarity.Basic) && card.IsRemovable;
		}
	}
}
