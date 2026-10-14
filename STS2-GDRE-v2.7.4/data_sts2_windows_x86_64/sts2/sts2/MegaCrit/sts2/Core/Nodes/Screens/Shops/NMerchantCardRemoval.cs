using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x0200027B RID: 635
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantCardRemoval.cs")]
	public class NMerchantCardRemoval : NMerchantSlot
	{
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000E5244 File Offset: 0x000E3444
		private LocString Title
		{
			get
			{
				return new LocString("merchant_room", "MERCHANT.cardRemovalService.title");
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x000E5255 File Offset: 0x000E3455
		private LocString Description
		{
			get
			{
				return new LocString("merchant_room", "MERCHANT.cardRemovalService.description");
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000E5266 File Offset: 0x000E3466
		public override MerchantEntry Entry
		{
			get
			{
				return this._removalEntry;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060021A4 RID: 8612 RVA: 0x000E526E File Offset: 0x000E346E
		protected override CanvasItem Visual
		{
			get
			{
				return this._removalVisual;
			}
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000E5278 File Offset: 0x000E3478
		public override void _Ready()
		{
			this.ConnectSignals();
			this._removalVisual = base.GetNode<Sprite2D>("%Visual");
			this._animator = base.GetNode<AnimationPlayer>("%Animation");
			this._costContainer = base.GetNode<Control>("Cost");
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000E52D0 File Offset: 0x000E34D0
		public void FillSlot(MerchantCardRemovalEntry removalEntry)
		{
			this._removalEntry = removalEntry;
			this._removalEntry.EntryUpdated += this.UpdateVisual;
			this._removalEntry.PurchaseFailed += base.OnPurchaseFailed;
			this._removalEntry.PurchaseCompleted += this.OnSuccessfulPurchase;
			if (!Hook.ShouldAllowMerchantCardRemoval(base.Player.RunState, base.Player))
			{
				this._removalEntry.SetUsed();
			}
			this.UpdateVisual();
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000E5354 File Offset: 0x000E3554
		protected override void UpdateVisual()
		{
			base.UpdateVisual();
			if (this._isUnavailable)
			{
				return;
			}
			if (this._removalEntry.Used)
			{
				this._hitbox.MouseFilter = 2L;
				this._animator.CurrentAnimation = "Used";
				this._isUnavailable = true;
				this._animator.Play(null, -1.0, 1f, false);
				this._costLabel.Visible = false;
				this._costContainer.Visible = false;
				base.FocusMode = 0L;
			}
			else
			{
				base.MouseFilter = 0L;
				this._costLabel.Visible = true;
				this._costLabel.SetTextAutoSize(this._removalEntry.Cost.ToString());
				this._costLabel.Modulate = (this._removalEntry.EnoughGold ? StsColors.cream : StsColors.red);
				this._costContainer.Visible = true;
				base.FocusMode = 2L;
			}
			base.ClearHoverTip();
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000E5450 File Offset: 0x000E3650
		protected override async Task OnTryPurchase([Nullable(2)] MerchantInventory inventory)
		{
			await this._removalEntry.OnTryPurchaseWrapper(inventory, false, true);
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000E549B File Offset: 0x000E369B
		protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
		{
			base.TriggerMerchantHandToPointHere();
			this.UpdateVisual();
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000E54A9 File Offset: 0x000E36A9
		public void OnCardRemovalUsed()
		{
			this._removalEntry.SetUsed();
			this.UpdateVisual();
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000E54BC File Offset: 0x000E36BC
		protected override void CreateHoverTip()
		{
			LocString title = this.Title;
			LocString description = this.Description;
			description.Add("Amount", MerchantCardRemovalEntry.PriceIncrease);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(title, description, null), HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.GlobalPosition = base.GlobalPosition;
				bool flag = base.GlobalPosition.X > base.GetViewport().GetVisibleRect().Size.X * 0.5f;
				if (flag)
				{
					nhoverTipSet.SetAlignment(this, HoverTipAlignment.Left);
					nhoverTipSet.GlobalPosition -= base.Size * 0.5f * base.Scale;
					return;
				}
				nhoverTipSet.GlobalPosition += Vector2.Right * base.Size.X * 0.5f * base.Scale + Vector2.Up * base.Size.Y * 0.5f * base.Scale;
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000E55E4 File Offset: 0x000E37E4
		public override void _ExitTree()
		{
			base._ExitTree();
			this._removalEntry.EntryUpdated -= this.UpdateVisual;
			this._removalEntry.PurchaseFailed -= base.OnPurchaseFailed;
			this._removalEntry.PurchaseCompleted -= this.OnSuccessfulPurchase;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021AD RID: 8621 RVA: 0x000E5640 File Offset: 0x000E3840
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMerchantCardRemoval.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCardRemoval.MethodName.UpdateVisual, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCardRemoval.MethodName.OnCardRemovalUsed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCardRemoval.MethodName.CreateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCardRemoval.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021AE RID: 8622 RVA: 0x000E5740 File Offset: 0x000E3940
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantCardRemoval.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCardRemoval.MethodName.UpdateVisual && args.Count == 0)
			{
				this.UpdateVisual();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCardRemoval.MethodName.OnCardRemovalUsed && args.Count == 0)
			{
				this.OnCardRemovalUsed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCardRemoval.MethodName.CreateHoverTip && args.Count == 0)
			{
				this.CreateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCardRemoval.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060021AF RID: 8623 RVA: 0x000E5810 File Offset: 0x000E3A10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantCardRemoval.MethodName._Ready || (ref method) == NMerchantCardRemoval.MethodName.UpdateVisual || (ref method) == NMerchantCardRemoval.MethodName.OnCardRemovalUsed || (ref method) == NMerchantCardRemoval.MethodName.CreateHoverTip || (ref method) == NMerchantCardRemoval.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060021B0 RID: 8624 RVA: 0x000E5870 File Offset: 0x000E3A70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantCardRemoval.PropertyName._removalVisual)
			{
				this._removalVisual = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._animator)
			{
				this._animator = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._costContainer)
			{
				this._costContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._isUnavailable)
			{
				this._isUnavailable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060021B1 RID: 8625 RVA: 0x000E58F4 File Offset: 0x000E3AF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantCardRemoval.PropertyName.Visual)
			{
				CanvasItem visual = this.Visual;
				value = VariantUtils.CreateFrom<CanvasItem>(ref visual);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._removalVisual)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._removalVisual);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._animator)
			{
				value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._animator);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._costContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._costContainer);
				return true;
			}
			if ((ref name) == NMerchantCardRemoval.PropertyName._isUnavailable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isUnavailable);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021B2 RID: 8626 RVA: 0x000E59AC File Offset: 0x000E3BAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantCardRemoval.PropertyName._removalVisual, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCardRemoval.PropertyName._animator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCardRemoval.PropertyName._costContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantCardRemoval.PropertyName._isUnavailable, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCardRemoval.PropertyName.Visual, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021B3 RID: 8627 RVA: 0x000E5A64 File Offset: 0x000E3C64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantCardRemoval.PropertyName._removalVisual, Variant.From<Sprite2D>(ref this._removalVisual));
			info.AddProperty(NMerchantCardRemoval.PropertyName._animator, Variant.From<AnimationPlayer>(ref this._animator));
			info.AddProperty(NMerchantCardRemoval.PropertyName._costContainer, Variant.From<Control>(ref this._costContainer));
			info.AddProperty(NMerchantCardRemoval.PropertyName._isUnavailable, Variant.From<bool>(ref this._isUnavailable));
		}

		/// <inheritdoc />
		// Token: 0x060021B4 RID: 8628 RVA: 0x000E5AD0 File Offset: 0x000E3CD0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._removalVisual, ref variant))
			{
				this._removalVisual = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._animator, ref variant2))
			{
				this._animator = variant2.As<AnimationPlayer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._costContainer, ref variant3))
			{
				this._costContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._isUnavailable, ref variant4))
			{
				this._isUnavailable = variant4.As<bool>();
			}
		}

		// Token: 0x04000D21 RID: 3361
		private const string _locTable = "merchant_room";

		// Token: 0x04000D22 RID: 3362
		private Sprite2D _removalVisual;

		// Token: 0x04000D23 RID: 3363
		private AnimationPlayer _animator;

		// Token: 0x04000D24 RID: 3364
		private Control _costContainer;

		// Token: 0x04000D25 RID: 3365
		private bool _isUnavailable;

		// Token: 0x04000D26 RID: 3366
		private MerchantCardRemovalEntry _removalEntry;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200131A RID: 4890
		[NullableContext(0)]
		public new class MethodName : NMerchantSlot.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A3F RID: 19007
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateVisual' method.
			/// </summary>
			// Token: 0x04004A40 RID: 19008
			public new static readonly StringName UpdateVisual = "UpdateVisual";

			/// <summary>
			/// Cached name for the 'OnCardRemovalUsed' method.
			/// </summary>
			// Token: 0x04004A41 RID: 19009
			public static readonly StringName OnCardRemovalUsed = "OnCardRemovalUsed";

			/// <summary>
			/// Cached name for the 'CreateHoverTip' method.
			/// </summary>
			// Token: 0x04004A42 RID: 19010
			public new static readonly StringName CreateHoverTip = "CreateHoverTip";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004A43 RID: 19011
			public new static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200131B RID: 4891
		[NullableContext(0)]
		public new class PropertyName : NMerchantSlot.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Visual' property.
			/// </summary>
			// Token: 0x04004A44 RID: 19012
			public new static readonly StringName Visual = "Visual";

			/// <summary>
			/// Cached name for the '_removalVisual' field.
			/// </summary>
			// Token: 0x04004A45 RID: 19013
			public static readonly StringName _removalVisual = "_removalVisual";

			/// <summary>
			/// Cached name for the '_animator' field.
			/// </summary>
			// Token: 0x04004A46 RID: 19014
			public static readonly StringName _animator = "_animator";

			/// <summary>
			/// Cached name for the '_costContainer' field.
			/// </summary>
			// Token: 0x04004A47 RID: 19015
			public static readonly StringName _costContainer = "_costContainer";

			/// <summary>
			/// Cached name for the '_isUnavailable' field.
			/// </summary>
			// Token: 0x04004A48 RID: 19016
			public static readonly StringName _isUnavailable = "_isUnavailable";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200131C RID: 4892
		[NullableContext(0)]
		public new class SignalName : NMerchantSlot.SignalName
		{
		}
	}
}
