using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004DC RID: 1244
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CardModel : AbstractModel
	{
		/// <summary>
		/// These values are constructor parameters rather than abstract properties to avoid virtual dispatch.
		/// Most cards have constant values for these properties, so storing them in fields eliminates the
		/// overhead of virtual method calls on every access. Cards with dynamic behavior (e.g., MadScience)
		/// can still override the virtual properties.
		/// </summary>
		// Token: 0x06004AE5 RID: 19173 RVA: 0x00228205 File Offset: 0x00226405
		protected CardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true)
		{
			this.CanonicalEnergyCost = canonicalEnergyCost;
			this.Type = type;
			this.Rarity = rarity;
			this.TargetType = targetType;
			this.ShouldShowInCardLibrary = shouldShowInCardLibrary;
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06004AE6 RID: 19174 RVA: 0x00228240 File Offset: 0x00226440
		// (remove) Token: 0x06004AE7 RID: 19175 RVA: 0x00228278 File Offset: 0x00226478
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action AfflictionChanged;

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06004AE8 RID: 19176 RVA: 0x002282B0 File Offset: 0x002264B0
		// (remove) Token: 0x06004AE9 RID: 19177 RVA: 0x002282E8 File Offset: 0x002264E8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action EnchantmentChanged;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06004AEA RID: 19178 RVA: 0x00228320 File Offset: 0x00226520
		// (remove) Token: 0x06004AEB RID: 19179 RVA: 0x00228358 File Offset: 0x00226558
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action EnergyCostChanged;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06004AEC RID: 19180 RVA: 0x00228390 File Offset: 0x00226590
		// (remove) Token: 0x06004AED RID: 19181 RVA: 0x002283C8 File Offset: 0x002265C8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action KeywordsChanged;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06004AEE RID: 19182 RVA: 0x00228400 File Offset: 0x00226600
		// (remove) Token: 0x06004AEF RID: 19183 RVA: 0x00228438 File Offset: 0x00226638
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action ReplayCountChanged;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06004AF0 RID: 19184 RVA: 0x00228470 File Offset: 0x00226670
		// (remove) Token: 0x06004AF1 RID: 19185 RVA: 0x002284A8 File Offset: 0x002266A8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Played;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06004AF2 RID: 19186 RVA: 0x002284E0 File Offset: 0x002266E0
		// (remove) Token: 0x06004AF3 RID: 19187 RVA: 0x00228518 File Offset: 0x00226718
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Drawn;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06004AF4 RID: 19188 RVA: 0x00228550 File Offset: 0x00226750
		// (remove) Token: 0x06004AF5 RID: 19189 RVA: 0x00228588 File Offset: 0x00226788
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action StarCostChanged;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06004AF6 RID: 19190 RVA: 0x002285C0 File Offset: 0x002267C0
		// (remove) Token: 0x06004AF7 RID: 19191 RVA: 0x002285F8 File Offset: 0x002267F8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Upgraded;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06004AF8 RID: 19192 RVA: 0x00228630 File Offset: 0x00226830
		// (remove) Token: 0x06004AF9 RID: 19193 RVA: 0x00228668 File Offset: 0x00226868
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Forged;

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x002286A0 File Offset: 0x002268A0
		public LocString TitleLocString
		{
			get
			{
				LocString locString;
				if ((locString = this._titleLocString) == null)
				{
					locString = (this._titleLocString = new LocString("cards", base.Id.Entry + ".title"));
				}
				return locString;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x002286E0 File Offset: 0x002268E0
		public virtual string Title
		{
			get
			{
				LocString titleLocString = this.TitleLocString;
				if (!this.IsUpgraded)
				{
					return titleLocString.GetFormattedText();
				}
				if (this.MaxUpgradeLevel > 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(titleLocString.GetFormattedText());
					defaultInterpolatedStringHandler.AppendLiteral("+");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentUpgradeLevel);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return titleLocString.GetFormattedText() + "+";
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004AFC RID: 19196 RVA: 0x00228753 File Offset: 0x00226953
		public LocString Description
		{
			get
			{
				return new LocString("cards", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x00228774 File Offset: 0x00226974
		protected LocString SelectionScreenPrompt
		{
			get
			{
				LocString locString = new LocString("cards", base.Id.Entry + ".selectionScreenPrompt");
				if (!locString.Exists())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
					defaultInterpolatedStringHandler.AppendLiteral("No selection screen prompt for ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.DynamicVars.AddTo(locString);
				return locString;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x002287F4 File Offset: 0x002269F4
		public virtual string PortraitPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("atlases/card_atlas.sprites/");
				defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral(".tres");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06004AFF RID: 19199 RVA: 0x0022886C File Offset: 0x00226A6C
		public virtual string BetaPortraitPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("atlases/card_atlas.sprites/");
				defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/beta/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral(".tres");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06004B00 RID: 19200 RVA: 0x002288E1 File Offset: 0x00226AE1
		public static string MissingPortraitPath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x002288F0 File Offset: 0x00226AF0
		protected virtual string PortraitPngPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("packed/card_portraits/");
				defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06004B02 RID: 19202 RVA: 0x00228968 File Offset: 0x00226B68
		private string BetaPortraitPngPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("packed/card_portraits/");
				defaultInterpolatedStringHandler.AppendFormatted(this.Pool.Title.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/beta/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x002289DD File Offset: 0x00226BDD
		public bool HasPortrait
		{
			get
			{
				return ResourceLoader.Exists(this.PortraitPngPath, "");
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x002289EF File Offset: 0x00226BEF
		public bool HasBetaPortrait
		{
			get
			{
				return ResourceLoader.Exists(this.BetaPortraitPngPath, "");
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x00228A01 File Offset: 0x00226C01
		public Texture2D Portrait
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PortraitPath, null, 1L);
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004B06 RID: 19206 RVA: 0x00228A14 File Offset: 0x00226C14
		private string FramePath
		{
			get
			{
				CardType cardType;
				switch (this.Type)
				{
				case CardType.None:
				case CardType.Status:
				case CardType.Curse:
					cardType = CardType.Skill;
					break;
				case CardType.Attack:
				case CardType.Skill:
				case CardType.Power:
				case CardType.Quest:
					cardType = this.Type;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				if (this.Rarity != CardRarity.Ancient)
				{
					return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_frame_" + cardType.ToString().ToLowerInvariant() + "_s.tres");
				}
				return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004B07 RID: 19207 RVA: 0x00228A99 File Offset: 0x00226C99
		public Texture2D Frame
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.FramePath, null, 1L);
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x00228AAC File Offset: 0x00226CAC
		private string PortraitBorderPath
		{
			get
			{
				CardType cardType;
				switch (this.Type)
				{
				case CardType.None:
				case CardType.Status:
				case CardType.Curse:
				case CardType.Quest:
					cardType = CardType.Skill;
					break;
				case CardType.Attack:
				case CardType.Skill:
				case CardType.Power:
					cardType = this.Type;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_portrait_border_" + cardType.ToString().ToLowerInvariant() + "_s.tres");
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x00228B1D File Offset: 0x00226D1D
		private static string AncientBorderPath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/compressed_atlas.sprites/ancient_card_border.png.tres");
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004B0A RID: 19210 RVA: 0x00228B2C File Offset: 0x00226D2C
		private string AncientTextBgPath
		{
			get
			{
				if (this.Rarity != CardRarity.Ancient)
				{
					throw new InvalidOperationException("This card is not an ancient card.");
				}
				CardType cardType;
				switch (this.Type)
				{
				case CardType.None:
				case CardType.Status:
				case CardType.Curse:
					cardType = CardType.Skill;
					break;
				case CardType.Attack:
				case CardType.Skill:
				case CardType.Power:
				case CardType.Quest:
					cardType = this.Type;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				CardType cardType2 = cardType;
				return ImageHelper.GetImagePath("atlases/compressed_atlas.sprites/ancient_text_bg_" + cardType2.ToString().ToLowerInvariant() + ".png.tres");
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x00228BB3 File Offset: 0x00226DB3
		public Texture2D AncientTextBg
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.AncientTextBgPath, null, 1L);
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x00228BC3 File Offset: 0x00226DC3
		public Texture2D AncientBorder
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(CardModel.AncientBorderPath, null, 1L);
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004B0D RID: 19213 RVA: 0x00228BD2 File Offset: 0x00226DD2
		public Texture2D PortraitBorder
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PortraitBorderPath, null, 1L);
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004B0E RID: 19214 RVA: 0x00228BE2 File Offset: 0x00226DE2
		private string EnergyIconPath
		{
			get
			{
				return this.VisualCardPool.EnergyIconPath;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004B0F RID: 19215 RVA: 0x00228BEF File Offset: 0x00226DEF
		public Texture2D EnergyIcon
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.EnergyIconPath, null, 1L);
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00228BFF File Offset: 0x00226DFF
		protected IHoverTip EnergyHoverTip
		{
			get
			{
				return HoverTipFactory.ForEnergy(this);
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004B11 RID: 19217 RVA: 0x00228C07 File Offset: 0x00226E07
		private string BannerTexturePath
		{
			get
			{
				if (this.Rarity != CardRarity.Ancient)
				{
					return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner.tres");
				}
				return ImageHelper.GetImagePath("atlases/ui_atlas.sprites/card/card_banner_ancient_s.tres");
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004B12 RID: 19218 RVA: 0x00228C27 File Offset: 0x00226E27
		public Texture2D BannerTexture
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.BannerTexturePath, null, 1L);
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004B13 RID: 19219 RVA: 0x00228C38 File Offset: 0x00226E38
		private string BannerMaterialPath
		{
			get
			{
				switch (this.Rarity)
				{
				case CardRarity.Uncommon:
					return "res://materials/cards/banners/card_banner_uncommon_mat.tres";
				case CardRarity.Rare:
					return "res://materials/cards/banners/card_banner_rare_mat.tres";
				case CardRarity.Ancient:
					return "res://materials/cards/banners/card_banner_ancient_mat.tres";
				case CardRarity.Event:
					return "res://materials/cards/banners/card_banner_event_mat.tres";
				case CardRarity.Status:
					return "res://materials/cards/banners/card_banner_status_mat.tres";
				case CardRarity.Curse:
					return "res://materials/cards/banners/card_banner_curse_mat.tres";
				case CardRarity.Quest:
					return "res://materials/cards/banners/card_banner_quest_mat.tres";
				}
				return "res://materials/cards/banners/card_banner_common_mat.tres";
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06004B14 RID: 19220 RVA: 0x00228CB5 File Offset: 0x00226EB5
		public Material BannerMaterial
		{
			get
			{
				return PreloadManager.Cache.GetMaterial(this.BannerMaterialPath);
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x00228CC7 File Offset: 0x00226EC7
		public Material FrameMaterial
		{
			get
			{
				return this.VisualCardPool.FrameMaterial;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004B16 RID: 19222 RVA: 0x00228CD4 File Offset: 0x00226ED4
		public virtual CardType Type { get; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x00228CDC File Offset: 0x00226EDC
		public virtual CardRarity Rarity { get; }

		/// <summary>
		/// Manages card restrictions based on how many players there are in a run.
		/// </summary>
		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004B18 RID: 19224 RVA: 0x00228CE4 File Offset: 0x00226EE4
		public virtual CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.None;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00228CE8 File Offset: 0x00226EE8
		public virtual CardPoolModel Pool
		{
			get
			{
				if (this._pool != null)
				{
					return this._pool;
				}
				this._pool = ModelDb.AllCardPools.FirstOrDefault<CardPoolModel>((CardPoolModel pool) => pool.AllCardIds.Contains(base.Id));
				if (this._pool != null)
				{
					return this._pool;
				}
				if (ModelDb.CardPool<MockCardPool>().AllCardIds.Contains(base.Id))
				{
					this._pool = ModelDb.CardPool<MockCardPool>();
					return this._pool;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" is not in any card pool!");
				throw new InvalidProgramException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		/// <summary>
		/// Visually what pool we want the card to be from. Normally this is the same as the regular pool.
		/// It is not the case when we want it to be part of one pool, but look like its from another pool
		/// ie Trash Heap cards are event cards, but we want the colors to reflect their original character cards from sts1
		/// </summary>
		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x00228D8E File Offset: 0x00226F8E
		public virtual CardPoolModel VisualCardPool
		{
			get
			{
				return this.Pool;
			}
		}

		/// <summary>
		/// Get the Player that this card belongs to.
		/// Will technically be null on a canonical card model and in certain edge-case timing moments (end of combat before
		/// transitioning to the next room), but we should rarely be checking that, so we leave this as non-nullable for
		/// convenience in 99% of cases.
		/// If you really need to check for null, override the warning with a comment.
		/// </summary>
		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x00228D96 File Offset: 0x00226F96
		// (set) Token: 0x06004B1C RID: 19228 RVA: 0x00228DA4 File Offset: 0x00226FA4
		public Player Owner
		{
			get
			{
				base.AssertMutable();
				return this._owner;
			}
			set
			{
				base.AssertMutable();
				if (this._owner != null && value != null)
				{
					throw new InvalidOperationException("Card " + base.Id.Entry + " already has an owner.");
				}
				this._owner = value;
			}
		}

		/// <summary>
		/// The pile which this card is in.
		/// Can be null when used by Card HoverTips, Card Rewards, Card Library, things like that.
		/// </summary>
		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x00228DDE File Offset: 0x00226FDE
		[Nullable(2)]
		public CardPile Pile
		{
			[NullableContext(2)]
			get
			{
				Player owner = this._owner;
				if (owner == null)
				{
					return null;
				}
				return owner.Piles.FirstOrDefault<CardPile>((CardPile p) => p.Cards.Contains(this));
			}
		}

		/// <summary>
		/// This card's "official" starting energy cost.
		/// This is what would appear on the card if it was printed out on paper.
		///
		/// Note: If you want to check a card's canonical energy cost, use <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.Canonical" /> instead.
		/// </summary>
		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x00228E02 File Offset: 0x00227002
		protected virtual int CanonicalEnergyCost { get; }

		/// <summary>
		/// Whether this card has an energy cost of X.
		/// X-cost-cards automatically spend all of the player's remaining energy when played, and their effect is
		/// multiplied by the amount spent.
		///
		/// Note: This exists on CardModel for the purposes of overriding in subclasses. If you want to check if a card has
		/// an energy cost of X, use <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.CostsX" /> instead.
		/// </summary>
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x00228E0A File Offset: 0x0022700A
		protected virtual bool HasEnergyCostX
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// WARNING: Only use this in tests.
		/// Set this card's energy cost.
		/// </summary>
		// Token: 0x06004B20 RID: 19232 RVA: 0x00228E0D File Offset: 0x0022700D
		protected void MockSetEnergyCost(CardEnergyCost cost)
		{
			this._energyCost = cost;
		}

		/// <summary>
		/// All energy-cost-related information and logic about this card.
		/// </summary>
		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x00228E16 File Offset: 0x00227016
		public CardEnergyCost EnergyCost
		{
			get
			{
				if (this._energyCost == null)
				{
					this._energyCost = new CardEnergyCost(this, this.CanonicalEnergyCost, this.HasEnergyCostX);
				}
				return this._energyCost;
			}
		}

		/// <summary>
		/// Internal method for <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost" /> to invoke the EnergyCostChanged event.
		/// </summary>
		// Token: 0x06004B22 RID: 19234 RVA: 0x00228E3E File Offset: 0x0022703E
		public void InvokeEnergyCostChanged()
		{
			Action energyCostChanged = this.EnergyCostChanged;
			if (energyCostChanged == null)
			{
				return;
			}
			energyCostChanged();
		}

		/// <summary>
		/// Resolve this card's X energy value.
		/// Takes modifications to X values (like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.ChemicalX" />) into account.
		/// </summary>
		// Token: 0x06004B23 RID: 19235 RVA: 0x00228E50 File Offset: 0x00227050
		public int ResolveEnergyXValue()
		{
			if (!this.EnergyCost.CostsX)
			{
				throw new InvalidOperationException("This card does not have an X-cost.");
			}
			return Hook.ModifyXValue(this.CombatState, this, this.EnergyCost.CapturedXValue);
		}

		/// <summary>
		/// The number of extra times this card's logic should be executed when it's played, excluding any effects from
		/// other models.
		/// Defaults to 0, but various effects can permanently set this to a higher value.
		/// </summary>
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x00228E81 File Offset: 0x00227081
		// (set) Token: 0x06004B25 RID: 19237 RVA: 0x00228E89 File Offset: 0x00227089
		public int BaseReplayCount
		{
			get
			{
				return this._baseReplayCount;
			}
			set
			{
				base.AssertMutable();
				this._baseReplayCount = value;
				Action replayCountChanged = this.ReplayCountChanged;
				if (replayCountChanged == null)
				{
					return;
				}
				replayCountChanged();
			}
		}

		/// <summary>
		/// The number of extra times this card's logic should be executed when it's played, including any effects from
		/// this card's enchantment if it has one.
		/// </summary>
		// Token: 0x06004B26 RID: 19238 RVA: 0x00228EA8 File Offset: 0x002270A8
		public int GetEnchantedReplayCount()
		{
			EnchantmentModel enchantment = this.Enchantment;
			if (enchantment == null)
			{
				return this.BaseReplayCount;
			}
			return enchantment.EnchantPlayCount(this.BaseReplayCount);
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x00228EC6 File Offset: 0x002270C6
		public virtual int CanonicalStarCost
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x00228EC9 File Offset: 0x002270C9
		// (set) Token: 0x06004B29 RID: 19241 RVA: 0x00228EFB File Offset: 0x002270FB
		public int BaseStarCost
		{
			get
			{
				if (!base.IsMutable)
				{
					return this.CanonicalStarCost;
				}
				if (!this._starCostSet)
				{
					this._baseStarCost = this.CanonicalStarCost;
					this._starCostSet = true;
				}
				return this._baseStarCost;
			}
			private set
			{
				base.AssertMutable();
				if (!this.HasStarCostX)
				{
					this._baseStarCost = value;
					this._starCostSet = true;
				}
				Action starCostChanged = this.StarCostChanged;
				if (starCostChanged == null)
				{
					return;
				}
				starCostChanged();
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00228F29 File Offset: 0x00227129
		public bool WasStarCostJustUpgraded
		{
			get
			{
				return this._wasStarCostJustUpgraded;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x00228F31 File Offset: 0x00227131
		[Nullable(2)]
		public TemporaryCardCost TemporaryStarCost
		{
			[NullableContext(2)]
			get
			{
				return this._temporaryStarCosts.LastOrDefault<TemporaryCardCost>();
			}
		}

		/// <summary>
		/// Get this card's current star cost.
		///
		/// This works just like <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.GetWithModifiers(MegaCrit.Sts2.Core.Entities.Cards.CostModifiers)" /> passing <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers.Local" /> but
		/// for stars, and with one exception.
		/// If the card had no star cost (it was negative) and it is temporarily set to zero, then we still treat the card
		/// as if it has no star cost, so that it still doesn't show up on the card.
		/// </summary>
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004B2C RID: 19244 RVA: 0x00228F40 File Offset: 0x00227140
		public virtual int CurrentStarCost
		{
			get
			{
				TemporaryCardCost temporaryCardCost = this._temporaryStarCosts.LastOrDefault<TemporaryCardCost>();
				int? num = ((temporaryCardCost != null) ? new int?(temporaryCardCost.Cost) : null);
				if (num == null)
				{
					return this.BaseStarCost;
				}
				int? num2 = num;
				int num3 = 0;
				if (((num2.GetValueOrDefault() == num3) & (num2 != null)) && this.BaseStarCost < 0)
				{
					return this.BaseStarCost;
				}
				return num.Value;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x00228FB1 File Offset: 0x002271B1
		public virtual bool HasStarCostX
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// The amount of stars most recently spent to play this card.
		/// Used when duplicating X-cost cards, to make sure the duplicates are played with the same value.
		///
		/// WARNING: Only use this for calculations related to stars spent. If you're using this to calculate a cost-X-stars
		/// card's effect, use <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.ResolveStarXValue" /> instead, as it will take X-value modifications (like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.ChemicalX" />) into account.
		/// </summary>
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00228FB4 File Offset: 0x002271B4
		// (set) Token: 0x06004B2F RID: 19247 RVA: 0x00228FBC File Offset: 0x002271BC
		public int LastStarsSpent
		{
			get
			{
				return this._lastStarsSpent;
			}
			set
			{
				base.AssertMutable();
				this._lastStarsSpent = value;
			}
		}

		/// <summary>
		/// Resolve this card's X star value.
		/// Takes modifications to X values (like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.ChemicalX" />) into account.
		/// </summary>
		// Token: 0x06004B30 RID: 19248 RVA: 0x00228FCB File Offset: 0x002271CB
		public int ResolveStarXValue()
		{
			if (!this.HasStarCostX)
			{
				throw new InvalidOperationException("This card does not have an X-cost.");
			}
			return Hook.ModifyXValue(this.CombatState, this, this.LastStarsSpent);
		}

		/// <summary>
		/// Defines who what creature we should be targeting/highlighting. Some examples:
		/// - AnyEnemy means that this is a single target attack.
		/// - AllEnemies: this is an AOE attack so highlight all valid enemies
		/// - Self: This is a power applied to the player, highlight the players character
		/// </summary>
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x00228FF2 File Offset: 0x002271F2
		public virtual TargetType TargetType { get; }

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004B32 RID: 19250 RVA: 0x00228FFA File Offset: 0x002271FA
		public virtual IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return Array.Empty<CardKeyword>();
			}
		}

		/// <summary>
		/// The keywords applied directly to this card: its canonical keywords, plus any added by <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.AddKeyword(MegaCrit.Sts2.Core.Entities.Cards.CardKeyword)" />,
		/// minus any removed by <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.RemoveKeyword(MegaCrit.Sts2.Core.Entities.Cards.CardKeyword)" />. This is the persistent, instance-owned keyword state; it is
		/// what gets cloned, and is unaffected by other models in the combat state.
		/// See <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.KeywordSources.Local" /> for more details.
		/// </summary>
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x00229001 File Offset: 0x00227201
		private HashSet<CardKeyword> LocalKeywords
		{
			get
			{
				if (this._keywords != null)
				{
					return this._keywords;
				}
				this._keywords = new HashSet<CardKeyword>();
				this._keywords.UnionWith(this.CanonicalKeywords);
				return this._keywords;
			}
		}

		/// <summary>
		/// This card's current keywords, including both local keywords (<see cref="F:MegaCrit.Sts2.Core.Entities.Cards.KeywordSources.Local" />) and any
		/// global keywords (<see cref="F:MegaCrit.Sts2.Core.Entities.Cards.KeywordSources.Global" />) granted by other models in the combat state (e.g.
		/// Ethereal from <see cref="T:MegaCrit.Sts2.Core.Models.Powers.HexPower" />). See <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.GetKeywordsWithSources(MegaCrit.Sts2.Core.Entities.Cards.KeywordSources)" />.
		/// </summary>
		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x00229034 File Offset: 0x00227234
		public IReadOnlySet<CardKeyword> Keywords
		{
			get
			{
				return this.GetKeywordsWithSources(KeywordSources.All);
			}
		}

		/// <summary>
		/// Get this card's keywords, including the specified source types.
		/// This mirrors <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.GetWithModifiers(MegaCrit.Sts2.Core.Entities.Cards.CostModifiers)" />: local keywords live on the card, while global
		/// keywords are computed on demand from other models in the combat state and are never stored.
		/// </summary>
		// Token: 0x06004B35 RID: 19253 RVA: 0x00229040 File Offset: 0x00227240
		public IReadOnlySet<CardKeyword> GetKeywordsWithSources(KeywordSources sources)
		{
			bool flag = sources.HasFlag(KeywordSources.Local);
			if (sources.HasFlag(KeywordSources.Global) && !base.IsCanonical && this.CombatState != null)
			{
				HashSet<CardKeyword> hashSet2;
				if (flag)
				{
					HashSet<CardKeyword> hashSet = new HashSet<CardKeyword>();
					foreach (CardKeyword cardKeyword in this.LocalKeywords)
					{
						hashSet.Add(cardKeyword);
					}
					hashSet2 = hashSet;
				}
				else
				{
					hashSet2 = new HashSet<CardKeyword>();
				}
				HashSet<CardKeyword> hashSet3 = hashSet2;
				Hook.ModifyKeywordsInCombat(this.CombatState, this, hashSet3);
				return hashSet3;
			}
			if (!flag)
			{
				return ImmutableHashSet<CardKeyword>.Empty;
			}
			return this.LocalKeywords;
		}

		/// <summary>
		/// This card's tags. See <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.CardTag" /> for details on how to use these.
		///
		/// NOTE: The current implementation assumes a card's tags will never change. If we ever need to dynamically add or
		/// remove tags after creation, change this to work more like Keywords.
		/// </summary>
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x00229108 File Offset: 0x00227308
		public virtual IEnumerable<CardTag> Tags
		{
			get
			{
				HashSet<CardTag> hashSet;
				if ((hashSet = this._tags) == null)
				{
					hashSet = (this._tags = this.CanonicalTags);
				}
				return hashSet;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x0022912E File Offset: 0x0022732E
		protected virtual HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag>();
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x00229135 File Offset: 0x00227335
		public DynamicVarSet DynamicVars
		{
			get
			{
				if (this._dynamicVars != null)
				{
					return this._dynamicVars;
				}
				this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
				this._dynamicVars.InitializeWithOwner(this);
				return this._dynamicVars;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x00229169 File Offset: 0x00227369
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		/// <summary>
		/// This is used for cards like Havoc and Cinder to mark cards that should be sent to the exhaust pile instead
		/// of the discard pile on their next play.
		///
		/// TODO: This might be kind of a hacky solution. We should look into other ways to pass this info along.
		/// </summary>
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x00229170 File Offset: 0x00227370
		// (set) Token: 0x06004B3B RID: 19259 RVA: 0x00229178 File Offset: 0x00227378
		public bool ExhaustOnNextPlay
		{
			get
			{
				return this._exhaustOnNextPlay;
			}
			set
			{
				base.AssertMutable();
				this._exhaustOnNextPlay = value;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x00229187 File Offset: 0x00227387
		// (set) Token: 0x06004B3D RID: 19261 RVA: 0x0022918F File Offset: 0x0022738F
		private bool HasSingleTurnRetain
		{
			get
			{
				return this._hasSingleTurnRetain;
			}
			set
			{
				base.AssertMutable();
				this._hasSingleTurnRetain = value;
			}
		}

		/// <summary>
		/// Should this card be retained this turn?
		/// True if the card has the Retain keyword, or if some effect like Expertise has made it retain for a single
		/// turn and it's still that turn.
		/// </summary>
		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004B3E RID: 19262 RVA: 0x0022919E File Offset: 0x0022739E
		public bool ShouldRetainThisTurn
		{
			get
			{
				return this.Keywords.Contains(CardKeyword.Retain) || this.HasSingleTurnRetain;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x002291B6 File Offset: 0x002273B6
		// (set) Token: 0x06004B40 RID: 19264 RVA: 0x002291BE File Offset: 0x002273BE
		private bool HasSingleTurnSly
		{
			get
			{
				return this._hasSingleTurnSly;
			}
			set
			{
				base.AssertMutable();
				this._hasSingleTurnSly = value;
			}
		}

		/// <summary>
		/// Is this card Sly this turn?
		/// True if the card has the Sly keyword, or if some effect like Hand Trick has made it Sly for a single turn.
		/// </summary>
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x002291CD File Offset: 0x002273CD
		public bool IsSlyThisTurn
		{
			get
			{
				return this.Keywords.Contains(CardKeyword.Sly) || this.HasSingleTurnSly;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x002291E5 File Offset: 0x002273E5
		// (set) Token: 0x06004B43 RID: 19267 RVA: 0x002291ED File Offset: 0x002273ED
		[Nullable(2)]
		public EnchantmentModel Enchantment
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004B44 RID: 19268 RVA: 0x002291F6 File Offset: 0x002273F6
		// (set) Token: 0x06004B45 RID: 19269 RVA: 0x002291FE File Offset: 0x002273FE
		[Nullable(2)]
		public AfflictionModel Affliction
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// Whether or not playing this card can heal the player or their pets, either directly (like Feed) or by giving
		/// you something else that can heal you (like Alchemize giving you a Regen Potion).
		///
		/// Used primarily to filter cards out of random in-combat generation effects, to avoid making annoying
		/// "optimal play" behavior.
		/// </summary>
		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x00229207 File Offset: 0x00227407
		public virtual bool CanBeGeneratedInCombat
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Used to filter cards out of modifier pools. Ancient curses and Ascender's Bane should not be generated by Cursed
		/// Run.
		/// </summary>
		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004B47 RID: 19271 RVA: 0x0022920A File Offset: 0x0022740A
		public virtual bool CanBeGeneratedByModifiers
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Manages if and how a card will Evoke an Orb.
		/// Applies to cards like Dualcast, which say "Evoke your next Orb".
		/// Does NOT apply to cards that may evoke an Orb as a side-effect (like Zap's channeling causing an Orb to evoke).
		///
		/// Used primarily to visually update the numbers displayed on your Orbs to reflect their Evoke values while
		/// dragging a card.
		/// </summary>
		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004B48 RID: 19272 RVA: 0x0022920D File Offset: 0x0022740D
		public virtual OrbEvokeType OrbEvokeType
		{
			get
			{
				return OrbEvokeType.None;
			}
		}

		/// <summary>
		/// Whether or not playing this card gains you block immediately.
		/// True for cards like Defend and Survivor.
		/// False for cards like Shadowmeld (which doesn't gain you block until you play an attack after).
		///
		/// Used for things like filtering the Nimble enchantment's targets.
		/// Also automatically sets the block HoverTip.
		///
		/// Note: Don't convert this into a CardTag. It's used by the system for automatic Osty targeting behavior.
		/// </summary>
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004B49 RID: 19273 RVA: 0x00229210 File Offset: 0x00227410
		public virtual bool GainsBlock
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Is this card one of the basic Strikes or Defends that you start the game with?
		///
		/// Used for things like Pandora's Box that only operate on these specific starter cards.
		/// </summary>
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06004B4A RID: 19274 RVA: 0x00229213 File Offset: 0x00227413
		public virtual bool IsBasicStrikeOrDefend
		{
			get
			{
				return this.Rarity == CardRarity.Basic && (this.Tags.Contains(CardTag.Strike) || this.Tags.Contains(CardTag.Defend));
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004B4B RID: 19275 RVA: 0x00229241 File Offset: 0x00227441
		[Nullable(2)]
		public CardModel CloneOf
		{
			[NullableContext(2)]
			get
			{
				return this._cloneOf;
			}
		}

		/// <summary>
		/// Whether or not this card is considered a clone. See <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.CloneOf" /> for more info on clones.
		/// </summary>
		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004B4C RID: 19276 RVA: 0x00229249 File Offset: 0x00227449
		public bool IsClone
		{
			get
			{
				return this.CloneOf != null;
			}
		}

		/// <summary>
		/// The card that this is a duplicate of. Null when this card is not a duplicate (which is most of the time).
		///
		/// Dupes behave slightly differently from original cards in some situations. For example:
		/// * After playing a dupe, it is sent to the Limbo pile, rather than Discard/Exhaust.
		/// * Dupes cannot be further duplicated by effects like Duplication Potion.
		/// * If an X-cost card is duped, the dupe retains the X-value from the original.
		/// </summary>
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004B4D RID: 19277 RVA: 0x00229254 File Offset: 0x00227454
		[Nullable(2)]
		public CardModel DupeOf
		{
			[NullableContext(2)]
			get
			{
				if (!this.IsDupe)
				{
					return null;
				}
				return this.CloneOf;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004B4E RID: 19278 RVA: 0x00229266 File Offset: 0x00227466
		// (set) Token: 0x06004B4F RID: 19279 RVA: 0x0022926E File Offset: 0x0022746E
		public bool IsDupe
		{
			get
			{
				return this._isDupe;
			}
			private set
			{
				base.AssertMutable();
				this._isDupe = value;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06004B50 RID: 19280 RVA: 0x0022927D File Offset: 0x0022747D
		public bool IsRemovable
		{
			get
			{
				return !this.Keywords.Contains(CardKeyword.Eternal);
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x00229290 File Offset: 0x00227490
		public bool IsTransformable
		{
			get
			{
				if (!this.IsRemovable)
				{
					CardPile pile = this.Pile;
					return pile == null || pile.Type != PileType.Deck;
				}
				return true;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06004B52 RID: 19282 RVA: 0x002292C0 File Offset: 0x002274C0
		public bool IsInCombat
		{
			get
			{
				if (base.IsMutable)
				{
					CardPile pile = this.Pile;
					return pile != null && pile.IsCombatPile;
				}
				return false;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06004B53 RID: 19283 RVA: 0x002292E9 File Offset: 0x002274E9
		// (set) Token: 0x06004B54 RID: 19284 RVA: 0x002292F4 File Offset: 0x002274F4
		public int CurrentUpgradeLevel
		{
			get
			{
				return this._currentUpgradeLevel;
			}
			private set
			{
				base.AssertMutable();
				if (value > this.MaxUpgradeLevel)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" cannot be upgraded past its MaxUpgradeLevel.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._currentUpgradeLevel = value;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06004B55 RID: 19285 RVA: 0x00229347 File Offset: 0x00227547
		public virtual int MaxUpgradeLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06004B56 RID: 19286 RVA: 0x0022934A File Offset: 0x0022754A
		public bool IsUpgraded
		{
			get
			{
				return this.CurrentUpgradeLevel > 0;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004B57 RID: 19287 RVA: 0x00229355 File Offset: 0x00227555
		public bool IsUpgradable
		{
			get
			{
				return this.CurrentUpgradeLevel < this.MaxUpgradeLevel;
			}
		}

		/// <summary>
		/// Is this card currently appearing as a previewed upgrade in <see cref="T:MegaCrit.Sts2.Core.Nodes.Cards.NUpgradePreview" />?
		/// And if so, what type of upgrade preview is it?
		/// This is to facilitate having upgrade previews reflect power values from the player in combat (i.e. Armaments).
		/// </summary>
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004B58 RID: 19288 RVA: 0x00229368 File Offset: 0x00227568
		// (set) Token: 0x06004B59 RID: 19289 RVA: 0x00229370 File Offset: 0x00227570
		public CardUpgradePreviewType UpgradePreviewType
		{
			get
			{
				return this._upgradePreviewType;
			}
			set
			{
				base.AssertMutable();
				if (!value.IsPreview() && this._upgradePreviewType.IsPreview())
				{
					throw new InvalidOperationException("A card cannot go to from being upgrade preview. Consider making a new card model instead.");
				}
				this._upgradePreviewType = value;
			}
		}

		/// <summary>
		/// Override this property to add extra conditions to check before allowing play.
		/// For example, Grand Finale is only playable if your draw pile is empty, so it would override this.
		/// </summary>
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06004B5A RID: 19290 RVA: 0x0022939F File Offset: 0x0022759F
		protected virtual bool IsPlayable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Set via constructor parameter to block a card from appearing in the Card Library screen.
		/// </summary>
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06004B5B RID: 19291 RVA: 0x002293A2 File Offset: 0x002275A2
		public bool ShouldShowInCardLibrary { get; }

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06004B5C RID: 19292 RVA: 0x002293AA File Offset: 0x002275AA
		public bool ShouldGlowGold
		{
			get
			{
				if (!this.ShouldGlowGoldInternal)
				{
					EnchantmentModel enchantment = this.Enchantment;
					return enchantment != null && enchantment.ShouldGlowGold;
				}
				return true;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x002293C7 File Offset: 0x002275C7
		public bool ShouldGlowRed
		{
			get
			{
				if (!this.ShouldGlowRedInternal)
				{
					EnchantmentModel enchantment = this.Enchantment;
					return enchantment != null && enchantment.ShouldGlowRed;
				}
				return true;
			}
		}

		/// <summary>
		/// Override this property to add conditions to check to determine whether to show a gold glow on this card.
		/// For example, Evil Eye adds 6 extra block if you've exhausted a card this turn, so it would override this.
		/// </summary>
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004B5E RID: 19294 RVA: 0x002293E4 File Offset: 0x002275E4
		protected virtual bool ShouldGlowGoldInternal
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Override this property to add conditions to check to determine whether to show a red glow on this card.
		/// For example, Normality should glow red when it blocks card plays.
		/// </summary>
		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x002293E7 File Offset: 0x002275E7
		protected virtual bool ShouldGlowRedInternal
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Is this card currently appearing as a previewed enchanted card in <see cref="T:MegaCrit.Sts2.Core.Nodes.Cards.NEnchantPreview" />?
		/// </summary>
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x002293EA File Offset: 0x002275EA
		// (set) Token: 0x06004B61 RID: 19297 RVA: 0x002293F2 File Offset: 0x002275F2
		public bool IsEnchantmentPreview
		{
			get
			{
				return this._isEnchantmentPreview;
			}
			set
			{
				base.AssertMutable();
				this._isEnchantmentPreview = value;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x00229401 File Offset: 0x00227601
		public virtual bool HasBuiltInOverlay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x00229404 File Offset: 0x00227604
		public string OverlayPath
		{
			get
			{
				return SceneHelper.GetScenePath("cards/overlays/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00229425 File Offset: 0x00227625
		public Control CreateOverlay()
		{
			return PreloadManager.Cache.GetScene(this.OverlayPath).Instantiate<Control>(0L);
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06004B65 RID: 19301 RVA: 0x0022943E File Offset: 0x0022763E
		// (set) Token: 0x06004B66 RID: 19302 RVA: 0x00229446 File Offset: 0x00227646
		public int? FloorAddedToDeck
		{
			get
			{
				return this._floorAddedToDeck;
			}
			set
			{
				base.AssertMutable();
				this._floorAddedToDeck = value;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004B67 RID: 19303 RVA: 0x00229455 File Offset: 0x00227655
		// (set) Token: 0x06004B68 RID: 19304 RVA: 0x0022945D File Offset: 0x0022765D
		[Nullable(2)]
		public Creature CurrentTarget
		{
			[NullableContext(2)]
			get
			{
				return this._currentTarget;
			}
			[NullableContext(2)]
			private set
			{
				base.AssertMutable();
				this._currentTarget = value;
			}
		}

		/// <summary>
		/// Index of the play currently in progress while this card is being played (0 = first play, 1 = first Replay,
		/// etc.). Lets damage previews reflect mid-Replay state before the current play's History entry has been logged.
		/// </summary>
		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004B69 RID: 19305 RVA: 0x0022946C File Offset: 0x0022766C
		// (set) Token: 0x06004B6A RID: 19306 RVA: 0x00229474 File Offset: 0x00227674
		public int CurrentPlayIndex
		{
			get
			{
				return this._currentPlayIndex;
			}
			private set
			{
				base.AssertMutable();
				this._currentPlayIndex = value;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004B6B RID: 19307 RVA: 0x00229483 File Offset: 0x00227683
		// (set) Token: 0x06004B6C RID: 19308 RVA: 0x0022948B File Offset: 0x0022768B
		[Nullable(2)]
		public CardModel DeckVersion
		{
			[NullableContext(2)]
			get
			{
				return this._deckVersion;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._deckVersion = value;
			}
		}

		/// <summary>
		/// Set to true when this card is removed from the combat/run state (happens pretty rarely, like when a card is
		/// transformed or removed from the player's deck).
		/// Set to false when the card is added _back_ to the combat/run state (even more rarely, like when
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper" /> steals a card and then you add it back to your deck later).
		/// </summary>
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004B6D RID: 19309 RVA: 0x0022949A File Offset: 0x0022769A
		// (set) Token: 0x06004B6E RID: 19310 RVA: 0x002294A2 File Offset: 0x002276A2
		public bool HasBeenRemovedFromState { get; set; }

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004B6F RID: 19311 RVA: 0x002294AB File Offset: 0x002276AB
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004B70 RID: 19312 RVA: 0x002294B4 File Offset: 0x002276B4
		public IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
				if (this.Enchantment != null)
				{
					list.AddRange(this.Enchantment.HoverTips);
				}
				if (this.Affliction != null)
				{
					list.AddRange(this.Affliction.HoverTips);
				}
				int enchantedReplayCount = this.GetEnchantedReplayCount();
				if (enchantedReplayCount > 0)
				{
					list.Add(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, new DynamicVar[]
					{
						new DynamicVar("Times", enchantedReplayCount)
					}));
				}
				if (this.OrbEvokeType != OrbEvokeType.None)
				{
					list.Add(HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>()));
				}
				if (this.GainsBlock)
				{
					list.Add(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
				}
				foreach (CardKeyword cardKeyword in this.Keywords)
				{
					list.Add(HoverTipFactory.FromKeyword(cardKeyword));
					if (cardKeyword == CardKeyword.Ethereal)
					{
						list.Add(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
					}
				}
				return list.Distinct<IHoverTip>();
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004B71 RID: 19313 RVA: 0x002295C0 File Offset: 0x002277C0
		// (set) Token: 0x06004B72 RID: 19314 RVA: 0x002295D2 File Offset: 0x002277D2
		public CardModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		/// <summary>
		/// The state of the run that this card exists in.
		/// Null for cards that exist outside of a run (in the Compendium, in preview HoverTips, etc.)
		/// </summary>
		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004B73 RID: 19315 RVA: 0x002295E1 File Offset: 0x002277E1
		[Nullable(2)]
		public IRunState RunState
		{
			[NullableContext(2)]
			get
			{
				Player owner = this._owner;
				if (owner == null)
				{
					return null;
				}
				return owner.RunState;
			}
		}

		/// <summary>
		/// The state of the combat that this card exists in.
		/// Null for cards that exist outside of a combat (deck cards, cards offered in rewards or at events, etc.)
		/// </summary>
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x002295F4 File Offset: 0x002277F4
		[Nullable(2)]
		public ICombatState CombatState
		{
			[NullableContext(2)]
			get
			{
				CardPile pile = this.Pile;
				if ((pile == null || !pile.IsCombatPile) && this.UpgradePreviewType != CardUpgradePreviewType.Combat)
				{
					return null;
				}
				Player owner = this._owner;
				if (owner == null)
				{
					return null;
				}
				return owner.Creature.CombatState;
			}
		}

		/// <summary>
		/// The lowest-level scope that this card exists in.
		/// Combat takes precedence over run, since all cards in a run have a <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.RunState" />, but cards that have been added
		/// to combat also have a CombatState.
		/// Note: For operations that need a scope during combat, we check the owner's CombatState as a fallback because
		/// CombatState is null for cards in non-combat piles (like the deck).
		/// </summary>
		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06004B75 RID: 19317 RVA: 0x00229634 File Offset: 0x00227834
		[Nullable(2)]
		public ICardScope CardScope
		{
			[NullableContext(2)]
			get
			{
				ICardScope cardScope;
				if ((cardScope = (ICardScope)this.CombatState) == null)
				{
					Player owner = this._owner;
					cardScope = ((ICardScope)((owner != null) ? owner.Creature.CombatState : null)) ?? this.RunState;
				}
				return cardScope;
			}
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x0022966C File Offset: 0x0022786C
		public CardModel ToMutable()
		{
			base.AssertCanonical();
			return (CardModel)base.MutableClone();
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x0022968C File Offset: 0x0022788C
		protected override void DeepCloneFields()
		{
			HashSet<CardKeyword> hashSet = new HashSet<CardKeyword>();
			foreach (CardKeyword cardKeyword in this.GetKeywordsWithSources(KeywordSources.Local))
			{
				hashSet.Add(cardKeyword);
			}
			this._keywords = hashSet;
			this._dynamicVars = this.DynamicVars.Clone(this);
			CardEnergyCost energyCost = this._energyCost;
			this._energyCost = ((energyCost != null) ? energyCost.Clone(this) : null);
			this._temporaryStarCosts = this._temporaryStarCosts.ToList<TemporaryCardCost>();
			if (this.Enchantment != null)
			{
				EnchantmentModel enchantmentModel = (EnchantmentModel)this.Enchantment.ClonePreservingMutability();
				this.Enchantment = null;
				this.EnchantInternal(enchantmentModel, enchantmentModel.Amount);
			}
			if (this.Affliction != null)
			{
				AfflictionModel afflictionModel = (AfflictionModel)this.Affliction.ClonePreservingMutability();
				this.Affliction = null;
				this.AfflictInternal(afflictionModel, afflictionModel.Amount);
			}
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x0022978C File Offset: 0x0022798C
		protected override void AfterCloned()
		{
			base.AfterCloned();
			if (this._canonicalInstance == null)
			{
				this._canonicalInstance = ModelDb.GetById<CardModel>(base.Id);
			}
			this.CurrentTarget = null;
			this.CurrentPlayIndex = 0;
			this.DeckVersion = null;
			this.HasBeenRemovedFromState = false;
			this.AfflictionChanged = null;
			this.Drawn = null;
			this.EnchantmentChanged = null;
			this.EnergyCostChanged = null;
			this.Forged = null;
			this.KeywordsChanged = null;
			this.Played = null;
			this.ReplayCountChanged = null;
			this.StarCostChanged = null;
			this.Upgraded = null;
		}

		/// <summary>
		/// Extra logic that should be run after the card is created (NOT during deserialization).
		/// At this point, the card will have an owner, and will be in a <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.CombatState" /> or <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.RunState" />.
		/// </summary>
		// Token: 0x06004B79 RID: 19321 RVA: 0x0022981A File Offset: 0x00227A1A
		public virtual void AfterCreated()
		{
		}

		/// <summary>
		/// Extra logic that should be run after deserializing.
		/// This should rarely be overridden, just for very unusual cards like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.MadScience" />.
		/// </summary>
		// Token: 0x06004B7A RID: 19322 RVA: 0x0022981C File Offset: 0x00227A1C
		protected virtual void AfterDeserialized()
		{
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x0022981E File Offset: 0x00227A1E
		protected void NeverEverCallThisOutsideOfTests_ClearOwner()
		{
			if (TestMode.IsOff)
			{
				throw new InvalidOperationException("You monster!");
			}
			this._owner = null;
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x00229839 File Offset: 0x00227A39
		public void SetToFreeThisTurn()
		{
			this.EnergyCost.SetThisTurnOrUntilPlayed(0, false);
			this.SetStarCostThisTurn(0);
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0022984F File Offset: 0x00227A4F
		public void SetToFreeThisCombat()
		{
			this.EnergyCost.SetThisCombat(0, false);
			this.SetStarCostThisCombat(0);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x00229865 File Offset: 0x00227A65
		public void SetStarCostUntilPlayed(int cost)
		{
			this.AddTemporaryStarCost(TemporaryCardCost.UntilPlayed(cost));
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x00229873 File Offset: 0x00227A73
		public void SetStarCostThisTurn(int cost)
		{
			this.AddTemporaryStarCost(TemporaryCardCost.ThisTurn(cost));
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x00229881 File Offset: 0x00227A81
		public void SetStarCostThisCombat(int cost)
		{
			this.AddTemporaryStarCost(TemporaryCardCost.ThisCombat(cost));
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0022988F File Offset: 0x00227A8F
		public int GetStarCostThisCombat()
		{
			TemporaryCardCost temporaryCardCost = this._temporaryStarCosts.FirstOrDefault<TemporaryCardCost>((TemporaryCardCost cost) => cost != null && !cost.ClearsWhenTurnEnds && !cost.ClearsWhenCardIsPlayed);
			if (temporaryCardCost == null)
			{
				return this.BaseStarCost;
			}
			return temporaryCardCost.Cost;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x002298CB File Offset: 0x00227ACB
		private void AddTemporaryStarCost(TemporaryCardCost cost)
		{
			base.AssertMutable();
			this._temporaryStarCosts.Add(cost);
			Action starCostChanged = this.StarCostChanged;
			if (starCostChanged == null)
			{
				return;
			}
			starCostChanged();
		}

		/// <summary>
		/// Upgrade the star cost of this card by the specified amount.
		/// This is meant to be called in OnUpgrade.
		/// </summary>
		/// <param name="addend">Amount to add to the current cost (usually negative).</param>
		// Token: 0x06004B83 RID: 19331 RVA: 0x002298F0 File Offset: 0x00227AF0
		protected void UpgradeStarCostBy(int addend)
		{
			if (this.HasStarCostX)
			{
				throw new InvalidOperationException("UpgradeStarCostBy called on " + base.Id.Entry + " which has star cost X.");
			}
			if (addend == 0)
			{
				return;
			}
			int baseStarCost = this.BaseStarCost;
			this.BaseStarCost += addend;
			this._wasStarCostJustUpgraded = true;
			if (this.BaseStarCost < baseStarCost)
			{
				this._temporaryStarCosts.RemoveAll((TemporaryCardCost c) => c.Cost > this.BaseStarCost);
			}
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00229966 File Offset: 0x00227B66
		public void AddKeyword(CardKeyword keyword)
		{
			base.AssertMutable();
			this.LocalKeywords.Add(keyword);
			Action keywordsChanged = this.KeywordsChanged;
			if (keywordsChanged == null)
			{
				return;
			}
			keywordsChanged();
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x0022998B File Offset: 0x00227B8B
		public void RemoveKeyword(CardKeyword keyword)
		{
			base.AssertMutable();
			this.LocalKeywords.Remove(keyword);
			Action keywordsChanged = this.KeywordsChanged;
			if (keywordsChanged == null)
			{
				return;
			}
			keywordsChanged();
		}

		/// <summary>
		/// Set this card to be retained this turn.
		/// Will be cleared at the end of the turn.
		/// </summary>
		// Token: 0x06004B86 RID: 19334 RVA: 0x002299B0 File Offset: 0x00227BB0
		public void GiveSingleTurnRetain()
		{
			this.HasSingleTurnRetain = true;
		}

		/// <summary>
		/// Set this card to be Sly this turn.
		/// Will be cleared at the end of the turn.
		/// </summary>
		// Token: 0x06004B87 RID: 19335 RVA: 0x002299B9 File Offset: 0x00227BB9
		public void GiveSingleTurnSly()
		{
			this.HasSingleTurnSly = true;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x002299C2 File Offset: 0x00227BC2
		public string GetDescriptionForPile(PileType pileType, [Nullable(2)] Creature target = null)
		{
			return this.GetDescriptionForPile(pileType, CardModel.DescriptionPreviewType.None, target);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x002299CD File Offset: 0x00227BCD
		public string GetDescriptionForUpgradePreview()
		{
			return this.GetDescriptionForPile(PileType.None, CardModel.DescriptionPreviewType.Upgrade, null);
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x002299D8 File Offset: 0x00227BD8
		private unsafe string GetDescriptionForPile(PileType pileType, CardModel.DescriptionPreviewType previewType, [Nullable(2)] Creature target = null)
		{
			LocString description = this.Description;
			this.DynamicVars.AddTo(description);
			this.AddExtraArgsToDescription(description);
			UpgradeDisplay upgradeDisplay;
			if (previewType == CardModel.DescriptionPreviewType.Upgrade)
			{
				upgradeDisplay = UpgradeDisplay.UpgradePreview;
			}
			else if (this.IsUpgraded)
			{
				upgradeDisplay = UpgradeDisplay.Upgraded;
			}
			else
			{
				upgradeDisplay = UpgradeDisplay.Normal;
			}
			description.Add(new IfUpgradedVar(upgradeDisplay));
			bool flag = pileType == PileType.Hand || pileType == PileType.Play;
			bool flag2 = flag;
			description.Add("OnTable", flag2);
			bool flag3;
			if (CombatManager.Instance.IsInProgress)
			{
				CardPile pile = this.Pile;
				flag3 = ((pile != null) ? pile.IsCombatPile : pileType.IsCombatPile());
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			description.Add("InCombat", flag4);
			description.Add("IsTargeting", target != null);
			description.Add("TargetType", this.TargetType.ToString());
			description.Add("GainsBlock", this.GainsBlock);
			LocString locString = description;
			string text = "IsOstyAlive";
			bool flag5;
			if (base.IsMutable)
			{
				Player owner = this.Owner;
				flag5 = owner != null && owner.IsOstyAlive;
			}
			else
			{
				flag5 = false;
			}
			locString.Add(text, flag5);
			string prefix = EnergyIconHelper.GetPrefix(this);
			description.Add("energyPrefix", prefix);
			description.Add("singleStarIcon", "[img]res://images/packed/sprite_fonts/star_icon.png[/img]");
			foreach (KeyValuePair<string, object> keyValuePair in description.Variables)
			{
				EnergyVar energyVar = keyValuePair.Value as EnergyVar;
				if (energyVar != null)
				{
					energyVar.ColorPrefix = prefix;
				}
			}
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = description.GetFormattedText();
			List<string> list2 = list;
			EnchantmentModel enchantment = this.Enchantment;
			LocString locString2 = ((enchantment != null) ? enchantment.DynamicExtraCardText : null);
			if (locString2 != null)
			{
				list2.Add("[purple]" + locString2.GetFormattedText() + "[/purple]");
			}
			AfflictionModel affliction = this.Affliction;
			LocString locString3 = ((affliction != null) ? affliction.DynamicExtraCardText : null);
			if (locString3 != null)
			{
				list2.Add("[purple]" + locString3.GetFormattedText() + "[/purple]");
			}
			IReadOnlySet<CardKeyword> keywords = this.Keywords;
			foreach (CardKeyword cardKeyword in CardKeywordOrder.beforeDescription)
			{
				if (cardKeyword != CardKeyword.Retain)
				{
					if (cardKeyword == CardKeyword.Sly)
					{
						flag = this.IsSlyThisTurn;
					}
					else
					{
						flag = keywords.Contains(cardKeyword);
					}
				}
				else
				{
					flag = this.ShouldRetainThisTurn;
				}
				bool flag6 = flag;
				if (flag6)
				{
					list2.Insert(0, cardKeyword.GetCardText());
				}
			}
			int enchantedReplayCount = this.GetEnchantedReplayCount();
			if (enchantedReplayCount > 0)
			{
				LocString locString4 = new LocString("static_hover_tips", "REPLAY.extraText");
				locString4.Add("Times", enchantedReplayCount);
				list2.Add(locString4.GetFormattedText() ?? "");
			}
			foreach (CardKeyword cardKeyword2 in CardKeywordOrder.afterDescription.Intersect<CardKeyword>(keywords))
			{
				list2.Add(cardKeyword2.GetCardText());
			}
			return string.Join<string>('\n', list2.Where<string>((string l) => !string.IsNullOrEmpty(l)));
		}

		/// <summary>
		/// Updates the dynamic variables of this card based on hooks (i.e damage, block, and powers).
		/// This is so powers and relic modifications to these values can be reflected in card descriptions.
		/// </summary>
		/// <param name="previewMode">
		/// The type of preview to show in the card's visuals. See <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.CardPreviewMode" /> for details.
		/// </param>
		/// <param name="target">Creature who this card is targeting, if it exists.</param>
		/// <param name="dynamicVarSet">The dynamic variables for this card.</param>
		// Token: 0x06004B8B RID: 19339 RVA: 0x00229D24 File Offset: 0x00227F24
		public void UpdateDynamicVarPreview(CardPreviewMode previewMode, [Nullable(2)] Creature target, DynamicVarSet dynamicVarSet)
		{
			if (this.RunState == null && this.CombatState == null)
			{
				return;
			}
			bool flag = this.CombatState != null;
			bool flag2 = flag;
			if (flag2)
			{
				CardPile pile = this.Pile;
				PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : null);
				bool flag3;
				if (pileType != null)
				{
					PileType valueOrDefault = pileType.GetValueOrDefault();
					if (valueOrDefault == PileType.Hand || valueOrDefault == PileType.Play)
					{
						flag3 = true;
						goto IL_0066;
					}
				}
				flag3 = false;
				IL_0066:
				flag2 = flag3 || this.UpgradePreviewType == CardUpgradePreviewType.Combat;
			}
			bool flag4 = flag2;
			IEnumerable<DynamicVar> enumerable = dynamicVarSet.Values.ToList<DynamicVar>();
			foreach (DynamicVar dynamicVar in enumerable)
			{
				dynamicVar.UpdateCardPreview(this, previewMode, target, flag4);
			}
		}

		/// <summary>
		/// Add an Enchantment to this card.
		/// </summary>
		/// <remarks>
		/// You should generally use CardCommands.Enchant instead of this method. If you do use this method, you may need to
		/// call <see cref="M:MegaCrit.Sts2.Core.Models.EnchantmentModel.ModifyCard" /> after.
		/// </remarks>
		/// <param name="enchantment">Enchantment to add.</param>
		/// <param name="amount">Amount to set on the added enchantment.</param>
		// Token: 0x06004B8C RID: 19340 RVA: 0x00229DFC File Offset: 0x00227FFC
		public void EnchantInternal(EnchantmentModel enchantment, decimal amount)
		{
			base.AssertMutable();
			enchantment.AssertMutable();
			this.Enchantment = enchantment;
			this.Enchantment.ApplyInternal(this, amount);
			Action enchantmentChanged = this.EnchantmentChanged;
			if (enchantmentChanged == null)
			{
				return;
			}
			enchantmentChanged();
		}

		/// <summary>
		/// Add an Affliction to this card.
		/// </summary>
		/// <remarks>You should generally use CardCmd.Afflict instead of this method.</remarks>
		/// <param name="affliction">Affliction to add.</param>
		/// <param name="amount">Amount to set on the added affliction</param>
		/// <returns>Whether or not adding the affliction was successful.</returns>
		// Token: 0x06004B8D RID: 19341 RVA: 0x00229E30 File Offset: 0x00228030
		public void AfflictInternal(AfflictionModel affliction, decimal amount)
		{
			base.AssertMutable();
			affliction.AssertMutable();
			if (this.Affliction != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to afflict card ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" that was already afflicted! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Affliction = affliction;
			this.Affliction.Card = this;
			this.Affliction.Amount = (int)amount;
			Action afflictionChanged = this.AfflictionChanged;
			if (afflictionChanged == null)
			{
				return;
			}
			afflictionChanged();
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00229EBC File Offset: 0x002280BC
		public void ClearEnchantmentInternal()
		{
			if (this.Enchantment == null)
			{
				return;
			}
			base.AssertMutable();
			this.Enchantment.ClearInternal();
			this.Enchantment = null;
			Action enchantmentChanged = this.EnchantmentChanged;
			if (enchantmentChanged == null)
			{
				return;
			}
			enchantmentChanged();
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x00229EF0 File Offset: 0x002280F0
		public void ClearAfflictionInternal()
		{
			base.AssertMutable();
			if (this.Affliction == null)
			{
				return;
			}
			this.Affliction.ClearInternal();
			this.Affliction = null;
			this.Owner.PlayerCombatState.RecalculateCardValues();
			Action afflictionChanged = this.AfflictionChanged;
			if (afflictionChanged == null)
			{
				return;
			}
			afflictionChanged();
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x00229F3E File Offset: 0x0022813E
		protected virtual void AddExtraArgsToDescription(LocString description)
		{
		}

		/// <summary>
		/// Get this card's current star cost, including all modifiers.
		/// Usually, this will just be the same as CurrentStarCost, but there are 2 exceptions:
		/// 1. X-cost cards will return the amount of stars that will be spent to play them instead of a 0 placeholder value.
		/// 2. Effects that modify star costs will be reflected here.
		/// </summary>
		/// <returns>Current energy cost including modifiers.</returns>
		// Token: 0x06004B91 RID: 19345 RVA: 0x00229F40 File Offset: 0x00228140
		public int GetStarCostWithModifiers()
		{
			if (this.HasStarCostX)
			{
				PlayerCombatState playerCombatState = this.Owner.PlayerCombatState;
				if (playerCombatState == null)
				{
					return 0;
				}
				return playerCombatState.Stars;
			}
			else
			{
				CardPile pile = this.Pile;
				if (pile != null && pile.IsCombatPile && this.CombatState != null)
				{
					return (int)Hook.ModifyStarCost(this.CombatState, this, this.CurrentStarCost);
				}
				return this.CurrentStarCost;
			}
		}

		/// <summary>
		/// Does this card have an energy or star (or both) cost?
		/// Used by effects that make cards totally free (no energy OR star cost), to avoid selecting cards that are already
		/// free.
		/// </summary>
		/// <param name="includeGlobalModifiers">
		/// Whether to include global modifiers in the cost.
		/// See <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers.Global" /> for details.
		/// </param>
		// Token: 0x06004B92 RID: 19346 RVA: 0x00229FAC File Offset: 0x002281AC
		public bool CostsEnergyOrStars(bool includeGlobalModifiers)
		{
			if (includeGlobalModifiers)
			{
				if (!this.EnergyCost.CostsX && this.EnergyCost.GetWithModifiers(CostModifiers.All) > 0)
				{
					return true;
				}
				if (!this.HasStarCostX && this.GetStarCostWithModifiers() > 0)
				{
					return true;
				}
			}
			else if (this.EnergyCost.GetWithModifiers(CostModifiers.Local) > 0 || this.CurrentStarCost > 0)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x0022A008 File Offset: 0x00228208
		public void RemoveFromCurrentPile(bool silent = false)
		{
			base.AssertMutable();
			CardPile pile = this.Pile;
			if (pile == null)
			{
				return;
			}
			pile.RemoveInternal(this, silent);
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x0022A022 File Offset: 0x00228222
		public void RemoveFromState()
		{
			this.RemoveFromCurrentPile(false);
			this.HasBeenRemovedFromState = true;
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x0022A034 File Offset: 0x00228234
		public void EndOfTurnCleanup()
		{
			this.ExhaustOnNextPlay = false;
			this.HasSingleTurnRetain = false;
			this.HasSingleTurnSly = false;
			if (this.EnergyCost.EndOfTurnCleanup())
			{
				Action energyCostChanged = this.EnergyCostChanged;
				if (energyCostChanged != null)
				{
					energyCostChanged();
				}
			}
			if (this._temporaryStarCosts.RemoveAll((TemporaryCardCost c) => c.ClearsWhenTurnEnds) > 0)
			{
				Action starCostChanged = this.StarCostChanged;
				if (starCostChanged == null)
				{
					return;
				}
				starCostChanged();
			}
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x0022A0B1 File Offset: 0x002282B1
		public virtual void AfterTransformedFrom()
		{
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x0022A0B3 File Offset: 0x002282B3
		public virtual void AfterTransformedTo()
		{
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x0022A0B5 File Offset: 0x002282B5
		public void AfterForged()
		{
			Action forged = this.Forged;
			if (forged == null)
			{
				return;
			}
			forged();
		}

		/// <summary>
		/// Override this method for when this card is played.
		/// </summary>
		/// <param name="choiceContext"></param>
		/// <param name="cardPlay">The CardPlay that is being executed.</param>
		// Token: 0x06004B99 RID: 19353 RVA: 0x0022A0C7 File Offset: 0x002282C7
		protected virtual Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Override this method to add VFX that should run the moment the mouse is released when playing this card.
		/// This is different from OnPlay, which is meant for game logic (dealing damage, gaining block, etc.), and won't
		/// run until the card play action is dequeued from the action queue.
		/// WARNING: Don't put any game logic in here! It might do things you don't expect.
		/// </summary>
		/// <param name="target">Creature that this card is targeting. Should be null for un-targeted cards.</param>
		// Token: 0x06004B9A RID: 19354 RVA: 0x0022A0CE File Offset: 0x002282CE
		public virtual Task OnEnqueuePlayVfx([Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Override this method with logic for when this card is upgraded.
		/// To upgrade a card's dynamic vars, use <see cref="M:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar.UpgradeValueBy(System.Decimal)" />.
		/// To upgrade a card's energy cost, use <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.UpgradeBy(System.Int32)" />.
		/// </summary>
		// Token: 0x06004B9B RID: 19355 RVA: 0x0022A0D5 File Offset: 0x002282D5
		protected virtual void OnUpgrade()
		{
		}

		/// <summary>
		/// Override this property to run this card's <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnTurnEndInHand(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext)" /> when the player ends the turn with this
		/// card in their hand.
		/// </summary>
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x0022A0D7 File Offset: 0x002282D7
		public virtual bool HasTurnEndInHandEffect
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Override this method for when the player ends the turn with this card in their hand.
		/// NOTES:
		/// * You must also override <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.HasTurnEndInHandEffect" /> to return true.
		/// * While this method is being run, this card will be in the Play pile.
		/// * After this method is run, this card will be added to the Discard pile.
		/// </summary>
		/// <param name="choiceContext">The choice context to use in the event of a player choice.</param>
		// Token: 0x06004B9D RID: 19357 RVA: 0x0022A0DA File Offset: 0x002282DA
		protected virtual Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x0022A0E4 File Offset: 0x002282E4
		public async Task OnTurnEndInHandWrapper(PlayerChoiceContext choiceContext)
		{
			await CardPileCmd.Add(this, PileType.Play, CardPilePosition.Bottom, null, false);
			if (LocalContext.IsMe(this.Owner))
			{
				await Cmd.CustomScaledWait(0.3f, 0.6f, false, default(CancellationToken));
			}
			await this.OnTurnEndInHand(choiceContext);
			if (this.Keywords.Contains(CardKeyword.Ethereal))
			{
				await CardCmd.Exhaust(choiceContext, this, true, false);
			}
			else
			{
				await CardPileCmd.Add(this, PileType.Discard.GetPile(this.Owner), CardPilePosition.Bottom, null, false);
			}
		}

		/// <summary>
		/// Can this card be played with the specified creature as the target?
		/// </summary>
		/// <param name="target">Creature for this card to target. Null when attempting to play with no target (like Defend).</param>
		/// <returns>Whether the card can be played with the specified target.</returns>
		// Token: 0x06004B9F RID: 19359 RVA: 0x0022A12F File Offset: 0x0022832F
		[NullableContext(2)]
		public bool CanPlayTargeting(Creature target)
		{
			return this.IsValidTarget(target) && this.CanPlay();
		}

		/// <summary>
		/// Can this card be played?
		/// </summary>
		// Token: 0x06004BA0 RID: 19360 RVA: 0x0022A144 File Offset: 0x00228344
		public bool CanPlay()
		{
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			return this.CanPlay(out unplayableReason, out abstractModel);
		}

		/// <summary>
		/// Can this card be played?
		/// </summary>
		/// <param name="reason">
		/// Out param containing the reason that this card cannot be played (None if it can be played).
		/// </param>
		/// <param name="preventer">
		/// First model that made this card unable to be played (null if there is no preventer).
		/// </param>
		/// <returns>Whether this card can be played.</returns>
		// Token: 0x06004BA1 RID: 19361 RVA: 0x0022A15C File Offset: 0x0022835C
		[NullableContext(2)]
		public bool CanPlay(out UnplayableReason reason, out AbstractModel preventer)
		{
			reason = UnplayableReason.None;
			ICombatState combatState;
			if ((combatState = this.CombatState) == null)
			{
				Player owner = this._owner;
				combatState = ((owner != null) ? owner.Creature.CombatState : null);
			}
			ICombatState combatState2 = combatState;
			if (combatState2 == null || this.Owner.PlayerCombatState == null)
			{
				preventer = null;
				return false;
			}
			if (this.Keywords.Contains(CardKeyword.Unplayable))
			{
				reason |= UnplayableReason.HasUnplayableKeyword;
			}
			UnplayableReason unplayableReason;
			if (!this.Owner.PlayerCombatState.HasEnoughResourcesFor(this, out unplayableReason))
			{
				reason |= unplayableReason;
			}
			if (this.TargetType == TargetType.AnyAlly)
			{
				if (combatState2.PlayerCreatures.Count<Creature>((Creature c) => c.IsAlive) <= 1)
				{
					reason |= UnplayableReason.NoLivingAllies;
				}
			}
			if (!Hook.ShouldPlay(combatState2, this, out preventer, AutoPlayType.None))
			{
				reason |= UnplayableReason.BlockedByHook;
			}
			if (!this.IsPlayable)
			{
				reason |= UnplayableReason.BlockedByCardLogic;
			}
			return reason == UnplayableReason.None;
		}

		/// <summary>
		/// Returns true if target is valid for this card.
		/// NOTE: This operates differently than potions! Do not try to unify this with PotionModel.IsValidTarget unless you
		/// change UI targeting; namely, PotionModel's TargetType.Self passes a target, whereas this one doesn't.
		/// </summary>
		// Token: 0x06004BA2 RID: 19362 RVA: 0x0022A234 File Offset: 0x00228434
		[NullableContext(2)]
		public bool IsValidTarget(Creature target)
		{
			if (target == null)
			{
				return this.TargetType != TargetType.AnyEnemy && this.TargetType != TargetType.AnyAlly;
			}
			if (!target.IsAlive)
			{
				return false;
			}
			if (this.TargetType == TargetType.AnyEnemy)
			{
				return target.Side != this.Owner.Creature.Side;
			}
			return this.TargetType == TargetType.AnyAlly && target.Side == this.Owner.Creature.Side;
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x0022A2AE File Offset: 0x002284AE
		[NullableContext(2)]
		public bool TryManualPlay(Creature target)
		{
			if (this.CanPlayTargeting(target))
			{
				this.EnqueueManualPlay(target);
				return true;
			}
			return false;
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x0022A2C3 File Offset: 0x002284C3
		[NullableContext(2)]
		private void EnqueueManualPlay(Creature target)
		{
			TaskHelper.RunSafely(this.OnEnqueuePlayVfx(target));
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new PlayCardAction(this, target));
		}

		/// <summary>
		/// Spend the resources required to play this card.
		/// </summary>
		/// <returns>The energy and stars spent.</returns>
		// Token: 0x06004BA5 RID: 19365 RVA: 0x0022A2E8 File Offset: 0x002284E8
		[return: Nullable(new byte[] { 1, 0 })]
		public async Task<ValueTuple<int, int>> SpendResources()
		{
			int energy = this.Owner.PlayerCombatState.Energy;
			int energyToSpend = this.EnergyCost.GetAmountToSpend();
			int starsToSpend = Math.Max(0, this.GetStarCostWithModifiers());
			if (energyToSpend > energy && Hook.ShouldPayExcessEnergyCostWithStars(this.CombatState, this.Owner))
			{
				starsToSpend += (energyToSpend - energy) * 2;
				energyToSpend = energy;
			}
			await this.SpendEnergy(energyToSpend);
			await this.SpendStars(starsToSpend);
			return new ValueTuple<int, int>(energyToSpend, starsToSpend);
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x0022A32C File Offset: 0x0022852C
		private async Task SpendEnergy(int amount)
		{
			if (this.EnergyCost.CostsX)
			{
				this.EnergyCost.CapturedXValue = amount;
			}
			if (amount > 0)
			{
				CombatManager.Instance.History.EnergySpent(this.CombatState, amount, this.Owner);
				this.Owner.PlayerCombatState.LoseEnergy(Math.Max(0, amount));
			}
			await Hook.AfterEnergySpent(this.CombatState, this, amount);
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x0022A378 File Offset: 0x00228578
		private async Task SpendStars(int amount)
		{
			this.LastStarsSpent = amount;
			if (amount > 0)
			{
				this.Owner.PlayerCombatState.LoseStars(amount);
				await Hook.AfterStarsSpent(this.Owner.Creature.CombatState, amount, this.Owner);
			}
		}

		/// <summary>
		/// Run all the logic for playing this card.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">The creature that this card is targeting. Null for un-targeted cards.</param>
		/// <param name="isAutoPlay">
		/// Whether this card is being auto-played.
		/// False when the player plays the card manually from their hand.
		/// True when played automatically by an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MayhemPower" />.
		/// </param>
		/// <param name="resources">Info about the resources used when playing this card.</param>
		/// <param name="skipCardPileVisuals">Skip card pile visuals (tween to/from pile, smoke puff VFX, etc).</param>
		// Token: 0x06004BA8 RID: 19368 RVA: 0x0022A3C4 File Offset: 0x002285C4
		public Task OnPlayWrapper(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
		{
			CardModel.<OnPlayWrapper>d__339 <OnPlayWrapper>d__;
			<OnPlayWrapper>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnPlayWrapper>d__.<>4__this = this;
			<OnPlayWrapper>d__.choiceContext = choiceContext;
			<OnPlayWrapper>d__.target = target;
			<OnPlayWrapper>d__.isAutoPlay = isAutoPlay;
			<OnPlayWrapper>d__.resources = resources;
			<OnPlayWrapper>d__.skipCardPileVisuals = skipCardPileVisuals;
			<OnPlayWrapper>d__.<>1__state = -1;
			<OnPlayWrapper>d__.<>t__builder.Start<CardModel.<OnPlayWrapper>d__339>(ref <OnPlayWrapper>d__);
			return <OnPlayWrapper>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Generate the number of times this card's OnPlay effects should be executed when it's played.
		/// Runs hooks to modify the play count, including state-modifying hooks like
		/// <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterModifyingCardPlayCount(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Models.CardModel,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Models.AbstractModel})" />. This means you can only run this method in situations where it's
		/// okay to modify combat state (i.e. not as part of previews).
		/// </summary>
		/// <param name="combatState">The combat state that this card is being played in.</param>
		/// <param name="target">The creature that this card is targeting. Null for un-targeted cards.</param>
		// Token: 0x06004BA9 RID: 19369 RVA: 0x0022A434 File Offset: 0x00228634
		protected async Task<int> GeneratePlayCount(ICombatState combatState, [Nullable(2)] Creature target)
		{
			int playCount = this.GetEnchantedReplayCount() + 1;
			List<AbstractModel> list;
			playCount = Hook.ModifyCardPlayCount(combatState, this, playCount, target, out list);
			await Hook.AfterModifyingCardPlayCount(combatState, this, list);
			return playCount;
		}

		/// <summary>
		/// Plays the VFX which makes the card swirl and fly into the player.
		/// Should only be done for power cards.
		/// </summary>
		// Token: 0x06004BAA RID: 19370 RVA: 0x0022A488 File Offset: 0x00228688
		private async Task PlayPowerCardFlyVfx()
		{
			NCard node = NCard.FindOnTable(this, null);
			bool flag = false;
			if (node != null)
			{
				foreach (NCardFlyPowerVfx ncardFlyPowerVfx in NCombatRoom.Instance.CombatVfxContainer.GetChildren(false).OfType<NCardFlyPowerVfx>())
				{
					if (ncardFlyPowerVfx.CardNode == node)
					{
						flag = true;
						break;
					}
				}
			}
			if (node == null || flag)
			{
				node = NCard.Create(this, ModelVisibility.Visible);
				if (node != null)
				{
					Tween tween = node.CreateTween();
					tween.Parallel().TweenProperty(node, "scale", Vector2.One * 1f, 0.10000000149011612).From(Vector2.Zero)
						.SetEase(1L)
						.SetTrans(7L);
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.CombatVfxContainer.AddChildSafely(node);
					}
					node.GlobalPosition = PileType.Play.GetTargetPosition(node);
					node.UpdateVisuals(PileType.Play, CardPreviewMode.Normal);
				}
				await Cmd.CustomScaledWait(0.1f, 0.8f, false, default(CancellationToken));
			}
			if (node != null)
			{
				NCardFlyPowerVfx ncardFlyPowerVfx2 = NCardFlyPowerVfx.Create(node);
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 != null)
				{
					instance2.CombatVfxContainer.AddChildSafely(ncardFlyPowerVfx2);
				}
				TaskHelper.RunSafely(ncardFlyPowerVfx2.PlayAnim());
				float duration = ncardFlyPowerVfx2.GetDuration();
				await Cmd.CustomScaledWait(duration * 0.2f, duration, false, default(CancellationToken));
			}
		}

		/// <summary>
		/// Get the pile that this card should be moved to after being played, and the position it should be added to.
		/// </summary>
		// Token: 0x06004BAB RID: 19371 RVA: 0x0022A4CC File Offset: 0x002286CC
		protected virtual CardLocation GetResultLocationForCardPlay()
		{
			if (this.IsDupe || this.Type == CardType.Power)
			{
				return new CardLocation(this.Owner, PileType.None, CardPilePosition.Bottom);
			}
			if (this.ExhaustOnNextPlay || this.Keywords.Contains(CardKeyword.Exhaust))
			{
				this.ExhaustOnNextPlay = false;
				return new CardLocation(this.Owner, PileType.Exhaust, CardPilePosition.Bottom);
			}
			return new CardLocation(this.Owner, PileType.Discard, CardPilePosition.Bottom);
		}

		/// <summary>
		/// Send the card to the correct pile after it was attempted to be played while unplayable.
		/// This is the same as MoveCardToResultPileAfterPlay with one important exception: Power cards do not get sent
		/// to Limbo, and instead get sent to the discard.
		/// </summary>
		// Token: 0x06004BAC RID: 19372 RVA: 0x0022A530 File Offset: 0x00228730
		public async Task MoveToResultPileWithoutPlaying(PlayerChoiceContext choiceContext)
		{
			CardPile pile = this.Pile;
			if (pile != null && pile.Type == PileType.Play)
			{
				if (this.IsDupe)
				{
					await CardPileCmd.RemoveFromCombat(this, false);
				}
				else if (this.ExhaustOnNextPlay || this.Keywords.Contains(CardKeyword.Exhaust))
				{
					await CardCmd.Exhaust(choiceContext, this, false, false);
				}
				else
				{
					await CardPileCmd.Add(this, PileType.Discard, CardPilePosition.Bottom, null, false);
				}
			}
		}

		/// <summary>
		/// WARNING: If you're thinking of calling this from inside a model, you probably want
		/// <see cref="M:MegaCrit.Sts2.Core.Commands.CardCmd.Upgrade(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Nodes.CommonUi.CardPreviewStyle)" /> instead.
		///
		/// Upgrade this card. This does not run any hooks.
		/// </summary>
		// Token: 0x06004BAD RID: 19373 RVA: 0x0022A57C File Offset: 0x0022877C
		public void UpgradeInternal()
		{
			base.AssertMutable();
			int currentUpgradeLevel = this.CurrentUpgradeLevel;
			this.CurrentUpgradeLevel = currentUpgradeLevel + 1;
			this.OnUpgrade();
			this.DynamicVars.RecalculateForUpgradeOrEnchant();
			Action upgraded = this.Upgraded;
			if (upgraded == null)
			{
				return;
			}
			upgraded();
		}

		/// <summary>
		/// Finalize an upgrade after calling UpgradeInternal. This clears out state that is used for displaying an upgrade
		/// preview.
		/// </summary>
		// Token: 0x06004BAE RID: 19374 RVA: 0x0022A5C0 File Offset: 0x002287C0
		public void FinalizeUpgradeInternal()
		{
			this.DynamicVars.FinalizeUpgrade();
			this.EnergyCost.FinalizeUpgrade();
			this._wasStarCostJustUpgraded = false;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0022A5E0 File Offset: 0x002287E0
		public void DowngradeInternal()
		{
			base.AssertMutable();
			this.CurrentUpgradeLevel = 0;
			CardModel cardModel = ModelDb.GetById<CardModel>(base.Id).ToMutable();
			this._dynamicVars = cardModel.DynamicVars.Clone(this);
			this.EnergyCost.ResetForDowngrade();
			this._baseStarCost = cardModel.CanonicalStarCost;
			this._keywords = cardModel.GetKeywordsWithSources(KeywordSources.Local).ToHashSet<CardKeyword>();
			this.AfterDowngraded();
			EnchantmentModel enchantment = this.Enchantment;
			if (enchantment != null)
			{
				enchantment.ModifyCard();
			}
			AfflictionModel affliction = this.Affliction;
			if (affliction != null)
			{
				affliction.AfterApplied();
			}
			Action upgraded = this.Upgraded;
			if (upgraded == null)
			{
				return;
			}
			upgraded();
		}

		/// <summary>
		/// Gives cards a chance to add extra "cleanup" logic for downgrades.
		/// No-op by default.
		/// </summary>
		// Token: 0x06004BB0 RID: 19376 RVA: 0x0022A67E File Offset: 0x0022887E
		protected virtual void AfterDowngraded()
		{
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x0022A680 File Offset: 0x00228880
		public void InvokeDrawn()
		{
			Action drawn = this.Drawn;
			if (drawn == null)
			{
				return;
			}
			drawn();
		}

		/// <summary>
		/// Create a clone of this card.
		/// Clones must only be created in combat; for run-level clones, see <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CloneCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// See <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.CloneOf" /> for more info on clones, and the difference between a clone and a dupe.
		/// </summary>
		// Token: 0x06004BB2 RID: 19378 RVA: 0x0022A694 File Offset: 0x00228894
		public CardModel CreateClone()
		{
			if (this.Pile != null && !this.Pile.Type.IsCombatPile())
			{
				throw new InvalidOperationException("Cannot create a clone of a card that is not in a combat pile.");
			}
			base.AssertMutable();
			CardModel cardModel = this.CardScope.CloneCard(this);
			cardModel._cloneOf = this;
			cardModel.ExhaustOnNextPlay = false;
			return cardModel;
		}

		/// <summary>
		/// Creates a clone of this card for a given player.
		/// Required because normally clone cards retain the same player as the original.
		/// </summary>
		// Token: 0x06004BB3 RID: 19379 RVA: 0x0022A6E8 File Offset: 0x002288E8
		public CardModel CreateCloneForPlayer(Player player)
		{
			CardModel cardModel = this.CreateClone();
			cardModel._owner = player;
			return cardModel;
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0022A704 File Offset: 0x00228904
		public void GiveToAnotherPlayer(Player player)
		{
			this._owner = player;
		}

		/// <summary>
		/// Create a dupe of this card.
		/// See <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.DupeOf" /> for more info on dupes, and the difference between a clone and a dupe.
		/// </summary>
		// Token: 0x06004BB5 RID: 19381 RVA: 0x0022A710 File Offset: 0x00228910
		public CardModel CreateDupe(Player newOwner)
		{
			if (this.IsDupe)
			{
				return this.DupeOf.CreateDupe(newOwner);
			}
			base.AssertMutable();
			CardModel cardModel = this.CreateCloneForPlayer(newOwner);
			cardModel.IsDupe = true;
			cardModel.RemoveKeyword(CardKeyword.Exhaust);
			return cardModel;
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004BB6 RID: 19382 RVA: 0x0022A750 File Offset: 0x00228950
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				CardPile pile = this.Pile;
				return pile != null && pile.IsCombatPile;
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0022A770 File Offset: 0x00228970
		public SerializableCard ToSerializable()
		{
			base.AssertMutable();
			SerializableCard serializableCard = new SerializableCard();
			serializableCard.Id = base.Id;
			serializableCard.CurrentUpgradeLevel = this.CurrentUpgradeLevel;
			serializableCard.Props = SavedProperties.From(this);
			EnchantmentModel enchantment = this.Enchantment;
			serializableCard.Enchantment = ((enchantment != null) ? enchantment.ToSerializable() : null);
			serializableCard.FloorAddedToDeck = this.FloorAddedToDeck;
			return serializableCard;
		}

		/// <summary>
		/// Create a CardModel from a SerializableCard.
		/// Be careful calling this! Make sure all callers eventually use <see cref="T:MegaCrit.Sts2.Core.Runs.ICardScope" /> to add the card to the
		/// correct scope.
		/// </summary>
		// Token: 0x06004BB8 RID: 19384 RVA: 0x0022A7D0 File Offset: 0x002289D0
		public static CardModel FromSerializable(SerializableCard save)
		{
			CardModel cardModel = SaveUtil.CardOrDeprecated(save.Id).ToMutable();
			SavedProperties props = save.Props;
			if (props != null)
			{
				props.Fill(cardModel);
			}
			if (save.FloorAddedToDeck != null)
			{
				cardModel.FloorAddedToDeck = save.FloorAddedToDeck;
			}
			cardModel.AfterDeserialized();
			if (!(cardModel is DeprecatedCard))
			{
				if (save.Enchantment != null)
				{
					cardModel.EnchantInternal(EnchantmentModel.FromSerializable(save.Enchantment), save.Enchantment.Amount);
					cardModel.Enchantment.ModifyCard();
					cardModel.FinalizeUpgradeInternal();
				}
				for (int i = 0; i < save.CurrentUpgradeLevel; i++)
				{
					cardModel.UpgradeInternal();
					cardModel.FinalizeUpgradeInternal();
				}
			}
			return cardModel;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0022A884 File Offset: 0x00228A84
		[NullableContext(2)]
		public override int CompareTo(AbstractModel other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			int num = base.CompareTo(other);
			if (num != 0)
			{
				return num;
			}
			CardModel cardModel = (CardModel)other;
			int num2 = this.CurrentUpgradeLevel.CompareTo(cardModel.CurrentUpgradeLevel);
			if (num2 != 0)
			{
				return num2;
			}
			return 0;
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06004BBA RID: 19386 RVA: 0x0022A8CB File Offset: 0x00228ACB
		public virtual IEnumerable<string> AllPortraitPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(this.PortraitPath);
			}
		}

		/// <summary>
		/// Returns all card-related asset paths for a run.
		/// </summary>
		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004BBB RID: 19387 RVA: 0x0022A8D8 File Offset: 0x00228AD8
		public IEnumerable<string> RunAssetPaths
		{
			get
			{
				return this.ExtraRunAssetPaths;
			}
		}

		/// <summary>
		/// Cards can define VFX that are displayed in combat here.
		/// These are not preloaded when only the card would be displayed, e.g. in the compendium.
		/// </summary>
		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004BBC RID: 19388 RVA: 0x0022A8E0 File Offset: 0x00228AE0
		protected virtual IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x04001CFD RID: 7421
		[Nullable(2)]
		private LocString _titleLocString;

		// Token: 0x04001D00 RID: 7424
		[Nullable(2)]
		private CardPoolModel _pool;

		// Token: 0x04001D01 RID: 7425
		[Nullable(2)]
		private Player _owner;

		// Token: 0x04001D03 RID: 7427
		[Nullable(2)]
		private CardEnergyCost _energyCost;

		// Token: 0x04001D04 RID: 7428
		private int _baseReplayCount;

		// Token: 0x04001D05 RID: 7429
		private bool _starCostSet;

		// Token: 0x04001D06 RID: 7430
		private int _baseStarCost;

		/// <summary>
		/// Was this card's star cost just recently upgraded?
		/// This is mainly used to show upgrade preview values in green.
		/// This should be cleared after the upgrade is complete.
		/// </summary>
		// Token: 0x04001D07 RID: 7431
		private bool _wasStarCostJustUpgraded;

		// Token: 0x04001D08 RID: 7432
		private List<TemporaryCardCost> _temporaryStarCosts = new List<TemporaryCardCost>();

		// Token: 0x04001D09 RID: 7433
		private int _lastStarsSpent;

		// Token: 0x04001D0B RID: 7435
		[Nullable(2)]
		private HashSet<CardKeyword> _keywords;

		// Token: 0x04001D0C RID: 7436
		[Nullable(2)]
		private HashSet<CardTag> _tags;

		// Token: 0x04001D0D RID: 7437
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D0E RID: 7438
		private bool _exhaustOnNextPlay;

		// Token: 0x04001D0F RID: 7439
		private bool _hasSingleTurnRetain;

		// Token: 0x04001D10 RID: 7440
		private bool _hasSingleTurnSly;

		/// <summary>
		/// The card that this is a clone of. Null when this card is not a clone (which is most of the time).
		/// Clones are exactly the same as any other type of card, we just keep track of the original to make some effects
		/// easier to implement (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Terraforming" />).
		/// </summary>
		// Token: 0x04001D13 RID: 7443
		[Nullable(2)]
		private CardModel _cloneOf;

		/// <summary>
		/// Whether or not this card is considered a duplicate. See <see cref="P:MegaCrit.Sts2.Core.Models.CardModel.DupeOf" /> for more info on dupes.
		/// </summary>
		// Token: 0x04001D14 RID: 7444
		private bool _isDupe;

		// Token: 0x04001D15 RID: 7445
		private int _currentUpgradeLevel;

		// Token: 0x04001D16 RID: 7446
		private CardUpgradePreviewType _upgradePreviewType;

		// Token: 0x04001D18 RID: 7448
		private bool _isEnchantmentPreview;

		// Token: 0x04001D19 RID: 7449
		private int? _floorAddedToDeck;

		// Token: 0x04001D1A RID: 7450
		[Nullable(2)]
		private Creature _currentTarget;

		// Token: 0x04001D1B RID: 7451
		private int _currentPlayIndex;

		// Token: 0x04001D1C RID: 7452
		[Nullable(2)]
		private CardModel _deckVersion;

		// Token: 0x04001D1E RID: 7454
		[Nullable(2)]
		private CardModel _canonicalInstance;

		// Token: 0x02001A09 RID: 6665
		[NullableContext(0)]
		private enum DescriptionPreviewType
		{
			// Token: 0x04006692 RID: 26258
			None,
			// Token: 0x04006693 RID: 26259
			Upgrade
		}
	}
}
