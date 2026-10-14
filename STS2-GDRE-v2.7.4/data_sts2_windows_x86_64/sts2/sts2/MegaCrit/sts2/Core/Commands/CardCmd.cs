using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D29 RID: 3369
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardCmd
	{
		/// <summary>
		/// Automatically play a card for free. Used for non-player-choice card playing effects.
		/// </summary>
		/// <example>
		/// Examples of where this would be used:
		/// * Havoc ("Play the top card of your Draw Pile and Exhaust it.")
		/// * Duplication Potion ("This turn, your next card is played twice.")
		/// </example>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card to autoplay.</param>
		/// <param name="target">Target for the autoplay. Will be randomized if null.</param>
		/// <param name="type">Type of autoplay. Certain checks may be bypassed for different autoplay types.</param>
		/// <param name="skipXCapture">
		/// If true, skip capturing the X value for X-cost cards and X-star cost cards. Use this when the caller has already
		/// spent energy/stars via SpendResources, which sets CapturedXValue to the energy spent and stars spent.
		/// </param>
		/// <param name="skipCardPileVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087C3 RID: 34755 RVA: 0x002DB324 File Offset: 0x002D9524
		public static async Task AutoPlay(PlayerChoiceContext choiceContext, CardModel card, [Nullable(2)] Creature target, AutoPlayType type = AutoPlayType.Default, bool skipXCapture = false, bool skipCardPileVisuals = false)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				if (!card.Owner.Creature.IsDead)
				{
					ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
					AbstractModel preventer;
					if (card.Keywords.Contains(CardKeyword.Unplayable))
					{
						await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
					}
					else if (!Hook.ShouldPlay(combatState, card, out preventer, type))
					{
						await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
						LocString playerDialogueLine = UnplayableReason.BlockedByHook.GetPlayerDialogueLine(preventer);
						if (playerDialogueLine != null)
						{
							Control vfxContainer = card.Owner.Creature.GetVfxContainer();
							if (vfxContainer != null)
							{
								vfxContainer.AddChildSafely(NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), card.Owner.Creature, new double?((double)1f)));
							}
						}
					}
					else
					{
						if (card.TargetType == TargetType.AnyEnemy)
						{
							if (target == null)
							{
								target = card.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(combatState.HittableEnemies);
							}
							if (target == null)
							{
								await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
								return;
							}
						}
						if (card.TargetType == TargetType.AnyAlly)
						{
							IEnumerable<Creature> enumerable = combatState.Allies.Where<Creature>((Creature c) => c != null && c.IsAlive && c.IsPlayer && c != card.Owner.Creature);
							if (target == null)
							{
								target = card.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(enumerable);
							}
							if (target == null)
							{
								await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
								return;
							}
						}
						PlayerCombatState playerCombatState = card.Owner.PlayerCombatState;
						if (card.EnergyCost.CostsX && !skipXCapture)
						{
							card.EnergyCost.CapturedXValue = playerCombatState.Energy;
						}
						if (!skipXCapture)
						{
							if (card.HasStarCostX)
							{
								card.LastStarsSpent = playerCombatState.Stars;
							}
							else
							{
								card.LastStarsSpent = Math.Max(0, card.GetStarCostWithModifiers());
							}
						}
						if (card.Pile == null)
						{
							await CardPileCmd.Add(card, PileType.Play, CardPilePosition.Bottom, null, false);
						}
						if (!skipCardPileVisuals)
						{
							TaskHelper.RunSafely(card.OnEnqueuePlayVfx(target));
						}
						await Hook.BeforeCardAutoPlayed(combatState, card, target, type);
						ResourceInfo resourceInfo = new ResourceInfo
						{
							EnergySpent = 0,
							EnergyValue = card.EnergyCost.GetAmountToSpend(),
							StarsSpent = 0,
							StarValue = Math.Max(0, card.GetStarCostWithModifiers())
						};
						await card.OnPlayWrapper(choiceContext, target, true, resourceInfo, skipCardPileVisuals);
					}
				}
			}
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x002DB394 File Offset: 0x002D9594
		private static async Task MoveToResultPileWithoutPlaying(PlayerChoiceContext choiceContext, CardModel card)
		{
			await CardPileCmd.Add(card, PileType.Play, CardPilePosition.Bottom, null, false);
			await card.MoveToResultPileWithoutPlaying(choiceContext);
		}

		/// <summary>
		/// Discard a card.
		/// WARNING: If you're discarding multiple cards at once for an effect like Concentrate or Gambler's Brew, do NOT
		/// use this method inside a loop, because the timing of the Sly effect will be incorrect. Instead, use the overload
		/// of this method that takes an IEnumerable.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card to discard.</param>
		// Token: 0x060087C5 RID: 34757 RVA: 0x002DB3E0 File Offset: 0x002D95E0
		public static async Task Discard(PlayerChoiceContext choiceContext, CardModel card)
		{
			await CardCmd.Discard(choiceContext, new <>z__ReadOnlySingleElementList<CardModel>(card));
		}

		/// <summary>
		/// Discard multiple cards.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="cards">Cards to discard.</param>
		// Token: 0x060087C6 RID: 34758 RVA: 0x002DB42C File Offset: 0x002D962C
		public static async Task Discard(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cards)
		{
			await CardCmd.DiscardAndDraw(choiceContext, cards, 0);
		}

		/// <summary>
		/// Discard cards, then draw cards.
		/// Unlike calling <see cref="M:MegaCrit.Sts2.Core.Commands.CardCmd.Discard(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Models.CardModel)" /> followed by
		/// <see cref="M:MegaCrit.Sts2.Core.Commands.CardPileCmd.Draw(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Players.Player)" />, this will wait to trigger the discard-related hooks
		/// until after the draw is complete.
		/// Good for effects like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.CalculatedGamble" />.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="cardsToDiscard">Cards to discard.</param>
		/// <param name="cardsToDraw">Number of cards to draw.</param>
		// Token: 0x060087C7 RID: 34759 RVA: 0x002DB478 File Offset: 0x002D9678
		public static async Task DiscardAndDraw(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cardsToDiscard, int cardsToDraw)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				List<CardModel> discardCards = cardsToDiscard.ToList<CardModel>();
				if (discardCards.Count != 0)
				{
					ICombatState combatState = discardCards[0].CombatState ?? discardCards[0].Owner.Creature.CombatState;
					List<CardModel> slyCards = new List<CardModel>();
					CardPile discardPile = PileType.Discard.GetPile(discardCards[0].Owner);
					foreach (CardModel card in discardCards)
					{
						if (card.IsSlyThisTurn)
						{
							slyCards.Add(card);
						}
						await CardPileCmd.Add(card, discardPile, CardPilePosition.Bottom, null, false);
						CombatManager.Instance.History.CardDiscarded(combatState, card);
						await Hook.AfterCardDiscarded(combatState, choiceContext, card);
						card = null;
					}
					List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
					discardPile.InvokeContentsChanged();
					if (cardsToDraw > 0)
					{
						await CardPileCmd.Draw(choiceContext, cardsToDraw, discardCards[0].Owner, false);
					}
					foreach (CardModel cardModel in slyCards)
					{
						await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.SlyDiscard, false, false);
					}
					enumerator = default(List<CardModel>.Enumerator);
				}
			}
		}

		/// <summary>
		/// Downgrades a card to its base form.
		/// Keeps things like enchantments and conditions.
		/// </summary>
		/// <param name="card">Card to downgrade.</param>
		// Token: 0x060087C8 RID: 34760 RVA: 0x002DB4CC File Offset: 0x002D96CC
		public static void Downgrade(CardModel card)
		{
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			CardPile pile = card.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = card.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(card.Owner.NetId).DowngradedCards.Add(card.Id);
				}
			}
			card.DowngradeInternal();
		}

		/// <summary>
		/// Exhaust a card.
		/// Note: do NOT make a bulk version of this; the hooks for one exhausted card should fully run before the next
		/// card starts exhausting.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="card">Card to exhaust.</param>
		/// <param name="causedByEthereal">
		/// Was this Exhaust caused by Ethereal?
		/// This should always be false except the specific case in <see cref="T:MegaCrit.Sts2.Core.Combat.CombatManager" />.
		/// </param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087C9 RID: 34761 RVA: 0x002DB538 File Offset: 0x002D9738
		public static async Task Exhaust(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal = false, bool skipVisuals = false)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
				await CardPileCmd.Add(card, PileType.Exhaust, CardPilePosition.Bottom, null, skipVisuals);
				CombatManager.Instance.History.CardExhausted(combatState, card);
				await Hook.AfterCardExhausted(combatState, choiceContext, card, causedByEthereal);
			}
		}

		/// <summary>
		/// Upgrade a card.
		/// Use this for actually upgrading a card, not for previewing an upgrade, because it won't highlight value changes.
		/// </summary>
		/// <param name="card">Card to upgrade.</param>
		/// <param name="style">How the upgraded card is displayed to the player.</param>
		// Token: 0x060087CA RID: 34762 RVA: 0x002DB593 File Offset: 0x002D9793
		public static void Upgrade(CardModel card, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			CardCmd.Upgrade(new <>z__ReadOnlySingleElementList<CardModel>(card), style);
		}

		/// <summary>
		/// Upgrade a set of cards.
		/// Use this for actually upgrading cards, not for previewing upgrades, because it won't highlight value changes.
		/// </summary>
		/// <param name="cards">Cards to upgrade.</param>
		/// <param name="style">How multiple cards are aligned if previewed together.</param>
		// Token: 0x060087CB RID: 34763 RVA: 0x002DB5A4 File Offset: 0x002D97A4
		public static void Upgrade(IEnumerable<CardModel> cards, CardPreviewStyle style)
		{
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			foreach (CardModel cardModel in cards)
			{
				if (cardModel.IsUpgradable)
				{
					CardPile cardPile = cardModel.Pile;
					if (cardPile != null && cardPile.Type == PileType.Deck)
					{
						MapPointHistoryEntry currentMapPointHistoryEntry = cardModel.Owner.RunState.CurrentMapPointHistoryEntry;
						if (currentMapPointHistoryEntry != null)
						{
							currentMapPointHistoryEntry.GetEntry(cardModel.Owner.NetId).UpgradedCards.Add(cardModel.Id);
						}
					}
					cardModel.UpgradeInternal();
					cardModel.FinalizeUpgradeInternal();
					if (LocalContext.IsMine(cardModel))
					{
						cardPile = cardModel.Pile;
						if (cardPile != null && cardPile.Type == PileType.Deck && style != CardPreviewStyle.None)
						{
							Control control;
							switch (style)
							{
							case CardPreviewStyle.HorizontalLayout:
							{
								NRun instance = NRun.Instance;
								control = ((instance != null) ? instance.GlobalUi.CardPreviewContainer : null);
								break;
							}
							case CardPreviewStyle.MessyLayout:
							{
								NRun instance2 = NRun.Instance;
								control = ((instance2 != null) ? instance2.GlobalUi.MessyCardPreviewContainer : null);
								break;
							}
							case CardPreviewStyle.EventLayout:
							{
								NRun instance3 = NRun.Instance;
								control = ((instance3 != null) ? instance3.GlobalUi.EventCardPreviewContainer : null);
								break;
							}
							case CardPreviewStyle.GridLayout:
							{
								NRun instance4 = NRun.Instance;
								control = ((instance4 != null) ? instance4.GlobalUi.GridCardPreviewContainer : null);
								break;
							}
							default:
							{
								string text = "style";
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Unexpected ");
								defaultInterpolatedStringHandler.AppendFormatted("CardPreviewStyle");
								defaultInterpolatedStringHandler.AppendLiteral(" ");
								defaultInterpolatedStringHandler.AppendFormatted<CardPreviewStyle>(style);
								defaultInterpolatedStringHandler.AppendLiteral("!");
								throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
							}
							}
							Control control2 = control;
							if (control2 != null)
							{
								control2.AddChildSafely(NCardUpgradeVfx.Create(cardModel));
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Transform a card to another randomly-selected card.
		/// </summary>
		/// <param name="original">Card to transform from.</param>
		/// <param name="rng">RNG to use for random card.</param>
		/// <param name="style">How the transformed card is displayed to the player.</param>
		/// <returns>The replacement card.</returns>
		// Token: 0x060087CC RID: 34764 RVA: 0x002DB77C File Offset: 0x002D997C
		public static async Task<CardPileAddResult> TransformToRandom(CardModel original, Rng rng, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			IEnumerable<CardPileAddResult> enumerable = await CardCmd.Transform(new CardTransformation(original).Yield(), rng, style);
			IEnumerable<CardPileAddResult> enumerable2 = enumerable;
			return enumerable2.First<CardPileAddResult>();
		}

		/// <summary>
		/// Transform a card into a new card of the specified type.
		/// </summary>
		/// <param name="original">Card to transform from.</param>
		/// <param name="style">How the transformed card is displayed to the player.</param>
		/// <typeparam name="T">Card type to transform to.</typeparam>
		/// <returns>The replacement card.</returns>
		// Token: 0x060087CD RID: 34765 RVA: 0x002DB7D0 File Offset: 0x002D99D0
		public static async Task<CardPileAddResult?> TransformTo<[Nullable(0)] T>(CardModel original, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout) where T : CardModel
		{
			CardModel cardModel = original.CardScope.CreateCard<T>(original.Owner);
			return await CardCmd.Transform(original, cardModel, style);
		}

		/// <summary>
		/// Transform a card to another card.
		/// </summary>
		/// <param name="original">Card to transform from.</param>
		/// <param name="replacement">Card to transform to.</param>
		/// <param name="style">How the transformed card is displayed to the player.</param>
		/// <returns>The replacement card.</returns>
		// Token: 0x060087CE RID: 34766 RVA: 0x002DB81C File Offset: 0x002D9A1C
		public static async Task<CardPileAddResult?> Transform(CardModel original, CardModel replacement, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			IEnumerable<CardPileAddResult> enumerable = await CardCmd.Transform(new CardTransformation(original, replacement).Yield(), null, style);
			IEnumerable<CardPileAddResult> enumerable2 = enumerable;
			return new CardPileAddResult?(enumerable2.FirstOrDefault<CardPileAddResult>());
		}

		/// <summary>
		/// Comparer to sort a list of CardPile and card index in ascending card index order.
		/// </summary>
		// Token: 0x060087CF RID: 34767 RVA: 0x002DB870 File Offset: 0x002D9A70
		private static int PileIndexSort([Nullable(new byte[] { 0, 1, 1 })] ValueTuple<CardTransformation, CardPile, int, CardModel> value1, [Nullable(new byte[] { 0, 1, 1 })] ValueTuple<CardTransformation, CardPile, int, CardModel> value2)
		{
			if (value1.Item2.Type != value2.Item2.Type)
			{
				return value1.Item2.Type.CompareTo(value2.Item2.Type);
			}
			return value1.Item3.CompareTo(value2.Item3);
		}

		/// <summary>
		/// Transforms several cards to other cards.
		/// </summary>
		/// <param name="transformations">Cards to transform.</param>
		/// <param name="rng">Random number generator to use when choosing from random options.</param>
		/// <param name="style">How the transformed cards are displayed to the player.</param>
		/// <returns>The replacement card.</returns>
		// Token: 0x060087D0 RID: 34768 RVA: 0x002DB8D4 File Offset: 0x002D9AD4
		public static async Task<IEnumerable<CardPileAddResult>> Transform(IEnumerable<CardTransformation> transformations, [Nullable(2)] Rng rng, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			IEnumerable<CardPileAddResult> enumerable;
			if (CombatManager.Instance.IsEnding)
			{
				enumerable = Array.Empty<CardPileAddResult>();
			}
			else
			{
				CardTransformation[] transformationsArr = transformations.ToArray<CardTransformation>();
				if (transformationsArr.Length == 0)
				{
					enumerable = Array.Empty<CardPileAddResult>();
				}
				else
				{
					ICombatState combatState = transformationsArr[0].Original.CombatState;
					List<ValueTuple<CardTransformation, CardPile, int, CardModel>> transformationsWithOriginalData = new List<ValueTuple<CardTransformation, CardPile, int, CardModel>>();
					foreach (CardTransformation cardTransformation in transformationsArr)
					{
						cardTransformation.Original.AssertMutable();
						if (!cardTransformation.Original.IsTransformable)
						{
							throw new InvalidOperationException("Can't transform " + cardTransformation.Original.Id.Entry + " because it's un-transformable.");
						}
						CardPile pile2 = cardTransformation.Original.Pile;
						if (pile2 == null)
						{
							throw new InvalidOperationException("Can't transform " + cardTransformation.Original.Id.Entry + " because it has no pile.");
						}
						int num = pile2.Cards.IndexOf(cardTransformation.Original);
						CardModel replacement2 = cardTransformation.GetReplacement(rng);
						if (replacement2 == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Attempting to transform un-transformable card ");
							defaultInterpolatedStringHandler.AppendFormatted<CardModel>(cardTransformation.Original);
							defaultInterpolatedStringHandler.AppendLiteral("!");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						cardTransformation.Original.RemoveFromCurrentPile(false);
						transformationsWithOriginalData.Add(new ValueTuple<CardTransformation, CardPile, int, CardModel>(cardTransformation, pile2, num, replacement2));
					}
					List<ValueTuple<CardTransformation, CardPile, int, CardModel>> list = transformationsWithOriginalData;
					Comparison<ValueTuple<CardTransformation, CardPile, int, CardModel>> comparison;
					if ((comparison = CardCmd.<>O.<0>__PileIndexSort) == null)
					{
						comparison = (CardCmd.<>O.<0>__PileIndexSort = new Comparison<ValueTuple<CardTransformation, CardPile, int, CardModel>>(CardCmd.PileIndexSort));
					}
					list.Sort(comparison);
					List<CardPileAddResult> results = new List<CardPileAddResult>();
					foreach (ValueTuple<CardTransformation, CardPile, int, CardModel> valueTuple in transformationsWithOriginalData)
					{
						ValueTuple<CardTransformation, CardPile, int, CardModel> valueTuple;
						CardTransformation item = valueTuple.Item1;
						CardPile pile = valueTuple.Item2;
						int item2 = valueTuple.Item3;
						CardModel item3 = valueTuple.Item4;
						CardModel original = item.Original;
						IRunState runState = original.Owner.RunState;
						CardModel replacement = item3;
						replacement.AssertMutable();
						CardPileAddResult result = new CardPileAddResult
						{
							success = true,
							cardAdded = replacement,
							modifyingModels = null
						};
						if (replacement.Owner != original.Owner)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Attempting to transform card ");
							defaultInterpolatedStringHandler.AppendFormatted<CardModel>(original);
							defaultInterpolatedStringHandler.AppendLiteral(" to ");
							defaultInterpolatedStringHandler.AppendFormatted<CardModel>(replacement);
							defaultInterpolatedStringHandler.AppendLiteral(", but the replacement has a different owner!");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (pile.Type == PileType.Deck)
						{
							List<AbstractModel> list2;
							CardModel cardModel = Hook.ModifyCardBeingAddedToDeck(runState, replacement, out list2);
							replacement = cardModel;
							result.cardAdded = replacement;
							result.modifyingModels = list2;
							replacement.FloorAddedToDeck = new int?(runState.TotalFloor);
							MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
							if (currentMapPointHistoryEntry != null)
							{
								currentMapPointHistoryEntry.GetEntry(original.Owner.NetId).CardsTransformed.Add(new CardTransformationHistoryEntry(original, replacement));
							}
						}
						PileType type = pile.Type;
						if (type == PileType.Deck)
						{
							pile.AddInternal(replacement, -1, false);
						}
						else
						{
							pile.AddInternal(replacement, item2, false);
							CombatManager.Instance.History.CardGenerated(combatState, replacement, replacement.Owner);
							await Hook.AfterCardEnteredCombat(combatState, replacement);
						}
						await Hook.AfterCardChangedPiles(runState, combatState, replacement, pile.Type, null);
						pile.InvokeCardAddFinished();
						original.AfterTransformedFrom();
						replacement.AfterTransformedTo();
						results.Add(result);
						original = null;
						runState = null;
						replacement = null;
						result = default(CardPileAddResult);
						pile = null;
					}
					List<ValueTuple<CardTransformation, CardPile, int, CardModel>>.Enumerator enumerator = default(List<ValueTuple<CardTransformation, CardPile, int, CardModel>>.Enumerator);
					List<Task> vfxTasks = new List<Task>();
					for (int i = 0; i < results.Count; i++)
					{
						ValueTuple<CardTransformation, CardPile, int, CardModel> valueTuple = transformationsWithOriginalData[i];
						CardModel original2 = valueTuple.Item1.Original;
						CardModel cardAdded = results[i].cardAdded;
						if (LocalContext.IsMine(cardAdded))
						{
							if (cardAdded.Pile.Type == PileType.Hand)
							{
								if (!TestMode.IsOn)
								{
									NCardPlayQueue playQueue = NCombatRoom.Instance.Ui.PlayQueue;
									NPlayerHand hand = NCombatRoom.Instance.Ui.Hand;
									NCard ncard = NCard.FindOnTable(original2, new PileType?(PileType.Hand));
									if (ncard == null)
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
										defaultInterpolatedStringHandler.AppendLiteral("Couldn't get hand node for original card ");
										defaultInterpolatedStringHandler.AppendFormatted<CardModel>(transformationsArr[i].Original);
										defaultInterpolatedStringHandler.AppendLiteral("!");
										throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
									}
									if (playQueue.IsAncestorOf(ncard))
									{
										playQueue.RemoveCardFromQueueForCancellation(ncard, true);
									}
									hand.TryCancelCardPlay(original2);
									NCardTransformShineVfx ncardTransformShineVfx = NCardTransformShineVfx.Create(ncard, cardAdded, Array.Empty<RelicModel>());
									if (ncardTransformShineVfx != null)
									{
										vfxTasks.Add(ncardTransformShineVfx.PlayAnimationWithoutWaitingForEnd(false));
										await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
									}
								}
							}
							else if (style != CardPreviewStyle.None && TestMode.IsOff)
							{
								Control control2;
								switch (style)
								{
								case CardPreviewStyle.HorizontalLayout:
								{
									NCombatRoom instance = NCombatRoom.Instance;
									Control control;
									if ((control = ((instance != null) ? instance.Ui.CardPreviewContainer : null)) == null)
									{
										NRun instance2 = NRun.Instance;
										control = ((instance2 != null) ? instance2.GlobalUi.CardPreviewContainer : null);
									}
									control2 = control;
									break;
								}
								case CardPreviewStyle.MessyLayout:
								{
									NCombatRoom instance3 = NCombatRoom.Instance;
									NMessyCardPreviewContainer nmessyCardPreviewContainer;
									if ((nmessyCardPreviewContainer = ((instance3 != null) ? instance3.Ui.MessyCardPreviewContainer : null)) == null)
									{
										NRun instance4 = NRun.Instance;
										nmessyCardPreviewContainer = ((instance4 != null) ? instance4.GlobalUi.MessyCardPreviewContainer : null);
									}
									control2 = nmessyCardPreviewContainer;
									break;
								}
								case CardPreviewStyle.EventLayout:
								{
									NRun instance5 = NRun.Instance;
									control2 = ((instance5 != null) ? instance5.GlobalUi.EventCardPreviewContainer : null);
									break;
								}
								case CardPreviewStyle.GridLayout:
								{
									NRun instance6 = NRun.Instance;
									control2 = ((instance6 != null) ? instance6.GlobalUi.GridCardPreviewContainer : null);
									break;
								}
								default:
								{
									string text = "style";
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
									defaultInterpolatedStringHandler.AppendLiteral("Unexpected ");
									defaultInterpolatedStringHandler.AppendFormatted("CardPreviewStyle");
									defaultInterpolatedStringHandler.AppendLiteral(" ");
									defaultInterpolatedStringHandler.AppendFormatted<CardPreviewStyle>(style);
									defaultInterpolatedStringHandler.AppendLiteral("!");
									throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
								}
								}
								Control control3 = control2;
								if (control3 != null)
								{
									List<AbstractModel> modifyingModels = results[i].modifyingModels;
									control3.AddChildSafely(NCardTransformVfx.Create(original2, cardAdded, (modifyingModels != null) ? modifyingModels.OfType<RelicModel>() : null));
								}
							}
						}
					}
					await Task.WhenAll(vfxTasks);
					for (int i = 0; i < results.Count; i++)
					{
						CardPileAddResult cardPileAddResult = results[i];
						if (cardPileAddResult.success && cardPileAddResult.cardAdded.Pile.Type.IsCombatPile())
						{
							await Hook.AfterCardGeneratedForCombat(cardPileAddResult.cardAdded.CombatState, cardPileAddResult.cardAdded, cardPileAddResult.cardAdded.Owner);
						}
						ValueTuple<CardTransformation, CardPile, int, CardModel> valueTuple = transformationsWithOriginalData[i];
						valueTuple.Item1.Original.RemoveFromState();
					}
					enumerable = results;
				}
			}
			return enumerable;
		}

		/// <summary>
		/// Apply an Enchantment to a card.
		/// </summary>
		/// <param name="card">Card to enchant.</param>
		/// <param name="amount">Amount of the enchantment to apply.</param>
		/// <typeparam name="T">Type of enchantment to apply.</typeparam>
		/// <returns>Enchantment that was applied, or null if it failed.</returns>
		// Token: 0x060087D1 RID: 34769 RVA: 0x002DB927 File Offset: 0x002D9B27
		[return: Nullable(2)]
		public static T Enchant<[Nullable(0)] T>(CardModel card, decimal amount) where T : EnchantmentModel
		{
			return CardCmd.Enchant(ModelDb.Enchantment<T>().ToMutable(), card, amount) as T;
		}

		/// <summary>
		/// Apply an Enchantment to a card.
		/// Use this for actually enchanting a card, not for previewing an enchantment, because it won't highlight value
		/// changes.
		/// </summary>
		/// <param name="enchantment">Enchantment to apply.</param>
		/// <param name="card">Card to enchant.</param>
		/// <param name="amount">Amount of the enchantment to apply.</param>
		/// <returns>Enchantment that was applied, or null if it failed.</returns>
		// Token: 0x060087D2 RID: 34770 RVA: 0x002DB94C File Offset: 0x002D9B4C
		[return: Nullable(2)]
		public static EnchantmentModel Enchant(EnchantmentModel enchantment, CardModel card, decimal amount)
		{
			enchantment.AssertMutable();
			if (!enchantment.CanEnchant(card))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot enchant ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" with ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(enchantment.Id);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (card.Enchantment == null)
			{
				card.EnchantInternal(enchantment, amount);
				enchantment.ModifyCard();
			}
			else
			{
				if (!(card.Enchantment.GetType() == enchantment.GetType()))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot enchant ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" with ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(enchantment.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" because it already has enchantment ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Enchantment.Id);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				card.Enchantment.Amount += (int)amount;
			}
			card.FinalizeUpgradeInternal();
			CardPile pile = card.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = card.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(card.Owner.NetId).CardsEnchanted.Add(new CardEnchantmentHistoryEntry(card, enchantment.Id));
				}
			}
			return card.Enchantment;
		}

		/// <summary>
		/// Clear a card's Enchantment if it has one.
		/// </summary>
		/// <param name="card">Card whose enchantment we want to clear.</param>
		// Token: 0x060087D3 RID: 34771 RVA: 0x002DBAD8 File Offset: 0x002D9CD8
		public static void ClearEnchantment(CardModel card)
		{
			card.ClearEnchantmentInternal();
		}

		/// <summary>
		/// Apply Afflictions to cards and show it to the owning player if they are the local player.
		/// </summary>
		/// <param name="cards">Cards to afflict.</param>
		/// <param name="amount">Amount of the affliction to apply.</param>
		/// <param name="style">The style of preview to use.</param>
		/// <typeparam name="T">Type of affliction to apply.</typeparam>
		/// <returns>Afflictions that were applied.</returns>
		// Token: 0x060087D4 RID: 34772 RVA: 0x002DBAE0 File Offset: 0x002D9CE0
		public static async Task<IEnumerable<T>> AfflictAndPreview<[Nullable(0)] T>(IEnumerable<CardModel> cards, decimal amount, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout) where T : AfflictionModel
		{
			List<T> afflictions = new List<T>();
			List<CardModel> cardList = new List<CardModel>();
			foreach (CardModel card in cards)
			{
				T t = await CardCmd.Afflict<T>(card, amount);
				T t2 = t;
				if (t2 != null)
				{
					afflictions.Add(t2);
					cardList.Add(card);
				}
				card = null;
			}
			IEnumerator<CardModel> enumerator = null;
			if (cardList.Count > 0 && style != CardPreviewStyle.None)
			{
				if (cardList.Any<CardModel>((CardModel c) => c.Owner != cardList[0].Owner))
				{
					throw new InvalidOperationException("All cards passed to AfflictAndPreview must have the same owner!");
				}
				if (LocalContext.IsMine(cardList[0]))
				{
					CardCmd.Preview(cardList, 1.2f, style);
					await Cmd.Wait(1.25f, false);
				}
			}
			return afflictions;
		}

		/// <summary>
		/// Apply an Affliction to a card.
		/// </summary>
		/// <param name="card">Card to afflict.</param>
		/// <param name="amount">Amount of the affliction to apply.</param>
		/// <typeparam name="T">Type of affliction to apply.</typeparam>
		/// <returns>Affliction that was applied, or null if it failed.</returns>
		// Token: 0x060087D5 RID: 34773 RVA: 0x002DBB34 File Offset: 0x002D9D34
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<T> Afflict<[Nullable(0)] T>(CardModel card, decimal amount) where T : AfflictionModel
		{
			AfflictionModel afflictionModel = await CardCmd.Afflict(ModelDb.Affliction<T>().ToMutable(), card, amount);
			return afflictionModel as T;
		}

		/// <summary>
		/// Apply an Affliction to a card.
		/// </summary>
		/// <param name="affliction">Affliction to apply.</param>
		/// <param name="card">Card to afflict.</param>
		/// <param name="amount">Amount of the affliction to apply.</param>
		/// <returns>Affliction that was applied, or null if it failed.</returns>
		// Token: 0x060087D6 RID: 34774 RVA: 0x002DBB80 File Offset: 0x002D9D80
		[return: Nullable(new byte[] { 1, 2 })]
		public static Task<AfflictionModel> Afflict(AfflictionModel affliction, CardModel card, decimal amount)
		{
			if (CombatManager.Instance.IsOverOrEnding)
			{
				CardPile pile = card.Pile;
				if (pile != null && pile.IsCombatPile)
				{
					return Task.FromResult<AfflictionModel>(null);
				}
			}
			affliction.AssertMutable();
			ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
			if (combatState == null || !Hook.ShouldAfflict(combatState, card, affliction))
			{
				return Task.FromResult<AfflictionModel>(null);
			}
			if (!affliction.CanAfflict(card))
			{
				return Task.FromResult<AfflictionModel>(null);
			}
			if (card.Affliction == null)
			{
				card.AfflictInternal(affliction, amount);
				affliction.AfterApplied();
			}
			else
			{
				if (!(card.Affliction.GetType() == affliction.GetType()))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot afflict ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" with ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(affliction.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" because it already has affliction ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Affliction.Id);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				card.Affliction.Amount += (int)amount;
			}
			CombatManager.Instance.History.CardAfflicted(combatState, card, affliction);
			return Task.FromResult<AfflictionModel>(card.Affliction);
		}

		/// <summary>
		/// Clear a card's Affliction if it has one.
		/// </summary>
		/// <param name="card">Card whose affliction we want to clear.</param>
		// Token: 0x060087D7 RID: 34775 RVA: 0x002DBCD4 File Offset: 0x002D9ED4
		public static void ClearAffliction(CardModel card)
		{
			card.ClearAfflictionInternal();
		}

		/// <summary>
		/// Apply keywords to a card.
		/// </summary>
		/// <param name="card">Card to apply keyword too.</param>
		/// <param name="keywords">keywords to apply.</param>
		// Token: 0x060087D8 RID: 34776 RVA: 0x002DBCDC File Offset: 0x002D9EDC
		public static void ApplyKeyword(CardModel card, params CardKeyword[] keywords)
		{
			foreach (CardKeyword cardKeyword in keywords)
			{
				card.AddKeyword(cardKeyword);
			}
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard == null)
			{
				return;
			}
			ncard.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x002DBD2C File Offset: 0x002D9F2C
		public static void RemoveKeyword(CardModel card, params CardKeyword[] keywords)
		{
			foreach (CardKeyword cardKeyword in keywords)
			{
				card.RemoveKeyword(cardKeyword);
			}
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard == null)
			{
				return;
			}
			ncard.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Apply Sly to a card for the current turn only.
		/// </summary>
		/// <param name="card">Card to apply single-turn Sly to.</param>
		// Token: 0x060087DA RID: 34778 RVA: 0x002DBD7C File Offset: 0x002D9F7C
		public static void ApplySingleTurnSly(CardModel card)
		{
			card.GiveSingleTurnSly();
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard == null)
			{
				return;
			}
			ncard.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Apply Retain to a card for the current turn only.
		/// </summary>
		/// <param name="card">Card to apply single-turn Retain to.</param>
		// Token: 0x060087DB RID: 34779 RVA: 0x002DBDB4 File Offset: 0x002D9FB4
		public static void ApplySingleTurnRetain(CardModel card)
		{
			card.GiveSingleTurnRetain();
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard == null)
			{
				return;
			}
			ncard.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Creates a set of NCards that spawn in the middle of the screen, then fly to the pile they're in.
		/// Useful for when you want to preview cards that are being added to a pile.
		/// </summary>
		/// <param name="card">Card to preview.</param>
		/// <param name="time">How long the card lingers before it goes off screen</param>
		/// <param name="style">How multiple cards are aligned if previewed together.</param>
		// Token: 0x060087DC RID: 34780 RVA: 0x002DBDEC File Offset: 0x002D9FEC
		[return: Nullable(2)]
		public static TaskCompletionSource Preview(CardModel card, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			return CardCmd.PreviewInternal(card, false, null, time, style);
		}

		/// <summary>
		/// Creates a set of NCards that spawn in the middle of the screen, then fly to the pile they're in.
		/// Useful for when you want to preview cards that are being added to a pile.
		/// </summary>
		/// <param name="cards">Cards to preview.</param>
		/// <param name="time">How long the card lingers before it goes off screen</param>
		/// <param name="style">How multiple cards are aligned if previewed together.</param>
		// Token: 0x060087DD RID: 34781 RVA: 0x002DBDF8 File Offset: 0x002D9FF8
		public static void Preview(IReadOnlyList<CardModel> cards, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			foreach (CardModel cardModel in cards)
			{
				CardCmd.PreviewInternal(cardModel, false, null, time, style);
			}
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x002DBE5C File Offset: 0x002DA05C
		public static void PreviewCardPileAdd(CardPileAddResult result, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			if (!result.success)
			{
				return;
			}
			if (!LocalContext.IsMine(result.cardAdded))
			{
				return;
			}
			CardModel cardAdded = result.cardAdded;
			bool flag = true;
			List<AbstractModel> modifyingModels = result.modifyingModels;
			CardCmd.PreviewInternal(cardAdded, flag, ((modifyingModels != null) ? modifyingModels.OfType<RelicModel>() : null) ?? null, time, style);
		}

		// Token: 0x060087DF RID: 34783 RVA: 0x002DBEBC File Offset: 0x002DA0BC
		public static void PreviewCardPileAdd(IReadOnlyList<CardPileAddResult> results, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return;
			}
			if (results.Count > 5 && style == CardPreviewStyle.HorizontalLayout)
			{
				Log.Warn("Horizontal layout is being used with more than five cards! They will go offscreen", 2);
			}
			foreach (CardPileAddResult cardPileAddResult in results)
			{
				if (cardPileAddResult.success && LocalContext.IsMine(cardPileAddResult.cardAdded))
				{
					CardModel cardAdded = cardPileAddResult.cardAdded;
					bool flag = true;
					List<AbstractModel> modifyingModels = cardPileAddResult.modifyingModels;
					CardCmd.PreviewInternal(cardAdded, flag, ((modifyingModels != null) ? modifyingModels.OfType<RelicModel>() : null) ?? null, time, style);
				}
			}
		}

		// Token: 0x060087E0 RID: 34784 RVA: 0x002DBF68 File Offset: 0x002DA168
		[return: Nullable(2)]
		private static TaskCompletionSource PreviewInternal(CardModel card, bool isAddingCardsToPile, [Nullable(new byte[] { 2, 1 })] IEnumerable<RelicModel> relicsToFlash = null, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
		{
			if (card.Pile == null)
			{
				return null;
			}
			if (TestMode.IsOn)
			{
				return null;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return null;
			}
			if (!LocalContext.IsMine(card))
			{
				return null;
			}
			PileType pileType = card.Pile.Type;
			if (CardCmd.GetTotalCardsBeingPreviewed() > 50)
			{
				return null;
			}
			Control control2;
			switch (style)
			{
			case CardPreviewStyle.HorizontalLayout:
			{
				Control control;
				if (!pileType.IsCombatPile())
				{
					NRun instance = NRun.Instance;
					control = ((instance != null) ? instance.GlobalUi.CardPreviewContainer : null);
				}
				else
				{
					control = NCombatRoom.Instance.Ui.CardPreviewContainer;
				}
				control2 = control;
				break;
			}
			case CardPreviewStyle.MessyLayout:
			{
				Control control3;
				if (!pileType.IsCombatPile())
				{
					NRun instance2 = NRun.Instance;
					control3 = ((instance2 != null) ? instance2.GlobalUi.MessyCardPreviewContainer : null);
				}
				else
				{
					control3 = NCombatRoom.Instance.Ui.MessyCardPreviewContainer;
				}
				control2 = control3;
				break;
			}
			case CardPreviewStyle.EventLayout:
			{
				if (pileType.IsCombatPile())
				{
					throw new InvalidOperationException();
				}
				NRun instance3 = NRun.Instance;
				control2 = ((instance3 != null) ? instance3.GlobalUi.EventCardPreviewContainer : null);
				break;
			}
			case CardPreviewStyle.GridLayout:
			{
				if (pileType.IsCombatPile())
				{
					throw new InvalidOperationException();
				}
				NRun instance4 = NRun.Instance;
				control2 = ((instance4 != null) ? instance4.GlobalUi.GridCardPreviewContainer : null);
				break;
			}
			default:
			{
				string text = "style";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected ");
				defaultInterpolatedStringHandler.AppendFormatted("CardPreviewStyle");
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<CardPreviewStyle>(style);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			Control control4 = control2;
			if (control4 == null)
			{
				return null;
			}
			if (style == CardPreviewStyle.HorizontalLayout && control4.GetChildCount(false) > 5)
			{
				Control control5;
				if (!pileType.IsCombatPile())
				{
					NRun instance5 = NRun.Instance;
					control5 = ((instance5 != null) ? instance5.GlobalUi.MessyCardPreviewContainer : null);
				}
				else
				{
					control5 = NCombatRoom.Instance.Ui.MessyCardPreviewContainer;
				}
				control4 = control5;
			}
			NCard node = NCard.Create(card, ModelVisibility.Visible);
			if (control4 != null)
			{
				control4.AddChildSafely(node);
			}
			node.UpdateVisuals(pileType, CardPreviewMode.Normal);
			TaskCompletionSource source = new TaskCompletionSource();
			Tween tween = node.CreateTween();
			tween.TweenProperty(node, "scale", Vector2.One, 0.25).From(Vector2.Zero).SetEase(1L)
				.SetTrans(7L);
			tween.TweenCallback(Callable.From(delegate
			{
				TaskHelper.RunSafely(CardCmd.FlashRelics(node, relicsToFlash));
			}));
			Action<Task> <>9__2;
			tween.TweenCallback(Callable.From(delegate
			{
				NCardFlyVfx ncardFlyVfx = null;
				Node node2;
				if (pileType == PileType.Deck)
				{
					NRun instance6 = NRun.Instance;
					node2 = ((instance6 != null) ? instance6.GlobalUi.TopBar.TrailContainer : null);
				}
				else
				{
					node2 = card.Owner.Creature.GetVfxContainer();
				}
				if (node2 != null)
				{
					PileType pileType2 = ((card.Pile != null) ? card.Pile.Type : pileType);
					ncardFlyVfx = NCardFlyVfx.Create(node, pileType2, isAddingCardsToPile, card.Owner.Character.TrailPath);
				}
				if (ncardFlyVfx != null && node2 != null)
				{
					node2.AddChildSafely(ncardFlyVfx);
					Task task = ncardFlyVfx.SwooshAwayCompletion.Task;
					Action<Task> action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate(Task _)
						{
							source.SetResult();
						});
					}
					TaskHelper.RunSafely(task.ContinueWith(action));
					return;
				}
				node.QueueFreeSafely();
				source.SetResult();
			})).SetDelay((double)time);
			return source;
		}

		// Token: 0x060087E1 RID: 34785 RVA: 0x002DC248 File Offset: 0x002DA448
		private static int GetTotalCardsBeingPreviewed()
		{
			int num = 0;
			if (NCombatRoom.Instance != null)
			{
				num = NCombatRoom.Instance.Ui.CardPreviewContainer.GetChildCount(false);
				num += NCombatRoom.Instance.Ui.MessyCardPreviewContainer.GetChildCount(false);
			}
			if (NRun.Instance != null)
			{
				num += NRun.Instance.GlobalUi.CardPreviewContainer.GetChildCount(false);
				num += NRun.Instance.GlobalUi.MessyCardPreviewContainer.GetChildCount(false);
				num += NRun.Instance.GlobalUi.EventCardPreviewContainer.GetChildCount(false);
				num += NRun.Instance.GlobalUi.GridCardPreviewContainer.GetChildCount(false);
			}
			return num;
		}

		// Token: 0x060087E2 RID: 34786 RVA: 0x002DC2F4 File Offset: 0x002DA4F4
		private static Task FlashRelics(NCard node, [Nullable(new byte[] { 2, 1 })] IEnumerable<RelicModel> relicsToFlash)
		{
			if (relicsToFlash == null)
			{
				return Task.CompletedTask;
			}
			foreach (RelicModel relicModel in relicsToFlash)
			{
				relicModel.Flash();
				node.FlashRelicOnCard(relicModel);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0200243B RID: 9275
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04009903 RID: 39171
			[Nullable(new byte[] { 0, 0, 1, 1 })]
			public static Comparison<ValueTuple<CardTransformation, CardPile, int, CardModel>> <0>__PileIndexSort;
		}
	}
}
