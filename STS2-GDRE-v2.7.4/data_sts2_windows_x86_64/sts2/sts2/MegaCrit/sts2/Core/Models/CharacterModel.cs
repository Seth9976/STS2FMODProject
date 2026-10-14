using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004DE RID: 1246
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CharacterModel : AbstractModel
	{
		/// <summary>
		/// Is this a playable character?
		/// True for the characters that can be played in the game.
		/// False for test characters (<see cref="T:MegaCrit.Sts2.Core.Models.Characters.Deprived" />), meta-characters (<see cref="T:MegaCrit.Sts2.Core.Models.Characters.RandomCharacter" />), etc.
		/// </summary>
		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x0022AAAC File Offset: 0x00228CAC
		public virtual bool IsPlayable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// The name of the character. e.g. Ironclad, Silent, etc.
		/// </summary>
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x0022AAAF File Offset: 0x00228CAF
		public LocString Title
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".title");
			}
		}

		/// <summary>
		/// The name of the character in object form.
		/// This is the same as Title in english, but may be different in other languages.
		/// For example,
		/// </summary>
		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06004BD5 RID: 19413 RVA: 0x0022AAD0 File Offset: 0x00228CD0
		public LocString TitleObject
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".titleObject");
			}
		}

		/// <summary>
		/// Color used when rendering this character's name in the Statistics screen.
		/// </summary>
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06004BD6 RID: 19414
		public abstract Color NameColor { get; }

		/// <summary>
		/// The gender of the character for grammatical purposes.
		/// </summary>
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004BD7 RID: 19415
		public abstract CharacterGender Gender { get; }

		/// <summary>
		/// What character do you need to do a run with to unlock this one?
		/// Returns null if there is no pre-requisite character.
		/// </summary>
		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004BD8 RID: 19416
		[Nullable(2)]
		protected abstract CharacterModel UnlocksAfterRunAs
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Quote to display in the Bestiary Stats screen when a character has encountered a monster but has never killed it.
		/// The quote is unique per character and is the same quote for all monsters.
		/// </summary>
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004BD9 RID: 19417 RVA: 0x0022AAF1 File Offset: 0x00228CF1
		public LocString BestiarySeenQuote
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".bestiaryQuote");
			}
		}

		/// <summary>
		/// Unique Character x Monster quote. There are a lot of these. If this quote is missing, we display a placeholder quote
		/// from bestiary.json -&gt; "QUOTE_PLACEHOLDER"
		/// </summary>
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x0022AB12 File Offset: 0x00228D12
		[Nullable(2)]
		public LocString BestiaryKillQuote
		{
			[NullableContext(2)]
			get
			{
				return LocString.GetIfExists("characters", base.Id.Entry + ".bestiaryKillQuote");
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004BDB RID: 19419 RVA: 0x0022AB33 File Offset: 0x00228D33
		public LocString PronounObject
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".pronounObject");
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x0022AB54 File Offset: 0x00228D54
		public LocString PossessiveAdjective
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".possessiveAdjective");
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06004BDD RID: 19421 RVA: 0x0022AB75 File Offset: 0x00228D75
		public LocString PronounPossessive
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".pronounPossessive");
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0022AB96 File Offset: 0x00228D96
		public LocString PronounSubject
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".pronounSubject");
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004BDF RID: 19423 RVA: 0x0022ABB7 File Offset: 0x00228DB7
		public LocString CardsModifierTitle
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".cardsModifierTitle");
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x0022ABD8 File Offset: 0x00228DD8
		public LocString CardsModifierDescription
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".cardsModifierDescription");
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06004BE1 RID: 19425 RVA: 0x0022ABF9 File Offset: 0x00228DF9
		public LocString EventDeathPreventionLine
		{
			get
			{
				return new LocString("characters", base.Id.Entry + ".eventDeathPrevention");
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x0022AC1A File Offset: 0x00228E1A
		public string TrailPath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/card_trail_" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004BE3 RID: 19427
		public abstract int StartingHp { get; }

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004BE4 RID: 19428
		public abstract int StartingGold { get; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x0022AC3B File Offset: 0x00228E3B
		public virtual int MaxEnergy
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x0022AC3E File Offset: 0x00228E3E
		public virtual Color EnergyLabelOutlineColor
		{
			get
			{
				return new Color("0000000D");
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x0022AC4A File Offset: 0x00228E4A
		public virtual int BaseOrbSlotCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x0022AC4D File Offset: 0x00228E4D
		public virtual bool ShouldAlwaysShowStarCounter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004BE9 RID: 19433
		public abstract CardPoolModel CardPool { get; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004BEA RID: 19434
		public abstract RelicPoolModel RelicPool { get; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004BEB RID: 19435
		public abstract PotionPoolModel PotionPool { get; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004BEC RID: 19436
		public abstract IEnumerable<CardModel> StartingDeck { get; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004BED RID: 19437
		public abstract IReadOnlyList<RelicModel> StartingRelics { get; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x0022AC50 File Offset: 0x00228E50
		public virtual IReadOnlyList<PotionModel> StartingPotions
		{
			get
			{
				return Array.Empty<PotionModel>();
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x0022AC57 File Offset: 0x00228E57
		private string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0022AC78 File Offset: 0x00228E78
		public NCreatureVisuals CreateVisuals()
		{
			return PreloadManager.Cache.GetScene(this.VisualsPath).Instantiate<NCreatureVisuals>(0L);
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x0022AC91 File Offset: 0x00228E91
		private string IconTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/top_panel/character_icon_" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0022ACB7 File Offset: 0x00228EB7
		public Texture2D IconTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.IconTexturePath);
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x0022ACC9 File Offset: 0x00228EC9
		private string IconOutlineTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/top_panel/character_icon_" + base.Id.Entry.ToLowerInvariant() + "_outline.png");
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x0022ACEF File Offset: 0x00228EEF
		public Texture2D IconOutlineTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.IconOutlineTexturePath);
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x0022AD01 File Offset: 0x00228F01
		protected virtual string IconPath
		{
			get
			{
				return SceneHelper.GetScenePath("ui/character_icons/" + base.Id.Entry.ToLowerInvariant() + "_icon");
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0022AD27 File Offset: 0x00228F27
		public Control Icon
		{
			get
			{
				return PreloadManager.Cache.GetScene(this.IconPath).Instantiate<Control>(0L);
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004BF7 RID: 19447 RVA: 0x0022AD40 File Offset: 0x00228F40
		public string EnergyCounterPath
		{
			get
			{
				return SceneHelper.GetScenePath("combat/energy_counters/" + base.Id.Entry.ToLowerInvariant() + "_energy_counter");
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x0022AD66 File Offset: 0x00228F66
		public string MerchantAnimPath
		{
			get
			{
				return SceneHelper.GetScenePath("merchant/characters/" + base.Id.Entry.ToLowerInvariant() + "_merchant");
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x0022AD8C File Offset: 0x00228F8C
		public string RestSiteAnimPath
		{
			get
			{
				return SceneHelper.GetScenePath("rest_site/characters/" + base.Id.Entry.ToLowerInvariant() + "_rest_site");
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x0022ADB2 File Offset: 0x00228FB2
		private string ArmPointingTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/hands/multiplayer_hand_" + base.Id.Entry.ToLowerInvariant() + "_point.png");
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x0022ADD8 File Offset: 0x00228FD8
		public Texture2D ArmPointingTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ArmPointingTexturePath);
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0022ADEA File Offset: 0x00228FEA
		private string ArmRockTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/hands/multiplayer_hand_" + base.Id.Entry.ToLowerInvariant() + "_rock.png");
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x0022AE10 File Offset: 0x00229010
		public Texture2D ArmRockTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ArmRockTexturePath);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x0022AE22 File Offset: 0x00229022
		private string ArmPaperTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/hands/multiplayer_hand_" + base.Id.Entry.ToLowerInvariant() + "_paper.png");
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06004BFF RID: 19455 RVA: 0x0022AE48 File Offset: 0x00229048
		public Texture2D ArmPaperTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ArmPaperTexturePath);
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x0022AE5A File Offset: 0x0022905A
		private string ArmScissorsTexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/hands/multiplayer_hand_" + base.Id.Entry.ToLowerInvariant() + "_scissors.png");
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x0022AE80 File Offset: 0x00229080
		public Texture2D ArmScissorsTexture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.ArmScissorsTexturePath);
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x0022AE92 File Offset: 0x00229092
		public Achievement RunWonAchievement
		{
			get
			{
				return Enum.Parse<Achievement>(StringExtensions.Capitalize(base.Id.Entry) + "Win");
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x0022AEB3 File Offset: 0x002290B3
		protected virtual IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x0022AEBA File Offset: 0x002290BA
		public string CharacterSelectTitle
		{
			get
			{
				return base.Id.Entry.ToUpperInvariant() + ".title";
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x0022AED6 File Offset: 0x002290D6
		public string CharacterSelectDesc
		{
			get
			{
				return base.Id.Entry.ToUpperInvariant() + ".description";
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0022AEF2 File Offset: 0x002290F2
		public string CharacterSelectBg
		{
			get
			{
				return SceneHelper.GetScenePath("screens/char_select/char_select_bg_" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x0022AF13 File Offset: 0x00229113
		protected virtual string CharacterSelectIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/character_select/char_select_" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x0022AF39 File Offset: 0x00229139
		public CompressedTexture2D CharacterSelectIcon
		{
			get
			{
				return ResourceLoader.Load<CompressedTexture2D>(this.CharacterSelectIconPath, null, 1L);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x0022AF49 File Offset: 0x00229149
		protected virtual string CharacterSelectLockedIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/character_select/char_select_" + base.Id.Entry.ToLowerInvariant() + "_locked.png");
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0022AF6F File Offset: 0x0022916F
		public CompressedTexture2D CharacterSelectLockedIcon
		{
			get
			{
				return ResourceLoader.Load<CompressedTexture2D>(this.CharacterSelectLockedIconPath, null, 1L);
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x0022AF7F File Offset: 0x0022917F
		public string CharacterSelectTransitionPath
		{
			get
			{
				return "res://materials/transitions/" + base.Id.Entry.ToLowerInvariant() + "_transition_mat.tres";
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x0022AFA0 File Offset: 0x002291A0
		protected virtual string MapMarkerPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/map/icons/map_marker_" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x0022AFC6 File Offset: 0x002291C6
		public CompressedTexture2D MapMarker
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapMarkerPath);
			}
		}

		/// <summary>
		/// The color of the speech bubble during Ancient dialogues.
		/// </summary>
		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x0022AFD8 File Offset: 0x002291D8
		public virtual Color DialogueColor { get; } = new Color("28454f");

		/// <summary>
		/// Color of speech bubbles when the character talks (e.g. to the Architect)
		/// </summary>
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004C0F RID: 19471 RVA: 0x0022AFE0 File Offset: 0x002291E0
		public virtual VfxColor SpeechBubbleColor { get; } = VfxColor.Cyan;

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004C10 RID: 19472 RVA: 0x0022AFE8 File Offset: 0x002291E8
		public virtual Color MapDrawingColor
		{
			get
			{
				return Colors.Black;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004C11 RID: 19473 RVA: 0x0022AFEF File Offset: 0x002291EF
		public virtual Color RemoteTargetingLineColor
		{
			get
			{
				return Colors.Black;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004C12 RID: 19474 RVA: 0x0022AFF6 File Offset: 0x002291F6
		public virtual Color RemoteTargetingLineOutline
		{
			get
			{
				return Colors.Black;
			}
		}

		/// <summary>
		/// Assets required for the character select screen
		/// </summary>
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x0022AFFD File Offset: 0x002291FD
		public IEnumerable<string> AssetPathsCharacterSelect
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { this.CharacterSelectBg, this.CharacterSelectIconPath, this.IconTexturePath, this.CharacterSelectLockedIconPath, this.CharacterSelectTransitionPath });
			}
		}

		/// <summary>
		/// Assets required for the run.
		/// </summary>
		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004C14 RID: 19476 RVA: 0x0022B038 File Offset: 0x00229238
		public IEnumerable<string> AssetPaths
		{
			get
			{
				return new string[] { this.VisualsPath, this.IconTexturePath, this.IconPath, this.EnergyCounterPath, this.RestSiteAnimPath, this.MerchantAnimPath, this.CharacterSelectTransitionPath, this.MapMarkerPath, this.TrailPath }.Concat<string>(this.ExtraAssetPaths);
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004C15 RID: 19477
		public abstract float AttackAnimDelay { get; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004C16 RID: 19478
		public abstract float CastAnimDelay { get; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004C17 RID: 19479 RVA: 0x0022B0A8 File Offset: 0x002292A8
		public virtual float PowerUpAnimDelay
		{
			get
			{
				return this.CastAnimDelay;
			}
		}

		/// <summary>
		/// Get the VFX paths to use when this character is attacking <see cref="T:MegaCrit.Sts2.Core.Models.Events.TheArchitect" />.
		/// </summary>
		// Token: 0x06004C18 RID: 19480
		public abstract List<string> GetArchitectAttackVfx();

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x0022B0B0 File Offset: 0x002292B0
		public virtual string CharacterSelectSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/characters/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_select");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x0022B120 File Offset: 0x00229320
		public string AttackSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/characters/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_attack");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x0022B190 File Offset: 0x00229390
		public string CastSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/characters/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_cast");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x0022B200 File Offset: 0x00229400
		public string PowerUpSfx
		{
			get
			{
				return this.CastSfx;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x0022B208 File Offset: 0x00229408
		public string DeathSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/characters/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_die");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x0022B278 File Offset: 0x00229478
		public virtual string CharacterTransitionSfx
		{
			get
			{
				return "event:/sfx/ui/wipe_" + base.Id.Entry.ToLowerInvariant();
			}
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x0022B294 File Offset: 0x00229494
		public virtual CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			AnimState animState6 = new AnimState("relaxed_loop", true);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState6.AddBranch("Idle", animState, null);
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("PowerUp", animState2, null);
			creatureAnimator.AddAnyState("Relaxed", animState6, null);
			return creatureAnimator;
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x0022B37D File Offset: 0x0022957D
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Add details about this character to the given LocString.
		/// </summary>
		// Token: 0x06004C21 RID: 19489 RVA: 0x0022B380 File Offset: 0x00229580
		public void AddDetailsTo(LocString str)
		{
			str.Add("character", this.Title);
			str.Add("characterObject", this.TitleObject);
			str.Add("characterGender", this.Gender.ToString().ToLowerInvariant());
			str.Add("possessiveAdjective", this.PossessiveAdjective);
			str.Add("pronounObject", this.PronounObject);
			str.Add("pronounPossessive", this.PronounPossessive);
			str.Add("pronounSubject", this.PronounSubject);
		}

		/// <summary>
		/// Get the text to show on the character selection screen that explains how to unlock this character.
		/// </summary>
		// Token: 0x06004C22 RID: 19490 RVA: 0x0022B418 File Offset: 0x00229618
		public LocString GetUnlockText()
		{
			LocString locString = new LocString("characters", base.Id.Entry + ".unlockText");
			LocString locString2 = new LocString("characters", "LOCKED.title");
			LocString locString3;
			if (this.UnlocksAfterRunAs == null)
			{
				locString3 = locString2;
			}
			else
			{
				UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
				if (unlockState.Characters.Contains(this.UnlocksAfterRunAs))
				{
					locString3 = this.UnlocksAfterRunAs.Title;
				}
				else
				{
					locString3 = locString2;
				}
			}
			locString.Add("Prerequisite", locString3);
			return locString;
		}

		// Token: 0x04001D21 RID: 7457
		public const string locTable = "characters";

		// Token: 0x04001D24 RID: 7460
		protected const string _relaxedTrigger = "Relaxed";

		// Token: 0x04001D25 RID: 7461
		protected const string _idleTrigger = "Idle";

		// Token: 0x04001D26 RID: 7462
		public const string relaxedAnim = "relaxed_loop";
	}
}
