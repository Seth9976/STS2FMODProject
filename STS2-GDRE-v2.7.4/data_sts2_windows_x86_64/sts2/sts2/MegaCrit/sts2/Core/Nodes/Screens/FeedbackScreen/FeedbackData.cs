using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000320 RID: 800
	[NullableContext(1)]
	[Nullable(0)]
	public struct FeedbackData
	{
		// Token: 0x040010F4 RID: 4340
		[JsonPropertyName("description")]
		public string description;

		// Token: 0x040010F5 RID: 4341
		[JsonPropertyName("category")]
		public string category;

		// Token: 0x040010F6 RID: 4342
		[JsonPropertyName("game_version")]
		public string gameVersion;

		// Token: 0x040010F7 RID: 4343
		[JsonPropertyName("unique_id")]
		public string uniqueId;

		// Token: 0x040010F8 RID: 4344
		[JsonPropertyName("commit")]
		public string commit;

		// Token: 0x040010F9 RID: 4345
		[Nullable(2)]
		[JsonPropertyName("platform_branch")]
		public string platformBranch;

		// Token: 0x040010FA RID: 4346
		[JsonPropertyName("session_id")]
		public string sessionId;

		// Token: 0x040010FB RID: 4347
		[JsonPropertyName("is_modded")]
		public bool isModded;

		// Token: 0x040010FC RID: 4348
		[JsonPropertyName("is_full_console")]
		public bool isFullConsole;

		// Token: 0x040010FD RID: 4349
		[JsonPropertyName("lang")]
		public string lang;
	}
}
