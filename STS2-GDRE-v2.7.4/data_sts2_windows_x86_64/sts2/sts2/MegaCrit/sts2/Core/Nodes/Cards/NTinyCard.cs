using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	// Token: 0x02000418 RID: 1048
	[ScriptPath("res://src/Core/Nodes/Cards/NTinyCard.cs")]
	public class NTinyCard : NButton
	{
		// Token: 0x0600429D RID: 17053 RVA: 0x00205FE0 File Offset: 0x002041E0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._cardBack = base.GetNode<TextureRect>("%CardBack");
			this._cardPortrait = base.GetNode<TextureRect>("%Portrait");
			this._cardPortraitShadow = base.GetNode<TextureRect>("%PortraitShadow");
			this._cardBanner = base.GetNode<Control>("%Banner");
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0020604B File Offset: 0x0020424B
		[NullableContext(1)]
		public void SetCard(CardModel card)
		{
			this.SetCardBackColor(card.Pool);
			this.SetCardPortraitShape(card.Type);
			this.SetBannerColor(card.Rarity);
			this._cardBack.Material = card.FrameMaterial;
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00206082 File Offset: 0x00204282
		[NullableContext(1)]
		public void Set(CardPoolModel cardPool, CardType type, CardRarity rarity)
		{
			this.SetCardBackColor(cardPool);
			this.SetCardPortraitShape(type);
			this.SetBannerColor(rarity);
			this._cardBack.Material = cardPool.AllCards.First<CardModel>().FrameMaterial;
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x002060B4 File Offset: 0x002042B4
		[NullableContext(1)]
		private void SetCardBackColor(CardPoolModel cardPool)
		{
			this._cardBack.Modulate = cardPool.DeckEntryCardColor;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x002060C8 File Offset: 0x002042C8
		private void SetCardPortraitShape(CardType type)
		{
			if (type == CardType.Attack)
			{
				this._cardPortrait.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/attack_portrait.png");
				this._cardPortraitShadow.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/attack_portrait_shadow.png");
				return;
			}
			if (type != CardType.Power)
			{
				this._cardPortrait.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/skill_portrait.png");
				this._cardPortraitShadow.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/skill_portrait_shadow.png");
				return;
			}
			this._cardPortrait.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/power_portrait.png");
			this._cardPortraitShadow.Texture = PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/power_portrait_shadow.png");
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x0020617D File Offset: 0x0020437D
		private void SetBannerColor(CardRarity rarity)
		{
			this._cardBanner.Modulate = this.GetBannerColor(rarity);
		}

		/// <summary>
		/// A simple lookup function to fetch the correct banner color given the rarity of the card.
		/// Note that the colors used here are slightly more vivid than in-game to improve legibility.
		/// This is because the deck history entries are tiny icons.
		/// </summary>
		/// <param name="rarity"></param>
		/// <returns></returns>
		// Token: 0x060042A3 RID: 17059 RVA: 0x00206194 File Offset: 0x00204394
		private Color GetBannerColor(CardRarity rarity)
		{
			switch (rarity)
			{
			case CardRarity.Basic:
			case CardRarity.Common:
				return new Color("9C9C9CFF");
			case CardRarity.Uncommon:
				return new Color("64FFFFFF");
			case CardRarity.Rare:
				return new Color("FFDA36FF");
			case CardRarity.Event:
				return new Color("13BE1AFF");
			case CardRarity.Curse:
				return new Color("E669FFFF");
			case CardRarity.Quest:
				return new Color("F46836FF");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unspecified Rarity: ");
			defaultInterpolatedStringHandler.AppendFormatted<CardRarity>(rarity);
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return Colors.White;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042A4 RID: 17060 RVA: 0x00206248 File Offset: 0x00204448
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NTinyCard.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTinyCard.MethodName.SetCardPortraitShape, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "type", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTinyCard.MethodName.SetBannerColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTinyCard.MethodName.GetBannerColor, new PropertyInfo(20L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042A5 RID: 17061 RVA: 0x0020638C File Offset: 0x0020458C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTinyCard.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTinyCard.MethodName.SetCardPortraitShape && args.Count == 1)
			{
				this.SetCardPortraitShape(VariantUtils.ConvertTo<CardType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTinyCard.MethodName.SetBannerColor && args.Count == 1)
			{
				this.SetBannerColor(VariantUtils.ConvertTo<CardRarity>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTinyCard.MethodName.GetBannerColor && args.Count == 1)
			{
				Color bannerColor = this.GetBannerColor(VariantUtils.ConvertTo<CardRarity>(args[0]));
				ret = VariantUtils.CreateFrom<Color>(ref bannerColor);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060042A6 RID: 17062 RVA: 0x00206468 File Offset: 0x00204668
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTinyCard.MethodName._Ready || (ref method) == NTinyCard.MethodName.SetCardPortraitShape || (ref method) == NTinyCard.MethodName.SetBannerColor || (ref method) == NTinyCard.MethodName.GetBannerColor || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060042A7 RID: 17063 RVA: 0x002064B8 File Offset: 0x002046B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTinyCard.PropertyName._cardBack)
			{
				this._cardBack = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardPortrait)
			{
				this._cardPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardPortraitShadow)
			{
				this._cardPortraitShadow = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardBanner)
			{
				this._cardBanner = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060042A8 RID: 17064 RVA: 0x0020653C File Offset: 0x0020473C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTinyCard.PropertyName._cardBack)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._cardBack);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardPortrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._cardPortrait);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardPortraitShadow)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._cardPortraitShadow);
				return true;
			}
			if ((ref name) == NTinyCard.PropertyName._cardBanner)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardBanner);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042A9 RID: 17065 RVA: 0x002065D4 File Offset: 0x002047D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTinyCard.PropertyName._cardBack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTinyCard.PropertyName._cardPortrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTinyCard.PropertyName._cardPortraitShadow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTinyCard.PropertyName._cardBanner, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042AA RID: 17066 RVA: 0x0020666C File Offset: 0x0020486C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTinyCard.PropertyName._cardBack, Variant.From<TextureRect>(ref this._cardBack));
			info.AddProperty(NTinyCard.PropertyName._cardPortrait, Variant.From<TextureRect>(ref this._cardPortrait));
			info.AddProperty(NTinyCard.PropertyName._cardPortraitShadow, Variant.From<TextureRect>(ref this._cardPortraitShadow));
			info.AddProperty(NTinyCard.PropertyName._cardBanner, Variant.From<Control>(ref this._cardBanner));
		}

		/// <inheritdoc />
		// Token: 0x060042AB RID: 17067 RVA: 0x002066D8 File Offset: 0x002048D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTinyCard.PropertyName._cardBack, ref variant))
			{
				this._cardBack = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTinyCard.PropertyName._cardPortrait, ref variant2))
			{
				this._cardPortrait = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTinyCard.PropertyName._cardPortraitShadow, ref variant3))
			{
				this._cardPortraitShadow = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTinyCard.PropertyName._cardBanner, ref variant4))
			{
				this._cardBanner = variant4.As<Control>();
			}
		}

		// Token: 0x04001A01 RID: 6657
		[Nullable(1)]
		private TextureRect _cardBack;

		// Token: 0x04001A02 RID: 6658
		[Nullable(1)]
		private TextureRect _cardPortrait;

		// Token: 0x04001A03 RID: 6659
		[Nullable(1)]
		private TextureRect _cardPortraitShadow;

		// Token: 0x04001A04 RID: 6660
		[Nullable(1)]
		private Control _cardBanner;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001951 RID: 6481
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040063FF RID: 25599
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetCardPortraitShape' method.
			/// </summary>
			// Token: 0x04006400 RID: 25600
			public static readonly StringName SetCardPortraitShape = "SetCardPortraitShape";

			/// <summary>
			/// Cached name for the 'SetBannerColor' method.
			/// </summary>
			// Token: 0x04006401 RID: 25601
			public static readonly StringName SetBannerColor = "SetBannerColor";

			/// <summary>
			/// Cached name for the 'GetBannerColor' method.
			/// </summary>
			// Token: 0x04006402 RID: 25602
			public static readonly StringName GetBannerColor = "GetBannerColor";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001952 RID: 6482
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_cardBack' field.
			/// </summary>
			// Token: 0x04006403 RID: 25603
			public static readonly StringName _cardBack = "_cardBack";

			/// <summary>
			/// Cached name for the '_cardPortrait' field.
			/// </summary>
			// Token: 0x04006404 RID: 25604
			public static readonly StringName _cardPortrait = "_cardPortrait";

			/// <summary>
			/// Cached name for the '_cardPortraitShadow' field.
			/// </summary>
			// Token: 0x04006405 RID: 25605
			public static readonly StringName _cardPortraitShadow = "_cardPortraitShadow";

			/// <summary>
			/// Cached name for the '_cardBanner' field.
			/// </summary>
			// Token: 0x04006406 RID: 25606
			public static readonly StringName _cardBanner = "_cardBanner";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001953 RID: 6483
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
