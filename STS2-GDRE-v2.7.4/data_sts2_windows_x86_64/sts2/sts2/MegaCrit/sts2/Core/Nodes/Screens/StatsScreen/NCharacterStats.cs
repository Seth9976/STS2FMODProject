using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000273 RID: 627
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NCharacterStats.cs")]
	public class NCharacterStats : Node
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x000E0944 File Offset: 0x000DEB44
		public static string[] AssetPaths
		{
			get
			{
				return new string[]
				{
					NCharacterStats.ScenePath,
					NCharacterStats._playtimeIconPath,
					NCharacterStats._winLossIconPath,
					NCharacterStats._chainIconPath
				};
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x000E096C File Offset: 0x000DEB6C
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/stats_screen/character_stats");
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000E0978 File Offset: 0x000DEB78
		public static NCharacterStats Create(CharacterStats characterStats)
		{
			NCharacterStats ncharacterStats = PreloadManager.Cache.GetScene(NCharacterStats.ScenePath).Instantiate<NCharacterStats>(0L);
			ncharacterStats._characterStats = characterStats;
			return ncharacterStats;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000E09A4 File Offset: 0x000DEBA4
		public override void _Ready()
		{
			CharacterModel byId = ModelDb.GetById<CharacterModel>(this._characterStats.Id);
			this._characterIcon = base.GetNode<Control>("%CharacterIcon");
			this._characterIcon.AddChildSafely(byId.Icon);
			this._statsContainer = base.GetNode<Node>("%StatsContainer");
			this._playtimeEntry = this.CreateSection(NCharacterStats._playtimeIconPath);
			this._winLossEntry = this.CreateSection(NCharacterStats._winLossIconPath);
			this._streakEntry = this.CreateSection(NCharacterStats._chainIconPath);
			this._nameLabel = base.GetNode<MegaLabel>("%NameLabel");
			this._unlocksLabel = base.GetNode<MegaLabel>("%UnlocksLabel");
			this._nameLabel.SetTextAutoSize(byId.Title.GetRawText());
			this._nameLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, byId.NameColor);
			this.LoadStats();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000E0A90 File Offset: 0x000DEC90
		private void LoadStats()
		{
			this._unlocksLabel.Visible = false;
			LocString locString = new LocString("stats_screen", "ENTRY_CHAR_PLAYTIME.top");
			locString.Add("Playtime", TimeFormatting.Format((float)this._characterStats.Playtime));
			this._playtimeEntry.SetTopText(locString.GetFormattedText());
			if (this._characterStats.FastestWinTime >= 0L)
			{
				locString = new LocString("stats_screen", "ENTRY_CHAR_PLAYTIME.bottom");
				locString.Add("FastestWin", TimeFormatting.Format((float)this._characterStats.FastestWinTime));
				this._playtimeEntry.SetBottomText(locString.GetFormattedText());
			}
			locString = new LocString("stats_screen", "ENTRY_CHAR_WIN_LOSS.top");
			if (this._characterStats.MaxAscension > 0)
			{
				locString.Add("Amount", this._characterStats.MaxAscension);
				this._winLossEntry.SetTopText("[red]" + locString.GetFormattedText() + "[/red]");
			}
			locString = new LocString("stats_screen", "ENTRY_CHAR_WIN_LOSS.bottom");
			locString.Add("Wins", this._characterStats.TotalWins);
			locString.Add("Losses", this._characterStats.TotalLosses);
			this._winLossEntry.SetBottomText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_CHAR_STREAK.top");
			locString.Add("Amount", this._characterStats.CurrentWinStreak);
			this._streakEntry.SetTopText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_CHAR_STREAK.bottom");
			locString.Add("Amount", this._characterStats.BestWinStreak);
			this._streakEntry.SetBottomText(locString.GetFormattedText());
		}

		/// <summary>
		/// Helper function to create a Section in the StatsScreen.
		/// </summary>
		/// <param name="imgUrl"></param>
		// Token: 0x06002124 RID: 8484 RVA: 0x000E0C5C File Offset: 0x000DEE5C
		private NStatEntry CreateSection(string imgUrl)
		{
			NStatEntry nstatEntry = NStatEntry.Create(imgUrl);
			this._statsContainer.AddChildSafely(nstatEntry);
			return nstatEntry;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002125 RID: 8485 RVA: 0x000E0C80 File Offset: 0x000DEE80
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NCharacterStats.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterStats.MethodName.LoadStats, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterStats.MethodName.CreateSection, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "imgUrl", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002126 RID: 8486 RVA: 0x000E0D54 File Offset: 0x000DEF54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCharacterStats.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterStats.MethodName.LoadStats && args.Count == 0)
			{
				this.LoadStats();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterStats.MethodName.CreateSection && args.Count == 1)
			{
				NStatEntry nstatEntry = this.CreateSection(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002127 RID: 8487 RVA: 0x000E0DEE File Offset: 0x000DEFEE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCharacterStats.MethodName._Ready || (ref method) == NCharacterStats.MethodName.LoadStats || (ref method) == NCharacterStats.MethodName.CreateSection || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002128 RID: 8488 RVA: 0x000E0E24 File Offset: 0x000DF024
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCharacterStats.PropertyName._characterIcon)
			{
				this._characterIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._statsContainer)
			{
				this._statsContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._nameLabel)
			{
				this._nameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._unlocksLabel)
			{
				this._unlocksLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._playtimeEntry)
			{
				this._playtimeEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._winLossEntry)
			{
				this._winLossEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._streakEntry)
			{
				this._streakEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002129 RID: 8489 RVA: 0x000E0EF8 File Offset: 0x000DF0F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCharacterStats.PropertyName._characterIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterIcon);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._statsContainer)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._statsContainer);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._nameLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameLabel);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._unlocksLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._unlocksLabel);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._playtimeEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._playtimeEntry);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._winLossEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._winLossEntry);
				return true;
			}
			if ((ref name) == NCharacterStats.PropertyName._streakEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._streakEntry);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600212A RID: 8490 RVA: 0x000E0FF0 File Offset: 0x000DF1F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCharacterStats.PropertyName._characterIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._statsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._nameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._unlocksLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._playtimeEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._winLossEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCharacterStats.PropertyName._streakEntry, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600212B RID: 8491 RVA: 0x000E10EC File Offset: 0x000DF2EC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCharacterStats.PropertyName._characterIcon, Variant.From<Control>(ref this._characterIcon));
			info.AddProperty(NCharacterStats.PropertyName._statsContainer, Variant.From<Node>(ref this._statsContainer));
			info.AddProperty(NCharacterStats.PropertyName._nameLabel, Variant.From<MegaLabel>(ref this._nameLabel));
			info.AddProperty(NCharacterStats.PropertyName._unlocksLabel, Variant.From<MegaLabel>(ref this._unlocksLabel));
			info.AddProperty(NCharacterStats.PropertyName._playtimeEntry, Variant.From<NStatEntry>(ref this._playtimeEntry));
			info.AddProperty(NCharacterStats.PropertyName._winLossEntry, Variant.From<NStatEntry>(ref this._winLossEntry));
			info.AddProperty(NCharacterStats.PropertyName._streakEntry, Variant.From<NStatEntry>(ref this._streakEntry));
		}

		/// <inheritdoc />
		// Token: 0x0600212C RID: 8492 RVA: 0x000E119C File Offset: 0x000DF39C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCharacterStats.PropertyName._characterIcon, ref variant))
			{
				this._characterIcon = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCharacterStats.PropertyName._statsContainer, ref variant2))
			{
				this._statsContainer = variant2.As<Node>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCharacterStats.PropertyName._nameLabel, ref variant3))
			{
				this._nameLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCharacterStats.PropertyName._unlocksLabel, ref variant4))
			{
				this._unlocksLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCharacterStats.PropertyName._playtimeEntry, ref variant5))
			{
				this._playtimeEntry = variant5.As<NStatEntry>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCharacterStats.PropertyName._winLossEntry, ref variant6))
			{
				this._winLossEntry = variant6.As<NStatEntry>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCharacterStats.PropertyName._streakEntry, ref variant7))
			{
				this._streakEntry = variant7.As<NStatEntry>();
			}
		}

		// Token: 0x04000CE6 RID: 3302
		private static readonly string _playtimeIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_clock.tres");

		// Token: 0x04000CE7 RID: 3303
		private static readonly string _winLossIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_swords.tres");

		// Token: 0x04000CE8 RID: 3304
		private static readonly string _chainIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chain.tres");

		// Token: 0x04000CE9 RID: 3305
		private CharacterStats _characterStats;

		// Token: 0x04000CEA RID: 3306
		private Control _characterIcon;

		// Token: 0x04000CEB RID: 3307
		private Node _statsContainer;

		// Token: 0x04000CEC RID: 3308
		private MegaLabel _nameLabel;

		// Token: 0x04000CED RID: 3309
		private MegaLabel _unlocksLabel;

		// Token: 0x04000CEE RID: 3310
		private NStatEntry _playtimeEntry;

		// Token: 0x04000CEF RID: 3311
		private NStatEntry _winLossEntry;

		// Token: 0x04000CF0 RID: 3312
		private NStatEntry _streakEntry;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012FB RID: 4859
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049DC RID: 18908
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'LoadStats' method.
			/// </summary>
			// Token: 0x040049DD RID: 18909
			public static readonly StringName LoadStats = "LoadStats";

			/// <summary>
			/// Cached name for the 'CreateSection' method.
			/// </summary>
			// Token: 0x040049DE RID: 18910
			public static readonly StringName CreateSection = "CreateSection";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012FC RID: 4860
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_characterIcon' field.
			/// </summary>
			// Token: 0x040049DF RID: 18911
			public static readonly StringName _characterIcon = "_characterIcon";

			/// <summary>
			/// Cached name for the '_statsContainer' field.
			/// </summary>
			// Token: 0x040049E0 RID: 18912
			public static readonly StringName _statsContainer = "_statsContainer";

			/// <summary>
			/// Cached name for the '_nameLabel' field.
			/// </summary>
			// Token: 0x040049E1 RID: 18913
			public static readonly StringName _nameLabel = "_nameLabel";

			/// <summary>
			/// Cached name for the '_unlocksLabel' field.
			/// </summary>
			// Token: 0x040049E2 RID: 18914
			public static readonly StringName _unlocksLabel = "_unlocksLabel";

			/// <summary>
			/// Cached name for the '_playtimeEntry' field.
			/// </summary>
			// Token: 0x040049E3 RID: 18915
			public static readonly StringName _playtimeEntry = "_playtimeEntry";

			/// <summary>
			/// Cached name for the '_winLossEntry' field.
			/// </summary>
			// Token: 0x040049E4 RID: 18916
			public static readonly StringName _winLossEntry = "_winLossEntry";

			/// <summary>
			/// Cached name for the '_streakEntry' field.
			/// </summary>
			// Token: 0x040049E5 RID: 18917
			public static readonly StringName _streakEntry = "_streakEntry";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012FD RID: 4861
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
