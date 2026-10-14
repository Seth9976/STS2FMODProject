using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class CardInfo : IGameInfo
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00010F0F File Offset: 0x0000F10F
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00010F17 File Offset: 0x0000F117
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00010F20 File Offset: 0x0000F120
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00010F28 File Offset: 0x0000F128
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00010F31 File Offset: 0x0000F131
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00010F39 File Offset: 0x0000F139
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00010F42 File Offset: 0x0000F142
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00010F4A File Offset: 0x0000F14A
		[RequiredMember]
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00010F53 File Offset: 0x0000F153
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00010F5B File Offset: 0x0000F15B
		[RequiredMember]
		[JsonPropertyName("upgraded")]
		public bool Upgraded { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00010F64 File Offset: 0x0000F164
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00010F6C File Offset: 0x0000F16C
		[RequiredMember]
		[JsonPropertyName("color")]
		public string Color { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00010F75 File Offset: 0x0000F175
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00010F7D File Offset: 0x0000F17D
		[RequiredMember]
		[JsonPropertyName("rarity")]
		public string Rarity { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00010F86 File Offset: 0x0000F186
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00010F8E File Offset: 0x0000F18E
		[RequiredMember]
		[JsonPropertyName("type")]
		public string Type { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00010F97 File Offset: 0x0000F197
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00010F9F File Offset: 0x0000F19F
		[RequiredMember]
		[JsonPropertyName("base_damage")]
		public int BaseDamage { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00010FA8 File Offset: 0x0000F1A8
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		[RequiredMember]
		[JsonPropertyName("energy")]
		public int Energy { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00010FB9 File Offset: 0x0000F1B9
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00010FC1 File Offset: 0x0000F1C1
		[RequiredMember]
		[JsonPropertyName("star_cost")]
		public int StarCost { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00010FCA File Offset: 0x0000F1CA
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00010FD2 File Offset: 0x0000F1D2
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00010FDB File Offset: 0x0000F1DB
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00010FE3 File Offset: 0x0000F1E3
		[RequiredMember]
		[JsonPropertyName("has_art")]
		public bool HasArt { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00010FEC File Offset: 0x0000F1EC
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		[RequiredMember]
		[JsonPropertyName("has_joke_art")]
		public bool HasJokeArt { get; set; }

		// Token: 0x060001B1 RID: 433 RVA: 0x00010FFD File Offset: 0x0000F1FD
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public CardInfo()
		{
		}
	}
}
