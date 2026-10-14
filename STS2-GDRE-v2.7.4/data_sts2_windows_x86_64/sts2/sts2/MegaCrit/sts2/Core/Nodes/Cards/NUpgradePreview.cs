using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	/// <summary>
	/// Used in Rest Site, Armaments, etc.
	/// This is only for upgrades. Not used for previewing enchantments.
	/// </summary>
	// Token: 0x0200041A RID: 1050
	[ScriptPath("res://src/Core/Nodes/Cards/NUpgradePreview.cs")]
	public class NUpgradePreview : Control
	{
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060042BD RID: 17085 RVA: 0x00206F4C File Offset: 0x0020514C
		public Vector2 SelectedCardPosition
		{
			get
			{
				return this._before.GlobalPosition;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060042BE RID: 17086 RVA: 0x00206F59 File Offset: 0x00205159
		// (set) Token: 0x060042BF RID: 17087 RVA: 0x00206F61 File Offset: 0x00205161
		[Nullable(2)]
		public CardModel Card
		{
			[NullableContext(2)]
			get
			{
				return this._card;
			}
			[NullableContext(2)]
			set
			{
				this._card = value;
				this.Reload();
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x00206F70 File Offset: 0x00205170
		public override void _Ready()
		{
			this._before = base.GetNode<Control>("%Before");
			this._after = base.GetNode<Control>("%After");
			this._arrows = base.GetNode<Control>("Arrows");
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00206FC0 File Offset: 0x002051C0
		private void Reload()
		{
			this.RemoveExistingCards();
			this._arrows.Visible = this.Card != null;
			if (this.Card == null)
			{
				return;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NPlayerHand nplayerHand = ((instance != null) ? instance.Ui.Hand : null);
			NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(NCard.Create(this.Card, ModelVisibility.Visible), nplayerHand == null, nplayerHand != null);
			this._before.AddChildSafely(npreviewCardHolder);
			npreviewCardHolder.FocusMode = 2L;
			npreviewCardHolder.CardNode.UpdateVisuals(this.Card.Pile.Type, CardPreviewMode.Normal);
			if (nplayerHand != null)
			{
				npreviewCardHolder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.ReturnCard)), 0U);
			}
			CardModel cardModel = this.Card.CardScope.CloneCard(this.Card);
			cardModel.UpgradeInternal();
			cardModel.UpgradePreviewType = (this.Card.Pile.IsCombatPile ? CardUpgradePreviewType.Combat : CardUpgradePreviewType.Deck);
			NPreviewCardHolder npreviewCardHolder2 = NPreviewCardHolder.Create(NCard.Create(cardModel, ModelVisibility.Visible), true, false);
			npreviewCardHolder2.FocusMode = 0L;
			this._after.AddChildSafely(npreviewCardHolder2);
			npreviewCardHolder2.CardNode.ShowUpgradePreview();
		}

		/// <summary>
		/// Helper function to remove any existing preview cards
		/// </summary>
		// Token: 0x060042C2 RID: 17090 RVA: 0x002070DC File Offset: 0x002052DC
		private void RemoveExistingCards()
		{
			foreach (Node node in this._before.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			foreach (Node node2 in this._after.GetChildren(false))
			{
				node2.QueueFreeSafely();
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00207170 File Offset: 0x00205370
		[NullableContext(1)]
		private void ReturnCard(NCardHolder holder)
		{
			holder.Pressed -= this.ReturnCard;
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.Ui.Hand.DeselectCard(holder.CardNode);
			}
			this.Card = null;
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060042C4 RID: 17092 RVA: 0x002071AB File Offset: 0x002053AB
		[Nullable(1)]
		public Control DefaultFocusedControl
		{
			[NullableContext(1)]
			get
			{
				return this._before.GetChild<NPreviewCardHolder>(0, false);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042C5 RID: 17093 RVA: 0x002071BC File Offset: 0x002053BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NUpgradePreview.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUpgradePreview.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUpgradePreview.MethodName.RemoveExistingCards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUpgradePreview.MethodName.ReturnCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042C6 RID: 17094 RVA: 0x002072C0 File Offset: 0x002054C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUpgradePreview.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUpgradePreview.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUpgradePreview.MethodName.RemoveExistingCards && args.Count == 0)
			{
				this.RemoveExistingCards();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUpgradePreview.MethodName.ReturnCard && args.Count == 1)
			{
				this.ReturnCard(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060042C7 RID: 17095 RVA: 0x00207378 File Offset: 0x00205578
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUpgradePreview.MethodName._Ready || (ref method) == NUpgradePreview.MethodName.Reload || (ref method) == NUpgradePreview.MethodName.RemoveExistingCards || (ref method) == NUpgradePreview.MethodName.ReturnCard || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060042C8 RID: 17096 RVA: 0x002073C8 File Offset: 0x002055C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUpgradePreview.PropertyName._before)
			{
				this._before = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName._after)
			{
				this._after = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName._arrows)
			{
				this._arrows = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060042C9 RID: 17097 RVA: 0x00207430 File Offset: 0x00205630
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUpgradePreview.PropertyName.SelectedCardPosition)
			{
				Vector2 selectedCardPosition = this.SelectedCardPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref selectedCardPosition);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName._before)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._before);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName._after)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._after);
				return true;
			}
			if ((ref name) == NUpgradePreview.PropertyName._arrows)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._arrows);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042CA RID: 17098 RVA: 0x002074EC File Offset: 0x002056EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUpgradePreview.PropertyName._before, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUpgradePreview.PropertyName._after, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUpgradePreview.PropertyName._arrows, 0L, "", 4096L, false),
				new PropertyInfo(5L, NUpgradePreview.PropertyName.SelectedCardPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUpgradePreview.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042CB RID: 17099 RVA: 0x002075A4 File Offset: 0x002057A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUpgradePreview.PropertyName._before, Variant.From<Control>(ref this._before));
			info.AddProperty(NUpgradePreview.PropertyName._after, Variant.From<Control>(ref this._after));
			info.AddProperty(NUpgradePreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
		}

		/// <inheritdoc />
		// Token: 0x060042CC RID: 17100 RVA: 0x002075FC File Offset: 0x002057FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUpgradePreview.PropertyName._before, ref variant))
			{
				this._before = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUpgradePreview.PropertyName._after, ref variant2))
			{
				this._after = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUpgradePreview.PropertyName._arrows, ref variant3))
			{
				this._arrows = variant3.As<Control>();
			}
		}

		// Token: 0x04001A09 RID: 6665
		[Nullable(1)]
		private Control _before;

		// Token: 0x04001A0A RID: 6666
		[Nullable(1)]
		private Control _after;

		// Token: 0x04001A0B RID: 6667
		[Nullable(1)]
		private Control _arrows;

		// Token: 0x04001A0C RID: 6668
		[Nullable(2)]
		private CardModel _card;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001958 RID: 6488
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006418 RID: 25624
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04006419 RID: 25625
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'RemoveExistingCards' method.
			/// </summary>
			// Token: 0x0400641A RID: 25626
			public static readonly StringName RemoveExistingCards = "RemoveExistingCards";

			/// <summary>
			/// Cached name for the 'ReturnCard' method.
			/// </summary>
			// Token: 0x0400641B RID: 25627
			public static readonly StringName ReturnCard = "ReturnCard";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001959 RID: 6489
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SelectedCardPosition' property.
			/// </summary>
			// Token: 0x0400641C RID: 25628
			public static readonly StringName SelectedCardPosition = "SelectedCardPosition";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400641D RID: 25629
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_before' field.
			/// </summary>
			// Token: 0x0400641E RID: 25630
			public static readonly StringName _before = "_before";

			/// <summary>
			/// Cached name for the '_after' field.
			/// </summary>
			// Token: 0x0400641F RID: 25631
			public static readonly StringName _after = "_after";

			/// <summary>
			/// Cached name for the '_arrows' field.
			/// </summary>
			// Token: 0x04006420 RID: 25632
			public static readonly StringName _arrows = "_arrows";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200195A RID: 6490
		public class SignalName : Control.SignalName
		{
		}
	}
}
