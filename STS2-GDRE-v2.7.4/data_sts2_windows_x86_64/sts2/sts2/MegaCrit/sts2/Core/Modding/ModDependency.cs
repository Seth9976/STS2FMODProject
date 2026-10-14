using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B91 RID: 2961
	[NullableContext(1)]
	[Nullable(0)]
	public class ModDependency
	{
		// Token: 0x06007BB8 RID: 31672 RVA: 0x002A1367 File Offset: 0x0029F567
		public ModDependency(string id, [Nullable(2)] string minVersion = null)
		{
			this.id = id;
			this.minVersion = minVersion;
		}

		// Token: 0x0400291E RID: 10526
		[JsonPropertyName("id")]
		public string id;

		// Token: 0x0400291F RID: 10527
		[Nullable(2)]
		[JsonPropertyName("min_version")]
		public string minVersion;
	}
}
