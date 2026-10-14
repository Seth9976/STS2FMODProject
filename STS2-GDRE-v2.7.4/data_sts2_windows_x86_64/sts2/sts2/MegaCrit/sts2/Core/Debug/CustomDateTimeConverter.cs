using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Custom JsonConverter for DateTime to handle the specific format in the JSON.
	/// </summary>
	// Token: 0x02000D13 RID: 3347
	[NullableContext(1)]
	[Nullable(0)]
	public class CustomDateTimeConverter : JsonConverter<DateTime>
	{
		// Token: 0x0600870C RID: 34572 RVA: 0x002D6390 File Offset: 0x002D4590
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			string text = reader.GetString() ?? string.Empty;
			return DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
		}

		// Token: 0x0600870D RID: 34573 RVA: 0x002D63BA File Offset: 0x002D45BA
		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture));
		}
	}
}
