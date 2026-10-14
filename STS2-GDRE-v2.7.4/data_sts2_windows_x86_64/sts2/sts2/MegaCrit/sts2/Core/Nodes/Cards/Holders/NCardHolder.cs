using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	/// <summary>
	/// CardHolder is meant to control the animation visuals for cards when they are presented in the PlayerHand,
	/// grid screens, card reward screens and so on. Examples include hovering/un-hovering and fancy fanning logic in hand.
	/// Clicked/buttonReleased behavior is handled by its parent: CardHolderContainer (ie PlayerHand).
	/// </summary>
	// Token: 0x0200041B RID: 1051
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NCardHolder.cs")]
	public abstract class NCardHolder : Control
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x0020766C File Offset: 0x0020586C
		protected virtual Vector2 HoverScale
		{
			get
			{
				return Vector2.One;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x00207673 File Offset: 0x00205873
		public virtual Vector2 SmallScale
		{
			get
			{
				return NCardHolder.smallScale;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x0020767A File Offset: 0x0020587A
		[Nullable(1)]
		public NClickableControl Hitbox
		{
			[NullableContext(1)]
			get
			{
				return this._hitbox;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x00207682 File Offset: 0x00205882
		// (set) Token: 0x060042D2 RID: 17106 RVA: 0x0020768A File Offset: 0x0020588A
		[Nullable(2)]
		public NCard CardNode
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			protected set;
		}

		/// <summary>
		/// The CardModel that this holder's CardNode is displaying.
		/// Null if the holder is empty.
		/// </summary>
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x00207693 File Offset: 0x00205893
		[Nullable(2)]
		public virtual CardModel CardModel
		{
			[NullableContext(2)]
			get
			{
				NCard cardNode = this.CardNode;
				if (cardNode == null)
				{
					return null;
				}
				return cardNode.Model;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x002076A6 File Offset: 0x002058A6
		public virtual bool IsShowingUpgradedCard
		{
			get
			{
				CardModel cardModel = this.CardModel;
				return cardModel != null && cardModel.IsUpgraded;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x002076B9 File Offset: 0x002058B9
		protected bool CanBeFocused
		{
			get
			{
				return this._isHovered;
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x002076C4 File Offset: 0x002058C4
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCardHolder))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		/// <summary>
		/// Set whether the card holder may be selected (e.g. clicked by mouse or confirmed by controller).
		/// Different than calling _hitbox.Disable, as the card holder can still be focused.
		/// </summary>
		// Token: 0x060042D7 RID: 17111 RVA: 0x0020771C File Offset: 0x0020591C
		public void SetClickable(bool isClickable)
		{
			this._isClickable = isClickable;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00207728 File Offset: 0x00205928
		protected void ConnectSignals()
		{
			if (this.CardNode != null)
			{
				this.CardNode.Position = Vector2.Zero;
			}
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this._hitbox = base.GetNode<NClickableControl>("%Hitbox");
			this._hitbox.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				this.OnFocus();
			}), 0U);
			this._hitbox.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				this.OnUnfocus();
			}), 0U);
			this._hitbox.Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMousePressed)), 0U);
			this._hitbox.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
			base.Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.OnChildExitingTree)), 0U);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00207850 File Offset: 0x00205A50
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			base._GuiInput(inputEvent);
			if (!this._isClickable)
			{
				return;
			}
			if (this.CardNode != null)
			{
				if (inputEvent.IsActionPressed(MegaInput.select, false, false))
				{
					SfxCmd.Play("event:/sfx/ui/clicks/ui_click", 1f);
					base.CallDeferred(NCardHolder.MethodName.EmitPressed, Array.Empty<Variant>());
					return;
				}
				if (inputEvent.IsActionPressed(MegaInput.accept, false, false))
				{
					SfxCmd.Play("event:/sfx/ui/clicks/ui_click", 1f);
					base.CallDeferred(NCardHolder.MethodName.EmitAltPressed, Array.Empty<Variant>());
				}
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x002078D4 File Offset: 0x00205AD4
		private void EmitPressed()
		{
			base.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { this });
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x002078F5 File Offset: 0x00205AF5
		private void EmitAltPressed()
		{
			base.EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[] { this });
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x00207916 File Offset: 0x00205B16
		[NullableContext(1)]
		protected virtual void SetCard(NCard node)
		{
			if (this.CardNode != null)
			{
				throw new InvalidOperationException("Cannot set a card node on a holder that already has one.");
			}
			this.CardNode = node;
			if (this.CardNode.GetParent() == null)
			{
				this.AddChildSafely(node);
				return;
			}
			node.Reparent(this, true);
		}

		/// <summary>
		/// This is used for reducing the number of unnecessary costly instancing and godot tree operations.
		/// </summary>
		// Token: 0x060042DD RID: 17117 RVA: 0x0020794F File Offset: 0x00205B4F
		[NullableContext(1)]
		public void ReassignToCard(CardModel cardModel, PileType pileType, [Nullable(2)] Creature target, ModelVisibility visibility)
		{
			this.CardNode.Visibility = visibility;
			this.CardNode.Model = cardModel;
			this.CardNode.SetPreviewTarget(target);
			this.CardNode.UpdateVisuals(pileType, CardPreviewMode.Normal);
			this.OnCardReassigned();
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00207989 File Offset: 0x00205B89
		protected virtual void OnCardReassigned()
		{
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0020798C File Offset: 0x00205B8C
		[NullableContext(1)]
		protected virtual void OnMousePressed(InputEvent inputEvent)
		{
			if (this._currentPressedAction != null)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && this._isClickable)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				bool flag = buttonIndex - 1L <= 1L;
				if (flag)
				{
					SfxCmd.Play("event:/sfx/ui/clicks/ui_click", 1f);
				}
				this._currentPressedAction = inputEventMouseButton;
			}
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x002079E4 File Offset: 0x00205BE4
		[NullableContext(1)]
		protected virtual void OnMouseReleased(InputEvent inputEvent)
		{
			if (this.CardNode == null)
			{
				return;
			}
			if (!this._isHovered)
			{
				return;
			}
			if (this._currentPressedAction == null)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && this._isClickable)
			{
				if (inputEventMouseButton.ButtonIndex != this._currentPressedAction.ButtonIndex)
				{
					return;
				}
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					base.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { this });
				}
				else
				{
					base.EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[] { this });
				}
			}
			this._currentPressedAction = null;
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x00207A83 File Offset: 0x00205C83
		protected virtual void OnFocus()
		{
			this._isHovered = true;
			this.RefreshFocusState();
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x00207A94 File Offset: 0x00205C94
		protected virtual void CreateHoverTips()
		{
			if (this.CardNode == null)
			{
				return;
			}
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this.CardNode.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignmentForCardHolder(this);
			}
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x00207ACC File Offset: 0x00205CCC
		protected void ClearHoverTips()
		{
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x00207AD4 File Offset: 0x00205CD4
		protected virtual void OnUnfocus()
		{
			this._isHovered = false;
			this._currentPressedAction = null;
			this.RefreshFocusState();
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00207AEA File Offset: 0x00205CEA
		protected void RefreshFocusState()
		{
			if (this._isFocused == this.CanBeFocused)
			{
				return;
			}
			this._isFocused = this.CanBeFocused;
			this.DoCardHoverEffects(this._isFocused);
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00207B14 File Offset: 0x00205D14
		protected virtual void DoCardHoverEffects(bool isHovered)
		{
			if (isHovered)
			{
				Tween hoverTween = this._hoverTween;
				if (hoverTween != null)
				{
					hoverTween.Kill();
				}
				base.Scale = this.HoverScale;
				if (this.CardNode.Visibility == ModelVisibility.Visible)
				{
					this.CreateHoverTips();
					return;
				}
			}
			else if (!isHovered)
			{
				Tween hoverTween2 = this._hoverTween;
				if (hoverTween2 != null)
				{
					hoverTween2.Kill();
				}
				this._hoverTween = base.CreateTween();
				this._hoverTween.TweenProperty(this, "scale", this.SmallScale, 0.5).SetEase(1L).SetTrans(5L);
				this.ClearHoverTips();
			}
		}

		/// <summary>
		/// Clean up the CardHolder if its NCard is removed.
		/// We can get rid of this if we start wanting to reuse CardHolders.
		/// </summary>
		// Token: 0x060042E7 RID: 17127 RVA: 0x00207BB5 File Offset: 0x00205DB5
		[NullableContext(1)]
		private void OnChildExitingTree(Node node)
		{
			if (node != this.CardNode)
			{
				return;
			}
			if (node.GetParent() == this)
			{
				return;
			}
			this.ClearHoverTips();
			this.CardNode = null;
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x00207BD8 File Offset: 0x00205DD8
		public virtual void Clear()
		{
			if (this.CardNode == null)
			{
				return;
			}
			if (this.CardNode.GetParent() == this)
			{
				this.RemoveChildSafely(this.CardNode);
			}
			this.CardNode = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042E9 RID: 17129 RVA: 0x00207C04 File Offset: 0x00205E04
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NCardHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.SetClickable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isClickable", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.EmitPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.EmitAltPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.SetCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.OnCardReassigned, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.OnMousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.OnMouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.CreateHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.ClearHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.RefreshFocusState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardHolder.MethodName.DoCardHoverEffects, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isHovered", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.OnChildExitingTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NCardHolder.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042EA RID: 17130 RVA: 0x002080A4 File Offset: 0x002062A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.SetClickable && args.Count == 1)
			{
				this.SetClickable(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.EmitPressed && args.Count == 0)
			{
				this.EmitPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.EmitAltPressed && args.Count == 0)
			{
				this.EmitAltPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.SetCard && args.Count == 1)
			{
				this.SetCard(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnCardReassigned && args.Count == 0)
			{
				this.OnCardReassigned();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnMousePressed && args.Count == 1)
			{
				this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnMouseReleased && args.Count == 1)
			{
				this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.CreateHoverTips && args.Count == 0)
			{
				this.CreateHoverTips();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.ClearHoverTips && args.Count == 0)
			{
				this.ClearHoverTips();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.RefreshFocusState && args.Count == 0)
			{
				this.RefreshFocusState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.DoCardHoverEffects && args.Count == 1)
			{
				this.DoCardHoverEffects(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.OnChildExitingTree && args.Count == 1)
			{
				this.OnChildExitingTree(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardHolder.MethodName.Clear && args.Count == 0)
			{
				this.Clear();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060042EB RID: 17131 RVA: 0x002083B8 File Offset: 0x002065B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardHolder.MethodName._Ready || (ref method) == NCardHolder.MethodName.SetClickable || (ref method) == NCardHolder.MethodName.ConnectSignals || (ref method) == NCardHolder.MethodName._GuiInput || (ref method) == NCardHolder.MethodName.EmitPressed || (ref method) == NCardHolder.MethodName.EmitAltPressed || (ref method) == NCardHolder.MethodName.SetCard || (ref method) == NCardHolder.MethodName.OnCardReassigned || (ref method) == NCardHolder.MethodName.OnMousePressed || (ref method) == NCardHolder.MethodName.OnMouseReleased || (ref method) == NCardHolder.MethodName.OnFocus || (ref method) == NCardHolder.MethodName.CreateHoverTips || (ref method) == NCardHolder.MethodName.ClearHoverTips || (ref method) == NCardHolder.MethodName.OnUnfocus || (ref method) == NCardHolder.MethodName.RefreshFocusState || (ref method) == NCardHolder.MethodName.DoCardHoverEffects || (ref method) == NCardHolder.MethodName.OnChildExitingTree || (ref method) == NCardHolder.MethodName.Clear || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060042EC RID: 17132 RVA: 0x002084DC File Offset: 0x002066DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardHolder.PropertyName.CardNode)
			{
				this.CardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._hitbox)
			{
				this._hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isFocused)
			{
				this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._currentPressedAction)
			{
				this._currentPressedAction = VariantUtils.ConvertTo<InputEventMouseButton>(ref value);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isClickable)
			{
				this._isClickable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060042ED RID: 17133 RVA: 0x002085B0 File Offset: 0x002067B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardHolder.PropertyName.HoverScale)
			{
				Vector2 hoverScale = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName.SmallScale)
			{
				Vector2 hoverScale = this.SmallScale;
				value = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName.Hitbox)
			{
				NClickableControl hitbox = this.Hitbox;
				value = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName.CardNode)
			{
				NCard cardNode = this.CardNode;
				value = VariantUtils.CreateFrom<NCard>(ref cardNode);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName.IsShowingUpgradedCard)
			{
				bool flag = this.IsShowingUpgradedCard;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName.CanBeFocused)
			{
				bool flag = this.CanBeFocused;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._hitbox)
			{
				value = VariantUtils.CreateFrom<NClickableControl>(ref this._hitbox);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isFocused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._currentPressedAction)
			{
				value = VariantUtils.CreateFrom<InputEventMouseButton>(ref this._currentPressedAction);
				return true;
			}
			if ((ref name) == NCardHolder.PropertyName._isClickable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isClickable);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042EE RID: 17134 RVA: 0x00208758 File Offset: 0x00206958
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NCardHolder.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardHolder.PropertyName.SmallScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHolder.PropertyName._hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHolder.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHolder.PropertyName.CardNode, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardHolder.PropertyName.IsShowingUpgradedCard, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardHolder.PropertyName.CanBeFocused, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardHolder.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardHolder.PropertyName._isFocused, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHolder.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardHolder.PropertyName._currentPressedAction, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardHolder.PropertyName._isClickable, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042EF RID: 17135 RVA: 0x002088F4 File Offset: 0x00206AF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cardNode = NCardHolder.PropertyName.CardNode;
			NCard cardNode2 = this.CardNode;
			info.AddProperty(cardNode, Variant.From<NCard>(ref cardNode2));
			info.AddProperty(NCardHolder.PropertyName._hitbox, Variant.From<NClickableControl>(ref this._hitbox));
			info.AddProperty(NCardHolder.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NCardHolder.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
			info.AddProperty(NCardHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NCardHolder.PropertyName._currentPressedAction, Variant.From<InputEventMouseButton>(ref this._currentPressedAction));
			info.AddProperty(NCardHolder.PropertyName._isClickable, Variant.From<bool>(ref this._isClickable));
			info.AddSignalEventDelegate(NCardHolder.SignalName.Pressed, this.backing_Pressed);
			info.AddSignalEventDelegate(NCardHolder.SignalName.AltPressed, this.backing_AltPressed);
		}

		/// <inheritdoc />
		// Token: 0x060042F0 RID: 17136 RVA: 0x002089C8 File Offset: 0x00206BC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardHolder.PropertyName.CardNode, ref variant))
			{
				this.CardNode = variant.As<NCard>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardHolder.PropertyName._hitbox, ref variant2))
			{
				this._hitbox = variant2.As<NClickableControl>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardHolder.PropertyName._isHovered, ref variant3))
			{
				this._isHovered = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardHolder.PropertyName._isFocused, ref variant4))
			{
				this._isFocused = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardHolder.PropertyName._hoverTween, ref variant5))
			{
				this._hoverTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardHolder.PropertyName._currentPressedAction, ref variant6))
			{
				this._currentPressedAction = variant6.As<InputEventMouseButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardHolder.PropertyName._isClickable, ref variant7))
			{
				this._isClickable = variant7.As<bool>();
			}
			NCardHolder.PressedEventHandler pressedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardHolder.PressedEventHandler>(NCardHolder.SignalName.Pressed, ref pressedEventHandler))
			{
				this.backing_Pressed = pressedEventHandler;
			}
			NCardHolder.AltPressedEventHandler altPressedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardHolder.AltPressedEventHandler>(NCardHolder.SignalName.AltPressed, ref altPressedEventHandler))
			{
				this.backing_AltPressed = altPressedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042F1 RID: 17137 RVA: 0x00208AD0 File Offset: 0x00206CD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardHolder.SignalName.Pressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardHolder.SignalName.AltPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NCardHolder.PressedEventHandler" />
		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060042F2 RID: 17138 RVA: 0x00208BA3 File Offset: 0x00206DA3
		// (remove) Token: 0x060042F3 RID: 17139 RVA: 0x00208BBC File Offset: 0x00206DBC
		public event NCardHolder.PressedEventHandler Pressed
		{
			add
			{
				this.backing_Pressed = (NCardHolder.PressedEventHandler)Delegate.Combine(this.backing_Pressed, value);
			}
			remove
			{
				this.backing_Pressed = (NCardHolder.PressedEventHandler)Delegate.Remove(this.backing_Pressed, value);
			}
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00208BD5 File Offset: 0x00206DD5
		protected void EmitSignalPressed(NCardHolder cardHolder)
		{
			base.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { cardHolder });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NCardHolder.AltPressedEventHandler" />
		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060042F5 RID: 17141 RVA: 0x00208BF6 File Offset: 0x00206DF6
		// (remove) Token: 0x060042F6 RID: 17142 RVA: 0x00208C0F File Offset: 0x00206E0F
		public event NCardHolder.AltPressedEventHandler AltPressed
		{
			add
			{
				this.backing_AltPressed = (NCardHolder.AltPressedEventHandler)Delegate.Combine(this.backing_AltPressed, value);
			}
			remove
			{
				this.backing_AltPressed = (NCardHolder.AltPressedEventHandler)Delegate.Remove(this.backing_AltPressed, value);
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00208C28 File Offset: 0x00206E28
		protected void EmitSignalAltPressed(NCardHolder cardHolder)
		{
			base.EmitSignal(NCardHolder.SignalName.AltPressed, new Variant[] { cardHolder });
		}

		/// <inheritdoc />
		// Token: 0x060042F8 RID: 17144 RVA: 0x00208C4C File Offset: 0x00206E4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NCardHolder.SignalName.Pressed && args.Count == 1)
			{
				NCardHolder.PressedEventHandler pressedEventHandler = this.backing_Pressed;
				if (pressedEventHandler == null)
				{
					return;
				}
				pressedEventHandler(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NCardHolder.SignalName.AltPressed) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NCardHolder.AltPressedEventHandler altPressedEventHandler = this.backing_AltPressed;
				if (altPressedEventHandler == null)
				{
					return;
				}
				altPressedEventHandler(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060042F9 RID: 17145 RVA: 0x00208CCB File Offset: 0x00206ECB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardHolder.SignalName.Pressed || (ref signal) == NCardHolder.SignalName.AltPressed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001A0D RID: 6669
		public static readonly Vector2 smallScale = Vector2.One * 0.8f;

		// Token: 0x04001A0E RID: 6670
		[Nullable(1)]
		protected NClickableControl _hitbox;

		// Token: 0x04001A10 RID: 6672
		protected bool _isHovered;

		// Token: 0x04001A11 RID: 6673
		protected bool _isFocused;

		// Token: 0x04001A12 RID: 6674
		[Nullable(2)]
		protected Tween _hoverTween;

		// Token: 0x04001A13 RID: 6675
		[Nullable(2)]
		private InputEventMouseButton _currentPressedAction;

		// Token: 0x04001A14 RID: 6676
		protected bool _isClickable = true;

		// Token: 0x04001A15 RID: 6677
		private NCardHolder.PressedEventHandler backing_Pressed;

		// Token: 0x04001A16 RID: 6678
		private NCardHolder.AltPressedEventHandler backing_AltPressed;

		// Token: 0x0200195B RID: 6491
		// (Invoke) Token: 0x0600AA97 RID: 43671
		[Signal]
		public delegate void PressedEventHandler(NCardHolder cardHolder);

		// Token: 0x0200195C RID: 6492
		// (Invoke) Token: 0x0600AA9B RID: 43675
		[Signal]
		public delegate void AltPressedEventHandler(NCardHolder cardHolder);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200195D RID: 6493
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006421 RID: 25633
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetClickable' method.
			/// </summary>
			// Token: 0x04006422 RID: 25634
			public static readonly StringName SetClickable = "SetClickable";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04006423 RID: 25635
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04006424 RID: 25636
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'EmitPressed' method.
			/// </summary>
			// Token: 0x04006425 RID: 25637
			public static readonly StringName EmitPressed = "EmitPressed";

			/// <summary>
			/// Cached name for the 'EmitAltPressed' method.
			/// </summary>
			// Token: 0x04006426 RID: 25638
			public static readonly StringName EmitAltPressed = "EmitAltPressed";

			/// <summary>
			/// Cached name for the 'SetCard' method.
			/// </summary>
			// Token: 0x04006427 RID: 25639
			public static readonly StringName SetCard = "SetCard";

			/// <summary>
			/// Cached name for the 'OnCardReassigned' method.
			/// </summary>
			// Token: 0x04006428 RID: 25640
			public static readonly StringName OnCardReassigned = "OnCardReassigned";

			/// <summary>
			/// Cached name for the 'OnMousePressed' method.
			/// </summary>
			// Token: 0x04006429 RID: 25641
			public static readonly StringName OnMousePressed = "OnMousePressed";

			/// <summary>
			/// Cached name for the 'OnMouseReleased' method.
			/// </summary>
			// Token: 0x0400642A RID: 25642
			public static readonly StringName OnMouseReleased = "OnMouseReleased";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400642B RID: 25643
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'CreateHoverTips' method.
			/// </summary>
			// Token: 0x0400642C RID: 25644
			public static readonly StringName CreateHoverTips = "CreateHoverTips";

			/// <summary>
			/// Cached name for the 'ClearHoverTips' method.
			/// </summary>
			// Token: 0x0400642D RID: 25645
			public static readonly StringName ClearHoverTips = "ClearHoverTips";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400642E RID: 25646
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'RefreshFocusState' method.
			/// </summary>
			// Token: 0x0400642F RID: 25647
			public static readonly StringName RefreshFocusState = "RefreshFocusState";

			/// <summary>
			/// Cached name for the 'DoCardHoverEffects' method.
			/// </summary>
			// Token: 0x04006430 RID: 25648
			public static readonly StringName DoCardHoverEffects = "DoCardHoverEffects";

			/// <summary>
			/// Cached name for the 'OnChildExitingTree' method.
			/// </summary>
			// Token: 0x04006431 RID: 25649
			public static readonly StringName OnChildExitingTree = "OnChildExitingTree";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04006432 RID: 25650
			public static readonly StringName Clear = "Clear";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200195E RID: 6494
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04006433 RID: 25651
			public static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'SmallScale' property.
			/// </summary>
			// Token: 0x04006434 RID: 25652
			public static readonly StringName SmallScale = "SmallScale";

			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x04006435 RID: 25653
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the 'CardNode' property.
			/// </summary>
			// Token: 0x04006436 RID: 25654
			public static readonly StringName CardNode = "CardNode";

			/// <summary>
			/// Cached name for the 'IsShowingUpgradedCard' property.
			/// </summary>
			// Token: 0x04006437 RID: 25655
			public static readonly StringName IsShowingUpgradedCard = "IsShowingUpgradedCard";

			/// <summary>
			/// Cached name for the 'CanBeFocused' property.
			/// </summary>
			// Token: 0x04006438 RID: 25656
			public static readonly StringName CanBeFocused = "CanBeFocused";

			/// <summary>
			/// Cached name for the '_hitbox' field.
			/// </summary>
			// Token: 0x04006439 RID: 25657
			public static readonly StringName _hitbox = "_hitbox";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x0400643A RID: 25658
			public static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_isFocused' field.
			/// </summary>
			// Token: 0x0400643B RID: 25659
			public static readonly StringName _isFocused = "_isFocused";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x0400643C RID: 25660
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_currentPressedAction' field.
			/// </summary>
			// Token: 0x0400643D RID: 25661
			public static readonly StringName _currentPressedAction = "_currentPressedAction";

			/// <summary>
			/// Cached name for the '_isClickable' field.
			/// </summary>
			// Token: 0x0400643E RID: 25662
			public static readonly StringName _isClickable = "_isClickable";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200195F RID: 6495
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Pressed' signal.
			/// </summary>
			// Token: 0x0400643F RID: 25663
			public static readonly StringName Pressed = "Pressed";

			/// <summary>
			/// Cached name for the 'AltPressed' signal.
			/// </summary>
			// Token: 0x04006440 RID: 25664
			public static readonly StringName AltPressed = "AltPressed";
		}
	}
}
