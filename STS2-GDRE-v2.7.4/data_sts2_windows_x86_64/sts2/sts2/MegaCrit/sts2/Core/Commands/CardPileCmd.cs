using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D2A RID: 3370
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardPileCmd
	{
		/// <summary>
		/// Remove a card from the deck and play an animation.
		/// </summary>
		/// <param name="card">Card to remove.</param>
		/// <param name="showPreview">Whether to show a preview of the card being removed.</param>
		// Token: 0x060087E3 RID: 34787 RVA: 0x002DC350 File Offset: 0x002DA550
		public static async Task RemoveFromDeck(CardModel card, bool showPreview = true)
		{
			await CardPileCmd.RemoveFromDeck(new <>z__ReadOnlySingleElementList<CardModel>(card), showPreview);
		}

		/// <summary>
		/// Remove cards from the deck and play an animation.
		/// </summary>
		/// <param name="cards">Cards to remove.</param>
		/// <param name="showPreview">Whether to show a preview of the card being removed.</param>
		// Token: 0x060087E4 RID: 34788 RVA: 0x002DC39C File Offset: 0x002DA59C
		public static async Task RemoveFromDeck(IReadOnlyList<CardModel> cards, bool showPreview = true)
		{
			foreach (CardModel card in cards)
			{
				if (card.Pile.Type != PileType.Deck)
				{
					throw new InvalidOperationException("You cannot remove a card that is not in the deck.");
				}
				MapPointHistoryEntry currentMapPointHistoryEntry = card.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(card.Owner.NetId).CardsRemoved.Add(card.ToSerializable());
				}
				await Hook.BeforeCardRemoved(card.Owner.RunState, card);
				card.RemoveFromCurrentPile(false);
				if (showPreview && LocalContext.IsMine(card))
				{
					NCard cardNode = NCard.Create(card, ModelVisibility.Visible);
					if (cardNode != null)
					{
						NRun.Instance.GlobalUi.CardPreviewContainer.AddChildSafely(cardNode);
						cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
						Tween tween = cardNode.CreateTween();
						tween.TweenProperty(cardNode, "scale", Vector2.One * 1f, 0.25).From(Vector2.Zero).SetEase(1L)
							.SetTrans(7L);
						if (!TestMode.IsOn)
						{
							tween.TweenInterval(0.25);
							tween.TweenCallback(Callable.From(delegate
							{
								NCardRemoveVfx ncardRemoveVfx = NCardRemoveVfx.Create(cardNode);
								NRun.Instance.GlobalUi.AboveTopBarVfxContainer.AddChildSafely(ncardRemoveVfx);
							}));
							tween.TweenInterval(0.4000000059604645);
						}
						tween.TweenCallback(Callable.From(new Action(cardNode.QueueFreeSafely)));
					}
				}
				card.RemoveFromState();
				card = null;
			}
			IEnumerator<CardModel> enumerator = null;
		}

		/// <summary>
		/// Remove a card from combat and play an animation.
		/// This will remove the card from any combat pile (<see cref="M:MegaCrit.Sts2.Core.Entities.Cards.PileTypeExtensions.IsCombatPile(MegaCrit.Sts2.Core.Entities.Cards.PileType)" />) it's in,
		/// but not from the player's deck.
		/// </summary>
		/// <param name="card">Card to remove.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087E5 RID: 34789 RVA: 0x002DC3E8 File Offset: 0x002DA5E8
		public static async Task RemoveFromCombat(CardModel card, bool skipVisuals = false)
		{
			await CardPileCmd.RemoveFromCombat(new <>z__ReadOnlySingleElementList<CardModel>(card), skipVisuals);
		}

		/// <summary>
		/// Remove cards from combat and play an animation.
		/// This will remove the cards from any combat pile (<see cref="M:MegaCrit.Sts2.Core.Entities.Cards.PileTypeExtensions.IsCombatPile(MegaCrit.Sts2.Core.Entities.Cards.PileType)" />) they're
		/// in, but not from the player's deck.
		/// </summary>
		/// <param name="cards">Cards to remove.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087E6 RID: 34790 RVA: 0x002DC434 File Offset: 0x002DA634
		public static async Task RemoveFromCombat(IEnumerable<CardModel> cards, bool skipVisuals = false)
		{
			if (cards.Any<CardModel>())
			{
				ICombatState combatState = cards.First<CardModel>().CombatState;
				IRunState runState = cards.First<CardModel>().Owner.RunState;
				List<NCard> list = new List<NCard>();
				Dictionary<CardModel, CardPile> oldPiles = new Dictionary<CardModel, CardPile>();
				foreach (CardModel cardModel in cards)
				{
					CardPile pile = cardModel.Pile;
					if (pile == null || !pile.IsCombatPile)
					{
						throw new InvalidOperationException("Card must be in a combat pile for it to be removed");
					}
					if ((cardModel.Pile.Type != PileType.Play || cardModel.Type != CardType.Power) && !skipVisuals)
					{
						NCard ncard = NCard.FindOnTable(cardModel, null);
						if (ncard != null)
						{
							list.Add(ncard);
						}
					}
					oldPiles.Add(cardModel, cardModel.Pile);
					cardModel.RemoveFromCurrentPile(false);
				}
				if (list.Count != 0)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NPlayerHand nplayerHand = ((instance != null) ? instance.Ui.Hand : null);
					NCombatRoom instance2 = NCombatRoom.Instance;
					NCardPlayQueue ncardPlayQueue = ((instance2 != null) ? instance2.Ui.PlayQueue : null);
					Tween tween = null;
					for (int i = 0; i < list.Count; i++)
					{
						NCard node = list[i];
						Vector2 globalPosition = node.GlobalPosition;
						CardModel model = node.Model;
						CardPile cardPile = oldPiles[model];
						bool isInPlayQueue = ncardPlayQueue != null && ncardPlayQueue.IsAncestorOf(node);
						if (isInPlayQueue)
						{
							ncardPlayQueue.RemoveCardFromQueueForCancellation(node, false);
						}
						if (nplayerHand != null && nplayerHand.IsAncestorOf(node))
						{
							nplayerHand.Remove(model);
						}
						else
						{
							Node parent = node.GetParent();
							if (parent != null)
							{
								parent.RemoveChildSafely(node);
							}
						}
						NCombatRoom instance3 = NCombatRoom.Instance;
						if (instance3 != null)
						{
							instance3.Ui.AddChildSafely(node);
						}
						node.GlobalPosition = globalPosition;
						if (tween == null)
						{
							NCombatRoom instance4 = NCombatRoom.Instance;
							tween = ((instance4 != null) ? instance4.CreateTween() : null);
							if (tween != null)
							{
								tween.SetParallel(true);
							}
						}
						CardPile pile2 = model.Pile;
						if (pile2 != null)
						{
							pile2.InvokeCardAddFinished();
						}
						if (cardPile.Type != PileType.Hand && cardPile.Type != PileType.Play)
						{
							CardPileCmd.AppendPileLerpTween(tween, node, PileType.Play, cardPile);
						}
						if (tween != null)
						{
							tween.Chain().TweenCallback(Callable.From(delegate
							{
								NCombatRoom instance5 = NCombatRoom.Instance;
								NCardExhaustVfx ncardExhaustVfx = ((instance5 != null) ? NCardExhaustVfx.Create(node) : null);
								if (ncardExhaustVfx != null)
								{
									if (instance5 != null)
									{
										instance5.Ui.AddChildSafely(ncardExhaustVfx);
									}
									NDebugAudioManager instance6 = NDebugAudioManager.Instance;
									if (instance6 != null)
									{
										instance6.Play("card_exhaust.mp3", 1f, PitchVariance.None);
									}
									TaskHelper.RunSafely(ncardExhaustVfx.PlayAnimation());
									return;
								}
								if (!isInPlayQueue)
								{
									node.QueueFreeSafely();
								}
							}));
						}
					}
					if (tween != null)
					{
						tween.Play();
						if (NCombatRoom.Instance != null)
						{
							bool flag = await tween.AwaitFinished(NCombatRoom.Instance);
						}
					}
				}
				foreach (KeyValuePair<CardModel, CardPile> keyValuePair in oldPiles)
				{
					CardPile pile;
					CardModel cardModel2;
					keyValuePair.Deconstruct(out cardModel2, out pile);
					CardModel oldCard = cardModel2;
					await Hook.AfterCardChangedPiles(runState, combatState, oldCard, pile.Type, null);
					oldCard.RemoveFromState();
					oldCard = null;
				}
				Dictionary<CardModel, CardPile>.Enumerator enumerator2 = default(Dictionary<CardModel, CardPile>.Enumerator);
			}
		}

		/// <summary>
		/// Change the owner of the card, placing it in <paramref name="player" />'s card pile.
		/// In most cases, you DO NOT want to call this directly. Calling this method during <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />
		/// will cause post-play hooks like <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.AfterCardPlayed(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> to trigger on the receiving player,
		/// not the owning player. Instead, override <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.GetResultLocationForCardPlay" /> or use the hook
		/// <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.ModifyCardPlayResultLocation(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Models.CardModel,System.Boolean,MegaCrit.Sts2.Core.Entities.Cards.ResourceInfo,MegaCrit.Sts2.Core.Entities.Cards.CardLocation,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Models.AbstractModel}@)" /> to change the result player, which will call this after the
		/// card play is fully complete.
		/// </summary>
		/// <param name="card">The card to change owners.</param>
		/// <param name="player">The new owner of the card.</param>
		/// <param name="pileType">The new pile where the card will be placed.</param>
		/// <param name="position">The position within the new pile where the card will be placed.</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from
		/// recursing. TODO is this necessary?</param>
		// Token: 0x060087E7 RID: 34791 RVA: 0x002DC480 File Offset: 0x002DA680
		public static async Task GiveToAnotherPlayer(CardModel card, Player player, PileType pileType, CardPilePosition position = CardPilePosition.Bottom, [Nullable(2)] AbstractModel clonedBy = null)
		{
			NCard cardNode = NCard.FindOnTable(card, null);
			card.RemoveFromCurrentPile(true);
			card.GiveToAnotherPlayer(player);
			bool islocalPlayerTheReceivingPlayer = LocalContext.IsMine(card);
			await CardPileCmd.Add(new <>z__ReadOnlySingleElementList<CardModel>(card), pileType.GetPile(player), position, clonedBy, true, true);
			if (cardNode != null && cardNode.IsValid())
			{
				Node vfxContainer = card.Owner.Creature.GetVfxContainer();
				cardNode.Reparent(vfxContainer, true);
				if (islocalPlayerTheReceivingPlayer)
				{
					NCard ncard = cardNode;
					CardPile pile = card.Pile;
					NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(ncard, (pile != null) ? pile.Type : pileType, true, card.Owner.Character.TrailPath);
					if (vfxContainer != null)
					{
						vfxContainer.AddChildSafely(ncardFlyVfx);
					}
				}
				else
				{
					NCardFlyVfx ncardFlyVfx2 = NCardFlyVfx.Create(cardNode, player.Creature, card.Owner.Character.TrailPath);
					if (vfxContainer != null)
					{
						vfxContainer.AddChildSafely(ncardFlyVfx2);
					}
				}
			}
		}

		/// <summary>
		/// Adds a new card into one of the combat piles.
		/// Card must have just been generated (ie shivs, infernal blade generation, attack potion).
		/// We do this, instead of a regular add, because this adds the generated card entry to the combat history.
		/// </summary>
		/// <param name="card">Card to add.</param>
		/// <param name="newPileType">Type of pile to add the card to.</param>
		/// <param name="creator">Player that created this card if there is one</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		// Token: 0x060087E8 RID: 34792 RVA: 0x002DC4E4 File Offset: 0x002DA6E4
		public static async Task<CardPileAddResult> AddGeneratedCardToCombat(CardModel card, PileType newPileType, [Nullable(2)] Player creator, CardPilePosition position = CardPilePosition.Bottom)
		{
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(new <>z__ReadOnlySingleElementList<CardModel>(card), newPileType, creator, position);
			return readOnlyList[0];
		}

		/// <summary>
		/// Adds a new card into one of the combat piles.
		/// Card must have just been generated (ie shivs, infernal blade generation, attack potion).
		/// We do this, instead of a regular add, because this adds the generated card entry to the combat history.
		/// </summary>
		/// <param name="cards">Cards to add.</param>
		/// <param name="newPileType">Type of pile to add the card to.</param>
		/// <param name="creator">Player that created this card if there is one</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		// Token: 0x060087E9 RID: 34793 RVA: 0x002DC540 File Offset: 0x002DA740
		public static async Task<IReadOnlyList<CardPileAddResult>> AddGeneratedCardsToCombat(IEnumerable<CardModel> cards, PileType newPileType, [Nullable(2)] Player creator, CardPilePosition position = CardPilePosition.Bottom)
		{
			List<CardModel> list = cards.ToList<CardModel>();
			IReadOnlyList<CardPileAddResult> readOnlyList;
			if (list.Count == 0)
			{
				readOnlyList = Array.Empty<CardPileAddResult>();
			}
			else if (!CombatManager.Instance.IsInProgress)
			{
				readOnlyList = Array.Empty<CardPileAddResult>();
			}
			else
			{
				if (list.Any<CardModel>((CardModel c) => c.Pile != null))
				{
					throw new InvalidOperationException("You are not allowed to generate cards that already have a pile");
				}
				if (!newPileType.IsCombatPile())
				{
					throw new InvalidOperationException("You are not allowed to added generated cards to a non combat pile");
				}
				ICombatState combatState = list[0].Owner.Creature.CombatState;
				if (combatState == null)
				{
					readOnlyList = Array.Empty<CardPileAddResult>();
				}
				else
				{
					List<CardPileAddResult> results = new List<CardPileAddResult>();
					foreach (CardModel card in list)
					{
						CombatManager.Instance.History.CardGenerated(combatState, card, creator);
						List<CardPileAddResult> list2 = results;
						CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, newPileType.GetPile(card.Owner), position, null, false);
						list2.Add(cardPileAddResult);
						list2 = null;
						await Hook.AfterCardGeneratedForCombat(combatState, card, creator);
						card = null;
					}
					List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
					readOnlyList = results;
				}
			}
			return readOnlyList;
		}

		/// <summary>
		/// Add a card to a pile.
		/// </summary>
		/// <param name="card">Card to add.</param>
		/// <param name="newPileType">Type of pile to add the card to.</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087EA RID: 34794 RVA: 0x002DC59C File Offset: 0x002DA79C
		public static async Task<CardPileAddResult> Add(CardModel card, PileType newPileType, CardPilePosition position = CardPilePosition.Bottom, [Nullable(2)] AbstractModel clonedBy = null, bool skipVisuals = false)
		{
			if (card.Owner == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to add card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" to pile, but it has no owner!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return await CardPileCmd.Add(card, newPileType.GetPile(card.Owner), position, clonedBy, skipVisuals);
		}

		/// <summary>
		/// Add a card to a pile.
		/// </summary>
		/// <param name="card">Card to add.</param>
		/// <param name="newPile">Pile to add the card to.</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087EB RID: 34795 RVA: 0x002DC600 File Offset: 0x002DA800
		public static async Task<CardPileAddResult> Add(CardModel card, CardPile newPile, CardPilePosition position = CardPilePosition.Bottom, [Nullable(2)] AbstractModel clonedBy = null, bool skipVisuals = false)
		{
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.Add(new <>z__ReadOnlySingleElementList<CardModel>(card), newPile, position, clonedBy, skipVisuals, false);
			return readOnlyList[0];
		}

		/// <summary>
		/// Add multiple cards to a pile.
		/// </summary>
		/// <param name="cards">Cards to add.</param>
		/// <param name="newPileType">Type of pile to add the cards to.</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x060087EC RID: 34796 RVA: 0x002DC664 File Offset: 0x002DA864
		public static async Task<IReadOnlyList<CardPileAddResult>> Add(IEnumerable<CardModel> cards, PileType newPileType, CardPilePosition position = CardPilePosition.Bottom, [Nullable(2)] AbstractModel clonedBy = null, bool skipVisuals = false)
		{
			IReadOnlyList<CardPileAddResult> readOnlyList;
			if (!cards.Any<CardModel>())
			{
				readOnlyList = Array.Empty<CardPileAddResult>();
			}
			else
			{
				IReadOnlyList<CardPileAddResult> readOnlyList2 = await CardPileCmd.Add(cards, newPileType.GetPile(cards.First<CardModel>().Owner), position, clonedBy, skipVisuals, false);
				readOnlyList = readOnlyList2;
			}
			return readOnlyList;
		}

		/// <summary>
		/// Add multiple cards to a pile.
		/// </summary>
		/// <param name="cards">Cards to add.</param>
		/// <param name="newPile">Pile to add the cards to.</param>
		/// <param name="position">Optional position in the pile to add the cards to. Defaults to bottom.</param>
		/// <param name="clonedBy">The model that cloned this card, if applicable. Used to prevent copy effects from recursing.</param>
		/// <param name="skipVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		/// <param name="isChangingOwners">The card is being handed from one player to another. It was already in combat,
		/// so it must not be treated as newly entering combat (which would re-fire AfterCardEnteredCombat).</param>
		// Token: 0x060087ED RID: 34797 RVA: 0x002DC6C8 File Offset: 0x002DA8C8
		public static async Task<IReadOnlyList<CardPileAddResult>> Add(IEnumerable<CardModel> cards, CardPile newPile, CardPilePosition position = CardPilePosition.Bottom, [Nullable(2)] AbstractModel clonedBy = null, bool skipVisuals = false, bool isChangingOwners = false)
		{
			IReadOnlyList<CardPileAddResult> readOnlyList;
			if (!cards.Any<CardModel>())
			{
				readOnlyList = Array.Empty<CardPileAddResult>();
			}
			else if (newPile.IsCombatPile && CombatManager.Instance.IsEnding)
			{
				readOnlyList = cards.Select<CardModel, CardPileAddResult>((CardModel c) => new CardPileAddResult
				{
					cardAdded = c,
					success = false
				}).ToList<CardPileAddResult>();
			}
			else
			{
				List<CardPileAddResult> results = new List<CardPileAddResult>();
				Player owningPlayer = null;
				foreach (CardModel cardModel in cards)
				{
					if (cardModel.Owner == null)
					{
						throw new InvalidOperationException(cardModel.Id.Entry + " has no owner.");
					}
					Creature creature = cardModel.Owner.Creature;
					bool flag = cardModel.HasBeenRemovedFromState || creature.IsDead || (cardModel.IsInCombat && creature.CombatState == null);
					if (flag)
					{
						CardPileAddResult cardPileAddResult = new CardPileAddResult
						{
							success = false,
							cardAdded = cardModel,
							oldPile = cardModel.Pile,
							modifyingModels = null
						};
						results.Add(cardPileAddResult);
					}
					else
					{
						if (newPile.Type == PileType.Deck)
						{
							if (!cardModel.Owner.RunState.ContainsCard(cardModel))
							{
								if (cardModel.Owner.RunState is NullRunState)
								{
									throw new InvalidOperationException("Tried to add card " + cardModel.Id.Entry + " to deck for an owner with a NullRunState!");
								}
								throw new InvalidOperationException(cardModel.Id.Entry + " must be added to a RunState before adding it to your deck.");
							}
						}
						else if (cardModel.IsInCombat && creature.CombatState != null && !creature.CombatState.ContainsCard(cardModel))
						{
							throw new InvalidOperationException(cardModel.Id.Entry + " must be added to a CombatState before adding it to this pile.");
						}
						if (cardModel.UpgradePreviewType.IsPreview())
						{
							throw new InvalidOperationException("A card preview cannot be added to a pile.");
						}
						CardPileAddResult cardPileAddResult2 = new CardPileAddResult
						{
							success = true,
							cardAdded = cardModel,
							oldPile = cardModel.Pile,
							modifyingModels = null
						};
						results.Add(cardPileAddResult2);
						if (owningPlayer == null)
						{
							owningPlayer = cardModel.Owner;
						}
						if (owningPlayer != cardModel.Owner)
						{
							throw new InvalidOperationException("Tried to add cards with different owners to the same pile!");
						}
					}
				}
				bool owningPlayerIsLocal = LocalContext.IsMe(owningPlayer);
				if (newPile.Type == PileType.Deck)
				{
					for (int i = 0; i < results.Count; i++)
					{
						CardPileAddResult result = results[i];
						AbstractModel abstractModel;
						if (Hook.ShouldAddToDeck(owningPlayer.RunState, result.cardAdded, out abstractModel))
						{
							IRunState runState = owningPlayer.RunState;
							MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
							if (currentMapPointHistoryEntry != null)
							{
								currentMapPointHistoryEntry.GetEntry(owningPlayer.NetId).CardsGained.Add(result.cardAdded.ToSerializable());
							}
							result.cardAdded.FloorAddedToDeck = new int?(runState.TotalFloor);
						}
						else
						{
							await abstractModel.AfterAddToDeckPrevented(result.cardAdded);
							result.success = false;
							results[i] = result;
						}
						result = default(CardPileAddResult);
					}
				}
				if (newPile.IsCombatPile && !CombatManager.Instance.IsInProgress)
				{
					readOnlyList = results;
				}
				else if (!results.Any<CardPileAddResult>((CardPileAddResult r) => r.success))
				{
					readOnlyList = results;
				}
				else
				{
					List<NCard> cardNodes = new List<NCard>();
					List<CardModel> cardsWithoutNodesChangingPiles = new List<CardModel>();
					for (int i = 0; i < results.Count; i++)
					{
						CardPileAddResult cardPileAddResult3 = results[i];
						if (cardPileAddResult3.success)
						{
							NCard cardNode = null;
							CardPile oldPile4 = cardPileAddResult3.oldPile;
							CardModel card2 = cardPileAddResult3.cardAdded;
							CardPile targetPile = newPile;
							bool isFullHandAdd = targetPile.Type == PileType.Hand && targetPile.Cards.Count >= CardPile.MaxCardsInHand;
							if (isFullHandAdd)
							{
								targetPile = CardPile.Get(PileType.Discard, card2.Owner);
							}
							bool flag2;
							if (!owningPlayerIsLocal && targetPile.Type != PileType.Play)
							{
								CardPile cardPile = oldPile4;
								flag2 = cardPile != null && cardPile.Type == PileType.Play;
							}
							else
							{
								flag2 = true;
							}
							bool flag3 = flag2;
							if (TestMode.IsOff && flag3 && !skipVisuals)
							{
								cardNode = NCard.FindOnTable(card2, null);
								bool flag4 = cardNode == null && targetPile.Type.IsCombatPile() && (isFullHandAdd || oldPile4 != null || targetPile.Type == PileType.Hand);
								bool flag5 = cardNode == null;
								if (flag5)
								{
									if (oldPile4 == null)
									{
										goto IL_0646;
									}
									bool flag6;
									switch (oldPile4.Type)
									{
									case PileType.Draw:
									case PileType.Discard:
									case PileType.Exhaust:
									case PileType.Deck:
										flag6 = true;
										break;
									case PileType.Hand:
									case PileType.Play:
										goto IL_0646;
									default:
										goto IL_0646;
									}
									IL_0649:
									flag5 = flag6;
									goto IL_064D;
									IL_0646:
									flag6 = false;
									goto IL_0649;
								}
								IL_064D:
								bool flag7 = flag5;
								if (flag7)
								{
									PileType pileType = targetPile.Type;
									flag7 = pileType == PileType.Draw || pileType == PileType.Discard || pileType == PileType.Deck;
								}
								if (flag7)
								{
									cardsWithoutNodesChangingPiles.Add(card2);
								}
								else if (flag4)
								{
									cardNode = CardPileCmd.CreateCardNodeAndUpdateVisuals(card2, targetPile.Type, owningPlayerIsLocal);
								}
								if (cardNode != null)
								{
									cardNodes.Add(cardNode);
								}
							}
							CardModel cardModel2 = card2;
							if (oldPile4 != null)
							{
								card2.RemoveFromCurrentPile(skipVisuals);
							}
							else if (targetPile.Type == PileType.Deck)
							{
								List<AbstractModel> list;
								CardModel cardModel3 = (cardModel2 = Hook.ModifyCardBeingAddedToDeck(card2.Owner.RunState, card2, out list));
								if (list != null && list.Count > 0)
								{
									cardPileAddResult3.cardAdded = cardModel3;
									cardPileAddResult3.modifyingModels = list;
									results[i] = cardPileAddResult3;
								}
							}
							int num;
							switch (position)
							{
							case CardPilePosition.Bottom:
								num = -1;
								break;
							case CardPilePosition.Top:
								num = 0;
								break;
							case CardPilePosition.Random:
								num = card2.Owner.RunState.Rng.Shuffle.NextInt(targetPile.Cards.Count + 1);
								break;
							default:
								throw new ArgumentOutOfRangeException("position", position, null);
							}
							targetPile.AddInternal(cardModel2, num, false);
							if (oldPile4 == null && targetPile.IsCombatPile && !isChangingOwners)
							{
								await Hook.AfterCardEnteredCombat(card2.CombatState, card2);
							}
							if (isFullHandAdd && owningPlayerIsLocal)
							{
								ThinkCmd.Play(new LocString("combat_messages", "HAND_FULL"), owningPlayer.Creature, 2.0);
							}
							CardPile cardPile2 = oldPile4;
							if (cardPile2 == null || cardPile2.Type != PileType.Play || newPile.Type == PileType.Hand || card2.IsDupe)
							{
								NCard ncard = cardNode;
								if (ncard != null)
								{
									ncard.UpdateVisuals(targetPile.Type, CardPreviewMode.Normal);
								}
							}
							cardNode = null;
							oldPile4 = null;
							card2 = null;
							targetPile = null;
						}
					}
					Tween tween = null;
					if (cardNodes.Count != 0)
					{
						CardPileCmd.<>c__DisplayClass10_0 CS$<>8__locals1 = new CardPileCmd.<>c__DisplayClass10_0();
						NCombatRoom instance = NCombatRoom.Instance;
						CS$<>8__locals1.handNode = ((instance != null) ? instance.Ui.Hand : null);
						NCombatRoom instance2 = NCombatRoom.Instance;
						tween = ((instance2 != null) ? instance2.CreateTween().SetParallel(true) : null);
						using (List<NCard>.Enumerator enumerator2 = cardNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								CardPileCmd.<>c__DisplayClass10_1 CS$<>8__locals2 = new CardPileCmd.<>c__DisplayClass10_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals2.cardNode = enumerator2.Current;
								CardModel card = CS$<>8__locals2.cardNode.Model;
								CardPile oldPile2 = results.Find((CardPileAddResult r) => r.cardAdded == card).oldPile;
								CardPileCmd.MoveCardNodeToNewPileBeforeTween(CS$<>8__locals2.cardNode, card.Pile.Type);
								bool flag8 = !owningPlayerIsLocal;
								if (flag8)
								{
									PileType pileType = card.Pile.Type;
									flag8 = pileType - PileType.Draw <= 2 || pileType == PileType.Deck;
								}
								if (flag8)
								{
									if (tween != null)
									{
										tween.Parallel().TweenProperty(CS$<>8__locals2.cardNode, "position", CS$<>8__locals2.cardNode.Position + Vector2.Down * 25f, (double)((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.2f : 0.3f));
									}
									if (tween != null)
									{
										tween.Parallel().TweenProperty(CS$<>8__locals2.cardNode, "modulate", StsColors.exhaustGray, (double)((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.2f : 0.3f));
									}
									if (tween != null)
									{
										tween.Chain().TweenCallback(Callable.From(new Action(CS$<>8__locals2.cardNode.QueueFreeSafely)));
									}
								}
								else
								{
									switch (card.Pile.Type)
									{
									case PileType.Hand:
										CardPileCmd.AppendPileLerpTween(tween, CS$<>8__locals2.cardNode, card.Pile.Type, oldPile2);
										if (tween != null)
										{
											tween.Parallel().TweenCallback(Callable.From(delegate
											{
												NPlayerHand handNode = CS$<>8__locals2.CS$<>8__locals1.handNode;
												if (handNode == null)
												{
													return;
												}
												handNode.Add(CS$<>8__locals2.cardNode, -1);
											}));
											continue;
										}
										continue;
									case PileType.Exhaust:
										card.Pile.InvokeCardAddFinished();
										if (oldPile2 != null && oldPile2.Type != PileType.Hand && oldPile2.Type != PileType.Play)
										{
											CardPileCmd.AppendPileLerpTween(tween, CS$<>8__locals2.cardNode, PileType.Play, oldPile2);
											FastModeType fastMode = SaveManager.Instance.PrefsSave.FastMode;
											float num2 = ((fastMode == FastModeType.Fast) ? 0.2f : ((fastMode != FastModeType.Instant) ? 0.5f : 0.01f));
											if (tween != null)
											{
												tween.Chain().TweenInterval((double)num2);
											}
										}
										if (oldPile2 != null && oldPile2.Type == PileType.Hand)
										{
											if (tween != null)
											{
												tween.Chain().TweenCallback(Callable.From(delegate
												{
													NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(CS$<>8__locals2.cardNode);
													if (ncardExhaustQuickVfx != null)
													{
														NDebugAudioManager instance3 = NDebugAudioManager.Instance;
														if (instance3 != null)
														{
															instance3.Play("card_exhaust.mp3", 1f, PitchVariance.None);
														}
														TaskHelper.RunSafely(ncardExhaustQuickVfx.PlayAnimation());
														return;
													}
													CS$<>8__locals2.cardNode.QueueFreeSafely();
												}));
												continue;
											}
											continue;
										}
										else
										{
											if (tween != null)
											{
												tween.Chain().TweenCallback(Callable.From(delegate
												{
													NCombatRoom instance4 = NCombatRoom.Instance;
													NCardExhaustVfx ncardExhaustVfx = ((instance4 != null) ? NCardExhaustVfx.Create(CS$<>8__locals2.cardNode) : null);
													if (ncardExhaustVfx != null)
													{
														instance4.Ui.AddChildSafely(ncardExhaustVfx);
														NDebugAudioManager instance5 = NDebugAudioManager.Instance;
														if (instance5 != null)
														{
															instance5.Play("card_exhaust.mp3", 1f, PitchVariance.None);
														}
														TaskHelper.RunSafely(ncardExhaustVfx.PlayAnimation());
														return;
													}
													CS$<>8__locals2.cardNode.QueueFreeSafely();
												}));
												continue;
											}
											continue;
										}
										break;
									case PileType.Play:
										CardPileCmd.AppendPlayPileLerpTween(tween, CS$<>8__locals2.cardNode, oldPile2);
										continue;
									}
									if (tween != null)
									{
										tween.TweenCallback(Callable.From(delegate
										{
											Node node = ((card.Pile.Type != PileType.Deck) ? card.Owner.Creature.GetVfxContainer() : NRun.Instance.GlobalUi.TopBar.TrailContainer);
											CS$<>8__locals2.cardNode.Reparent(node, true);
											NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(CS$<>8__locals2.cardNode, card.Pile.Type, true, card.Owner.Character.TrailPath);
											if (node != null)
											{
												node.AddChildSafely(ncardFlyVfx);
											}
										}));
									}
								}
							}
						}
					}
					if (cardsWithoutNodesChangingPiles.Count != 0)
					{
						using (List<CardModel>.Enumerator enumerator3 = cardsWithoutNodesChangingPiles.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								CardPileCmd.<>c__DisplayClass10_3 CS$<>8__locals4 = new CardPileCmd.<>c__DisplayClass10_3();
								CS$<>8__locals4.card = enumerator3.Current;
								CardPile oldPile = results.Find((CardPileAddResult r) => r.cardAdded == CS$<>8__locals4.card).oldPile;
								Node vfxContainer = ((CS$<>8__locals4.card.Pile.Type != PileType.Deck) ? CS$<>8__locals4.card.Owner.Creature.GetVfxContainer() : NRun.Instance.GlobalUi.TopBar.TrailContainer);
								if (tween != null)
								{
									tween.TweenCallback(Callable.From(delegate
									{
										NCardFlyShuffleVfx ncardFlyShuffleVfx2 = NCardFlyShuffleVfx.Create(oldPile, CS$<>8__locals4.card.Pile, CS$<>8__locals4.card.Owner.Character.TrailPath);
										Node vfxContainer2 = vfxContainer;
										if (vfxContainer2 == null)
										{
											return;
										}
										vfxContainer2.AddChildSafely(ncardFlyShuffleVfx2);
									}));
								}
								else
								{
									NCardFlyShuffleVfx ncardFlyShuffleVfx = NCardFlyShuffleVfx.Create(oldPile, CS$<>8__locals4.card.Pile, CS$<>8__locals4.card.Owner.Character.TrailPath);
									Node vfxContainer3 = vfxContainer;
									if (vfxContainer3 != null)
									{
										vfxContainer3.AddChildSafely(ncardFlyShuffleVfx);
									}
								}
							}
						}
					}
					if (tween != null)
					{
						tween.Play();
						TaskAwaiter<bool> taskAwaiter = tween.AwaitFinished(NCombatRoom.Instance).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							return results;
						}
					}
					foreach (CardPileAddResult cardPileAddResult4 in results)
					{
						if (cardPileAddResult4.success)
						{
							CardModel cardAdded = cardPileAddResult4.cardAdded;
							IRunState runState2 = cardAdded.Owner.RunState;
							ICombatState combatState = cardAdded.CombatState;
							CardPile oldPile3 = cardPileAddResult4.oldPile;
							await Hook.AfterCardChangedPiles(runState2, combatState, cardAdded, (oldPile3 != null) ? oldPile3.Type : PileType.None, clonedBy);
						}
					}
					List<CardPileAddResult>.Enumerator enumerator4 = default(List<CardPileAddResult>.Enumerator);
					readOnlyList = results;
				}
			}
			return readOnlyList;
		}

		/// <summary>
		/// Add a card to the play pile during manual card play.
		/// This is a highly simplified version of <see cref="M:MegaCrit.Sts2.Core.Commands.CardPileCmd.Add(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPile,MegaCrit.Sts2.Core.Entities.Cards.CardPilePosition,MegaCrit.Sts2.Core.Models.AbstractModel,System.Boolean)" />
		/// that makes a bunch of assumptions, and also that doesn't wait on its tweens. This makes most card plays feel more responsive.
		/// Autoplay from effects like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Havoc" /> don't use this, because they need to wait on their tweens.
		/// </summary>
		// Token: 0x060087EE RID: 34798 RVA: 0x002DC738 File Offset: 0x002DA938
		public static async Task AddDuringManualCardPlay(CardModel card)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				ICombatState combatState = card.Owner.Creature.CombatState;
				if (combatState == null || !combatState.ContainsCard(card))
				{
					throw new InvalidOperationException(card.Id.Entry + " must be added to a CombatState before playing it.");
				}
				bool flag = LocalContext.IsMe(card.Owner);
				CardPile oldPile = card.Pile;
				NCard ncard = null;
				if (TestMode.IsOff)
				{
					ncard = NCard.FindOnTable(card, null);
					if (ncard == null)
					{
						ncard = CardPileCmd.CreateCardNodeAndUpdateVisuals(card, PileType.Play, flag);
					}
				}
				card.RemoveFromCurrentPile(false);
				PileType.Play.GetPile(card.Owner).AddInternal(card, -1, false);
				if (ncard != null)
				{
					CardPileCmd.MoveCardNodeToNewPileBeforeTween(ncard, PileType.Play);
					Tween tween = NCombatRoom.Instance.CreateTween().SetParallel(true);
					CardPileCmd.AppendPlayPileLerpTween(tween, ncard, oldPile);
					ncard.PlayPileTween = tween;
					tween.Play();
					if (card.Type == CardType.Power)
					{
						TaskAwaiter<bool> taskAwaiter = tween.AwaitFinished(NCombatRoom.Instance).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							return;
						}
					}
				}
				IRunState runState = card.Owner.RunState;
				ICombatState combatState2 = card.CombatState;
				CardPile cardPile = oldPile;
				await Hook.AfterCardChangedPiles(runState, combatState2, card, (cardPile != null) ? cardPile.Type : PileType.None, null);
			}
		}

		// Token: 0x060087EF RID: 34799 RVA: 0x002DC77C File Offset: 0x002DA97C
		private static NCard CreateCardNodeAndUpdateVisuals(CardModel card, PileType targetPileType, bool owningPlayerIsLocal)
		{
			NCard ncard = NCard.Create(card, ModelVisibility.Visible);
			NCombatRoom.Instance.Ui.AddChildSafely(ncard);
			ncard.UpdateVisuals(targetPileType, CardPreviewMode.Normal);
			if (!owningPlayerIsLocal)
			{
				ncard.Position = NCombatRoom.Instance.GetCreatureNode(card.Owner.Creature).IntentContainer.GlobalPosition;
			}
			else if (card.Pile != null)
			{
				ncard.Position = card.Pile.Type.GetTargetPosition(ncard);
			}
			else
			{
				ncard.Position = targetPileType.GetTargetPosition(ncard);
			}
			return ncard;
		}

		// Token: 0x060087F0 RID: 34800 RVA: 0x002DC804 File Offset: 0x002DAA04
		private static void MoveCardNodeToNewPileBeforeTween(NCard cardNode, PileType newPileType)
		{
			NPlayerHand hand = NCombatRoom.Instance.Ui.Hand;
			NCardPlayQueue playQueue = NCombatRoom.Instance.Ui.PlayQueue;
			Control playContainer = NCombatRoom.Instance.Ui.PlayContainer;
			Vector2 globalPosition = cardNode.GlobalPosition;
			CardModel model = cardNode.Model;
			if (playQueue.IsAncestorOf(cardNode))
			{
				playQueue.RemoveCardFromQueueForExecution(model);
			}
			if (hand.IsAncestorOf(cardNode))
			{
				hand.Remove(model);
			}
			else
			{
				Node parent = cardNode.GetParent();
				if (parent != null)
				{
					parent.RemoveChildSafely(cardNode);
				}
			}
			if (newPileType == PileType.Play)
			{
				playContainer.AddChildSafely(cardNode);
				if (NCombatUi.IsDebugHidingPlayContainer)
				{
					cardNode.Visible = false;
				}
			}
			else
			{
				NCombatRoom.Instance.Ui.AddChildSafely(cardNode);
			}
			cardNode.GlobalPosition = globalPosition;
			Tween playPileTween = cardNode.PlayPileTween;
			if (playPileTween == null)
			{
				return;
			}
			playPileTween.FastForwardToCompletion();
		}

		// Token: 0x060087F1 RID: 34801 RVA: 0x002DC8C8 File Offset: 0x002DAAC8
		[NullableContext(2)]
		private static void AppendPlayPileLerpTween(Tween tween, [Nullable(1)] NCard cardNode, CardPile oldPile)
		{
			CardPileCmd.AppendPileLerpTween(tween, cardNode, cardNode.Model.Pile.Type, oldPile);
			if (tween != null)
			{
				tween.Parallel().TweenCallback(Callable.From(delegate
				{
					NCombatRoom.Instance.Ui.AddToPlayContainer(cardNode);
				}));
			}
		}

		// Token: 0x060087F2 RID: 34802 RVA: 0x002DC924 File Offset: 0x002DAB24
		[NullableContext(2)]
		private static void AppendPileLerpTween(Tween tween, [Nullable(1)] NCard cardNode, PileType typePile, CardPile oldPile)
		{
			if (tween == null)
			{
				return;
			}
			Vector2 targetPosition = typePile.GetTargetPosition(cardNode);
			FastModeType fastMode = SaveManager.Instance.PrefsSave.FastMode;
			float num;
			if (fastMode != FastModeType.Fast)
			{
				if (fastMode == FastModeType.Instant)
				{
					num = 0.01f;
				}
				else
				{
					num = 0.25f;
				}
			}
			else
			{
				num = 0.1f;
			}
			float num2 = num;
			if (typePile != PileType.Hand)
			{
				tween.TweenProperty(cardNode, "position", targetPosition, (double)num2).SetEase(1L).SetTrans(7L);
			}
			if (typePile == PileType.Play)
			{
				tween.TweenProperty(cardNode, "scale", Vector2.One * 0.8f, 0.25).SetEase(1L).SetTrans(7L);
				return;
			}
			if (oldPile == null)
			{
				tween.TweenProperty(cardNode, "scale", Vector2.One, (double)num2).SetEase(1L).SetTrans(7L)
					.From(Vector2.Zero);
				return;
			}
			tween.Parallel().TweenProperty(cardNode, "scale", Vector2.One, (double)num2).SetEase(1L)
				.SetTrans(7L);
		}

		/// <summary>
		/// Draw a card.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="player">Player who the hand and draw pile belongs to.</param>
		/// <returns>Card that was drawn, or null if no cards were drawn.</returns>
		// Token: 0x060087F3 RID: 34803 RVA: 0x002DCA4C File Offset: 0x002DAC4C
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<CardModel> Draw(PlayerChoiceContext choiceContext, Player player)
		{
			IEnumerable<CardModel> enumerable = await CardPileCmd.Draw(choiceContext, 1m, player, false);
			return enumerable.FirstOrDefault<CardModel>();
		}

		/// <summary>
		/// Draw cards.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="count">Number of cards to draw.</param>
		/// <param name="player">Player who the hand and draw pile belongs to.</param>
		/// <param name="fromHandDraw">If this draw happened as part of the initial card draws at the start of your turn.</param>
		/// <returns>Cards that were drawn.</returns>
		// Token: 0x060087F4 RID: 34804 RVA: 0x002DCA97 File Offset: 0x002DAC97
		public static Task<IEnumerable<CardModel>> Draw(PlayerChoiceContext choiceContext, decimal count, Player player, bool fromHandDraw = false)
		{
			return CardPileCmd.DrawInternal(choiceContext, count, player, fromHandDraw);
		}

		/// <summary>
		/// Draw cards without needing to know the result.
		/// This can be used as a shortcut when the target of the draw may be different than the owner of the action which
		/// caused the draw, and you do not wish the draw to block the originating action.
		/// If you must know the results of the draw, then you need to construct your own DelegatingPlayerChoiceContext and
		/// pass it the full task of the draw + modifications.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="count">Number of cards to draw.</param>
		/// <param name="player">Player who the hand and draw pile belongs to.</param>
		/// <param name="fromHandDraw">If this draw happened as part of the initial card draws at the start of your turn.</param>
		/// <returns>Cards that were drawn.</returns>
		// Token: 0x060087F5 RID: 34805 RVA: 0x002DCAA4 File Offset: 0x002DACA4
		public static Task DrawWithoutBlockingOnOtherPlayers(PlayerChoiceContext choiceContext, decimal count, Player player, bool fromHandDraw = false)
		{
			BranchingPlayerChoiceContext branchingPlayerChoiceContext = new BranchingPlayerChoiceContext(LocalContext.NetId.Value, GameActionType.Combat, choiceContext);
			Task<IEnumerable<CardModel>> task = CardPileCmd.Draw(branchingPlayerChoiceContext, count, player, fromHandDraw);
			return branchingPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
		}

		// Token: 0x060087F6 RID: 34806 RVA: 0x002DCAD8 File Offset: 0x002DACD8
		private static async Task<IEnumerable<CardModel>> DrawInternal(PlayerChoiceContext choiceContext, decimal count, Player player, bool fromHandDraw = false)
		{
			IEnumerable<CardModel> enumerable;
			AbstractModel abstractModel;
			if (CombatManager.Instance.IsOverOrEnding)
			{
				enumerable = Array.Empty<CardModel>();
			}
			else if (!Hook.ShouldDraw(player.Creature.CombatState, player, fromHandDraw, out abstractModel))
			{
				await Hook.AfterPreventingDraw(player.Creature.CombatState, abstractModel);
				enumerable = Array.Empty<CardModel>();
			}
			else
			{
				ICombatState combatState = player.Creature.CombatState;
				List<CardModel> result = new List<CardModel>();
				CardPile hand = PileType.Hand.GetPile(player);
				CardPile drawPile = PileType.Draw.GetPile(player);
				int drawsRequested = ((count > 0m) ? ((int)Math.Ceiling(count)) : 0);
				if (drawsRequested == 0)
				{
					enumerable = result;
				}
				else
				{
					int num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
					if (num == 0)
					{
						CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player);
						enumerable = result;
					}
					else
					{
						int i = 0;
						while (i < drawsRequested && num > 0 && !CombatManager.Instance.IsOverOrEnding && CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player))
						{
							await CardPileCmd.ShuffleIfNecessary(choiceContext, player);
							if (!CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player))
							{
								break;
							}
							CardModel card = drawPile.Cards.FirstOrDefault<CardModel>();
							if (card == null || hand.Cards.Count >= CardPile.MaxCardsInHand)
							{
								break;
							}
							result.Add(card);
							await CardPileCmd.Add(card, hand, CardPilePosition.Bottom, null, false);
							CombatManager.Instance.History.CardDrawn(combatState, card, fromHandDraw);
							await Hook.AfterCardDrawn(combatState, choiceContext, card, fromHandDraw);
							card.InvokeDrawn();
							NDebugAudioManager instance = NDebugAudioManager.Instance;
							if (instance != null)
							{
								instance.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
							}
							num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
							card = null;
							i++;
						}
						enumerable = result;
					}
				}
			}
			return enumerable;
		}

		/// <summary>
		/// Shuffle the player's discard pile into their draw pile.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose piles we should shuffle.</param>
		// Token: 0x060087F7 RID: 34807 RVA: 0x002DCB34 File Offset: 0x002DAD34
		public static async Task Shuffle(PlayerChoiceContext choiceContext, Player player)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				CardPile drawPile = PileType.Draw.GetPile(player);
				List<CardModel> list = PileType.Discard.GetPile(player).Cards.ToList<CardModel>();
				float timeBetweenCardAdds = Mathf.Min(0.045f, 0.8f / (float)list.Count);
				float randomTimeBetweenCardAdds = 1.11f * timeBetweenCardAdds;
				HashSet<CardModel> drawPileCards = drawPile.Cards.ToHashSet<CardModel>();
				list.AddRange(drawPileCards);
				list.StableShuffle<CardModel>(player.RunState.Rng.Shuffle);
				Hook.ModifyShuffleOrder(player.Creature.CombatState, player, list, false);
				foreach (CardModel cardModel in drawPileCards)
				{
					drawPile.RemoveInternal(cardModel, true);
				}
				if (CombatManager.Instance.DebugForcedTopCardOnNextShuffle != null)
				{
					if (!list.Remove(CombatManager.Instance.DebugForcedTopCardOnNextShuffle))
					{
						throw new InvalidOperationException("Could not find card " + CombatManager.Instance.DebugForcedTopCardOnNextShuffle.Id.Entry + " in discard pile.");
					}
					list.Insert(0, CombatManager.Instance.DebugForcedTopCardOnNextShuffle);
					CombatManager.Instance.DebugClearForcedTopCardOnNextShuffle();
				}
				float waitTimeAccumulator = 0f;
				foreach (CardModel cardModel2 in list)
				{
					if (!drawPileCards.Contains(cardModel2))
					{
						await CardPileCmd.Add(cardModel2, drawPile, CardPilePosition.Bottom, null, false);
						if (CombatManager.Instance.IsOverOrEnding)
						{
							return;
						}
						float num = timeBetweenCardAdds + Rng.Chaotic.NextFloat(-randomTimeBetweenCardAdds * 0.5f, randomTimeBetweenCardAdds * 0.5f);
						waitTimeAccumulator += num;
						if ((double)waitTimeAccumulator >= ((SceneTree)Engine.GetMainLoop()).Root.GetProcessDeltaTime())
						{
							await Cmd.Wait(num, false);
							waitTimeAccumulator = 0f;
						}
					}
					else
					{
						drawPile.AddInternal(cardModel2, -1, true);
					}
				}
				List<CardModel>.Enumerator enumerator2 = default(List<CardModel>.Enumerator);
				await Cmd.CustomScaledWait(0.2f, 0.5f, false, default(CancellationToken));
				if (!CombatManager.Instance.IsOverOrEnding)
				{
					await Hook.AfterShuffle(player.Creature.CombatState, choiceContext, player);
				}
			}
		}

		/// <summary>
		/// Play cards directly from the draw pile.
		/// If the draw pile becomes empty before the specified number of cards are played, the discard pile will
		/// automatically be shuffled into it.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="player">Player whose draw pile we should play from.</param>
		/// <param name="count">Number of cards to play.</param>
		/// <param name="position">Position to play the cards from.</param>
		/// <param name="forceExhaust">Whether or not to force the played cards to be exhausted after.</param>
		// Token: 0x060087F8 RID: 34808 RVA: 0x002DCB80 File Offset: 0x002DAD80
		public static async Task AutoPlayFromDrawPile(PlayerChoiceContext choiceContext, Player player, int count, CardPilePosition position, bool forceExhaust)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				List<CardModel> cards = new List<CardModel>(count);
				CardPile drawPile = PileType.Draw.GetPile(player);
				for (int i = 0; i < count; i++)
				{
					await CardPileCmd.ShuffleIfNecessary(choiceContext, player);
					CardModel cardModel;
					switch (position)
					{
					case CardPilePosition.Bottom:
						cardModel = drawPile.Cards.LastOrDefault<CardModel>();
						break;
					case CardPilePosition.Top:
						cardModel = drawPile.Cards.FirstOrDefault<CardModel>();
						break;
					case CardPilePosition.Random:
						cardModel = player.RunState.Rng.CombatCardSelection.NextItem<CardModel>(drawPile.Cards);
						break;
					default:
						throw new ArgumentOutOfRangeException("position", position, null);
					}
					CardModel cardModel2 = cardModel;
					if (cardModel2 == null)
					{
						break;
					}
					cards.Add(cardModel2);
					await CardPileCmd.Add(cardModel2, PileType.Play, CardPilePosition.Bottom, null, false);
				}
				foreach (CardModel cardModel3 in cards)
				{
					if (cardModel3.Owner.Creature.IsDead)
					{
						break;
					}
					cardModel3.ExhaustOnNextPlay = forceExhaust;
					await CardCmd.AutoPlay(choiceContext, cardModel3, null, AutoPlayType.Default, false, false);
				}
				List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			}
		}

		/// <summary>
		/// Shuffle the specified player's discard pile into their draw pile IF their draw pile is currently empty.
		/// If their draw pile has at least one card in it OR their discard pile is empty, this method will do nothing.
		/// </summary>
		// Token: 0x060087F9 RID: 34809 RVA: 0x002DCBE4 File Offset: 0x002DADE4
		public static async Task ShuffleIfNecessary(PlayerChoiceContext choiceContext, Player player)
		{
			CardPile pile = PileType.Draw.GetPile(player);
			CardPile pile2 = PileType.Discard.GetPile(player);
			if (!pile.Cards.Any<CardModel>() && pile2.Cards.Any<CardModel>())
			{
				await CardPileCmd.ShuffleFtueCheck();
				await CardPileCmd.Shuffle(choiceContext, player);
			}
		}

		/// <summary>
		/// The first time the discard pile is shuffled into the draw pile, the Shuffle FTUE shows up
		/// </summary>
		// Token: 0x060087FA RID: 34810 RVA: 0x002DCC30 File Offset: 0x002DAE30
		private static async Task ShuffleFtueCheck()
		{
			if (!SaveManager.Instance.SeenFtue("shuffle_ftue"))
			{
				if (NModalContainer.Instance != null)
				{
					NShuffleFtue nshuffleFtue = NShuffleFtue.Create();
					NModalContainer.Instance.Add(nshuffleFtue, true);
					SaveManager.Instance.MarkFtueAsComplete("shuffle_ftue");
					await nshuffleFtue.WaitForPlayerToConfirm();
				}
			}
		}

		// Token: 0x060087FB RID: 34811 RVA: 0x002DCC6C File Offset: 0x002DAE6C
		public static async Task AddToCombatAndPreview<[Nullable(0)] T>(IEnumerable<Creature> targets, PileType pileType, int count, [Nullable(2)] Player creator, CardPilePosition position = CardPilePosition.Bottom) where T : CardModel
		{
			foreach (Creature creature in targets)
			{
				await CardPileCmd.AddToCombatAndPreview<T>(creature, pileType, count, creator, position);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060087FC RID: 34812 RVA: 0x002DCCD0 File Offset: 0x002DAED0
		public static async Task AddToCombatAndPreview<[Nullable(0)] T>(Creature target, PileType pileType, int count, [Nullable(2)] Player creator, CardPilePosition position = CardPilePosition.Bottom) where T : CardModel
		{
			Player player = target.Player ?? target.PetOwner;
			if (!player.Creature.IsDead)
			{
				CardPileAddResult[] statusCards = new CardPileAddResult[count];
				for (int i = 0; i < count; i++)
				{
					ICombatState combatState = target.CombatState;
					CardModel cardModel = ((combatState != null) ? combatState.CreateCard<T>(player) : default(T));
					if (cardModel != null)
					{
						CardPileAddResult[] array = statusCards;
						int num = i;
						CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, pileType, creator, position);
						array[num] = cardPileAddResult;
						array = null;
					}
				}
				if (LocalContext.IsMe(player))
				{
					if (pileType == PileType.Hand)
					{
						await Cmd.Wait(0.1f, false);
					}
					else
					{
						CardCmd.PreviewCardPileAdd(statusCards, 1.2f, (statusCards.Length > 5) ? CardPreviewStyle.MessyLayout : CardPreviewStyle.HorizontalLayout);
						await Cmd.Wait(1f, false);
					}
				}
			}
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x002DCD34 File Offset: 0x002DAF34
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<CardModel> AddCurseToDeck<[Nullable(0)] T>(Player owner) where T : CardModel
		{
			IEnumerable<CardPileAddResult> enumerable = await CardPileCmd.AddCursesToDeck(new <>z__ReadOnlySingleElementList<CardModel>(ModelDb.Card<T>()), owner);
			IEnumerable<CardPileAddResult> enumerable2 = enumerable;
			return enumerable2.FirstOrDefault<CardPileAddResult>().cardAdded;
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x002DCD78 File Offset: 0x002DAF78
		public static async Task<IEnumerable<CardPileAddResult>> AddCursesToDeck(IEnumerable<CardModel> curses, Player owner)
		{
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			foreach (CardModel cardModel in curses)
			{
				if (cardModel.Type != CardType.Curse)
				{
					throw new ArgumentException(cardModel.Id.Entry + " is not a curse");
				}
				CardModel cardModel2 = owner.RunState.CreateCard(cardModel, owner);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel2, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				results.Add(cardPileAddResult2);
			}
			IEnumerator<CardModel> enumerator = null;
			CardCmd.PreviewCardPileAdd(results, 2f, CardPreviewStyle.HorizontalLayout);
			return results;
		}

		// Token: 0x060087FF RID: 34815 RVA: 0x002DCDC4 File Offset: 0x002DAFC4
		private static bool CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(Player player)
		{
			if (PileType.Draw.GetPile(player).Cards.Count + PileType.Discard.GetPile(player).Cards.Count == 0)
			{
				ThinkCmd.Play(new LocString("combat_messages", "NO_DRAW"), player.Creature, 2.0);
				return false;
			}
			if (PileType.Hand.GetPile(player).Cards.Count >= CardPile.MaxCardsInHand)
			{
				ThinkCmd.Play(new LocString("combat_messages", "HAND_FULL"), player.Creature, 2.0);
				return false;
			}
			return true;
		}
	}
}
