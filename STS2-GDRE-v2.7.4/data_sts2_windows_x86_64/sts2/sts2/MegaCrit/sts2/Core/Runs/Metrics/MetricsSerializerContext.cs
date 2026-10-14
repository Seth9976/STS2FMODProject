using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.GameInfo;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000119 RID: 281
	[NullableContext(1)]
	[Nullable(0)]
	[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, UseStringEnumConverter = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, Converters = new Type[] { typeof(ModelIdMetricsConverter) })]
	[JsonSerializable(typeof(RunMetrics))]
	[JsonSerializable(typeof(AchievementMetric))]
	[JsonSerializable(typeof(EpochMetric))]
	[JsonSerializable(typeof(SettingsDataMetric))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class MetricsSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x000527C8 File Offset: 0x000509C8
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

		// Token: 0x06000D4A RID: 3402 RVA: 0x00052804 File Offset: 0x00050A04
		private JsonTypeInfo<bool> Create_Boolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<bool> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, JsonMetadataServices.BooleanConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00052830 File Offset: 0x00050A30
		[Nullable(0)]
		public JsonTypeInfo<float> Single
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<float> jsonTypeInfo;
				if ((jsonTypeInfo = this._Single) == null)
				{
					jsonTypeInfo = (this._Single = (JsonTypeInfo<float>)base.Options.GetTypeInfo(typeof(float)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0005286C File Offset: 0x00050A6C
		private JsonTypeInfo<float> Create_Single(JsonSerializerOptions options)
		{
			JsonTypeInfo<float> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<float>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<float>(options, JsonMetadataServices.SingleConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00052898 File Offset: 0x00050A98
		[Nullable(0)]
		public JsonTypeInfo<Vector2I> Vector2I
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<Vector2I> jsonTypeInfo;
				if ((jsonTypeInfo = this._Vector2I) == null)
				{
					jsonTypeInfo = (this._Vector2I = (JsonTypeInfo<Vector2I>)base.Options.GetTypeInfo(typeof(Vector2I)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000528D4 File Offset: 0x00050AD4
		private JsonTypeInfo<Vector2I> Create_Vector2I(JsonSerializerOptions options)
		{
			JsonTypeInfo<Vector2I> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Vector2I>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<Vector2I> jsonObjectInfoValues = new JsonObjectInfoValues<Vector2I>();
				jsonObjectInfoValues.ObjectCreator = () => default(Vector2I);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.Vector2IPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(Vector2I).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, Vector2I>(this.Vector2ISerializeHandler);
				JsonObjectInfoValues<Vector2I> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Vector2I>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000529A8 File Offset: 0x00050BA8
		private static JsonPropertyInfo[] Vector2IPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Vector2I);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Vector2I)obj).X;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<Vector2I>(obj).X = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "X";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Vector2I).GetField("X", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Vector2I);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Vector2I)obj).Y;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<Vector2I>(obj).Y = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Y";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Vector2I).GetField("Y", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues3);
			return array;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00052B97 File Offset: 0x00050D97
		private void Vector2ISerializeHandler(Utf8JsonWriter writer, Vector2I value)
		{
			writer.WriteStartObject();
			writer.WriteNumber(MetricsSerializerContext.PropName_x, value.X);
			writer.WriteNumber(MetricsSerializerContext.PropName_y, value.Y);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00052BC8 File Offset: 0x00050DC8
		[Nullable(0)]
		public JsonTypeInfo<AchievementMetric> AchievementMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<AchievementMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._AchievementMetric) == null)
				{
					jsonTypeInfo = (this._AchievementMetric = (JsonTypeInfo<AchievementMetric>)base.Options.GetTypeInfo(typeof(AchievementMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00052C04 File Offset: 0x00050E04
		private JsonTypeInfo<AchievementMetric> Create_AchievementMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<AchievementMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AchievementMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AchievementMetric> jsonObjectInfoValues = new JsonObjectInfoValues<AchievementMetric>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new AchievementMetric
				{
					BuildId = (string)args[0],
					Achievement = (string)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.AchievementMetricPropInit(options);
				JsonObjectInfoValues<AchievementMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MetricsSerializerContext.<>O.<0>__AchievementMetricCtorParamInit) == null)
				{
					func = (MetricsSerializerContext.<>O.<0>__AchievementMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.AchievementMetricCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AchievementMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, AchievementMetric>(this.AchievementMetricSerializeHandler);
				JsonObjectInfoValues<AchievementMetric> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AchievementMetric>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00052CF4 File Offset: 0x00050EF4
		private static JsonPropertyInfo[] AchievementMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AchievementMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AchievementMetric)obj).BuildId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((AchievementMetric)obj).BuildId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BuildId";
			jsonPropertyInfoValues.JsonPropertyName = "buildId";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AchievementMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AchievementMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AchievementMetric)obj).Achievement;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((AchievementMetric)obj).Achievement = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Achievement";
			jsonPropertyInfoValues.JsonPropertyName = "achievement";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AchievementMetric).GetProperty("Achievement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(AchievementMetric);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((AchievementMetric)obj).TotalAchievements;
			jsonPropertyInfoValues4.Setter = delegate(object obj, int value)
			{
				((AchievementMetric)obj).TotalAchievements = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "TotalAchievements";
			jsonPropertyInfoValues4.JsonPropertyName = "totalAchievements";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(AchievementMetric).GetProperty("TotalAchievements", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(AchievementMetric);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((AchievementMetric)obj).TotalPlaytime;
			jsonPropertyInfoValues6.Setter = delegate(object obj, long value)
			{
				((AchievementMetric)obj).TotalPlaytime = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "TotalPlaytime";
			jsonPropertyInfoValues6.JsonPropertyName = "totalPlaytime";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(AchievementMetric).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues4 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(AchievementMetric);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((AchievementMetric)obj).TotalRuns;
			jsonPropertyInfoValues4.Setter = delegate(object obj, int value)
			{
				((AchievementMetric)obj).TotalRuns = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "TotalRuns";
			jsonPropertyInfoValues4.JsonPropertyName = "totalRuns";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(AchievementMetric).GetProperty("TotalRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues4;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			return array;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00053248 File Offset: 0x00051448
		private void AchievementMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] AchievementMetric value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
			writer.WriteString(MetricsSerializerContext.PropName_achievement, value.Achievement);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalAchievements, value.TotalAchievements);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalRuns, value.TotalRuns);
			writer.WriteEndObject();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000532C0 File Offset: 0x000514C0
		private static JsonParameterInfoValues[] AchievementMetricCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "BuildId",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Achievement",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00053348 File Offset: 0x00051548
		[Nullable(0)]
		public JsonTypeInfo<ModelId> ModelId
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<ModelId> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModelId) == null)
				{
					jsonTypeInfo = (this._ModelId = (JsonTypeInfo<ModelId>)base.Options.GetTypeInfo(typeof(ModelId)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00053384 File Offset: 0x00051584
		private JsonTypeInfo<ModelId> Create_ModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModelId> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModelId>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues = new JsonObjectInfoValues<ModelId>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new ModelId((string)args[0], (string)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.ModelIdPropInit(options);
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MetricsSerializerContext.<>O.<1>__ModelIdCtorParamInit) == null)
				{
					func = (MetricsSerializerContext.<>O.<1>__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.ModelIdCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModelId).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(string)
				}, null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, ModelId>(this.ModelIdSerializeHandler);
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModelId>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00053474 File Offset: 0x00051674
		private static JsonPropertyInfo[] ModelIdPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModelId);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModelId)obj).Category;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Category";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModelId).GetProperty("Category", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModelId);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModelId)obj).Entry;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Entry";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModelId).GetProperty("Entry", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			return array;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00053639 File Offset: 0x00051839
		private void ModelIdSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] ModelId value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_category, value.Category);
			writer.WriteString(MetricsSerializerContext.PropName_entry, value.Entry);
			writer.WriteEndObject();
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00053674 File Offset: 0x00051874
		private static JsonParameterInfoValues[] ModelIdCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "category",
					ParameterType = typeof(string),
					Position = 0,
					HasDefaultValue = false,
					DefaultValue = null,
					IsNullable = false
				},
				new JsonParameterInfoValues
				{
					Name = "entry",
					ParameterType = typeof(string),
					Position = 1,
					HasDefaultValue = false,
					DefaultValue = null,
					IsNullable = false
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x0005370C File Offset: 0x0005190C
		[Nullable(0)]
		public JsonTypeInfo<ActWinMetric> ActWinMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<ActWinMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._ActWinMetric) == null)
				{
					jsonTypeInfo = (this._ActWinMetric = (JsonTypeInfo<ActWinMetric>)base.Options.GetTypeInfo(typeof(ActWinMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00053748 File Offset: 0x00051948
		private JsonTypeInfo<ActWinMetric> Create_ActWinMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<ActWinMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ActWinMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ActWinMetric> jsonObjectInfoValues = new JsonObjectInfoValues<ActWinMetric>();
				jsonObjectInfoValues.ObjectCreator = () => default(ActWinMetric);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.ActWinMetricPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ActWinMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, ActWinMetric>(this.ActWinMetricSerializeHandler);
				JsonObjectInfoValues<ActWinMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ActWinMetric>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0005381C File Offset: 0x00051A1C
		private static JsonPropertyInfo[] ActWinMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ActWinMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ActWinMetric)obj).act;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "act";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ActWinMetric).GetField("act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(ActWinMetric);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((ActWinMetric)obj).win;
			jsonPropertyInfoValues3.Setter = null;
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "win";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(ActWinMetric).GetField("win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues4);
			return array;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000539F1 File Offset: 0x00051BF1
		private void ActWinMetricSerializeHandler(Utf8JsonWriter writer, ActWinMetric value)
		{
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_act, value.act);
			writer.WriteBoolean(MetricsSerializerContext.PropName_win, value.win);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00053A24 File Offset: 0x00051C24
		[Nullable(0)]
		public JsonTypeInfo<AncientMetric> AncientMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<AncientMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._AncientMetric) == null)
				{
					jsonTypeInfo = (this._AncientMetric = (JsonTypeInfo<AncientMetric>)base.Options.GetTypeInfo(typeof(AncientMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00053A60 File Offset: 0x00051C60
		private JsonTypeInfo<AncientMetric> Create_AncientMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<AncientMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AncientMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AncientMetric> jsonObjectInfoValues = new JsonObjectInfoValues<AncientMetric>();
				jsonObjectInfoValues.ObjectCreator = () => default(AncientMetric);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.AncientMetricPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AncientMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, AncientMetric>(this.AncientMetricSerializeHandler);
				JsonObjectInfoValues<AncientMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AncientMetric>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00053B34 File Offset: 0x00051D34
		private static JsonPropertyInfo[] AncientMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientMetric)obj).picked;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "picked";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientMetric);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((AncientMetric)obj).skipped;
			jsonPropertyInfoValues3.Setter = null;
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "skipped";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientMetric).GetField("skipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00053D1B File Offset: 0x00051F1B
		private void AncientMetricSerializeHandler(Utf8JsonWriter writer, AncientMetric value)
		{
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_picked, value.picked);
			writer.WritePropertyName(MetricsSerializerContext.PropName_skipped);
			this.ListStringSerializeHandler(writer, value.skipped);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00053D54 File Offset: 0x00051F54
		[Nullable(0)]
		public JsonTypeInfo<CardChoiceMetric> CardChoiceMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<CardChoiceMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardChoiceMetric) == null)
				{
					jsonTypeInfo = (this._CardChoiceMetric = (JsonTypeInfo<CardChoiceMetric>)base.Options.GetTypeInfo(typeof(CardChoiceMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00053D90 File Offset: 0x00051F90
		private JsonTypeInfo<CardChoiceMetric> Create_CardChoiceMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardChoiceMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardChoiceMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardChoiceMetric> jsonObjectInfoValues = new JsonObjectInfoValues<CardChoiceMetric>();
				jsonObjectInfoValues.ObjectCreator = () => default(CardChoiceMetric);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.CardChoiceMetricPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardChoiceMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, CardChoiceMetric>(this.CardChoiceMetricSerializeHandler);
				JsonObjectInfoValues<CardChoiceMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardChoiceMetric>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00053E64 File Offset: 0x00052064
		private static JsonPropertyInfo[] CardChoiceMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardChoiceMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardChoiceMetric)obj).picked;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "picked";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardChoiceMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardChoiceMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardChoiceMetric)obj).skipped;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "skipped";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardChoiceMetric).GetField("skipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0005403B File Offset: 0x0005223B
		private void CardChoiceMetricSerializeHandler(Utf8JsonWriter writer, CardChoiceMetric value)
		{
			writer.WriteStartObject();
			writer.WritePropertyName(MetricsSerializerContext.PropName_picked);
			this.ListStringSerializeHandler(writer, value.picked);
			writer.WritePropertyName(MetricsSerializerContext.PropName_skipped);
			this.ListStringSerializeHandler(writer, value.skipped);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x0005407C File Offset: 0x0005227C
		[Nullable(0)]
		public JsonTypeInfo<EncounterMetric> EncounterMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EncounterMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._EncounterMetric) == null)
				{
					jsonTypeInfo = (this._EncounterMetric = (JsonTypeInfo<EncounterMetric>)base.Options.GetTypeInfo(typeof(EncounterMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x000540B8 File Offset: 0x000522B8
		private JsonTypeInfo<EncounterMetric> Create_EncounterMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<EncounterMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EncounterMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EncounterMetric> jsonObjectInfoValues = new JsonObjectInfoValues<EncounterMetric>();
				jsonObjectInfoValues.ObjectCreator = () => default(EncounterMetric);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.EncounterMetricPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EncounterMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EncounterMetric>(this.EncounterMetricSerializeHandler);
				JsonObjectInfoValues<EncounterMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EncounterMetric>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0005418C File Offset: 0x0005238C
		private static JsonPropertyInfo[] EncounterMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterMetric)obj).id;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "id";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterMetric).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(EncounterMetric);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((EncounterMetric)obj).damage;
			jsonPropertyInfoValues3.Setter = null;
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "damage";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(EncounterMetric).GetField("damage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(EncounterMetric);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((EncounterMetric)obj).turns;
			jsonPropertyInfoValues3.Setter = null;
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "turns";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(EncounterMetric).GetField("turns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			return array;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00054450 File Offset: 0x00052650
		private void EncounterMetricSerializeHandler(Utf8JsonWriter writer, EncounterMetric value)
		{
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_id, value.id);
			writer.WriteNumber(MetricsSerializerContext.PropName_damage, value.damage);
			writer.WriteNumber(MetricsSerializerContext.PropName_turns, value.turns);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0005449C File Offset: 0x0005269C
		[Nullable(0)]
		public JsonTypeInfo<EventChoiceMetric> EventChoiceMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EventChoiceMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._EventChoiceMetric) == null)
				{
					jsonTypeInfo = (this._EventChoiceMetric = (JsonTypeInfo<EventChoiceMetric>)base.Options.GetTypeInfo(typeof(EventChoiceMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x000544D8 File Offset: 0x000526D8
		private JsonTypeInfo<EventChoiceMetric> Create_EventChoiceMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<EventChoiceMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EventChoiceMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EventChoiceMetric> jsonObjectInfoValues = new JsonObjectInfoValues<EventChoiceMetric>();
				jsonObjectInfoValues.ObjectCreator = () => default(EventChoiceMetric);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.EventChoiceMetricPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EventChoiceMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EventChoiceMetric>(this.EventChoiceMetricSerializeHandler);
				JsonObjectInfoValues<EventChoiceMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EventChoiceMetric>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000545AC File Offset: 0x000527AC
		private static JsonPropertyInfo[] EventChoiceMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventChoiceMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventChoiceMetric)obj).id;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "id";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventChoiceMetric).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventChoiceMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventChoiceMetric)obj).act;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "act";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventChoiceMetric).GetField("act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventChoiceMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventChoiceMetric)obj).picked;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "picked";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventChoiceMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00054894 File Offset: 0x00052A94
		private void EventChoiceMetricSerializeHandler(Utf8JsonWriter writer, EventChoiceMetric value)
		{
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_id, value.id);
			writer.WriteString(MetricsSerializerContext.PropName_act, value.act);
			writer.WriteString(MetricsSerializerContext.PropName_picked, value.picked);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000548E0 File Offset: 0x00052AE0
		[Nullable(0)]
		public JsonTypeInfo<RunMetrics> RunMetrics
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<RunMetrics> jsonTypeInfo;
				if ((jsonTypeInfo = this._RunMetrics) == null)
				{
					jsonTypeInfo = (this._RunMetrics = (JsonTypeInfo<RunMetrics>)base.Options.GetTypeInfo(typeof(RunMetrics)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0005491C File Offset: 0x00052B1C
		private JsonTypeInfo<RunMetrics> Create_RunMetrics(JsonSerializerOptions options)
		{
			JsonTypeInfo<RunMetrics> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RunMetrics>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<RunMetrics> jsonObjectInfoValues = new JsonObjectInfoValues<RunMetrics>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new RunMetrics
				{
					BuildId = (string)args[0],
					PlayerId = (string)args[1],
					Character = (ModelId)args[2],
					Win = (bool)args[3],
					NumPlayers = (int)args[4],
					Team = (List<ModelId>)args[5],
					BuildType = (string)args[6],
					Ascension = (int)args[7],
					TotalPlaytime = (float)args[8],
					TotalWinRate = (float)args[9],
					NumReloads = (int)args[10],
					RunPlaytime = (float)args[11],
					FloorReached = (int)args[12],
					KilledByEncounter = (ModelId)args[13],
					CardChoices = (List<CardChoiceMetric>)args[14],
					CampfireUpgrades = (List<string>)args[15],
					EventChoices = (List<EventChoiceMetric>)args[16],
					AncientChoices = (List<AncientMetric>)args[17],
					RelicBuys = (List<string>)args[18],
					PotionBuys = (List<string>)args[19],
					ColorlessBuys = (List<string>)args[20],
					PotionDiscards = (List<string>)args[21],
					Encounters = (List<EncounterMetric>)args[22],
					ActWins = (List<ActWinMetric>)args[23],
					Deck = (IEnumerable<ModelId>)args[24],
					Relics = (IEnumerable<ModelId>)args[25]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.RunMetricsPropInit(options);
				JsonObjectInfoValues<RunMetrics> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MetricsSerializerContext.<>O.<2>__RunMetricsCtorParamInit) == null)
				{
					func = (MetricsSerializerContext.<>O.<2>__RunMetricsCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.RunMetricsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(RunMetrics).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, RunMetrics>(this.RunMetricsSerializeHandler);
				JsonObjectInfoValues<RunMetrics> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<RunMetrics>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00054A0C File Offset: 0x00052C0C
		private static JsonPropertyInfo[] RunMetricsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[26];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunMetrics)obj).BuildId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BuildId";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunMetrics)obj).PlayerId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "PlayerId";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("PlayerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((RunMetrics)obj).Character;
			jsonPropertyInfoValues4.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "Character";
			jsonPropertyInfoValues4.JsonPropertyName = null;
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues5);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((RunMetrics)obj).Win;
			jsonPropertyInfoValues6.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "Win";
			jsonPropertyInfoValues6.JsonPropertyName = null;
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues7);
			array[3].IsRequired = true;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((RunMetrics)obj).NumPlayers;
			jsonPropertyInfoValues8.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "NumPlayers";
			jsonPropertyInfoValues8.JsonPropertyName = null;
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("NumPlayers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = jsonPropertyInfoValues8;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues9);
			array[4].IsRequired = true;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((RunMetrics)obj).Team;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "Team";
			jsonPropertyInfoValues10.JsonPropertyName = null;
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Team", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[5] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues11);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunMetrics)obj).BuildType;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BuildType";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("BuildType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues12 = jsonPropertyInfoValues;
			array[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues12);
			array[6].IsRequired = true;
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((RunMetrics)obj).Ascension;
			jsonPropertyInfoValues8.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "Ascension";
			jsonPropertyInfoValues8.JsonPropertyName = null;
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues13 = jsonPropertyInfoValues8;
			array[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues13);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues14 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((RunMetrics)obj).TotalPlaytime;
			jsonPropertyInfoValues14.Setter = delegate(object obj, float value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "TotalPlaytime";
			jsonPropertyInfoValues14.JsonPropertyName = null;
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues15 = jsonPropertyInfoValues14;
			array[8] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues15);
			array[8].IsRequired = true;
			jsonPropertyInfoValues14 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((RunMetrics)obj).TotalWinRate;
			jsonPropertyInfoValues14.Setter = delegate(object obj, float value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "TotalWinRate";
			jsonPropertyInfoValues14.JsonPropertyName = null;
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("TotalWinRate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues16 = jsonPropertyInfoValues14;
			array[9] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues16);
			array[9].IsRequired = true;
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((RunMetrics)obj).NumReloads;
			jsonPropertyInfoValues8.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "NumReloads";
			jsonPropertyInfoValues8.JsonPropertyName = null;
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("NumReloads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues17 = jsonPropertyInfoValues8;
			array[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues17);
			array[10].IsRequired = true;
			jsonPropertyInfoValues14 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((RunMetrics)obj).RunPlaytime;
			jsonPropertyInfoValues14.Setter = delegate(object obj, float value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "RunPlaytime";
			jsonPropertyInfoValues14.JsonPropertyName = null;
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("RunPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues18 = jsonPropertyInfoValues14;
			array[11] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues18);
			array[11].IsRequired = true;
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((RunMetrics)obj).FloorReached;
			jsonPropertyInfoValues8.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "FloorReached";
			jsonPropertyInfoValues8.JsonPropertyName = null;
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("FloorReached", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues19 = jsonPropertyInfoValues8;
			array[12] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues19);
			array[12].IsRequired = true;
			jsonPropertyInfoValues4 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((RunMetrics)obj).KilledByEncounter;
			jsonPropertyInfoValues4.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "KilledByEncounter";
			jsonPropertyInfoValues4.JsonPropertyName = null;
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("KilledByEncounter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues20 = jsonPropertyInfoValues4;
			array[13] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues20);
			array[13].IsRequired = true;
			array[13].IsGetNullable = false;
			array[13].IsSetNullable = false;
			JsonPropertyInfoValues<List<CardChoiceMetric>> jsonPropertyInfoValues21 = new JsonPropertyInfoValues<List<CardChoiceMetric>>();
			jsonPropertyInfoValues21.IsProperty = true;
			jsonPropertyInfoValues21.IsPublic = true;
			jsonPropertyInfoValues21.IsVirtual = false;
			jsonPropertyInfoValues21.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues21.Converter = null;
			jsonPropertyInfoValues21.Getter = (object obj) => ((RunMetrics)obj).CardChoices;
			jsonPropertyInfoValues21.Setter = delegate(object obj, [Nullable(2)] List<CardChoiceMetric> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues21.IgnoreCondition = null;
			jsonPropertyInfoValues21.HasJsonInclude = false;
			jsonPropertyInfoValues21.IsExtensionData = false;
			jsonPropertyInfoValues21.NumberHandling = null;
			jsonPropertyInfoValues21.PropertyName = "CardChoices";
			jsonPropertyInfoValues21.JsonPropertyName = null;
			jsonPropertyInfoValues21.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("CardChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CardChoiceMetric>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CardChoiceMetric>> jsonPropertyInfoValues22 = jsonPropertyInfoValues21;
			array[14] = JsonMetadataServices.CreatePropertyInfo<List<CardChoiceMetric>>(options, jsonPropertyInfoValues22);
			array[14].IsRequired = true;
			array[14].IsGetNullable = false;
			array[14].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunMetrics)obj).CampfireUpgrades;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "CampfireUpgrades";
			jsonPropertyInfoValues23.JsonPropertyName = null;
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("CampfireUpgrades", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues24 = jsonPropertyInfoValues23;
			array[15] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues24);
			array[15].IsRequired = true;
			array[15].IsGetNullable = false;
			array[15].IsSetNullable = false;
			JsonPropertyInfoValues<List<EventChoiceMetric>> jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<EventChoiceMetric>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((RunMetrics)obj).EventChoices;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(2)] List<EventChoiceMetric> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "EventChoices";
			jsonPropertyInfoValues25.JsonPropertyName = null;
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("EventChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<EventChoiceMetric>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<EventChoiceMetric>> jsonPropertyInfoValues26 = jsonPropertyInfoValues25;
			array[16] = JsonMetadataServices.CreatePropertyInfo<List<EventChoiceMetric>>(options, jsonPropertyInfoValues26);
			array[16].IsRequired = true;
			array[16].IsGetNullable = false;
			array[16].IsSetNullable = false;
			JsonPropertyInfoValues<List<AncientMetric>> jsonPropertyInfoValues27 = new JsonPropertyInfoValues<List<AncientMetric>>();
			jsonPropertyInfoValues27.IsProperty = true;
			jsonPropertyInfoValues27.IsPublic = true;
			jsonPropertyInfoValues27.IsVirtual = false;
			jsonPropertyInfoValues27.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues27.Converter = null;
			jsonPropertyInfoValues27.Getter = (object obj) => ((RunMetrics)obj).AncientChoices;
			jsonPropertyInfoValues27.Setter = delegate(object obj, [Nullable(2)] List<AncientMetric> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues27.IgnoreCondition = null;
			jsonPropertyInfoValues27.HasJsonInclude = false;
			jsonPropertyInfoValues27.IsExtensionData = false;
			jsonPropertyInfoValues27.NumberHandling = null;
			jsonPropertyInfoValues27.PropertyName = "AncientChoices";
			jsonPropertyInfoValues27.JsonPropertyName = null;
			jsonPropertyInfoValues27.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("AncientChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<AncientMetric>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<AncientMetric>> jsonPropertyInfoValues28 = jsonPropertyInfoValues27;
			array[17] = JsonMetadataServices.CreatePropertyInfo<List<AncientMetric>>(options, jsonPropertyInfoValues28);
			array[17].IsRequired = true;
			array[17].IsGetNullable = false;
			array[17].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunMetrics)obj).RelicBuys;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "RelicBuys";
			jsonPropertyInfoValues23.JsonPropertyName = null;
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("RelicBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues29 = jsonPropertyInfoValues23;
			array[18] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues29);
			array[18].IsRequired = true;
			array[18].IsGetNullable = false;
			array[18].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunMetrics)obj).PotionBuys;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "PotionBuys";
			jsonPropertyInfoValues23.JsonPropertyName = null;
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("PotionBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues30 = jsonPropertyInfoValues23;
			array[19] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues30);
			array[19].IsRequired = true;
			array[19].IsGetNullable = false;
			array[19].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunMetrics)obj).ColorlessBuys;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "ColorlessBuys";
			jsonPropertyInfoValues23.JsonPropertyName = null;
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("ColorlessBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues31 = jsonPropertyInfoValues23;
			array[20] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues31);
			array[20].IsRequired = true;
			array[20].IsGetNullable = false;
			array[20].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunMetrics)obj).PotionDiscards;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "PotionDiscards";
			jsonPropertyInfoValues23.JsonPropertyName = null;
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("PotionDiscards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues32 = jsonPropertyInfoValues23;
			array[21] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues32);
			array[21].IsRequired = true;
			array[21].IsGetNullable = false;
			array[21].IsSetNullable = false;
			JsonPropertyInfoValues<List<EncounterMetric>> jsonPropertyInfoValues33 = new JsonPropertyInfoValues<List<EncounterMetric>>();
			jsonPropertyInfoValues33.IsProperty = true;
			jsonPropertyInfoValues33.IsPublic = true;
			jsonPropertyInfoValues33.IsVirtual = false;
			jsonPropertyInfoValues33.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues33.Converter = null;
			jsonPropertyInfoValues33.Getter = (object obj) => ((RunMetrics)obj).Encounters;
			jsonPropertyInfoValues33.Setter = delegate(object obj, [Nullable(2)] List<EncounterMetric> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues33.IgnoreCondition = null;
			jsonPropertyInfoValues33.HasJsonInclude = false;
			jsonPropertyInfoValues33.IsExtensionData = false;
			jsonPropertyInfoValues33.NumberHandling = null;
			jsonPropertyInfoValues33.PropertyName = "Encounters";
			jsonPropertyInfoValues33.JsonPropertyName = null;
			jsonPropertyInfoValues33.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Encounters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<EncounterMetric>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<EncounterMetric>> jsonPropertyInfoValues34 = jsonPropertyInfoValues33;
			array[22] = JsonMetadataServices.CreatePropertyInfo<List<EncounterMetric>>(options, jsonPropertyInfoValues34);
			array[22].IsRequired = true;
			array[22].IsGetNullable = false;
			array[22].IsSetNullable = false;
			JsonPropertyInfoValues<List<ActWinMetric>> jsonPropertyInfoValues35 = new JsonPropertyInfoValues<List<ActWinMetric>>();
			jsonPropertyInfoValues35.IsProperty = true;
			jsonPropertyInfoValues35.IsPublic = true;
			jsonPropertyInfoValues35.IsVirtual = false;
			jsonPropertyInfoValues35.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues35.Converter = null;
			jsonPropertyInfoValues35.Getter = (object obj) => ((RunMetrics)obj).ActWins;
			jsonPropertyInfoValues35.Setter = delegate(object obj, [Nullable(2)] List<ActWinMetric> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues35.IgnoreCondition = null;
			jsonPropertyInfoValues35.HasJsonInclude = false;
			jsonPropertyInfoValues35.IsExtensionData = false;
			jsonPropertyInfoValues35.NumberHandling = null;
			jsonPropertyInfoValues35.PropertyName = "ActWins";
			jsonPropertyInfoValues35.JsonPropertyName = null;
			jsonPropertyInfoValues35.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("ActWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ActWinMetric>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ActWinMetric>> jsonPropertyInfoValues36 = jsonPropertyInfoValues35;
			array[23] = JsonMetadataServices.CreatePropertyInfo<List<ActWinMetric>>(options, jsonPropertyInfoValues36);
			array[23].IsRequired = true;
			array[23].IsGetNullable = false;
			array[23].IsSetNullable = false;
			JsonPropertyInfoValues<IEnumerable<ModelId>> jsonPropertyInfoValues37 = new JsonPropertyInfoValues<IEnumerable<ModelId>>();
			jsonPropertyInfoValues37.IsProperty = true;
			jsonPropertyInfoValues37.IsPublic = true;
			jsonPropertyInfoValues37.IsVirtual = false;
			jsonPropertyInfoValues37.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues37.Converter = null;
			jsonPropertyInfoValues37.Getter = (object obj) => ((RunMetrics)obj).Deck;
			jsonPropertyInfoValues37.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<ModelId> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues37.IgnoreCondition = null;
			jsonPropertyInfoValues37.HasJsonInclude = false;
			jsonPropertyInfoValues37.IsExtensionData = false;
			jsonPropertyInfoValues37.NumberHandling = null;
			jsonPropertyInfoValues37.PropertyName = "Deck";
			jsonPropertyInfoValues37.JsonPropertyName = null;
			jsonPropertyInfoValues37.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<ModelId>> jsonPropertyInfoValues38 = jsonPropertyInfoValues37;
			array[24] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<ModelId>>(options, jsonPropertyInfoValues38);
			array[24].IsRequired = true;
			array[24].IsGetNullable = false;
			array[24].IsSetNullable = false;
			jsonPropertyInfoValues37 = new JsonPropertyInfoValues<IEnumerable<ModelId>>();
			jsonPropertyInfoValues37.IsProperty = true;
			jsonPropertyInfoValues37.IsPublic = true;
			jsonPropertyInfoValues37.IsVirtual = false;
			jsonPropertyInfoValues37.DeclaringType = typeof(RunMetrics);
			jsonPropertyInfoValues37.Converter = null;
			jsonPropertyInfoValues37.Getter = (object obj) => ((RunMetrics)obj).Relics;
			jsonPropertyInfoValues37.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<ModelId> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues37.IgnoreCondition = null;
			jsonPropertyInfoValues37.HasJsonInclude = false;
			jsonPropertyInfoValues37.IsExtensionData = false;
			jsonPropertyInfoValues37.NumberHandling = null;
			jsonPropertyInfoValues37.PropertyName = "Relics";
			jsonPropertyInfoValues37.JsonPropertyName = null;
			jsonPropertyInfoValues37.AttributeProviderFactory = () => typeof(RunMetrics).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<ModelId>> jsonPropertyInfoValues39 = jsonPropertyInfoValues37;
			array[25] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<ModelId>>(options, jsonPropertyInfoValues39);
			array[25].IsRequired = true;
			array[25].IsGetNullable = false;
			array[25].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00056664 File Offset: 0x00054864
		private void RunMetricsSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] RunMetrics value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
			writer.WriteString(MetricsSerializerContext.PropName_playerId, value.PlayerId);
			writer.WritePropertyName(MetricsSerializerContext.PropName_character);
			this.ModelIdSerializeHandler(writer, value.Character);
			writer.WriteBoolean(MetricsSerializerContext.PropName_win, value.Win);
			writer.WriteNumber(MetricsSerializerContext.PropName_numPlayers, value.NumPlayers);
			writer.WritePropertyName(MetricsSerializerContext.PropName_team);
			this.ListModelIdSerializeHandler(writer, value.Team);
			writer.WriteString(MetricsSerializerContext.PropName_buildType, value.BuildType);
			writer.WriteNumber(MetricsSerializerContext.PropName_ascension, value.Ascension);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalWinRate, value.TotalWinRate);
			writer.WriteNumber(MetricsSerializerContext.PropName_numReloads, value.NumReloads);
			writer.WriteNumber(MetricsSerializerContext.PropName_runPlaytime, value.RunPlaytime);
			writer.WriteNumber(MetricsSerializerContext.PropName_floorReached, value.FloorReached);
			writer.WritePropertyName(MetricsSerializerContext.PropName_killedByEncounter);
			this.ModelIdSerializeHandler(writer, value.KilledByEncounter);
			writer.WritePropertyName(MetricsSerializerContext.PropName_cardChoices);
			this.ListCardChoiceMetricSerializeHandler(writer, value.CardChoices);
			writer.WritePropertyName(MetricsSerializerContext.PropName_campfireUpgrades);
			this.ListStringSerializeHandler(writer, value.CampfireUpgrades);
			writer.WritePropertyName(MetricsSerializerContext.PropName_eventChoices);
			this.ListEventChoiceMetricSerializeHandler(writer, value.EventChoices);
			writer.WritePropertyName(MetricsSerializerContext.PropName_ancientChoices);
			this.ListAncientMetricSerializeHandler(writer, value.AncientChoices);
			writer.WritePropertyName(MetricsSerializerContext.PropName_relicBuys);
			this.ListStringSerializeHandler(writer, value.RelicBuys);
			writer.WritePropertyName(MetricsSerializerContext.PropName_potionBuys);
			this.ListStringSerializeHandler(writer, value.PotionBuys);
			writer.WritePropertyName(MetricsSerializerContext.PropName_colorlessBuys);
			this.ListStringSerializeHandler(writer, value.ColorlessBuys);
			writer.WritePropertyName(MetricsSerializerContext.PropName_potionDiscards);
			this.ListStringSerializeHandler(writer, value.PotionDiscards);
			writer.WritePropertyName(MetricsSerializerContext.PropName_encounters);
			this.ListEncounterMetricSerializeHandler(writer, value.Encounters);
			writer.WritePropertyName(MetricsSerializerContext.PropName_actWins);
			this.ListActWinMetricSerializeHandler(writer, value.ActWins);
			writer.WritePropertyName(MetricsSerializerContext.PropName_deck);
			this.IEnumerableModelIdSerializeHandler(writer, value.Deck);
			writer.WritePropertyName(MetricsSerializerContext.PropName_relics);
			this.IEnumerableModelIdSerializeHandler(writer, value.Relics);
			writer.WriteEndObject();
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000568AC File Offset: 0x00054AAC
		private static JsonParameterInfoValues[] RunMetricsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "BuildId",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "PlayerId",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Character",
					ParameterType = typeof(ModelId),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Win",
					ParameterType = typeof(bool),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "NumPlayers",
					ParameterType = typeof(int),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Team",
					ParameterType = typeof(List<ModelId>),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BuildType",
					ParameterType = typeof(string),
					Position = 6,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Ascension",
					ParameterType = typeof(int),
					Position = 7,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "TotalPlaytime",
					ParameterType = typeof(float),
					Position = 8,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "TotalWinRate",
					ParameterType = typeof(float),
					Position = 9,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "NumReloads",
					ParameterType = typeof(int),
					Position = 10,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "RunPlaytime",
					ParameterType = typeof(float),
					Position = 11,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "FloorReached",
					ParameterType = typeof(int),
					Position = 12,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "KilledByEncounter",
					ParameterType = typeof(ModelId),
					Position = 13,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "CardChoices",
					ParameterType = typeof(List<CardChoiceMetric>),
					Position = 14,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "CampfireUpgrades",
					ParameterType = typeof(List<string>),
					Position = 15,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "EventChoices",
					ParameterType = typeof(List<EventChoiceMetric>),
					Position = 16,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "AncientChoices",
					ParameterType = typeof(List<AncientMetric>),
					Position = 17,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "RelicBuys",
					ParameterType = typeof(List<string>),
					Position = 18,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "PotionBuys",
					ParameterType = typeof(List<string>),
					Position = 19,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "ColorlessBuys",
					ParameterType = typeof(List<string>),
					Position = 20,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "PotionDiscards",
					ParameterType = typeof(List<string>),
					Position = 21,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Encounters",
					ParameterType = typeof(List<EncounterMetric>),
					Position = 22,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "ActWins",
					ParameterType = typeof(List<ActWinMetric>),
					Position = 23,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Deck",
					ParameterType = typeof(IEnumerable<ModelId>),
					Position = 24,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Relics",
					ParameterType = typeof(IEnumerable<ModelId>),
					Position = 25,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00056EC8 File Offset: 0x000550C8
		[Nullable(0)]
		public JsonTypeInfo<SettingsDataMetric> SettingsDataMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<SettingsDataMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._SettingsDataMetric) == null)
				{
					jsonTypeInfo = (this._SettingsDataMetric = (JsonTypeInfo<SettingsDataMetric>)base.Options.GetTypeInfo(typeof(SettingsDataMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00056F04 File Offset: 0x00055104
		private JsonTypeInfo<SettingsDataMetric> Create_SettingsDataMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<SettingsDataMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SettingsDataMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SettingsDataMetric> jsonObjectInfoValues = new JsonObjectInfoValues<SettingsDataMetric>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new SettingsDataMetric
				{
					BuildId = (string)args[0],
					FastModeType = (FastModeType)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.SettingsDataMetricPropInit(options);
				JsonObjectInfoValues<SettingsDataMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MetricsSerializerContext.<>O.<3>__SettingsDataMetricCtorParamInit) == null)
				{
					func = (MetricsSerializerContext.<>O.<3>__SettingsDataMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.SettingsDataMetricCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SettingsDataMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, SettingsDataMetric>(this.SettingsDataMetricSerializeHandler);
				JsonObjectInfoValues<SettingsDataMetric> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SettingsDataMetric>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00056FF4 File Offset: 0x000551F4
		private static JsonPropertyInfo[] SettingsDataMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[18];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsDataMetric)obj).BuildId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).BuildId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BuildId";
			jsonPropertyInfoValues.JsonPropertyName = "buildId";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsDataMetric)obj).Os;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).Os = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Os";
			jsonPropertyInfoValues.JsonPropertyName = "os";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("Os", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsDataMetric)obj).Platform;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).Platform = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Platform";
			jsonPropertyInfoValues.JsonPropertyName = "platform";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("Platform", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsDataMetric)obj).SystemRam;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).SystemRam = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "SystemRam";
			jsonPropertyInfoValues5.JsonPropertyName = "systemRam";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("SystemRam", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsDataMetric)obj).LanguageCode;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).LanguageCode = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "LanguageCode";
			jsonPropertyInfoValues.JsonPropertyName = "language";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("LanguageCode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			JsonPropertyInfoValues<FastModeType> jsonPropertyInfoValues8 = new JsonPropertyInfoValues<FastModeType>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((SettingsDataMetric)obj).FastModeType;
			jsonPropertyInfoValues8.Setter = delegate(object obj, FastModeType value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).FastModeType = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "FastModeType";
			jsonPropertyInfoValues8.JsonPropertyName = "combatSpeed";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("FastModeType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(FastModeType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<FastModeType> jsonPropertyInfoValues9 = jsonPropertyInfoValues8;
			array[5] = JsonMetadataServices.CreatePropertyInfo<FastModeType>(options, jsonPropertyInfoValues9);
			array[5].IsRequired = true;
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsDataMetric)obj).Screenshake;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).Screenshake = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Screenshake";
			jsonPropertyInfoValues5.JsonPropertyName = "screenshake";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("Screenshake", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues10 = jsonPropertyInfoValues5;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues10);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsDataMetric)obj).ShowRunTimer;
			jsonPropertyInfoValues11.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).ShowRunTimer = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "ShowRunTimer";
			jsonPropertyInfoValues11.JsonPropertyName = "runTimer";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("ShowRunTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues11 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsDataMetric)obj).PhobiaMode;
			jsonPropertyInfoValues11.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).PhobiaMode = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "PhobiaMode";
			jsonPropertyInfoValues11.JsonPropertyName = "phobiaMode";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("PhobiaMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues13 = jsonPropertyInfoValues11;
			array[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues13);
			jsonPropertyInfoValues11 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsDataMetric)obj).ShowCardIndices;
			jsonPropertyInfoValues11.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).ShowCardIndices = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "ShowCardIndices";
			jsonPropertyInfoValues11.JsonPropertyName = "cardIndices";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("ShowCardIndices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues14 = jsonPropertyInfoValues11;
			array[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues14);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsDataMetric)obj).DisplayCount;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).DisplayCount = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "DisplayCount";
			jsonPropertyInfoValues5.JsonPropertyName = "displayCount";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("DisplayCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues15 = jsonPropertyInfoValues5;
			array[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues15);
			JsonPropertyInfoValues<Vector2I> jsonPropertyInfoValues16 = new JsonPropertyInfoValues<Vector2I>();
			jsonPropertyInfoValues16.IsProperty = true;
			jsonPropertyInfoValues16.IsPublic = true;
			jsonPropertyInfoValues16.IsVirtual = false;
			jsonPropertyInfoValues16.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues16.Converter = null;
			jsonPropertyInfoValues16.Getter = (object obj) => ((SettingsDataMetric)obj).DisplayResolution;
			jsonPropertyInfoValues16.Setter = delegate(object obj, Vector2I value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).DisplayResolution = value;
			};
			jsonPropertyInfoValues16.IgnoreCondition = null;
			jsonPropertyInfoValues16.HasJsonInclude = false;
			jsonPropertyInfoValues16.IsExtensionData = false;
			jsonPropertyInfoValues16.NumberHandling = null;
			jsonPropertyInfoValues16.PropertyName = "DisplayResolution";
			jsonPropertyInfoValues16.JsonPropertyName = "displayResolution";
			jsonPropertyInfoValues16.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("DisplayResolution", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Vector2I), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Vector2I> jsonPropertyInfoValues17 = jsonPropertyInfoValues16;
			array[11] = JsonMetadataServices.CreatePropertyInfo<Vector2I>(options, jsonPropertyInfoValues17);
			jsonPropertyInfoValues11 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsDataMetric)obj).Fullscreen;
			jsonPropertyInfoValues11.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).Fullscreen = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "Fullscreen";
			jsonPropertyInfoValues11.JsonPropertyName = "fullscreen";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("Fullscreen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues18 = jsonPropertyInfoValues11;
			array[12] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues18);
			JsonPropertyInfoValues<AspectRatioSetting> jsonPropertyInfoValues19 = new JsonPropertyInfoValues<AspectRatioSetting>();
			jsonPropertyInfoValues19.IsProperty = true;
			jsonPropertyInfoValues19.IsPublic = true;
			jsonPropertyInfoValues19.IsVirtual = false;
			jsonPropertyInfoValues19.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues19.Converter = null;
			jsonPropertyInfoValues19.Getter = (object obj) => ((SettingsDataMetric)obj).AspectRatio;
			jsonPropertyInfoValues19.Setter = delegate(object obj, AspectRatioSetting value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).AspectRatio = value;
			};
			jsonPropertyInfoValues19.IgnoreCondition = null;
			jsonPropertyInfoValues19.HasJsonInclude = false;
			jsonPropertyInfoValues19.IsExtensionData = false;
			jsonPropertyInfoValues19.NumberHandling = null;
			jsonPropertyInfoValues19.PropertyName = "AspectRatio";
			jsonPropertyInfoValues19.JsonPropertyName = "aspectRatio";
			jsonPropertyInfoValues19.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("AspectRatio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(AspectRatioSetting), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<AspectRatioSetting> jsonPropertyInfoValues20 = jsonPropertyInfoValues19;
			array[13] = JsonMetadataServices.CreatePropertyInfo<AspectRatioSetting>(options, jsonPropertyInfoValues20);
			jsonPropertyInfoValues11 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsDataMetric)obj).ResizeWindows;
			jsonPropertyInfoValues11.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).ResizeWindows = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "ResizeWindows";
			jsonPropertyInfoValues11.JsonPropertyName = "resizeWindows";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("ResizeWindows", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues21 = jsonPropertyInfoValues11;
			array[14] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues21);
			JsonPropertyInfoValues<VSyncType> jsonPropertyInfoValues22 = new JsonPropertyInfoValues<VSyncType>();
			jsonPropertyInfoValues22.IsProperty = true;
			jsonPropertyInfoValues22.IsPublic = true;
			jsonPropertyInfoValues22.IsVirtual = false;
			jsonPropertyInfoValues22.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues22.Converter = null;
			jsonPropertyInfoValues22.Getter = (object obj) => ((SettingsDataMetric)obj).VSync;
			jsonPropertyInfoValues22.Setter = delegate(object obj, VSyncType value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).VSync = value;
			};
			jsonPropertyInfoValues22.IgnoreCondition = null;
			jsonPropertyInfoValues22.HasJsonInclude = false;
			jsonPropertyInfoValues22.IsExtensionData = false;
			jsonPropertyInfoValues22.NumberHandling = null;
			jsonPropertyInfoValues22.PropertyName = "VSync";
			jsonPropertyInfoValues22.JsonPropertyName = "vSync";
			jsonPropertyInfoValues22.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("VSync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(VSyncType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<VSyncType> jsonPropertyInfoValues23 = jsonPropertyInfoValues22;
			array[15] = JsonMetadataServices.CreatePropertyInfo<VSyncType>(options, jsonPropertyInfoValues23);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsDataMetric)obj).FpsLimit;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).FpsLimit = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "FpsLimit";
			jsonPropertyInfoValues5.JsonPropertyName = "fpsLimit";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("FpsLimit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues24 = jsonPropertyInfoValues5;
			array[16] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues24);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsDataMetric);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsDataMetric)obj).Msaa;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SettingsDataMetric>(obj).Msaa = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Msaa";
			jsonPropertyInfoValues5.JsonPropertyName = "msaa";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsDataMetric).GetProperty("Msaa", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues25 = jsonPropertyInfoValues5;
			array[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues25);
			return array;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000582A0 File Offset: 0x000564A0
		private void SettingsDataMetricSerializeHandler(Utf8JsonWriter writer, SettingsDataMetric value)
		{
			writer.WriteStartObject();
			JsonEncodedText propName_buildId = MetricsSerializerContext.PropName_buildId;
			SettingsDataMetric settingsDataMetric = value;
			writer.WriteString(propName_buildId, settingsDataMetric.BuildId);
			JsonEncodedText propName_os = MetricsSerializerContext.PropName_os;
			settingsDataMetric = value;
			writer.WriteString(propName_os, settingsDataMetric.Os);
			JsonEncodedText propName_platform = MetricsSerializerContext.PropName_platform;
			settingsDataMetric = value;
			writer.WriteString(propName_platform, settingsDataMetric.Platform);
			JsonEncodedText propName_systemRam = MetricsSerializerContext.PropName_systemRam;
			settingsDataMetric = value;
			writer.WriteNumber(propName_systemRam, settingsDataMetric.SystemRam);
			JsonEncodedText propName_language = MetricsSerializerContext.PropName_language;
			settingsDataMetric = value;
			writer.WriteString(propName_language, settingsDataMetric.LanguageCode);
			writer.WritePropertyName(MetricsSerializerContext.PropName_combatSpeed);
			settingsDataMetric = value;
			JsonSerializer.Serialize<FastModeType>(writer, settingsDataMetric.FastModeType, this.FastModeType);
			JsonEncodedText propName_screenshake = MetricsSerializerContext.PropName_screenshake;
			settingsDataMetric = value;
			writer.WriteNumber(propName_screenshake, settingsDataMetric.Screenshake);
			JsonEncodedText propName_runTimer = MetricsSerializerContext.PropName_runTimer;
			settingsDataMetric = value;
			writer.WriteBoolean(propName_runTimer, settingsDataMetric.ShowRunTimer);
			JsonEncodedText propName_phobiaMode = MetricsSerializerContext.PropName_phobiaMode;
			settingsDataMetric = value;
			writer.WriteBoolean(propName_phobiaMode, settingsDataMetric.PhobiaMode);
			JsonEncodedText propName_cardIndices = MetricsSerializerContext.PropName_cardIndices;
			settingsDataMetric = value;
			writer.WriteBoolean(propName_cardIndices, settingsDataMetric.ShowCardIndices);
			JsonEncodedText propName_displayCount = MetricsSerializerContext.PropName_displayCount;
			settingsDataMetric = value;
			writer.WriteNumber(propName_displayCount, settingsDataMetric.DisplayCount);
			writer.WritePropertyName(MetricsSerializerContext.PropName_displayResolution);
			settingsDataMetric = value;
			this.Vector2ISerializeHandler(writer, settingsDataMetric.DisplayResolution);
			JsonEncodedText propName_fullscreen = MetricsSerializerContext.PropName_fullscreen;
			settingsDataMetric = value;
			writer.WriteBoolean(propName_fullscreen, settingsDataMetric.Fullscreen);
			writer.WritePropertyName(MetricsSerializerContext.PropName_aspectRatio);
			settingsDataMetric = value;
			JsonSerializer.Serialize<AspectRatioSetting>(writer, settingsDataMetric.AspectRatio, this.AspectRatioSetting);
			JsonEncodedText propName_resizeWindows = MetricsSerializerContext.PropName_resizeWindows;
			settingsDataMetric = value;
			writer.WriteBoolean(propName_resizeWindows, settingsDataMetric.ResizeWindows);
			writer.WritePropertyName(MetricsSerializerContext.PropName_vSync);
			settingsDataMetric = value;
			JsonSerializer.Serialize<VSyncType>(writer, settingsDataMetric.VSync, this.VSyncType);
			JsonEncodedText propName_fpsLimit = MetricsSerializerContext.PropName_fpsLimit;
			settingsDataMetric = value;
			writer.WriteNumber(propName_fpsLimit, settingsDataMetric.FpsLimit);
			JsonEncodedText propName_msaa = MetricsSerializerContext.PropName_msaa;
			settingsDataMetric = value;
			writer.WriteNumber(propName_msaa, settingsDataMetric.Msaa);
			writer.WriteEndObject();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0005844C File Offset: 0x0005664C
		private static JsonParameterInfoValues[] SettingsDataMetricCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "BuildId",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "FastModeType",
					ParameterType = typeof(FastModeType),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x000584D4 File Offset: 0x000566D4
		[Nullable(0)]
		public JsonTypeInfo<AspectRatioSetting> AspectRatioSetting
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<AspectRatioSetting> jsonTypeInfo;
				if ((jsonTypeInfo = this._AspectRatioSetting) == null)
				{
					jsonTypeInfo = (this._AspectRatioSetting = (JsonTypeInfo<AspectRatioSetting>)base.Options.GetTypeInfo(typeof(AspectRatioSetting)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00058510 File Offset: 0x00056710
		private JsonTypeInfo<AspectRatioSetting> Create_AspectRatioSetting(JsonSerializerOptions options)
		{
			JsonTypeInfo<AspectRatioSetting> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AspectRatioSetting>(options, out jsonTypeInfo))
			{
				JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof(AspectRatioSetting), new JsonStringEnumConverter<AspectRatioSetting>(), options, true);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<AspectRatioSetting>(options, jsonConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00058550 File Offset: 0x00056750
		[Nullable(0)]
		public JsonTypeInfo<FastModeType> FastModeType
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<FastModeType> jsonTypeInfo;
				if ((jsonTypeInfo = this._FastModeType) == null)
				{
					jsonTypeInfo = (this._FastModeType = (JsonTypeInfo<FastModeType>)base.Options.GetTypeInfo(typeof(FastModeType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0005858C File Offset: 0x0005678C
		private JsonTypeInfo<FastModeType> Create_FastModeType(JsonSerializerOptions options)
		{
			JsonTypeInfo<FastModeType> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<FastModeType>(options, out jsonTypeInfo))
			{
				JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof(FastModeType), new JsonStringEnumConverter<FastModeType>(), options, true);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<FastModeType>(options, jsonConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x000585CC File Offset: 0x000567CC
		[Nullable(0)]
		public JsonTypeInfo<VSyncType> VSyncType
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<VSyncType> jsonTypeInfo;
				if ((jsonTypeInfo = this._VSyncType) == null)
				{
					jsonTypeInfo = (this._VSyncType = (JsonTypeInfo<VSyncType>)base.Options.GetTypeInfo(typeof(VSyncType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00058608 File Offset: 0x00056808
		private JsonTypeInfo<VSyncType> Create_VSyncType(JsonSerializerOptions options)
		{
			JsonTypeInfo<VSyncType> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<VSyncType>(options, out jsonTypeInfo))
			{
				JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof(VSyncType), new JsonStringEnumConverter<VSyncType>(), options, true);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<VSyncType>(options, jsonConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00058648 File Offset: 0x00056848
		[Nullable(0)]
		public JsonTypeInfo<EpochMetric> EpochMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EpochMetric> jsonTypeInfo;
				if ((jsonTypeInfo = this._EpochMetric) == null)
				{
					jsonTypeInfo = (this._EpochMetric = (JsonTypeInfo<EpochMetric>)base.Options.GetTypeInfo(typeof(EpochMetric)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00058684 File Offset: 0x00056884
		private JsonTypeInfo<EpochMetric> Create_EpochMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<EpochMetric> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EpochMetric>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EpochMetric> jsonObjectInfoValues = new JsonObjectInfoValues<EpochMetric>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EpochMetric
				{
					BuildId = (string)args[0],
					Epoch = (string)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MetricsSerializerContext.EpochMetricPropInit(options);
				JsonObjectInfoValues<EpochMetric> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MetricsSerializerContext.<>O.<4>__EpochMetricCtorParamInit) == null)
				{
					func = (MetricsSerializerContext.<>O.<4>__EpochMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.EpochMetricCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EpochMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EpochMetric>(this.EpochMetricSerializeHandler);
				JsonObjectInfoValues<EpochMetric> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EpochMetric>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00058774 File Offset: 0x00056974
		private static JsonPropertyInfo[] EpochMetricPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EpochMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EpochMetric)obj).BuildId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((EpochMetric)obj).BuildId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BuildId";
			jsonPropertyInfoValues.JsonPropertyName = "buildId";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EpochMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EpochMetric);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EpochMetric)obj).Epoch;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((EpochMetric)obj).Epoch = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Epoch";
			jsonPropertyInfoValues.JsonPropertyName = "epoch";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EpochMetric).GetProperty("Epoch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(EpochMetric);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((EpochMetric)obj).TotalEpochs;
			jsonPropertyInfoValues4.Setter = delegate(object obj, int value)
			{
				((EpochMetric)obj).TotalEpochs = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "TotalEpochs";
			jsonPropertyInfoValues4.JsonPropertyName = "totalEpochs";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(EpochMetric).GetProperty("TotalEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(EpochMetric);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((EpochMetric)obj).TotalPlaytime;
			jsonPropertyInfoValues6.Setter = delegate(object obj, long value)
			{
				((EpochMetric)obj).TotalPlaytime = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "TotalPlaytime";
			jsonPropertyInfoValues6.JsonPropertyName = "totalPlaytime";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(EpochMetric).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues4 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(EpochMetric);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((EpochMetric)obj).TotalRuns;
			jsonPropertyInfoValues4.Setter = delegate(object obj, int value)
			{
				((EpochMetric)obj).TotalRuns = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "TotalRuns";
			jsonPropertyInfoValues4.JsonPropertyName = "totalRuns";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(EpochMetric).GetProperty("TotalRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues4;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			return array;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00058CC8 File Offset: 0x00056EC8
		private void EpochMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] EpochMetric value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
			writer.WriteString(MetricsSerializerContext.PropName_epoch, value.Epoch);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalEpochs, value.TotalEpochs);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
			writer.WriteNumber(MetricsSerializerContext.PropName_totalRuns, value.TotalRuns);
			writer.WriteEndObject();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00058D40 File Offset: 0x00056F40
		private static JsonParameterInfoValues[] EpochMetricCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "BuildId",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Epoch",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00058DC8 File Offset: 0x00056FC8
		[Nullable(0)]
		public JsonTypeInfo<IEnumerable<ModelId>> IEnumerableModelId
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<IEnumerable<ModelId>> jsonTypeInfo;
				if ((jsonTypeInfo = this._IEnumerableModelId) == null)
				{
					jsonTypeInfo = (this._IEnumerableModelId = (JsonTypeInfo<IEnumerable<ModelId>>)base.Options.GetTypeInfo(typeof(IEnumerable<ModelId>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00058E04 File Offset: 0x00057004
		private JsonTypeInfo<IEnumerable<ModelId>> Create_IEnumerableModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<IEnumerable<ModelId>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<ModelId>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<IEnumerable<ModelId>> jsonCollectionInfoValues = new JsonCollectionInfoValues<IEnumerable<ModelId>>
				{
					ObjectCreator = null,
					SerializeHandler = new Action<Utf8JsonWriter, IEnumerable<ModelId>>(this.IEnumerableModelIdSerializeHandler)
				};
				jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<ModelId>, ModelId>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00058E5C File Offset: 0x0005705C
		private void IEnumerableModelIdSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] IEnumerable<ModelId> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			foreach (ModelId modelId in value)
			{
				this.ModelIdSerializeHandler(writer, modelId);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00058EBC File Offset: 0x000570BC
		[Nullable(0)]
		public JsonTypeInfo<List<ModelId>> ListModelId
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<ModelId>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListModelId) == null)
				{
					jsonTypeInfo = (this._ListModelId = (JsonTypeInfo<List<ModelId>>)base.Options.GetTypeInfo(typeof(List<ModelId>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00058EF8 File Offset: 0x000570F8
		private JsonTypeInfo<List<ModelId>> Create_ListModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ModelId>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ModelId>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ModelId>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ModelId>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ModelId>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<ModelId>>(this.ListModelIdSerializeHandler);
				JsonCollectionInfoValues<List<ModelId>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ModelId>, ModelId>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00058F6C File Offset: 0x0005716C
		private void ListModelIdSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.ModelIdSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00058FB0 File Offset: 0x000571B0
		[Nullable(0)]
		public JsonTypeInfo<List<ActWinMetric>> ListActWinMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<ActWinMetric>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListActWinMetric) == null)
				{
					jsonTypeInfo = (this._ListActWinMetric = (JsonTypeInfo<List<ActWinMetric>>)base.Options.GetTypeInfo(typeof(List<ActWinMetric>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00058FEC File Offset: 0x000571EC
		private JsonTypeInfo<List<ActWinMetric>> Create_ListActWinMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ActWinMetric>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ActWinMetric>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ActWinMetric>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ActWinMetric>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ActWinMetric>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<ActWinMetric>>(this.ListActWinMetricSerializeHandler);
				JsonCollectionInfoValues<List<ActWinMetric>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ActWinMetric>, ActWinMetric>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00059060 File Offset: 0x00057260
		private void ListActWinMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] List<ActWinMetric> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.ActWinMetricSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000590A4 File Offset: 0x000572A4
		[Nullable(0)]
		public JsonTypeInfo<List<AncientMetric>> ListAncientMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<AncientMetric>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListAncientMetric) == null)
				{
					jsonTypeInfo = (this._ListAncientMetric = (JsonTypeInfo<List<AncientMetric>>)base.Options.GetTypeInfo(typeof(List<AncientMetric>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000590E0 File Offset: 0x000572E0
		private JsonTypeInfo<List<AncientMetric>> Create_ListAncientMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<AncientMetric>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<AncientMetric>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<AncientMetric>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<AncientMetric>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<AncientMetric>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<AncientMetric>>(this.ListAncientMetricSerializeHandler);
				JsonCollectionInfoValues<List<AncientMetric>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<AncientMetric>, AncientMetric>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00059154 File Offset: 0x00057354
		private void ListAncientMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] List<AncientMetric> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.AncientMetricSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00059198 File Offset: 0x00057398
		[Nullable(0)]
		public JsonTypeInfo<List<CardChoiceMetric>> ListCardChoiceMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<CardChoiceMetric>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCardChoiceMetric) == null)
				{
					jsonTypeInfo = (this._ListCardChoiceMetric = (JsonTypeInfo<List<CardChoiceMetric>>)base.Options.GetTypeInfo(typeof(List<CardChoiceMetric>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000591D4 File Offset: 0x000573D4
		private JsonTypeInfo<List<CardChoiceMetric>> Create_ListCardChoiceMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CardChoiceMetric>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CardChoiceMetric>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CardChoiceMetric>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CardChoiceMetric>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CardChoiceMetric>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<CardChoiceMetric>>(this.ListCardChoiceMetricSerializeHandler);
				JsonCollectionInfoValues<List<CardChoiceMetric>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CardChoiceMetric>, CardChoiceMetric>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00059248 File Offset: 0x00057448
		private void ListCardChoiceMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] List<CardChoiceMetric> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.CardChoiceMetricSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0005928C File Offset: 0x0005748C
		[Nullable(0)]
		public JsonTypeInfo<List<EncounterMetric>> ListEncounterMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<EncounterMetric>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListEncounterMetric) == null)
				{
					jsonTypeInfo = (this._ListEncounterMetric = (JsonTypeInfo<List<EncounterMetric>>)base.Options.GetTypeInfo(typeof(List<EncounterMetric>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000592C8 File Offset: 0x000574C8
		private JsonTypeInfo<List<EncounterMetric>> Create_ListEncounterMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<EncounterMetric>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<EncounterMetric>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<EncounterMetric>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<EncounterMetric>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<EncounterMetric>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<EncounterMetric>>(this.ListEncounterMetricSerializeHandler);
				JsonCollectionInfoValues<List<EncounterMetric>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<EncounterMetric>, EncounterMetric>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0005933C File Offset: 0x0005753C
		private void ListEncounterMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] List<EncounterMetric> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.EncounterMetricSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00059380 File Offset: 0x00057580
		[Nullable(0)]
		public JsonTypeInfo<List<EventChoiceMetric>> ListEventChoiceMetric
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<EventChoiceMetric>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListEventChoiceMetric) == null)
				{
					jsonTypeInfo = (this._ListEventChoiceMetric = (JsonTypeInfo<List<EventChoiceMetric>>)base.Options.GetTypeInfo(typeof(List<EventChoiceMetric>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000593BC File Offset: 0x000575BC
		private JsonTypeInfo<List<EventChoiceMetric>> Create_ListEventChoiceMetric(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<EventChoiceMetric>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<EventChoiceMetric>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<EventChoiceMetric>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<EventChoiceMetric>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<EventChoiceMetric>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<EventChoiceMetric>>(this.ListEventChoiceMetricSerializeHandler);
				JsonCollectionInfoValues<List<EventChoiceMetric>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<EventChoiceMetric>, EventChoiceMetric>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00059430 File Offset: 0x00057630
		private void ListEventChoiceMetricSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] List<EventChoiceMetric> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				this.EventChoiceMetricSerializeHandler(writer, value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00059474 File Offset: 0x00057674
		[Nullable(0)]
		public JsonTypeInfo<List<string>> ListString
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<string>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListString) == null)
				{
					jsonTypeInfo = (this._ListString = (JsonTypeInfo<List<string>>)base.Options.GetTypeInfo(typeof(List<string>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000594B0 File Offset: 0x000576B0
		private JsonTypeInfo<List<string>> Create_ListString(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<string>> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<string>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<string>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<string>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<string>>(this.ListStringSerializeHandler);
				JsonCollectionInfoValues<List<string>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<string>, string>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00059524 File Offset: 0x00057724
		private void ListStringSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] List<string> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				writer.WriteStringValue(value[i]);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00059568 File Offset: 0x00057768
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

		// Token: 0x06000D9D RID: 3485 RVA: 0x000595A4 File Offset: 0x000577A4
		private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x000595D0 File Offset: 0x000577D0
		[Nullable(0)]
		public JsonTypeInfo<long> Int64
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<long> jsonTypeInfo;
				if ((jsonTypeInfo = this._Int64) == null)
				{
					jsonTypeInfo = (this._Int64 = (JsonTypeInfo<long>)base.Options.GetTypeInfo(typeof(long)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0005960C File Offset: 0x0005780C
		private JsonTypeInfo<long> Create_Int64(JsonSerializerOptions options)
		{
			JsonTypeInfo<long> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<long>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<long>(options, JsonMetadataServices.Int64Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00059638 File Offset: 0x00057838
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

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00059674 File Offset: 0x00057874
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0005969F File Offset: 0x0005789F
		public static MetricsSerializerContext Default { get; } = new MetricsSerializerContext(new JsonSerializerOptions(MetricsSerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x000596A6 File Offset: 0x000578A6
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = MetricsSerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x06000DA4 RID: 3492 RVA: 0x000596AE File Offset: 0x000578AE
		public MetricsSerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x06000DA5 RID: 3493 RVA: 0x000596C2 File Offset: 0x000578C2
		public MetricsSerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000596D8 File Offset: 0x000578D8
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = MetricsSerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00059708 File Offset: 0x00057908
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return MetricsSerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00059750 File Offset: 0x00057950
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
		// Token: 0x06000DA9 RID: 3497 RVA: 0x000597B8 File Offset: 0x000579B8
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000597D8 File Offset: 0x000579D8
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(bool))
			{
				return this.Create_Boolean(options);
			}
			if (type == typeof(float))
			{
				return this.Create_Single(options);
			}
			if (type == typeof(Vector2I))
			{
				return this.Create_Vector2I(options);
			}
			if (type == typeof(AchievementMetric))
			{
				return this.Create_AchievementMetric(options);
			}
			if (type == typeof(ModelId))
			{
				return this.Create_ModelId(options);
			}
			if (type == typeof(ActWinMetric))
			{
				return this.Create_ActWinMetric(options);
			}
			if (type == typeof(AncientMetric))
			{
				return this.Create_AncientMetric(options);
			}
			if (type == typeof(CardChoiceMetric))
			{
				return this.Create_CardChoiceMetric(options);
			}
			if (type == typeof(EncounterMetric))
			{
				return this.Create_EncounterMetric(options);
			}
			if (type == typeof(EventChoiceMetric))
			{
				return this.Create_EventChoiceMetric(options);
			}
			if (type == typeof(RunMetrics))
			{
				return this.Create_RunMetrics(options);
			}
			if (type == typeof(SettingsDataMetric))
			{
				return this.Create_SettingsDataMetric(options);
			}
			if (type == typeof(AspectRatioSetting))
			{
				return this.Create_AspectRatioSetting(options);
			}
			if (type == typeof(FastModeType))
			{
				return this.Create_FastModeType(options);
			}
			if (type == typeof(VSyncType))
			{
				return this.Create_VSyncType(options);
			}
			if (type == typeof(EpochMetric))
			{
				return this.Create_EpochMetric(options);
			}
			if (type == typeof(IEnumerable<ModelId>))
			{
				return this.Create_IEnumerableModelId(options);
			}
			if (type == typeof(List<ModelId>))
			{
				return this.Create_ListModelId(options);
			}
			if (type == typeof(List<ActWinMetric>))
			{
				return this.Create_ListActWinMetric(options);
			}
			if (type == typeof(List<AncientMetric>))
			{
				return this.Create_ListAncientMetric(options);
			}
			if (type == typeof(List<CardChoiceMetric>))
			{
				return this.Create_ListCardChoiceMetric(options);
			}
			if (type == typeof(List<EncounterMetric>))
			{
				return this.Create_ListEncounterMetric(options);
			}
			if (type == typeof(List<EventChoiceMetric>))
			{
				return this.Create_ListEventChoiceMetric(options);
			}
			if (type == typeof(List<string>))
			{
				return this.Create_ListString(options);
			}
			if (type == typeof(int))
			{
				return this.Create_Int32(options);
			}
			if (type == typeof(long))
			{
				return this.Create_Int64(options);
			}
			if (type == typeof(string))
			{
				return this.Create_String(options);
			}
			return null;
		}

		// Token: 0x04000495 RID: 1173
		[Nullable(2)]
		private JsonTypeInfo<bool> _Boolean;

		// Token: 0x04000496 RID: 1174
		[Nullable(2)]
		private JsonTypeInfo<float> _Single;

		// Token: 0x04000497 RID: 1175
		[Nullable(2)]
		private JsonTypeInfo<Vector2I> _Vector2I;

		// Token: 0x04000498 RID: 1176
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<AchievementMetric> _AchievementMetric;

		// Token: 0x04000499 RID: 1177
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModelId> _ModelId;

		// Token: 0x0400049A RID: 1178
		[Nullable(2)]
		private JsonTypeInfo<ActWinMetric> _ActWinMetric;

		// Token: 0x0400049B RID: 1179
		[Nullable(2)]
		private JsonTypeInfo<AncientMetric> _AncientMetric;

		// Token: 0x0400049C RID: 1180
		[Nullable(2)]
		private JsonTypeInfo<CardChoiceMetric> _CardChoiceMetric;

		// Token: 0x0400049D RID: 1181
		[Nullable(2)]
		private JsonTypeInfo<EncounterMetric> _EncounterMetric;

		// Token: 0x0400049E RID: 1182
		[Nullable(2)]
		private JsonTypeInfo<EventChoiceMetric> _EventChoiceMetric;

		// Token: 0x0400049F RID: 1183
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<RunMetrics> _RunMetrics;

		// Token: 0x040004A0 RID: 1184
		[Nullable(2)]
		private JsonTypeInfo<SettingsDataMetric> _SettingsDataMetric;

		// Token: 0x040004A1 RID: 1185
		[Nullable(2)]
		private JsonTypeInfo<AspectRatioSetting> _AspectRatioSetting;

		// Token: 0x040004A2 RID: 1186
		[Nullable(2)]
		private JsonTypeInfo<FastModeType> _FastModeType;

		// Token: 0x040004A3 RID: 1187
		[Nullable(2)]
		private JsonTypeInfo<VSyncType> _VSyncType;

		// Token: 0x040004A4 RID: 1188
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EpochMetric> _EpochMetric;

		// Token: 0x040004A5 RID: 1189
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<IEnumerable<ModelId>> _IEnumerableModelId;

		// Token: 0x040004A6 RID: 1190
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<ModelId>> _ListModelId;

		// Token: 0x040004A7 RID: 1191
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<ActWinMetric>> _ListActWinMetric;

		// Token: 0x040004A8 RID: 1192
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<AncientMetric>> _ListAncientMetric;

		// Token: 0x040004A9 RID: 1193
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<CardChoiceMetric>> _ListCardChoiceMetric;

		// Token: 0x040004AA RID: 1194
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<EncounterMetric>> _ListEncounterMetric;

		// Token: 0x040004AB RID: 1195
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<EventChoiceMetric>> _ListEventChoiceMetric;

		// Token: 0x040004AC RID: 1196
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<string>> _ListString;

		// Token: 0x040004AD RID: 1197
		[Nullable(2)]
		private JsonTypeInfo<int> _Int32;

		// Token: 0x040004AE RID: 1198
		[Nullable(2)]
		private JsonTypeInfo<long> _Int64;

		// Token: 0x040004AF RID: 1199
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x040004B0 RID: 1200
		private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions
		{
			Converters = 
			{
				new ModelIdMetricsConverter()
			},
			IncludeFields = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		// Token: 0x040004B1 RID: 1201
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x040004B4 RID: 1204
		private static readonly JsonEncodedText PropName_x = JsonEncodedText.Encode("x", null);

		// Token: 0x040004B5 RID: 1205
		private static readonly JsonEncodedText PropName_y = JsonEncodedText.Encode("y", null);

		// Token: 0x040004B6 RID: 1206
		private static readonly JsonEncodedText PropName_buildId = JsonEncodedText.Encode("buildId", null);

		// Token: 0x040004B7 RID: 1207
		private static readonly JsonEncodedText PropName_achievement = JsonEncodedText.Encode("achievement", null);

		// Token: 0x040004B8 RID: 1208
		private static readonly JsonEncodedText PropName_totalAchievements = JsonEncodedText.Encode("totalAchievements", null);

		// Token: 0x040004B9 RID: 1209
		private static readonly JsonEncodedText PropName_totalPlaytime = JsonEncodedText.Encode("totalPlaytime", null);

		// Token: 0x040004BA RID: 1210
		private static readonly JsonEncodedText PropName_totalRuns = JsonEncodedText.Encode("totalRuns", null);

		// Token: 0x040004BB RID: 1211
		private static readonly JsonEncodedText PropName_category = JsonEncodedText.Encode("category", null);

		// Token: 0x040004BC RID: 1212
		private static readonly JsonEncodedText PropName_entry = JsonEncodedText.Encode("entry", null);

		// Token: 0x040004BD RID: 1213
		private static readonly JsonEncodedText PropName_act = JsonEncodedText.Encode("act", null);

		// Token: 0x040004BE RID: 1214
		private static readonly JsonEncodedText PropName_win = JsonEncodedText.Encode("win", null);

		// Token: 0x040004BF RID: 1215
		private static readonly JsonEncodedText PropName_picked = JsonEncodedText.Encode("picked", null);

		// Token: 0x040004C0 RID: 1216
		private static readonly JsonEncodedText PropName_skipped = JsonEncodedText.Encode("skipped", null);

		// Token: 0x040004C1 RID: 1217
		private static readonly JsonEncodedText PropName_id = JsonEncodedText.Encode("id", null);

		// Token: 0x040004C2 RID: 1218
		private static readonly JsonEncodedText PropName_damage = JsonEncodedText.Encode("damage", null);

		// Token: 0x040004C3 RID: 1219
		private static readonly JsonEncodedText PropName_turns = JsonEncodedText.Encode("turns", null);

		// Token: 0x040004C4 RID: 1220
		private static readonly JsonEncodedText PropName_playerId = JsonEncodedText.Encode("playerId", null);

		// Token: 0x040004C5 RID: 1221
		private static readonly JsonEncodedText PropName_character = JsonEncodedText.Encode("character", null);

		// Token: 0x040004C6 RID: 1222
		private static readonly JsonEncodedText PropName_numPlayers = JsonEncodedText.Encode("numPlayers", null);

		// Token: 0x040004C7 RID: 1223
		private static readonly JsonEncodedText PropName_team = JsonEncodedText.Encode("team", null);

		// Token: 0x040004C8 RID: 1224
		private static readonly JsonEncodedText PropName_buildType = JsonEncodedText.Encode("buildType", null);

		// Token: 0x040004C9 RID: 1225
		private static readonly JsonEncodedText PropName_ascension = JsonEncodedText.Encode("ascension", null);

		// Token: 0x040004CA RID: 1226
		private static readonly JsonEncodedText PropName_totalWinRate = JsonEncodedText.Encode("totalWinRate", null);

		// Token: 0x040004CB RID: 1227
		private static readonly JsonEncodedText PropName_numReloads = JsonEncodedText.Encode("numReloads", null);

		// Token: 0x040004CC RID: 1228
		private static readonly JsonEncodedText PropName_runPlaytime = JsonEncodedText.Encode("runPlaytime", null);

		// Token: 0x040004CD RID: 1229
		private static readonly JsonEncodedText PropName_floorReached = JsonEncodedText.Encode("floorReached", null);

		// Token: 0x040004CE RID: 1230
		private static readonly JsonEncodedText PropName_killedByEncounter = JsonEncodedText.Encode("killedByEncounter", null);

		// Token: 0x040004CF RID: 1231
		private static readonly JsonEncodedText PropName_cardChoices = JsonEncodedText.Encode("cardChoices", null);

		// Token: 0x040004D0 RID: 1232
		private static readonly JsonEncodedText PropName_campfireUpgrades = JsonEncodedText.Encode("campfireUpgrades", null);

		// Token: 0x040004D1 RID: 1233
		private static readonly JsonEncodedText PropName_eventChoices = JsonEncodedText.Encode("eventChoices", null);

		// Token: 0x040004D2 RID: 1234
		private static readonly JsonEncodedText PropName_ancientChoices = JsonEncodedText.Encode("ancientChoices", null);

		// Token: 0x040004D3 RID: 1235
		private static readonly JsonEncodedText PropName_relicBuys = JsonEncodedText.Encode("relicBuys", null);

		// Token: 0x040004D4 RID: 1236
		private static readonly JsonEncodedText PropName_potionBuys = JsonEncodedText.Encode("potionBuys", null);

		// Token: 0x040004D5 RID: 1237
		private static readonly JsonEncodedText PropName_colorlessBuys = JsonEncodedText.Encode("colorlessBuys", null);

		// Token: 0x040004D6 RID: 1238
		private static readonly JsonEncodedText PropName_potionDiscards = JsonEncodedText.Encode("potionDiscards", null);

		// Token: 0x040004D7 RID: 1239
		private static readonly JsonEncodedText PropName_encounters = JsonEncodedText.Encode("encounters", null);

		// Token: 0x040004D8 RID: 1240
		private static readonly JsonEncodedText PropName_actWins = JsonEncodedText.Encode("actWins", null);

		// Token: 0x040004D9 RID: 1241
		private static readonly JsonEncodedText PropName_deck = JsonEncodedText.Encode("deck", null);

		// Token: 0x040004DA RID: 1242
		private static readonly JsonEncodedText PropName_relics = JsonEncodedText.Encode("relics", null);

		// Token: 0x040004DB RID: 1243
		private static readonly JsonEncodedText PropName_os = JsonEncodedText.Encode("os", null);

		// Token: 0x040004DC RID: 1244
		private static readonly JsonEncodedText PropName_platform = JsonEncodedText.Encode("platform", null);

		// Token: 0x040004DD RID: 1245
		private static readonly JsonEncodedText PropName_systemRam = JsonEncodedText.Encode("systemRam", null);

		// Token: 0x040004DE RID: 1246
		private static readonly JsonEncodedText PropName_language = JsonEncodedText.Encode("language", null);

		// Token: 0x040004DF RID: 1247
		private static readonly JsonEncodedText PropName_combatSpeed = JsonEncodedText.Encode("combatSpeed", null);

		// Token: 0x040004E0 RID: 1248
		private static readonly JsonEncodedText PropName_screenshake = JsonEncodedText.Encode("screenshake", null);

		// Token: 0x040004E1 RID: 1249
		private static readonly JsonEncodedText PropName_runTimer = JsonEncodedText.Encode("runTimer", null);

		// Token: 0x040004E2 RID: 1250
		private static readonly JsonEncodedText PropName_phobiaMode = JsonEncodedText.Encode("phobiaMode", null);

		// Token: 0x040004E3 RID: 1251
		private static readonly JsonEncodedText PropName_cardIndices = JsonEncodedText.Encode("cardIndices", null);

		// Token: 0x040004E4 RID: 1252
		private static readonly JsonEncodedText PropName_displayCount = JsonEncodedText.Encode("displayCount", null);

		// Token: 0x040004E5 RID: 1253
		private static readonly JsonEncodedText PropName_displayResolution = JsonEncodedText.Encode("displayResolution", null);

		// Token: 0x040004E6 RID: 1254
		private static readonly JsonEncodedText PropName_fullscreen = JsonEncodedText.Encode("fullscreen", null);

		// Token: 0x040004E7 RID: 1255
		private static readonly JsonEncodedText PropName_aspectRatio = JsonEncodedText.Encode("aspectRatio", null);

		// Token: 0x040004E8 RID: 1256
		private static readonly JsonEncodedText PropName_resizeWindows = JsonEncodedText.Encode("resizeWindows", null);

		// Token: 0x040004E9 RID: 1257
		private static readonly JsonEncodedText PropName_vSync = JsonEncodedText.Encode("vSync", null);

		// Token: 0x040004EA RID: 1258
		private static readonly JsonEncodedText PropName_fpsLimit = JsonEncodedText.Encode("fpsLimit", null);

		// Token: 0x040004EB RID: 1259
		private static readonly JsonEncodedText PropName_msaa = JsonEncodedText.Encode("msaa", null);

		// Token: 0x040004EC RID: 1260
		private static readonly JsonEncodedText PropName_epoch = JsonEncodedText.Encode("epoch", null);

		// Token: 0x040004ED RID: 1261
		private static readonly JsonEncodedText PropName_totalEpochs = JsonEncodedText.Encode("totalEpochs", null);

		// Token: 0x02000ED5 RID: 3797
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003A90 RID: 14992
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <0>__AchievementMetricCtorParamInit;

			// Token: 0x04003A91 RID: 14993
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <1>__ModelIdCtorParamInit;

			// Token: 0x04003A92 RID: 14994
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <2>__RunMetricsCtorParamInit;

			// Token: 0x04003A93 RID: 14995
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <3>__SettingsDataMetricCtorParamInit;

			// Token: 0x04003A94 RID: 14996
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <4>__EpochMetricCtorParamInit;
		}
	}
}
