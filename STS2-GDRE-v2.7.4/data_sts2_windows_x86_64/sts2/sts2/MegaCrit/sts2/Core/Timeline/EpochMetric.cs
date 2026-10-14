using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Timeline
{
	// Token: 0x02000037 RID: 55
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class EpochMetric
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00011A06 File Offset: 0x0000FC06
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00011A0E File Offset: 0x0000FC0E
		[RequiredMember]
		[JsonPropertyName("buildId")]
		public string BuildId { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00011A17 File Offset: 0x0000FC17
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00011A1F File Offset: 0x0000FC1F
		[RequiredMember]
		[JsonPropertyName("epoch")]
		public string Epoch { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00011A28 File Offset: 0x0000FC28
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00011A30 File Offset: 0x0000FC30
		[JsonPropertyName("totalEpochs")]
		public int TotalEpochs { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00011A39 File Offset: 0x0000FC39
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00011A41 File Offset: 0x0000FC41
		[JsonPropertyName("totalPlaytime")]
		public long TotalPlaytime { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00011A4A File Offset: 0x0000FC4A
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00011A52 File Offset: 0x0000FC52
		[JsonPropertyName("totalRuns")]
		public int TotalRuns { get; set; }

		// Token: 0x0600023D RID: 573 RVA: 0x00011A5B File Offset: 0x0000FC5B
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public EpochMetric()
		{
		}
	}
}
