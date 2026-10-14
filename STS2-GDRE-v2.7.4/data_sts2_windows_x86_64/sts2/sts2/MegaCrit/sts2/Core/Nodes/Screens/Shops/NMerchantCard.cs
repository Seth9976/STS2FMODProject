using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x0200027A RID: 634
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantCard.cs")]
	public class NMerchantCard : NMerchantSlot
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000E483C File Offset: 0x000E2A3C
		public bool IsShowingUpgradedCard
		{
			get
			{
				NCard cardNode = this._cardNode;
				bool? flag;
				if (cardNode == null)
				{
					flag = null;
				}
				else
				{
					CardModel model = cardNode.Model;
					flag = ((model != null) ? new bool?(model.IsUpgraded) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000E4884 File Offset: 0x000E2A84
		public override MerchantEntry Entry
		{
			get
			{
				return this._cardEntry;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000E488C File Offset: 0x000E2A8C
		protected override CanvasItem Visual
		{
			get
			{
				return this._cardHolder;
			}
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000E4894 File Offset: 0x000E2A94
		public override void _Ready()
		{
			this.ConnectSignals();
			this._cardHolder = base.GetNode<Control>("%CardHolder");
			this._saleVisual = base.GetNode<Node2D>("%SaleVisual");
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000E48C8 File Offset: 0x000E2AC8
		public void FillSlot(MerchantCardEntry cardEntry)
		{
			this._cardEntry = cardEntry;
			cardEntry.EntryUpdated += this.UpdateVisual;
			cardEntry.PurchaseFailed += base.OnPurchaseFailed;
			cardEntry.PurchaseCompleted += this.OnSuccessfulPurchase;
			this.UpdateVisual();
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000E491C File Offset: 0x000E2B1C
		protected override void UpdateVisual()
		{
			base.UpdateVisual();
			if (this._cardEntry.CreationResult == null)
			{
				base.Visible = false;
				base.MouseFilter = 2L;
				base.ClearHoverTip();
				return;
			}
			if (this._cardNode != null && this._cardNode.Model != this._cardEntry.CreationResult.Card)
			{
				this._cardNode.QueueFreeSafely();
				this._cardNode = null;
			}
			if (this._cardNode == null)
			{
				this._cardNode = NCard.Create(this._cardEntry.CreationResult.Card, ModelVisibility.Visible);
				this._cardHolder.AddChildSafely(this._cardNode);
				this._cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
			}
			this._costLabel.SetTextAutoSize(this._cardEntry.Cost.ToString());
			this._saleVisual.Visible = this._cardEntry.IsOnSale;
			if (!this._cardEntry.EnoughGold)
			{
				this._costLabel.Modulate = StsColors.red;
				return;
			}
			this._costLabel.Modulate = (this._cardEntry.IsOnSale ? StsColors.green : StsColors.cream);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000E4A41 File Offset: 0x000E2C41
		public void OnInventoryOpened()
		{
			CardCreationResult creationResult = this._cardEntry.CreationResult;
			if (creationResult != null && creationResult.HasBeenModified)
			{
				TaskHelper.RunSafely(this.DoRelicFlash());
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000E4A68 File Offset: 0x000E2C68
		private async Task DoRelicFlash()
		{
			SceneTreeTimer sceneTreeTimer = base.GetTree().CreateTimer(0.4, true, false, false);
			await sceneTreeTimer.AwaitSignal(SceneTreeTimer.SignalName.Timeout, this);
			foreach (RelicModel relicModel in this._cardEntry.CreationResult.ModifyingRelics)
			{
				relicModel.Flash();
				NCard cardNode = this._cardNode;
				if (cardNode != null)
				{
					cardNode.FlashRelicOnCard(relicModel);
				}
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000E4AAC File Offset: 0x000E2CAC
		protected override async Task OnTryPurchase([Nullable(2)] MerchantInventory inventory)
		{
			await this._cardEntry.OnTryPurchaseWrapper(inventory, false);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000E4AF8 File Offset: 0x000E2CF8
		protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
		{
			base.TriggerMerchantHandToPointHere();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.GlobalUi.ReparentCard(this._cardNode);
			}
			NRun instance2 = NRun.Instance;
			if (instance2 != null)
			{
				instance2.GlobalUi.TopBar.TrailContainer.AddChildSafely(NCardFlyVfx.Create(this._cardNode, PileType.Deck, true, this._cardNode.Model.Owner.Character.TrailPath));
			}
			this._cardNode = null;
			this.UpdateVisual();
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000E4B7C File Offset: 0x000E2D7C
		protected unsafe override void OnPreview()
		{
			base.ClearHoverTip();
			NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
			int num = 1;
			List<CardModel> list = new List<CardModel>(num);
			CollectionsMarshal.SetCount<CardModel>(list, num);
			Span<CardModel> span = CollectionsMarshal.AsSpan<CardModel>(list);
			int num2 = 0;
			*span[num2] = this._cardNode.Model;
			inspectCardScreen.Open(list, 0, false);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000E4BCC File Offset: 0x000E2DCC
		protected override void CreateHoverTip()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._cardEntry.CreationResult.Card.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignment(this._hitbox, HoverTip.GetHoverTipAlignment(this, 0.75f));
			}
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000E4C10 File Offset: 0x000E2E10
		public override void _ExitTree()
		{
			base._ExitTree();
			NCard cardNode = this._cardNode;
			if (cardNode != null)
			{
				cardNode.QueueFreeSafely();
			}
			this._cardEntry.EntryUpdated -= this.UpdateVisual;
			this._cardEntry.PurchaseFailed -= base.OnPurchaseFailed;
			this._cardEntry.PurchaseCompleted -= this.OnSuccessfulPurchase;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002198 RID: 8600 RVA: 0x000E4C7C File Offset: 0x000E2E7C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NMerchantCard.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCard.MethodName.UpdateVisual, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCard.MethodName.OnInventoryOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCard.MethodName.OnPreview, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCard.MethodName.CreateHoverTip, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCard.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002199 RID: 8601 RVA: 0x000E4DAC File Offset: 0x000E2FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantCard.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCard.MethodName.UpdateVisual && args.Count == 0)
			{
				this.UpdateVisual();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCard.MethodName.OnInventoryOpened && args.Count == 0)
			{
				this.OnInventoryOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCard.MethodName.OnPreview && args.Count == 0)
			{
				this.OnPreview();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCard.MethodName.CreateHoverTip && args.Count == 0)
			{
				this.CreateHoverTip();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCard.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600219A RID: 8602 RVA: 0x000E4EA0 File Offset: 0x000E30A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantCard.MethodName._Ready || (ref method) == NMerchantCard.MethodName.UpdateVisual || (ref method) == NMerchantCard.MethodName.OnInventoryOpened || (ref method) == NMerchantCard.MethodName.OnPreview || (ref method) == NMerchantCard.MethodName.CreateHoverTip || (ref method) == NMerchantCard.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600219B RID: 8603 RVA: 0x000E4F10 File Offset: 0x000E3110
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantCard.PropertyName._saleVisual)
			{
				this._saleVisual = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._cardHolder)
			{
				this._cardHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600219C RID: 8604 RVA: 0x000E4F94 File Offset: 0x000E3194
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantCard.PropertyName.IsShowingUpgradedCard)
			{
				bool isShowingUpgradedCard = this.IsShowingUpgradedCard;
				value = VariantUtils.CreateFrom<bool>(ref isShowingUpgradedCard);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName.Visual)
			{
				CanvasItem visual = this.Visual;
				value = VariantUtils.CreateFrom<CanvasItem>(ref visual);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._saleVisual)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._saleVisual);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._cardHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardHolder);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			if ((ref name) == NMerchantCard.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600219D RID: 8605 RVA: 0x000E5070 File Offset: 0x000E3270
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantCard.PropertyName._saleVisual, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCard.PropertyName._cardHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCard.PropertyName._cardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCard.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMerchantCard.PropertyName.IsShowingUpgradedCard, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantCard.PropertyName.Visual, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600219E RID: 8606 RVA: 0x000E514C File Offset: 0x000E334C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantCard.PropertyName._saleVisual, Variant.From<Node2D>(ref this._saleVisual));
			info.AddProperty(NMerchantCard.PropertyName._cardHolder, Variant.From<Control>(ref this._cardHolder));
			info.AddProperty(NMerchantCard.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
			info.AddProperty(NMerchantCard.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x0600219F RID: 8607 RVA: 0x000E51B8 File Offset: 0x000E33B8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantCard.PropertyName._saleVisual, ref variant))
			{
				this._saleVisual = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantCard.PropertyName._cardHolder, ref variant2))
			{
				this._cardHolder = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantCard.PropertyName._cardNode, ref variant3))
			{
				this._cardNode = variant3.As<NCard>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantCard.PropertyName._hoverTween, ref variant4))
			{
				this._hoverTween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000D1C RID: 3356
		private Node2D _saleVisual;

		// Token: 0x04000D1D RID: 3357
		private Control _cardHolder;

		// Token: 0x04000D1E RID: 3358
		[Nullable(2)]
		private NCard _cardNode;

		// Token: 0x04000D1F RID: 3359
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04000D20 RID: 3360
		private MerchantCardEntry _cardEntry;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001315 RID: 4885
		[NullableContext(0)]
		public new class MethodName : NMerchantSlot.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A2A RID: 18986
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateVisual' method.
			/// </summary>
			// Token: 0x04004A2B RID: 18987
			public new static readonly StringName UpdateVisual = "UpdateVisual";

			/// <summary>
			/// Cached name for the 'OnInventoryOpened' method.
			/// </summary>
			// Token: 0x04004A2C RID: 18988
			public static readonly StringName OnInventoryOpened = "OnInventoryOpened";

			/// <summary>
			/// Cached name for the 'OnPreview' method.
			/// </summary>
			// Token: 0x04004A2D RID: 18989
			public new static readonly StringName OnPreview = "OnPreview";

			/// <summary>
			/// Cached name for the 'CreateHoverTip' method.
			/// </summary>
			// Token: 0x04004A2E RID: 18990
			public new static readonly StringName CreateHoverTip = "CreateHoverTip";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004A2F RID: 18991
			public new static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001316 RID: 4886
		[NullableContext(0)]
		public new class PropertyName : NMerchantSlot.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsShowingUpgradedCard' property.
			/// </summary>
			// Token: 0x04004A30 RID: 18992
			public static readonly StringName IsShowingUpgradedCard = "IsShowingUpgradedCard";

			/// <summary>
			/// Cached name for the 'Visual' property.
			/// </summary>
			// Token: 0x04004A31 RID: 18993
			public new static readonly StringName Visual = "Visual";

			/// <summary>
			/// Cached name for the '_saleVisual' field.
			/// </summary>
			// Token: 0x04004A32 RID: 18994
			public static readonly StringName _saleVisual = "_saleVisual";

			/// <summary>
			/// Cached name for the '_cardHolder' field.
			/// </summary>
			// Token: 0x04004A33 RID: 18995
			public static readonly StringName _cardHolder = "_cardHolder";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x04004A34 RID: 18996
			public static readonly StringName _cardNode = "_cardNode";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004A35 RID: 18997
			public new static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001317 RID: 4887
		[NullableContext(0)]
		public new class SignalName : NMerchantSlot.SignalName
		{
		}
	}
}
