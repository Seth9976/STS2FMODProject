using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x02000169 RID: 361
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class NullLeaderboardFileEntry
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x00063FCB File Offset: 0x000621CB
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public NullLeaderboardFileEntry()
		{
		}

		// Token: 0x0400060A RID: 1546
		[RequiredMember]
		[JsonPropertyName("name")]
		public string name;

		// Token: 0x0400060B RID: 1547
		[JsonPropertyName("score")]
		public int score;

		// Token: 0x0400060C RID: 1548
		[JsonPropertyName("id")]
		public ulong id;

		// Token: 0x0400060D RID: 1549
		[JsonPropertyName("other_ids")]
		public List<ulong> userIds = new List<ulong>();
	}
}
