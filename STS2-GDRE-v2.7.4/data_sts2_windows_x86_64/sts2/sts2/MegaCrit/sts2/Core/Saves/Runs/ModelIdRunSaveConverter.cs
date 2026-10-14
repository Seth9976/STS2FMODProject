using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000BF RID: 191
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	public class ModelIdRunSaveConverter : JsonConverter<ModelId>
	{
		// Token: 0x0600092C RID: 2348 RVA: 0x0004286C File Offset: 0x00040A6C
		public override ModelId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			string @string = reader.GetString();
			if (@string != null)
			{
				return ModelId.Deserialize(@string);
			}
			return ModelId.none;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0004288F File Offset: 0x00040A8F
		public override void Write(Utf8JsonWriter writer, ModelId value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}
