using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x02000167 RID: 359
	[NullableContext(1)]
	[Nullable(0)]
	public class NullLeaderboard
	{
		// Token: 0x04000606 RID: 1542
		[JsonPropertyName("name")]
		public string name = "";

		// Token: 0x04000607 RID: 1543
		[JsonPropertyName("entries")]
		public List<NullLeaderboardFileEntry> entries = new List<NullLeaderboardFileEntry>();
	}
}
