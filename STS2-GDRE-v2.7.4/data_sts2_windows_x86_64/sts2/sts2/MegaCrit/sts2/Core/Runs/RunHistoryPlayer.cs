using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x0200010D RID: 269
	[NullableContext(1)]
	[Nullable(0)]
	public class RunHistoryPlayer
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0004E0CA File Offset: 0x0004C2CA
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x0004E0D2 File Offset: 0x0004C2D2
		[JsonPropertyName("id")]
		public ulong Id { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0004E0DB File Offset: 0x0004C2DB
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0004E0E3 File Offset: 0x0004C2E3
		[JsonPropertyName("character")]
		public ModelId Character { get; set; } = ModelId.none;

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0004E0EC File Offset: 0x0004C2EC
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x0004E0F4 File Offset: 0x0004C2F4
		[JsonPropertyName("deck")]
		public IEnumerable<SerializableCard> Deck { get; set; } = new List<SerializableCard>();

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0004E0FD File Offset: 0x0004C2FD
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x0004E105 File Offset: 0x0004C305
		[JsonPropertyName("relics")]
		public IEnumerable<SerializableRelic> Relics { get; set; } = new List<SerializableRelic>();

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0004E10E File Offset: 0x0004C30E
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x0004E116 File Offset: 0x0004C316
		[JsonPropertyName("potions")]
		public IEnumerable<SerializablePotion> Potions { get; set; } = new List<SerializablePotion>();

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0004E11F File Offset: 0x0004C31F
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x0004E127 File Offset: 0x0004C327
		[JsonPropertyName("badges")]
		public IEnumerable<SerializableBadge> Badges { get; set; } = new List<SerializableBadge>();

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0004E130 File Offset: 0x0004C330
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0004E138 File Offset: 0x0004C338
		[JsonPropertyName("max_potion_slot_count")]
		public int MaxPotionSlotCount { get; set; } = 3;
	}
}
