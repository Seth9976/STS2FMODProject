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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x02000280 RID: 640
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantPotion.cs")]
	public class NMerchantPotion : NMerchantSlot
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x000E9156 File Offset: 0x000E7356
		public override MerchantEntry Entry
		{
			get
			{
				return this._potionEntry;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x000E915E File Offset: 0x000E735E
		protected override CanvasItem Visual
		{
			get
			{
				return this._potionHolder;
			}
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000E9166 File Offset: 0x000E7366
		public override void _Ready()
		{
			this.ConnectSignals();
			this._potionHolder = base.GetNode<Control>("%PotionHolder");
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000E9184 File Offset: 0x000E7384
		public void FillSlot(MerchantPotionEntry potionEntry)
		{
			this._potionEntry = potionEntry;
			this._potion = potionEntry.Model;
			this._potionEntry.EntryUpdated += this.UpdateVisual;
			this._potionEntry.PurchaseFailed += base.OnPurchaseFailed;
			this._potionEntry.PurchaseCompleted += this.OnSuccessfulPurchase;
			this.UpdateVisual();
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000E91F0 File Offset: 0x000E73F0
		protected override void UpdateVisual()
		{
			base.UpdateVisual();
			if (this._potionEntry.Model == null)
			{
				base.Visible = false;
				base.MouseFilter = 2L;
				if (this._potionNode != null)
				{
					this._potionNode.QueueFreeSafely();
					this._potionNode = null;
				}
				base.ClearHoverTip();
				return;
			}
			if (this._potionNode != null && this._potionNode.Model != this._potionEntry.Model)
			{
				this._potionNode.QueueFreeSafely();
				this._potionNode = null;
			}
			if (this._potionNode == null)
			{
				this._potionNode = NPotion.Create(this._potionEntry.Model);
				this._potionHolder.AddChildSafely(this._potionNode);
				this._potionNode.Position = Vector2.Zero;
			}
			this._potionNodePosition = this._potionNode.GlobalPosition;
			this._costLabel.SetTextAutoSize(this._potionEntry.Cost.ToString());
			this._costLabel.Modulate = (this._potionEntry.EnoughGold ? StsColors.cream : StsColors.red);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000E9304 File Offset: 0x000E7504
		protected override async Task OnTryPurchase([Nullable(2)] MerchantInventory inventory)
		{
			await this._potionEntry.OnTryPurchaseWrapper(inventory, false);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000E9350 File Offset: 0x000E7550
		protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
		{
			base.TriggerMerchantHandToPointHere();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.TopBar.PotionContainer.AnimatePotion(this._potion, new Vector2?(this._potionNodePosition));
			}
			this.UpdateVisual();
			this._potion = this._potionEntry.Model;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000E93AC File Offset: 0x000E75AC
		protected override void CreateHoverTip()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._potionNode.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition, false);
			}
			if (nhoverTipSet != null)
			{
				bool flag = base.GlobalPosition.X > base.GetViewport().GetVisibleRect().Size.X * 0.5f;
				if (flag)
				{
					nhoverTipSet.SetAlignment(this, HoverTipAlignment.Left);
					nhoverTipSet.GlobalPosition -= base.Size * 0.5f * base.Scale;
					return;
				}
				nhoverTipSet.SetAlignment(this, HoverTipAlignment.Right);
				nhoverTipSet.GlobalPosition += Vector2.Right * base.Size.X * 0.5f * base.Scale + Vector2.Up * base.Size.Y * 0.5f * base.Scale;
			}
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000E94C0 File Offset: 0x000E76C0
		public override void _ExitTree()
		{
			base._ExitTree();
			this._potionEntry.EntryUpdated -= this.UpdateVisual;
			this._potionEntry.PurchaseFailed -= base.OnPurchaseFailed;
			this._potionEntry.PurchaseCompleted -= this.OnSuccessfulPurchase;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002211 RID: 8721 RVA: 0x000E951C File Offset: 0x000E771C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NMerchantPotion.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantPotion.MethodName.UpdateVisual, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantPotion.MethodName.CreateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantPotion.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002212 RID: 8722 RVA: 0x000E95F0 File Offset: 0x000E77F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantPotion.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantPotion.MethodName.UpdateVisual && args.Count == 0)
			{
				this.UpdateVisual();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantPotion.MethodName.CreateHoverTip && args.Count == 0)
			{
				this.CreateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantPotion.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002213 RID: 8723 RVA: 0x000E969C File Offset: 0x000E789C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantPotion.MethodName._Ready || (ref method) == NMerchantPotion.MethodName.UpdateVisual || (ref method) == NMerchantPotion.MethodName.CreateHoverTip || (ref method) == NMerchantPotion.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002214 RID: 8724 RVA: 0x000E96EC File Offset: 0x000E78EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantPotion.PropertyName._potionHolder)
			{
				this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantPotion.PropertyName._potionNode)
			{
				this._potionNode = VariantUtils.ConvertTo<NPotion>(ref value);
				return true;
			}
			if ((ref name) == NMerchantPotion.PropertyName._potionNodePosition)
			{
				this._potionNodePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002215 RID: 8725 RVA: 0x000E9754 File Offset: 0x000E7954
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantPotion.PropertyName.Visual)
			{
				CanvasItem visual = this.Visual;
				value = VariantUtils.CreateFrom<CanvasItem>(ref visual);
				return true;
			}
			if ((ref name) == NMerchantPotion.PropertyName._potionHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
				return true;
			}
			if ((ref name) == NMerchantPotion.PropertyName._potionNode)
			{
				value = VariantUtils.CreateFrom<NPotion>(ref this._potionNode);
				return true;
			}
			if ((ref name) == NMerchantPotion.PropertyName._potionNodePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._potionNodePosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002216 RID: 8726 RVA: 0x000E97EC File Offset: 0x000E79EC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantPotion.PropertyName._potionHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantPotion.PropertyName._potionNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantPotion.PropertyName.Visual, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMerchantPotion.PropertyName._potionNodePosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002217 RID: 8727 RVA: 0x000E9884 File Offset: 0x000E7A84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantPotion.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
			info.AddProperty(NMerchantPotion.PropertyName._potionNode, Variant.From<NPotion>(ref this._potionNode));
			info.AddProperty(NMerchantPotion.PropertyName._potionNodePosition, Variant.From<Vector2>(ref this._potionNodePosition));
		}

		/// <inheritdoc />
		// Token: 0x06002218 RID: 8728 RVA: 0x000E98DC File Offset: 0x000E7ADC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantPotion.PropertyName._potionHolder, ref variant))
			{
				this._potionHolder = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantPotion.PropertyName._potionNode, ref variant2))
			{
				this._potionNode = variant2.As<NPotion>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantPotion.PropertyName._potionNodePosition, ref variant3))
			{
				this._potionNodePosition = variant3.As<Vector2>();
			}
		}

		// Token: 0x04000D4F RID: 3407
		private Control _potionHolder;

		// Token: 0x04000D50 RID: 3408
		[Nullable(2)]
		private NPotion _potionNode;

		// Token: 0x04000D51 RID: 3409
		private MerchantPotionEntry _potionEntry;

		// Token: 0x04000D52 RID: 3410
		[Nullable(2)]
		private PotionModel _potion;

		// Token: 0x04000D53 RID: 3411
		private Vector2 _potionNodePosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001330 RID: 4912
		[NullableContext(0)]
		public new class MethodName : NMerchantSlot.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A99 RID: 19097
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateVisual' method.
			/// </summary>
			// Token: 0x04004A9A RID: 19098
			public new static readonly StringName UpdateVisual = "UpdateVisual";

			/// <summary>
			/// Cached name for the 'CreateHoverTip' method.
			/// </summary>
			// Token: 0x04004A9B RID: 19099
			public new static readonly StringName CreateHoverTip = "CreateHoverTip";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004A9C RID: 19100
			public new static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001331 RID: 4913
		[NullableContext(0)]
		public new class PropertyName : NMerchantSlot.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Visual' property.
			/// </summary>
			// Token: 0x04004A9D RID: 19101
			public new static readonly StringName Visual = "Visual";

			/// <summary>
			/// Cached name for the '_potionHolder' field.
			/// </summary>
			// Token: 0x04004A9E RID: 19102
			public static readonly StringName _potionHolder = "_potionHolder";

			/// <summary>
			/// Cached name for the '_potionNode' field.
			/// </summary>
			// Token: 0x04004A9F RID: 19103
			public static readonly StringName _potionNode = "_potionNode";

			/// <summary>
			/// Cached name for the '_potionNodePosition' field.
			/// </summary>
			// Token: 0x04004AA0 RID: 19104
			public static readonly StringName _potionNodePosition = "_potionNodePosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001332 RID: 4914
		[NullableContext(0)]
		public new class SignalName : NMerchantSlot.SignalName
		{
		}
	}
}
