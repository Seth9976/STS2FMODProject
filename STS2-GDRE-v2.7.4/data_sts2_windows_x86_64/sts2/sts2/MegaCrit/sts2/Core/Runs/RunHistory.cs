using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x0200010C RID: 268
	[NullableContext(1)]
	[Nullable(0)]
	public class RunHistory : ISaveSchema
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0004DF48 File Offset: 0x0004C148
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0004DF50 File Offset: 0x0004C150
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0004DF59 File Offset: 0x0004C159
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0004DF61 File Offset: 0x0004C161
		[JsonPropertyName("platform_type")]
		public PlatformType PlatformType { get; set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0004DF6A File Offset: 0x0004C16A
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0004DF72 File Offset: 0x0004C172
		[JsonPropertyName("game_mode")]
		public GameMode GameMode { get; set; } = GameMode.Standard;

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0004DF7B File Offset: 0x0004C17B
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0004DF83 File Offset: 0x0004C183
		[JsonPropertyName("win")]
		public bool Win { get; set; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0004DF8C File Offset: 0x0004C18C
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0004DF94 File Offset: 0x0004C194
		[JsonPropertyName("seed")]
		public string Seed { get; set; } = "";

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0004DF9D File Offset: 0x0004C19D
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0004DFA5 File Offset: 0x0004C1A5
		[JsonPropertyName("start_time")]
		public long StartTime { get; set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0004DFAE File Offset: 0x0004C1AE
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0004DFB6 File Offset: 0x0004C1B6
		[JsonPropertyName("run_time")]
		public float RunTime { get; set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0004DFBF File Offset: 0x0004C1BF
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0004DFC7 File Offset: 0x0004C1C7
		[JsonPropertyName("ascension")]
		public int Ascension { get; set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0004DFD0 File Offset: 0x0004C1D0
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0004DFD8 File Offset: 0x0004C1D8
		[JsonPropertyName("build_id")]
		public string BuildId { get; set; } = "pre-v0.42";

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0004DFE1 File Offset: 0x0004C1E1
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0004DFE9 File Offset: 0x0004C1E9
		[JsonPropertyName("was_abandoned")]
		public bool WasAbandoned { get; set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0004DFF2 File Offset: 0x0004C1F2
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0004DFFA File Offset: 0x0004C1FA
		[JsonPropertyName("killed_by_encounter")]
		public ModelId KilledByEncounter { get; set; } = ModelId.none;

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0004E003 File Offset: 0x0004C203
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0004E00B File Offset: 0x0004C20B
		[JsonPropertyName("killed_by_event")]
		public ModelId KilledByEvent { get; set; } = ModelId.none;

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0004E014 File Offset: 0x0004C214
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0004E01C File Offset: 0x0004C21C
		[JsonPropertyName("players")]
		public List<RunHistoryPlayer> Players { get; set; } = new List<RunHistoryPlayer>();

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0004E025 File Offset: 0x0004C225
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0004E02D File Offset: 0x0004C22D
		[JsonPropertyName("acts")]
		public List<ModelId> Acts { get; set; } = new List<ModelId>();

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0004E036 File Offset: 0x0004C236
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0004E03E File Offset: 0x0004C23E
		[JsonPropertyName("modifiers")]
		public List<SerializableModifier> Modifiers { get; set; } = new List<SerializableModifier>();

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0004E047 File Offset: 0x0004C247
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0004E04F File Offset: 0x0004C24F
		[JsonPropertyName("map_point_history")]
		public List<List<MapPointHistoryEntry>> MapPointHistory { get; set; } = new List<List<MapPointHistoryEntry>>();
	}
}
