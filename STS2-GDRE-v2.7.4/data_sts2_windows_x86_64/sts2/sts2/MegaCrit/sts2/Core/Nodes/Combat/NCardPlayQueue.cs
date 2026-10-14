using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Takes control of card nodes that are about to be played and arranges them in the order of play.
	/// </summary>
	// Token: 0x020003F2 RID: 1010
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCardPlayQueue.cs")]
	public class NCardPlayQueue : Control
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x001DADFF File Offset: 0x001D8FFF
		[Nullable(2)]
		public static NCardPlayQueue Instance
		{
			[NullableContext(2)]
			get
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.Ui.PlayQueue;
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x001DAE16 File Offset: 0x001D9016
		public override void _Ready()
		{
			RunManager.Instance.ActionQueueSet.ActionEnqueued += this.OnActionEnqueued;
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x001DAE33 File Offset: 0x001D9033
		public override void _ExitTree()
		{
			RunManager.Instance.ActionQueueSet.ActionEnqueued -= this.OnActionEnqueued;
			this._playQueue.Clear();
		}

		/// <summary>
		/// Called after a card is played by the local player.
		/// If the card is not yet in the play pile, it tweens the card into its queue position.
		/// </summary>
		/// <param name="action">The play card action that caused the card to be played.</param>
		/// <param name="holder">The card holder that was played.</param>
		/// <param name="card">The card that was played.</param>
		// Token: 0x06003DFF RID: 15871 RVA: 0x001DAE5C File Offset: 0x001D905C
		public void OnLocalCardPlayed(PlayCardAction action, [Nullable(2)] NCardHolder holder, CardModel card)
		{
			NCard ncard = ((holder != null) ? holder.CardNode : null) ?? NCard.Create(card, ModelVisibility.Visible);
			CardModel model = ncard.Model;
			bool flag;
			if (model == null)
			{
				flag = true;
			}
			else
			{
				CardPile pile = model.Pile;
				flag = ((pile != null) ? new PileType?(pile.Type) : null).GetValueOrDefault() != PileType.Hand;
			}
			if (flag)
			{
				return;
			}
			NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem
			{
				card = ncard,
				action = action
			};
			if (ncard.IsInsideTree())
			{
				ncard.Reparent(this, true);
			}
			else
			{
				this.AddChildSafely(ncard);
			}
			this.MoveChildSafely(ncard, 0);
			if (holder != null && holder.IsValid())
			{
				NPlayerHand.Instance.RemoveCardHolder(holder);
			}
			this._playQueue.Add(queueItem);
			this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
		}

		/// <summary>
		/// Called when any action is enqueued onto the action queue.
		/// Handles cases in which a card is played by a remote player and we want to display the card in the queue.
		/// </summary>
		/// <param name="action">The action that was enqueued.</param>
		// Token: 0x06003E00 RID: 15872 RVA: 0x001DAF28 File Offset: 0x001D9128
		private void OnActionEnqueued(GameAction action)
		{
			PlayCardAction playCardAction = action as PlayCardAction;
			if (playCardAction == null)
			{
				return;
			}
			CardModel cardModel = playCardAction.NetCombatCard.ToCardModelOrNull();
			if (cardModel == null)
			{
				try
				{
					cardModel = ModelDb.GetById<CardModel>(playCardAction.CardModelId);
				}
				catch (ModelNotFoundException)
				{
					cardModel = ModelDb.Card<DeprecatedCard>();
				}
			}
			if (LocalContext.IsMe(playCardAction.Player))
			{
				NCardHolder cardHolder = NPlayerHand.Instance.GetCardHolder(cardModel);
				this.OnLocalCardPlayed(playCardAction, cardHolder, cardModel);
				return;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(playCardAction.Player.Creature);
			NMultiplayerPlayerIntentHandler playerIntentHandler = creatureNode.PlayerIntentHandler;
			NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
			Vector2 vector = playerIntentHandler.CardIntent.GlobalPosition + playerIntentHandler.CardIntent.Size * 0.5f;
			ncard.GlobalPosition = vector;
			ncard.Scale = Vector2.One * 0.25f;
			this.AddChildSafely(ncard);
			this.MoveChildSafely(ncard, 0);
			NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem
			{
				card = ncard,
				action = playCardAction
			};
			this._playQueue.Add(queueItem);
			this.UpdateCardVisuals(queueItem);
			this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
		}

		/// <summary>
		/// After remote player choice is complete, this re-adds the card back into the queue if necessary.
		/// Note that this is not called for local player choice - that just sits in the center of the screen.
		/// </summary>
		/// <param name="card">The card that finished player choice.</param>
		/// <param name="action">The action that ran the player choice.</param>
		// Token: 0x06003E01 RID: 15873 RVA: 0x001DB05C File Offset: 0x001D925C
		public void ReAddCardAfterPlayerChoice(NCard card, GameAction action)
		{
			if (action.State == GameActionState.Executing)
			{
				card.Reparent(NCombatRoom.Instance.Ui.PlayContainer, true);
				card.AnimCardToPlayPile();
				return;
			}
			NCardPlayQueue.QueueItem queueItem = new NCardPlayQueue.QueueItem
			{
				card = card,
				action = action
			};
			card.Reparent(this, true);
			this.MoveChildSafely(card, 0);
			this._playQueue.Add(queueItem);
			this.TweenCardToQueuePosition(queueItem, this._playQueue.Count - 1);
			action.BeforeResumedAfterPlayerChoice += this.BeforeRemoteCardPlayResumedAfterPlayerChoice;
		}

		/// <summary>
		/// This is a bit of a hack. When a card resumes execution after player choice, there's nothing to tell the card
		/// that it needs to animate back to the play pile. We do it here, even though we're not really responsible for
		/// the play pile.
		/// </summary>
		/// <param name="action">Action that finished player choice.</param>
		// Token: 0x06003E02 RID: 15874 RVA: 0x001DB0E8 File Offset: 0x001D92E8
		private void BeforeRemoteCardPlayResumedAfterPlayerChoice(GameAction action)
		{
			action.BeforeResumedAfterPlayerChoice -= this.BeforeRemoteCardPlayResumedAfterPlayerChoice;
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.action == action);
			if (num < 0)
			{
				return;
			}
			NCardPlayQueue.QueueItem queueItem = this._playQueue[num];
			this.RemoveCardFromQueue(num);
			queueItem.card.Reparent(NCombatRoom.Instance.Ui.PlayContainer, true);
			queueItem.card.AnimCardToPlayPile();
		}

		/// <summary>
		/// Called when a card play is canceled.
		/// When a card play is canceled (e.g. because it's now targeting something invalid), this removes the card from the
		/// queue. If the card was a local card, then it is returned to the hand.
		/// </summary>
		/// <param name="action">The play card action that was canceled.</param>
		// Token: 0x06003E03 RID: 15875 RVA: 0x001DB170 File Offset: 0x001D9370
		public void RemoveCardFromQueueForCancellation(PlayCardAction action)
		{
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.action == action);
			if (num < 0)
			{
				return;
			}
			this.RemoveCardFromQueueForCancellation(num, false);
		}

		/// <summary>
		/// Called when a card play is canceled.
		/// When a card play is canceled (e.g. because it's now targeting something invalid), this removes the card from the
		/// queue. If the card was a local card, then it is returned to the hand.
		/// </summary>
		/// <param name="card">The card node that was cancelled.</param>
		/// <param name="forceReturnToHand">In rare circumstances, we want to force the card to return to the hand even
		/// though it's not in the hand pile. If true is passed, the card holder will be added to the player hand instead of
		/// fading away (e.g. as if autoplayed). Only use this if you know what you're doing.</param>
		// Token: 0x06003E04 RID: 15876 RVA: 0x001DB1B0 File Offset: 0x001D93B0
		public void RemoveCardFromQueueForCancellation(NCard card, bool forceReturnToHand = false)
		{
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.card == card);
			if (num < 0)
			{
				return;
			}
			this.RemoveCardFromQueueForCancellation(num, forceReturnToHand);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x001DB1F0 File Offset: 0x001D93F0
		private void RemoveCardFromQueueForCancellation(int index, bool forceReturnToHand = false)
		{
			NCardPlayQueue.QueueItem queueItem = this._playQueue[index];
			this.RemoveCardFromQueue(index);
			ulong ownerId = queueItem.action.OwnerId;
			ulong? netId = LocalContext.NetId;
			if (!((ownerId == netId.GetValueOrDefault()) & (netId != null)))
			{
				this.TweenCardForCancellation(queueItem);
				return;
			}
			CardModel model = queueItem.card.Model;
			bool flag;
			if (model == null)
			{
				flag = false;
			}
			else
			{
				CardPile pile = model.Pile;
				flag = ((pile != null) ? new PileType?(pile.Type) : null).GetValueOrDefault() == PileType.Hand;
			}
			if (flag || forceReturnToHand)
			{
				NPlayerHand.Instance.Add(queueItem.card, -1);
				return;
			}
			this.TweenCardForCancellation(queueItem);
		}

		/// <summary>
		/// Called just before a card begins execution.
		/// For the play pile to properly take control of cards that are in the queue, the queue must reference the exact
		/// card that is getting played. In most instances, this is true when the action is enqueued. However, if the card was
		/// dynamically created in combat but not yet created on our peer at enqueue time, then it was created with a
		/// placeholder card. By this time, it should be available.
		/// </summary>
		/// <param name="playCardAction">The action that is about to be executed.</param>
		// Token: 0x06003E06 RID: 15878 RVA: 0x001DB298 File Offset: 0x001D9498
		public void UpdateCardBeforeExecution(PlayCardAction playCardAction)
		{
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.action == playCardAction);
			if (num < 0)
			{
				return;
			}
			NCardPlayQueue.QueueItem queueItem = this._playQueue[num];
			queueItem.card.Model = playCardAction.NetCombatCard.ToCardModel();
			this.UpdateCardVisuals(queueItem);
			if (LocalContext.IsMe(queueItem.card.Model.Owner))
			{
				NPlayerHand instance = NPlayerHand.Instance;
				NCardHolder ncardHolder = ((instance != null) ? instance.GetCardHolder(queueItem.card.Model) : null);
				if (ncardHolder != null)
				{
					NPlayerHand instance2 = NPlayerHand.Instance;
					if (instance2 == null)
					{
						return;
					}
					instance2.Remove(queueItem.card.Model);
				}
			}
		}

		/// <summary>
		/// Called when a card begins execution.
		/// The play pile (specifically CardPileCmd.Add) takes control of the NCard when it begins executing. This method
		/// relinquishes control of the NCard, but does nothing with the node afterward - it is left where it is, and we
		/// expect CardPileCmd.Add to tween it to the play pile position.
		/// </summary>
		/// <param name="card">The card that was canceled.</param>
		// Token: 0x06003E07 RID: 15879 RVA: 0x001DB354 File Offset: 0x001D9554
		public void RemoveCardFromQueueForExecution(CardModel card)
		{
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.card.Model == card);
			if (num < 0)
			{
				throw new InvalidOperationException();
			}
			this.RemoveCardFromQueue(num);
		}

		/// <summary>
		/// Updates a card's visuals (i.e. updates target, updates text).
		/// </summary>
		// Token: 0x06003E08 RID: 15880 RVA: 0x001DB398 File Offset: 0x001D9598
		private void UpdateCardVisuals(NCardPlayQueue.QueueItem item)
		{
			PlayCardAction playCardAction = item.action as PlayCardAction;
			if (playCardAction != null)
			{
				item.card.SetPreviewTarget(playCardAction.Target);
			}
			NCard card = item.card;
			CardPile pile = item.card.Model.Pile;
			card.UpdateVisuals((pile != null) ? pile.Type : PileType.None, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Removes a specific node from the queue and tweens other cards to their new position.
		/// The node is not freed.
		/// </summary>
		// Token: 0x06003E09 RID: 15881 RVA: 0x001DB3F0 File Offset: 0x001D95F0
		private void RemoveCardFromQueue(NCard card)
		{
			int num = this._playQueue.FindIndex((NCardPlayQueue.QueueItem i) => i.card == card);
			if (num < 0)
			{
				return;
			}
			this.RemoveCardFromQueue(num);
		}

		/// <summary>
		/// Removes a specific card index from the queue and tweens other cards to their new position.
		/// The node is not freed.
		/// </summary>
		// Token: 0x06003E0A RID: 15882 RVA: 0x001DB430 File Offset: 0x001D9630
		private void RemoveCardFromQueue(int index)
		{
			NCardPlayQueue.QueueItem queueItem = this._playQueue[index];
			Tween currentTween = queueItem.currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._playQueue.RemoveAt(index);
			this.TweenAllToQueuePosition();
		}

		/// <summary>
		/// Tweens all cards to their position/scale in the queue.
		/// </summary>
		// Token: 0x06003E0B RID: 15883 RVA: 0x001DB470 File Offset: 0x001D9670
		private void TweenAllToQueuePosition()
		{
			for (int i = 0; i < this._playQueue.Count; i++)
			{
				this.TweenCardToQueuePosition(this._playQueue[i], i);
			}
		}

		/// <summary>
		/// Returns the NCard representing the card if it is in the queue.
		/// Remember that:
		///  - Cards that are being dragged are in the NPlayerHand
		///  - Cards that are being executed are in NCombatUi.PlayContainer
		/// This only returns cards that are enqueued, but not yet executing.
		/// </summary>
		/// <param name="card">The card whose node we will return.</param>
		/// <returns>The node for the card, if it is in the play queue.</returns>
		// Token: 0x06003E0C RID: 15884 RVA: 0x001DB4A8 File Offset: 0x001D96A8
		[return: Nullable(2)]
		public NCard GetCardNode(CardModel card)
		{
			NCardPlayQueue.QueueItem queueItem = this._playQueue.FirstOrDefault<NCardPlayQueue.QueueItem>((NCardPlayQueue.QueueItem i) => i.card.Model == card);
			if (queueItem == null)
			{
				return null;
			}
			return queueItem.card;
		}

		/// <summary>
		/// Dequeue and hide all cards that are in the queue.
		/// This is called when combat ends so that cards don't just hang out.
		/// </summary>
		// Token: 0x06003E0D RID: 15885 RVA: 0x001DB4E4 File Offset: 0x001D96E4
		public void AnimOut()
		{
			foreach (NCardPlayQueue.QueueItem queueItem in this._playQueue)
			{
				Tween currentTween = queueItem.currentTween;
				if (currentTween != null)
				{
					currentTween.Kill();
				}
				ulong ownerId = queueItem.action.OwnerId;
				ulong? netId = LocalContext.NetId;
				if ((ownerId == netId.GetValueOrDefault()) & (netId != null))
				{
					CardModel model = queueItem.card.Model;
					bool flag;
					if (model == null)
					{
						flag = false;
					}
					else
					{
						CardPile pile = model.Pile;
						flag = ((pile != null) ? new PileType?(pile.Type) : null).GetValueOrDefault() == PileType.Hand;
					}
					if (flag)
					{
						NPlayerHand.Instance.Add(queueItem.card, -1);
						continue;
					}
				}
				this.TweenCardForCancellation(queueItem);
			}
			this._playQueue.Clear();
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x001DB5D0 File Offset: 0x001D97D0
		private void TweenCardForCancellation(NCardPlayQueue.QueueItem item)
		{
			Tween currentTween = item.currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			item.currentTween = base.CreateTween().SetParallel(true);
			item.currentTween.TweenProperty(item.card, "position:y", 30f, 0.5).AsRelative().SetEase(1L)
				.SetTrans(7L);
			item.currentTween.TweenProperty(item.card, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(7L);
			item.currentTween.Chain().TweenCallback(Callable.From(new Action(item.card.QueueFreeSafely)));
			item.currentTween.Play();
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x001DB6B4 File Offset: 0x001D98B4
		private void TweenCardToQueuePosition(NCardPlayQueue.QueueItem item, int queueIndex)
		{
			Tween currentTween = item.currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			item.currentTween = base.CreateTween().SetParallel(true);
			item.currentTween.TweenProperty(item.card, "position", this.GetPositionForQueueIndex(item.card, queueIndex), 0.3499999940395355).SetEase(1L).SetTrans(7L);
			item.currentTween.TweenProperty(item.card, "scale", this.GetScaleForQueueIndex(queueIndex), 0.3499999940395355).SetEase(1L).SetTrans(7L);
			item.currentTween.TweenProperty(item.card, "modulate:a", 1f, 0.3499999940395355).SetEase(1L).SetTrans(7L);
			item.currentTween.Play();
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x001DB7B0 File Offset: 0x001D99B0
		private Vector2 GetScaleForQueueIndex(int index)
		{
			index++;
			float num = 1f - (float)index / (float)(index + 1);
			return num * Vector2.One * 0.8f;
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x001DB7E8 File Offset: 0x001D99E8
		private Vector2 GetPositionForQueueIndex(NCard card, int index)
		{
			index++;
			float num = (float)index / (float)(index + 2);
			return PileType.Play.GetTargetPosition(card) + Vector2.Left * 300f * num;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E12 RID: 15890 RVA: 0x001DB824 File Offset: 0x001D9A24
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardPlayQueue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlayQueue.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "forceReturnToHand", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardPlayQueue.MethodName.RemoveCardFromQueue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardPlayQueue.MethodName.TweenAllToQueuePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlayQueue.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlayQueue.MethodName.GetScaleForQueueIndex, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardPlayQueue.MethodName.GetPositionForQueueIndex, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E13 RID: 15891 RVA: 0x001DBAA8 File Offset: 0x001D9CA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardPlayQueue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation && args.Count == 2)
			{
				this.RemoveCardFromQueueForCancellation(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.RemoveCardFromQueue && args.Count == 1)
			{
				this.RemoveCardFromQueue(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.TweenAllToQueuePosition && args.Count == 0)
			{
				this.TweenAllToQueuePosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.GetScaleForQueueIndex && args.Count == 1)
			{
				Vector2 scaleForQueueIndex = this.GetScaleForQueueIndex(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref scaleForQueueIndex);
				return true;
			}
			if ((ref method) == NCardPlayQueue.MethodName.GetPositionForQueueIndex && args.Count == 2)
			{
				Vector2 positionForQueueIndex = this.GetPositionForQueueIndex(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<Vector2>(ref positionForQueueIndex);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003E14 RID: 15892 RVA: 0x001DBC48 File Offset: 0x001D9E48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardPlayQueue.MethodName._Ready || (ref method) == NCardPlayQueue.MethodName._ExitTree || (ref method) == NCardPlayQueue.MethodName.RemoveCardFromQueueForCancellation || (ref method) == NCardPlayQueue.MethodName.RemoveCardFromQueue || (ref method) == NCardPlayQueue.MethodName.TweenAllToQueuePosition || (ref method) == NCardPlayQueue.MethodName.AnimOut || (ref method) == NCardPlayQueue.MethodName.GetScaleForQueueIndex || (ref method) == NCardPlayQueue.MethodName.GetPositionForQueueIndex || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003E15 RID: 15893 RVA: 0x001DBCD4 File Offset: 0x001D9ED4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003E16 RID: 15894 RVA: 0x001DBCDD File Offset: 0x001D9EDD
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001822 RID: 6178
		private List<NCardPlayQueue.QueueItem> _playQueue = new List<NCardPlayQueue.QueueItem>();

		// Token: 0x0200189D RID: 6301
		[Nullable(0)]
		[RequiredMember]
		private class QueueItem
		{
			// Token: 0x0600A914 RID: 43284 RVA: 0x0035682D File Offset: 0x00354A2D
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public QueueItem()
			{
			}

			// Token: 0x04006045 RID: 24645
			[RequiredMember]
			public NCard card;

			// Token: 0x04006046 RID: 24646
			[RequiredMember]
			public GameAction action;

			// Token: 0x04006047 RID: 24647
			[Nullable(2)]
			public Tween currentTween;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200189E RID: 6302
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006048 RID: 24648
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006049 RID: 24649
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'RemoveCardFromQueueForCancellation' method.
			/// </summary>
			// Token: 0x0400604A RID: 24650
			public static readonly StringName RemoveCardFromQueueForCancellation = "RemoveCardFromQueueForCancellation";

			/// <summary>
			/// Cached name for the 'RemoveCardFromQueue' method.
			/// </summary>
			// Token: 0x0400604B RID: 24651
			public static readonly StringName RemoveCardFromQueue = "RemoveCardFromQueue";

			/// <summary>
			/// Cached name for the 'TweenAllToQueuePosition' method.
			/// </summary>
			// Token: 0x0400604C RID: 24652
			public static readonly StringName TweenAllToQueuePosition = "TweenAllToQueuePosition";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x0400604D RID: 24653
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'GetScaleForQueueIndex' method.
			/// </summary>
			// Token: 0x0400604E RID: 24654
			public static readonly StringName GetScaleForQueueIndex = "GetScaleForQueueIndex";

			/// <summary>
			/// Cached name for the 'GetPositionForQueueIndex' method.
			/// </summary>
			// Token: 0x0400604F RID: 24655
			public static readonly StringName GetPositionForQueueIndex = "GetPositionForQueueIndex";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200189F RID: 6303
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018A0 RID: 6304
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
