using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.GameInfo.Objects;

namespace MegaCrit.Sts2.GameInfo
{
	// Token: 0x02000023 RID: 35
	[NullableContext(1)]
	[Nullable(0)]
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, IncludeFields = true, Converters = new Type[] { typeof(ModelIdMetricsConverter) })]
	[JsonSerializable(typeof(List<IGameInfo>))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class GameInfoUploaderSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009808 File Offset: 0x00007A08
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

		// Token: 0x06000134 RID: 308 RVA: 0x00009844 File Offset: 0x00007A44
		private JsonTypeInfo<bool> Create_Boolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<bool> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, JsonMetadataServices.BooleanConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00009870 File Offset: 0x00007A70
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

		// Token: 0x06000136 RID: 310 RVA: 0x000098AC File Offset: 0x00007AAC
		private JsonTypeInfo<ModelId> Create_ModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModelId> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModelId>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues = new JsonObjectInfoValues<ModelId>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new ModelId((string)args[0], (string)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.ModelIdPropInit(options);
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<0>__ModelIdCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<0>__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.ModelIdCtorParamInit));
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

		// Token: 0x06000137 RID: 311 RVA: 0x0000999C File Offset: 0x00007B9C
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

		// Token: 0x06000138 RID: 312 RVA: 0x00009B61 File Offset: 0x00007D61
		private void ModelIdSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] ModelId value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_category, value.Category);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_entry, value.Entry);
			writer.WriteEndObject();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009B9C File Offset: 0x00007D9C
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
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00009C34 File Offset: 0x00007E34
		[Nullable(0)]
		public JsonTypeInfo<AncientChoiceInfo> AncientChoiceInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<AncientChoiceInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._AncientChoiceInfo) == null)
				{
					jsonTypeInfo = (this._AncientChoiceInfo = (JsonTypeInfo<AncientChoiceInfo>)base.Options.GetTypeInfo(typeof(AncientChoiceInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009C70 File Offset: 0x00007E70
		private JsonTypeInfo<AncientChoiceInfo> Create_AncientChoiceInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<AncientChoiceInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AncientChoiceInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AncientChoiceInfo> jsonObjectInfoValues = new JsonObjectInfoValues<AncientChoiceInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new AncientChoiceInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (string)args[3],
					Text = (string)args[4],
					Ancient = (string)args[5]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.AncientChoiceInfoPropInit(options);
				JsonObjectInfoValues<AncientChoiceInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<1>__AncientChoiceInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<1>__AncientChoiceInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.AncientChoiceInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AncientChoiceInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, AncientChoiceInfo>(this.AncientChoiceInfoSerializeHandler);
				JsonObjectInfoValues<AncientChoiceInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AncientChoiceInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009D60 File Offset: 0x00007F60
		private static JsonPropertyInfo[] AncientChoiceInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[6];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceInfo)obj).Ancient;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Ancient";
			jsonPropertyInfoValues.JsonPropertyName = "ancient";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceInfo).GetProperty("Ancient", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			return array;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A424 File Offset: 0x00008624
		private void AncientChoiceInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] AncientChoiceInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_id, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_ancient, value.Ancient);
			writer.WriteEndObject();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A4B0 File Offset: 0x000086B0
		private static JsonParameterInfoValues[] AncientChoiceInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(string),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Ancient",
					ParameterType = typeof(string),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000A620 File Offset: 0x00008820
		[Nullable(0)]
		public JsonTypeInfo<CardInfo> CardInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<CardInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardInfo) == null)
				{
					jsonTypeInfo = (this._CardInfo = (JsonTypeInfo<CardInfo>)base.Options.GetTypeInfo(typeof(CardInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A65C File Offset: 0x0000885C
		private JsonTypeInfo<CardInfo> Create_CardInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardInfo> jsonObjectInfoValues = new JsonObjectInfoValues<CardInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new CardInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Upgraded = (bool)args[4],
					Color = (string)args[5],
					Rarity = (string)args[6],
					Type = (string)args[7],
					BaseDamage = (int)args[8],
					Energy = (int)args[9],
					StarCost = (int)args[10],
					Text = (string)args[11],
					HasArt = (bool)args[12],
					HasJokeArt = (bool)args[13]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.CardInfoPropInit(options);
				JsonObjectInfoValues<CardInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<2>__CardInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<2>__CardInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.CardInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, CardInfo>(this.CardInfoSerializeHandler);
				JsonObjectInfoValues<CardInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A74C File Offset: 0x0000894C
		private static JsonPropertyInfo[] CardInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[14];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((CardInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((CardInfo)obj).Upgraded;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "Upgraded";
			jsonPropertyInfoValues7.JsonPropertyName = "upgraded";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Upgraded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues8);
			array[4].IsRequired = true;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).Color;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Color";
			jsonPropertyInfoValues.JsonPropertyName = "color";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues9 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues9);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).Rarity;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Rarity";
			jsonPropertyInfoValues.JsonPropertyName = "rarity";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues10 = jsonPropertyInfoValues;
			array[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues10);
			array[6].IsRequired = true;
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).Type;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Type";
			jsonPropertyInfoValues.JsonPropertyName = "type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues11 = jsonPropertyInfoValues;
			array[7] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues11);
			array[7].IsRequired = true;
			array[7].IsGetNullable = false;
			array[7].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues12 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues12.IsProperty = true;
			jsonPropertyInfoValues12.IsPublic = true;
			jsonPropertyInfoValues12.IsVirtual = false;
			jsonPropertyInfoValues12.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues12.Converter = null;
			jsonPropertyInfoValues12.Getter = (object obj) => ((CardInfo)obj).BaseDamage;
			jsonPropertyInfoValues12.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues12.IgnoreCondition = null;
			jsonPropertyInfoValues12.HasJsonInclude = false;
			jsonPropertyInfoValues12.IsExtensionData = false;
			jsonPropertyInfoValues12.NumberHandling = null;
			jsonPropertyInfoValues12.PropertyName = "BaseDamage";
			jsonPropertyInfoValues12.JsonPropertyName = "base_damage";
			jsonPropertyInfoValues12.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("BaseDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues13 = jsonPropertyInfoValues12;
			array[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues13);
			array[8].IsRequired = true;
			jsonPropertyInfoValues12 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues12.IsProperty = true;
			jsonPropertyInfoValues12.IsPublic = true;
			jsonPropertyInfoValues12.IsVirtual = false;
			jsonPropertyInfoValues12.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues12.Converter = null;
			jsonPropertyInfoValues12.Getter = (object obj) => ((CardInfo)obj).Energy;
			jsonPropertyInfoValues12.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues12.IgnoreCondition = null;
			jsonPropertyInfoValues12.HasJsonInclude = false;
			jsonPropertyInfoValues12.IsExtensionData = false;
			jsonPropertyInfoValues12.NumberHandling = null;
			jsonPropertyInfoValues12.PropertyName = "Energy";
			jsonPropertyInfoValues12.JsonPropertyName = "energy";
			jsonPropertyInfoValues12.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Energy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues14 = jsonPropertyInfoValues12;
			array[9] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues14);
			array[9].IsRequired = true;
			jsonPropertyInfoValues12 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues12.IsProperty = true;
			jsonPropertyInfoValues12.IsPublic = true;
			jsonPropertyInfoValues12.IsVirtual = false;
			jsonPropertyInfoValues12.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues12.Converter = null;
			jsonPropertyInfoValues12.Getter = (object obj) => ((CardInfo)obj).StarCost;
			jsonPropertyInfoValues12.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues12.IgnoreCondition = null;
			jsonPropertyInfoValues12.HasJsonInclude = false;
			jsonPropertyInfoValues12.IsExtensionData = false;
			jsonPropertyInfoValues12.NumberHandling = null;
			jsonPropertyInfoValues12.PropertyName = "StarCost";
			jsonPropertyInfoValues12.JsonPropertyName = "star_cost";
			jsonPropertyInfoValues12.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("StarCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues15 = jsonPropertyInfoValues12;
			array[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues15);
			array[10].IsRequired = true;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues16 = jsonPropertyInfoValues;
			array[11] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues16);
			array[11].IsRequired = true;
			array[11].IsGetNullable = false;
			array[11].IsSetNullable = false;
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((CardInfo)obj).HasArt;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "HasArt";
			jsonPropertyInfoValues7.JsonPropertyName = "has_art";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("HasArt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues17 = jsonPropertyInfoValues7;
			array[12] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues17);
			array[12].IsRequired = true;
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(CardInfo);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((CardInfo)obj).HasJokeArt;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "HasJokeArt";
			jsonPropertyInfoValues7.JsonPropertyName = "has_joke_art";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(CardInfo).GetProperty("HasJokeArt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues18 = jsonPropertyInfoValues7;
			array[13] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues18);
			array[13].IsRequired = true;
			return array;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000B6A0 File Offset: 0x000098A0
		private void CardInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] CardInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_upgraded, value.Upgraded);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_type, value.Type);
			writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_base_damage, value.BaseDamage);
			writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_energy, value.Energy);
			writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_star_cost, value.StarCost);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_has_art, value.HasArt);
			writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_has_joke_art, value.HasJokeArt);
			writer.WriteEndObject();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000B7B8 File Offset: 0x000099B8
		private static JsonParameterInfoValues[] CardInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Upgraded",
					ParameterType = typeof(bool),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Color",
					ParameterType = typeof(string),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Rarity",
					ParameterType = typeof(string),
					Position = 6,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Type",
					ParameterType = typeof(string),
					Position = 7,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BaseDamage",
					ParameterType = typeof(int),
					Position = 8,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Energy",
					ParameterType = typeof(int),
					Position = 9,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "StarCost",
					ParameterType = typeof(int),
					Position = 10,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 11,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "HasArt",
					ParameterType = typeof(bool),
					Position = 12,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "HasJokeArt",
					ParameterType = typeof(bool),
					Position = 13,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000BB04 File Offset: 0x00009D04
		[Nullable(0)]
		public JsonTypeInfo<DailyMods> DailyMods
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<DailyMods> jsonTypeInfo;
				if ((jsonTypeInfo = this._DailyMods) == null)
				{
					jsonTypeInfo = (this._DailyMods = (JsonTypeInfo<DailyMods>)base.Options.GetTypeInfo(typeof(DailyMods)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000BB40 File Offset: 0x00009D40
		private JsonTypeInfo<DailyMods> Create_DailyMods(JsonSerializerOptions options)
		{
			JsonTypeInfo<DailyMods> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<DailyMods>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<DailyMods> jsonObjectInfoValues = new JsonObjectInfoValues<DailyMods>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new DailyMods
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Text = (string)args[3]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.DailyModsPropInit(options);
				JsonObjectInfoValues<DailyMods> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<3>__DailyModsCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<3>__DailyModsCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.DailyModsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(DailyMods).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, DailyMods>(this.DailyModsSerializeHandler);
				JsonObjectInfoValues<DailyMods> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<DailyMods>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000BC30 File Offset: 0x00009E30
		private static JsonPropertyInfo[] DailyModsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(DailyMods);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((DailyMods)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(DailyMods).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(DailyMods);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((DailyMods)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(DailyMods).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(DailyMods);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((DailyMods)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(DailyMods).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(DailyMods);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((DailyMods)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(DailyMods).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		private void DailyModsSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] DailyMods value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteEndObject();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000C120 File Offset: 0x0000A320
		private static JsonParameterInfoValues[] DailyModsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000C21C File Offset: 0x0000A41C
		[Nullable(0)]
		public JsonTypeInfo<EnchantmentInfo> EnchantmentInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EnchantmentInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._EnchantmentInfo) == null)
				{
					jsonTypeInfo = (this._EnchantmentInfo = (JsonTypeInfo<EnchantmentInfo>)base.Options.GetTypeInfo(typeof(EnchantmentInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000C258 File Offset: 0x0000A458
		private JsonTypeInfo<EnchantmentInfo> Create_EnchantmentInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<EnchantmentInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EnchantmentInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EnchantmentInfo> jsonObjectInfoValues = new JsonObjectInfoValues<EnchantmentInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EnchantmentInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Text = (string)args[4]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.EnchantmentInfoPropInit(options);
				JsonObjectInfoValues<EnchantmentInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<4>__EnchantmentInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<4>__EnchantmentInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EnchantmentInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EnchantmentInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EnchantmentInfo>(this.EnchantmentInfoSerializeHandler);
				JsonObjectInfoValues<EnchantmentInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EnchantmentInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000C348 File Offset: 0x0000A548
		private static JsonPropertyInfo[] EnchantmentInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EnchantmentInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EnchantmentInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EnchantmentInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EnchantmentInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EnchantmentInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EnchantmentInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EnchantmentInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EnchantmentInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EnchantmentInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EnchantmentInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((EnchantmentInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EnchantmentInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EnchantmentInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EnchantmentInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EnchantmentInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			return array;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		private void EnchantmentInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] EnchantmentInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteEndObject();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000C970 File Offset: 0x0000AB70
		private static JsonParameterInfoValues[] EnchantmentInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000CAA8 File Offset: 0x0000ACA8
		[Nullable(0)]
		public JsonTypeInfo<EncounterInfo> EncounterInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EncounterInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._EncounterInfo) == null)
				{
					jsonTypeInfo = (this._EncounterInfo = (JsonTypeInfo<EncounterInfo>)base.Options.GetTypeInfo(typeof(EncounterInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		private JsonTypeInfo<EncounterInfo> Create_EncounterInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<EncounterInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EncounterInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EncounterInfo> jsonObjectInfoValues = new JsonObjectInfoValues<EncounterInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EncounterInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Act = (string)args[4],
					Tier = (string)args[5]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.EncounterInfoPropInit(options);
				JsonObjectInfoValues<EncounterInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<5>__EncounterInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<5>__EncounterInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EncounterInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EncounterInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EncounterInfo>(this.EncounterInfoSerializeHandler);
				JsonObjectInfoValues<EncounterInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EncounterInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		private static JsonPropertyInfo[] EncounterInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[6];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((EncounterInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterInfo)obj).Act;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Act";
			jsonPropertyInfoValues.JsonPropertyName = "act";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("Act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterInfo)obj).Tier;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Tier";
			jsonPropertyInfoValues.JsonPropertyName = "tier";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterInfo).GetProperty("Tier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues8 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues8);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000D298 File Offset: 0x0000B498
		private void EncounterInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] EncounterInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_act, value.Act);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_tier, value.Tier);
			writer.WriteEndObject();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000D328 File Offset: 0x0000B528
		private static JsonParameterInfoValues[] EncounterInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Act",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Tier",
					ParameterType = typeof(string),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000D498 File Offset: 0x0000B698
		[Nullable(0)]
		public JsonTypeInfo<EventInfo> EventInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<EventInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._EventInfo) == null)
				{
					jsonTypeInfo = (this._EventInfo = (JsonTypeInfo<EventInfo>)base.Options.GetTypeInfo(typeof(EventInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		private JsonTypeInfo<EventInfo> Create_EventInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<EventInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EventInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EventInfo> jsonObjectInfoValues = new JsonObjectInfoValues<EventInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EventInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Act = (string)args[4],
					Options = (List<string>)args[5]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.EventInfoPropInit(options);
				JsonObjectInfoValues<EventInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<6>__EventInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<6>__EventInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EventInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EventInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, EventInfo>(this.EventInfoSerializeHandler);
				JsonObjectInfoValues<EventInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EventInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000D5C4 File Offset: 0x0000B7C4
		private static JsonPropertyInfo[] EventInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[6];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((EventInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventInfo)obj).Act;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Act";
			jsonPropertyInfoValues.JsonPropertyName = "act";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("Act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues8 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(EventInfo);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((EventInfo)obj).Options;
			jsonPropertyInfoValues8.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "Options";
			jsonPropertyInfoValues8.JsonPropertyName = "options";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(EventInfo).GetProperty("Options", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues9 = jsonPropertyInfoValues8;
			array[5] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues9);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000DC88 File Offset: 0x0000BE88
		private void EventInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] EventInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_act, value.Act);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_options);
			this.ListStringSerializeHandler(writer, value.Options);
			writer.WriteEndObject();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000DD20 File Offset: 0x0000BF20
		private static JsonParameterInfoValues[] EventInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Act",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Options",
					ParameterType = typeof(List<string>),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000DE90 File Offset: 0x0000C090
		[Nullable(0)]
		public JsonTypeInfo<IGameInfo> IGameInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<IGameInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._IGameInfo) == null)
				{
					jsonTypeInfo = (this._IGameInfo = (JsonTypeInfo<IGameInfo>)base.Options.GetTypeInfo(typeof(IGameInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000DECC File Offset: 0x0000C0CC
		private JsonTypeInfo<IGameInfo> Create_IGameInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<IGameInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IGameInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<IGameInfo> jsonObjectInfoValues = new JsonObjectInfoValues<IGameInfo>
				{
					ObjectCreator = null,
					ObjectWithParameterizedConstructorCreator = null,
					PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.IGameInfoPropInit(options),
					ConstructorParameterMetadataInitializer = null,
					ConstructorAttributeProviderFactory = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<IGameInfo>(options, jsonObjectInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000DF54 File Offset: 0x0000C154
		private static JsonPropertyInfo[] IGameInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = true;
			jsonPropertyInfoValues.DeclaringType = typeof(IGameInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((IGameInfo)obj).Name;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(IGameInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = true;
			jsonPropertyInfoValues.DeclaringType = typeof(IGameInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((IGameInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(IGameInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = true;
			jsonPropertyInfoValues.DeclaringType = typeof(IGameInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((IGameInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(IGameInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsGetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000E230 File Offset: 0x0000C430
		[Nullable(0)]
		public JsonTypeInfo<Keywords> Keywords
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<Keywords> jsonTypeInfo;
				if ((jsonTypeInfo = this._Keywords) == null)
				{
					jsonTypeInfo = (this._Keywords = (JsonTypeInfo<Keywords>)base.Options.GetTypeInfo(typeof(Keywords)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000E26C File Offset: 0x0000C46C
		private JsonTypeInfo<Keywords> Create_Keywords(JsonSerializerOptions options)
		{
			JsonTypeInfo<Keywords> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Keywords>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<Keywords> jsonObjectInfoValues = new JsonObjectInfoValues<Keywords>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new Keywords
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.KeywordsPropInit(options);
				JsonObjectInfoValues<Keywords> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<7>__KeywordsCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<7>__KeywordsCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.KeywordsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(Keywords).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, Keywords>(this.KeywordsSerializeHandler);
				JsonObjectInfoValues<Keywords> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Keywords>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000E35C File Offset: 0x0000C55C
		private static JsonPropertyInfo[] KeywordsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Keywords);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Keywords)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Keywords).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Keywords);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Keywords)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Keywords).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Keywords);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Keywords)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Keywords).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			return array;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		private void KeywordsSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] Keywords value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WriteEndObject();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000E720 File Offset: 0x0000C920
		private static JsonParameterInfoValues[] KeywordsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000E7E4 File Offset: 0x0000C9E4
		[Nullable(0)]
		public JsonTypeInfo<NeowBonusInfo> NeowBonusInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<NeowBonusInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._NeowBonusInfo) == null)
				{
					jsonTypeInfo = (this._NeowBonusInfo = (JsonTypeInfo<NeowBonusInfo>)base.Options.GetTypeInfo(typeof(NeowBonusInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000E820 File Offset: 0x0000CA20
		private JsonTypeInfo<NeowBonusInfo> Create_NeowBonusInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<NeowBonusInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<NeowBonusInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<NeowBonusInfo> jsonObjectInfoValues = new JsonObjectInfoValues<NeowBonusInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new NeowBonusInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (string)args[3],
					Text = (string)args[4]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.NeowBonusInfoPropInit(options);
				JsonObjectInfoValues<NeowBonusInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<8>__NeowBonusInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<8>__NeowBonusInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.NeowBonusInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(NeowBonusInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, NeowBonusInfo>(this.NeowBonusInfoSerializeHandler);
				JsonObjectInfoValues<NeowBonusInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<NeowBonusInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000E910 File Offset: 0x0000CB10
		private static JsonPropertyInfo[] NeowBonusInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NeowBonusInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NeowBonusInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NeowBonusInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NeowBonusInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NeowBonusInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NeowBonusInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NeowBonusInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NeowBonusInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NeowBonusInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NeowBonusInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NeowBonusInfo)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NeowBonusInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NeowBonusInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NeowBonusInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NeowBonusInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000EEB8 File Offset: 0x0000D0B8
		private void NeowBonusInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] NeowBonusInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_id, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteEndObject();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000EF30 File Offset: 0x0000D130
		private static JsonParameterInfoValues[] NeowBonusInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(string),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000F068 File Offset: 0x0000D268
		[Nullable(0)]
		public JsonTypeInfo<PotionInfo> PotionInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<PotionInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._PotionInfo) == null)
				{
					jsonTypeInfo = (this._PotionInfo = (JsonTypeInfo<PotionInfo>)base.Options.GetTypeInfo(typeof(PotionInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000F0A4 File Offset: 0x0000D2A4
		private JsonTypeInfo<PotionInfo> Create_PotionInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<PotionInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<PotionInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<PotionInfo> jsonObjectInfoValues = new JsonObjectInfoValues<PotionInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new PotionInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Rarity = (string)args[4],
					Text = (string)args[5],
					Color = (string)args[6]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.PotionInfoPropInit(options);
				JsonObjectInfoValues<PotionInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<9>__PotionInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<9>__PotionInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.PotionInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(PotionInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, PotionInfo>(this.PotionInfoSerializeHandler);
				JsonObjectInfoValues<PotionInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<PotionInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000F194 File Offset: 0x0000D394
		private static JsonPropertyInfo[] PotionInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[7];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PotionInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).Rarity;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Rarity";
			jsonPropertyInfoValues.JsonPropertyName = "rarity";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues8 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues8);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PotionInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PotionInfo)obj).Color;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Color";
			jsonPropertyInfoValues.JsonPropertyName = "color";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PotionInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues9 = jsonPropertyInfoValues;
			array[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues9);
			array[6].IsRequired = true;
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000F978 File Offset: 0x0000DB78
		private void PotionInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] PotionInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
			writer.WriteEndObject();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		private static JsonParameterInfoValues[] PotionInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Rarity",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Color",
					ParameterType = typeof(string),
					Position = 6,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		[Nullable(0)]
		public JsonTypeInfo<RelicInfo> RelicInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<RelicInfo> jsonTypeInfo;
				if ((jsonTypeInfo = this._RelicInfo) == null)
				{
					jsonTypeInfo = (this._RelicInfo = (JsonTypeInfo<RelicInfo>)base.Options.GetTypeInfo(typeof(RelicInfo)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000FC04 File Offset: 0x0000DE04
		private JsonTypeInfo<RelicInfo> Create_RelicInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<RelicInfo> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RelicInfo>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<RelicInfo> jsonObjectInfoValues = new JsonObjectInfoValues<RelicInfo>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new RelicInfo
				{
					Name = (string)args[0],
					BotKeyword = (string)args[1],
					BotText = (string)args[2],
					Id = (ModelId)args[3],
					Rarity = (string)args[4],
					Text = (string)args[5],
					Color = (string)args[6]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => GameInfoUploaderSerializerContext.RelicInfoPropInit(options);
				JsonObjectInfoValues<RelicInfo> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = GameInfoUploaderSerializerContext.<>O.<10>__RelicInfoCtorParamInit) == null)
				{
					func = (GameInfoUploaderSerializerContext.<>O.<10>__RelicInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.RelicInfoCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(RelicInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = new Action<Utf8JsonWriter, RelicInfo>(this.RelicInfoSerializeHandler);
				JsonObjectInfoValues<RelicInfo> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<RelicInfo>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		private static JsonPropertyInfo[] RelicInfoPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[7];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).Name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).BotKeyword;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotKeyword";
			jsonPropertyInfoValues.JsonPropertyName = "bot_keyword";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsRequired = true;
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).BotText;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "BotText";
			jsonPropertyInfoValues.JsonPropertyName = "bot_text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsRequired = true;
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((RelicInfo)obj).Id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues6);
			array[3].IsRequired = true;
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).Rarity;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Rarity";
			jsonPropertyInfoValues.JsonPropertyName = "rarity";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			array[4].IsRequired = true;
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).Text;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Text";
			jsonPropertyInfoValues.JsonPropertyName = "text";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues8 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues8);
			array[5].IsRequired = true;
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RelicInfo);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RelicInfo)obj).Color;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Color";
			jsonPropertyInfoValues.JsonPropertyName = "color";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RelicInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues9 = jsonPropertyInfoValues;
			array[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues9);
			array[6].IsRequired = true;
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			return array;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000104D8 File Offset: 0x0000E6D8
		private void RelicInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(2)] RelicInfo value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartObject();
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
			writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
			this.ModelIdSerializeHandler(writer, value.Id);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
			writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
			writer.WriteEndObject();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0001057C File Offset: 0x0000E77C
		private static JsonParameterInfoValues[] RelicInfoCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotKeyword",
					ParameterType = typeof(string),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BotText",
					ParameterType = typeof(string),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Rarity",
					ParameterType = typeof(string),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Text",
					ParameterType = typeof(string),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Color",
					ParameterType = typeof(string),
					Position = 6,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00010728 File Offset: 0x0000E928
		[Nullable(0)]
		public JsonTypeInfo<List<IGameInfo>> ListIGameInfo
		{
			[NullableContext(0)]
			get
			{
				JsonTypeInfo<List<IGameInfo>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListIGameInfo) == null)
				{
					jsonTypeInfo = (this._ListIGameInfo = (JsonTypeInfo<List<IGameInfo>>)base.Options.GetTypeInfo(typeof(List<IGameInfo>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00010764 File Offset: 0x0000E964
		private JsonTypeInfo<List<IGameInfo>> Create_ListIGameInfo(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<IGameInfo>> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<IGameInfo>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<IGameInfo>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<IGameInfo>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<IGameInfo>();
				jsonCollectionInfoValues.SerializeHandler = new Action<Utf8JsonWriter, List<IGameInfo>>(this.ListIGameInfoSerializeHandler);
				JsonCollectionInfoValues<List<IGameInfo>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<IGameInfo>, IGameInfo>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000107D8 File Offset: 0x0000E9D8
		private void ListIGameInfoSerializeHandler(Utf8JsonWriter writer, [Nullable(new byte[] { 2, 1 })] List<IGameInfo> value)
		{
			if (value == null)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStartArray();
			for (int i = 0; i < value.Count; i++)
			{
				JsonSerializer.Serialize<IGameInfo>(writer, value[i], this.IGameInfo);
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00010820 File Offset: 0x0000EA20
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

		// Token: 0x06000173 RID: 371 RVA: 0x0001085C File Offset: 0x0000EA5C
		private JsonTypeInfo<List<string>> Create_ListString(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<string>> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
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

		// Token: 0x06000174 RID: 372 RVA: 0x000108D0 File Offset: 0x0000EAD0
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
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00010914 File Offset: 0x0000EB14
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

		// Token: 0x06000176 RID: 374 RVA: 0x00010950 File Offset: 0x0000EB50
		private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0001097C File Offset: 0x0000EB7C
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

		// Token: 0x06000178 RID: 376 RVA: 0x000109B8 File Offset: 0x0000EBB8
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000109E3 File Offset: 0x0000EBE3
		public static GameInfoUploaderSerializerContext Default { get; } = new GameInfoUploaderSerializerContext(new JsonSerializerOptions(GameInfoUploaderSerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000109EA File Offset: 0x0000EBEA
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = GameInfoUploaderSerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x0600017B RID: 379 RVA: 0x000109F2 File Offset: 0x0000EBF2
		public GameInfoUploaderSerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x0600017C RID: 380 RVA: 0x00010A06 File Offset: 0x0000EC06
		public GameInfoUploaderSerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00010A1C File Offset: 0x0000EC1C
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = GameInfoUploaderSerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00010A4C File Offset: 0x0000EC4C
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return GameInfoUploaderSerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00010A94 File Offset: 0x0000EC94
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
		// Token: 0x06000180 RID: 384 RVA: 0x00010AFC File Offset: 0x0000ECFC
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00010B1C File Offset: 0x0000ED1C
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(bool))
			{
				return this.Create_Boolean(options);
			}
			if (type == typeof(ModelId))
			{
				return this.Create_ModelId(options);
			}
			if (type == typeof(AncientChoiceInfo))
			{
				return this.Create_AncientChoiceInfo(options);
			}
			if (type == typeof(CardInfo))
			{
				return this.Create_CardInfo(options);
			}
			if (type == typeof(DailyMods))
			{
				return this.Create_DailyMods(options);
			}
			if (type == typeof(EnchantmentInfo))
			{
				return this.Create_EnchantmentInfo(options);
			}
			if (type == typeof(EncounterInfo))
			{
				return this.Create_EncounterInfo(options);
			}
			if (type == typeof(EventInfo))
			{
				return this.Create_EventInfo(options);
			}
			if (type == typeof(IGameInfo))
			{
				return this.Create_IGameInfo(options);
			}
			if (type == typeof(Keywords))
			{
				return this.Create_Keywords(options);
			}
			if (type == typeof(NeowBonusInfo))
			{
				return this.Create_NeowBonusInfo(options);
			}
			if (type == typeof(PotionInfo))
			{
				return this.Create_PotionInfo(options);
			}
			if (type == typeof(RelicInfo))
			{
				return this.Create_RelicInfo(options);
			}
			if (type == typeof(List<IGameInfo>))
			{
				return this.Create_ListIGameInfo(options);
			}
			if (type == typeof(List<string>))
			{
				return this.Create_ListString(options);
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

		// Token: 0x04000068 RID: 104
		[Nullable(2)]
		private JsonTypeInfo<bool> _Boolean;

		// Token: 0x04000069 RID: 105
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModelId> _ModelId;

		// Token: 0x0400006A RID: 106
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<AncientChoiceInfo> _AncientChoiceInfo;

		// Token: 0x0400006B RID: 107
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<CardInfo> _CardInfo;

		// Token: 0x0400006C RID: 108
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<DailyMods> _DailyMods;

		// Token: 0x0400006D RID: 109
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EnchantmentInfo> _EnchantmentInfo;

		// Token: 0x0400006E RID: 110
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EncounterInfo> _EncounterInfo;

		// Token: 0x0400006F RID: 111
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EventInfo> _EventInfo;

		// Token: 0x04000070 RID: 112
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<IGameInfo> _IGameInfo;

		// Token: 0x04000071 RID: 113
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<Keywords> _Keywords;

		// Token: 0x04000072 RID: 114
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<NeowBonusInfo> _NeowBonusInfo;

		// Token: 0x04000073 RID: 115
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<PotionInfo> _PotionInfo;

		// Token: 0x04000074 RID: 116
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<RelicInfo> _RelicInfo;

		// Token: 0x04000075 RID: 117
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<IGameInfo>> _ListIGameInfo;

		// Token: 0x04000076 RID: 118
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<string>> _ListString;

		// Token: 0x04000077 RID: 119
		[Nullable(2)]
		private JsonTypeInfo<int> _Int32;

		// Token: 0x04000078 RID: 120
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x04000079 RID: 121
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

		// Token: 0x0400007A RID: 122
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x0400007D RID: 125
		private static readonly JsonEncodedText PropName_category = JsonEncodedText.Encode("category", null);

		// Token: 0x0400007E RID: 126
		private static readonly JsonEncodedText PropName_entry = JsonEncodedText.Encode("entry", null);

		// Token: 0x0400007F RID: 127
		private static readonly JsonEncodedText PropName_name = JsonEncodedText.Encode("name", null);

		// Token: 0x04000080 RID: 128
		private static readonly JsonEncodedText PropName_bot_keyword = JsonEncodedText.Encode("bot_keyword", null);

		// Token: 0x04000081 RID: 129
		private static readonly JsonEncodedText PropName_bot_text = JsonEncodedText.Encode("bot_text", null);

		// Token: 0x04000082 RID: 130
		private static readonly JsonEncodedText PropName_id = JsonEncodedText.Encode("id", null);

		// Token: 0x04000083 RID: 131
		private static readonly JsonEncodedText PropName_text = JsonEncodedText.Encode("text", null);

		// Token: 0x04000084 RID: 132
		private static readonly JsonEncodedText PropName_ancient = JsonEncodedText.Encode("ancient", null);

		// Token: 0x04000085 RID: 133
		private static readonly JsonEncodedText PropName_upgraded = JsonEncodedText.Encode("upgraded", null);

		// Token: 0x04000086 RID: 134
		private static readonly JsonEncodedText PropName_color = JsonEncodedText.Encode("color", null);

		// Token: 0x04000087 RID: 135
		private static readonly JsonEncodedText PropName_rarity = JsonEncodedText.Encode("rarity", null);

		// Token: 0x04000088 RID: 136
		private static readonly JsonEncodedText PropName_type = JsonEncodedText.Encode("type", null);

		// Token: 0x04000089 RID: 137
		private static readonly JsonEncodedText PropName_base_damage = JsonEncodedText.Encode("base_damage", null);

		// Token: 0x0400008A RID: 138
		private static readonly JsonEncodedText PropName_energy = JsonEncodedText.Encode("energy", null);

		// Token: 0x0400008B RID: 139
		private static readonly JsonEncodedText PropName_star_cost = JsonEncodedText.Encode("star_cost", null);

		// Token: 0x0400008C RID: 140
		private static readonly JsonEncodedText PropName_has_art = JsonEncodedText.Encode("has_art", null);

		// Token: 0x0400008D RID: 141
		private static readonly JsonEncodedText PropName_has_joke_art = JsonEncodedText.Encode("has_joke_art", null);

		// Token: 0x0400008E RID: 142
		private static readonly JsonEncodedText PropName_act = JsonEncodedText.Encode("act", null);

		// Token: 0x0400008F RID: 143
		private static readonly JsonEncodedText PropName_tier = JsonEncodedText.Encode("tier", null);

		// Token: 0x04000090 RID: 144
		private static readonly JsonEncodedText PropName_options = JsonEncodedText.Encode("options", null);

		// Token: 0x02000DF3 RID: 3571
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040031E9 RID: 12777
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <0>__ModelIdCtorParamInit;

			// Token: 0x040031EA RID: 12778
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <1>__AncientChoiceInfoCtorParamInit;

			// Token: 0x040031EB RID: 12779
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <2>__CardInfoCtorParamInit;

			// Token: 0x040031EC RID: 12780
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <3>__DailyModsCtorParamInit;

			// Token: 0x040031ED RID: 12781
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <4>__EnchantmentInfoCtorParamInit;

			// Token: 0x040031EE RID: 12782
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <5>__EncounterInfoCtorParamInit;

			// Token: 0x040031EF RID: 12783
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <6>__EventInfoCtorParamInit;

			// Token: 0x040031F0 RID: 12784
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <7>__KeywordsCtorParamInit;

			// Token: 0x040031F1 RID: 12785
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <8>__NeowBonusInfoCtorParamInit;

			// Token: 0x040031F2 RID: 12786
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <9>__PotionInfoCtorParamInit;

			// Token: 0x040031F3 RID: 12787
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <10>__RelicInfoCtorParamInit;
		}
	}
}
