using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000B3 RID: 179
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableRun : ISaveSchema, IPacketSerializable
	{
		/// <summary>
		/// The schema version of this save.
		/// </summary>
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00040CCF File Offset: 0x0003EECF
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00040CD7 File Offset: 0x0003EED7
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00040CE0 File Offset: 0x0003EEE0
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00040CE8 File Offset: 0x0003EEE8
		[JsonPropertyName("acts")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SerializableActModel> Acts { get; set; } = new List<SerializableActModel>();

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00040CF1 File Offset: 0x0003EEF1
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00040CF9 File Offset: 0x0003EEF9
		[JsonPropertyName("modifiers")]
		public List<SerializableModifier> Modifiers { get; set; } = new List<SerializableModifier>();

		/// <summary>
		/// This is null if the run is not a daily.
		/// Otherwise, it contains the date from the time server of the daily.
		/// </summary>
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00040D02 File Offset: 0x0003EF02
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x00040D0A File Offset: 0x0003EF0A
		[JsonPropertyName("dailyTime")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
		public DateTimeOffset? DailyTime { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00040D13 File Offset: 0x0003EF13
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00040D1B File Offset: 0x0003EF1B
		[JsonPropertyName("current_act_index")]
		public int CurrentActIndex { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00040D24 File Offset: 0x0003EF24
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x00040D2C File Offset: 0x0003EF2C
		[JsonPropertyName("events_seen")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<ModelId> EventsSeen { get; set; } = new List<ModelId>();

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00040D35 File Offset: 0x0003EF35
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x00040D3D File Offset: 0x0003EF3D
		[Nullable(2)]
		[JsonPropertyName("pre_finished_room")]
		public SerializableRoom PreFinishedRoom
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00040D46 File Offset: 0x0003EF46
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x00040D4E File Offset: 0x0003EF4E
		[JsonPropertyName("odds")]
		public SerializableRunOddsSet SerializableOdds { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00040D57 File Offset: 0x0003EF57
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x00040D5F File Offset: 0x0003EF5F
		[JsonPropertyName("shared_relic_grab_bag")]
		public SerializableRelicGrabBag SerializableSharedRelicGrabBag { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00040D68 File Offset: 0x0003EF68
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00040D70 File Offset: 0x0003EF70
		[JsonPropertyName("players")]
		public List<SerializablePlayer> Players { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00040D79 File Offset: 0x0003EF79
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00040D81 File Offset: 0x0003EF81
		[JsonPropertyName("rng")]
		public SerializableRunRngSet SerializableRng { get; set; }

		/// <summary>
		/// The map coordinates you've visited in the current Act.
		/// </summary>
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00040D8A File Offset: 0x0003EF8A
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x00040D92 File Offset: 0x0003EF92
		[JsonPropertyName("visited_map_coords")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<MapCoord> VisitedMapCoords { get; set; } = new List<MapCoord>();

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00040D9B File Offset: 0x0003EF9B
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00040DA3 File Offset: 0x0003EFA3
		[JsonPropertyName("map_point_history")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<List<MapPointHistoryEntry>> MapPointHistory { get; set; } = new List<List<MapPointHistoryEntry>>();

		/// <summary>
		/// When this save was created or last updated.
		/// </summary>
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00040DAC File Offset: 0x0003EFAC
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00040DB4 File Offset: 0x0003EFB4
		[JsonPropertyName("save_time")]
		public long SaveTime { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00040DBD File Offset: 0x0003EFBD
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00040DC5 File Offset: 0x0003EFC5
		[JsonPropertyName("start_time")]
		public long StartTime { get; set; }

		/// <summary>
		/// The amount of seconds that has elapsed for this run.
		/// </summary>
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00040DCE File Offset: 0x0003EFCE
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00040DD6 File Offset: 0x0003EFD6
		[JsonPropertyName("run_time")]
		public long RunTime { get; set; }

		/// <summary>
		/// The exact moment when a Win was clocked on the RunTime. (Currently when you beat the Act 3 boss)
		/// </summary>
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00040DDF File Offset: 0x0003EFDF
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00040DE7 File Offset: 0x0003EFE7
		[JsonPropertyName("win_time")]
		public long WinTime { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00040DF0 File Offset: 0x0003EFF0
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00040DF8 File Offset: 0x0003EFF8
		[JsonPropertyName("ascension")]
		public int Ascension { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00040E01 File Offset: 0x0003F001
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00040E09 File Offset: 0x0003F009
		[JsonPropertyName("num_reloads")]
		public int NumReloads { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00040E12 File Offset: 0x0003F012
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00040E1A File Offset: 0x0003F01A
		[JsonPropertyName("platform_type")]
		public PlatformType PlatformType { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00040E23 File Offset: 0x0003F023
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00040E2B File Offset: 0x0003F02B
		[Nullable(2)]
		[JsonConverter(typeof(SerializableMapDrawingsJsonConverter))]
		[JsonPropertyName("map_drawings")]
		public SerializableMapDrawings MapDrawings
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00040E34 File Offset: 0x0003F034
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00040E3C File Offset: 0x0003F03C
		[JsonPropertyName("extra_fields")]
		public SerializableExtraRunFields ExtraFields { get; set; } = new SerializableExtraRunFields();

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00040E45 File Offset: 0x0003F045
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00040E4D File Offset: 0x0003F04D
		[JsonPropertyName("game_mode")]
		public GameMode GameMode { get; set; }

		// Token: 0x06000898 RID: 2200 RVA: 0x00040E58 File Offset: 0x0003F058
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.SchemaVersion, 32);
			writer.WriteList<SerializableActModel>(this.Acts, 32);
			writer.WriteList<SerializableModifier>(this.Modifiers, 32);
			writer.WriteBool(this.DailyTime != null);
			if (this.DailyTime != null)
			{
				writer.WriteLong(this.DailyTime.Value.ToUnixTimeSeconds(), 64);
			}
			writer.WriteEnum<GameMode>(this.GameMode);
			writer.WriteInt(this.CurrentActIndex, 4);
			writer.WriteModelEntriesInList(this.EventsSeen);
			writer.WriteBool(this.PreFinishedRoom != null);
			if (this.PreFinishedRoom != null)
			{
				writer.Write<SerializableRoom>(this.PreFinishedRoom);
			}
			writer.Write<SerializableRunOddsSet>(this.SerializableOdds);
			writer.WriteList<SerializablePlayer>(this.Players, 32);
			writer.Write<SerializableRunRngSet>(this.SerializableRng);
			writer.Write<SerializableRelicGrabBag>(this.SerializableSharedRelicGrabBag);
			writer.WriteList<MapCoord>(this.VisitedMapCoords, 32);
			writer.WriteInt(this.MapPointHistory.Count, 32);
			foreach (List<MapPointHistoryEntry> list in this.MapPointHistory)
			{
				writer.WriteList<MapPointHistoryEntry>(list, 32);
			}
			writer.WriteLong(this.SaveTime, 64);
			writer.WriteLong(this.StartTime, 64);
			writer.WriteLong(this.RunTime, 64);
			writer.WriteLong(this.WinTime, 64);
			writer.WriteInt(this.Ascension, 8);
			writer.WriteBool(this.MapDrawings != null);
			if (this.MapDrawings != null)
			{
				writer.Write<SerializableMapDrawings>(this.MapDrawings);
			}
			writer.Write<SerializableExtraRunFields>(this.ExtraFields);
			writer.WriteInt(this.NumReloads, 32);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00041038 File Offset: 0x0003F238
		public void Deserialize(PacketReader reader)
		{
			this.SchemaVersion = reader.ReadInt(32);
			this.Acts = reader.ReadList<SerializableActModel>(32);
			this.Modifiers = reader.ReadList<SerializableModifier>(32);
			if (reader.ReadBool())
			{
				this.DailyTime = new DateTimeOffset?(DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong(64)));
			}
			this.GameMode = reader.ReadEnum<GameMode>();
			this.CurrentActIndex = reader.ReadInt(4);
			this.EventsSeen = reader.ReadModelIdListAssumingType<EventModel>();
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.PreFinishedRoom = reader.Read<SerializableRoom>();
			}
			this.SerializableOdds = reader.Read<SerializableRunOddsSet>();
			this.Players = reader.ReadList<SerializablePlayer>(32);
			this.SerializableRng = reader.Read<SerializableRunRngSet>();
			this.SerializableSharedRelicGrabBag = reader.Read<SerializableRelicGrabBag>();
			this.VisitedMapCoords = reader.ReadList<MapCoord>(32);
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				List<MapPointHistoryEntry> list = reader.ReadList<MapPointHistoryEntry>(32);
				this.MapPointHistory.Add(list);
			}
			this.SaveTime = reader.ReadLong(64);
			this.StartTime = reader.ReadLong(64);
			this.RunTime = reader.ReadLong(64);
			this.WinTime = reader.ReadLong(64);
			this.Ascension = reader.ReadInt(8);
			bool flag2 = reader.ReadBool();
			if (flag2)
			{
				this.MapDrawings = reader.Read<SerializableMapDrawings>();
			}
			this.ExtraFields = reader.Read<SerializableExtraRunFields>();
			this.NumReloads = reader.ReadInt(32);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000411AC File Offset: 0x0003F3AC
		public SerializableRun Anonymized()
		{
			SerializableRun serializableRun = new SerializableRun();
			serializableRun.SchemaVersion = this.SchemaVersion;
			serializableRun.Acts = this.Acts;
			serializableRun.Modifiers = this.Modifiers;
			serializableRun.DailyTime = this.DailyTime;
			serializableRun.CurrentActIndex = this.CurrentActIndex;
			serializableRun.EventsSeen = this.EventsSeen;
			serializableRun.GameMode = this.GameMode;
			serializableRun.PreFinishedRoom = this.PreFinishedRoom;
			serializableRun.SerializableOdds = this.SerializableOdds;
			serializableRun.SerializableSharedRelicGrabBag = this.SerializableSharedRelicGrabBag;
			serializableRun.Players = this.Players.Select<SerializablePlayer, SerializablePlayer>((SerializablePlayer p) => p.Anonymized()).ToList<SerializablePlayer>();
			serializableRun.SerializableRng = this.SerializableRng;
			serializableRun.VisitedMapCoords = this.VisitedMapCoords;
			serializableRun.MapPointHistory = this.MapPointHistory.Select<List<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((List<MapPointHistoryEntry> l) => l.Select<MapPointHistoryEntry, MapPointHistoryEntry>((MapPointHistoryEntry h) => h.Anonymized()).ToList<MapPointHistoryEntry>()).ToList<List<MapPointHistoryEntry>>();
			serializableRun.SaveTime = this.SaveTime;
			serializableRun.StartTime = this.StartTime;
			serializableRun.RunTime = this.RunTime;
			serializableRun.WinTime = this.WinTime;
			serializableRun.Ascension = this.Ascension;
			serializableRun.PlatformType = this.PlatformType;
			SerializableMapDrawings mapDrawings = this.MapDrawings;
			serializableRun.MapDrawings = ((mapDrawings != null) ? mapDrawings.Anonymized() : null);
			serializableRun.ExtraFields = this.ExtraFields;
			return serializableRun;
		}

		/// <summary>
		/// The furthest floor count reached during this run.
		/// Used when uploading the "floor" value when uploading to the daily leaderboards.
		/// </summary>
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00041324 File Offset: 0x0003F524
		[JsonIgnore]
		public int FloorReached
		{
			get
			{
				return this.MapPointHistory.Sum<List<MapPointHistoryEntry>>((List<MapPointHistoryEntry> c) => c.Count);
			}
		}
	}
}
