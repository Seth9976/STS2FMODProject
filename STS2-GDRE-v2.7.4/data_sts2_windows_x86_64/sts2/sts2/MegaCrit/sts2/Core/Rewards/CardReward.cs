using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000145 RID: 325
	[NullableContext(1)]
	[Nullable(0)]
	public class CardReward : Reward
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0005F8A5 File Offset: 0x0005DAA5
		private static string RareRewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_rare.png");
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0005F8B1 File Offset: 0x0005DAB1
		private static string UncommonRewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_uncommon.png");
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0005F8BD File Offset: 0x0005DABD
		private static string RewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card.png");
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0005F8C9 File Offset: 0x0005DAC9
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.Card;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0005F8CC File Offset: 0x0005DACC
		public override int RewardsSetIndex
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0005F8D0 File Offset: 0x0005DAD0
		protected override string IconPath
		{
			get
			{
				CardCreationOptions options = this.Options;
				if (options != null && options.Source == CardCreationSource.Encounter && options.RarityOdds == CardRarityOddsType.BossEncounter)
				{
					return CardReward.RareRewardIcon;
				}
				if (this.Options.TryGetSingleRarityInPool() != null)
				{
					CardRarity rarity = this._cards[0].Card.Rarity;
					string text;
					if (rarity != CardRarity.Uncommon)
					{
						if (rarity == CardRarity.Rare)
						{
							text = CardReward.RareRewardIcon;
						}
						else
						{
							text = CardReward.RewardIcon;
						}
					}
					else
					{
						text = CardReward.UncommonRewardIcon;
					}
					return text;
				}
				return CardReward.RewardIcon;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0005F950 File Offset: 0x0005DB50
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					CardReward.RareRewardIcon,
					CardReward.UncommonRewardIcon,
					CardReward.RewardIcon
				});
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0005F975 File Offset: 0x0005DB75
		public override LocString Description
		{
			get
			{
				return new LocString("gameplay_ui", "COMBAT_REWARD_ADD_CARD");
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0005F986 File Offset: 0x0005DB86
		public IEnumerable<CardModel> Cards
		{
			get
			{
				return this._cards.Select<CardCreationResult, CardModel>((CardCreationResult e) => e.Card);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000F97 RID: 3991 RVA: 0x0005F9B4 File Offset: 0x0005DBB4
		// (remove) Token: 0x06000F98 RID: 3992 RVA: 0x0005F9EC File Offset: 0x0005DBEC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action AfterGenerated;

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0005FA21 File Offset: 0x0005DC21
		private int OptionCount { get; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0005FA29 File Offset: 0x0005DC29
		private CardCreationOptions Options { get; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0005FA31 File Offset: 0x0005DC31
		private CardCreationOptions RerollOptions { get; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0005FA39 File Offset: 0x0005DC39
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x0005FA41 File Offset: 0x0005DC41
		public bool CanReroll { get; set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0005FA4A File Offset: 0x0005DC4A
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x0005FA52 File Offset: 0x0005DC52
		public bool CanSkip { get; set; } = true;

		/// <summary>
		/// Create a card reward from a specified source, using the current character's card pool and the source's
		/// rarity-rolling logic.
		/// This is the typical way one would generate a card reward for something like the end of combat.
		/// </summary>
		/// <param name="options">Options used to generate the card rewards.</param>
		/// <param name="cardCount">How many choices to offer (usually 3).</param>
		/// <param name="player">The player that this reward is for.</param>
		/// <param name="synchronizer">PlayerChoiceSynchronizer, for injection in tests.</param>
		// Token: 0x06000FA0 RID: 4000 RVA: 0x0005FA5C File Offset: 0x0005DC5C
		public CardReward(CardCreationOptions options, int cardCount, Player player, [Nullable(2)] PlayerChoiceSynchronizer synchronizer = null)
			: base(player)
		{
			this.OptionCount = cardCount;
			this.Options = options.WithFlags(CardCreationFlags.IsCardReward);
			this.RerollOptions = options.WithFlags(CardCreationFlags.IsCardReward);
			this._synchronizer = synchronizer ?? RunManager.Instance.PlayerChoiceSynchronizer;
			player.RelicObtained += this.OnRelicObtained;
		}

		/// <summary>
		/// Create a card reward with specific cards in it. This is for exceptionally rare cases where we want to offer a
		/// very specific set of cards that ignores modification, like during the tutorial run.
		/// Note that <see cref="M:MegaCrit.Sts2.Core.Rewards.CardReward.Populate" /> will do nothing if called after this method.
		/// </summary>
		/// <param name="cardsToOffer">List of cards to offer as rewards.</param>
		/// <param name="source">The source of the reward.</param>
		/// <param name="player">The player that this reward is for.</param>
		/// <param name="rerollOptions">If the options are rerolled with <see cref="T:MegaCrit.Sts2.Core.Models.Relics.Driftwood" />, then these options will
		/// be used to generate a new set of cards.</param>
		/// <param name="synchronizer">PlayerChoiceSynchronizer, for injection in tests.</param>
		// Token: 0x06000FA1 RID: 4001 RVA: 0x0005FAD4 File Offset: 0x0005DCD4
		public CardReward(IEnumerable<CardModel> cardsToOffer, CardCreationSource source, Player player, CardCreationOptions rerollOptions, [Nullable(2)] PlayerChoiceSynchronizer synchronizer = null)
			: base(player)
		{
			this.Options = new CardCreationOptions(Array.Empty<CardPoolModel>(), source, CardRarityOddsType.Uniform, null).WithFlags(CardCreationFlags.NoCardPoolModifications | CardCreationFlags.NoCardModelModifications | CardCreationFlags.IsCardReward);
			this.RerollOptions = rerollOptions;
			this._cardsWereManuallySet = true;
			this._cards = cardsToOffer.Select<CardModel, CardCreationResult>((CardModel c) => new CardCreationResult(c)).ToList<CardCreationResult>();
			this.OptionCount = this._cards.Count;
			this._synchronizer = synchronizer ?? RunManager.Instance.PlayerChoiceSynchronizer;
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0005FB7D File Offset: 0x0005DD7D
		public override bool IsPopulated
		{
			get
			{
				return this._cards.Count > 0;
			}
		}

		/// <summary>
		/// Called when the CardReward rolls options for the first time, and may be called again afterward to generate a new
		/// set of the same type of rewards.
		/// </summary>
		// Token: 0x06000FA3 RID: 4003 RVA: 0x0005FB90 File Offset: 0x0005DD90
		public override void Populate()
		{
			CardCreationOptions cardCreationOptions = (this._hasBeenRerolled ? this.RerollOptions : this.Options);
			if (this._cardsWereManuallySet && !this._hasBeenRerolled)
			{
				List<AbstractModel> list;
				if (Hook.TryModifyCardRewardOptions(base.Player.RunState, base.Player, this._cards, cardCreationOptions, out list))
				{
					TaskHelper.RunSafely(Hook.AfterModifyingCardRewardOptions(base.Player.RunState, list));
				}
				return;
			}
			if (this._cards.Count > 0)
			{
				return;
			}
			IEnumerable<CardCreationResult> enumerable = CardFactory.CreateForReward(base.Player, this.OptionCount, cardCreationOptions);
			this._cards.Clear();
			this._cards.AddRange(enumerable);
			IReadOnlyList<CardRewardAlternative> readOnlyList = CardRewardAlternative.Generate(this);
			Action afterGenerated = this.AfterGenerated;
			if (afterGenerated != null)
			{
				afterGenerated();
			}
			NCardRewardSelectionScreen currentlyShownScreen = this._currentlyShownScreen;
			if (currentlyShownScreen == null)
			{
				return;
			}
			currentlyShownScreen.RefreshOptions(this._cards, readOnlyList);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0005FC68 File Offset: 0x0005DE68
		private void OnRelicObtained(RelicModel relic)
		{
			if (this._cards == null)
			{
				throw new InvalidOperationException("cards must be set first before you can update them");
			}
			if (relic.TryModifyCardRewardOptions(base.Player, this._cards, this.Options))
			{
				TaskHelper.RunSafely(relic.AfterModifyingRewards());
			}
			if (relic.TryModifyCardRewardOptionsLate(base.Player, this._cards, this.Options))
			{
				TaskHelper.RunSafely(relic.AfterModifyingRewards());
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0005FCD4 File Offset: 0x0005DED4
		protected override async Task<bool> OnSelect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" selected card reward");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			bool rewardComplete = false;
			bool endSelection = false;
			List<CardModel> chosenCardIds = new List<CardModel>();
			IReadOnlyList<CardRewardAlternative> cardRewardOption = CardRewardAlternative.Generate(this);
			if (LocalContext.IsMe(base.Player))
			{
				this._currentlyShownScreen = NCardRewardSelectionScreen.ShowScreen(this._cards, cardRewardOption);
			}
			while (!endSelection)
			{
				CardReward.<>c__DisplayClass49_0 CS$<>8__locals1 = new CardReward.<>c__DisplayClass49_0();
				uint choiceId = this._synchronizer.ReserveChoiceId(base.Player);
				int? num2;
				if (LocalContext.IsMe(base.Player))
				{
					if (this._currentlyShownScreen != null)
					{
						int? num = await this._currentlyShownScreen.OptionSelected();
						num2 = num;
					}
					else
					{
						MegaCrit.Sts2.Core.TestSupport.ICardSelector selector = CardSelectCmd.Selector;
						CardRewardSelection? selection = ((selector != null) ? new CardRewardSelection?(selector.GetSelectedCardReward(this._cards, cardRewardOption)) : null);
						if (selection == null)
						{
							throw new InvalidOperationException("Card selector unset during test!");
						}
						if (selection.Value.alternative != null)
						{
							num2 = new int?(this._cards.Count + cardRewardOption.FirstIndex<CardRewardAlternative>((CardRewardAlternative r) => r == selection.Value.alternative));
						}
						else
						{
							CS$<>8__locals1.obtainedCard = selection.Value.card;
							num2 = ((CS$<>8__locals1.obtainedCard != null) ? new int?(this._cards.FirstIndex<CardCreationResult>((CardCreationResult c) => c.Card == selection.Value.card)) : null);
						}
					}
					PlayerChoiceResult playerChoiceResult = PlayerChoiceResult.FromIndex(num2);
					this._synchronizer.SyncLocalChoice(base.Player, choiceId, playerChoiceResult);
				}
				else
				{
					num2 = (await this._synchronizer.WaitForRemoteChoice(base.Player, choiceId)).AsIndexOrNull();
				}
				NCardHolder cardHolder;
				CardRewardAlternative cardRewardAlternative;
				if (num2 != null)
				{
					int? num3 = num2;
					int num4 = this._cards.Count;
					if ((num3.GetValueOrDefault() < num4) & (num3 != null))
					{
						CS$<>8__locals1.obtainedCard = this._cards[num2.Value].Card;
						rewardComplete = true;
						endSelection = !Hook.ShouldAllowSelectingMoreCardRewards(base.Player.RunState, base.Player, this);
						NCardRewardSelectionScreen currentlyShownScreen = this._currentlyShownScreen;
						cardHolder = ((currentlyShownScreen != null) ? currentlyShownScreen.GetCardHolder(CS$<>8__locals1.obtainedCard) : null);
						cardRewardAlternative = null;
					}
					else
					{
						num3 = num2;
						num4 = this._cards.Count + cardRewardOption.Count;
						if (!((num3.GetValueOrDefault() < num4) & (num3 != null)))
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Received bad player choice index ");
							defaultInterpolatedStringHandler.AppendFormatted<int?>(num2);
							defaultInterpolatedStringHandler.AppendLiteral(" for a card reward with ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(this._cards.Count);
							defaultInterpolatedStringHandler.AppendLiteral(" cards and ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(cardRewardOption.Count);
							defaultInterpolatedStringHandler.AppendLiteral(" alternatives!");
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							continue;
						}
						cardRewardAlternative = cardRewardOption[num2.Value - this._cards.Count];
						rewardComplete = cardRewardAlternative.AfterSelected == PostAlternateCardRewardAction.EndSelectionAndCompleteReward;
						endSelection = cardRewardAlternative.AfterSelected - PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward <= 1;
						cardHolder = null;
						CS$<>8__locals1.obtainedCard = null;
					}
				}
				else
				{
					rewardComplete = false;
					endSelection = true;
					cardHolder = null;
					CS$<>8__locals1.obtainedCard = null;
					cardRewardAlternative = null;
				}
				if (CS$<>8__locals1.obtainedCard != null || cardRewardAlternative != null || rewardComplete)
				{
					if (CS$<>8__locals1.obtainedCard != null)
					{
						CardPileAddResult cardPileAddResult = await CardPileCmd.Add(CS$<>8__locals1.obtainedCard, PileType.Deck, CardPilePosition.Bottom, null, false);
						if (cardPileAddResult.success)
						{
							CS$<>8__locals1.obtainedCard = cardPileAddResult.cardAdded;
							chosenCardIds.Add(CS$<>8__locals1.obtainedCard);
							this._cards.RemoveAll((CardCreationResult c) => c.Card == CS$<>8__locals1.obtainedCard);
							if (cardHolder != null)
							{
								NCard cardNode = cardHolder.CardNode;
								NRun.Instance.GlobalUi.ReparentCard(cardNode);
								cardHolder.QueueFreeSafely();
								NRun.Instance.GlobalUi.TopBar.TrailContainer.AddChildSafely(NCardFlyVfx.Create(cardNode, PileType.Deck, true, CS$<>8__locals1.obtainedCard.Owner.Character.TrailPath));
							}
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Player ");
							defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
							defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
							defaultInterpolatedStringHandler.AppendFormatted<ModelId>(CS$<>8__locals1.obtainedCard.Id);
							defaultInterpolatedStringHandler.AppendLiteral(" from card reward");
							Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						}
					}
					else if (cardRewardAlternative != null)
					{
						await cardRewardAlternative.OnSelect();
					}
				}
				CS$<>8__locals1 = null;
				cardHolder = null;
			}
			base.Player.RelicObtained -= this.OnRelicObtained;
			foreach (CardModel cardModel in chosenCardIds)
			{
				base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardModel, true));
			}
			if (rewardComplete)
			{
				foreach (CardCreationResult cardCreationResult in this._cards)
				{
					base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardCreationResult.Card, false));
				}
			}
			if (this._currentlyShownScreen != null)
			{
				NOverlayStack instance = NOverlayStack.Instance;
				if (instance != null)
				{
					instance.Remove(this._currentlyShownScreen);
				}
				this._currentlyShownScreen = null;
			}
			return rewardComplete;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0005FD18 File Offset: 0x0005DF18
		public override void OnSkipped()
		{
			foreach (CardCreationResult cardCreationResult in this._cards)
			{
				base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardCreationResult.Card, false));
			}
			base.Player.RelicObtained -= this.OnRelicObtained;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0005FDB4 File Offset: 0x0005DFB4
		public void Reroll()
		{
			this.CanReroll = false;
			foreach (CardCreationResult cardCreationResult in this._cards)
			{
				base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardCreationResult.Card, false));
			}
			this._hasBeenRerolled = true;
			this._cards.Clear();
			this.Populate();
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0005FE58 File Offset: 0x0005E058
		public override SerializableReward ToSerializable()
		{
			if (this.Options.CardPools.Count <= 0)
			{
				throw new NotImplementedException("Tried to serialize a CardReward without any card pools! This is not currently supported.");
			}
			if (this.Options.CardPoolFilter != null)
			{
				throw new NotImplementedException("Tried to serialize a CardReward with a card pool filter! This is not currently supported.");
			}
			CardCreationFlags cardCreationFlags = this.Options.Flags & ~CardCreationFlags.IsCardReward;
			if (cardCreationFlags != (CardCreationFlags)0)
			{
				string text = "Tried to serialize a CardReward with card creation flags! ";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("This is not currently supported. Flags: ");
				defaultInterpolatedStringHandler.AppendFormatted<CardCreationFlags>(cardCreationFlags);
				throw new NotImplementedException(text + defaultInterpolatedStringHandler.ToStringAndClear());
			}
			SerializableReward serializableReward = new SerializableReward();
			serializableReward.RewardType = RewardType.Card;
			serializableReward.Source = this.Options.Source;
			serializableReward.RarityOdds = this.Options.RarityOdds;
			serializableReward.CardPoolIds = this.Options.CardPools.Select<CardPoolModel, ModelId>((CardPoolModel p) => p.Id).ToList<ModelId>();
			serializableReward.OptionCount = this.OptionCount;
			return serializableReward;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005FF59 File Offset: 0x0005E159
		public override void MarkContentAsSeen()
		{
		}

		// Token: 0x04000584 RID: 1412
		private readonly PlayerChoiceSynchronizer _synchronizer;

		// Token: 0x04000585 RID: 1413
		private readonly List<CardCreationResult> _cards = new List<CardCreationResult>();

		/// <summary>
		/// This is true if cards were manually set through the constructor, and Options has an empty card pool. The intent
		/// when this is set to true is that _cards is a static list that should not be re-populated nor changed.
		/// </summary>
		// Token: 0x04000586 RID: 1414
		private bool _cardsWereManuallySet;

		// Token: 0x04000587 RID: 1415
		private bool _hasBeenRerolled;

		// Token: 0x0400058E RID: 1422
		[Nullable(2)]
		private NCardRewardSelectionScreen _currentlyShownScreen;
	}
}
