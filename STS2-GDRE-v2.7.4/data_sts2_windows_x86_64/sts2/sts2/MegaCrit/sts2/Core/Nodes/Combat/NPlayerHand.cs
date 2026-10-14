using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000408 RID: 1032
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NPlayerHand.cs")]
	public class NPlayerHand : Control
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06004091 RID: 16529 RVA: 0x001F1BBA File Offset: 0x001EFDBA
		[Nullable(2)]
		public static NPlayerHand Instance
		{
			[NullableContext(2)]
			get
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.Ui.Hand;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x001F1BD1 File Offset: 0x001EFDD1
		// (set) Token: 0x06004093 RID: 16531 RVA: 0x001F1BD9 File Offset: 0x001EFDD9
		public Control CardHolderContainer { get; private set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x001F1BE2 File Offset: 0x001EFDE2
		// (set) Token: 0x06004095 RID: 16533 RVA: 0x001F1BEA File Offset: 0x001EFDEA
		public NPeekButton PeekButton { get; private set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x001F1BF3 File Offset: 0x001EFDF3
		public bool InCardPlay
		{
			get
			{
				return this._currentCardPlay != null && GodotObject.IsInstanceValid(this._currentCardPlay);
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x001F1C0C File Offset: 0x001EFE0C
		public bool IsInCardSelection
		{
			get
			{
				NPlayerHand.Mode currentMode = this.CurrentMode;
				return currentMode - NPlayerHand.Mode.SimpleSelect <= 1;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x001F1C2D File Offset: 0x001EFE2D
		// (set) Token: 0x06004099 RID: 16537 RVA: 0x001F1C35 File Offset: 0x001EFE35
		public NPlayerHand.Mode CurrentMode
		{
			get
			{
				return this._currentMode;
			}
			private set
			{
				this._currentMode = value;
				base.EmitSignal(NPlayerHand.SignalName.ModeChanged, Array.Empty<Variant>());
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x001F1C4F File Offset: 0x001EFE4F
		private bool HasDraggedHolder
		{
			get
			{
				return this._draggedHolderIndex >= 0;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x001F1C5D File Offset: 0x001EFE5D
		[Nullable(new byte[] { 2, 1 })]
		public Func<CardModel, bool> SelectModeGoldGlowOverride
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._prefs.ShouldGlowGold;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x001F1C6A File Offset: 0x001EFE6A
		// (set) Token: 0x0600409D RID: 16541 RVA: 0x001F1C72 File Offset: 0x001EFE72
		[Nullable(2)]
		public NHandCardHolder FocusedHolder
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x001F1C7B File Offset: 0x001EFE7B
		public IReadOnlyList<NHandCardHolder> ActiveHolders
		{
			get
			{
				return this.Holders.Where<NHandCardHolder>((NHandCardHolder child) => child.IsVisibleInTree()).ToList<NHandCardHolder>();
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600409F RID: 16543 RVA: 0x001F1CAC File Offset: 0x001EFEAC
		private IReadOnlyList<NHandCardHolder> Holders
		{
			get
			{
				return this.CardHolderContainer.GetChildren(false).OfType<NHandCardHolder>().ToList<NHandCardHolder>();
			}
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x001F1CC4 File Offset: 0x001EFEC4
		public override void _Ready()
		{
			this._selectModeBackstop = base.GetNode<Control>("%SelectModeBackstop");
			this.CardHolderContainer = base.GetNode<Control>("%CardHolderContainer");
			this._upgradePreviewContainer = base.GetNode<Control>("%UpgradePreviewContainer");
			this._selectModeConfirmButton = base.GetNode<NConfirmButton>("%SelectModeConfirmButton");
			this._upgradePreview = base.GetNode<NUpgradePreview>("%UpgradePreview");
			this._selectionHeader = base.GetNode<MegaRichTextLabel>("%SelectionHeader");
			this._selectionHeader.Visible = false;
			this._selectedHandCardContainer = base.GetNode<NSelectedHandCardContainer>("%SelectedHandCardContainer");
			this._selectedHandCardContainer.Hand = this;
			this._selectModeConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSelectModeConfirmButtonPressed)), 0U);
			this._selectModeConfirmButton.Disable();
			this._selectedHandCardContainer.Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.OnCardDeselected)), 0U);
			this._selectedHandCardContainer.Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.OnCardSelected)), 0U);
			this.PeekButton = base.GetNode<NPeekButton>("%PeekButton");
			this.PeekButton.Disable();
			this.PeekButton.AddTargets(new Control[] { this._selectModeBackstop, this._upgradePreviewContainer, this._selectModeConfirmButton, this._selectionHeader, this._selectedHandCardContainer });
			this.PeekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(new Action<NPeekButton>(this.OnPeekButtonToggled)), 0U);
			this.CardHolderContainer.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this.DefaultFocusedControl.TryGrabFocus();
			}), 0U);
			this.CardHolderContainer.FocusNeighborBottom = this.CardHolderContainer.GetPath();
			this.CardHolderContainer.FocusNeighborLeft = this.CardHolderContainer.GetPath();
			this.CardHolderContainer.FocusNeighborRight = this.CardHolderContainer.GetPath();
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x001F1EE0 File Offset: 0x001F00E0
		public override void _EnterTree()
		{
			base._EnterTree();
			CombatManager.Instance.PlayerActionsDisabledChanged += this.OnPlayerActionsDisabledChanged;
			CombatManager.Instance.PlayerUnendedTurn += this.OnPlayerUnendedTurn;
			CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x001F1F50 File Offset: 0x001F0150
		public override void _ExitTree()
		{
			base._ExitTree();
			TaskCompletionSource<IEnumerable<CardModel>> selectionCompletionSource = this._selectionCompletionSource;
			if (selectionCompletionSource != null)
			{
				Task<IEnumerable<CardModel>> task = selectionCompletionSource.Task;
				if (task != null && !task.IsCompleted)
				{
					this._selectionCompletionSource.SetResult(Array.Empty<CardModel>());
				}
			}
			CombatManager.Instance.PlayerActionsDisabledChanged -= this.OnPlayerActionsDisabledChanged;
			CombatManager.Instance.PlayerUnendedTurn -= this.OnPlayerUnendedTurn;
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x001F1FEC File Offset: 0x001F01EC
		[return: Nullable(2)]
		public NCard GetCard(CardModel card)
		{
			NCardHolder cardHolder = this.GetCardHolder(card);
			if (cardHolder == null)
			{
				return null;
			}
			return cardHolder.CardNode;
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x001F2000 File Offset: 0x001F0200
		[NullableContext(2)]
		public bool IsAwaitingPlay(NHandCardHolder holder)
		{
			return holder != null && this._holdersAwaitingQueue.Contains(holder);
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x001F2014 File Offset: 0x001F0214
		[return: Nullable(2)]
		public NCardHolder GetCardHolder(CardModel card)
		{
			return this.Holders.Concat<NCardHolder>(this._selectedHandCardContainer.Holders).Concat<NCardHolder>(this._holdersAwaitingQueue).FirstOrDefault<NCardHolder>((NCardHolder h) => h.CardNode != null && h.CardNode.Model == card);
		}

		/// <summary>
		/// Called whenever we add a card into the hand.
		/// Prunes any empty CardHolders and creates a CardHolder for the newly added card.
		/// </summary>
		// Token: 0x060040A6 RID: 16550 RVA: 0x001F2060 File Offset: 0x001F0260
		public NHandCardHolder Add(NCard card, int index = -1)
		{
			Vector2 globalPosition = card.GlobalPosition;
			NHandCardHolder nhandCardHolder = NHandCardHolder.Create(card, this);
			this.AddCardHolder(nhandCardHolder, index);
			nhandCardHolder.GlobalPosition = globalPosition;
			this.RefreshLayout();
			if (this.IsInCardSelection)
			{
				this.UpdateSelectModeCardVisibility();
			}
			return nhandCardHolder;
		}

		/// <summary>
		/// Remove an NCard and its holder from the hand.
		/// </summary>
		/// <param name="card">CardModel whose node we want to remove</param>
		// Token: 0x060040A7 RID: 16551 RVA: 0x001F20A0 File Offset: 0x001F02A0
		public void Remove(CardModel card)
		{
			NCardHolder cardHolder = this.GetCardHolder(card);
			if (cardHolder == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No holder for card ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Id);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.InCardPlay && card == this._currentCardPlay.Holder.CardModel)
			{
				this._currentCardPlay.CancelPlayCard();
			}
			this.RemoveCardHolder(cardHolder);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x001F2118 File Offset: 0x001F0318
		private void AddCardHolder(NHandCardHolder holder, int index)
		{
			this.CardHolderContainer.AddChildSafely(holder);
			if (index >= 0)
			{
				this.CardHolderContainer.MoveChildSafely(holder, index);
			}
			holder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
			holder.Connect(NHandCardHolder.SignalName.HolderMouseClicked, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
			holder.Connect(NHandCardHolder.SignalName.HolderFocused, Callable.From<NHandCardHolder>(new Action<NHandCardHolder>(this.OnHolderFocused)), 0U);
			holder.Connect(NHandCardHolder.SignalName.HolderUnfocused, Callable.From<NHandCardHolder>(new Action<NHandCardHolder>(this.OnHolderUnfocused)), 0U);
			this.RefreshLayout();
			if (this.CardHolderContainer.HasFocus())
			{
				holder.TryGrabFocus();
			}
		}

		/// <summary>
		/// Translates a card's position in the backend Hand pile into a child index within CardHolderContainer.
		///
		/// Cards that are selected, awaiting play, or being dragged stay in the backend Hand pile while their holders are
		/// absent from CardHolderContainer (selected holders move to the selected-card container; dragged/queued holders
		/// reparent onto the hand itself), so the raw pile index can exceed the live child count and Godot rejects the
		/// MoveChild with "Invalid new child index" (PRG-6847). Counting only the holders currently in the container
		/// keeps the result within its child range.
		/// </summary>
		/// <returns>The target child index, or -1 if the card is not in the Hand pile (caller should not move).</returns>
		// Token: 0x060040A9 RID: 16553 RVA: 0x001F21D4 File Offset: 0x001F03D4
		private int GetHandInsertIndex(CardModel card)
		{
			IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(card.Owner).Cards;
			List<CardModel> list = (from holder in this.Holders.Where<NHandCardHolder>(delegate(NHandCardHolder holder)
				{
					NCard cardNode = holder.CardNode;
					return ((cardNode != null) ? cardNode.Model : null) != null;
				})
				select holder.CardNode.Model).ToList<CardModel>();
			return HandLayoutHelper.GetInsertIndex<CardModel>(cards, list, card);
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x001F2250 File Offset: 0x001F0450
		public void RemoveCardHolder(NCardHolder holder)
		{
			NHandCardHolder nhandCardHolder = holder as NHandCardHolder;
			if (nhandCardHolder != null)
			{
				this._holdersAwaitingQueue.Remove(nhandCardHolder);
			}
			if (this.InCardPlay && this._currentCardPlay.Holder == holder)
			{
				this._currentCardPlay.CancelPlayCard();
			}
			bool flag = holder.HasFocus();
			holder.GetParent().RemoveChildSafely(holder);
			holder.Clear();
			holder.QueueFreeSafely();
			this.RefreshLayout();
			if (flag)
			{
				this.DefaultFocusedControl.TryGrabFocus();
			}
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x001F22C8 File Offset: 0x001F04C8
		private void OnHolderFocused(NHandCardHolder holder)
		{
			this.FocusedHolder = holder;
			this._lastFocusedHolderIdx = holder.GetIndex(false);
			RunManager.Instance.HoveredModelTracker.OnLocalCardHovered(this.FocusedHolder.CardModel);
			this.RefreshLayout();
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x001F22FE File Offset: 0x001F04FE
		private void OnHolderUnfocused(NHandCardHolder holder)
		{
			this.FocusedHolder = null;
			RunManager.Instance.HoveredModelTracker.OnLocalCardUnhovered();
			this.RefreshLayout();
		}

		/// <summary>
		/// If the specific model is being played, then cancel it.
		/// </summary>
		/// <param name="card">Card we are trying to stop from being played</param>
		// Token: 0x060040AD RID: 16557 RVA: 0x001F231C File Offset: 0x001F051C
		public void TryCancelCardPlay(CardModel card)
		{
			NCardHolder cardHolder = this.GetCardHolder(card);
			NHandCardHolder nhandCardHolder = cardHolder as NHandCardHolder;
			if (nhandCardHolder != null && this.IsAwaitingPlay(nhandCardHolder))
			{
				this.ReturnHolderToHand(nhandCardHolder);
				nhandCardHolder.UpdateCard();
				if (this.InCardPlay && this._currentCardPlay.Holder == nhandCardHolder)
				{
					this._currentCardPlay.CancelPlayCard();
					return;
				}
				this.RefreshLayout();
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001F237C File Offset: 0x001F057C
		public void CancelAllCardPlay()
		{
			if (this.InCardPlay)
			{
				this._currentCardPlay.CancelPlayCard();
			}
			foreach (NHandCardHolder nhandCardHolder in this._holdersAwaitingQueue.ToList<NHandCardHolder>())
			{
				this.ReturnHolderToHand(nhandCardHolder);
			}
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x001F23E8 File Offset: 0x001F05E8
		private void ReturnHolderToHand(NHandCardHolder holder)
		{
			if (this.IsAwaitingPlay(holder))
			{
				this._holdersAwaitingQueue.Remove(holder);
				holder.Reparent(this.CardHolderContainer, true);
				int handInsertIndex = this.GetHandInsertIndex(holder.CardNode.Model);
				if (handInsertIndex >= 0)
				{
					this.CardHolderContainer.MoveChildSafely(holder, handInsertIndex);
				}
				holder.SetDefaultTargets();
			}
		}

		/// <summary>
		/// Be sure you know what your doing when you call this function. Should only be used
		/// by the NHandCardCountTickbox to toggle on/off the player hand count labels right away.
		/// </summary>
		// Token: 0x060040B0 RID: 16560 RVA: 0x001F2441 File Offset: 0x001F0641
		public void ForceRefreshCardIndices()
		{
			this.RefreshLayout();
		}

		/// <summary>
		/// Call this whenever we gain/lose cards so that we fan out the cards for our players.
		/// </summary>
		// Token: 0x060040B1 RID: 16561 RVA: 0x001F244C File Offset: 0x001F064C
		private void RefreshLayout()
		{
			int count = this.ActiveHolders.Count;
			if (count <= 0)
			{
				return;
			}
			int num = count;
			Vector2 scale = HandPosHelper.GetScale(count);
			int num2 = -1;
			if (this.FocusedHolder != null)
			{
				num2 = this.ActiveHolders.IndexOf(this.FocusedHolder);
			}
			for (int i = 0; i < count; i++)
			{
				int num3 = i;
				Vector2 vector = HandPosHelper.GetPosition(num, num3);
				if (num2 > -1)
				{
					float num4 = Mathf.Lerp(100f, 0f, Mathf.Min(1f, (float)Mathf.Abs(num2 - i) / 4f));
					vector += Vector2.Left * (float)Mathf.Sign(num2 - i) * num4;
				}
				NHandCardHolder nhandCardHolder = this.ActiveHolders[i];
				if (num2 == i)
				{
					nhandCardHolder.SetAngleInstantly(0f);
					nhandCardHolder.SetScaleInstantly(Vector2.One);
					vector.Y = -nhandCardHolder.Hitbox.Size.Y * 0.5f + 2f;
					if (this._isDisabled)
					{
						vector -= NPlayerHand._disablePosition;
					}
					nhandCardHolder.Position = new Vector2(nhandCardHolder.Position.X, vector.Y);
					nhandCardHolder.SetTargetPosition(vector);
				}
				else
				{
					nhandCardHolder.SetTargetPosition(vector);
					nhandCardHolder.SetTargetScale(scale);
					nhandCardHolder.SetTargetAngle(HandPosHelper.GetAngle(num, num3));
				}
				nhandCardHolder.Hitbox.MouseFilter = (this.HasDraggedHolder ? 2L : 0L);
				Control control = nhandCardHolder;
				NodePath nodePath;
				if (i <= 0)
				{
					IReadOnlyList<NHandCardHolder> activeHolders = this.ActiveHolders;
					nodePath = activeHolders[activeHolders.Count - 1].GetPath();
				}
				else
				{
					nodePath = this.ActiveHolders[i - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				nhandCardHolder.FocusNeighborRight = ((i < this.ActiveHolders.Count - 1) ? this.ActiveHolders[i + 1].GetPath() : this.ActiveHolders[0].GetPath());
				nhandCardHolder.FocusNeighborBottom = nhandCardHolder.GetPath();
				if (this.HasDraggedHolder && i >= this._draggedHolderIndex)
				{
					nhandCardHolder.SetIndexLabel(i + 2);
				}
				else
				{
					nhandCardHolder.SetIndexLabel(i + 1);
				}
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x001F2682 File Offset: 0x001F0882
		private void OnPlayerUnendedTurn(Player player)
		{
			this.UpdateHandDisabledState(player.Creature.CombatState);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x001F2695 File Offset: 0x001F0895
		private void OnPlayerActionsDisabledChanged(CombatState state)
		{
			this.UpdateHandDisabledState(state);
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x001F26A0 File Offset: 0x001F08A0
		private void UpdateHandDisabledState(ICombatState state)
		{
			Player me = LocalContext.GetMe(state);
			bool flag = CombatManager.Instance.PlayerActionsDisabled;
			if (!flag && CombatManager.Instance.PlayersTakingExtraTurn.Count > 0 && me != null && !CombatManager.Instance.PlayersTakingExtraTurn.Contains(me))
			{
				flag = true;
			}
			if (flag)
			{
				bool flag2 = me == null || !state.Players.Except<Player>(new <>z__ReadOnlySingleElementList<Player>(me)).All<Player>(new Func<Player, bool>(CombatManager.Instance.IsPlayerReadyToEndTurn));
				bool flag3 = state.CurrentSide == CombatSide.Enemy;
				if (flag3 || flag2)
				{
					this.AnimDisable();
					return;
				}
			}
			else
			{
				this.AnimEnable();
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x001F273C File Offset: 0x001F093C
		private void OnCombatStateChanged(CombatState state)
		{
			this._combatState = state;
			if (this.IsInCardSelection)
			{
				this.RevalidateSelectionAfterStateChange();
			}
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				nhandCardHolder.UpdateCard();
			}
			foreach (NHandCardHolder nhandCardHolder2 in this._holdersAwaitingQueue)
			{
				nhandCardHolder2.UpdateCard();
			}
			foreach (NSelectedHandCardHolder nselectedHandCardHolder in this._selectedHandCardContainer.Holders)
			{
				NCard cardNode = nselectedHandCardHolder.CardNode;
				if (cardNode != null)
				{
					cardNode.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
				}
			}
			this.UpdateHandDisabledState(state);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x001F283C File Offset: 0x001F0A3C
		private void OnCombatEnded(CombatRoom _)
		{
			this.CancelAllCardPlay();
			this.CardHolderContainer.FocusMode = 0L;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x001F2854 File Offset: 0x001F0A54
		private void OnPeekButtonToggled(NPeekButton button)
		{
			if (button.IsPeeking)
			{
				NCombatRoom.Instance.EnableControllerNavigation();
				this._selectModeConfirmButton.Disable();
			}
			else
			{
				NCombatRoom.Instance.RestrictControllerNavigation(Array.Empty<Control>());
				this.EnableControllerNavigation();
				this.RefreshSelectModeConfirmButton();
				Viewport viewport = base.GetViewport();
				if (viewport != null && viewport.GuiGetFocusOwner() == this.CardHolderContainer)
				{
					this.DefaultFocusedControl.TryGrabFocus();
				}
			}
			this.UpdateSelectModeCardVisibility();
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x001F28D0 File Offset: 0x001F0AD0
		public async Task<IEnumerable<CardModel>> SelectCards(CardSelectorPrefs prefs, [Nullable(new byte[] { 2, 1 })] Func<CardModel, bool> filter, [Nullable(2)] AbstractModel source, NPlayerHand.Mode mode = NPlayerHand.Mode.SimpleSelect)
		{
			this.CancelAllCardPlay();
			this._selectModeBackstop.Visible = true;
			this._selectModeBackstop.MouseFilter = 0L;
			CanvasItem selectModeBackstop = this._selectModeBackstop;
			Color selfModulate = this._selectModeBackstop.SelfModulate;
			selfModulate.A = 0f;
			selectModeBackstop.SelfModulate = selfModulate;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._selectModeBackstop, "self_modulate:a", 1f, 0.20000000298023224);
			bool wasDisabled = this._isDisabled;
			if (this._isDisabled)
			{
				this.AnimEnable();
			}
			this.CurrentMode = mode;
			this._currentSelectionFilter = filter;
			NCombatRoom.Instance.RestrictControllerNavigation(Array.Empty<Control>());
			NCombatUi ui = NCombatRoom.Instance.Ui;
			ui.OnHandSelectModeEntered();
			this.EnableControllerNavigation();
			this._prefs = prefs;
			this._selectionCompletionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
			this._selectionHeader.Visible = true;
			this._selectionHeader.Text = "[center]" + prefs.Prompt.GetFormattedText() + "[/center]";
			this.PeekButton.Enable();
			this.UpdateSelectModeCardVisibility();
			this.RefreshSelectModeConfirmButton();
			IEnumerable<CardModel> enumerable = await this._selectionCompletionSource.Task;
			IEnumerable<CardModel> enumerable2 = enumerable;
			IEnumerable<CardModel> enumerable3;
			if (!base.IsInsideTree())
			{
				enumerable3 = enumerable2;
			}
			else
			{
				tween.Kill();
				this.AfterCardsSelected(source);
				if (wasDisabled)
				{
					this.AnimDisable();
				}
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.EnableControllerNavigation();
				}
				enumerable3 = enumerable2;
			}
			return enumerable3;
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x001F2934 File Offset: 0x001F0B34
		private void UpdateSelectModeCardVisibility()
		{
			if (this.CurrentMode != NPlayerHand.Mode.SimpleSelect && this.CurrentMode != NPlayerHand.Mode.UpgradeSelect)
			{
				throw new InvalidOperationException("Can only be used when we are selecting a card");
			}
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				if (nhandCardHolder.CardNode != null)
				{
					if (this.PeekButton.IsPeeking)
					{
						nhandCardHolder.Visible = true;
						nhandCardHolder.CardNode.SetPretendCardCanBePlayed(false);
						nhandCardHolder.CardNode.SetForceUnpoweredPreview(false);
					}
					else
					{
						CanvasItem canvasItem = nhandCardHolder;
						Func<CardModel, bool> currentSelectionFilter = this._currentSelectionFilter;
						canvasItem.Visible = currentSelectionFilter == null || currentSelectionFilter(nhandCardHolder.CardNode.Model);
						nhandCardHolder.CardNode.SetPretendCardCanBePlayed(this._prefs.PretendCardsCanBePlayed);
						nhandCardHolder.CardNode.SetForceUnpoweredPreview(this._prefs.UnpoweredPreviews);
					}
					nhandCardHolder.UpdateCard();
				}
			}
			this.RefreshLayout();
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x001F2A34 File Offset: 0x001F0C34
		[NullableContext(2)]
		private void AfterCardsSelected(AbstractModel source)
		{
			this._selectedCards.Clear();
			this.CurrentMode = NPlayerHand.Mode.Play;
			this._prefs = default(CardSelectorPrefs);
			this._currentSelectionFilter = null;
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				nhandCardHolder.InSelectMode = false;
				nhandCardHolder.Visible = true;
				NCard cardNode = nhandCardHolder.CardNode;
				if (cardNode != null)
				{
					cardNode.SetPretendCardCanBePlayed(false);
				}
				NCard cardNode2 = nhandCardHolder.CardNode;
				if (cardNode2 != null)
				{
					cardNode2.SetForceUnpoweredPreview(false);
				}
				nhandCardHolder.UpdateCard();
			}
			this.RefreshLayout();
			this._selectModeBackstop.Visible = false;
			this._selectModeBackstop.MouseFilter = 2L;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._selectModeBackstop, "self_modulate:a", 0f, 0.20000000298023224);
			this._selectModeConfirmButton.Disable();
			this._upgradePreviewContainer.Visible = false;
			this._selectionHeader.Visible = false;
			this.PeekButton.Disable();
			NCombatRoom.Instance.Ui.OnHandSelectModeExited();
			if (source != null)
			{
				source.ExecutionFinished += this.OnSelectModeSourceFinished;
				return;
			}
			this.OnSelectModeSourceFinished(null);
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x001F2B84 File Offset: 0x001F0D84
		private void CancelHandSelectionIfNecessary()
		{
			if (this.IsInCardSelection && this._selectionCompletionSource != null)
			{
				this._selectionCompletionSource.SetCanceled();
				this.AfterCardsSelected(null);
			}
		}

		/// <summary>
		/// Called when a card is clicked in the hand.
		/// </summary>
		/// <param name="holder">CardHolder that was clicked.</param>
		// Token: 0x060040BC RID: 16572 RVA: 0x001F2BA8 File Offset: 0x001F0DA8
		private void OnHolderPressed(NCardHolder holder)
		{
			if (this.PeekButton.IsPeeking)
			{
				this.PeekButton.Wiggle();
				return;
			}
			NHandCardHolder nhandCardHolder = (NHandCardHolder)holder;
			if (nhandCardHolder.CardNode == null)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (NOverlayStack.Instance.ScreenCount > 0)
			{
				return;
			}
			switch (this.CurrentMode)
			{
			case NPlayerHand.Mode.None:
				break;
			case NPlayerHand.Mode.Play:
				if (this.CanPlayCards())
				{
					this.StartCardPlay(nhandCardHolder, false);
					return;
				}
				break;
			case NPlayerHand.Mode.SimpleSelect:
				this.SelectCardInSimpleMode(nhandCardHolder);
				return;
			case NPlayerHand.Mode.UpgradeSelect:
				this.SelectCardInUpgradeMode(nhandCardHolder);
				return;
			default:
				throw new ArgumentOutOfRangeException("CurrentMode");
			}
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x001F2C44 File Offset: 0x001F0E44
		private bool CanPlayCards()
		{
			return !this.InCardPlay && this.AreCardActionsAllowed();
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x001F2C58 File Offset: 0x001F0E58
		private bool AreCardActionsAllowed()
		{
			if (CombatManager.Instance.PlayersTakingExtraTurn.Count > 0 && this._combatState != null)
			{
				Player me = LocalContext.GetMe(this._combatState);
				if (me == null || !CombatManager.Instance.PlayersTakingExtraTurn.Contains(me))
				{
					return false;
				}
			}
			return !CombatManager.Instance.PlayerActionsDisabled && !this.PeekButton.IsPeeking;
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x001F2CC0 File Offset: 0x001F0EC0
		private void StartCardPlay(NHandCardHolder holder, bool startedViaShortcut)
		{
			this._draggedHolderIndex = holder.GetIndex(false);
			this._holdersAwaitingQueue.Add(holder);
			holder.Reparent(this, true);
			holder.BeginDrag();
			this._currentCardPlay = (NControllerManager.Instance.IsUsingController ? NControllerCardPlay.Create(holder) : NMouseCardPlay.Create(holder, this._selectCardShortcuts[this._draggedHolderIndex], startedViaShortcut));
			this.AddChildSafely(this._currentCardPlay);
			this._currentCardPlay.Connect(NCardPlay.SignalName.Finished, Callable.From<bool>(delegate(bool success)
			{
				RunManager.Instance.HoveredModelTracker.OnLocalCardDeselected();
				if (!success)
				{
					this.ReturnHolderToHand(holder);
				}
				this._draggedHolderIndex = -1;
				this.RefreshLayout();
			}), 0U);
			RunManager.Instance.HoveredModelTracker.OnLocalCardSelected(holder.CardNode.Model);
			this._currentCardPlay.Start();
			this.RefreshLayout();
			holder.SetIndexLabel(this._draggedHolderIndex + 1);
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x001F2DC8 File Offset: 0x001F0FC8
		private void SelectCardInSimpleMode(NHandCardHolder holder)
		{
			if (this._selectedCards.Count >= this._prefs.MaxSelect)
			{
				this._selectedHandCardContainer.DeselectCard(this._selectedCards.Last<CardModel>());
			}
			this._selectedCards.Add(holder.CardNode.Model);
			this._selectedHandCardContainer.Add(holder);
			this.RemoveCardHolder(holder);
			this.RefreshSelectModeConfirmButton();
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x001F2E34 File Offset: 0x001F1034
		private void SelectCardInUpgradeMode(NHandCardHolder holder)
		{
			CardModel model = holder.CardNode.Model;
			if (this._selectedCards.Count != 0)
			{
				NCard ncard = NCard.Create(this._selectedCards.Last<CardModel>(), ModelVisibility.Visible);
				ncard.GlobalPosition = this._upgradePreview.SelectedCardPosition;
				this.DeselectCard(ncard);
			}
			this._selectedCards.Add(model);
			this._upgradePreviewContainer.Visible = true;
			this._upgradePreview.Card = model;
			this.RemoveCardHolder(holder);
			this.RefreshSelectModeConfirmButton();
		}

		/// <summary>
		/// Deselect a card when in a Select Mode.
		/// </summary>
		/// <param name="card"></param>
		// Token: 0x060040C2 RID: 16578 RVA: 0x001F2EB8 File Offset: 0x001F10B8
		public void DeselectCard(NCard card)
		{
			if (!this.IsInCardSelection)
			{
				throw new InvalidOperationException("Only valid when in Select Mode.");
			}
			NHandCardHolder nhandCardHolder = this.Add(card, this.GetHandInsertIndex(card.Model));
			nhandCardHolder.InSelectMode = true;
			nhandCardHolder.Visible = true;
			this._selectedCards.Remove(card.Model);
			this.RefreshSelectModeConfirmButton();
			nhandCardHolder.TryGrabFocus();
		}

		/// <summary>
		/// Combat state can change while the player is mid-selection (e.g. another player kills a monster whose
		/// power was downgrading our cards). Re-evaluate the filter against the current hand and the currently-
		/// selected cards, deselecting anything that no longer qualifies and cancelling the whole selection with
		/// an empty result if no valid candidates remain.
		/// </summary>
		// Token: 0x060040C3 RID: 16579 RVA: 0x001F2F18 File Offset: 0x001F1118
		private void RevalidateSelectionAfterStateChange()
		{
			Func<CardModel, bool> filter = this._currentSelectionFilter;
			if (filter == null)
			{
				return;
			}
			if (this._upgradePreview.Card != null && !filter(this._upgradePreview.Card))
			{
				CardModel card = this._upgradePreview.Card;
				this._upgradePreview.Card = null;
				this._upgradePreviewContainer.Visible = false;
				this._selectedCards.Remove(card);
				this.Add(NCard.Create(card, ModelVisibility.Visible), this.GetHandInsertIndex(card));
			}
			foreach (NSelectedHandCardHolder nselectedHandCardHolder in this._selectedHandCardContainer.Holders.ToList<NSelectedHandCardHolder>())
			{
				NCard cardNode = nselectedHandCardHolder.CardNode;
				CardModel cardModel = ((cardNode != null) ? cardNode.Model : null);
				if (cardModel != null && !filter(cardModel))
				{
					this._selectedHandCardContainer.DeselectCard(cardModel);
				}
			}
			this.UpdateSelectModeCardVisibility();
			bool flag = this.Holders.Any<NHandCardHolder>((NHandCardHolder h) => h.CardNode != null && filter(h.CardNode.Model));
			if (this._selectedCards.Count != 0 || flag)
			{
				this.RefreshSelectModeConfirmButton();
				return;
			}
			TaskCompletionSource<IEnumerable<CardModel>> selectionCompletionSource = this._selectionCompletionSource;
			if (selectionCompletionSource == null)
			{
				return;
			}
			selectionCompletionSource.TrySetResult(Array.Empty<CardModel>());
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x001F3078 File Offset: 0x001F1278
		private void OnSelectModeConfirmButtonPressed(NButton _)
		{
			this._selectionCompletionSource.SetResult(this._selectedCards.ToList<CardModel>());
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x001F3090 File Offset: 0x001F1290
		private void CheckIfSelectionComplete()
		{
			if (this._selectedCards.Count >= this._prefs.MaxSelect)
			{
				this._selectionCompletionSource.SetResult(this._selectedCards.ToList<CardModel>());
			}
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x001F30C0 File Offset: 0x001F12C0
		private void RefreshSelectModeConfirmButton()
		{
			int count = this._selectedCards.Count;
			if (count >= this._prefs.MinSelect && count <= this._prefs.MaxSelect)
			{
				this._selectModeConfirmButton.Enable();
				return;
			}
			this._selectModeConfirmButton.Disable();
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x001F310C File Offset: 0x001F130C
		[NullableContext(2)]
		private void OnSelectModeSourceFinished(AbstractModel source)
		{
			foreach (NSelectedHandCardHolder nselectedHandCardHolder in this._selectedHandCardContainer.Holders.ToList<NSelectedHandCardHolder>())
			{
				NCard cardNode = nselectedHandCardHolder.CardNode;
				nselectedHandCardHolder.QueueFreeSafely();
				this.Add(cardNode, -1);
			}
			if (this._upgradePreview.Card != null)
			{
				this.Add(NCard.Create(this._upgradePreview.Card, ModelVisibility.Visible), -1);
				this._upgradePreview.Card = null;
			}
			if (source != null)
			{
				source.ExecutionFinished -= this.OnSelectModeSourceFinished;
			}
		}

		/// <summary>
		/// Animation for entering combat
		/// </summary>
		// Token: 0x060040C8 RID: 16584 RVA: 0x001F31C0 File Offset: 0x001F13C0
		public void AnimIn()
		{
			Tween animOutTween = this._animOutTween;
			if (animOutTween != null)
			{
				animOutTween.Kill();
			}
			Tween animEnableTween = this._animEnableTween;
			if (animEnableTween != null)
			{
				animEnableTween.Kill();
			}
			this._animInTween = base.CreateTween();
			this._animInTween.TweenProperty(this, "position", NPlayerHand._showPosition, 0.800000011920929).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Animation when winning combat
		/// </summary>
		// Token: 0x060040C9 RID: 16585 RVA: 0x001F3234 File Offset: 0x001F1434
		public void AnimOut()
		{
			this.CancelHandSelectionIfNecessary();
			Tween animInTween = this._animInTween;
			if (animInTween != null)
			{
				animInTween.Kill();
			}
			Tween animEnableTween = this._animEnableTween;
			if (animEnableTween != null)
			{
				animEnableTween.Kill();
			}
			this._animOutTween = base.CreateTween();
			this._animOutTween.TweenProperty(this, "position", NPlayerHand._hidePosition, 0.800000011920929).SetEase(0L).SetTrans(10L);
		}

		/// <summary>
		/// Animation when player actions are disabled during player turn
		/// </summary>
		// Token: 0x060040CA RID: 16586 RVA: 0x001F32B0 File Offset: 0x001F14B0
		private void AnimDisable()
		{
			if (this._isDisabled)
			{
				return;
			}
			this._animEnableTween = base.CreateTween().SetParallel(true);
			this._animEnableTween.TweenProperty(this, "position", NPlayerHand._disablePosition, 0.2).SetEase(1L).SetTrans(7L);
			this._animEnableTween.TweenProperty(this, "modulate", NPlayerHand._disableModulate, 0.2).SetEase(1L).SetTrans(7L);
			this._isDisabled = true;
			this.DisableControllerNavigation();
		}

		/// <summary>
		/// Animation when player actions are re-enabled during player turn
		/// </summary>
		// Token: 0x060040CB RID: 16587 RVA: 0x001F3358 File Offset: 0x001F1558
		private void AnimEnable()
		{
			if (!this._isDisabled)
			{
				return;
			}
			this._animEnableTween = base.CreateTween().SetParallel(true);
			this._animEnableTween.TweenProperty(this, "position", NPlayerHand._showPosition, 0.2).SetEase(1L).SetTrans(7L);
			this._animEnableTween.TweenProperty(this, "modulate", Colors.White, 0.2).SetEase(1L).SetTrans(7L);
			this._isDisabled = false;
			this.EnableControllerNavigation();
			this.DefaultFocusedControl.TryGrabFocus();
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x001F3408 File Offset: 0x001F1608
		public void FlashPlayableHolders()
		{
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				if (nhandCardHolder.CardNode != null && nhandCardHolder.CardNode.Model.CanPlay())
				{
					nhandCardHolder.Flash();
				}
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x001F3470 File Offset: 0x001F1670
		private void OnCardSelected(Node _)
		{
			this.UpdateSelectedCardContainer(this._selectedHandCardContainer.GetChildCount(false));
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x001F3484 File Offset: 0x001F1684
		private void OnCardDeselected(Node _)
		{
			this.UpdateSelectedCardContainer(this._selectedHandCardContainer.GetChildCount(false) - 1);
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x001F349C File Offset: 0x001F169C
		private void UpdateSelectedCardContainer(int count)
		{
			float num = 1f;
			float num2 = base.Size.Y * 0.5f;
			if (count > 6)
			{
				num = 0.55f;
				num2 -= 150f;
			}
			else if (count > 3)
			{
				num = 0.8f;
				num2 -= 75f;
			}
			Tween selectedCardScaleTween = this._selectedCardScaleTween;
			if (selectedCardScaleTween != null)
			{
				selectedCardScaleTween.Kill();
			}
			this._selectedCardScaleTween = base.CreateTween().SetParallel(true);
			this._selectedCardScaleTween.TweenProperty(this._selectedHandCardContainer, "position:y", num2, 0.5).SetTrans(4L).SetEase(2L);
			this._selectedCardScaleTween.TweenProperty(this._selectedHandCardContainer, "scale", Vector2.One * num, 0.5).SetTrans(4L).SetEase(2L);
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060040D0 RID: 16592 RVA: 0x001F3588 File Offset: 0x001F1788
		public Control DefaultFocusedControl
		{
			get
			{
				if (this.ActiveHolders.Count > 0)
				{
					if (this._lastFocusedHolderIdx >= 0)
					{
						return this.ActiveHolders[Mathf.Clamp(this._lastFocusedHolderIdx, 0, this.ActiveHolders.Count - 1)];
					}
					return this.ActiveHolders[this.ActiveHolders.Count / 2];
				}
				else
				{
					if (this.CurrentMode == NPlayerHand.Mode.SimpleSelect && this._selectedCards.Count > 0 && !this.PeekButton.IsPeeking)
					{
						return this.GetCardHolder(this._selectedCards.First<CardModel>());
					}
					if (this.CurrentMode == NPlayerHand.Mode.UpgradeSelect && !this.PeekButton.IsPeeking)
					{
						return this._upgradePreview.DefaultFocusedControl;
					}
					return this.CardHolderContainer;
				}
			}
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x001F3648 File Offset: 0x001F1848
		public void EnableControllerNavigation()
		{
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				nhandCardHolder.FocusMode = 2L;
			}
			if (this.InCardPlay)
			{
				this._currentCardPlay.Holder.FocusMode = 2L;
			}
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x001F36B0 File Offset: 0x001F18B0
		public void DisableControllerNavigation()
		{
			foreach (NHandCardHolder nhandCardHolder in this.Holders)
			{
				nhandCardHolder.FocusMode = 0L;
			}
			if (this.InCardPlay)
			{
				this._currentCardPlay.Holder.FocusMode = 0L;
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x001F3718 File Offset: 0x001F1918
		public override void _UnhandledInput(InputEvent input)
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (!ActiveScreenContext.Instance.IsCurrent(NCombatRoom.Instance))
			{
				return;
			}
			if (CombatManager.Instance.IsOverOrEnding)
			{
				return;
			}
			List<NHandCardHolder> list = new List<NHandCardHolder>();
			list.AddRange(this.ActiveHolders);
			if (this.HasDraggedHolder)
			{
				list.Insert(this._draggedHolderIndex, null);
			}
			for (int i = 0; i < this._selectCardShortcuts.Length; i++)
			{
				StringName stringName = this._selectCardShortcuts[i];
				if (input.IsActionPressed(stringName, false, false) && list.Count > i)
				{
					NHandCardHolder nhandCardHolder = list[i];
					if (nhandCardHolder != null)
					{
						if (NTargetManager.Instance.IsInSelection)
						{
							NTargetManager.Instance.CancelTargeting();
						}
						switch (this.CurrentMode)
						{
						case NPlayerHand.Mode.Play:
							if (this.AreCardActionsAllowed())
							{
								if (this.InCardPlay)
								{
									this._currentCardPlay.CancelPlayCard();
								}
								this.StartCardPlay(nhandCardHolder, true);
							}
							break;
						case NPlayerHand.Mode.SimpleSelect:
							if (!this.PeekButton.IsPeeking)
							{
								this.SelectCardInSimpleMode(nhandCardHolder);
							}
							break;
						case NPlayerHand.Mode.UpgradeSelect:
							if (!this.PeekButton.IsPeeking)
							{
								this.SelectCardInUpgradeMode(nhandCardHolder);
							}
							break;
						}
						Viewport viewport = base.GetViewport();
						if (viewport != null)
						{
							viewport.SetInputAsHandled();
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060040D4 RID: 16596 RVA: 0x001F3858 File Offset: 0x001F1A58
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(38)
			{
				new MethodInfo(NPlayerHand.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.IsAwaitingPlay, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.Add, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.AddCardHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.RemoveCardHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.OnHolderFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.OnHolderUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.CancelAllCardPlay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.ReturnHolderToHand, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.ForceRefreshCardIndices, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.RefreshLayout, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.OnPeekButtonToggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.UpdateSelectModeCardVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.CancelHandSelectionIfNecessary, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.OnHolderPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.CanPlayCards, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.AreCardActionsAllowed, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.StartCardPlay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "startedViaShortcut", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.SelectCardInSimpleMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.SelectCardInUpgradeMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.DeselectCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.RevalidateSelectionAfterStateChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.RefreshSelectModeConfirmButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.AnimDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.AnimEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.FlashPlayableHolders, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.OnCardSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.OnCardDeselected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.UpdateSelectedCardContainer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "count", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPlayerHand.MethodName.EnableControllerNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName.DisableControllerNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPlayerHand.MethodName._UnhandledInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "input", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060040D5 RID: 16597 RVA: 0x001F432C File Offset: 0x001F252C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPlayerHand.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.IsAwaitingPlay && args.Count == 1)
			{
				bool flag = this.IsAwaitingPlay(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.Add && args.Count == 2)
			{
				NHandCardHolder nhandCardHolder = this.Add(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = VariantUtils.CreateFrom<NHandCardHolder>(ref nhandCardHolder);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AddCardHolder && args.Count == 2)
			{
				this.AddCardHolder(VariantUtils.ConvertTo<NHandCardHolder>(args[0]), VariantUtils.ConvertTo<int>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.RemoveCardHolder && args.Count == 1)
			{
				this.RemoveCardHolder(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnHolderFocused && args.Count == 1)
			{
				this.OnHolderFocused(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnHolderUnfocused && args.Count == 1)
			{
				this.OnHolderUnfocused(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.CancelAllCardPlay && args.Count == 0)
			{
				this.CancelAllCardPlay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.ReturnHolderToHand && args.Count == 1)
			{
				this.ReturnHolderToHand(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.ForceRefreshCardIndices && args.Count == 0)
			{
				this.ForceRefreshCardIndices();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.RefreshLayout && args.Count == 0)
			{
				this.RefreshLayout();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnPeekButtonToggled && args.Count == 1)
			{
				this.OnPeekButtonToggled(VariantUtils.ConvertTo<NPeekButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.UpdateSelectModeCardVisibility && args.Count == 0)
			{
				this.UpdateSelectModeCardVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.CancelHandSelectionIfNecessary && args.Count == 0)
			{
				this.CancelHandSelectionIfNecessary();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnHolderPressed && args.Count == 1)
			{
				this.OnHolderPressed(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.CanPlayCards && args.Count == 0)
			{
				bool flag2 = this.CanPlayCards();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AreCardActionsAllowed && args.Count == 0)
			{
				bool flag3 = this.AreCardActionsAllowed();
				ret = VariantUtils.CreateFrom<bool>(ref flag3);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.StartCardPlay && args.Count == 2)
			{
				this.StartCardPlay(VariantUtils.ConvertTo<NHandCardHolder>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.SelectCardInSimpleMode && args.Count == 1)
			{
				this.SelectCardInSimpleMode(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.SelectCardInUpgradeMode && args.Count == 1)
			{
				this.SelectCardInUpgradeMode(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.DeselectCard && args.Count == 1)
			{
				this.DeselectCard(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.RevalidateSelectionAfterStateChange && args.Count == 0)
			{
				this.RevalidateSelectionAfterStateChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed && args.Count == 1)
			{
				this.OnSelectModeConfirmButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.RefreshSelectModeConfirmButton && args.Count == 0)
			{
				this.RefreshSelectModeConfirmButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AnimDisable && args.Count == 0)
			{
				this.AnimDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.AnimEnable && args.Count == 0)
			{
				this.AnimEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.FlashPlayableHolders && args.Count == 0)
			{
				this.FlashPlayableHolders();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnCardSelected && args.Count == 1)
			{
				this.OnCardSelected(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.OnCardDeselected && args.Count == 1)
			{
				this.OnCardDeselected(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.UpdateSelectedCardContainer && args.Count == 1)
			{
				this.UpdateSelectedCardContainer(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.EnableControllerNavigation && args.Count == 0)
			{
				this.EnableControllerNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName.DisableControllerNavigation && args.Count == 0)
			{
				this.DisableControllerNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPlayerHand.MethodName._UnhandledInput && args.Count == 1)
			{
				this._UnhandledInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060040D6 RID: 16598 RVA: 0x001F4A00 File Offset: 0x001F2C00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPlayerHand.MethodName._Ready || (ref method) == NPlayerHand.MethodName._EnterTree || (ref method) == NPlayerHand.MethodName._ExitTree || (ref method) == NPlayerHand.MethodName.IsAwaitingPlay || (ref method) == NPlayerHand.MethodName.Add || (ref method) == NPlayerHand.MethodName.AddCardHolder || (ref method) == NPlayerHand.MethodName.RemoveCardHolder || (ref method) == NPlayerHand.MethodName.OnHolderFocused || (ref method) == NPlayerHand.MethodName.OnHolderUnfocused || (ref method) == NPlayerHand.MethodName.CancelAllCardPlay || (ref method) == NPlayerHand.MethodName.ReturnHolderToHand || (ref method) == NPlayerHand.MethodName.ForceRefreshCardIndices || (ref method) == NPlayerHand.MethodName.RefreshLayout || (ref method) == NPlayerHand.MethodName.OnPeekButtonToggled || (ref method) == NPlayerHand.MethodName.UpdateSelectModeCardVisibility || (ref method) == NPlayerHand.MethodName.CancelHandSelectionIfNecessary || (ref method) == NPlayerHand.MethodName.OnHolderPressed || (ref method) == NPlayerHand.MethodName.CanPlayCards || (ref method) == NPlayerHand.MethodName.AreCardActionsAllowed || (ref method) == NPlayerHand.MethodName.StartCardPlay || (ref method) == NPlayerHand.MethodName.SelectCardInSimpleMode || (ref method) == NPlayerHand.MethodName.SelectCardInUpgradeMode || (ref method) == NPlayerHand.MethodName.DeselectCard || (ref method) == NPlayerHand.MethodName.RevalidateSelectionAfterStateChange || (ref method) == NPlayerHand.MethodName.OnSelectModeConfirmButtonPressed || (ref method) == NPlayerHand.MethodName.CheckIfSelectionComplete || (ref method) == NPlayerHand.MethodName.RefreshSelectModeConfirmButton || (ref method) == NPlayerHand.MethodName.AnimIn || (ref method) == NPlayerHand.MethodName.AnimOut || (ref method) == NPlayerHand.MethodName.AnimDisable || (ref method) == NPlayerHand.MethodName.AnimEnable || (ref method) == NPlayerHand.MethodName.FlashPlayableHolders || (ref method) == NPlayerHand.MethodName.OnCardSelected || (ref method) == NPlayerHand.MethodName.OnCardDeselected || (ref method) == NPlayerHand.MethodName.UpdateSelectedCardContainer || (ref method) == NPlayerHand.MethodName.EnableControllerNavigation || (ref method) == NPlayerHand.MethodName.DisableControllerNavigation || (ref method) == NPlayerHand.MethodName._UnhandledInput || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060040D7 RID: 16599 RVA: 0x001F4C50 File Offset: 0x001F2E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPlayerHand.PropertyName.CardHolderContainer)
			{
				this.CardHolderContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.PeekButton)
			{
				this.PeekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.CurrentMode)
			{
				this.CurrentMode = VariantUtils.ConvertTo<NPlayerHand.Mode>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.FocusedHolder)
			{
				this.FocusedHolder = VariantUtils.ConvertTo<NHandCardHolder>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectCardShortcuts)
			{
				this._selectCardShortcuts = VariantUtils.ConvertTo<StringName[]>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectModeBackstop)
			{
				this._selectModeBackstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._upgradePreviewContainer)
			{
				this._upgradePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectedHandCardContainer)
			{
				this._selectedHandCardContainer = VariantUtils.ConvertTo<NSelectedHandCardContainer>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._upgradePreview)
			{
				this._upgradePreview = VariantUtils.ConvertTo<NUpgradePreview>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectModeConfirmButton)
			{
				this._selectModeConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectionHeader)
			{
				this._selectionHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._currentCardPlay)
			{
				this._currentCardPlay = VariantUtils.ConvertTo<NCardPlay>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._currentMode)
			{
				this._currentMode = VariantUtils.ConvertTo<NPlayerHand.Mode>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._draggedHolderIndex)
			{
				this._draggedHolderIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._lastFocusedHolderIdx)
			{
				this._lastFocusedHolderIdx = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animEnableTween)
			{
				this._animEnableTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._isDisabled)
			{
				this._isDisabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animOutTween)
			{
				this._animOutTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectedCardScaleTween)
			{
				this._selectedCardScaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060040D8 RID: 16600 RVA: 0x001F4E84 File Offset: 0x001F3084
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPlayerHand.PropertyName.CardHolderContainer)
			{
				Control control = this.CardHolderContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.PeekButton)
			{
				NPeekButton peekButton = this.PeekButton;
				value = VariantUtils.CreateFrom<NPeekButton>(ref peekButton);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.InCardPlay)
			{
				bool flag = this.InCardPlay;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.IsInCardSelection)
			{
				bool flag = this.IsInCardSelection;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.CurrentMode)
			{
				NPlayerHand.Mode currentMode = this.CurrentMode;
				value = VariantUtils.CreateFrom<NPlayerHand.Mode>(ref currentMode);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.HasDraggedHolder)
			{
				bool flag = this.HasDraggedHolder;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.FocusedHolder)
			{
				NHandCardHolder focusedHolder = this.FocusedHolder;
				value = VariantUtils.CreateFrom<NHandCardHolder>(ref focusedHolder);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectCardShortcuts)
			{
				value = VariantUtils.CreateFrom<StringName[]>(ref this._selectCardShortcuts);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectModeBackstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._selectModeBackstop);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._upgradePreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._upgradePreviewContainer);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectedHandCardContainer)
			{
				value = VariantUtils.CreateFrom<NSelectedHandCardContainer>(ref this._selectedHandCardContainer);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._upgradePreview)
			{
				value = VariantUtils.CreateFrom<NUpgradePreview>(ref this._upgradePreview);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectModeConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._selectModeConfirmButton);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectionHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._selectionHeader);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._currentCardPlay)
			{
				value = VariantUtils.CreateFrom<NCardPlay>(ref this._currentCardPlay);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._currentMode)
			{
				value = VariantUtils.CreateFrom<NPlayerHand.Mode>(ref this._currentMode);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._draggedHolderIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._draggedHolderIndex);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._lastFocusedHolderIdx)
			{
				value = VariantUtils.CreateFrom<int>(ref this._lastFocusedHolderIdx);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animEnableTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animEnableTween);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._isDisabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDisabled);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._animOutTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animOutTween);
				return true;
			}
			if ((ref name) == NPlayerHand.PropertyName._selectedCardScaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._selectedCardScaleTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060040D9 RID: 16601 RVA: 0x001F51B4 File Offset: 0x001F33B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NPlayerHand.PropertyName._selectCardShortcuts, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName.CardHolderContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName.PeekButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._selectModeBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._upgradePreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._selectedHandCardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._upgradePreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._selectModeConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._selectionHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._currentCardPlay, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPlayerHand.PropertyName.InCardPlay, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPlayerHand.PropertyName.IsInCardSelection, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPlayerHand.PropertyName._currentMode, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPlayerHand.PropertyName.CurrentMode, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPlayerHand.PropertyName._draggedHolderIndex, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPlayerHand.PropertyName.HasDraggedHolder, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPlayerHand.PropertyName._lastFocusedHolderIdx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._animEnableTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPlayerHand.PropertyName._isDisabled, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._animOutTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName._selectedCardScaleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName.FocusedHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerHand.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060040DA RID: 16602 RVA: 0x001F54D8 File Offset: 0x001F36D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cardHolderContainer = NPlayerHand.PropertyName.CardHolderContainer;
			Control cardHolderContainer2 = this.CardHolderContainer;
			info.AddProperty(cardHolderContainer, Variant.From<Control>(ref cardHolderContainer2));
			StringName peekButton = NPlayerHand.PropertyName.PeekButton;
			NPeekButton peekButton2 = this.PeekButton;
			info.AddProperty(peekButton, Variant.From<NPeekButton>(ref peekButton2));
			StringName currentMode = NPlayerHand.PropertyName.CurrentMode;
			NPlayerHand.Mode currentMode2 = this.CurrentMode;
			info.AddProperty(currentMode, Variant.From<NPlayerHand.Mode>(ref currentMode2));
			StringName focusedHolder = NPlayerHand.PropertyName.FocusedHolder;
			NHandCardHolder focusedHolder2 = this.FocusedHolder;
			info.AddProperty(focusedHolder, Variant.From<NHandCardHolder>(ref focusedHolder2));
			info.AddProperty(NPlayerHand.PropertyName._selectCardShortcuts, Variant.From<StringName[]>(ref this._selectCardShortcuts));
			info.AddProperty(NPlayerHand.PropertyName._selectModeBackstop, Variant.From<Control>(ref this._selectModeBackstop));
			info.AddProperty(NPlayerHand.PropertyName._upgradePreviewContainer, Variant.From<Control>(ref this._upgradePreviewContainer));
			info.AddProperty(NPlayerHand.PropertyName._selectedHandCardContainer, Variant.From<NSelectedHandCardContainer>(ref this._selectedHandCardContainer));
			info.AddProperty(NPlayerHand.PropertyName._upgradePreview, Variant.From<NUpgradePreview>(ref this._upgradePreview));
			info.AddProperty(NPlayerHand.PropertyName._selectModeConfirmButton, Variant.From<NConfirmButton>(ref this._selectModeConfirmButton));
			info.AddProperty(NPlayerHand.PropertyName._selectionHeader, Variant.From<MegaRichTextLabel>(ref this._selectionHeader));
			info.AddProperty(NPlayerHand.PropertyName._currentCardPlay, Variant.From<NCardPlay>(ref this._currentCardPlay));
			info.AddProperty(NPlayerHand.PropertyName._currentMode, Variant.From<NPlayerHand.Mode>(ref this._currentMode));
			info.AddProperty(NPlayerHand.PropertyName._draggedHolderIndex, Variant.From<int>(ref this._draggedHolderIndex));
			info.AddProperty(NPlayerHand.PropertyName._lastFocusedHolderIdx, Variant.From<int>(ref this._lastFocusedHolderIdx));
			info.AddProperty(NPlayerHand.PropertyName._animEnableTween, Variant.From<Tween>(ref this._animEnableTween));
			info.AddProperty(NPlayerHand.PropertyName._isDisabled, Variant.From<bool>(ref this._isDisabled));
			info.AddProperty(NPlayerHand.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
			info.AddProperty(NPlayerHand.PropertyName._animOutTween, Variant.From<Tween>(ref this._animOutTween));
			info.AddProperty(NPlayerHand.PropertyName._selectedCardScaleTween, Variant.From<Tween>(ref this._selectedCardScaleTween));
			info.AddSignalEventDelegate(NPlayerHand.SignalName.ModeChanged, this.backing_ModeChanged);
		}

		/// <inheritdoc />
		// Token: 0x060040DB RID: 16603 RVA: 0x001F56C4 File Offset: 0x001F38C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPlayerHand.PropertyName.CardHolderContainer, ref variant))
			{
				this.CardHolderContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPlayerHand.PropertyName.PeekButton, ref variant2))
			{
				this.PeekButton = variant2.As<NPeekButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPlayerHand.PropertyName.CurrentMode, ref variant3))
			{
				this.CurrentMode = variant3.As<NPlayerHand.Mode>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPlayerHand.PropertyName.FocusedHolder, ref variant4))
			{
				this.FocusedHolder = variant4.As<NHandCardHolder>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectCardShortcuts, ref variant5))
			{
				this._selectCardShortcuts = variant5.As<StringName[]>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectModeBackstop, ref variant6))
			{
				this._selectModeBackstop = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPlayerHand.PropertyName._upgradePreviewContainer, ref variant7))
			{
				this._upgradePreviewContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectedHandCardContainer, ref variant8))
			{
				this._selectedHandCardContainer = variant8.As<NSelectedHandCardContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NPlayerHand.PropertyName._upgradePreview, ref variant9))
			{
				this._upgradePreview = variant9.As<NUpgradePreview>();
			}
			Variant variant10;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectModeConfirmButton, ref variant10))
			{
				this._selectModeConfirmButton = variant10.As<NConfirmButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectionHeader, ref variant11))
			{
				this._selectionHeader = variant11.As<MegaRichTextLabel>();
			}
			Variant variant12;
			if (info.TryGetProperty(NPlayerHand.PropertyName._currentCardPlay, ref variant12))
			{
				this._currentCardPlay = variant12.As<NCardPlay>();
			}
			Variant variant13;
			if (info.TryGetProperty(NPlayerHand.PropertyName._currentMode, ref variant13))
			{
				this._currentMode = variant13.As<NPlayerHand.Mode>();
			}
			Variant variant14;
			if (info.TryGetProperty(NPlayerHand.PropertyName._draggedHolderIndex, ref variant14))
			{
				this._draggedHolderIndex = variant14.As<int>();
			}
			Variant variant15;
			if (info.TryGetProperty(NPlayerHand.PropertyName._lastFocusedHolderIdx, ref variant15))
			{
				this._lastFocusedHolderIdx = variant15.As<int>();
			}
			Variant variant16;
			if (info.TryGetProperty(NPlayerHand.PropertyName._animEnableTween, ref variant16))
			{
				this._animEnableTween = variant16.As<Tween>();
			}
			Variant variant17;
			if (info.TryGetProperty(NPlayerHand.PropertyName._isDisabled, ref variant17))
			{
				this._isDisabled = variant17.As<bool>();
			}
			Variant variant18;
			if (info.TryGetProperty(NPlayerHand.PropertyName._animInTween, ref variant18))
			{
				this._animInTween = variant18.As<Tween>();
			}
			Variant variant19;
			if (info.TryGetProperty(NPlayerHand.PropertyName._animOutTween, ref variant19))
			{
				this._animOutTween = variant19.As<Tween>();
			}
			Variant variant20;
			if (info.TryGetProperty(NPlayerHand.PropertyName._selectedCardScaleTween, ref variant20))
			{
				this._selectedCardScaleTween = variant20.As<Tween>();
			}
			NPlayerHand.ModeChangedEventHandler modeChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NPlayerHand.ModeChangedEventHandler>(NPlayerHand.SignalName.ModeChanged, ref modeChangedEventHandler))
			{
				this.backing_ModeChanged = modeChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060040DC RID: 16604 RVA: 0x001F5920 File Offset: 0x001F3B20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NPlayerHand.SignalName.ModeChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NPlayerHand.ModeChangedEventHandler" />
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x060040DD RID: 16605 RVA: 0x001F5964 File Offset: 0x001F3B64
		// (remove) Token: 0x060040DE RID: 16606 RVA: 0x001F597D File Offset: 0x001F3B7D
		[Nullable(0)]
		public event NPlayerHand.ModeChangedEventHandler ModeChanged
		{
			[NullableContext(0)]
			add
			{
				this.backing_ModeChanged = (NPlayerHand.ModeChangedEventHandler)Delegate.Combine(this.backing_ModeChanged, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_ModeChanged = (NPlayerHand.ModeChangedEventHandler)Delegate.Remove(this.backing_ModeChanged, value);
			}
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x001F5996 File Offset: 0x001F3B96
		protected void EmitSignalModeChanged()
		{
			base.EmitSignal(NPlayerHand.SignalName.ModeChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060040E0 RID: 16608 RVA: 0x001F59A9 File Offset: 0x001F3BA9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NPlayerHand.SignalName.ModeChanged) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NPlayerHand.ModeChangedEventHandler modeChangedEventHandler = this.backing_ModeChanged;
			if (modeChangedEventHandler == null)
			{
				return;
			}
			modeChangedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x060040E1 RID: 16609 RVA: 0x001F59DA File Offset: 0x001F3BDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NPlayerHand.SignalName.ModeChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001922 RID: 6434
		private StringName[] _selectCardShortcuts = new StringName[]
		{
			MegaInput.selectCard1,
			MegaInput.selectCard2,
			MegaInput.selectCard3,
			MegaInput.selectCard4,
			MegaInput.selectCard5,
			MegaInput.selectCard6,
			MegaInput.selectCard7,
			MegaInput.selectCard8,
			MegaInput.selectCard9,
			MegaInput.selectCard10
		};

		// Token: 0x04001925 RID: 6437
		private Control _selectModeBackstop;

		// Token: 0x04001926 RID: 6438
		private readonly List<CardModel> _selectedCards = new List<CardModel>();

		// Token: 0x04001927 RID: 6439
		private CardSelectorPrefs _prefs;

		// Token: 0x04001928 RID: 6440
		[Nullable(new byte[] { 2, 1, 1 })]
		private TaskCompletionSource<IEnumerable<CardModel>> _selectionCompletionSource;

		// Token: 0x04001929 RID: 6441
		private Control _upgradePreviewContainer;

		// Token: 0x0400192A RID: 6442
		private NSelectedHandCardContainer _selectedHandCardContainer;

		// Token: 0x0400192B RID: 6443
		private NUpgradePreview _upgradePreview;

		// Token: 0x0400192C RID: 6444
		private NConfirmButton _selectModeConfirmButton;

		// Token: 0x0400192D RID: 6445
		private MegaRichTextLabel _selectionHeader;

		// Token: 0x0400192E RID: 6446
		[Nullable(2)]
		private NCardPlay _currentCardPlay;

		// Token: 0x0400192F RID: 6447
		[Nullable(2)]
		private CombatState _combatState;

		// Token: 0x04001930 RID: 6448
		private NPlayerHand.Mode _currentMode = NPlayerHand.Mode.Play;

		// Token: 0x04001931 RID: 6449
		[Nullable(new byte[] { 2, 1 })]
		private Func<CardModel, bool> _currentSelectionFilter;

		/// <summary>
		/// If you're dragging a card, it won't be a child of the card holder container for the duration of the drag,
		/// so we need to leave an empty space for it in your hand. This index represents where that empty space should be.
		///
		/// For example, let's say you start dragging the card in your hand at index 2.
		/// Once it's removed from your hand for the drag, the card at index 3 moves to index 2, but visually, we
		/// still want to position it at index 3 so we can "leave a space" at index 2.
		/// However, the position/angle of the cards at indexes 0 and 1 should be unchanged.
		/// </summary>
		// Token: 0x04001932 RID: 6450
		private int _draggedHolderIndex = -1;

		// Token: 0x04001933 RID: 6451
		private int _lastFocusedHolderIdx = -1;

		/// <summary>
		/// There are two types of cards that are considered "awaiting play":
		/// 1. The card you're currently dragging and preparing to play.
		/// 2. If you've queued up a bunch of cards for play quickly, all the cards that haven't started playing yet.
		///    For example, if I play a Defend, then quickly play 2 Strikes while the Defend's animations are still running,
		///    the 2 Strikes are considered "awaiting play" for that time.
		///
		/// Cards that are awaiting play exist in a sort of "purgatory" state. From a back-end perspective, they're still in
		/// your hand (they count towards hand size, etc.), but on the front-end, they should be floating around somewhere
		/// separate from the other cards in your hand.
		///
		/// When a holder returns to the hand it is re-inserted at the position matching its model's order in the backend
		/// Hand pile (see <see cref="M:MegaCrit.Sts2.Core.Nodes.Combat.NPlayerHand.GetHandInsertIndex(MegaCrit.Sts2.Core.Models.CardModel)" />), so we do not need to remember its old child index here.
		/// </summary>
		// Token: 0x04001934 RID: 6452
		private readonly HashSet<NHandCardHolder> _holdersAwaitingQueue = new HashSet<NHandCardHolder>();

		// Token: 0x04001935 RID: 6453
		[Nullable(2)]
		private Tween _animEnableTween;

		// Token: 0x04001936 RID: 6454
		private bool _isDisabled;

		// Token: 0x04001937 RID: 6455
		private const double _enableDisableDuration = 0.2;

		// Token: 0x04001938 RID: 6456
		private static readonly Vector2 _disablePosition = new Vector2(0f, 100f);

		// Token: 0x04001939 RID: 6457
		private static readonly Color _disableModulate = StsColors.gray;

		// Token: 0x0400193A RID: 6458
		[Nullable(2)]
		private Tween _animInTween;

		// Token: 0x0400193B RID: 6459
		[Nullable(2)]
		private Tween _animOutTween;

		// Token: 0x0400193C RID: 6460
		[Nullable(2)]
		private Tween _selectedCardScaleTween;

		// Token: 0x0400193D RID: 6461
		private const float _showHideAnimDuration = 0.8f;

		// Token: 0x0400193E RID: 6462
		private static readonly Vector2 _showPosition = Vector2.Zero;

		// Token: 0x0400193F RID: 6463
		private static readonly Vector2 _hidePosition = new Vector2(0f, 500f);

		// Token: 0x04001941 RID: 6465
		[Nullable(0)]
		private NPlayerHand.ModeChangedEventHandler backing_ModeChanged;

		// Token: 0x02001908 RID: 6408
		// (Invoke) Token: 0x0600A9DE RID: 43486
		[NullableContext(0)]
		[Signal]
		public delegate void ModeChangedEventHandler();

		// Token: 0x02001909 RID: 6409
		[NullableContext(0)]
		public enum Mode
		{
			// Token: 0x0400625F RID: 25183
			None,
			// Token: 0x04006260 RID: 25184
			Play,
			// Token: 0x04006261 RID: 25185
			SimpleSelect,
			// Token: 0x04006262 RID: 25186
			UpgradeSelect
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200190A RID: 6410
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006263 RID: 25187
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006264 RID: 25188
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006265 RID: 25189
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'IsAwaitingPlay' method.
			/// </summary>
			// Token: 0x04006266 RID: 25190
			public static readonly StringName IsAwaitingPlay = "IsAwaitingPlay";

			/// <summary>
			/// Cached name for the 'Add' method.
			/// </summary>
			// Token: 0x04006267 RID: 25191
			public static readonly StringName Add = "Add";

			/// <summary>
			/// Cached name for the 'AddCardHolder' method.
			/// </summary>
			// Token: 0x04006268 RID: 25192
			public static readonly StringName AddCardHolder = "AddCardHolder";

			/// <summary>
			/// Cached name for the 'RemoveCardHolder' method.
			/// </summary>
			// Token: 0x04006269 RID: 25193
			public static readonly StringName RemoveCardHolder = "RemoveCardHolder";

			/// <summary>
			/// Cached name for the 'OnHolderFocused' method.
			/// </summary>
			// Token: 0x0400626A RID: 25194
			public static readonly StringName OnHolderFocused = "OnHolderFocused";

			/// <summary>
			/// Cached name for the 'OnHolderUnfocused' method.
			/// </summary>
			// Token: 0x0400626B RID: 25195
			public static readonly StringName OnHolderUnfocused = "OnHolderUnfocused";

			/// <summary>
			/// Cached name for the 'CancelAllCardPlay' method.
			/// </summary>
			// Token: 0x0400626C RID: 25196
			public static readonly StringName CancelAllCardPlay = "CancelAllCardPlay";

			/// <summary>
			/// Cached name for the 'ReturnHolderToHand' method.
			/// </summary>
			// Token: 0x0400626D RID: 25197
			public static readonly StringName ReturnHolderToHand = "ReturnHolderToHand";

			/// <summary>
			/// Cached name for the 'ForceRefreshCardIndices' method.
			/// </summary>
			// Token: 0x0400626E RID: 25198
			public static readonly StringName ForceRefreshCardIndices = "ForceRefreshCardIndices";

			/// <summary>
			/// Cached name for the 'RefreshLayout' method.
			/// </summary>
			// Token: 0x0400626F RID: 25199
			public static readonly StringName RefreshLayout = "RefreshLayout";

			/// <summary>
			/// Cached name for the 'OnPeekButtonToggled' method.
			/// </summary>
			// Token: 0x04006270 RID: 25200
			public static readonly StringName OnPeekButtonToggled = "OnPeekButtonToggled";

			/// <summary>
			/// Cached name for the 'UpdateSelectModeCardVisibility' method.
			/// </summary>
			// Token: 0x04006271 RID: 25201
			public static readonly StringName UpdateSelectModeCardVisibility = "UpdateSelectModeCardVisibility";

			/// <summary>
			/// Cached name for the 'CancelHandSelectionIfNecessary' method.
			/// </summary>
			// Token: 0x04006272 RID: 25202
			public static readonly StringName CancelHandSelectionIfNecessary = "CancelHandSelectionIfNecessary";

			/// <summary>
			/// Cached name for the 'OnHolderPressed' method.
			/// </summary>
			// Token: 0x04006273 RID: 25203
			public static readonly StringName OnHolderPressed = "OnHolderPressed";

			/// <summary>
			/// Cached name for the 'CanPlayCards' method.
			/// </summary>
			// Token: 0x04006274 RID: 25204
			public static readonly StringName CanPlayCards = "CanPlayCards";

			/// <summary>
			/// Cached name for the 'AreCardActionsAllowed' method.
			/// </summary>
			// Token: 0x04006275 RID: 25205
			public static readonly StringName AreCardActionsAllowed = "AreCardActionsAllowed";

			/// <summary>
			/// Cached name for the 'StartCardPlay' method.
			/// </summary>
			// Token: 0x04006276 RID: 25206
			public static readonly StringName StartCardPlay = "StartCardPlay";

			/// <summary>
			/// Cached name for the 'SelectCardInSimpleMode' method.
			/// </summary>
			// Token: 0x04006277 RID: 25207
			public static readonly StringName SelectCardInSimpleMode = "SelectCardInSimpleMode";

			/// <summary>
			/// Cached name for the 'SelectCardInUpgradeMode' method.
			/// </summary>
			// Token: 0x04006278 RID: 25208
			public static readonly StringName SelectCardInUpgradeMode = "SelectCardInUpgradeMode";

			/// <summary>
			/// Cached name for the 'DeselectCard' method.
			/// </summary>
			// Token: 0x04006279 RID: 25209
			public static readonly StringName DeselectCard = "DeselectCard";

			/// <summary>
			/// Cached name for the 'RevalidateSelectionAfterStateChange' method.
			/// </summary>
			// Token: 0x0400627A RID: 25210
			public static readonly StringName RevalidateSelectionAfterStateChange = "RevalidateSelectionAfterStateChange";

			/// <summary>
			/// Cached name for the 'OnSelectModeConfirmButtonPressed' method.
			/// </summary>
			// Token: 0x0400627B RID: 25211
			public static readonly StringName OnSelectModeConfirmButtonPressed = "OnSelectModeConfirmButtonPressed";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x0400627C RID: 25212
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";

			/// <summary>
			/// Cached name for the 'RefreshSelectModeConfirmButton' method.
			/// </summary>
			// Token: 0x0400627D RID: 25213
			public static readonly StringName RefreshSelectModeConfirmButton = "RefreshSelectModeConfirmButton";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x0400627E RID: 25214
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x0400627F RID: 25215
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'AnimDisable' method.
			/// </summary>
			// Token: 0x04006280 RID: 25216
			public static readonly StringName AnimDisable = "AnimDisable";

			/// <summary>
			/// Cached name for the 'AnimEnable' method.
			/// </summary>
			// Token: 0x04006281 RID: 25217
			public static readonly StringName AnimEnable = "AnimEnable";

			/// <summary>
			/// Cached name for the 'FlashPlayableHolders' method.
			/// </summary>
			// Token: 0x04006282 RID: 25218
			public static readonly StringName FlashPlayableHolders = "FlashPlayableHolders";

			/// <summary>
			/// Cached name for the 'OnCardSelected' method.
			/// </summary>
			// Token: 0x04006283 RID: 25219
			public static readonly StringName OnCardSelected = "OnCardSelected";

			/// <summary>
			/// Cached name for the 'OnCardDeselected' method.
			/// </summary>
			// Token: 0x04006284 RID: 25220
			public static readonly StringName OnCardDeselected = "OnCardDeselected";

			/// <summary>
			/// Cached name for the 'UpdateSelectedCardContainer' method.
			/// </summary>
			// Token: 0x04006285 RID: 25221
			public static readonly StringName UpdateSelectedCardContainer = "UpdateSelectedCardContainer";

			/// <summary>
			/// Cached name for the 'EnableControllerNavigation' method.
			/// </summary>
			// Token: 0x04006286 RID: 25222
			public static readonly StringName EnableControllerNavigation = "EnableControllerNavigation";

			/// <summary>
			/// Cached name for the 'DisableControllerNavigation' method.
			/// </summary>
			// Token: 0x04006287 RID: 25223
			public static readonly StringName DisableControllerNavigation = "DisableControllerNavigation";

			/// <summary>
			/// Cached name for the '_UnhandledInput' method.
			/// </summary>
			// Token: 0x04006288 RID: 25224
			public static readonly StringName _UnhandledInput = "_UnhandledInput";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200190B RID: 6411
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CardHolderContainer' property.
			/// </summary>
			// Token: 0x04006289 RID: 25225
			public static readonly StringName CardHolderContainer = "CardHolderContainer";

			/// <summary>
			/// Cached name for the 'PeekButton' property.
			/// </summary>
			// Token: 0x0400628A RID: 25226
			public static readonly StringName PeekButton = "PeekButton";

			/// <summary>
			/// Cached name for the 'InCardPlay' property.
			/// </summary>
			// Token: 0x0400628B RID: 25227
			public static readonly StringName InCardPlay = "InCardPlay";

			/// <summary>
			/// Cached name for the 'IsInCardSelection' property.
			/// </summary>
			// Token: 0x0400628C RID: 25228
			public static readonly StringName IsInCardSelection = "IsInCardSelection";

			/// <summary>
			/// Cached name for the 'CurrentMode' property.
			/// </summary>
			// Token: 0x0400628D RID: 25229
			public static readonly StringName CurrentMode = "CurrentMode";

			/// <summary>
			/// Cached name for the 'HasDraggedHolder' property.
			/// </summary>
			// Token: 0x0400628E RID: 25230
			public static readonly StringName HasDraggedHolder = "HasDraggedHolder";

			/// <summary>
			/// Cached name for the 'FocusedHolder' property.
			/// </summary>
			// Token: 0x0400628F RID: 25231
			public static readonly StringName FocusedHolder = "FocusedHolder";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04006290 RID: 25232
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_selectCardShortcuts' field.
			/// </summary>
			// Token: 0x04006291 RID: 25233
			public static readonly StringName _selectCardShortcuts = "_selectCardShortcuts";

			/// <summary>
			/// Cached name for the '_selectModeBackstop' field.
			/// </summary>
			// Token: 0x04006292 RID: 25234
			public static readonly StringName _selectModeBackstop = "_selectModeBackstop";

			/// <summary>
			/// Cached name for the '_upgradePreviewContainer' field.
			/// </summary>
			// Token: 0x04006293 RID: 25235
			public static readonly StringName _upgradePreviewContainer = "_upgradePreviewContainer";

			/// <summary>
			/// Cached name for the '_selectedHandCardContainer' field.
			/// </summary>
			// Token: 0x04006294 RID: 25236
			public static readonly StringName _selectedHandCardContainer = "_selectedHandCardContainer";

			/// <summary>
			/// Cached name for the '_upgradePreview' field.
			/// </summary>
			// Token: 0x04006295 RID: 25237
			public static readonly StringName _upgradePreview = "_upgradePreview";

			/// <summary>
			/// Cached name for the '_selectModeConfirmButton' field.
			/// </summary>
			// Token: 0x04006296 RID: 25238
			public static readonly StringName _selectModeConfirmButton = "_selectModeConfirmButton";

			/// <summary>
			/// Cached name for the '_selectionHeader' field.
			/// </summary>
			// Token: 0x04006297 RID: 25239
			public static readonly StringName _selectionHeader = "_selectionHeader";

			/// <summary>
			/// Cached name for the '_currentCardPlay' field.
			/// </summary>
			// Token: 0x04006298 RID: 25240
			public static readonly StringName _currentCardPlay = "_currentCardPlay";

			/// <summary>
			/// Cached name for the '_currentMode' field.
			/// </summary>
			// Token: 0x04006299 RID: 25241
			public static readonly StringName _currentMode = "_currentMode";

			/// <summary>
			/// Cached name for the '_draggedHolderIndex' field.
			/// </summary>
			// Token: 0x0400629A RID: 25242
			public static readonly StringName _draggedHolderIndex = "_draggedHolderIndex";

			/// <summary>
			/// Cached name for the '_lastFocusedHolderIdx' field.
			/// </summary>
			// Token: 0x0400629B RID: 25243
			public static readonly StringName _lastFocusedHolderIdx = "_lastFocusedHolderIdx";

			/// <summary>
			/// Cached name for the '_animEnableTween' field.
			/// </summary>
			// Token: 0x0400629C RID: 25244
			public static readonly StringName _animEnableTween = "_animEnableTween";

			/// <summary>
			/// Cached name for the '_isDisabled' field.
			/// </summary>
			// Token: 0x0400629D RID: 25245
			public static readonly StringName _isDisabled = "_isDisabled";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x0400629E RID: 25246
			public static readonly StringName _animInTween = "_animInTween";

			/// <summary>
			/// Cached name for the '_animOutTween' field.
			/// </summary>
			// Token: 0x0400629F RID: 25247
			public static readonly StringName _animOutTween = "_animOutTween";

			/// <summary>
			/// Cached name for the '_selectedCardScaleTween' field.
			/// </summary>
			// Token: 0x040062A0 RID: 25248
			public static readonly StringName _selectedCardScaleTween = "_selectedCardScaleTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200190C RID: 6412
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'ModeChanged' signal.
			/// </summary>
			// Token: 0x040062A1 RID: 25249
			public static readonly StringName ModeChanged = "ModeChanged";
		}
	}
}
