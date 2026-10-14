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
	// Token: 0x02000BBD RID: 3005
	[NullableContext(1)]
	[Nullable(0)]
	[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, UseStringEnumConverter = true)]
	[JsonSerializable(typeof(Dictionary<string, SerializableDynamicVar>))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class SerializableDynamicVarDictionarySerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700229F RID: 8863
		// (get) Token: 0x06007D4E RID: 32078 RVA: 0x002AACAC File Offset: 0x002A8EAC
		[Nullable(0)]
		public JsonTypeInfo<bool> Boolean
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<bool> jsonTypeInfo;
				if ((jsonTypeInfo = this._Boolean) == null)
				{
					jsonTypeInfo = (this._Boolean = (JsonTypeInfo<bool>)base.Options.GetTypeInfo(typeof(bool)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x002AACE8 File Offset: 0x002A8EE8
		private JsonTypeInfo<bool> Create_Boolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<bool> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, JsonMetadataServices.BooleanConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170022A0 RID: 8864
		// (get) Token: 0x06007D50 RID: 32080 RVA: 0x002AAD14 File Offset: 0x002A8F14
		[Nullable(0)]
		public JsonTypeInfo<decimal> Decimal
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<decimal> jsonTypeInfo;
				if ((jsonTypeInfo = this._Decimal) == null)
				{
					jsonTypeInfo = (this._Decimal = (JsonTypeInfo<decimal>)base.Options.GetTypeInfo(typeof(decimal)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x002AAD50 File Offset: 0x002A8F50
		private JsonTypeInfo<decimal> Create_Decimal(JsonSerializerOptions options)
		{
			JsonTypeInfo<decimal> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<decimal>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<decimal>(options, JsonMetadataServices.DecimalConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170022A1 RID: 8865
		// (get) Token: 0x06007D52 RID: 32082 RVA: 0x002AAD7C File Offset: 0x002A8F7C
		[Nullable(0)]
		public JsonTypeInfo<DynamicVarType> DynamicVarType
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<DynamicVarType> jsonTypeInfo;
				if ((jsonTypeInfo = this._DynamicVarType) == null)
				{
					jsonTypeInfo = (this._DynamicVarType = (JsonTypeInfo<DynamicVarType>)base.Options.GetTypeInfo(typeof(DynamicVarType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x002AADB8 File Offset: 0x002A8FB8
		private JsonTypeInfo<DynamicVarType> Create_DynamicVarType(JsonSerializerOptions options)
		{
			JsonTypeInfo<DynamicVarType> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<DynamicVarType>(options, out jsonTypeInfo))
			{
				JsonConverter jsonConverter = SerializableDynamicVarDictionarySerializerContext.ExpandConverter(typeof(DynamicVarType), new JsonStringEnumConverter<DynamicVarType>(), options, true);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<DynamicVarType>(options, jsonConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170022A2 RID: 8866
		// (get) Token: 0x06007D54 RID: 32084 RVA: 0x002AADF8 File Offset: 0x002A8FF8
		[Nullable(0)]
		public JsonTypeInfo<SerializableDynamicVar> SerializableDynamicVar
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<SerializableDynamicVar> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableDynamicVar) == null)
				{
					jsonTypeInfo = (this._SerializableDynamicVar = (JsonTypeInfo<SerializableDynamicVar>)base.Options.GetTypeInfo(typeof(SerializableDynamicVar)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x002AAE34 File Offset: 0x002A9034
		private JsonTypeInfo<SerializableDynamicVar> Create_SerializableDynamicVar(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableDynamicVar> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableDynamicVar>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableDynamicVar> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableDynamicVar>();
				jsonObjectInfoValues.ObjectCreator = () => default(SerializableDynamicVar);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => SerializableDynamicVarDictionarySerializerContext.SerializableDynamicVarPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableDynamicVar).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, SerializableDynamicVar>(this.SerializableDynamicVarSerializeHandler);
				JsonObjectInfoValues<SerializableDynamicVar> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableDynamicVar>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x002AAF08 File Offset: 0x002A9108
		private static JsonPropertyInfo[] SerializableDynamicVarPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<DynamicVarType> jsonPropertyInfoValues = new JsonPropertyInfoValues<DynamicVarType>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableDynamicVar);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableDynamicVar)obj).type;
			jsonPropertyInfoValues.Setter = delegate(object obj, DynamicVarType value)
			{
				Unsafe.Unbox<SerializableDynamicVar>(obj).type = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "type";
			jsonPropertyInfoValues.JsonPropertyName = "type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableDynamicVar).GetField("type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<DynamicVarType> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<DynamicVarType>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<decimal> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<decimal>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableDynamicVar);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableDynamicVar)obj).decimalValue;
			jsonPropertyInfoValues3.Setter = delegate(object obj, decimal value)
			{
				Unsafe.Unbox<SerializableDynamicVar>(obj).decimalValue = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "decimalValue";
			jsonPropertyInfoValues3.JsonPropertyName = "decimal_value";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableDynamicVar).GetField("decimalValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<decimal> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<decimal>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = false;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableDynamicVar);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableDynamicVar)obj).boolValue;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SerializableDynamicVar>(obj).boolValue = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "boolValue";
			jsonPropertyInfoValues5.JsonPropertyName = "bool_value";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableDynamicVar).GetField("boolValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableDynamicVar);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableDynamicVar)obj).stringValue;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SerializableDynamicVar>(obj).stringValue = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "stringValue";
			jsonPropertyInfoValues7.JsonPropertyName = "string_value";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableDynamicVar).GetField("stringValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues8);
			return array;
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x002AB324 File Offset: 0x002A9524
		private void SerializableDynamicVarSerializeHandler(Utf8JsonWriter writer, SerializableDynamicVar value)
		{
			writer.WriteStartObject();
			writer.WritePropertyName(SerializableDynamicVarDictionarySerializerContext.PropName_type);
			JsonSerializer.Serialize<DynamicVarType>(writer, value.type, this.DynamicVarType);
			writer.WriteNumber(SerializableDynamicVarDictionarySerializerContext.PropName_decimal_value, value.decimalValue);
			writer.WriteBoolean(SerializableDynamicVarDictionarySerializerContext.PropName_bool_value, value.boolValue);
			writer.WriteString(SerializableDynamicVarDictionarySerializerContext.PropName_string_value, value.stringValue);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170022A3 RID: 8867
		// (get) Token: 0x06007D58 RID: 32088 RVA: 0x002AB390 File Offset: 0x002A9590
		[Nullable(0)]
		public JsonTypeInfo<Dictionary<string, SerializableDynamicVar>> DictionaryStringSerializableDynamicVar
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<Dictionary<string, SerializableDynamicVar>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryStringSerializableDynamicVar) == null)
				{
					jsonTypeInfo = (this._DictionaryStringSerializableDynamicVar = (JsonTypeInfo<Dictionary<string, SerializableDynamicVar>>)base.Options.GetTypeInfo(typeof(Dictionary<string, SerializableDynamicVar>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x002AB3CC File Offset: 0x002A95CC
		private JsonTypeInfo<Dictionary<string, SerializableDynamicVar>> Create_DictionaryStringSerializableDynamicVar(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<string, SerializableDynamicVar>> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, SerializableDynamicVar>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<string, SerializableDynamicVar>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, SerializableDynamicVar>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<string, SerializableDynamicVar>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, SerializableDynamicVar>>(this.DictionaryStringSerializableDynamicVarSerializeHandler);
				JsonCollectionInfoValues<Dictionary<string, SerializableDynamicVar>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, SerializableDynamicVar>, string, SerializableDynamicVar>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06007D5A RID: 32090 RVA: 0x002AB440 File Offset: 0x002A9640
		private void DictionaryStringSerializableDynamicVarSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] Dictionary<string, SerializableDynamicVar> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			foreach (KeyValuePair<string, SerializableDynamicVar> keyValuePair in value)
			{
				writer.WritePropertyName(keyValuePair.Key);
				this.SerializableDynamicVarSerializeHandler(writer, keyValuePair.Value);
			}
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170022A4 RID: 8868
		// (get) Token: 0x06007D5B RID: 32091 RVA: 0x002AB4B8 File Offset: 0x002A96B8
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

		// Token: 0x06007D5C RID: 32092 RVA: 0x002AB4F4 File Offset: 0x002A96F4
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x170022A5 RID: 8869
		// (get) Token: 0x06007D5D RID: 32093 RVA: 0x002AB51F File Offset: 0x002A971F
		public static SerializableDynamicVarDictionarySerializerContext Default { get; } = new SerializableDynamicVarDictionarySerializerContext(new JsonSerializerOptions(SerializableDynamicVarDictionarySerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x170022A6 RID: 8870
		// (get) Token: 0x06007D5E RID: 32094 RVA: 0x002AB526 File Offset: 0x002A9726
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = SerializableDynamicVarDictionarySerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x06007D5F RID: 32095 RVA: 0x002AB52E File Offset: 0x002A972E
		public SerializableDynamicVarDictionarySerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x06007D60 RID: 32096 RVA: 0x002AB542 File Offset: 0x002A9742
		public SerializableDynamicVarDictionarySerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x002AB558 File Offset: 0x002A9758
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = SerializableDynamicVarDictionarySerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x002AB588 File Offset: 0x002A9788
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return SerializableDynamicVarDictionarySerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x002AB5D0 File Offset: 0x002A97D0
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
		// Token: 0x06007D64 RID: 32100 RVA: 0x002AB638 File Offset: 0x002A9838
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x002AB658 File Offset: 0x002A9858
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(bool))
			{
				return this.Create_Boolean(options);
			}
			if (type == typeof(decimal))
			{
				return this.Create_Decimal(options);
			}
			if (type == typeof(DynamicVarType))
			{
				return this.Create_DynamicVarType(options);
			}
			if (type == typeof(SerializableDynamicVar))
			{
				return this.Create_SerializableDynamicVar(options);
			}
			if (type == typeof(Dictionary<string, SerializableDynamicVar>))
			{
				return this.Create_DictionaryStringSerializableDynamicVar(options);
			}
			if (type == typeof(string))
			{
				return this.Create_String(options);
			}
			return null;
		}

		// Token: 0x040029DE RID: 10718
		[Nullable(2)]
		private JsonTypeInfo<bool> _Boolean;

		// Token: 0x040029DF RID: 10719
		[Nullable(2)]
		private JsonTypeInfo<decimal> _Decimal;

		// Token: 0x040029E0 RID: 10720
		[Nullable(2)]
		private JsonTypeInfo<DynamicVarType> _DynamicVarType;

		// Token: 0x040029E1 RID: 10721
		[Nullable(2)]
		private JsonTypeInfo<SerializableDynamicVar> _SerializableDynamicVar;

		// Token: 0x040029E2 RID: 10722
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<Dictionary<string, SerializableDynamicVar>> _DictionaryStringSerializableDynamicVar;

		// Token: 0x040029E3 RID: 10723
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x040029E4 RID: 10724
		private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions
		{
			IncludeFields = true,
			WriteIndented = true
		};

		// Token: 0x040029E5 RID: 10725
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x040029E8 RID: 10728
		private static readonly JsonEncodedText PropName_type = JsonEncodedText.Encode("type", null);

		// Token: 0x040029E9 RID: 10729
		private static readonly JsonEncodedText PropName_decimal_value = JsonEncodedText.Encode("decimal_value", null);

		// Token: 0x040029EA RID: 10730
		private static readonly JsonEncodedText PropName_bool_value = JsonEncodedText.Encode("bool_value", null);

		// Token: 0x040029EB RID: 10731
		private static readonly JsonEncodedText PropName_string_value = JsonEncodedText.Encode("string_value", null);
	}
}
