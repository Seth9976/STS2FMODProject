using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x02000168 RID: 360
	public class NullLeaderboardFile : ISaveSchema
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00063FA7 File Offset: 0x000621A7
		// (set) Token: 0x060010F7 RID: 4343 RVA: 0x00063FAF File Offset: 0x000621AF
		[JsonPropertyName("version")]
		public int SchemaVersion { get; set; }

		// Token: 0x04000609 RID: 1545
		[Nullable(1)]
		[JsonPropertyName("leaderboards")]
		public List<NullLeaderboard> leaderboards = new List<NullLeaderboard>();
	}
}
