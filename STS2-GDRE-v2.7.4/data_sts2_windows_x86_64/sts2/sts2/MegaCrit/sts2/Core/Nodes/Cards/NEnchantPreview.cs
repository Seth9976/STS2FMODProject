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

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	/// <summary>
	/// Shows a card's before and after for when they get enchanted.
	/// </summary>
	// Token: 0x02000417 RID: 1047
	[ScriptPath("res://src/Core/Nodes/Cards/NEnchantPreview.cs")]
	public class NEnchantPreview : Control
	{
		// Token: 0x06004291 RID: 17041 RVA: 0x00205B3C File Offset: 0x00203D3C
		public override void _Ready()
		{
			this._before = base.GetNode<Control>("%Before");
			this._after = base.GetNode<Control>("%After");
			this._arrows = base.GetNode<Control>("Arrows");
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x00205B8C File Offset: 0x00203D8C
		[NullableContext(1)]
		public void Init(CardModel card, EnchantmentModel canonicalEnchantment, int amount)
		{
			canonicalEnchantment.AssertCanonical();
			this.RemoveExistingCards();
			NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(NCard.Create(card, ModelVisibility.Visible), true, false);
			this._before.AddChildSafely(npreviewCardHolder);
			npreviewCardHolder.CardNode.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
			CardModel cardModel = card.CardScope.CloneCard(card);
			EnchantmentModel enchantmentModel = canonicalEnchantment.ToMutable();
			cardModel.EnchantInternal(enchantmentModel, amount);
			cardModel.IsEnchantmentPreview = true;
			enchantmentModel.ModifyCard();
			NPreviewCardHolder npreviewCardHolder2 = NPreviewCardHolder.Create(NCard.Create(cardModel, ModelVisibility.Visible), true, false);
			this._after.AddChildSafely(npreviewCardHolder2);
			npreviewCardHolder2.CardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Helper function to remove any existing preview cards
		/// </summary>
		// Token: 0x06004293 RID: 17043 RVA: 0x00205C30 File Offset: 0x00203E30
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004294 RID: 17044 RVA: 0x00205CC4 File Offset: 0x00203EC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NEnchantPreview.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEnchantPreview.MethodName.RemoveExistingCards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004295 RID: 17045 RVA: 0x00205D38 File Offset: 0x00203F38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEnchantPreview.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEnchantPreview.MethodName.RemoveExistingCards && args.Count == 0)
			{
				this.RemoveExistingCards();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004296 RID: 17046 RVA: 0x00205D98 File Offset: 0x00203F98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEnchantPreview.MethodName._Ready || (ref method) == NEnchantPreview.MethodName.RemoveExistingCards || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004297 RID: 17047 RVA: 0x00205DC0 File Offset: 0x00203FC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEnchantPreview.PropertyName._before)
			{
				this._before = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEnchantPreview.PropertyName._after)
			{
				this._after = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEnchantPreview.PropertyName._arrows)
			{
				this._arrows = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004298 RID: 17048 RVA: 0x00205E28 File Offset: 0x00204028
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEnchantPreview.PropertyName._before)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._before);
				return true;
			}
			if ((ref name) == NEnchantPreview.PropertyName._after)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._after);
				return true;
			}
			if ((ref name) == NEnchantPreview.PropertyName._arrows)
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
		// Token: 0x06004299 RID: 17049 RVA: 0x00205EA0 File Offset: 0x002040A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEnchantPreview.PropertyName._before, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnchantPreview.PropertyName._after, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEnchantPreview.PropertyName._arrows, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600429A RID: 17050 RVA: 0x00205F18 File Offset: 0x00204118
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEnchantPreview.PropertyName._before, Variant.From<Control>(ref this._before));
			info.AddProperty(NEnchantPreview.PropertyName._after, Variant.From<Control>(ref this._after));
			info.AddProperty(NEnchantPreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
		}

		/// <inheritdoc />
		// Token: 0x0600429B RID: 17051 RVA: 0x00205F70 File Offset: 0x00204170
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEnchantPreview.PropertyName._before, ref variant))
			{
				this._before = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEnchantPreview.PropertyName._after, ref variant2))
			{
				this._after = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEnchantPreview.PropertyName._arrows, ref variant3))
			{
				this._arrows = variant3.As<Control>();
			}
		}

		// Token: 0x040019FE RID: 6654
		[Nullable(1)]
		private Control _before;

		// Token: 0x040019FF RID: 6655
		[Nullable(1)]
		private Control _after;

		// Token: 0x04001A00 RID: 6656
		[Nullable(1)]
		private Control _arrows;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200194E RID: 6478
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040063FA RID: 25594
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RemoveExistingCards' method.
			/// </summary>
			// Token: 0x040063FB RID: 25595
			public static readonly StringName RemoveExistingCards = "RemoveExistingCards";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200194F RID: 6479
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_before' field.
			/// </summary>
			// Token: 0x040063FC RID: 25596
			public static readonly StringName _before = "_before";

			/// <summary>
			/// Cached name for the '_after' field.
			/// </summary>
			// Token: 0x040063FD RID: 25597
			public static readonly StringName _after = "_after";

			/// <summary>
			/// Cached name for the '_arrows' field.
			/// </summary>
			// Token: 0x040063FE RID: 25598
			public static readonly StringName _arrows = "_arrows";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001950 RID: 6480
		public class SignalName : Control.SignalName
		{
		}
	}
}
