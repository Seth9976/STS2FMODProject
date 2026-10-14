using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Achievements
{
	// Token: 0x02000D9C RID: 3484
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class AchievementMetric
	{
		// Token: 0x17002674 RID: 9844
		// (get) Token: 0x06008C19 RID: 35865 RVA: 0x002EC764 File Offset: 0x002EA964
		// (set) Token: 0x06008C1A RID: 35866 RVA: 0x002EC76C File Offset: 0x002EA96C
		[RequiredMember]
		[JsonPropertyName("buildId")]
		public string BuildId { get; set; }

		// Token: 0x17002675 RID: 9845
		// (get) Token: 0x06008C1B RID: 35867 RVA: 0x002EC775 File Offset: 0x002EA975
		// (set) Token: 0x06008C1C RID: 35868 RVA: 0x002EC77D File Offset: 0x002EA97D
		[RequiredMember]
		[JsonPropertyName("achievement")]
		public string Achievement { get; set; }

		// Token: 0x17002676 RID: 9846
		// (get) Token: 0x06008C1D RID: 35869 RVA: 0x002EC786 File Offset: 0x002EA986
		// (set) Token: 0x06008C1E RID: 35870 RVA: 0x002EC78E File Offset: 0x002EA98E
		[JsonPropertyName("totalAchievements")]
		public int TotalAchievements { get; set; }

		// Token: 0x17002677 RID: 9847
		// (get) Token: 0x06008C1F RID: 35871 RVA: 0x002EC797 File Offset: 0x002EA997
		// (set) Token: 0x06008C20 RID: 35872 RVA: 0x002EC79F File Offset: 0x002EA99F
		[JsonPropertyName("totalPlaytime")]
		public long TotalPlaytime { get; set; }

		// Token: 0x17002678 RID: 9848
		// (get) Token: 0x06008C21 RID: 35873 RVA: 0x002EC7A8 File Offset: 0x002EA9A8
		// (set) Token: 0x06008C22 RID: 35874 RVA: 0x002EC7B0 File Offset: 0x002EA9B0
		[JsonPropertyName("totalRuns")]
		public int TotalRuns { get; set; }

		// Token: 0x06008C23 RID: 35875 RVA: 0x002EC7B9 File Offset: 0x002EA9B9
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public AchievementMetric()
		{
		}
	}
}
