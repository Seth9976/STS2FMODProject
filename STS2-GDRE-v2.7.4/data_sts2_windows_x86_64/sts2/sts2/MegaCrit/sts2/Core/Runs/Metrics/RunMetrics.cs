using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x0200011B RID: 283
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class RunMetrics
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0005AA95 File Offset: 0x00058C95
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0005AA9D File Offset: 0x00058C9D
		[RequiredMember]
		public string BuildId { get; set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0005AAA6 File Offset: 0x00058CA6
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0005AAAE File Offset: 0x00058CAE
		[RequiredMember]
		public string PlayerId { get; set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0005AAB7 File Offset: 0x00058CB7
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x0005AABF File Offset: 0x00058CBF
		[RequiredMember]
		public ModelId Character { get; set; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0005AAC8 File Offset: 0x00058CC8
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0005AAD0 File Offset: 0x00058CD0
		[RequiredMember]
		public bool Win { get; set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0005AAD9 File Offset: 0x00058CD9
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x0005AAE1 File Offset: 0x00058CE1
		[RequiredMember]
		public int NumPlayers { get; set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0005AAEA File Offset: 0x00058CEA
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x0005AAF2 File Offset: 0x00058CF2
		[RequiredMember]
		public List<ModelId> Team { get; set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0005AAFB File Offset: 0x00058CFB
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x0005AB03 File Offset: 0x00058D03
		[RequiredMember]
		public string BuildType { get; set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0005AB0C File Offset: 0x00058D0C
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x0005AB14 File Offset: 0x00058D14
		public int Ascension { get; set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0005AB1D File Offset: 0x00058D1D
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x0005AB25 File Offset: 0x00058D25
		[RequiredMember]
		public float TotalPlaytime { get; set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0005AB2E File Offset: 0x00058D2E
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0005AB36 File Offset: 0x00058D36
		[RequiredMember]
		public float TotalWinRate { get; set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0005AB3F File Offset: 0x00058D3F
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0005AB47 File Offset: 0x00058D47
		[RequiredMember]
		public int NumReloads { get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0005AB50 File Offset: 0x00058D50
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0005AB58 File Offset: 0x00058D58
		[RequiredMember]
		public float RunPlaytime { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0005AB61 File Offset: 0x00058D61
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0005AB69 File Offset: 0x00058D69
		[RequiredMember]
		public int FloorReached { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0005AB72 File Offset: 0x00058D72
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0005AB7A File Offset: 0x00058D7A
		[RequiredMember]
		public ModelId KilledByEncounter { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0005AB83 File Offset: 0x00058D83
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0005AB8B File Offset: 0x00058D8B
		[RequiredMember]
		public List<CardChoiceMetric> CardChoices { get; set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0005AB94 File Offset: 0x00058D94
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x0005AB9C File Offset: 0x00058D9C
		[RequiredMember]
		public List<string> CampfireUpgrades { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0005ABA5 File Offset: 0x00058DA5
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x0005ABAD File Offset: 0x00058DAD
		[RequiredMember]
		public List<EventChoiceMetric> EventChoices { get; set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0005ABB6 File Offset: 0x00058DB6
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x0005ABBE File Offset: 0x00058DBE
		[RequiredMember]
		public List<AncientMetric> AncientChoices { get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0005ABC7 File Offset: 0x00058DC7
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x0005ABCF File Offset: 0x00058DCF
		[RequiredMember]
		public List<string> RelicBuys { get; set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0005ABD8 File Offset: 0x00058DD8
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x0005ABE0 File Offset: 0x00058DE0
		[RequiredMember]
		public List<string> PotionBuys { get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0005ABE9 File Offset: 0x00058DE9
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x0005ABF1 File Offset: 0x00058DF1
		[RequiredMember]
		public List<string> ColorlessBuys { get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x0005ABFA File Offset: 0x00058DFA
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x0005AC02 File Offset: 0x00058E02
		[RequiredMember]
		public List<string> PotionDiscards { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0005AC0B File Offset: 0x00058E0B
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x0005AC13 File Offset: 0x00058E13
		[RequiredMember]
		public List<EncounterMetric> Encounters { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0005AC1C File Offset: 0x00058E1C
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0005AC24 File Offset: 0x00058E24
		[RequiredMember]
		public List<ActWinMetric> ActWins { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0005AC2D File Offset: 0x00058E2D
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0005AC35 File Offset: 0x00058E35
		[RequiredMember]
		public IEnumerable<ModelId> Deck { get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0005AC3E File Offset: 0x00058E3E
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0005AC46 File Offset: 0x00058E46
		[RequiredMember]
		public IEnumerable<ModelId> Relics { get; set; }

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0005AC4F File Offset: 0x00058E4F
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public RunMetrics()
		{
		}
	}
}
