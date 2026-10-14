using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D14 RID: 3348
	[NullableContext(1)]
	[Nullable(0)]
	[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true, Converters = new Type[] { typeof(CustomDateTimeConverter) })]
	[JsonSerializable(typeof(ReleaseInfo))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class ReleaseInfoJsonSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17002530 RID: 9520
		// (get) Token: 0x0600870F RID: 34575 RVA: 0x002D63DC File Offset: 0x002D45DC
		[Nullable(0)]
		public JsonTypeInfo<ReleaseInfo> ReleaseInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<ReleaseInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._ReleaseInfo) == null)
				{
					jsonTypeInfo = (this._ReleaseInfo = (JsonTypeInfo<ReleaseInfo>)base.Options.GetTypeInfo(typeof(ReleaseInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06008710 RID: 34576 RVA: 0x002D6418 File Offset: 0x002D4618
		private JsonTypeInfo<ReleaseInfo> Create_ReleaseInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<ReleaseInfo> jsonTypeInfo;
			if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ReleaseInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ReleaseInfo> jsonObjectInfoValues = new JsonObjectInfoValues<ReleaseInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new ReleaseInfo
				{
					Commit = (string)args[0],
					Version = (string)args[1],
					Date = (DateTime)args[2],
					Branch = (string)args[3],
					MainAssemblyHash = (int)args[4]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => ReleaseInfoJsonSerializerContext.ReleaseInfoPropInit(options);
				JsonObjectInfoValues<ReleaseInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = ReleaseInfoJsonSerializerContext.<>O.<0>__ReleaseInfoCtorParamInit) == null)
				{
					func = (ReleaseInfoJsonSerializerContext.<>O.<0>__ReleaseInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(ReleaseInfoJsonSerializerContext.ReleaseInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ReleaseInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ReleaseInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ReleaseInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06008711 RID: 34577 RVA: 0x002D64FC File Offset: 0x002D46FC
		private static JsonPropertyInfo[] ReleaseInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ReleaseInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ReleaseInfo)obj).Commit;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Commit";
			jsonPropertyInfoValues.JsonPropertyName = "commit";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ReleaseInfo).GetProperty("Commit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ReleaseInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ReleaseInfo)obj).Version;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Version";
			jsonPropertyInfoValues.JsonPropertyName = "version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ReleaseInfo).GetProperty("Version", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<DateTime> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<DateTime>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(ReleaseInfo);
			jsonPropertyInfoValues4.Converter = (JsonConverter<DateTime>)ReleaseInfoJsonSerializerContext.ExpandConverter(typeof(DateTime), new CustomDateTimeConverter(), options, true);
			jsonPropertyInfoValues4.Getter = (object obj) => ((ReleaseInfo)obj).Date;
			jsonPropertyInfoValues4.Setter = delegate(object obj, DateTime value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "Date";
			jsonPropertyInfoValues4.JsonPropertyName = "date";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(ReleaseInfo).GetProperty("Date", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(DateTime), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<DateTime> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<DateTime>(options, jsonPropertyInfoValues5);
			array[2].IsRequired = true;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ReleaseInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ReleaseInfo)obj).Branch;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Branch";
			jsonPropertyInfoValues.JsonPropertyName = "branch";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ReleaseInfo).GetProperty("Branch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(ReleaseInfo);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((ReleaseInfo)obj).MainAssemblyHash;
			jsonPropertyInfoValues7.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "MainAssemblyHash";
			jsonPropertyInfoValues7.JsonPropertyName = "main_assembly_hash";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(ReleaseInfo).GetProperty("MainAssemblyHash", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			array[4].IsRequired = true;
			return array;
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x002D6A98 File Offset: 0x002D4C98
		private static JsonParameterInfoValues[] ReleaseInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Commit",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Version",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Date",
					ParameterType = typeof(DateTime),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Branch",
					ParameterType = typeof(string),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "MainAssemblyHash",
					ParameterType = typeof(int),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17002531 RID: 9521
		// (get) Token: 0x06008713 RID: 34579 RVA: 0x002D6BD0 File Offset: 0x002D4DD0
		[Nullable(0)]
		public JsonTypeInfo<DateTime> DateTime
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<DateTime> jsonTypeInfo;
				if ((jsonTypeInfo = this._DateTime) == null)
				{
					jsonTypeInfo = (this._DateTime = (JsonTypeInfo<DateTime>)base.Options.GetTypeInfo(typeof(DateTime)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06008714 RID: 34580 RVA: 0x002D6C0C File Offset: 0x002D4E0C
		private JsonTypeInfo<DateTime> Create_DateTime(JsonSerializerOptions options)
		{
			JsonTypeInfo<DateTime> jsonTypeInfo;
			if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<DateTime>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<DateTime>(options, JsonMetadataServices.DateTimeConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17002532 RID: 9522
		// (get) Token: 0x06008715 RID: 34581 RVA: 0x002D6C38 File Offset: 0x002D4E38
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

		// Token: 0x06008716 RID: 34582 RVA: 0x002D6C74 File Offset: 0x002D4E74
		private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int> jsonTypeInfo;
			if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17002533 RID: 9523
		// (get) Token: 0x06008717 RID: 34583 RVA: 0x002D6CA0 File Offset: 0x002D4EA0
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

		// Token: 0x06008718 RID: 34584 RVA: 0x002D6CDC File Offset: 0x002D4EDC
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x17002534 RID: 9524
		// (get) Token: 0x06008719 RID: 34585 RVA: 0x002D6D07 File Offset: 0x002D4F07
		public static ReleaseInfoJsonSerializerContext Default { get; } = new ReleaseInfoJsonSerializerContext(new JsonSerializerOptions(ReleaseInfoJsonSerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x17002535 RID: 9525
		// (get) Token: 0x0600871A RID: 34586 RVA: 0x002D6D0E File Offset: 0x002D4F0E
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = ReleaseInfoJsonSerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x0600871B RID: 34587 RVA: 0x002D6D16 File Offset: 0x002D4F16
		public ReleaseInfoJsonSerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x0600871C RID: 34588 RVA: 0x002D6D2A File Offset: 0x002D4F2A
		public ReleaseInfoJsonSerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x0600871D RID: 34589 RVA: 0x002D6D40 File Offset: 0x002D4F40
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = ReleaseInfoJsonSerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x0600871E RID: 34590 RVA: 0x002D6D70 File Offset: 0x002D4F70
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return ReleaseInfoJsonSerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x0600871F RID: 34591 RVA: 0x002D6DB8 File Offset: 0x002D4FB8
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
		// Token: 0x06008720 RID: 34592 RVA: 0x002D6E20 File Offset: 0x002D5020
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x06008721 RID: 34593 RVA: 0x002D6E40 File Offset: 0x002D5040
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(ReleaseInfo))
			{
				return this.Create_ReleaseInfo(options);
			}
			if (type == typeof(DateTime))
			{
				return this.Create_DateTime(options);
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

		// Token: 0x04002ECB RID: 11979
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ReleaseInfo> _ReleaseInfo;

		// Token: 0x04002ECC RID: 11980
		[Nullable(2)]
		private JsonTypeInfo<DateTime> _DateTime;

		// Token: 0x04002ECD RID: 11981
		[Nullable(2)]
		private JsonTypeInfo<int> _Int32;

		// Token: 0x04002ECE RID: 11982
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x04002ECF RID: 11983
		private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions
		{
			Converters = 
			{
				new CustomDateTimeConverter()
			},
			WriteIndented = true
		};

		// Token: 0x04002ED0 RID: 11984
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x02002423 RID: 9251
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040098A0 RID: 39072
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <0>__ReleaseInfoCtorParamInit;
		}
	}
}
