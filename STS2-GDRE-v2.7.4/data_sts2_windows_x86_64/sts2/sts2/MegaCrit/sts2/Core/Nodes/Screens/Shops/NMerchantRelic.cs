using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x02000281 RID: 641
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantRelic.cs")]
	public class NMerchantRelic : NMerchantSlot
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000E994C File Offset: 0x000E7B4C
		public override MerchantEntry Entry
		{
			get
			{
				return this._relicEntry;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000E9954 File Offset: 0x000E7B54
		protected override CanvasItem Visual
		{
			get
			{
				return this._relicHolder;
			}
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000E995C File Offset: 0x000E7B5C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._relicHolder = base.GetNode<Control>("%RelicHolder");
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000E997C File Offset: 0x000E7B7C
		public void FillSlot(MerchantRelicEntry relicEntry)
		{
			this._relicEntry = relicEntry;
			this._relic = relicEntry.Model;
			relicEntry.EntryUpdated += this.UpdateVisual;
			relicEntry.PurchaseFailed += base.OnPurchaseFailed;
			relicEntry.PurchaseCompleted += this.OnSuccessfulPurchase;
			this.UpdateVisual();
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000E99DC File Offset: 0x000E7BDC
		protected override void UpdateVisual()
		{
			base.UpdateVisual();
			if (this._relicEntry.Model == null)
			{
				base.Visible = false;
				base.MouseFilter = 2L;
				if (this._relicNode != null)
				{
					this._relicNode.QueueFreeSafely();
					this._relicNode = null;
				}
				base.ClearHoverTip();
				return;
			}
			if (this._relicNode != null && this._relicNode.Model != this._relicEntry.Model)
			{
				this._relicNode.QueueFreeSafely();
				this._relicNode = null;
			}
			if (this._relicNode == null)
			{
				this._relicNode = NRelic.Create(this._relicEntry.Model, this._iconSize);
				this._relicHolder.AddChildSafely(this._relicNode);
				if (this._iconSize == NRelic.IconSize.Large)
				{
					this._relicNode.Size = new Vector2(128f, 128f);
					this._relicNode.Icon.Position -= new Vector2(0f, this._costLabel.Size.Y);
				}
				base.Hitbox.Size = this._relicNode.Icon.Size;
				base.Hitbox.Scale = this._relicHolder.Scale;
				base.Hitbox.GlobalPosition = this._relicNode.Icon.GlobalPosition;
			}
			this._relicNodePosition = this._relicNode.Icon.GlobalPosition;
			this._costLabel.SetTextAutoSize(this._relicEntry.Cost.ToString());
			this._costLabel.Modulate = (this._relicEntry.EnoughGold ? StsColors.cream : StsColors.red);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000E9B94 File Offset: 0x000E7D94
		protected override async Task OnTryPurchase([Nullable(2)] MerchantInventory inventory)
		{
			await this._relicEntry.OnTryPurchaseWrapper(inventory, false);
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000E9BE0 File Offset: 0x000E7DE0
		private void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
		{
			base.TriggerMerchantHandToPointHere();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.RelicInventory.AnimateRelic(this._relic, new Vector2?(this._relicNodePosition), null);
			}
			this.UpdateVisual();
			this._relic = this._relicEntry.Model;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000E9C40 File Offset: 0x000E7E40
		protected override void CreateHoverTip()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._relicNode.Model.HoverTips, HoverTipAlignment.None);
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

		// Token: 0x06002222 RID: 8738 RVA: 0x000E9D54 File Offset: 0x000E7F54
		protected unsafe override void OnPreview()
		{
			base.ClearHoverTip();
			int num = 1;
			List<RelicModel> list = new List<RelicModel>(num);
			CollectionsMarshal.SetCount<RelicModel>(list, num);
			Span<RelicModel> span = CollectionsMarshal.AsSpan<RelicModel>(list);
			int num2 = 0;
			*span[num2] = this._relicNode.Model;
			List<RelicModel> list2 = list;
			NGame.Instance.GetInspectRelicScreen().Open(list2, this._relicNode.Model);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000E9DB0 File Offset: 0x000E7FB0
		public override void _ExitTree()
		{
			base._ExitTree();
			this._relicEntry.EntryUpdated -= this.UpdateVisual;
			this._relicEntry.PurchaseFailed -= base.OnPurchaseFailed;
			this._relicEntry.PurchaseCompleted -= this.OnSuccessfulPurchase;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002224 RID: 8740 RVA: 0x000E9E0C File Offset: 0x000E800C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMerchantRelic.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRelic.MethodName.UpdateVisual, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRelic.MethodName.CreateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRelic.MethodName.OnPreview, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantRelic.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002225 RID: 8741 RVA: 0x000E9F0C File Offset: 0x000E810C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantRelic.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRelic.MethodName.UpdateVisual && args.Count == 0)
			{
				this.UpdateVisual();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRelic.MethodName.CreateHoverTip && args.Count == 0)
			{
				this.CreateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRelic.MethodName.OnPreview && args.Count == 0)
			{
				this.OnPreview();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantRelic.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002226 RID: 8742 RVA: 0x000E9FDC File Offset: 0x000E81DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantRelic.MethodName._Ready || (ref method) == NMerchantRelic.MethodName.UpdateVisual || (ref method) == NMerchantRelic.MethodName.CreateHoverTip || (ref method) == NMerchantRelic.MethodName.OnPreview || (ref method) == NMerchantRelic.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002227 RID: 8743 RVA: 0x000EA03C File Offset: 0x000E823C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantRelic.PropertyName._iconSize)
			{
				this._iconSize = VariantUtils.ConvertTo<NRelic.IconSize>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicHolder)
			{
				this._relicHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicNode)
			{
				this._relicNode = VariantUtils.ConvertTo<NRelic>(ref value);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicNodePosition)
			{
				this._relicNodePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002228 RID: 8744 RVA: 0x000EA0C0 File Offset: 0x000E82C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantRelic.PropertyName.Visual)
			{
				CanvasItem visual = this.Visual;
				value = VariantUtils.CreateFrom<CanvasItem>(ref visual);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._iconSize)
			{
				value = VariantUtils.CreateFrom<NRelic.IconSize>(ref this._iconSize);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicHolder);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicNode)
			{
				value = VariantUtils.CreateFrom<NRelic>(ref this._relicNode);
				return true;
			}
			if ((ref name) == NMerchantRelic.PropertyName._relicNodePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._relicNodePosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002229 RID: 8745 RVA: 0x000EA178 File Offset: 0x000E8378
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMerchantRelic.PropertyName._iconSize, 2L, "Small,Large", 4102L, true),
				new PropertyInfo(24L, NMerchantRelic.PropertyName._relicHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRelic.PropertyName._relicNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantRelic.PropertyName.Visual, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMerchantRelic.PropertyName._relicNodePosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600222A RID: 8746 RVA: 0x000EA230 File Offset: 0x000E8430
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantRelic.PropertyName._iconSize, Variant.From<NRelic.IconSize>(ref this._iconSize));
			info.AddProperty(NMerchantRelic.PropertyName._relicHolder, Variant.From<Control>(ref this._relicHolder));
			info.AddProperty(NMerchantRelic.PropertyName._relicNode, Variant.From<NRelic>(ref this._relicNode));
			info.AddProperty(NMerchantRelic.PropertyName._relicNodePosition, Variant.From<Vector2>(ref this._relicNodePosition));
		}

		/// <inheritdoc />
		// Token: 0x0600222B RID: 8747 RVA: 0x000EA29C File Offset: 0x000E849C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantRelic.PropertyName._iconSize, ref variant))
			{
				this._iconSize = variant.As<NRelic.IconSize>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantRelic.PropertyName._relicHolder, ref variant2))
			{
				this._relicHolder = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantRelic.PropertyName._relicNode, ref variant3))
			{
				this._relicNode = variant3.As<NRelic>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantRelic.PropertyName._relicNodePosition, ref variant4))
			{
				this._relicNodePosition = variant4.As<Vector2>();
			}
		}

		// Token: 0x04000D54 RID: 3412
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NRelic.IconSize _iconSize;

		// Token: 0x04000D55 RID: 3413
		private Control _relicHolder;

		// Token: 0x04000D56 RID: 3414
		[Nullable(2)]
		private NRelic _relicNode;

		// Token: 0x04000D57 RID: 3415
		private MerchantRelicEntry _relicEntry;

		// Token: 0x04000D58 RID: 3416
		[Nullable(2)]
		private RelicModel _relic;

		// Token: 0x04000D59 RID: 3417
		private Vector2 _relicNodePosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001334 RID: 4916
		[NullableContext(0)]
		public new class MethodName : NMerchantSlot.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AA6 RID: 19110
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateVisual' method.
			/// </summary>
			// Token: 0x04004AA7 RID: 19111
			public new static readonly StringName UpdateVisual = "UpdateVisual";

			/// <summary>
			/// Cached name for the 'CreateHoverTip' method.
			/// </summary>
			// Token: 0x04004AA8 RID: 19112
			public new static readonly StringName CreateHoverTip = "CreateHoverTip";

			/// <summary>
			/// Cached name for the 'OnPreview' method.
			/// </summary>
			// Token: 0x04004AA9 RID: 19113
			public new static readonly StringName OnPreview = "OnPreview";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004AAA RID: 19114
			public new static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001335 RID: 4917
		[NullableContext(0)]
		public new class PropertyName : NMerchantSlot.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Visual' property.
			/// </summary>
			// Token: 0x04004AAB RID: 19115
			public new static readonly StringName Visual = "Visual";

			/// <summary>
			/// Cached name for the '_iconSize' field.
			/// </summary>
			// Token: 0x04004AAC RID: 19116
			public static readonly StringName _iconSize = "_iconSize";

			/// <summary>
			/// Cached name for the '_relicHolder' field.
			/// </summary>
			// Token: 0x04004AAD RID: 19117
			public static readonly StringName _relicHolder = "_relicHolder";

			/// <summary>
			/// Cached name for the '_relicNode' field.
			/// </summary>
			// Token: 0x04004AAE RID: 19118
			public static readonly StringName _relicNode = "_relicNode";

			/// <summary>
			/// Cached name for the '_relicNodePosition' field.
			/// </summary>
			// Token: 0x04004AAF RID: 19119
			public static readonly StringName _relicNodePosition = "_relicNodePosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001336 RID: 4918
		[NullableContext(0)]
		public new class SignalName : NMerchantSlot.SignalName
		{
		}
	}
}
