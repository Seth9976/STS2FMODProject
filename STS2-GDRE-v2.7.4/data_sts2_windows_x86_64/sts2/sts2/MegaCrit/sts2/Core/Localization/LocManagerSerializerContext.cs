using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BBA RID: 3002
	[NullableContext(1)]
	[Nullable(0)]
	[JsonSourceGenerationOptions(ReadCommentHandling = JsonCommentHandling.Skip)]
	[JsonSerializable(typeof(Dictionary<string, string>))]
	[JsonSerializable(typeof(Dictionary<string, int>))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class LocManagerSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700228C RID: 8844
		// (get) Token: 0x06007CF9 RID: 31993 RVA: 0x002A9118 File Offset: 0x002A7318
		[Nullable(0)]
		public JsonTypeInfo<Dictionary<string, int>> DictionaryStringInt32
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<Dictionary<string, int>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryStringInt32) == null)
				{
					jsonTypeInfo = (this._DictionaryStringInt32 = (JsonTypeInfo<Dictionary<string, int>>)base.Options.GetTypeInfo(typeof(Dictionary<string, int>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007CFA RID: 31994 RVA: 0x002A9154 File Offset: 0x002A7354
		private JsonTypeInfo<Dictionary<string, int>> Create_DictionaryStringInt32(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<string, int>> jsonTypeInfo;
			if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, int>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<string, int>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, int>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<string, int>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, int>>(this.DictionaryStringInt32SerializeHandler);
				JsonCollectionInfoValues<Dictionary<string, int>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, int>, string, int>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06007CFB RID: 31995 RVA: 0x002A91C8 File Offset: 0x002A73C8
		private void DictionaryStringInt32SerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] Dictionary<string, int> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			foreach (KeyValuePair<string, int> keyValuePair in value)
			{
				writer.WriteNumber(keyValuePair.Key, keyValuePair.Value);
			}
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700228D RID: 8845
		// (get) Token: 0x06007CFC RID: 31996 RVA: 0x002A923C File Offset: 0x002A743C
		[Nullable(0)]
		public JsonTypeInfo<Dictionary<string, string>> DictionaryStringString
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<Dictionary<string, string>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryStringString) == null)
				{
					jsonTypeInfo = (this._DictionaryStringString = (JsonTypeInfo<Dictionary<string, string>>)base.Options.GetTypeInfo(typeof(Dictionary<string, string>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x002A9278 File Offset: 0x002A7478
		private JsonTypeInfo<Dictionary<string, string>> Create_DictionaryStringString(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<string, string>> jsonTypeInfo;
			if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, string>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<string, string>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, string>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<string, string>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, string>>(this.DictionaryStringStringSerializeHandler);
				JsonCollectionInfoValues<Dictionary<string, string>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, string>, string, string>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x002A92EC File Offset: 0x002A74EC
		private void DictionaryStringStringSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<string, string> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			foreach (KeyValuePair<string, string> keyValuePair in value)
			{
				writer.WriteString(keyValuePair.Key, keyValuePair.Value);
			}
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700228E RID: 8846
		// (get) Token: 0x06007CFF RID: 31999 RVA: 0x002A9360 File Offset: 0x002A7560
		[Nullable(0)]
		public JsonTypeInfo<int> Int32
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<int> jsonTypeInfo;
				if ((jsonTypeInfo = this._Int32) == null)
				{
					jsonTypeInfo = (this._Int32 = (JsonTypeInfo<int>)base.Options.GetTypeInfo(typeof(int)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D00 RID: 32000 RVA: 0x002A939C File Offset: 0x002A759C
		private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int> jsonTypeInfo;
			if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700228F RID: 8847
		// (get) Token: 0x06007D01 RID: 32001 RVA: 0x002A93C8 File Offset: 0x002A75C8
		[Nullable(0)]
		public JsonTypeInfo<string> String
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<string> jsonTypeInfo;
				if ((jsonTypeInfo = this._String) == null)
				{
					jsonTypeInfo = (this._String = (JsonTypeInfo<string>)base.Options.GetTypeInfo(typeof(string)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D02 RID: 32002 RVA: 0x002A9404 File Offset: 0x002A7604
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x17002290 RID: 8848
		// (get) Token: 0x06007D03 RID: 32003 RVA: 0x002A942F File Offset: 0x002A762F
		public static LocManagerSerializerContext Default { get; } = new LocManagerSerializerContext(new JsonSerializerOptions(LocManagerSerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x17002291 RID: 8849
		// (get) Token: 0x06007D04 RID: 32004 RVA: 0x002A9436 File Offset: 0x002A7636
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = LocManagerSerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x06007D05 RID: 32005 RVA: 0x002A943E File Offset: 0x002A763E
		public LocManagerSerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x06007D06 RID: 32006 RVA: 0x002A9452 File Offset: 0x002A7652
		public LocManagerSerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x06007D07 RID: 32007 RVA: 0x002A9468 File Offset: 0x002A7668
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = LocManagerSerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x06007D08 RID: 32008 RVA: 0x002A9498 File Offset: 0x002A7698
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return LocManagerSerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x002A94E0 File Offset: 0x002A76E0
		private static JsonConverter ExpandConverter(Type type, JsonConverter converter, JsonSerializerOptions options, bool validateCanConvert = true)
		{
			if (validateCanConvert && !converter.CanConvert(type))
			{
				throw new InvalidOperationException(string.Format("The converter '{0}' is not compatible with the type '{1}'.", converter.GetType(), type));
			}
			JsonConverterFactory jsonConverterFactory = converter as JsonConverterFactory;
			if (jsonConverterFactory != null)
			{
				converter = jsonConverterFactory.CreateConverter(type, options);
				if (converter == null || converter is JsonConverterFactory)
				{
					throw new InvalidOperationException(string.Format("The converter '{0}' cannot return null or a JsonConverterFactory instance.", jsonConverterFactory.GetType()));
				}
			}
			return converter;
		}

		/// <inheritdoc />
		// Token: 0x06007D0A RID: 32010 RVA: 0x002A9548 File Offset: 0x002A7748
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x002A9568 File Offset: 0x002A7768
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(Dictionary<string, int>))
			{
				return this.Create_DictionaryStringInt32(options);
			}
			if (type == typeof(Dictionary<string, string>))
			{
				return this.Create_DictionaryStringString(options);
			}
			if (type == typeof(int))
			{
				return this.Create_Int32(options);
			}
			if (type == typeof(string))
			{
				return this.Create_String(options);
			}
			return null;
		}

		// Token: 0x040029C1 RID: 10689
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<Dictionary<string, int>> _DictionaryStringInt32;

		// Token: 0x040029C2 RID: 10690
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<Dictionary<string, string>> _DictionaryStringString;

		// Token: 0x040029C3 RID: 10691
		[Nullable(2)]
		private JsonTypeInfo<int> _Int32;

		// Token: 0x040029C4 RID: 10692
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x040029C5 RID: 10693
		private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions
		{
			ReadCommentHandling = JsonCommentHandling.Skip
		};

		// Token: 0x040029C6 RID: 10694
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
