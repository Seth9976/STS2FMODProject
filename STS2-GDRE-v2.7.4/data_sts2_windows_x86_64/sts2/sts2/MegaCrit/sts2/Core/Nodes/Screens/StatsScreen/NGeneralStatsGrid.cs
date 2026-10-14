using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000274 RID: 628
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NGeneralStatsGrid.cs")]
	public class NGeneralStatsGrid : Control
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x000E12AC File Offset: 0x000DF4AC
		public static string[] AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(NGeneralStatsGrid._achievementsIconPath);
				list.Add(NGeneralStatsGrid._playtimeIconPath);
				list.Add(NGeneralStatsGrid._cardsIconPath);
				list.Add(NGeneralStatsGrid._winLossIconPath);
				list.Add(NGeneralStatsGrid._monsterIconPath);
				list.Add(NGeneralStatsGrid._ancientsIconPath);
				list.Add(NGeneralStatsGrid._relicIconPath);
				list.Add(NGeneralStatsGrid._potionIconPath);
				list.Add(NGeneralStatsGrid._eventsIconPath);
				list.Add(NGeneralStatsGrid._streakIconPath);
				list.AddRange(NCharacterStats.AssetPaths);
				return list.ToArray();
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000E133C File Offset: 0x000DF53C
		private static HoverTip PlaytimeTip
		{
			get
			{
				return new HoverTip(new LocString("stats_screen", "TIP_PLAYTIME.header"), new LocString("stats_screen", "TIP_PLAYTIME.description"), null);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x000E1362 File Offset: 0x000DF562
		private static HoverTip WinsLossesTip
		{
			get
			{
				return new HoverTip(new LocString("stats_screen", "TIP_WIN_LOSS.header"), new LocString("stats_screen", "TIP_WIN_LOSS.description"), null);
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000E1388 File Offset: 0x000DF588
		public override void _Ready()
		{
			this._gridContainer = base.GetNode<Control>("%GridContainer");
			this._characterStatContainer = base.GetNode<Control>("%CharacterStatsContainer");
			this._achievementsEntry = this.CreateSection(NGeneralStatsGrid._achievementsIconPath);
			this._playtimeEntry = this.CreateSection(NGeneralStatsGrid._playtimeIconPath);
			this._cardsEntry = this.CreateSection(NGeneralStatsGrid._cardsIconPath);
			this._winLossEntry = this.CreateSection(NGeneralStatsGrid._winLossIconPath);
			this._monsterEntry = this.CreateSection(NGeneralStatsGrid._monsterIconPath);
			this._relicEntry = this.CreateSection(NGeneralStatsGrid._relicIconPath);
			this._potionEntry = this.CreateSection(NGeneralStatsGrid._potionIconPath);
			this._eventsEntry = this.CreateSection(NGeneralStatsGrid._eventsIconPath);
			this._streakEntry = this.CreateSection(NGeneralStatsGrid._streakIconPath);
			this.SetupHoverTips();
		}

		/// <summary>
		/// Helper function to create a Section in the StatsScreen.
		/// </summary>
		// Token: 0x06002133 RID: 8499 RVA: 0x000E1460 File Offset: 0x000DF660
		private NStatEntry CreateSection(string imgUrl)
		{
			NStatEntry nstatEntry = NStatEntry.Create(imgUrl);
			this._gridContainer.AddChildSafely(nstatEntry);
			return nstatEntry;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000E1484 File Offset: 0x000DF684
		public void LoadStats()
		{
			ProgressState progressSave = SaveManager.Instance.Progress;
			SaveManager instance = SaveManager.Instance;
			LocString locString = new LocString("stats_screen", "ENTRY_ACHIEVEMENTS.top");
			int num = AchievementsUtil.TotalAchievementCount();
			int num2 = AchievementsUtil.UnlockedAchievementCount();
			locString.Add("Amount", StringHelper.RatioFormat(num2, num));
			this._achievementsEntry.SetTopText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_ACHIEVEMENTS.bottom");
			int num3 = progressSave.Epochs.Count<SerializableEpoch>((SerializableEpoch epoch) => epoch.State >= EpochState.Revealed);
			bool flag = EpochModel.AllEpochIds.All<string>((string id) => progressSave.Epochs.Any<SerializableEpoch>((SerializableEpoch epoch) => epoch.Id == id));
			if (flag)
			{
				int count = progressSave.Epochs.Count;
				locString.Add("Amount", StringHelper.RatioFormat(num3, count));
			}
			else
			{
				locString.Add("Amount", StringHelper.RatioFormat(num3.ToString(), "??"));
			}
			this._achievementsEntry.SetBottomText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_PLAYTIME.top");
			locString.Add("Playtime", TimeFormatting.Format((float)progressSave.TotalPlaytime));
			this._playtimeEntry.SetTopText(locString.GetFormattedText());
			if (progressSave.Wins > 0)
			{
				locString = new LocString("stats_screen", "ENTRY_PLAYTIME.bottom");
				locString.Add("FastestWin", TimeFormatting.Format((float)progressSave.FastestVictory));
				this._playtimeEntry.SetBottomText(locString.GetFormattedText());
			}
			locString = new LocString("stats_screen", "ENTRY_CARDS.top");
			locString.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedCards(), SaveManager.GetUnlockableCardCount()));
			this._cardsEntry.SetTopText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_CARDS.bottom");
			locString.Add("Amount", StringHelper.RatioFormat(progressSave.DiscoveredCards.Count, ModelDb.AllCards.Count<CardModel>()));
			this._cardsEntry.SetBottomText(locString.GetFormattedText());
			int aggregateAscensionProgress = instance.GetAggregateAscensionProgress();
			if (aggregateAscensionProgress > 0)
			{
				locString = new LocString("stats_screen", "ENTRY_WIN_LOSS.top");
				locString.Add("Amount", StringHelper.RatioFormat(aggregateAscensionProgress, SaveManager.GetAggregateAscensionCount()));
				this._winLossEntry.SetTopText(locString.GetFormattedText() ?? "");
			}
			locString = new LocString("stats_screen", "ENTRY_WIN_LOSS.bottom");
			locString.Add("Wins", progressSave.Wins);
			locString.Add("Losses", progressSave.Losses);
			this._winLossEntry.SetBottomText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_MONSTER.top");
			locString.Add("Amount", StringHelper.Radix(instance.GetTotalKills()));
			this._monsterEntry.SetTopText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_MONSTER.bottom");
			locString.Add("Amount", StringHelper.RatioFormat(instance.Progress.EnemyStats.Count, ModelDb.Monsters.Count<MonsterModel>()));
			this._monsterEntry.SetBottomText(locString.GetFormattedText() ?? "");
			HashSet<ModelId> hashSet = ModelDb.AllEvents.Select<EventModel, ModelId>((EventModel e) => e.Id).ToHashSet<ModelId>();
			int num4 = progressSave.DiscoveredEvents.Intersect<ModelId>(hashSet).Count<ModelId>();
			int num5 = progressSave.DiscoveredEvents.Except<ModelId>(hashSet).Count<ModelId>();
			locString = new LocString("stats_screen", "ENTRY_RELIC.top");
			locString.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedRelics(), SaveManager.GetUnlockableRelicCount()));
			this._relicEntry.SetTopText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_RELIC.bottom");
			locString.Add("Amount", StringHelper.RatioFormat(progressSave.DiscoveredRelics.Count, ModelDb.AllRelics.Count<RelicModel>()));
			this._relicEntry.SetBottomText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_POTION.top");
			locString.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedPotions(), SaveManager.GetUnlockablePotionCount()));
			this._potionEntry.SetTopText(locString.GetFormattedText() ?? "");
			locString = new LocString("stats_screen", "ENTRY_POTION.bottom");
			locString.Add("Amount", ModelDb.AllPotions.Count<PotionModel>());
			this._potionEntry.SetBottomText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_EVENTS.top");
			locString.Add("Amount", "N/A");
			this._eventsEntry.SetTopText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_EVENTS.bottom");
			locString.Add("Amount", StringHelper.RatioFormat(num4, hashSet.Count));
			this._eventsEntry.SetBottomText(locString.GetFormattedText());
			locString = new LocString("stats_screen", "ENTRY_STREAK.top");
			locString.Add("Amount", progressSave.BestWinStreak);
			this._streakEntry.SetTopText(locString.GetFormattedText());
			if (aggregateAscensionProgress > 999999999)
			{
				locString = new LocString("stats_screen", "ENTRY_STREAK.bottom");
				locString.Add("Amount", 5m);
				this._streakEntry.SetBottomText("[red]" + locString.GetFormattedText() + "[/red]");
			}
			this._characterStatContainer.FreeChildren();
			this.CreateCharacterSection(progressSave, ModelDb.Character<Ironclad>().Id);
			this.CreateCharacterSection(progressSave, ModelDb.Character<Silent>().Id);
			this.CreateCharacterSection(progressSave, ModelDb.Character<Regent>().Id);
			this.CreateCharacterSection(progressSave, ModelDb.Character<Necrobinder>().Id);
			this.CreateCharacterSection(progressSave, ModelDb.Character<Defect>().Id);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000E1AE0 File Offset: 0x000DFCE0
		private void CreateCharacterSection(ProgressState progressSave, ModelId id)
		{
			CharacterStats statsForCharacter = progressSave.GetStatsForCharacter(id);
			if (statsForCharacter != null)
			{
				NCharacterStats ncharacterStats = NCharacterStats.Create(statsForCharacter);
				this._characterStatContainer.AddChildSafely(ncharacterStats);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000E1B0C File Offset: 0x000DFD0C
		private void SetupHoverTips()
		{
			this._playtimeEntry.SetHoverTip(NGeneralStatsGrid.PlaytimeTip);
			int aggregateAscensionProgress = SaveManager.Instance.GetAggregateAscensionProgress();
			if (aggregateAscensionProgress > 0)
			{
				this._winLossEntry.SetHoverTip(NGeneralStatsGrid.WinsLossesTip);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002137 RID: 8503 RVA: 0x000E1B48 File Offset: 0x000DFD48
		public Control DefaultFocusedControl
		{
			get
			{
				return this._achievementsEntry;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002138 RID: 8504 RVA: 0x000E1B50 File Offset: 0x000DFD50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGeneralStatsGrid.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGeneralStatsGrid.MethodName.CreateSection, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "imgUrl", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGeneralStatsGrid.MethodName.LoadStats, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGeneralStatsGrid.MethodName.SetupHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002139 RID: 8505 RVA: 0x000E1C54 File Offset: 0x000DFE54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGeneralStatsGrid.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGeneralStatsGrid.MethodName.CreateSection && args.Count == 1)
			{
				NStatEntry nstatEntry = this.CreateSection(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
				return true;
			}
			if ((ref method) == NGeneralStatsGrid.MethodName.LoadStats && args.Count == 0)
			{
				this.LoadStats();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGeneralStatsGrid.MethodName.SetupHoverTips && args.Count == 0)
			{
				this.SetupHoverTips();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600213A RID: 8506 RVA: 0x000E1D14 File Offset: 0x000DFF14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGeneralStatsGrid.MethodName._Ready || (ref method) == NGeneralStatsGrid.MethodName.CreateSection || (ref method) == NGeneralStatsGrid.MethodName.LoadStats || (ref method) == NGeneralStatsGrid.MethodName.SetupHoverTips || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600213B RID: 8507 RVA: 0x000E1D64 File Offset: 0x000DFF64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGeneralStatsGrid.PropertyName._gridContainer)
			{
				this._gridContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._achievementsEntry)
			{
				this._achievementsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._playtimeEntry)
			{
				this._playtimeEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._cardsEntry)
			{
				this._cardsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._winLossEntry)
			{
				this._winLossEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._monsterEntry)
			{
				this._monsterEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._relicEntry)
			{
				this._relicEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._potionEntry)
			{
				this._potionEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._eventsEntry)
			{
				this._eventsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._streakEntry)
			{
				this._streakEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._characterStatContainer)
			{
				this._characterStatContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600213C RID: 8508 RVA: 0x000E1EC0 File Offset: 0x000E00C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGeneralStatsGrid.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._gridContainer)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._gridContainer);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._achievementsEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._achievementsEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._playtimeEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._playtimeEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._cardsEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._cardsEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._winLossEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._winLossEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._monsterEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._monsterEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._relicEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._relicEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._potionEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._potionEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._eventsEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._eventsEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._streakEntry)
			{
				value = VariantUtils.CreateFrom<NStatEntry>(ref this._streakEntry);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._characterStatContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterStatContainer);
				return true;
			}
			if ((ref name) == NGeneralStatsGrid.PropertyName._screenTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600213D RID: 8509 RVA: 0x000E2078 File Offset: 0x000E0278
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._gridContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._achievementsEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._playtimeEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._cardsEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._winLossEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._monsterEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._relicEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._potionEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._eventsEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._streakEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._characterStatContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName._screenTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGeneralStatsGrid.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600213E RID: 8510 RVA: 0x000E223C File Offset: 0x000E043C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGeneralStatsGrid.PropertyName._gridContainer, Variant.From<Node>(ref this._gridContainer));
			info.AddProperty(NGeneralStatsGrid.PropertyName._achievementsEntry, Variant.From<NStatEntry>(ref this._achievementsEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._playtimeEntry, Variant.From<NStatEntry>(ref this._playtimeEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._cardsEntry, Variant.From<NStatEntry>(ref this._cardsEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._winLossEntry, Variant.From<NStatEntry>(ref this._winLossEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._monsterEntry, Variant.From<NStatEntry>(ref this._monsterEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._relicEntry, Variant.From<NStatEntry>(ref this._relicEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._potionEntry, Variant.From<NStatEntry>(ref this._potionEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._eventsEntry, Variant.From<NStatEntry>(ref this._eventsEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._streakEntry, Variant.From<NStatEntry>(ref this._streakEntry));
			info.AddProperty(NGeneralStatsGrid.PropertyName._characterStatContainer, Variant.From<Control>(ref this._characterStatContainer));
			info.AddProperty(NGeneralStatsGrid.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
		}

		/// <inheritdoc />
		// Token: 0x0600213F RID: 8511 RVA: 0x000E2358 File Offset: 0x000E0558
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._gridContainer, ref variant))
			{
				this._gridContainer = variant.As<Node>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._achievementsEntry, ref variant2))
			{
				this._achievementsEntry = variant2.As<NStatEntry>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._playtimeEntry, ref variant3))
			{
				this._playtimeEntry = variant3.As<NStatEntry>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._cardsEntry, ref variant4))
			{
				this._cardsEntry = variant4.As<NStatEntry>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._winLossEntry, ref variant5))
			{
				this._winLossEntry = variant5.As<NStatEntry>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._monsterEntry, ref variant6))
			{
				this._monsterEntry = variant6.As<NStatEntry>();
			}
			Variant variant7;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._relicEntry, ref variant7))
			{
				this._relicEntry = variant7.As<NStatEntry>();
			}
			Variant variant8;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._potionEntry, ref variant8))
			{
				this._potionEntry = variant8.As<NStatEntry>();
			}
			Variant variant9;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._eventsEntry, ref variant9))
			{
				this._eventsEntry = variant9.As<NStatEntry>();
			}
			Variant variant10;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._streakEntry, ref variant10))
			{
				this._streakEntry = variant10.As<NStatEntry>();
			}
			Variant variant11;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._characterStatContainer, ref variant11))
			{
				this._characterStatContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._screenTween, ref variant12))
			{
				this._screenTween = variant12.As<Tween>();
			}
		}

		// Token: 0x04000CF1 RID: 3313
		private static readonly string _achievementsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_achievements.tres");

		// Token: 0x04000CF2 RID: 3314
		private static readonly string _playtimeIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_clock.tres");

		// Token: 0x04000CF3 RID: 3315
		private static readonly string _cardsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_cards.tres");

		// Token: 0x04000CF4 RID: 3316
		private static readonly string _winLossIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_swords.tres");

		// Token: 0x04000CF5 RID: 3317
		private static readonly string _monsterIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_monsters.tres");

		// Token: 0x04000CF6 RID: 3318
		private static readonly string _ancientsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_ancients.tres");

		// Token: 0x04000CF7 RID: 3319
		private static readonly string _relicIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chest.tres");

		// Token: 0x04000CF8 RID: 3320
		private static readonly string _potionIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_potions_seen.tres");

		// Token: 0x04000CF9 RID: 3321
		private static readonly string _eventsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_questionmark.tres");

		// Token: 0x04000CFA RID: 3322
		private static readonly string _streakIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chain.tres");

		// Token: 0x04000CFB RID: 3323
		private Node _gridContainer;

		// Token: 0x04000CFC RID: 3324
		private NStatEntry _achievementsEntry;

		// Token: 0x04000CFD RID: 3325
		private NStatEntry _playtimeEntry;

		// Token: 0x04000CFE RID: 3326
		private NStatEntry _cardsEntry;

		// Token: 0x04000CFF RID: 3327
		private NStatEntry _winLossEntry;

		// Token: 0x04000D00 RID: 3328
		private NStatEntry _monsterEntry;

		// Token: 0x04000D01 RID: 3329
		private NStatEntry _relicEntry;

		// Token: 0x04000D02 RID: 3330
		private NStatEntry _potionEntry;

		// Token: 0x04000D03 RID: 3331
		private NStatEntry _eventsEntry;

		// Token: 0x04000D04 RID: 3332
		private NStatEntry _streakEntry;

		// Token: 0x04000D05 RID: 3333
		private Control _characterStatContainer;

		// Token: 0x04000D06 RID: 3334
		[Nullable(2)]
		private Tween _screenTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012FE RID: 4862
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049E6 RID: 18918
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CreateSection' method.
			/// </summary>
			// Token: 0x040049E7 RID: 18919
			public static readonly StringName CreateSection = "CreateSection";

			/// <summary>
			/// Cached name for the 'LoadStats' method.
			/// </summary>
			// Token: 0x040049E8 RID: 18920
			public static readonly StringName LoadStats = "LoadStats";

			/// <summary>
			/// Cached name for the 'SetupHoverTips' method.
			/// </summary>
			// Token: 0x040049E9 RID: 18921
			public static readonly StringName SetupHoverTips = "SetupHoverTips";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012FF RID: 4863
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040049EA RID: 18922
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_gridContainer' field.
			/// </summary>
			// Token: 0x040049EB RID: 18923
			public static readonly StringName _gridContainer = "_gridContainer";

			/// <summary>
			/// Cached name for the '_achievementsEntry' field.
			/// </summary>
			// Token: 0x040049EC RID: 18924
			public static readonly StringName _achievementsEntry = "_achievementsEntry";

			/// <summary>
			/// Cached name for the '_playtimeEntry' field.
			/// </summary>
			// Token: 0x040049ED RID: 18925
			public static readonly StringName _playtimeEntry = "_playtimeEntry";

			/// <summary>
			/// Cached name for the '_cardsEntry' field.
			/// </summary>
			// Token: 0x040049EE RID: 18926
			public static readonly StringName _cardsEntry = "_cardsEntry";

			/// <summary>
			/// Cached name for the '_winLossEntry' field.
			/// </summary>
			// Token: 0x040049EF RID: 18927
			public static readonly StringName _winLossEntry = "_winLossEntry";

			/// <summary>
			/// Cached name for the '_monsterEntry' field.
			/// </summary>
			// Token: 0x040049F0 RID: 18928
			public static readonly StringName _monsterEntry = "_monsterEntry";

			/// <summary>
			/// Cached name for the '_relicEntry' field.
			/// </summary>
			// Token: 0x040049F1 RID: 18929
			public static readonly StringName _relicEntry = "_relicEntry";

			/// <summary>
			/// Cached name for the '_potionEntry' field.
			/// </summary>
			// Token: 0x040049F2 RID: 18930
			public static readonly StringName _potionEntry = "_potionEntry";

			/// <summary>
			/// Cached name for the '_eventsEntry' field.
			/// </summary>
			// Token: 0x040049F3 RID: 18931
			public static readonly StringName _eventsEntry = "_eventsEntry";

			/// <summary>
			/// Cached name for the '_streakEntry' field.
			/// </summary>
			// Token: 0x040049F4 RID: 18932
			public static readonly StringName _streakEntry = "_streakEntry";

			/// <summary>
			/// Cached name for the '_characterStatContainer' field.
			/// </summary>
			// Token: 0x040049F5 RID: 18933
			public static readonly StringName _characterStatContainer = "_characterStatContainer";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x040049F6 RID: 18934
			public static readonly StringName _screenTween = "_screenTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001300 RID: 4864
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
