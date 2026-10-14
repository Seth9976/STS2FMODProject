using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Utility for serializing and deserializing save data.
	/// </summary>
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	public static class JsonSerializationUtility
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001808E File Offset: 0x0001628E
		public static IJsonTypeInfoResolver DefaultResolver { get; } = MegaCritSerializerContext.Default;

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00018095 File Offset: 0x00016295
		public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions(MegaCritSerializerContext.DefaultGeneratedSerializerOptions)
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			TypeInfoResolver = MegaCritSerializerContext.Default.WithAddedModifier(new Action<JsonTypeInfo>(JsonSerializationUtility.AlphabetizeProperties)).WithAddedModifier(new Action<JsonTypeInfo>(JsonSerializeConditionAttribute.CheckJsonSerializeConditionsModifier))
		};

		/// <summary>
		/// Adds a type info resolver to the resolving chain. Only used in tests to include test objects in the JSON
		/// serialization.
		/// </summary>
		// Token: 0x0600053E RID: 1342 RVA: 0x0001809C File Offset: 0x0001629C
		public static void AddTypeInfoResolver(IJsonTypeInfoResolver resolver)
		{
			JsonSerializationUtility.Options.TypeInfoResolverChain.Add(resolver);
		}

		/// <summary>
		/// Serializes a save schema to JSON asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of save schema</typeparam>
		/// <param name="data">The data to serialize</param>
		/// <returns>The JSON string</returns>
		// Token: 0x0600053F RID: 1343 RVA: 0x000180B0 File Offset: 0x000162B0
		public static async Task<string> SerializeAsync<[Nullable(0)] T>(T data) where T : ISaveSchema
		{
			string text;
			using (MemoryStream stream = new MemoryStream())
			{
				await JsonSerializer.SerializeAsync<T>(stream, data, JsonSerializationUtility.GetTypeInfo<T>(), default(CancellationToken));
				stream.Position = 0L;
				using (StreamReader reader = new StreamReader(stream))
				{
					text = await reader.ReadToEndAsync();
				}
			}
			return text;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000180F3 File Offset: 0x000162F3
		public static JsonTypeInfo<T> GetTypeInfo<[Nullable(2)] T>(T value)
		{
			return (JsonTypeInfo<T>)JsonSerializationUtility.Options.GetTypeInfo(typeof(T));
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001810E File Offset: 0x0001630E
		public static JsonTypeInfo<T> GetTypeInfo<[Nullable(2)] T>()
		{
			return (JsonTypeInfo<T>)JsonSerializationUtility.Options.GetTypeInfo(typeof(T));
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001812C File Offset: 0x0001632C
		public static void AlphabetizeProperties(JsonTypeInfo info)
		{
			if (info.Kind != JsonTypeInfoKind.Object)
			{
				return;
			}
			List<JsonPropertyInfo> list = new List<JsonPropertyInfo>();
			list.AddRange(info.Properties);
			list.Sort((JsonPropertyInfo p1, JsonPropertyInfo p2) => string.CompareOrdinal(p1.Name, p2.Name));
			info.Properties.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Order = i;
				info.Properties.Add(list[i]);
			}
		}

		/// <summary>
		/// Serializes a save schema to JSON.
		/// </summary>
		/// <typeparam name="T">The type of save schema</typeparam>
		/// <param name="obj">The object to serialize</param>
		/// <returns>The JSON string</returns>
		// Token: 0x06000543 RID: 1347 RVA: 0x000181B5 File Offset: 0x000163B5
		public static string ToJson<[Nullable(0)] T>(T obj) where T : ISaveSchema
		{
			return JsonSerializer.Serialize<T>(obj, JsonSerializationUtility.GetTypeInfo<T>());
		}

		/// <summary>
		/// Deserializes JSON to a save schema object.
		/// </summary>
		/// <typeparam name="T">The type of save schema</typeparam>
		/// <param name="json">The JSON string</param>
		/// <returns>The deserialization result</returns>
		// Token: 0x06000544 RID: 1348 RVA: 0x000181C4 File Offset: 0x000163C4
		public static ReadSaveResult<T> FromJson<[Nullable(0)] T>(string json) where T : ISaveSchema, new()
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The json for type=");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(" was empty!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return new ReadSaveResult<T>(ReadSaveStatus.FileEmpty, null);
			}
			ReadSaveResult<T> readSaveResult;
			try
			{
				T t = JsonSerializer.Deserialize<T>(json, JsonSerializationUtility.GetTypeInfo<T>());
				if (t == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Json parsed as null! type=");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					readSaveResult = new ReadSaveResult<T>(ReadSaveStatus.JsonParseError, null);
				}
				else
				{
					readSaveResult = new ReadSaveResult<T>(t);
				}
			}
			catch (JsonException ex)
			{
				string text = ex.Path ?? "unknown";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 5);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to deserialize type=");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(" at path=");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(", line=");
				defaultInterpolatedStringHandler.AppendFormatted<long?>(ex.LineNumber);
				defaultInterpolatedStringHandler.AppendLiteral(", position=");
				defaultInterpolatedStringHandler.AppendFormatted<long?>(ex.BytePositionInLine);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				ReadSaveStatus readSaveStatus = ReadSaveStatus.JsonParseError;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
				defaultInterpolatedStringHandler.AppendLiteral("JSON error at ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" (line ");
				defaultInterpolatedStringHandler.AppendFormatted<long?>(ex.LineNumber);
				defaultInterpolatedStringHandler.AppendLiteral("): ");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				readSaveResult = new ReadSaveResult<T>(readSaveStatus, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return readSaveResult;
		}
	}
}
