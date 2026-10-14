using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	/// <summary>
	/// UI wrapper class for MerchantEntry.
	/// </summary>
	// Token: 0x02000282 RID: 642
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantSlot.cs")]
	public abstract class NMerchantSlot : Control
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000EA328 File Offset: 0x000E8528
		[Nullable(1)]
		public NClickableControl Hitbox
		{
			[NullableContext(1)]
			get
			{
				return this._hitbox;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600222E RID: 8750
		[Nullable(1)]
		public abstract MerchantEntry Entry
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600222F RID: 8751
		[Nullable(1)]
		protected abstract CanvasItem Visual
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000EA330 File Offset: 0x000E8530
		[Nullable(2)]
		protected Player Player
		{
			[NullableContext(2)]
			get
			{
				NMerchantInventory merchantRug = this._merchantRug;
				if (merchantRug == null)
				{
					return null;
				}
				MerchantInventory inventory = merchantRug.Inventory;
				if (inventory == null)
				{
					return null;
				}
				return inventory.Player;
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000EA350 File Offset: 0x000E8550
		[NullableContext(1)]
		public void Initialize(NMerchantInventory rug)
		{
			this._merchantRug = rug;
			this.Player.GoldChanged += this.UpdateVisual;
			base.Connect(NMerchantSlot.SignalName.Hovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandHovered)), 0U);
			base.Connect(NMerchantSlot.SignalName.Unhovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandUnhovered)), 0U);
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x000EA3B8 File Offset: 0x000E85B8
		public override void _Ready()
		{
			if (base.GetType() != typeof(NMerchantSlot))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000EA410 File Offset: 0x000E8610
		protected virtual void ConnectSignals()
		{
			this._hitbox = base.GetNode<NClickableControl>("%Hitbox");
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this._hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
			this._hitbox.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this._hitbox.Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMousePressed)), 0U);
			this._hitbox.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
			this._costLabel = base.GetNode<MegaLabel>("%CostLabel");
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000EA514 File Offset: 0x000E8714
		public override void _ExitTree()
		{
			base.Disconnect(NMerchantSlot.SignalName.Unhovered, Callable.From<NMerchantSlot>(new Action<NMerchantSlot>(this.OnMerchantHandUnhovered)));
			this._hitbox.Disconnect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)));
			base.Disconnect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)));
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			if (this.Player != null)
			{
				this.Player.GoldChanged -= this.UpdateVisual;
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000EA5AB File Offset: 0x000E87AB
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				TaskHelper.RunSafely(this.OnSelected());
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.accept, false, false))
			{
				this.OnPreview();
				base.GetViewport().SetInputAsHandled();
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000EA5E9 File Offset: 0x000E87E9
		[NullableContext(1)]
		private void OnMousePressed(InputEvent inputEvent)
		{
			this._ignoreMouseRelease = false;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000EA5F4 File Offset: 0x000E87F4
		[NullableContext(1)]
		private void OnMouseReleased(InputEvent inputEvent)
		{
			if (!this._isHovered || this._ignoreMouseRelease)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					TaskHelper.RunSafely(this.OnSelected());
					return;
				}
				this.OnPreview();
			}
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000EA63C File Offset: 0x000E883C
		private void OnFocus()
		{
			this._isHovered = true;
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			base.Scale = NMerchantSlot._hoverScale;
			this.CreateHoverTip();
			base.EmitSignal(NMerchantSlot.SignalName.Hovered, new Variant[] { this });
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000EA694 File Offset: 0x000E8894
		private void OnUnfocus()
		{
			this._isHovered = false;
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this, "scale", NMerchantSlot._smallScale, 0.5).SetEase(1L).SetTrans(5L);
			this.ClearHoverTip();
			base.EmitSignal(NMerchantSlot.SignalName.Unhovered, new Variant[] { this });
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000EA724 File Offset: 0x000E8924
		[NullableContext(1)]
		private async Task OnSelected()
		{
			this.ClearHoverTip();
			NMerchantInventory merchantRug = this._merchantRug;
			await this.OnTryPurchase((merchantRug != null) ? merchantRug.Inventory : null);
			MerchantEntry entry = this.Entry;
			if (entry != null && entry.IsStocked && this._isHovered)
			{
				this.CreateHoverTip();
			}
		}

		// Token: 0x0600223B RID: 8763
		[NullableContext(1)]
		protected abstract Task OnTryPurchase([Nullable(2)] MerchantInventory inventory);

		/// <summary>
		/// Used when the entry slot is purchased to ensure that the hand tries to point at it.
		/// Particularly useful for the Merchant Robes relic since purchasing the slots happens automatically.
		/// </summary>
		// Token: 0x0600223C RID: 8764 RVA: 0x000EA767 File Offset: 0x000E8967
		protected void TriggerMerchantHandToPointHere()
		{
			NMerchantInventory merchantRug = this._merchantRug;
			if (merchantRug != null)
			{
				merchantRug.MerchantHand.PointAtTarget(this, Vector2.Zero);
			}
			NMerchantInventory merchantRug2 = this._merchantRug;
			if (merchantRug2 == null)
			{
				return;
			}
			merchantRug2.MerchantHand.StopPointing(2f);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000EA79F File Offset: 0x000E899F
		protected virtual void OnPreview()
		{
		}

		// Token: 0x0600223E RID: 8766
		protected abstract void CreateHoverTip();

		// Token: 0x0600223F RID: 8767 RVA: 0x000EA7A1 File Offset: 0x000E89A1
		protected void ClearHoverTip()
		{
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000EA7A9 File Offset: 0x000E89A9
		[NullableContext(1)]
		private void OnMerchantHandHovered(NMerchantSlot _)
		{
			NMerchantInventory merchantRug = this._merchantRug;
			if (merchantRug == null)
			{
				return;
			}
			merchantRug.MerchantHand.PointAtTarget(this, Vector2.Zero);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000EA7C6 File Offset: 0x000E89C6
		[NullableContext(1)]
		private void OnMerchantHandUnhovered(NMerchantSlot _)
		{
			NMerchantInventory merchantRug = this._merchantRug;
			if (merchantRug == null)
			{
				return;
			}
			merchantRug.MerchantHand.StopPointing(2f);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000EA7E4 File Offset: 0x000E89E4
		protected void OnPurchaseFailed(PurchaseStatus status)
		{
			if (status == PurchaseStatus.Success)
			{
				return;
			}
			if (this._originalVisualPosition == null)
			{
				Node2D node2D = this.Visual as Node2D;
				if (node2D != null)
				{
					this._originalVisualPosition = new float?(node2D.Position.X);
				}
				else
				{
					Control control = this.Visual as Control;
					if (control != null)
					{
						this._originalVisualPosition = new float?(control.Position.X);
					}
				}
			}
			Tween purchaseFailedTween = this._purchaseFailedTween;
			if (purchaseFailedTween != null)
			{
				purchaseFailedTween.Kill();
			}
			this._purchaseFailedTween = base.CreateTween();
			this._purchaseFailedTween.TweenMethod(Callable.From<float>(new Action<float>(this.WiggleAnimation)), 0f, 2f, 0.4000000059604645).SetEase(1L).SetTrans(4L);
			SfxCmd.Play("event:/sfx/npcs/merchant/merchant_dissapointment", 1f);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000EA8C4 File Offset: 0x000E8AC4
		protected virtual void UpdateVisual()
		{
			if (this.Entry.IsStocked)
			{
				this._costLabel.SetTextAutoSize(this.Entry.Cost.ToString());
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000EA8FC File Offset: 0x000E8AFC
		private void WiggleAnimation(float progress)
		{
			Node2D node2D = this.Visual as Node2D;
			if (node2D != null)
			{
				Node2D node2D2 = node2D;
				Vector2 vector = node2D.Position;
				vector.X = this._originalVisualPosition.Value + (float)Math.Sin((double)(progress * 3.1415927f * 2f)) * 10f;
				node2D2.Position = vector;
				return;
			}
			Control control = this.Visual as Control;
			if (control != null)
			{
				Control control2 = control;
				Vector2 vector = control.Position;
				vector.X = this._originalVisualPosition.Value + (float)Math.Sin((double)(progress * 3.1415927f * 2f)) * 10f;
				control2.Position = vector;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002245 RID: 8773 RVA: 0x000EA9A0 File Offset: 0x000E8BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NMerchantSlot.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "rug", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.OnMousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.OnMouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.TriggerMerchantHandToPointHere, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.OnPreview, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.CreateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.ClearHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.OnMerchantHandHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.OnMerchantHandUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.OnPurchaseFailed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "status", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMerchantSlot.MethodName.UpdateVisual, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantSlot.MethodName.WiggleAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "progress", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002246 RID: 8774 RVA: 0x000EAE70 File Offset: 0x000E9070
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantSlot.MethodName.Initialize && args.Count == 1)
			{
				this.Initialize(VariantUtils.ConvertTo<NMerchantInventory>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnMousePressed && args.Count == 1)
			{
				this.OnMousePressed(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnMouseReleased && args.Count == 1)
			{
				this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.TriggerMerchantHandToPointHere && args.Count == 0)
			{
				this.TriggerMerchantHandToPointHere();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnPreview && args.Count == 0)
			{
				this.OnPreview();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.CreateHoverTip && args.Count == 0)
			{
				this.CreateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.ClearHoverTip && args.Count == 0)
			{
				this.ClearHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnMerchantHandHovered && args.Count == 1)
			{
				this.OnMerchantHandHovered(VariantUtils.ConvertTo<NMerchantSlot>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnMerchantHandUnhovered && args.Count == 1)
			{
				this.OnMerchantHandUnhovered(VariantUtils.ConvertTo<NMerchantSlot>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.OnPurchaseFailed && args.Count == 1)
			{
				this.OnPurchaseFailed(VariantUtils.ConvertTo<PurchaseStatus>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.UpdateVisual && args.Count == 0)
			{
				this.UpdateVisual();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantSlot.MethodName.WiggleAnimation && args.Count == 1)
			{
				this.WiggleAnimation(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002247 RID: 8775 RVA: 0x000EB190 File Offset: 0x000E9390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantSlot.MethodName.Initialize || (ref method) == NMerchantSlot.MethodName._Ready || (ref method) == NMerchantSlot.MethodName.ConnectSignals || (ref method) == NMerchantSlot.MethodName._ExitTree || (ref method) == NMerchantSlot.MethodName._GuiInput || (ref method) == NMerchantSlot.MethodName.OnMousePressed || (ref method) == NMerchantSlot.MethodName.OnMouseReleased || (ref method) == NMerchantSlot.MethodName.OnFocus || (ref method) == NMerchantSlot.MethodName.OnUnfocus || (ref method) == NMerchantSlot.MethodName.TriggerMerchantHandToPointHere || (ref method) == NMerchantSlot.MethodName.OnPreview || (ref method) == NMerchantSlot.MethodName.CreateHoverTip || (ref method) == NMerchantSlot.MethodName.ClearHoverTip || (ref method) == NMerchantSlot.MethodName.OnMerchantHandHovered || (ref method) == NMerchantSlot.MethodName.OnMerchantHandUnhovered || (ref method) == NMerchantSlot.MethodName.OnPurchaseFailed || (ref method) == NMerchantSlot.MethodName.UpdateVisual || (ref method) == NMerchantSlot.MethodName.WiggleAnimation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002248 RID: 8776 RVA: 0x000EB2B4 File Offset: 0x000E94B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantSlot.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._hitbox)
			{
				this._hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._costLabel)
			{
				this._costLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._purchaseFailedTween)
			{
				this._purchaseFailedTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._merchantRug)
			{
				this._merchantRug = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._ignoreMouseRelease)
			{
				this._ignoreMouseRelease = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002249 RID: 8777 RVA: 0x000EB388 File Offset: 0x000E9588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantSlot.PropertyName.Hitbox)
			{
				NClickableControl hitbox = this.Hitbox;
				value = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName.Visual)
			{
				CanvasItem visual = this.Visual;
				value = VariantUtils.CreateFrom<CanvasItem>(ref visual);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._hitbox)
			{
				value = VariantUtils.CreateFrom<NClickableControl>(ref this._hitbox);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._costLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._costLabel);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._purchaseFailedTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._purchaseFailedTween);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._merchantRug)
			{
				value = VariantUtils.CreateFrom<NMerchantInventory>(ref this._merchantRug);
				return true;
			}
			if ((ref name) == NMerchantSlot.PropertyName._ignoreMouseRelease)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._ignoreMouseRelease);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600224A RID: 8778 RVA: 0x000EB4C4 File Offset: 0x000E96C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NMerchantSlot.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName._hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName._costLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName._purchaseFailedTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName._merchantRug, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantSlot.PropertyName._ignoreMouseRelease, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantSlot.PropertyName.Visual, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600224B RID: 8779 RVA: 0x000EB600 File Offset: 0x000E9800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantSlot.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NMerchantSlot.PropertyName._hitbox, Variant.From<NClickableControl>(ref this._hitbox));
			info.AddProperty(NMerchantSlot.PropertyName._costLabel, Variant.From<MegaLabel>(ref this._costLabel));
			info.AddProperty(NMerchantSlot.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NMerchantSlot.PropertyName._purchaseFailedTween, Variant.From<Tween>(ref this._purchaseFailedTween));
			info.AddProperty(NMerchantSlot.PropertyName._merchantRug, Variant.From<NMerchantInventory>(ref this._merchantRug));
			info.AddProperty(NMerchantSlot.PropertyName._ignoreMouseRelease, Variant.From<bool>(ref this._ignoreMouseRelease));
			info.AddSignalEventDelegate(NMerchantSlot.SignalName.Hovered, this.backing_Hovered);
			info.AddSignalEventDelegate(NMerchantSlot.SignalName.Unhovered, this.backing_Unhovered);
		}

		/// <inheritdoc />
		// Token: 0x0600224C RID: 8780 RVA: 0x000EB6D0 File Offset: 0x000E98D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._isHovered, ref variant))
			{
				this._isHovered = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._hitbox, ref variant2))
			{
				this._hitbox = variant2.As<NClickableControl>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._costLabel, ref variant3))
			{
				this._costLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._hoverTween, ref variant4))
			{
				this._hoverTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._purchaseFailedTween, ref variant5))
			{
				this._purchaseFailedTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._merchantRug, ref variant6))
			{
				this._merchantRug = variant6.As<NMerchantInventory>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMerchantSlot.PropertyName._ignoreMouseRelease, ref variant7))
			{
				this._ignoreMouseRelease = variant7.As<bool>();
			}
			NMerchantSlot.HoveredEventHandler hoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NMerchantSlot.HoveredEventHandler>(NMerchantSlot.SignalName.Hovered, ref hoveredEventHandler))
			{
				this.backing_Hovered = hoveredEventHandler;
			}
			NMerchantSlot.UnhoveredEventHandler unhoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NMerchantSlot.UnhoveredEventHandler>(NMerchantSlot.SignalName.Unhovered, ref unhoveredEventHandler))
			{
				this.backing_Unhovered = unhoveredEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600224D RID: 8781 RVA: 0x000EB7D8 File Offset: 0x000E99D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMerchantSlot.SignalName.Hovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "slot", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantSlot.SignalName.Unhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "slot", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantSlot.HoveredEventHandler" />
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600224E RID: 8782 RVA: 0x000EB8AB File Offset: 0x000E9AAB
		// (remove) Token: 0x0600224F RID: 8783 RVA: 0x000EB8C4 File Offset: 0x000E9AC4
		public event NMerchantSlot.HoveredEventHandler Hovered
		{
			add
			{
				this.backing_Hovered = (NMerchantSlot.HoveredEventHandler)Delegate.Combine(this.backing_Hovered, value);
			}
			remove
			{
				this.backing_Hovered = (NMerchantSlot.HoveredEventHandler)Delegate.Remove(this.backing_Hovered, value);
			}
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000EB8DD File Offset: 0x000E9ADD
		protected void EmitSignalHovered(NMerchantSlot slot)
		{
			base.EmitSignal(NMerchantSlot.SignalName.Hovered, new Variant[] { slot });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantSlot.UnhoveredEventHandler" />
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06002251 RID: 8785 RVA: 0x000EB8FE File Offset: 0x000E9AFE
		// (remove) Token: 0x06002252 RID: 8786 RVA: 0x000EB917 File Offset: 0x000E9B17
		public event NMerchantSlot.UnhoveredEventHandler Unhovered
		{
			add
			{
				this.backing_Unhovered = (NMerchantSlot.UnhoveredEventHandler)Delegate.Combine(this.backing_Unhovered, value);
			}
			remove
			{
				this.backing_Unhovered = (NMerchantSlot.UnhoveredEventHandler)Delegate.Remove(this.backing_Unhovered, value);
			}
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000EB930 File Offset: 0x000E9B30
		protected void EmitSignalUnhovered(NMerchantSlot slot)
		{
			base.EmitSignal(NMerchantSlot.SignalName.Unhovered, new Variant[] { slot });
		}

		/// <inheritdoc />
		// Token: 0x06002254 RID: 8788 RVA: 0x000EB954 File Offset: 0x000E9B54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NMerchantSlot.SignalName.Hovered && args.Count == 1)
			{
				NMerchantSlot.HoveredEventHandler hoveredEventHandler = this.backing_Hovered;
				if (hoveredEventHandler == null)
				{
					return;
				}
				hoveredEventHandler(VariantUtils.ConvertTo<NMerchantSlot>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NMerchantSlot.SignalName.Unhovered) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NMerchantSlot.UnhoveredEventHandler unhoveredEventHandler = this.backing_Unhovered;
				if (unhoveredEventHandler == null)
				{
					return;
				}
				unhoveredEventHandler(VariantUtils.ConvertTo<NMerchantSlot>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06002255 RID: 8789 RVA: 0x000EB9D3 File Offset: 0x000E9BD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NMerchantSlot.SignalName.Hovered || (ref signal) == NMerchantSlot.SignalName.Unhovered || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000D5A RID: 3418
		private bool _isHovered;

		// Token: 0x04000D5B RID: 3419
		private static readonly Vector2 _hoverScale = Vector2.One * 0.8f;

		// Token: 0x04000D5C RID: 3420
		private static readonly Vector2 _smallScale = Vector2.One * 0.65f;

		// Token: 0x04000D5D RID: 3421
		[Nullable(1)]
		protected NClickableControl _hitbox;

		// Token: 0x04000D5E RID: 3422
		[Nullable(1)]
		protected MegaLabel _costLabel;

		// Token: 0x04000D5F RID: 3423
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04000D60 RID: 3424
		[Nullable(2)]
		private Tween _purchaseFailedTween;

		// Token: 0x04000D61 RID: 3425
		[Nullable(2)]
		private NMerchantInventory _merchantRug;

		// Token: 0x04000D62 RID: 3426
		private bool _ignoreMouseRelease;

		// Token: 0x04000D63 RID: 3427
		private float? _originalVisualPosition;

		// Token: 0x04000D64 RID: 3428
		private NMerchantSlot.HoveredEventHandler backing_Hovered;

		// Token: 0x04000D65 RID: 3429
		private NMerchantSlot.UnhoveredEventHandler backing_Unhovered;

		// Token: 0x02001338 RID: 4920
		// (Invoke) Token: 0x06009EA3 RID: 40611
		[Signal]
		public delegate void HoveredEventHandler(NMerchantSlot slot);

		// Token: 0x02001339 RID: 4921
		// (Invoke) Token: 0x06009EA7 RID: 40615
		[Signal]
		public delegate void UnhoveredEventHandler(NMerchantSlot slot);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200133A RID: 4922
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004AB5 RID: 19125
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AB6 RID: 19126
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004AB7 RID: 19127
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004AB8 RID: 19128
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04004AB9 RID: 19129
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OnMousePressed' method.
			/// </summary>
			// Token: 0x04004ABA RID: 19130
			public static readonly StringName OnMousePressed = "OnMousePressed";

			/// <summary>
			/// Cached name for the 'OnMouseReleased' method.
			/// </summary>
			// Token: 0x04004ABB RID: 19131
			public static readonly StringName OnMouseReleased = "OnMouseReleased";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004ABC RID: 19132
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004ABD RID: 19133
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'TriggerMerchantHandToPointHere' method.
			/// </summary>
			// Token: 0x04004ABE RID: 19134
			public static readonly StringName TriggerMerchantHandToPointHere = "TriggerMerchantHandToPointHere";

			/// <summary>
			/// Cached name for the 'OnPreview' method.
			/// </summary>
			// Token: 0x04004ABF RID: 19135
			public static readonly StringName OnPreview = "OnPreview";

			/// <summary>
			/// Cached name for the 'CreateHoverTip' method.
			/// </summary>
			// Token: 0x04004AC0 RID: 19136
			public static readonly StringName CreateHoverTip = "CreateHoverTip";

			/// <summary>
			/// Cached name for the 'ClearHoverTip' method.
			/// </summary>
			// Token: 0x04004AC1 RID: 19137
			public static readonly StringName ClearHoverTip = "ClearHoverTip";

			/// <summary>
			/// Cached name for the 'OnMerchantHandHovered' method.
			/// </summary>
			// Token: 0x04004AC2 RID: 19138
			public static readonly StringName OnMerchantHandHovered = "OnMerchantHandHovered";

			/// <summary>
			/// Cached name for the 'OnMerchantHandUnhovered' method.
			/// </summary>
			// Token: 0x04004AC3 RID: 19139
			public static readonly StringName OnMerchantHandUnhovered = "OnMerchantHandUnhovered";

			/// <summary>
			/// Cached name for the 'OnPurchaseFailed' method.
			/// </summary>
			// Token: 0x04004AC4 RID: 19140
			public static readonly StringName OnPurchaseFailed = "OnPurchaseFailed";

			/// <summary>
			/// Cached name for the 'UpdateVisual' method.
			/// </summary>
			// Token: 0x04004AC5 RID: 19141
			public static readonly StringName UpdateVisual = "UpdateVisual";

			/// <summary>
			/// Cached name for the 'WiggleAnimation' method.
			/// </summary>
			// Token: 0x04004AC6 RID: 19142
			public static readonly StringName WiggleAnimation = "WiggleAnimation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200133B RID: 4923
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x04004AC7 RID: 19143
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the 'Visual' property.
			/// </summary>
			// Token: 0x04004AC8 RID: 19144
			public static readonly StringName Visual = "Visual";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x04004AC9 RID: 19145
			public static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_hitbox' field.
			/// </summary>
			// Token: 0x04004ACA RID: 19146
			public static readonly StringName _hitbox = "_hitbox";

			/// <summary>
			/// Cached name for the '_costLabel' field.
			/// </summary>
			// Token: 0x04004ACB RID: 19147
			public static readonly StringName _costLabel = "_costLabel";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004ACC RID: 19148
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_purchaseFailedTween' field.
			/// </summary>
			// Token: 0x04004ACD RID: 19149
			public static readonly StringName _purchaseFailedTween = "_purchaseFailedTween";

			/// <summary>
			/// Cached name for the '_merchantRug' field.
			/// </summary>
			// Token: 0x04004ACE RID: 19150
			public static readonly StringName _merchantRug = "_merchantRug";

			/// <summary>
			/// Cached name for the '_ignoreMouseRelease' field.
			/// </summary>
			// Token: 0x04004ACF RID: 19151
			public static readonly StringName _ignoreMouseRelease = "_ignoreMouseRelease";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200133C RID: 4924
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Hovered' signal.
			/// </summary>
			// Token: 0x04004AD0 RID: 19152
			public static readonly StringName Hovered = "Hovered";

			/// <summary>
			/// Cached name for the 'Unhovered' signal.
			/// </summary>
			// Token: 0x04004AD1 RID: 19153
			public static readonly StringName Unhovered = "Unhovered";
		}
	}
}
