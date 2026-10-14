using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.GameInfo
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	public class ModelIdMetricsConverter : JsonConverter<ModelId>
	{
		// Token: 0x06000130 RID: 304 RVA: 0x000097D5 File Offset: 0x000079D5
		public override ModelId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000097DC File Offset: 0x000079DC
		public override void Write(Utf8JsonWriter writer, ModelId value, JsonSerializerOptions options)
		{
			if (value == ModelId.none)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStringValue(value.Entry);
		}
	}
}
