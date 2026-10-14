using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004D7 RID: 1239
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ActModel : AbstractModel
	{
		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004A42 RID: 19010 RVA: 0x00226841 File Offset: 0x00224A41
		public LocString Title
		{
			get
			{
				return new LocString("acts", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004A43 RID: 19011 RVA: 0x00226862 File Offset: 0x00224A62
		protected string FilePathIdentifier
		{
			get
			{
				return base.Id.Entry.ToLowerInvariant();
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004A44 RID: 19012 RVA: 0x00226874 File Offset: 0x00224A74
		public string RestSiteBackgroundPath
		{
			get
			{
				return SceneHelper.GetScenePath("rest_site/" + this.FilePathIdentifier + "_rest_site");
			}
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x00226890 File Offset: 0x00224A90
		public Control CreateRestSiteBackground()
		{
			return PreloadManager.Cache.GetScene(this.RestSiteBackgroundPath).Instantiate<Control>(0L);
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004A46 RID: 19014 RVA: 0x002268AC File Offset: 0x00224AAC
		public string MapTopBgPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("packed/map/map_bgs/");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral("/map_top_");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004A47 RID: 19015 RVA: 0x0022690D File Offset: 0x00224B0D
		public Texture2D MapTopBg
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapTopBgPath);
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004A48 RID: 19016 RVA: 0x00226920 File Offset: 0x00224B20
		public string MapMidBgPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("packed/map/map_bgs/");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral("/map_middle_");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x00226981 File Offset: 0x00224B81
		public Texture2D MapMidBg
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapMidBgPath);
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x00226994 File Offset: 0x00224B94
		public string MapBotBgPath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("packed/map/map_bgs/");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral("/map_bottom_");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				return ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x002269F5 File Offset: 0x00224BF5
		public Texture2D MapBotBg
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.MapBotBgPath);
			}
		}

		/// <summary>
		/// The index where the act can appear when the list of acts is generated.
		/// 0 =&gt; first act, 1 =&gt; second act, 2 =&gt; third act.
		/// Returns negative if this act should never be organically shown to the player.
		/// </summary>
		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004A4C RID: 19020
		public abstract int Index { get; }

		/// <summary>
		/// Whether this is the default unlocked act.
		/// For example, Overgrowth is unlocked by default, but Underdocks is not.
		/// </summary>
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004A4D RID: 19021
		public abstract bool IsDefault { get; }

		/// <summary>
		/// Color of the dots on the map after you've traveled through them.
		/// Also affects the color of the Boss and Ancient nodes.
		/// </summary>
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004A4E RID: 19022
		public abstract Color MapTraveledColor { get; }

		/// <summary>
		/// Color of the dots on the map for paths you haven't traveled.
		/// Also affects the color of the Boss node before you get adjacent to it.
		/// </summary>
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004A4F RID: 19023
		public abstract Color MapUntraveledColor { get; }

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004A50 RID: 19024
		public abstract Color MapBgColor { get; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x00226A08 File Offset: 0x00224C08
		public IEnumerable<string> AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(this.BackgroundScenePath);
				list.Add(this.MapBotBgPath);
				list.Add(this.MapMidBgPath);
				list.Add(this.MapTopBgPath);
				IEnumerable<string> enumerable2;
				if (!this._rooms.HasAncient)
				{
					IEnumerable<string> enumerable = Array.Empty<string>();
					enumerable2 = enumerable;
				}
				else
				{
					enumerable2 = this._rooms.Ancient.MapNodeAssetPaths;
				}
				list.AddRange(enumerable2);
				list.AddRange(this._rooms.Boss.MapNodeAssetPaths);
				IEnumerable<string> enumerable3;
				if (!this._rooms.HasSecondBoss)
				{
					IEnumerable<string> enumerable = Array.Empty<string>();
					enumerable3 = enumerable;
				}
				else
				{
					enumerable3 = this._rooms.SecondBoss.MapNodeAssetPaths;
				}
				list.AddRange(enumerable3);
				return new <>z__ReadOnlyList<string>(list);
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004A52 RID: 19026
		public abstract string[] BgMusicOptions { get; }

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004A53 RID: 19027
		public abstract string[] MusicBankPaths { get; }

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004A54 RID: 19028
		public abstract string AmbientSfx { get; }

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x00226ABD File Offset: 0x00224CBD
		protected virtual int NumberOfWeakEncounters
		{
			get
			{
				return 3;
			}
		}

		/// <summary>
		/// The number of rooms we have for an act, not including any external modifications (ie multiplayer)
		/// NOTE: this excludes the boss room or ancient room
		/// </summary>
		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004A56 RID: 19030
		protected abstract int BaseNumberOfRooms { get; }

		/// <summary>
		/// Returns the number of rooms we have for an act accounting for if we are in multiplayer or not.
		/// NOTE: this excludes the boss room or ancient room
		/// </summary>
		/// <returns></returns>
		// Token: 0x06004A57 RID: 19031 RVA: 0x00226AC0 File Offset: 0x00224CC0
		public int GetNumberOfRooms(bool isMultiplayer)
		{
			int num = this.BaseNumberOfRooms;
			if (isMultiplayer)
			{
				num--;
			}
			return num;
		}

		/// <summary>
		/// Returns the number of floors for this act, which includes the boss floor and ancient floor
		/// </summary>
		/// <param name="isMultiplayer"></param>
		/// <returns></returns>
		// Token: 0x06004A58 RID: 19032 RVA: 0x00226ADC File Offset: 0x00224CDC
		public int GetNumberOfFloors(bool isMultiplayer)
		{
			return this.GetNumberOfRooms(isMultiplayer) + 2;
		}

		/// <summary>
		/// All the monster encounters in this act.
		/// Do not put conditional checks in here.
		/// </summary>
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x00226AE8 File Offset: 0x00224CE8
		public IEnumerable<EncounterModel> AllEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = this._allEncounters) == null)
				{
					enumerable = (this._allEncounters = this.GenerateAllEncounters());
				}
				return enumerable;
			}
		}

		/// <summary>
		/// Generates every encounter that is in this act.
		/// Overriden in subclasses, but should only be called once by <see cref="P:MegaCrit.Sts2.Core.Models.ActModel.AllEncounters" /> so it can be cached.
		/// </summary>
		// Token: 0x06004A5A RID: 19034
		public abstract IEnumerable<EncounterModel> GenerateAllEncounters();

		/// <summary>
		/// Returns true if the act is unlocked given the passed unlock state, false otherwise.
		/// </summary>
		// Token: 0x06004A5B RID: 19035
		public abstract bool IsUnlocked(UnlockState unlockState);

		/// <summary>
		/// All the weak monster encounters in this act.
		/// </summary>
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004A5C RID: 19036 RVA: 0x00226B10 File Offset: 0x00224D10
		public IEnumerable<EncounterModel> AllWeakEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = this._allWeakEncounters) == null)
				{
					IEnumerable<EncounterModel> enumerable2 = (this._allWeakEncounters = this.AllEncounters.Where<EncounterModel>((EncounterModel e) => e != null && e.RoomType == RoomType.Monster && e.IsWeak));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// All the regular (non-weak) monster encounters in this act.
		/// </summary>
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x00226B5C File Offset: 0x00224D5C
		public IEnumerable<EncounterModel> AllRegularEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = this._allRegularEncounters) == null)
				{
					IEnumerable<EncounterModel> enumerable2 = (this._allRegularEncounters = this.AllEncounters.Where<EncounterModel>((EncounterModel e) => e != null && e.RoomType == RoomType.Monster && !e.IsWeak));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// All the elite monster encounters in this act.
		/// </summary>
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x00226BA8 File Offset: 0x00224DA8
		public IEnumerable<EncounterModel> AllEliteEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = this._allEliteEncounters) == null)
				{
					IEnumerable<EncounterModel> enumerable2 = (this._allEliteEncounters = this.AllEncounters.Where<EncounterModel>((EncounterModel e) => e.RoomType == RoomType.Elite));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// All the boss encounters in this act.
		/// </summary>
		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004A5F RID: 19039 RVA: 0x00226BF4 File Offset: 0x00224DF4
		public IEnumerable<EncounterModel> AllBossEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = this._allBossEncounters) == null)
				{
					IEnumerable<EncounterModel> enumerable2 = (this._allBossEncounters = this.AllEncounters.Where<EncounterModel>((EncounterModel e) => e.RoomType == RoomType.Boss));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// All the monsters you can encounter in this act.
		/// </summary>
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x00226C40 File Offset: 0x00224E40
		public IEnumerable<MonsterModel> AllMonsters
		{
			get
			{
				IEnumerable<MonsterModel> enumerable;
				if ((enumerable = this._allMonsters) == null)
				{
					IEnumerable<MonsterModel> enumerable2 = (this._allMonsters = this.AllEncounters.SelectMany<EncounterModel, MonsterModel>((EncounterModel e) => e.AllPossibleMonsters).Distinct<MonsterModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x00226C91 File Offset: 0x00224E91
		public Achievement DefeatedAllEnemiesAchievement
		{
			get
			{
				return Enum.Parse<Achievement>("Defeat" + StringExtensions.Capitalize(base.Id.Entry) + "Enemies");
			}
		}

		/// <summary>
		/// The path for this act's Treasure Chest Spine animation resource.
		/// </summary>
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00226CB7 File Offset: 0x00224EB7
		public virtual string ChestSpineResourcePath
		{
			get
			{
				return "res://animations/backgrounds/treasure_room/chest_room_act_" + this.FilePathIdentifier + "_skel_data.tres";
			}
		}

		/// <summary>
		/// The normal (non-stroke) skin name for this act's chest.
		/// </summary>
		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004A63 RID: 19043
		public abstract string ChestSpineSkinNameNormal { get; }

		/// <summary>
		/// The stroke-outline skin name for this act's chest.
		/// </summary>
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004A64 RID: 19044
		public abstract string ChestSpineSkinNameStroke { get; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x00226CCE File Offset: 0x00224ECE
		public virtual MegaSkeletonDataResource ChestSpineResource
		{
			get
			{
				return new MegaSkeletonDataResource(PreloadManager.Cache.GetAsset<Resource>(this.ChestSpineResourcePath));
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004A66 RID: 19046
		public abstract string ChestOpenSfx { get; }

		/// <summary>
		/// Overridden by ActModel to define what order bosses should be encountered in.
		/// </summary>
		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004A67 RID: 19047
		public abstract IEnumerable<EncounterModel> BossDiscoveryOrder { get; }

		/// <summary>
		/// All the Ancients that are available in this act.
		/// Ignores Unlocks/Epoch state.
		/// Does not include shared Ancients.
		/// </summary>
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004A68 RID: 19048
		public abstract IEnumerable<AncientEventModel> AllAncients { get; }

		/// <summary>
		/// All the events that are available in this act (ignores Unlocks/Epoch state).
		/// Does not include shared events.
		/// </summary>
		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004A69 RID: 19049
		public abstract IEnumerable<EventModel> AllEvents { get; }

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004A6A RID: 19050 RVA: 0x00226CEA File Offset: 0x00224EEA
		// (set) Token: 0x06004A6B RID: 19051 RVA: 0x00226CFC File Offset: 0x00224EFC
		public ActModel CanonicalInstance
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

		// Token: 0x06004A6C RID: 19052 RVA: 0x00226D0B File Offset: 0x00224F0B
		protected override void DeepCloneFields()
		{
			this._rooms = new RoomSet();
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x00226D18 File Offset: 0x00224F18
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Returns every Ancient in this act that the player has unlocked.
		/// </summary>
		// Token: 0x06004A6E RID: 19054
		public abstract IEnumerable<AncientEventModel> GetUnlockedAncients(UnlockState state);

		// Token: 0x06004A6F RID: 19055 RVA: 0x00226D1C File Offset: 0x00224F1C
		protected string GetFullLayerPath(string layerName)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 3);
			defaultInterpolatedStringHandler.AppendLiteral("res://scenes/backgrounds/");
			defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
			defaultInterpolatedStringHandler.AppendLiteral("/layers/");
			defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted(layerName);
			defaultInterpolatedStringHandler.AppendLiteral(".tscn");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Set the subset of shared Ancients that will be available in a mutable act instance.
		/// Generally called in <see cref="M:MegaCrit.Sts2.Core.Runs.RunManager.GenerateRooms" />, and will differ from run to run.
		/// </summary>
		// Token: 0x06004A70 RID: 19056 RVA: 0x00226D8C File Offset: 0x00224F8C
		public void SetSharedAncientSubset(List<AncientEventModel> sharedAncientSubset)
		{
			base.AssertMutable();
			this._sharedAncientSubset = new List<AncientEventModel>();
			this._sharedAncientSubset.AddRange(sharedAncientSubset);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00226DAC File Offset: 0x00224FAC
		public IEnumerable<string> GetAllBackgroundLayerPaths()
		{
			string backgroundsPath = "res://scenes/backgrounds/" + this.FilePathIdentifier + "/layers";
			IEnumerable<string> enumerable;
			using (DirAccess dirAccess = DirAccess.Open(backgroundsPath))
			{
				if (dirAccess == null)
				{
					enumerable = Array.Empty<string>();
				}
				else
				{
					enumerable = (from path in dirAccess.GetFiles()
						where path.EndsWith(".tscn")
						select backgroundsPath + "/" + path).ToArray<string>();
				}
			}
			return enumerable;
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00226E50 File Offset: 0x00225050
		public void GenerateRooms(Rng rng, UnlockState unlockState, bool isMultiplayer = false)
		{
			base.AssertMutable();
			List<EventModel> list = this.AllEvents.Concat<EventModel>(ModelDb.AllSharedEvents).ToList<EventModel>();
			if (!unlockState.IsEpochRevealed<Event1Epoch>())
			{
				list.RemoveAll((EventModel e) => Event1Epoch.Events.Any<EventModel>((EventModel ev) => ev.Id == e.Id));
			}
			if (!unlockState.IsEpochRevealed<Event2Epoch>())
			{
				list.RemoveAll((EventModel e) => Event2Epoch.Events.Any<EventModel>((EventModel ev) => ev.Id == e.Id));
			}
			if (!unlockState.IsEpochRevealed<Event3Epoch>())
			{
				list.RemoveAll((EventModel e) => Event3Epoch.Events.Any<EventModel>((EventModel ev) => ev.Id == e.Id));
			}
			this._rooms.events.AddRange(list.UnstableShuffle<EventModel>(rng));
			GrabBag<EncounterModel> grabBag = new GrabBag<EncounterModel>();
			for (int i = 0; i < this.NumberOfWeakEncounters; i++)
			{
				if (!grabBag.Any())
				{
					foreach (EncounterModel encounterModel in this.AllWeakEncounters)
					{
						grabBag.Add(encounterModel, 1.0);
					}
				}
				ActModel.AddWithoutRepeatingTags(this._rooms.normalEncounters, grabBag, rng);
			}
			GrabBag<EncounterModel> grabBag2 = new GrabBag<EncounterModel>();
			for (int j = this.NumberOfWeakEncounters; j < this.GetNumberOfRooms(isMultiplayer); j++)
			{
				if (!grabBag2.Any())
				{
					foreach (EncounterModel encounterModel2 in this.AllRegularEncounters)
					{
						grabBag2.Add(encounterModel2, 1.0);
					}
				}
				ActModel.AddWithoutRepeatingTags(this._rooms.normalEncounters, grabBag2, rng);
			}
			GrabBag<EncounterModel> grabBag3 = new GrabBag<EncounterModel>();
			for (int k = 0; k < 15; k++)
			{
				if (!grabBag3.Any())
				{
					foreach (EncounterModel encounterModel3 in this.AllEliteEncounters)
					{
						grabBag3.Add(encounterModel3, 1.0);
					}
				}
				ActModel.AddWithoutRepeatingTags(this._rooms.eliteEncounters, grabBag3, rng);
			}
			this._rooms.Boss = rng.NextItem<EncounterModel>(this.AllBossEncounters);
			this._rooms.Ancient = rng.NextItem<AncientEventModel>(this.GetUnlockedAncients(unlockState).Concat<AncientEventModel>(this._sharedAncientSubset ?? new List<AncientEventModel>()));
		}

		/// <summary>
		/// Called after a load. Re-generates specific rooms if necessary.
		/// </summary>
		// Token: 0x06004A73 RID: 19059 RVA: 0x002270E8 File Offset: 0x002252E8
		public void ValidateRoomsAfterLoad(Rng rng)
		{
			if (this._rooms.Boss is DeprecatedEncounter)
			{
				this._rooms.Boss = rng.NextItem<EncounterModel>(this.AllBossEncounters.Where<EncounterModel>(delegate(EncounterModel e)
				{
					ModelId id = e.Id;
					EncounterModel secondBoss = this._rooms.SecondBoss;
					return id != ((secondBoss != null) ? secondBoss.Id : null);
				}));
			}
			if (this._rooms.SecondBoss is DeprecatedEncounter)
			{
				EncounterModel encounterModel = rng.NextItem<EncounterModel>(this.AllBossEncounters.Where<EncounterModel>((EncounterModel e) => e.Id != this._rooms.Boss.Id));
				this._rooms.SecondBoss = encounterModel;
			}
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0022716C File Offset: 0x0022536C
		public void ApplyDiscoveryOrderModifications(UnlockState unlockState)
		{
			foreach (EncounterModel encounterModel in this.BossDiscoveryOrder)
			{
				if (!unlockState.HasSeenEncounter(encounterModel))
				{
					this._rooms.Boss = encounterModel;
					break;
				}
			}
			this.ApplyActDiscoveryOrderModifications(unlockState);
		}

		// Token: 0x06004A75 RID: 19061
		protected abstract void ApplyActDiscoveryOrderModifications(UnlockState unlockState);

		// Token: 0x06004A76 RID: 19062 RVA: 0x002271D0 File Offset: 0x002253D0
		private static void AddWithoutRepeatingTags(ICollection<EncounterModel> encounters, GrabBag<EncounterModel> grabBag, Rng rng)
		{
			EncounterModel encounterModel = grabBag.GrabAndRemove(rng, (EncounterModel e) => !e.SharesTagsWith(encounters.LastOrDefault<EncounterModel>()) && e != encounters.LastOrDefault<EncounterModel>());
			if (encounterModel == null)
			{
				encounterModel = grabBag.GrabAndRemove(rng, null);
			}
			if (encounterModel != null)
			{
				encounters.Add(encounterModel);
			}
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x00227219 File Offset: 0x00225419
		public EventModel PullAncient()
		{
			return this._rooms.Ancient;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x00227228 File Offset: 0x00225428
		public EventModel PullNextEvent(RunState runState)
		{
			this._rooms.EnsureNextEventIsValid(runState);
			EventModel eventModel = Hook.ModifyNextEvent(runState, this._rooms.NextEvent);
			runState.AddVisitedEvent(eventModel);
			return eventModel;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0022725C File Offset: 0x0022545C
		public EncounterModel PullNextEncounter(RoomType roomType)
		{
			EncounterModel encounterModel;
			switch (roomType)
			{
			case RoomType.Monster:
				encounterModel = this._rooms.NextNormalEncounter;
				break;
			case RoomType.Elite:
				encounterModel = this._rooms.NextEliteEncounter;
				break;
			case RoomType.Boss:
				encounterModel = this._rooms.NextBossEncounter;
				break;
			default:
				throw new ArgumentOutOfRangeException("roomType", roomType, null);
			}
			return encounterModel;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x002272BC File Offset: 0x002254BC
		public void MarkRoomVisited(RoomType roomType)
		{
			this._rooms.MarkVisited(roomType);
		}

		/// <summary>
		/// The boss encounter that has been rolled for this mutable instance of the act.
		/// </summary>
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004A7B RID: 19067 RVA: 0x002272CA File Offset: 0x002254CA
		public EncounterModel BossEncounter
		{
			get
			{
				return this._rooms.Boss;
			}
		}

		/// <summary>
		/// The second boss encounter for Double Boss mode (Ascension 10+), if set.
		/// </summary>
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x002272D7 File Offset: 0x002254D7
		[Nullable(2)]
		public EncounterModel SecondBossEncounter
		{
			[NullableContext(2)]
			get
			{
				return this._rooms.SecondBoss;
			}
		}

		/// <summary>
		/// Whether this act has a second boss (Double Boss ascension mode).
		/// </summary>
		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004A7D RID: 19069 RVA: 0x002272E4 File Offset: 0x002254E4
		public bool HasSecondBoss
		{
			get
			{
				return this._rooms.HasSecondBoss;
			}
		}

		/// <summary>
		/// The Ancient event that has been rolled for this mutable instance of the act.
		/// </summary>
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x002272F1 File Offset: 0x002254F1
		public AncientEventModel Ancient
		{
			get
			{
				return this._rooms.Ancient;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004A7F RID: 19071 RVA: 0x00227300 File Offset: 0x00225500
		public string BackgroundScenePath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("backgrounds/");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(this.FilePathIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral("_background");
				return SceneHelper.GetScenePath(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x00227361 File Offset: 0x00225561
		public BackgroundAssets GenerateBackgroundAssets(Rng rng)
		{
			return new BackgroundAssets(this.FilePathIdentifier, rng);
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x0022736F File Offset: 0x0022556F
		public void SetBossEncounter(EncounterModel encounter)
		{
			base.AssertMutable();
			if (encounter.RoomType != RoomType.Boss)
			{
				throw new ArgumentException("The encounter must be a boss.");
			}
			this._rooms.Boss = encounter;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x00227397 File Offset: 0x00225597
		[NullableContext(2)]
		public void SetSecondBossEncounter(EncounterModel encounter)
		{
			base.AssertMutable();
			if (encounter != null && encounter.RoomType != RoomType.Boss)
			{
				throw new ArgumentException("The encounter must be a boss.");
			}
			this._rooms.SecondBoss = encounter;
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x002273C2 File Offset: 0x002255C2
		public void RemoveEventFromSet(EventModel eventModel)
		{
			eventModel.AssertCanonical();
			this._rooms.events.Remove(eventModel);
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x002273DC File Offset: 0x002255DC
		public ActModel ToMutable()
		{
			base.AssertCanonical();
			ActModel actModel = (ActModel)base.MutableClone();
			actModel.CanonicalInstance = this;
			return actModel;
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x00227403 File Offset: 0x00225603
		public SerializableActModel ToSave()
		{
			base.AssertMutable();
			return new SerializableActModel
			{
				Id = base.Id,
				SerializableRooms = this._rooms.ToSave()
			};
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x00227430 File Offset: 0x00225630
		public static ActModel FromSave(SerializableActModel save)
		{
			ActModel actModel = ModelDb.GetById<ActModel>(save.Id).ToMutable();
			actModel._rooms = RoomSet.FromSave(save.SerializableRooms);
			return actModel;
		}

		// Token: 0x06004A87 RID: 19079
		public abstract MapPointTypeCounts GetMapPointTypes(Rng mapRng);

		/// <summary>
		/// Creates the map for this act.
		/// </summary>
		// Token: 0x06004A88 RID: 19080 RVA: 0x00227460 File Offset: 0x00225660
		public ActMap CreateMap(RunState runState, bool replaceTreasureWithElites)
		{
			return StandardActMap.CreateFor(runState, replaceTreasureWithElites);
		}

		/// <summary>
		/// Get a list of random ActModels.
		/// The ActModel at index 0 will be Act 1, index 1 will be Act 2, etc.
		/// This list is not _truly_ random; a given ActModel will only be rolled for the act index it belongs in.
		/// For example, Overgrowth may or may not be at index 0, but it will never be at index 1 or 2.
		/// </summary>
		/// <param name="rng">Rng to use for randomly rolling alternate acts.</param>
		/// <param name="unlockState">The object to use for checking unlock state.</param>
		/// <param name="isMultiplayer">Set to true if we are in a multiplayer run. Disables checking local progress save
		/// for alt acts.</param>
		/// <returns>Randomized list of acts.</returns>
		// Token: 0x06004A89 RID: 19081 RVA: 0x0022746C File Offset: 0x0022566C
		public static IEnumerable<ActModel> GetRandomList(Rng rng, UnlockState unlockState, bool isMultiplayer)
		{
			IReadOnlyList<IReadOnlyList<ActModel>> actsByIndex = ModelDb.ActsByIndex;
			List<ActModel> list = new List<ActModel>();
			for (int i = 0; i < actsByIndex.Count; i++)
			{
				ActModel actModel = null;
				List<ActModel> list2 = new List<ActModel>();
				foreach (ActModel actModel2 in actsByIndex[i])
				{
					if (actModel2.IsUnlocked(unlockState))
					{
						if (!actModel2.IsDefault && !isMultiplayer && !SaveManager.Instance.Progress.DiscoveredActs.Contains(actModel2.Id) && TestMode.IsOff)
						{
							actModel = actModel2;
							break;
						}
						list2.Add(actModel2);
					}
				}
				if (actModel == null)
				{
					ActModel actModel3 = rng.NextItem<ActModel>(list2);
					if (actModel3 == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
						defaultInterpolatedStringHandler.AppendLiteral("No unlocked acts for index ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						defaultInterpolatedStringHandler.AppendLiteral("!");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					actModel = actModel3;
				}
				list.Add(actModel);
			}
			return list;
		}

		/// <summary>
		/// Get the default list of ActModels.
		/// Act 1 (index 0) is <see cref="T:MegaCrit.Sts2.Core.Models.Acts.Overgrowth" />.
		/// Act 2 is <see cref="T:MegaCrit.Sts2.Core.Models.Acts.Hive" />.
		/// Act 3 is <see cref="T:MegaCrit.Sts2.Core.Models.Acts.Glory" />.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06004A8A RID: 19082 RVA: 0x0022757C File Offset: 0x0022577C
		public static IReadOnlyList<ActModel> GetDefaultList()
		{
			IReadOnlyList<IReadOnlyList<ActModel>> actsByIndex = ModelDb.ActsByIndex;
			List<ActModel> list = new List<ActModel>();
			for (int i = 0; i < actsByIndex.Count; i++)
			{
				ActModel actModel = null;
				foreach (ActModel actModel2 in actsByIndex[i])
				{
					if (actModel2.IsDefault)
					{
						actModel = actModel2;
						break;
					}
				}
				if (actModel == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("No default act for index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				list.Add(actModel);
			}
			return list;
		}

		// Token: 0x04001CD1 RID: 7377
		protected RoomSet _rooms;

		// Token: 0x04001CD2 RID: 7378
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<EncounterModel> _allEncounters;

		// Token: 0x04001CD3 RID: 7379
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<EncounterModel> _allWeakEncounters;

		// Token: 0x04001CD4 RID: 7380
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<EncounterModel> _allRegularEncounters;

		// Token: 0x04001CD5 RID: 7381
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<EncounterModel> _allEliteEncounters;

		// Token: 0x04001CD6 RID: 7382
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<EncounterModel> _allBossEncounters;

		// Token: 0x04001CD7 RID: 7383
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<MonsterModel> _allMonsters;

		/// <summary>
		/// The subset of shared Ancients that will be available in a mutable act instance.
		/// Will differ from run to run.
		/// </summary>
		// Token: 0x04001CD8 RID: 7384
		[Nullable(new byte[] { 2, 1 })]
		private List<AncientEventModel> _sharedAncientSubset;

		// Token: 0x04001CD9 RID: 7385
		private ActModel _canonicalInstance;
	}
}
