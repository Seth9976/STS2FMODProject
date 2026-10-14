using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	/// <summary>
	/// JSON converter for List&lt;string&gt; of epoch IDs that provides backward compatibility
	/// with the old List&lt;ModelId&gt; format ("epoch.xxx") while writing the new format ("xxx").
	/// </summary>
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1 })]
	public class EpochIdListConverter : JsonConverter<List<string>>
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x000426B0 File Offset: 0x000408B0
		public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<string> list = new List<string>();
			if (reader.TokenType != JsonTokenType.StartArray)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected start of array for epoch ID list, but got ");
				defaultInterpolatedStringHandler.AppendFormatted<JsonTokenType>(reader.TokenType);
				throw new JsonException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
			{
				string text = reader.GetString();
				if (text != null)
				{
					int num = text.IndexOf('.');
					if (num >= 0)
					{
						string text2 = text;
						int num2 = num + 1;
						text = text2.Substring(num2, text2.Length - num2);
					}
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00042748 File Offset: 0x00040948
		public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (string text in value)
			{
				writer.WriteStringValue(text);
			}
			writer.WriteEndArray();
		}
	}
}
