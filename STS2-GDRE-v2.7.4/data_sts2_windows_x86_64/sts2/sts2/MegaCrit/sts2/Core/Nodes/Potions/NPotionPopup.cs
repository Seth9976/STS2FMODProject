using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x02000381 RID: 897
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Potions/NPotionPopup.cs")]
	public class NPotionPopup : Control
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x0018DF88 File Offset: 0x0018C188
		[Nullable(2)]
		private PotionModel Potion
		{
			[NullableContext(2)]
			get
			{
				NPotion potion = this._holder.Potion;
				if (potion == null)
				{
					return null;
				}
				return potion.Model;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600355B RID: 13659 RVA: 0x0018DFA0 File Offset: 0x0018C1A0
		public bool IsUsable
		{
			get
			{
				return this._useButton.IsEnabled;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x0600355C RID: 13660 RVA: 0x0018DFAD File Offset: 0x0018C1AD
		// (set) Token: 0x0600355D RID: 13661 RVA: 0x0018DFB5 File Offset: 0x0018C1B5
		public bool IsMarkedForRemoval { get; private set; }

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x0018DFBE File Offset: 0x0018C1BE
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("potions/potion_popup");
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600355F RID: 13663 RVA: 0x0018DFCA File Offset: 0x0018C1CA
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPotionPopup.ScenePath);
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x0018DFD8 File Offset: 0x0018C1D8
		public static NPotionPopup Create(NPotionHolder holder)
		{
			NPotionPopup npotionPopup = PreloadManager.Cache.GetScene(NPotionPopup.ScenePath).Instantiate<NPotionPopup>(0L);
			npotionPopup._holder = holder;
			return npotionPopup;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x0018E004 File Offset: 0x0018C204
		public override void _Ready()
		{
			base.GlobalPosition = this._holder.GlobalPosition + Vector2.Down * this._holder.Size.Y * 1.5f + Vector2.Right * this._holder.Size * 0.5f + Vector2.Left * base.Size * 0.5f;
			this._hoverTipBounds = base.GetNode<Control>("%HoverTipBounds");
			NHoverTipSet.CreateAndShow(this._hoverTipBounds, this._holder.Potion.Model.HoverTips, HoverTipAlignment.Right);
			NHoverTipSet.shouldBlockHoverTips = true;
			this._popupContainer = base.GetNode<Control>("%Container");
			this._useButton = base.GetNode<NPotionPopupButton>("%UseButton");
			this._useButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUseButtonPressed)), 0U);
			this._discardButton = base.GetNode<NPotionPopupButton>("%DiscardButton");
			this._discardButton.SetLocKey("POTION_POPUP.discard");
			this._discardButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDiscardButtonPressed)), 0U);
			this._useButton.FocusNeighborLeft = this._useButton.GetPath();
			this._useButton.FocusNeighborRight = this._useButton.GetPath();
			this._useButton.FocusNeighborTop = this._useButton.GetPath();
			this._useButton.FocusNeighborBottom = this._discardButton.GetPath();
			this._discardButton.FocusNeighborLeft = this._discardButton.GetPath();
			this._discardButton.FocusNeighborRight = this._discardButton.GetPath();
			this._discardButton.FocusNeighborTop = this._useButton.GetPath();
			this._discardButton.FocusNeighborBottom = this._discardButton.GetPath();
			if (this.Potion == null || this.Potion.IsQueued || this.Potion.Owner.Creature.IsDead)
			{
				this._useButton.Disable();
				this._discardButton.Disable();
			}
			else
			{
				switch (this.Potion.Usage)
				{
				case PotionUsage.None:
					throw new InvalidOperationException("No potions should have 'None' usage.");
				case PotionUsage.CombatOnly:
					CombatManager.Instance.StateTracker.CombatStateChanged += this.OnCombatStateChanged;
					CombatManager.Instance.TurnStarted += this.OnTurnStarted;
					CombatManager.Instance.PlayerEndedTurn += this.OnPlayerEndTurnStatusChanged;
					CombatManager.Instance.PlayerUnendedTurn += this.OnPlayerEndTurnStatusChanged;
					if (NOverlayStack.Instance != null)
					{
						NOverlayStack.Instance.Changed += this.Remove;
					}
					else
					{
						Log.Warn("NOverlayStack.Instance was null when creating potion popup", 2);
					}
					if (NCapstoneContainer.Instance != null)
					{
						NCapstoneContainer.Instance.Changed += this.Remove;
					}
					else
					{
						Log.Warn("NCapstoneContainer.Instance was null when creating potion popup", 2);
					}
					this.RefreshButtons();
					break;
				case PotionUsage.AnyTime:
					this._useButton.Enable();
					break;
				case PotionUsage.Automatic:
					this._useButton.Disable();
					break;
				default:
					throw new ArgumentOutOfRangeException("Usage");
				}
				this._subscribedPlayer = this.Potion.Owner;
				this._subscribedPlayer.CanUseOrRemovePotionsChanged += this.RefreshButtons;
				if (!this.Potion.Owner.CanUseOrRemovePotions)
				{
					this._useButton.Disable();
					this._discardButton.Disable();
				}
				if (!this.Potion.PassesCustomUsabilityCheck)
				{
					this._useButton.Disable();
				}
				if (this._useButton.IsEnabled)
				{
					this._useButton.TryGrabFocus();
				}
				else if (this._discardButton.IsEnabled)
				{
					this._discardButton.TryGrabFocus();
				}
				else
				{
					this.TryGrabFocus();
				}
			}
			string text;
			if (this.Potion == null)
			{
				text = "POTION_POPUP.drink";
			}
			else
			{
				TargetType targetType = this.Potion.TargetType;
				bool flag = targetType - TargetType.AnyEnemy <= 1 || targetType == TargetType.TargetedNoCreature;
				if (flag || this.Potion.CanThrowAtAlly())
				{
					text = "POTION_POPUP.throw";
				}
				else
				{
					text = "POTION_POPUP.drink";
				}
			}
			this._useButton.SetLocKey(text);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Modulate = Colors.Transparent;
			this._popupContainer.Position += Vector2.Up * 25f;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.10000000149011612).SetTrans(1L);
			this._tween.TweenProperty(this._popupContainer, "position:y", this._popupContainer.Position.Y + 25f, 0.15000000596046448).SetEase(0L).SetTrans(1L);
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x0018E53A File Offset: 0x0018C73A
		private void OnUseButtonPressed(NButton _)
		{
			TaskHelper.RunSafely(this.UsePotion());
			this.Remove();
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x0018E550 File Offset: 0x0018C750
		private async Task UsePotion()
		{
			if (this.Potion != null)
			{
				PotionModel potion = this.Potion;
				potion.BeforeUse += this.<UsePotion>g__DisableHolder|22_0;
				try
				{
					await this._holder.UsePotion();
				}
				finally
				{
					potion.BeforeUse -= this.<UsePotion>g__DisableHolder|22_0;
				}
			}
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x0018E594 File Offset: 0x0018C794
		private void OnDiscardButtonPressed(NButton _)
		{
			if (this.Potion == null)
			{
				return;
			}
			Player owner = this.Potion.Owner;
			int num = owner.PotionSlots.IndexOf(this._holder.Potion.Model);
			if (num < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to discard potion ");
				defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(this._holder.Potion.Model);
				defaultInterpolatedStringHandler.AppendLiteral(" but it's not in the player's belt!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._holder.DisableUntilPotionRemoved();
			DiscardPotionGameAction discardPotionGameAction = new DiscardPotionGameAction(owner, (uint)num, CombatManager.Instance.IsInProgress);
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(discardPotionGameAction);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x0018E64C File Offset: 0x0018C84C
		public override void _Input(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				bool flag = buttonIndex - 1L <= 1L;
				if (flag && inputEventMouseButton.IsReleased() && !base.GetGlobalRect().HasPoint(base.GetGlobalMousePosition()))
				{
					this.Remove();
					return;
				}
			}
			else if (inputEvent.IsActionPressed(MegaInput.cancel, false, false))
			{
				this.Remove();
				Viewport viewport = base.GetViewport();
				if (viewport != null)
				{
					viewport.SetInputAsHandled();
				}
				this._holder.TryGrabFocus();
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x0018E6D1 File Offset: 0x0018C8D1
		public override void _ExitTree()
		{
			if (!this.IsMarkedForRemoval)
			{
				this.IsMarkedForRemoval = true;
				NHoverTipSet.shouldBlockHoverTips = false;
				NHoverTipSet.Remove(this._hoverTipBounds);
				this.DisconnectSignals();
			}
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0018E70C File Offset: 0x0018C90C
		public void Remove()
		{
			if (this.IsMarkedForRemoval)
			{
				return;
			}
			this.IsMarkedForRemoval = true;
			NHoverTipSet.shouldBlockHoverTips = false;
			NHoverTipSet.Remove(this._hoverTipBounds);
			this.DisconnectSignals();
			this._useButton.Disable();
			this._discardButton.Disable();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate", Colors.Transparent, 0.10000000149011612).SetTrans(1L);
			this._tween.TweenProperty(this._popupContainer, "position:y", -25f, 0.20000000298023224).SetEase(1L).SetTrans(1L);
			this._tween.Chain().TweenCallback(Callable.From(new Action(base.QueueFreeSafely)));
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x0018E80C File Offset: 0x0018CA0C
		private void DisconnectSignals()
		{
			CombatManager.Instance.StateTracker.CombatStateChanged -= this.OnCombatStateChanged;
			CombatManager.Instance.TurnStarted -= this.OnTurnStarted;
			CombatManager.Instance.PlayerEndedTurn -= this.OnPlayerEndTurnStatusChanged;
			CombatManager.Instance.PlayerUnendedTurn -= this.OnPlayerEndTurnStatusChanged;
			if (this._subscribedPlayer != null)
			{
				this._subscribedPlayer.CanUseOrRemovePotionsChanged -= this.RefreshButtons;
			}
			if (NOverlayStack.Instance != null)
			{
				NOverlayStack.Instance.Changed -= this.Remove;
			}
			if (NCapstoneContainer.Instance != null)
			{
				NCapstoneContainer.Instance.Changed -= this.Remove;
			}
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x0018E8CF File Offset: 0x0018CACF
		private void OnTurnStarted(CombatState _)
		{
			this.RefreshButtons();
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0018E8D7 File Offset: 0x0018CAD7
		private void OnPlayerEndTurnStatusChanged(Player _, bool __)
		{
			this.RefreshButtons();
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x0018E8DF File Offset: 0x0018CADF
		private void OnPlayerEndTurnStatusChanged(Player _)
		{
			this.RefreshButtons();
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x0018E8E7 File Offset: 0x0018CAE7
		private void OnCombatStateChanged(CombatState _)
		{
			this.RefreshButtons();
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0018E8F0 File Offset: 0x0018CAF0
		private void RefreshButtons()
		{
			if (this.IsMarkedForRemoval)
			{
				return;
			}
			PotionModel potion = this.Potion;
			Creature creature = ((potion != null) ? potion.Owner.Creature : null);
			this._discardButton.Enable();
			PotionModel potion2 = this.Potion;
			if (potion2 != null && potion2.Usage == PotionUsage.AnyTime)
			{
				this._useButton.Enable();
			}
			else
			{
				if (creature != null && CombatManager.Instance.IsInProgress)
				{
					ICombatState combatState = creature.CombatState;
					CombatSide? combatSide = ((combatState != null) ? new CombatSide?(combatState.CurrentSide) : null);
					CombatSide side = creature.Side;
					if (((combatSide.GetValueOrDefault() == side) & (combatSide != null)) && creature.IsAlive && !this.InACardSelectScreen && !CombatManager.Instance.PlayerActionsDisabled)
					{
						this._useButton.Enable();
						goto IL_00CE;
					}
				}
				this._useButton.Disable();
			}
			IL_00CE:
			PotionModel potion3 = this.Potion;
			if (potion3 != null)
			{
				Player owner = potion3.Owner;
				if (owner != null && !owner.CanUseOrRemovePotions)
				{
					this._useButton.Disable();
					this._discardButton.Disable();
				}
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x0018EA03 File Offset: 0x0018CC03
		private bool InACardSelectScreen
		{
			get
			{
				NPlayerHand instance = NPlayerHand.Instance;
				if (instance == null || !instance.IsInCardSelection)
				{
					NOverlayStack instance2 = NOverlayStack.Instance;
					return ((instance2 != null) ? instance2.Peek() : null) is ICardSelector;
				}
				return true;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600356F RID: 13679 RVA: 0x0018EA34 File Offset: 0x0018CC34
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NPotionPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopup.MethodName.OnUseButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionPopup.MethodName.OnDiscardButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPotionPopup.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NPotionPopup.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopup.MethodName.Remove, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopup.MethodName.DisconnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopup.MethodName.RefreshButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003570 RID: 13680 RVA: 0x0018ECC0 File Offset: 0x0018CEC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionPopup.MethodName.Create && args.Count == 1)
			{
				NPotionPopup npotionPopup = NPotionPopup.Create(VariantUtils.ConvertTo<NPotionHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionPopup>(ref npotionPopup);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName.OnUseButtonPressed && args.Count == 1)
			{
				this.OnUseButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName.OnDiscardButtonPressed && args.Count == 1)
			{
				this.OnDiscardButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName.Remove && args.Count == 0)
			{
				this.Remove();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName.DisconnectSignals && args.Count == 0)
			{
				this.DisconnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopup.MethodName.RefreshButtons && args.Count == 0)
			{
				this.RefreshButtons();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x0018EE64 File Offset: 0x0018D064
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionPopup.MethodName.Create && args.Count == 1)
			{
				NPotionPopup npotionPopup = NPotionPopup.Create(VariantUtils.ConvertTo<NPotionHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NPotionPopup>(ref npotionPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003572 RID: 13682 RVA: 0x0018EEB4 File Offset: 0x0018D0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionPopup.MethodName.Create || (ref method) == NPotionPopup.MethodName._Ready || (ref method) == NPotionPopup.MethodName.OnUseButtonPressed || (ref method) == NPotionPopup.MethodName.OnDiscardButtonPressed || (ref method) == NPotionPopup.MethodName._Input || (ref method) == NPotionPopup.MethodName._ExitTree || (ref method) == NPotionPopup.MethodName.Remove || (ref method) == NPotionPopup.MethodName.DisconnectSignals || (ref method) == NPotionPopup.MethodName.RefreshButtons || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003573 RID: 13683 RVA: 0x0018EF50 File Offset: 0x0018D150
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionPopup.PropertyName.IsMarkedForRemoval)
			{
				this.IsMarkedForRemoval = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._holder)
			{
				this._holder = VariantUtils.ConvertTo<NPotionHolder>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._popupContainer)
			{
				this._popupContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._useButton)
			{
				this._useButton = VariantUtils.ConvertTo<NPotionPopupButton>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._discardButton)
			{
				this._discardButton = VariantUtils.ConvertTo<NPotionPopupButton>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._hoverTipBounds)
			{
				this._hoverTipBounds = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003574 RID: 13684 RVA: 0x0018F024 File Offset: 0x0018D224
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionPopup.PropertyName.IsUsable)
			{
				bool flag = this.IsUsable;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName.IsMarkedForRemoval)
			{
				bool flag = this.IsMarkedForRemoval;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName.InACardSelectScreen)
			{
				bool flag = this.InACardSelectScreen;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._holder)
			{
				value = VariantUtils.CreateFrom<NPotionHolder>(ref this._holder);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._popupContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._popupContainer);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._useButton)
			{
				value = VariantUtils.CreateFrom<NPotionPopupButton>(ref this._useButton);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._discardButton)
			{
				value = VariantUtils.CreateFrom<NPotionPopupButton>(ref this._discardButton);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._hoverTipBounds)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hoverTipBounds);
				return true;
			}
			if ((ref name) == NPotionPopup.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003575 RID: 13685 RVA: 0x0018F164 File Offset: 0x0018D364
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionPopup.PropertyName._holder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopup.PropertyName._popupContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopup.PropertyName._useButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopup.PropertyName._discardButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopup.PropertyName._hoverTipBounds, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopup.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionPopup.PropertyName.IsUsable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionPopup.PropertyName.IsMarkedForRemoval, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPotionPopup.PropertyName.InACardSelectScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003576 RID: 13686 RVA: 0x0018F2A0 File Offset: 0x0018D4A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isMarkedForRemoval = NPotionPopup.PropertyName.IsMarkedForRemoval;
			bool isMarkedForRemoval2 = this.IsMarkedForRemoval;
			info.AddProperty(isMarkedForRemoval, Variant.From<bool>(ref isMarkedForRemoval2));
			info.AddProperty(NPotionPopup.PropertyName._holder, Variant.From<NPotionHolder>(ref this._holder));
			info.AddProperty(NPotionPopup.PropertyName._popupContainer, Variant.From<Control>(ref this._popupContainer));
			info.AddProperty(NPotionPopup.PropertyName._useButton, Variant.From<NPotionPopupButton>(ref this._useButton));
			info.AddProperty(NPotionPopup.PropertyName._discardButton, Variant.From<NPotionPopupButton>(ref this._discardButton));
			info.AddProperty(NPotionPopup.PropertyName._hoverTipBounds, Variant.From<Control>(ref this._hoverTipBounds));
			info.AddProperty(NPotionPopup.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003577 RID: 13687 RVA: 0x0018F354 File Offset: 0x0018D554
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionPopup.PropertyName.IsMarkedForRemoval, ref variant))
			{
				this.IsMarkedForRemoval = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionPopup.PropertyName._holder, ref variant2))
			{
				this._holder = variant2.As<NPotionHolder>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotionPopup.PropertyName._popupContainer, ref variant3))
			{
				this._popupContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPotionPopup.PropertyName._useButton, ref variant4))
			{
				this._useButton = variant4.As<NPotionPopupButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPotionPopup.PropertyName._discardButton, ref variant5))
			{
				this._discardButton = variant5.As<NPotionPopupButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPotionPopup.PropertyName._hoverTipBounds, ref variant6))
			{
				this._hoverTipBounds = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPotionPopup.PropertyName._tween, ref variant7))
			{
				this._tween = variant7.As<Tween>();
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x0018F434 File Offset: 0x0018D634
		[CompilerGenerated]
		private void <UsePotion>g__DisableHolder|22_0()
		{
			this._holder.DisableUntilPotionRemoved();
		}

		// Token: 0x040014B2 RID: 5298
		private NPotionHolder _holder;

		// Token: 0x040014B3 RID: 5299
		private Control _popupContainer;

		// Token: 0x040014B4 RID: 5300
		private NPotionPopupButton _useButton;

		// Token: 0x040014B5 RID: 5301
		private NPotionPopupButton _discardButton;

		// Token: 0x040014B6 RID: 5302
		private Control _hoverTipBounds;

		// Token: 0x040014B7 RID: 5303
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040014B8 RID: 5304
		[Nullable(2)]
		private Player _subscribedPlayer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016FC RID: 5884
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400599C RID: 22940
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400599D RID: 22941
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnUseButtonPressed' method.
			/// </summary>
			// Token: 0x0400599E RID: 22942
			public static readonly StringName OnUseButtonPressed = "OnUseButtonPressed";

			/// <summary>
			/// Cached name for the 'OnDiscardButtonPressed' method.
			/// </summary>
			// Token: 0x0400599F RID: 22943
			public static readonly StringName OnDiscardButtonPressed = "OnDiscardButtonPressed";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040059A0 RID: 22944
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040059A1 RID: 22945
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Remove' method.
			/// </summary>
			// Token: 0x040059A2 RID: 22946
			public static readonly StringName Remove = "Remove";

			/// <summary>
			/// Cached name for the 'DisconnectSignals' method.
			/// </summary>
			// Token: 0x040059A3 RID: 22947
			public static readonly StringName DisconnectSignals = "DisconnectSignals";

			/// <summary>
			/// Cached name for the 'RefreshButtons' method.
			/// </summary>
			// Token: 0x040059A4 RID: 22948
			public static readonly StringName RefreshButtons = "RefreshButtons";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016FD RID: 5885
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsUsable' property.
			/// </summary>
			// Token: 0x040059A5 RID: 22949
			public static readonly StringName IsUsable = "IsUsable";

			/// <summary>
			/// Cached name for the 'IsMarkedForRemoval' property.
			/// </summary>
			// Token: 0x040059A6 RID: 22950
			public static readonly StringName IsMarkedForRemoval = "IsMarkedForRemoval";

			/// <summary>
			/// Cached name for the 'InACardSelectScreen' property.
			/// </summary>
			// Token: 0x040059A7 RID: 22951
			public static readonly StringName InACardSelectScreen = "InACardSelectScreen";

			/// <summary>
			/// Cached name for the '_holder' field.
			/// </summary>
			// Token: 0x040059A8 RID: 22952
			public static readonly StringName _holder = "_holder";

			/// <summary>
			/// Cached name for the '_popupContainer' field.
			/// </summary>
			// Token: 0x040059A9 RID: 22953
			public static readonly StringName _popupContainer = "_popupContainer";

			/// <summary>
			/// Cached name for the '_useButton' field.
			/// </summary>
			// Token: 0x040059AA RID: 22954
			public static readonly StringName _useButton = "_useButton";

			/// <summary>
			/// Cached name for the '_discardButton' field.
			/// </summary>
			// Token: 0x040059AB RID: 22955
			public static readonly StringName _discardButton = "_discardButton";

			/// <summary>
			/// Cached name for the '_hoverTipBounds' field.
			/// </summary>
			// Token: 0x040059AC RID: 22956
			public static readonly StringName _hoverTipBounds = "_hoverTipBounds";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040059AD RID: 22957
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016FE RID: 5886
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
