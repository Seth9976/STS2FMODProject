using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D2B RID: 3371
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardSelectCmd
	{
		/// <summary>
		/// Clears all active selectors. Call this during run cleanup to prevent selectors
		/// leaked by stuck async tasks (e.g., WhisperingEarring mid-auto-play when a run ends)
		/// from affecting subsequent runs.
		/// </summary>
		// Token: 0x06008800 RID: 34816 RVA: 0x002DCE5C File Offset: 0x002DB05C
		public static void Reset()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (CardSelectCmd._selectorStack.Count <= 0 && CardSelectCmd._localSelectorStack.Count <= 0)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
			defaultInterpolatedStringHandler.AppendLiteral("CardSelectCmd.Reset: clearing ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(CardSelectCmd._selectorStack.Count);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(CardSelectCmd._localSelectorStack.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" leaked selector(s) from the stack.");
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			CardSelectCmd._selectorStack.Clear();
			CardSelectCmd._localSelectorStack.Clear();
		}

		/// <summary>
		/// The currently active automated card selector.
		/// Used by AutoSlay and gameplay effects that auto-play cards (e.g., WhisperingEarring).
		/// When set, card selection UI is bypassed and cards are selected automatically.
		/// Returns the top of the stack, or null if empty.
		/// </summary>
		// Token: 0x17002577 RID: 9591
		// (get) Token: 0x06008801 RID: 34817 RVA: 0x002DCEFD File Offset: 0x002DB0FD
		[Nullable(2)]
		public static MegaCrit.Sts2.Core.TestSupport.ICardSelector Selector
		{
			[NullableContext(2)]
			get
			{
				if (CardSelectCmd._selectorStack.Count <= 0)
				{
					return null;
				}
				return CardSelectCmd._selectorStack.Peek();
			}
		}

		/// <summary>
		/// The currently active automated card selector used in tests.
		/// Used by tests. This differs from Selector; it is used within player choices in a NON-deterministic spot. It
		/// should only be used in tests when blocking on player choice is part of the scenario.
		/// </summary>
		// Token: 0x17002578 RID: 9592
		// (get) Token: 0x06008802 RID: 34818 RVA: 0x002DCF18 File Offset: 0x002DB118
		[Nullable(2)]
		public static MegaCrit.Sts2.Core.TestSupport.ICardSelector LocalSelector
		{
			[NullableContext(2)]
			get
			{
				if (CardSelectCmd._localSelectorStack.Count <= 0)
				{
					return null;
				}
				return CardSelectCmd._localSelectorStack.Peek();
			}
		}

		/// <summary>
		/// Sets the automated card selector for the duration of the returned scope.
		/// Disposing the scope clears all selectors.
		/// Throws if a selector is already active (use PushSelector for stacking behavior).
		/// </summary>
		// Token: 0x06008803 RID: 34819 RVA: 0x002DCF34 File Offset: 0x002DB134
		public static IDisposable UseSelector(MegaCrit.Sts2.Core.TestSupport.ICardSelector selector, bool localOnly = false)
		{
			Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = (localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack);
			if (stack.Count > 0)
			{
				throw new InvalidOperationException("A card selector is already active.");
			}
			stack.Push(selector);
			return new CardSelectCmd.SelectorScope(stack);
		}

		/// <summary>
		/// Pushes a new selector onto the stack. When disposed, the previous selector is restored.
		/// Use this when you need temporary selector behavior (e.g., WhisperingEarring's auto-play).
		/// </summary>
		// Token: 0x06008804 RID: 34820 RVA: 0x002DCF74 File Offset: 0x002DB174
		public static IDisposable PushSelector(MegaCrit.Sts2.Core.TestSupport.ICardSelector selector, bool localOnly = false)
		{
			Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = (localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack);
			stack.Push(selector);
			return new CardSelectCmd.StackedSelectorScope(stack, selector);
		}

		/// <summary>
		/// TEST ONLY!
		/// Temporarily pops the top of the selector stack so tests can exercise the network-synchronized card-selection
		/// branches that are otherwise bypassed when a selector is active.
		/// Returns a no-op scope if the stack is already empty.
		/// </summary>
		// Token: 0x06008805 RID: 34821 RVA: 0x002DCFA0 File Offset: 0x002DB1A0
		public static IDisposable SuspendSelectorForTest(bool localOnly = false)
		{
			Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = (localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack);
			if (stack.Count == 0)
			{
				return new CardSelectCmd.NoOpScope();
			}
			MegaCrit.Sts2.Core.TestSupport.ICardSelector cardSelector = stack.Pop();
			return new CardSelectCmd.RestoreSelectorScope(stack, cardSelector);
		}

		// Token: 0x06008806 RID: 34822 RVA: 0x002DCFD9 File Offset: 0x002DB1D9
		private static bool ShouldSelectLocalCard(Player player)
		{
			return LocalContext.IsMe(player) && RunManager.Instance.NetService.Type != NetGameType.Replay;
		}

		/// <summary>
		/// Reports a potential softlock to Sentry and logs an error. Called when a selection screen would have been
		/// shown with 0 options, which would leave the player stuck. The caller returns empty to gracefully recover,
		/// but the underlying cause is still a bug that should be investigated and fixed.
		/// </summary>
		// Token: 0x06008807 RID: 34823 RVA: 0x002DCFFC File Offset: 0x002DB1FC
		private static void ReportSoftlock()
		{
			string text = "A selection screen was about to be shown with 0 options. Returning empty to prevent softlock.";
			Log.Error(text, 2);
			SentryService.CaptureException(new SoftlockException(text));
		}

		/// <summary>
		/// Begins card selection for the given player, selecting from a small list.
		/// Only works with a small number of cards (right now 3 or fewer). Good for in-combat card generator effects like
		/// Attack Potion.
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// </summary>
		/// <param name="context">The context to signal when player choice has begun and ended.</param>
		/// <param name="cards">Cards to show.</param>
		/// <param name="player">The player that is picking cards.</param>
		/// <param name="canSkip">
		///     Whether or not the card choice can be skipped.
		///     NOTE: If we add any more params like this, we should probably refactor it to use CardSelectorPrefs.
		/// </param>
		/// <returns>The card that was selected.</returns>
		// Token: 0x06008808 RID: 34824 RVA: 0x002DD024 File Offset: 0x002DB224
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<CardModel> FromChooseACardScreen(PlayerChoiceContext context, IReadOnlyList<CardModel> cards, Player player, bool canSkip = false)
		{
			if (cards.Count > 3)
			{
				throw new ArgumentException("Only works with less than 3 cards", "cards");
			}
			CardModel cardModel;
			if (cards.Count == 0)
			{
				CardSelectCmd.ReportSoftlock();
				cardModel = null;
			}
			else
			{
				CardModel result;
				if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable = await CardSelectCmd.Selector.GetSelectedCards(cards, 0, 1);
					result = enumerable.FirstOrDefault<CardModel>();
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							result = (await CardSelectCmd.LocalSelector.GetSelectedCards(cards, 0, 1)).FirstOrDefault<CardModel>();
						}
						else
						{
							NPlayerHand instance = NPlayerHand.Instance;
							if (instance != null)
							{
								instance.CancelAllCardPlay();
							}
							NChooseACardSelectionScreen nchooseACardSelectionScreen = NChooseACardSelectionScreen.ShowScreen(cards, canSkip);
							if (LocalContext.IsMe(player))
							{
								foreach (CardModel cardModel2 in cards)
								{
									SaveManager.Instance.MarkCardAsSeen(cardModel2);
								}
							}
							result = (await nchooseACardSelectionScreen.CardsSelected()).FirstOrDefault<CardModel>();
							PlayerChoiceResult playerChoiceResult = PlayerChoiceResult.FromIndex(new int?(cards.IndexOf(result)));
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, playerChoiceResult);
						}
					}
					else
					{
						int num = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
						result = ((num < 0) ? null : cards[num]);
					}
					await context.SignalPlayerChoiceEnded();
				}
				CardSelectCmd.LogChoice(player, new <>z__ReadOnlySingleElementList<CardModel>(result));
				cardModel = result;
			}
			return cardModel;
		}

		/// <summary>
		/// Begins card selection for the given player, selecting from a grid.
		/// Intended for cards that the player will add to their deck. Supports flashing modifications of the card creation
		/// by relics over the card.
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// </summary>
		/// <param name="context">The context to use when signalling player choice.</param>
		/// <param name="cards">Cards to show in the grid, including any modifications applied to those cards.</param>
		/// <param name="player">The player that is picking cards.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>The selected cards.</returns>
		// Token: 0x06008809 RID: 34825 RVA: 0x002DD080 File Offset: 0x002DB280
		public static async Task<IEnumerable<CardModel>> FromSimpleGridForRewards(PlayerChoiceContext context, List<CardCreationResult> cards, Player player, CardSelectorPrefs prefs)
		{
			IEnumerable<CardModel> enumerable;
			if (CombatManager.Instance.IsEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else if (cards.Count == 0)
			{
				CardSelectCmd.ReportSoftlock();
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				List<CardModel> result;
				if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
				{
					result = cards.Select<CardCreationResult, CardModel>((CardCreationResult c) => c.Card).ToList<CardModel>();
				}
				else if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable2 = cards.Select<CardCreationResult, CardModel>((CardCreationResult c) => c.Card);
					IEnumerable<CardModel> enumerable3 = await CardSelectCmd.Selector.GetSelectedCards(enumerable2, prefs.MinSelect, prefs.MaxSelect);
					result = enumerable3.ToList<CardModel>();
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							IEnumerable<CardModel> enumerable4 = cards.Select<CardCreationResult, CardModel>((CardCreationResult c) => c.Card);
							result = (await CardSelectCmd.LocalSelector.GetSelectedCards(enumerable4, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
						}
						else
						{
							NSimpleCardSelectScreen nsimpleCardSelectScreen = NSimpleCardSelectScreen.Create(cards, prefs);
							NOverlayStack.Instance.Push(nsimpleCardSelectScreen);
							result = (await nsimpleCardSelectScreen.CardsSelected()).ToList<CardModel>();
							List<int> list = result.Select<CardModel, int>((CardModel c) => cards.FindIndex((CardCreationResult r) => r.Card == c)).ToList<int>();
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes(list));
						}
					}
					else
					{
						result = (from i in (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndexes()
							select cards[i].Card).ToList<CardModel>();
					}
					await context.SignalPlayerChoiceEnded();
				}
				CardSelectCmd.LogChoice(player, result);
				enumerable = result;
			}
			return enumerable;
		}

		/// <summary>
		/// Begins card selection for the given player, selecting from a grid.
		/// Good for selecting cards from potentially large lists (draw/discard pile, "any Ironclad card", etc.) without
		/// any extra bespoke UI (no upgrade/enchant previews, etc.).
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// </summary>
		/// <param name="context">The context to use when signalling player choice.</param>
		/// <param name="cardsIn">Cards to show in the grid. It is copied after passed.</param>
		/// <param name="player">The player that is picking cards.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>The selected cards.</returns>
		// Token: 0x0600880A RID: 34826 RVA: 0x002DD0DC File Offset: 0x002DB2DC
		public static async Task<IEnumerable<CardModel>> FromSimpleGrid(PlayerChoiceContext context, IReadOnlyList<CardModel> cardsIn, Player player, CardSelectorPrefs prefs)
		{
			IEnumerable<CardModel> enumerable;
			if (CombatManager.Instance.IsEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				List<CardModel> cards = cardsIn.ToList<CardModel>();
				if (cards.Count == 0)
				{
					enumerable = Array.Empty<CardModel>();
				}
				else
				{
					List<CardModel> result;
					if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
					{
						result = cards.ToList<CardModel>();
					}
					else if (CardSelectCmd.Selector != null)
					{
						IEnumerable<CardModel> enumerable2 = await CardSelectCmd.Selector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect);
						result = enumerable2.ToList<CardModel>();
					}
					else
					{
						uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
						await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
						if (CardSelectCmd.ShouldSelectLocalCard(player))
						{
							if (CardSelectCmd.LocalSelector != null)
							{
								result = (await CardSelectCmd.LocalSelector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
							}
							else
							{
								NPlayerHand instance = NPlayerHand.Instance;
								if (instance != null)
								{
									instance.CancelAllCardPlay();
								}
								NSimpleCardSelectScreen nsimpleCardSelectScreen = NSimpleCardSelectScreen.Create(cards, prefs);
								NOverlayStack.Instance.Push(nsimpleCardSelectScreen);
								result = (await nsimpleCardSelectScreen.CardsSelected()).ToList<CardModel>();
								List<int> list = result.Select<CardModel, int>((CardModel c) => cards.IndexOf(c)).ToList<int>();
								RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes(list));
							}
						}
						else
						{
							result = (from i in (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndexes()
								select cards[i]).ToList<CardModel>();
						}
						await context.SignalPlayerChoiceEnded();
					}
					CardSelectCmd.LogChoice(player, result);
					enumerable = result;
				}
			}
			return enumerable;
		}

		// Token: 0x0600880B RID: 34827 RVA: 0x002DD138 File Offset: 0x002DB338
		public static async Task<IEnumerable<CardModel>> FromCombatPile(PlayerChoiceContext context, CardPile pile, Player player, CardSelectorPrefs prefs)
		{
			return await CardSelectCmd.FromCombatPile(context, pile, player, prefs, (CardModel _) => true);
		}

		// Token: 0x0600880C RID: 34828 RVA: 0x002DD194 File Offset: 0x002DB394
		public static async Task<IEnumerable<CardModel>> FromCombatPile(PlayerChoiceContext context, CardPile pile, Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter)
		{
			IEnumerable<CardModel> enumerable;
			if (CombatManager.Instance.IsEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				if (!pile.IsCombatPile)
				{
					throw new InvalidOperationException("Cannot perform on a non combat pile");
				}
				IEnumerable<CardModel> enumerable2;
				if (filter == null)
				{
					enumerable2 = pile.Cards;
				}
				else
				{
					IReadOnlyList<CardModel> readOnlyList = pile.Cards.Where<CardModel>(filter).ToList<CardModel>();
					enumerable2 = readOnlyList;
				}
				IEnumerable<CardModel> filtered = enumerable2;
				int num = filtered.Count<CardModel>();
				if (num == 0)
				{
					enumerable = Array.Empty<CardModel>();
				}
				else
				{
					IEnumerable<CardModel> result;
					if (!prefs.RequireManualConfirmation && num <= prefs.MinSelect)
					{
						result = filtered;
					}
					else if (CardSelectCmd.Selector != null)
					{
						if (pile.Type == PileType.Draw)
						{
							filtered = from c in filtered
								orderby c.Rarity, c.Id
								select c;
						}
						IEnumerable<CardModel> enumerable3 = await CardSelectCmd.Selector.GetSelectedCards(filtered, prefs.MinSelect, prefs.MaxSelect);
						result = enumerable3.ToList<CardModel>();
					}
					else
					{
						uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
						await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
						if (CardSelectCmd.ShouldSelectLocalCard(player))
						{
							if (CardSelectCmd.LocalSelector != null)
							{
								if (pile.Type == PileType.Draw)
								{
									filtered = from c in filtered
										orderby c.Rarity, c.Id
										select c;
								}
								result = (await CardSelectCmd.LocalSelector.GetSelectedCards(filtered, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
							}
							else
							{
								NPlayerHand instance = NPlayerHand.Instance;
								if (instance != null)
								{
									instance.CancelAllCardPlay();
								}
								NCombatPileCardSelectScreen ncombatPileCardSelectScreen = NCombatPileCardSelectScreen.Create(pile, prefs, filter);
								NOverlayStack.Instance.Push(ncombatPileCardSelectScreen);
								result = (await ncombatPileCardSelectScreen.CardsSelected()).ToList<CardModel>();
								RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCards(result));
							}
						}
						else
						{
							result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards();
						}
						await context.SignalPlayerChoiceEnded();
					}
					CardSelectCmd.LogChoice(player, result);
					enumerable = result;
				}
			}
			return enumerable;
		}

		/// <summary>
		/// Select from the upgradable cards in the player's deck. Shows an upgrade preview before returning. Good for
		/// "select a card to upgrade" screens (Rest Site smith, some events, etc.).
		/// </summary>
		/// <param name="player">Player whose deck to show.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>Selected cards.</returns>
		// Token: 0x0600880D RID: 34829 RVA: 0x002DD1F8 File Offset: 0x002DB3F8
		public static async Task<IEnumerable<CardModel>> FromDeckForUpgrade(Player player, CardSelectorPrefs prefs)
		{
			List<CardModel> list = PileType.Deck.GetPile(player).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
			IEnumerable<CardModel> enumerable;
			if (list.Count == 0)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				IEnumerable<CardModel> enumerable2;
				if (list.Count <= prefs.MinSelect && !prefs.RequireManualConfirmation)
				{
					enumerable2 = list;
				}
				else if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable3 = await CardSelectCmd.Selector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
					enumerable2 = enumerable3;
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							enumerable2 = await CardSelectCmd.LocalSelector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
						}
						else
						{
							enumerable2 = await NDeckUpgradeSelectScreen.ShowScreen(list, prefs, player.RunState).CardsSelected();
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(enumerable2));
						}
					}
					else
					{
						enumerable2 = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
					}
				}
				CardSelectCmd.LogChoice(player, enumerable2);
				enumerable = enumerable2;
			}
			return enumerable;
		}

		/// <summary>
		/// Select from the transformable cards in the player's deck. Shows a transform preview before returning. Good for
		/// "select a card to upgrade" screens (Rest Site smith, some events, etc.).
		/// </summary>
		/// <param name="player">Player whose deck to show.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <param name="cardToTransformation">
		/// A delegate that is called to get the possible transformations for a card. If unsupplied, it will use the default
		/// transformation (the card is transformed to any other card from the same pool).
		/// </param>
		/// <returns>Selected cards.</returns>
		// Token: 0x0600880E RID: 34830 RVA: 0x002DD244 File Offset: 0x002DB444
		public static async Task<IEnumerable<CardModel>> FromDeckForTransformation(Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, CardTransformation> cardToTransformation = null)
		{
			List<CardModel> list = PileType.Deck.GetPile(player).Cards.Where<CardModel>((CardModel c) => c.Type != CardType.Quest && c.IsTransformable).ToList<CardModel>();
			IEnumerable<CardModel> enumerable;
			if (list.Count == 0)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				IEnumerable<CardModel> enumerable2;
				if (list.Count <= prefs.MinSelect && !prefs.RequireManualConfirmation)
				{
					enumerable2 = list;
				}
				else if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable3 = await CardSelectCmd.Selector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
					enumerable2 = enumerable3;
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							enumerable2 = await CardSelectCmd.LocalSelector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
						}
						else
						{
							if (cardToTransformation == null)
							{
								cardToTransformation = (CardModel c) => new CardTransformation(c);
							}
							enumerable2 = await NDeckTransformSelectScreen.ShowScreen(list, cardToTransformation, prefs).CardsSelected();
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(enumerable2));
						}
					}
					else
					{
						enumerable2 = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
					}
				}
				CardSelectCmd.LogChoice(player, enumerable2);
				enumerable = enumerable2;
			}
			return enumerable;
		}

		/// <summary>
		/// Select from the enchantable cards in the player's deck. Shows an enchant preview before returning. Good for
		/// "select a card to enchant" screens (events, etc.).
		/// </summary>
		/// <param name="player">Player whose deck to show.</param>
		/// <param name="enchantment">Enchantment to apply to the card.</param>
		/// <param name="amount">Amount of the enchantment to apply to the card.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>Selected cards.</returns>
		// Token: 0x0600880F RID: 34831 RVA: 0x002DD298 File Offset: 0x002DB498
		public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(Player player, EnchantmentModel enchantment, int amount, CardSelectorPrefs prefs)
		{
			return await CardSelectCmd.FromDeckForEnchantment(player, enchantment, amount, null, prefs);
		}

		/// <summary>
		/// Select from the enchantable cards in the player's deck. Shows an enchant preview before returning. Good for
		/// "select a card to enchant" screens (events, etc.).
		/// </summary>
		/// <param name="player">Player whose deck to show.</param>
		/// <param name="enchantment">Enchantment to apply to the card.</param>
		/// <param name="amount">Amount of the enchantment to apply to the card.</param>
		/// <param name="additionalFilter">Additional filter which should return true for cards that should be included in the selection.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>Selected cards.</returns>
		// Token: 0x06008810 RID: 34832 RVA: 0x002DD2F4 File Offset: 0x002DB4F4
		public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(Player player, EnchantmentModel enchantment, int amount, [Nullable(2)] Func<CardModel, bool> additionalFilter, CardSelectorPrefs prefs)
		{
			IReadOnlyList<CardModel> readOnlyList = PileType.Deck.GetPile(player).Cards.Where<CardModel>(delegate(CardModel c)
			{
				if (enchantment.CanEnchant(c))
				{
					Func<CardModel, bool> additionalFilter2 = additionalFilter;
					return additionalFilter2 == null || additionalFilter2(c);
				}
				return false;
			}).ToList<CardModel>();
			return await CardSelectCmd.FromDeckForEnchantment(readOnlyList, enchantment, amount, prefs);
		}

		/// <summary>
		/// Select from the enchantable cards in the player's deck. Shows an enchant preview before returning. Good for
		/// "select a card to enchant" screens (events, etc.).
		/// </summary>
		/// <param name="cards">Cards to select from. All must be in the player's deck and enchantable.</param>
		/// <param name="enchantment">Enchantment to apply to the card.</param>
		/// <param name="amount">Amount of the enchantment to apply to the card.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <returns>Selected cards.</returns>
		// Token: 0x06008811 RID: 34833 RVA: 0x002DD358 File Offset: 0x002DB558
		public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(IReadOnlyList<CardModel> cards, EnchantmentModel enchantment, int amount, CardSelectorPrefs prefs)
		{
			if (cards.Any<CardModel>((CardModel c) => c.Pile.Type != PileType.Deck || !enchantment.CanEnchant(c)))
			{
				throw new ArgumentException("All cards must be in the player's deck and enchantable.");
			}
			List<CardModel> list = new List<CardModel>();
			if (cards.Count > 0)
			{
				Player owner = cards[0].Owner;
				Dictionary<CardModel, int> indexMap = PileType.Deck.GetPile(owner).Cards.Select((CardModel card, int index) => new { card, index }).ToDictionary(x => x.card, x => x.index);
				list = cards.OrderBy<CardModel, int>((CardModel c) => indexMap[c]).ToList<CardModel>();
			}
			IEnumerable<CardModel> enumerable;
			if (cards.Count <= prefs.MinSelect)
			{
				enumerable = cards;
			}
			else if (CardSelectCmd.Selector != null)
			{
				IEnumerable<CardModel> enumerable2 = await CardSelectCmd.Selector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
				enumerable = enumerable2;
			}
			else
			{
				Player player = cards[0].Owner;
				if (player.Creature.IsDead)
				{
					return Array.Empty<CardModel>();
				}
				uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
				if (CardSelectCmd.ShouldSelectLocalCard(player))
				{
					if (CardSelectCmd.LocalSelector != null)
					{
						enumerable = await CardSelectCmd.LocalSelector.GetSelectedCards(list, prefs.MinSelect, prefs.MaxSelect);
					}
					else
					{
						enumerable = await NDeckEnchantSelectScreen.ShowScreen(list, enchantment, amount, prefs).CardsSelected();
						RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(enumerable));
					}
				}
				else
				{
					enumerable = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
				}
				player = null;
			}
			if (cards.Count > 0)
			{
				CardSelectCmd.LogChoice(cards[0].Owner, enumerable);
			}
			return enumerable;
		}

		/// <summary>
		/// Select from removable cards in the player's deck. Good for "select a card to remove" screens like the Merchant's
		/// card removal option.
		/// </summary>
		/// <param name="player">Player whose deck to remove from.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <param name="filter">
		/// Optional filter to exclude certain cards (like hiding un-upgradable cards for the Magic Pot relic).
		/// Note: We already automatically hide unremovable cards, so this filter doesn't have to worry about that.
		/// </param>
		/// <returns>Selected cards.</returns>
		// Token: 0x06008812 RID: 34834 RVA: 0x002DD3B4 File Offset: 0x002DB5B4
		public static Task<IEnumerable<CardModel>> FromDeckForRemoval(Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter = null)
		{
			List<CardModel> deck = PileType.Deck.GetPile(player).Cards.ToList<CardModel>();
			return CardSelectCmd.FromDeckGeneric(player, prefs, (CardModel c) => c.IsRemovable && (filter == null || filter(c)), delegate(CardModel c)
			{
				if (c.Type != CardType.Curse)
				{
					return deck.IndexOf(c);
				}
				return -999999999;
			});
		}

		/// <summary>
		/// A generic select screen for the cards in the player's deck. Shows a card selection before returning.
		/// Does not include upgrading, transforming, or applying enchantments to cards, as those have special UI
		/// </summary>
		/// <param name="player">Player whose deck to select from.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <param name="filter">Optional filter to exclude certain cards (like un-removable cards from the deck card removal screen).</param>
		/// <param name="sortingOrder">Optional func to define the sort order of the cards.</param>
		/// <returns>Selected cards.</returns>
		// Token: 0x06008813 RID: 34835 RVA: 0x002DD404 File Offset: 0x002DB604
		public static async Task<IEnumerable<CardModel>> FromDeckGeneric(Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter = null, [Nullable(new byte[] { 2, 1 })] Func<CardModel, int> sortingOrder = null)
		{
			List<CardModel> list = PileType.Deck.GetPile(player).Cards.ToList<CardModel>();
			List<CardModel> list2 = ((filter == null) ? list.ToList<CardModel>() : list.Where<CardModel>(filter).ToList<CardModel>());
			IEnumerable<CardModel> enumerable;
			if (player.Creature.IsDead)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				if (sortingOrder != null)
				{
					list2 = list2.OrderBy<CardModel, int>(sortingOrder).ToList<CardModel>();
				}
				if (list2.Count == 0)
				{
					enumerable = Array.Empty<CardModel>();
				}
				else
				{
					IEnumerable<CardModel> enumerable2;
					if (!prefs.RequireManualConfirmation && list2.Count <= prefs.MinSelect)
					{
						enumerable2 = list2;
					}
					else if (CardSelectCmd.Selector != null)
					{
						IEnumerable<CardModel> enumerable3 = await CardSelectCmd.Selector.GetSelectedCards(list2, prefs.MinSelect, prefs.MaxSelect);
						enumerable2 = enumerable3;
					}
					else
					{
						uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
						if (CardSelectCmd.ShouldSelectLocalCard(player))
						{
							if (CardSelectCmd.LocalSelector != null)
							{
								enumerable2 = await CardSelectCmd.LocalSelector.GetSelectedCards(list2, prefs.MinSelect, prefs.MaxSelect);
							}
							else
							{
								NDeckCardSelectScreen ndeckCardSelectScreen = NDeckCardSelectScreen.Create(list2, prefs);
								NOverlayStack.Instance.Push(ndeckCardSelectScreen);
								enumerable2 = await ndeckCardSelectScreen.CardsSelected();
								RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(enumerable2));
							}
						}
						else
						{
							enumerable2 = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
						}
					}
					CardSelectCmd.LogChoice(player, enumerable2);
					enumerable = enumerable2;
				}
			}
			return enumerable;
		}

		/// <summary>
		/// Begins card selection, targeting a specific player's hand.
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// Good for simple hand selection without any extra bespoke UI (no upgrade/enchant previews, etc.).
		/// </summary>
		/// <param name="context">The context to use when signalling player choice.</param>
		/// <param name="player">Player whose hand to select from.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <param name="filter">Function describing which cards can be selected. All can be selected if null.</param>
		/// <param name="source">Model that kicked off the hand selection.</param>
		/// <returns>The card that was chosen by the given player.</returns>
		// Token: 0x06008814 RID: 34836 RVA: 0x002DD460 File Offset: 0x002DB660
		public static async Task<IEnumerable<CardModel>> FromHand(PlayerChoiceContext context, Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter, AbstractModel source)
		{
			IEnumerable<CardModel> enumerable;
			if (CombatManager.Instance.IsOverOrEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				if (CardSelectCmd.ShouldSelectLocalCard(player))
				{
					NPlayerHand instance = NPlayerHand.Instance;
					if (instance != null)
					{
						instance.CancelAllCardPlay();
					}
				}
				IEnumerable<CardModel> cards2 = PileType.Hand.GetPile(player).Cards;
				Func<CardModel, bool> func = filter;
				if (filter == null && (func = CardSelectCmd.<>c.<>9__28_0) == null)
				{
					func = (CardSelectCmd.<>c.<>9__28_0 = (CardModel _) => true);
				}
				List<CardModel> cards = cards2.Where<CardModel>(func).ToList<CardModel>();
				IEnumerable<CardModel> result;
				if (cards.Count == 0)
				{
					result = cards;
				}
				else if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
				{
					result = cards;
				}
				else if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable2 = await CardSelectCmd.Selector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect);
					result = enumerable2;
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.CancelPlayCardActions);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							result = await CardSelectCmd.LocalSelector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect);
						}
						else
						{
							result = await NCombatRoom.Instance.Ui.Hand.SelectCards(prefs, filter, source, NPlayerHand.Mode.SimpleSelect);
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCards(result));
						}
					}
					else
					{
						result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards();
					}
					await context.SignalPlayerChoiceEnded();
				}
				CardSelectCmd.LogChoice(player, result);
				enumerable = result;
			}
			return enumerable;
		}

		/// <summary>
		/// Begins card selection for discarding, targeting a specific player's hand.
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// </summary>
		/// <param name="context">The context to use when signalling player choice.</param>
		/// <param name="player">Player whose hand to select from.</param>
		/// <param name="prefs">CardSelectorPrefs</param>
		/// <param name="filter">Function describing which cards can be selected. All can be selected if null.</param>
		/// <param name="source">Model that kicked off the hand selection.</param>
		/// <returns>The card that was chosen by the given player.</returns>
		// Token: 0x06008815 RID: 34837 RVA: 0x002DD4C4 File Offset: 0x002DB6C4
		public static async Task<IEnumerable<CardModel>> FromHandForDiscard(PlayerChoiceContext context, Player player, CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter, AbstractModel source)
		{
			prefs.ShouldGlowGold = delegate(CardModel c)
			{
				UnplayableReason unplayableReason;
				AbstractModel abstractModel;
				return c.IsSlyThisTurn && (c.CanPlay(out unplayableReason, out abstractModel) || unplayableReason.HasResourceCostReason());
			};
			return await CardSelectCmd.FromHand(context, player, prefs, filter, source);
		}

		/// <summary>
		/// Begins card selection for upgrading, targeting a specific player's hand.
		/// If the player is the local player, this brings up the card selection screen.
		/// If the player is a remote player, this waits for that player to select a card.
		/// Good for in-combat hand upgrades like Armaments.
		/// </summary>
		/// <param name="context">The context to use when signalling player choice.</param>
		/// <param name="player">Player whose hand to select from.</param>
		/// <param name="source">Model that kicked off the selection.</param>
		/// <returns>The card that was chosen by the given player.</returns>
		// Token: 0x06008816 RID: 34838 RVA: 0x002DD528 File Offset: 0x002DB728
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<CardModel> FromHandForUpgrade(PlayerChoiceContext context, Player player, AbstractModel source)
		{
			CardModel cardModel;
			if (CombatManager.Instance.IsOverOrEnding)
			{
				cardModel = null;
			}
			else
			{
				if (CardSelectCmd.ShouldSelectLocalCard(player))
				{
					NPlayerHand instance = NPlayerHand.Instance;
					if (instance != null)
					{
						instance.CancelAllCardPlay();
					}
				}
				List<CardModel> cards = PileType.Hand.GetPile(player).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
				CardModel result;
				if (cards.Count <= 1)
				{
					result = cards.FirstOrDefault<CardModel>();
				}
				else if (CardSelectCmd.Selector != null)
				{
					IEnumerable<CardModel> enumerable = await CardSelectCmd.Selector.GetSelectedCards(cards, 1, 1);
					result = enumerable.FirstOrDefault<CardModel>();
				}
				else
				{
					uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
					await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.CancelPlayCardActions);
					if (CardSelectCmd.ShouldSelectLocalCard(player))
					{
						if (CardSelectCmd.LocalSelector != null)
						{
							result = (await CardSelectCmd.LocalSelector.GetSelectedCards(cards, 1, 1)).FirstOrDefault<CardModel>();
						}
						else
						{
							result = (await NCombatRoom.Instance.Ui.Hand.SelectCards(new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_UPGRADE_HEADER"), 1), (CardModel c) => c.IsUpgradable, source, NPlayerHand.Mode.UpgradeSelect)).FirstOrDefault<CardModel>();
							RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCard(result));
						}
					}
					else
					{
						result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards().FirstOrDefault<CardModel>();
					}
					await context.SignalPlayerChoiceEnded();
				}
				CardSelectCmd.LogChoice(player, new <>z__ReadOnlySingleElementList<CardModel>(result));
				cardModel = result;
			}
			return cardModel;
		}

		/// <summary>
		/// Get a list of canonical CardModels from the Choose a Bundle screen.
		/// </summary>
		/// <param name="player">Player choosing the bundle.</param>
		/// <param name="bundles">Bundles for them to choose from.</param>
		/// <returns>Chosen bundle.</returns>
		// Token: 0x06008817 RID: 34839 RVA: 0x002DD57C File Offset: 0x002DB77C
		public static async Task<IEnumerable<CardModel>> FromChooseABundleScreen(Player player, IReadOnlyList<IReadOnlyList<CardModel>> bundles)
		{
			IEnumerable<CardModel> enumerable;
			if (CombatManager.Instance.IsEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else if (bundles.Count == 0)
			{
				CardSelectCmd.ReportSoftlock();
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
				IReadOnlyList<CardModel> readOnlyList;
				if (TestMode.IsOn)
				{
					readOnlyList = bundles[0];
				}
				else if (CardSelectCmd.ShouldSelectLocalCard(player))
				{
					NChooseABundleSelectionScreen nchooseABundleSelectionScreen = NChooseABundleSelectionScreen.ShowScreen(bundles);
					IEnumerable<IReadOnlyList<CardModel>> enumerable2 = await nchooseABundleSelectionScreen.CardsSelected();
					readOnlyList = enumerable2.FirstOrDefault<IReadOnlyList<CardModel>>() ?? Array.Empty<CardModel>();
					RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(new int?(bundles.IndexOf(readOnlyList))));
				}
				else
				{
					int num = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
					readOnlyList = ((num < 0) ? Array.Empty<CardModel>() : bundles[num]);
				}
				CardSelectCmd.LogChoice(player, readOnlyList);
				enumerable = readOnlyList;
			}
			return enumerable;
		}

		// Token: 0x06008818 RID: 34840 RVA: 0x002DD5C8 File Offset: 0x002DB7C8
		private static void LogChoice(Player player, [Nullable(new byte[] { 1, 2 })] IEnumerable<CardModel> cards)
		{
			string text = string.Join(",", from c in cards.OfType<CardModel>()
				select c.Id.Entry);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" chose cards [");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x04002F59 RID: 12121
		private static readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _selectorStack = new Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector>();

		// Token: 0x04002F5A RID: 12122
		private static readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _localSelectorStack = new Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector>();

		// Token: 0x0200246A RID: 9322
		[Nullable(0)]
		private sealed class StackedSelectorScope : IDisposable
		{
			// Token: 0x0600C388 RID: 50056 RVA: 0x0042E89A File Offset: 0x0042CA9A
			public StackedSelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack, MegaCrit.Sts2.Core.TestSupport.ICardSelector selector)
			{
				this._stack = stack;
				this._selector = selector;
			}

			// Token: 0x0600C389 RID: 50057 RVA: 0x0042E8B0 File Offset: 0x0042CAB0
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				if (this._stack.Count > 0 && this._stack.Peek() == this._selector)
				{
					this._stack.Pop();
				}
			}

			// Token: 0x04009A4B RID: 39499
			private readonly MegaCrit.Sts2.Core.TestSupport.ICardSelector _selector;

			// Token: 0x04009A4C RID: 39500
			private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;

			// Token: 0x04009A4D RID: 39501
			private bool _disposed;
		}

		/// <summary>
		/// Used by <see cref="M:MegaCrit.Sts2.Core.Commands.CardSelectCmd.SuspendSelectorForTest(System.Boolean)" /> to conform to the normal interface when there's no card selector.
		/// </summary>
		// Token: 0x0200246B RID: 9323
		[NullableContext(0)]
		private sealed class NoOpScope : IDisposable
		{
			// Token: 0x0600C38A RID: 50058 RVA: 0x0042E8EF File Offset: 0x0042CAEF
			public void Dispose()
			{
			}
		}

		/// <summary>
		/// Used by <see cref="M:MegaCrit.Sts2.Core.Commands.CardSelectCmd.SuspendSelectorForTest(System.Boolean)" /> to temporarily suspend the current card selector so it can be
		/// restored later.
		/// </summary>
		// Token: 0x0200246C RID: 9324
		[Nullable(0)]
		private sealed class RestoreSelectorScope : IDisposable
		{
			// Token: 0x0600C38C RID: 50060 RVA: 0x0042E8F9 File Offset: 0x0042CAF9
			public RestoreSelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack, MegaCrit.Sts2.Core.TestSupport.ICardSelector saved)
			{
				this._stack = stack;
				this._saved = saved;
			}

			// Token: 0x0600C38D RID: 50061 RVA: 0x0042E90F File Offset: 0x0042CB0F
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._stack.Push(this._saved);
			}

			// Token: 0x04009A4E RID: 39502
			private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;

			// Token: 0x04009A4F RID: 39503
			private readonly MegaCrit.Sts2.Core.TestSupport.ICardSelector _saved;

			// Token: 0x04009A50 RID: 39504
			private bool _disposed;
		}

		// Token: 0x0200246D RID: 9325
		[NullableContext(0)]
		private sealed class SelectorScope : IDisposable
		{
			// Token: 0x0600C38E RID: 50062 RVA: 0x0042E932 File Offset: 0x0042CB32
			[NullableContext(1)]
			public SelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack)
			{
				this._stack = stack;
			}

			// Token: 0x0600C38F RID: 50063 RVA: 0x0042E941 File Offset: 0x0042CB41
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._stack.Clear();
			}

			// Token: 0x04009A51 RID: 39505
			[Nullable(1)]
			private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;

			// Token: 0x04009A52 RID: 39506
			private bool _disposed;
		}
	}
}
