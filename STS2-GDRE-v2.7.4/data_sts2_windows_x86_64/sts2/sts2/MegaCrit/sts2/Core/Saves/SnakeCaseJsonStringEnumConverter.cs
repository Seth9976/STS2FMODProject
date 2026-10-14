using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000A1 RID: 161
	public class SnakeCaseJsonStringEnumConverter<TEnum> : JsonStringEnumConverter<TEnum> where TEnum : struct, Enum
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0003BC9B File Offset: 0x00039E9B
		public SnakeCaseJsonStringEnumConverter()
			: base(JsonNamingPolicy.SnakeCaseLower, true)
		{
		}
	}
}
