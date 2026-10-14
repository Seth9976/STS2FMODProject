using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Localization
{
	/// <summary>
	/// Serializes the variables of a LocString to JSON.
	/// Not all DynamicVar types are supported, so use with caution.
	/// </summary>
	// Token: 0x02000BBE RID: 3006
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1, 1 })]
	public class LocStringVariablesJsonConverter : JsonConverter<Dictionary<string, object>>
	{
		// Token: 0x06007D67 RID: 32103 RVA: 0x002AB780 File Offset: 0x002A9980
		public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			Dictionary<string, SerializableDynamicVar> dictionary = JsonSerializer.Deserialize<Dictionary<string, SerializableDynamicVar>>(ref reader, SerializableDynamicVarDictionarySerializerContext.Default.DictionaryStringSerializableDynamicVar);
			if (dictionary == null)
			{
				throw new InvalidOperationException("Could not read LocString variables");
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (KeyValuePair<string, SerializableDynamicVar> keyValuePair in dictionary)
			{
				dictionary2[keyValuePair.Key] = keyValuePair.Value.ToDynamicVar(keyValuePair.Key);
			}
			return dictionary2;
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x002AB814 File Offset: 0x002A9A14
		public override void Write(Utf8JsonWriter writer, Dictionary<string, object> varDict, JsonSerializerOptions options)
		{
			Dictionary<string, SerializableDynamicVar> dictionary = new Dictionary<string, SerializableDynamicVar>();
			foreach (KeyValuePair<string, object> keyValuePair in varDict)
			{
				SerializableDynamicVar? serializableDynamicVar = SerializableDynamicVar.FromDynamicVar(keyValuePair.Value);
				if (serializableDynamicVar != null && (serializableDynamicVar.Value.type != DynamicVarType.BaseDynamic || !(keyValuePair.Value.GetType() != typeof(DynamicVar))))
				{
					dictionary[keyValuePair.Key] = serializableDynamicVar.Value;
				}
			}
			JsonSerializer.Serialize<Dictionary<string, SerializableDynamicVar>>(writer, dictionary, SerializableDynamicVarDictionarySerializerContext.Default.DictionaryStringSerializableDynamicVar);
		}
	}
}
