using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Migrations;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000A0 RID: 160
	[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, ReadCommentHandling = JsonCommentHandling.Skip, Converters = new Type[]
	{
		typeof(ModelIdRunSaveConverter),
		typeof(SnakeCaseJsonStringEnumConverter<Achievement>),
		typeof(SnakeCaseJsonStringEnumConverter<AspectRatioSetting>),
		typeof(SnakeCaseJsonStringEnumConverter<VSyncType>),
		typeof(SnakeCaseJsonStringEnumConverter<GameMode>),
		typeof(SnakeCaseJsonStringEnumConverter<RelicRarity>),
		typeof(SnakeCaseJsonStringEnumConverter<RunRngType>),
		typeof(SnakeCaseJsonStringEnumConverter<PlayerRngType>),
		typeof(SnakeCaseJsonStringEnumConverter<MapPointType>),
		typeof(SnakeCaseJsonStringEnumConverter<ModSource>),
		typeof(SnakeCaseJsonStringEnumConverter<PlatformType>),
		typeof(SnakeCaseJsonStringEnumConverter<RewardType>),
		typeof(SnakeCaseJsonStringEnumConverter<RoomType>),
		typeof(SnakeCaseJsonStringEnumConverter<EpochState>),
		typeof(SnakeCaseJsonStringEnumConverter<FastModeType>),
		typeof(SnakeCaseJsonStringEnumConverter<CardCreationSource>),
		typeof(SnakeCaseJsonStringEnumConverter<CardRarityOddsType>),
		typeof(SnakeCaseJsonStringEnumConverter<ControllerMappingType>),
		typeof(SnakeCaseJsonStringEnumConverter<BadgeRarity>)
	}, UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip, GenerationMode = JsonSourceGenerationMode.Metadata)]
	[JsonSerializable(typeof(NullLeaderboardFile))]
	[JsonSerializable(typeof(SerializableRun))]
	[JsonSerializable(typeof(FeedbackData))]
	[JsonSerializable(typeof(SettingsSave))]
	[JsonSerializable(typeof(PrefsSave))]
	[JsonSerializable(typeof(ProfileSave))]
	[JsonSerializable(typeof(SerializableProgress))]
	[JsonSerializable(typeof(RunHistory))]
	[JsonSerializable(typeof(List<MigratingData>))]
	[JsonSerializable(typeof(List<List<PlayerMapPointHistoryEntry>>))]
	[JsonSerializable(typeof(ModManifest))]
	[JsonSerializable(typeof(JsonObject))]
	[JsonSerializable(typeof(List<JsonNode>))]
	[JsonSerializable(typeof(long))]
	[JsonSerializable(typeof(double))]
	[JsonSerializable(typeof(List<Dictionary<string, object>>))]
	[JsonSerializable(typeof(List<NullMultiplayerName>))]
	[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
	internal class MegaCritSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00018408 File Offset: 0x00016608
		[Nullable(1)]
		public static JsonSerializerOptions DefaultGeneratedSerializerOptions
		{
			[NullableContext(1)]
			get
			{
				return MegaCritSerializerContext.Default.GeneratedSerializerOptions;
			}
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00018414 File Offset: 0x00016614
		public JsonTypeInfo<bool> Boolean
		{
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

		// Token: 0x06000548 RID: 1352 RVA: 0x00018450 File Offset: 0x00016650
		[NullableContext(1)]
		private JsonTypeInfo<bool> Create_Boolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<bool> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, JsonMetadataServices.BooleanConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001847C File Offset: 0x0001667C
		public JsonTypeInfo<double> Double
		{
			get
			{
				JsonTypeInfo<double> jsonTypeInfo;
				if ((jsonTypeInfo = this._Double) == null)
				{
					jsonTypeInfo = (this._Double = (JsonTypeInfo<double>)base.Options.GetTypeInfo(typeof(double)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000184B8 File Offset: 0x000166B8
		[NullableContext(1)]
		private JsonTypeInfo<double> Create_Double(JsonSerializerOptions options)
		{
			JsonTypeInfo<double> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<double>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<double>(options, JsonMetadataServices.DoubleConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000184E4 File Offset: 0x000166E4
		public JsonTypeInfo<float> Single
		{
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

		// Token: 0x0600054C RID: 1356 RVA: 0x00018520 File Offset: 0x00016720
		[NullableContext(1)]
		private JsonTypeInfo<float> Create_Single(JsonSerializerOptions options)
		{
			JsonTypeInfo<float> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<float>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<float>(options, JsonMetadataServices.SingleConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001854C File Offset: 0x0001674C
		public JsonTypeInfo<Vector2> Vector2
		{
			get
			{
				JsonTypeInfo<Vector2> jsonTypeInfo;
				if ((jsonTypeInfo = this._Vector2) == null)
				{
					jsonTypeInfo = (this._Vector2 = (JsonTypeInfo<Vector2>)base.Options.GetTypeInfo(typeof(Vector2)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00018588 File Offset: 0x00016788
		[NullableContext(1)]
		private JsonTypeInfo<Vector2> Create_Vector2(JsonSerializerOptions options)
		{
			JsonTypeInfo<Vector2> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Vector2>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<Vector2> jsonObjectInfoValues = new JsonObjectInfoValues<Vector2>();
				jsonObjectInfoValues.ObjectCreator = () => default(Vector2);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.Vector2PropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(Vector2).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<Vector2> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Vector2>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00018650 File Offset: 0x00016850
		[NullableContext(1)]
		private static JsonPropertyInfo[] Vector2PropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<float> jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Vector2);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Vector2)obj).X;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				Unsafe.Unbox<Vector2>(obj).X = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "X";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Vector2).GetField("X", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(Vector2);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((Vector2)obj).Y;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				Unsafe.Unbox<Vector2>(obj).Y = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Y";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(Vector2).GetField("Y", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues3);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00018840 File Offset: 0x00016A40
		public JsonTypeInfo<Vector2I> Vector2I
		{
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

		// Token: 0x06000551 RID: 1361 RVA: 0x0001887C File Offset: 0x00016A7C
		[NullableContext(1)]
		private JsonTypeInfo<Vector2I> Create_Vector2I(JsonSerializerOptions options)
		{
			JsonTypeInfo<Vector2I> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Vector2I>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<Vector2I> jsonObjectInfoValues = new JsonObjectInfoValues<Vector2I>();
				jsonObjectInfoValues.ObjectCreator = () => default(Vector2I);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.Vector2IPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(Vector2I).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<Vector2I> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Vector2I>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00018944 File Offset: 0x00016B44
		[NullableContext(1)]
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

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00018B34 File Offset: 0x00016D34
		public JsonTypeInfo<ControllerMappingType> ControllerMappingType
		{
			get
			{
				JsonTypeInfo<ControllerMappingType> jsonTypeInfo;
				if ((jsonTypeInfo = this._ControllerMappingType) == null)
				{
					jsonTypeInfo = (this._ControllerMappingType = (JsonTypeInfo<ControllerMappingType>)base.Options.GetTypeInfo(typeof(ControllerMappingType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00018B70 File Offset: 0x00016D70
		[NullableContext(1)]
		private JsonTypeInfo<ControllerMappingType> Create_ControllerMappingType(JsonSerializerOptions options)
		{
			JsonTypeInfo<ControllerMappingType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ControllerMappingType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<ControllerMappingType>(options, JsonMetadataServices.GetEnumConverter<ControllerMappingType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00018B9C File Offset: 0x00016D9C
		public JsonTypeInfo<RelicRarity> RelicRarity
		{
			get
			{
				JsonTypeInfo<RelicRarity> jsonTypeInfo;
				if ((jsonTypeInfo = this._RelicRarity) == null)
				{
					jsonTypeInfo = (this._RelicRarity = (JsonTypeInfo<RelicRarity>)base.Options.GetTypeInfo(typeof(RelicRarity)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00018BD8 File Offset: 0x00016DD8
		[NullableContext(1)]
		private JsonTypeInfo<RelicRarity> Create_RelicRarity(JsonSerializerOptions options)
		{
			JsonTypeInfo<RelicRarity> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RelicRarity>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<RelicRarity>(options, JsonMetadataServices.GetEnumConverter<RelicRarity>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00018C04 File Offset: 0x00016E04
		public JsonTypeInfo<PlayerRngType> PlayerRngType
		{
			get
			{
				JsonTypeInfo<PlayerRngType> jsonTypeInfo;
				if ((jsonTypeInfo = this._PlayerRngType) == null)
				{
					jsonTypeInfo = (this._PlayerRngType = (JsonTypeInfo<PlayerRngType>)base.Options.GetTypeInfo(typeof(PlayerRngType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00018C40 File Offset: 0x00016E40
		[NullableContext(1)]
		private JsonTypeInfo<PlayerRngType> Create_PlayerRngType(JsonSerializerOptions options)
		{
			JsonTypeInfo<PlayerRngType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<PlayerRngType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<PlayerRngType>(options, JsonMetadataServices.GetEnumConverter<PlayerRngType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00018C6C File Offset: 0x00016E6C
		public JsonTypeInfo<RunRngType> RunRngType
		{
			get
			{
				JsonTypeInfo<RunRngType> jsonTypeInfo;
				if ((jsonTypeInfo = this._RunRngType) == null)
				{
					jsonTypeInfo = (this._RunRngType = (JsonTypeInfo<RunRngType>)base.Options.GetTypeInfo(typeof(RunRngType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00018CA8 File Offset: 0x00016EA8
		[NullableContext(1)]
		private JsonTypeInfo<RunRngType> Create_RunRngType(JsonSerializerOptions options)
		{
			JsonTypeInfo<RunRngType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RunRngType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<RunRngType>(options, JsonMetadataServices.GetEnumConverter<RunRngType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00018CD4 File Offset: 0x00016ED4
		public JsonTypeInfo<LocString> LocString
		{
			get
			{
				JsonTypeInfo<LocString> jsonTypeInfo;
				if ((jsonTypeInfo = this._LocString) == null)
				{
					jsonTypeInfo = (this._LocString = (JsonTypeInfo<LocString>)base.Options.GetTypeInfo(typeof(LocString)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00018D10 File Offset: 0x00016F10
		[NullableContext(1)]
		private JsonTypeInfo<LocString> Create_LocString(JsonSerializerOptions options)
		{
			JsonTypeInfo<LocString> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<LocString>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<LocString> jsonObjectInfoValues = new JsonObjectInfoValues<LocString>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new LocString((string)args[0], (string)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.LocStringPropInit(options);
				JsonObjectInfoValues<LocString> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<0>__LocStringCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<0>__LocStringCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.LocStringCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(LocString).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(string)
				}, null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<LocString> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<LocString>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00018DF4 File Offset: 0x00016FF4
		[NullableContext(1)]
		private static JsonPropertyInfo[] LocStringPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(LocString);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((LocString)obj).LocTable;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "LocTable";
			jsonPropertyInfoValues.JsonPropertyName = "table";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(LocString).GetProperty("LocTable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(LocString);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((LocString)obj).LocEntryKey;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "LocEntryKey";
			jsonPropertyInfoValues.JsonPropertyName = "key";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(LocString).GetProperty("LocEntryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(LocString);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = null;
			jsonPropertyInfoValues4.Setter = null;
			jsonPropertyInfoValues4.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "IsEmpty";
			jsonPropertyInfoValues4.JsonPropertyName = null;
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(LocString).GetProperty("IsEmpty", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues5);
			JsonPropertyInfoValues<IReadOnlyDictionary<string, object>> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<IReadOnlyDictionary<string, object>>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(LocString);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = null;
			jsonPropertyInfoValues6.Setter = null;
			jsonPropertyInfoValues6.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "Variables";
			jsonPropertyInfoValues6.JsonPropertyName = null;
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(LocString).GetProperty("Variables", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IReadOnlyDictionary<string, object>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IReadOnlyDictionary<string, object>> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<IReadOnlyDictionary<string, object>>(options, jsonPropertyInfoValues7);
			array[3].IsGetNullable = false;
			return array;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00019168 File Offset: 0x00017368
		[NullableContext(1)]
		private static JsonParameterInfoValues[] LocStringCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "locTable",
					ParameterType = typeof(string),
					Position = 0,
					HasDefaultValue = false,
					DefaultValue = null,
					IsNullable = false
				},
				new JsonParameterInfoValues
				{
					Name = "locEntryKey",
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
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00019200 File Offset: 0x00017400
		public JsonTypeInfo<MapCoord> MapCoord
		{
			get
			{
				JsonTypeInfo<MapCoord> jsonTypeInfo;
				if ((jsonTypeInfo = this._MapCoord) == null)
				{
					jsonTypeInfo = (this._MapCoord = (JsonTypeInfo<MapCoord>)base.Options.GetTypeInfo(typeof(MapCoord)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001923C File Offset: 0x0001743C
		[NullableContext(1)]
		private JsonTypeInfo<MapCoord> Create_MapCoord(JsonSerializerOptions options)
		{
			JsonTypeInfo<MapCoord> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MapCoord>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<MapCoord> jsonObjectInfoValues = new JsonObjectInfoValues<MapCoord>();
				jsonObjectInfoValues.ObjectCreator = () => default(MapCoord);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.MapCoordPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(MapCoord).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<MapCoord> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MapCoord>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00019304 File Offset: 0x00017504
		[NullableContext(1)]
		private static JsonPropertyInfo[] MapCoordPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(MapCoord);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((MapCoord)obj).col;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<MapCoord>(obj).col = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = true;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "col";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(MapCoord).GetField("col", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(MapCoord);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((MapCoord)obj).row;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<MapCoord>(obj).row = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = true;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "row";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(MapCoord).GetField("row", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues3);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000194F4 File Offset: 0x000176F4
		public JsonTypeInfo<MapPointType> MapPointType
		{
			get
			{
				JsonTypeInfo<MapPointType> jsonTypeInfo;
				if ((jsonTypeInfo = this._MapPointType) == null)
				{
					jsonTypeInfo = (this._MapPointType = (JsonTypeInfo<MapPointType>)base.Options.GetTypeInfo(typeof(MapPointType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00019530 File Offset: 0x00017730
		[NullableContext(1)]
		private JsonTypeInfo<MapPointType> Create_MapPointType(JsonSerializerOptions options)
		{
			JsonTypeInfo<MapPointType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MapPointType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MapPointType>(options, JsonMetadataServices.GetEnumConverter<MapPointType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001955C File Offset: 0x0001775C
		public JsonTypeInfo<ModDependency> ModDependency
		{
			get
			{
				JsonTypeInfo<ModDependency> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModDependency) == null)
				{
					jsonTypeInfo = (this._ModDependency = (JsonTypeInfo<ModDependency>)base.Options.GetTypeInfo(typeof(ModDependency)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00019598 File Offset: 0x00017798
		[NullableContext(1)]
		private JsonTypeInfo<ModDependency> Create_ModDependency(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModDependency> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModDependency>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModDependency> jsonObjectInfoValues = new JsonObjectInfoValues<ModDependency>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new ModDependency((string)args[0], (string)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ModDependencyPropInit(options);
				JsonObjectInfoValues<ModDependency> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<1>__ModDependencyCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<1>__ModDependencyCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.ModDependencyCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModDependency).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(string)
				}, null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ModDependency> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModDependency>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001967C File Offset: 0x0001787C
		[NullableContext(1)]
		private static JsonPropertyInfo[] ModDependencyPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModDependency);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModDependency)obj).id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModDependency)obj).id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModDependency).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModDependency);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModDependency)obj).minVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModDependency)obj).minVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "minVersion";
			jsonPropertyInfoValues.JsonPropertyName = "min_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModDependency).GetField("minVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			return array;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00019888 File Offset: 0x00017A88
		[NullableContext(1)]
		private static JsonParameterInfoValues[] ModDependencyCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "id",
					ParameterType = typeof(string),
					Position = 0,
					HasDefaultValue = false,
					DefaultValue = null,
					IsNullable = false
				},
				new JsonParameterInfoValues
				{
					Name = "minVersion",
					ParameterType = typeof(string),
					Position = 1,
					HasDefaultValue = true,
					DefaultValue = null,
					IsNullable = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00019920 File Offset: 0x00017B20
		public JsonTypeInfo<ModManifest> ModManifest
		{
			get
			{
				JsonTypeInfo<ModManifest> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModManifest) == null)
				{
					jsonTypeInfo = (this._ModManifest = (JsonTypeInfo<ModManifest>)base.Options.GetTypeInfo(typeof(ModManifest)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001995C File Offset: 0x00017B5C
		[NullableContext(1)]
		private JsonTypeInfo<ModManifest> Create_ModManifest(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModManifest> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModManifest>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModManifest> jsonObjectInfoValues = new JsonObjectInfoValues<ModManifest>();
				jsonObjectInfoValues.ObjectCreator = () => new ModManifest();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ModManifestPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModManifest).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ModManifest> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModManifest>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00019A24 File Offset: 0x00017C24
		[NullableContext(1)]
		private static JsonPropertyInfo[] ModManifestPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[10];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).author;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).author = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "author";
			jsonPropertyInfoValues.JsonPropertyName = "author";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("author", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).description;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).description = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "description";
			jsonPropertyInfoValues.JsonPropertyName = "description";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).version;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).version = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "version";
			jsonPropertyInfoValues.JsonPropertyName = "version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("version", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((ModManifest)obj).hasPck;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				((ModManifest)obj).hasPck = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "hasPck";
			jsonPropertyInfoValues7.JsonPropertyName = "has_pck";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(ModManifest).GetField("hasPck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((ModManifest)obj).hasDll;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				((ModManifest)obj).hasDll = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "hasDll";
			jsonPropertyInfoValues7.JsonPropertyName = "has_dll";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(ModManifest).GetField("hasDll", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues9 = jsonPropertyInfoValues7;
			array[6] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues9);
			JsonPropertyInfoValues<List<ModDependency>> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<List<ModDependency>>();
			jsonPropertyInfoValues10.IsProperty = false;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((ModManifest)obj).dependencies;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModDependency> value)
			{
				((ModManifest)obj).dependencies = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "dependencies";
			jsonPropertyInfoValues10.JsonPropertyName = "dependencies";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(ModManifest).GetField("dependencies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<ModDependency>> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[7] = JsonMetadataServices.CreatePropertyInfo<List<ModDependency>>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((ModManifest)obj).affectsGameplay;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				((ModManifest)obj).affectsGameplay = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "affectsGameplay";
			jsonPropertyInfoValues7.JsonPropertyName = "affects_gameplay";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(ModManifest).GetField("affectsGameplay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues12 = jsonPropertyInfoValues7;
			array[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModManifest);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModManifest)obj).minGameVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((ModManifest)obj).minGameVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "minGameVersion";
			jsonPropertyInfoValues.JsonPropertyName = "min_game_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModManifest).GetField("minGameVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues13 = jsonPropertyInfoValues;
			array[9] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues13);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001A454 File Offset: 0x00018654
		public JsonTypeInfo<ModSettings> ModSettings
		{
			get
			{
				JsonTypeInfo<ModSettings> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModSettings) == null)
				{
					jsonTypeInfo = (this._ModSettings = (JsonTypeInfo<ModSettings>)base.Options.GetTypeInfo(typeof(ModSettings)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001A490 File Offset: 0x00018690
		[NullableContext(1)]
		private JsonTypeInfo<ModSettings> Create_ModSettings(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModSettings> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModSettings>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModSettings> jsonObjectInfoValues = new JsonObjectInfoValues<ModSettings>();
				jsonObjectInfoValues.ObjectCreator = () => new ModSettings();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ModSettingsPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModSettings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ModSettings> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModSettings>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001A558 File Offset: 0x00018758
		[NullableContext(1)]
		private static JsonPropertyInfo[] ModSettingsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModSettings);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModSettings)obj).PlayerAgreedToModLoading;
			jsonPropertyInfoValues.Setter = delegate(object obj, bool value)
			{
				((ModSettings)obj).PlayerAgreedToModLoading = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "PlayerAgreedToModLoading";
			jsonPropertyInfoValues.JsonPropertyName = "mods_enabled";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModSettings).GetProperty("PlayerAgreedToModLoading", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<SettingsSaveMod>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<SettingsSaveMod>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(ModSettings);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((ModSettings)obj).ModList;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SettingsSaveMod> value)
			{
				((ModSettings)obj).ModList = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "ModList";
			jsonPropertyInfoValues3.JsonPropertyName = "mod_list";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(ModSettings).GetProperty("ModList", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SettingsSaveMod>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SettingsSaveMod>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<SettingsSaveMod>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001A774 File Offset: 0x00018974
		public JsonTypeInfo<ModSource> ModSource
		{
			get
			{
				JsonTypeInfo<ModSource> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModSource) == null)
				{
					jsonTypeInfo = (this._ModSource = (JsonTypeInfo<ModSource>)base.Options.GetTypeInfo(typeof(ModSource)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001A7B0 File Offset: 0x000189B0
		[NullableContext(1)]
		private JsonTypeInfo<ModSource> Create_ModSource(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModSource> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModSource>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<ModSource>(options, JsonMetadataServices.GetEnumConverter<ModSource>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001A7DC File Offset: 0x000189DC
		public JsonTypeInfo<SettingsSaveMod> SettingsSaveMod
		{
			get
			{
				JsonTypeInfo<SettingsSaveMod> jsonTypeInfo;
				if ((jsonTypeInfo = this._SettingsSaveMod) == null)
				{
					jsonTypeInfo = (this._SettingsSaveMod = (JsonTypeInfo<SettingsSaveMod>)base.Options.GetTypeInfo(typeof(SettingsSaveMod)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001A818 File Offset: 0x00018A18
		[NullableContext(1)]
		private JsonTypeInfo<SettingsSaveMod> Create_SettingsSaveMod(JsonSerializerOptions options)
		{
			JsonTypeInfo<SettingsSaveMod> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SettingsSaveMod>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SettingsSaveMod> jsonObjectInfoValues = new JsonObjectInfoValues<SettingsSaveMod>();
				jsonObjectInfoValues.ObjectCreator = () => new SettingsSaveMod();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SettingsSaveModPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SettingsSaveMod).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SettingsSaveMod> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SettingsSaveMod>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001A8E0 File Offset: 0x00018AE0
		[NullableContext(1)]
		private static JsonPropertyInfo[] SettingsSaveModPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsSaveMod);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsSaveMod)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((SettingsSaveMod)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsSaveMod).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<ModSource> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModSource>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SettingsSaveMod);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SettingsSaveMod)obj).Source;
			jsonPropertyInfoValues3.Setter = delegate(object obj, ModSource value)
			{
				((SettingsSaveMod)obj).Source = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Source";
			jsonPropertyInfoValues3.JsonPropertyName = "source";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SettingsSaveMod).GetProperty("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModSource), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModSource> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModSource>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SettingsSaveMod);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SettingsSaveMod)obj).IsEnabled;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((SettingsSaveMod)obj).IsEnabled = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "IsEnabled";
			jsonPropertyInfoValues5.JsonPropertyName = "is_enabled";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SettingsSaveMod).GetProperty("IsEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues6);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001AC0C File Offset: 0x00018E0C
		public JsonTypeInfo<BadgeRarity> BadgeRarity
		{
			get
			{
				JsonTypeInfo<BadgeRarity> jsonTypeInfo;
				if ((jsonTypeInfo = this._BadgeRarity) == null)
				{
					jsonTypeInfo = (this._BadgeRarity = (JsonTypeInfo<BadgeRarity>)base.Options.GetTypeInfo(typeof(BadgeRarity)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001AC48 File Offset: 0x00018E48
		[NullableContext(1)]
		private JsonTypeInfo<BadgeRarity> Create_BadgeRarity(JsonSerializerOptions options)
		{
			JsonTypeInfo<BadgeRarity> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<BadgeRarity>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<BadgeRarity>(options, JsonMetadataServices.GetEnumConverter<BadgeRarity>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001AC74 File Offset: 0x00018E74
		public JsonTypeInfo<ModelId> ModelId
		{
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

		// Token: 0x06000576 RID: 1398 RVA: 0x0001ACB0 File Offset: 0x00018EB0
		[NullableContext(1)]
		private JsonTypeInfo<ModelId> Create_ModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModelId> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModelId>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues = new JsonObjectInfoValues<ModelId>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new ModelId((string)args[0], (string)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ModelIdPropInit(options);
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<2>__ModelIdCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<2>__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.ModelIdCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModelId).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(string)
				}, null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ModelId> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModelId>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001AD94 File Offset: 0x00018F94
		[NullableContext(1)]
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

		// Token: 0x06000578 RID: 1400 RVA: 0x0001AF5C File Offset: 0x0001915C
		[NullableContext(1)]
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
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001AFF4 File Offset: 0x000191F4
		public JsonTypeInfo<FeedbackData> FeedbackData
		{
			get
			{
				JsonTypeInfo<FeedbackData> jsonTypeInfo;
				if ((jsonTypeInfo = this._FeedbackData) == null)
				{
					jsonTypeInfo = (this._FeedbackData = (JsonTypeInfo<FeedbackData>)base.Options.GetTypeInfo(typeof(FeedbackData)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001B030 File Offset: 0x00019230
		[NullableContext(1)]
		private JsonTypeInfo<FeedbackData> Create_FeedbackData(JsonSerializerOptions options)
		{
			JsonTypeInfo<FeedbackData> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<FeedbackData>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<FeedbackData> jsonObjectInfoValues = new JsonObjectInfoValues<FeedbackData>();
				jsonObjectInfoValues.ObjectCreator = () => default(FeedbackData);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.FeedbackDataPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(FeedbackData).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<FeedbackData> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<FeedbackData>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001B0F8 File Offset: 0x000192F8
		[NullableContext(1)]
		private static JsonPropertyInfo[] FeedbackDataPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[10];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).description;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).description = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "description";
			jsonPropertyInfoValues.JsonPropertyName = "description";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).category;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).category = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "category";
			jsonPropertyInfoValues.JsonPropertyName = "category";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("category", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).gameVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).gameVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "gameVersion";
			jsonPropertyInfoValues.JsonPropertyName = "game_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("gameVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).uniqueId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).uniqueId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "uniqueId";
			jsonPropertyInfoValues.JsonPropertyName = "unique_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("uniqueId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).commit;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).commit = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "commit";
			jsonPropertyInfoValues.JsonPropertyName = "commit";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("commit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).platformBranch;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).platformBranch = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "platformBranch";
			jsonPropertyInfoValues.JsonPropertyName = "platform_branch";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("platformBranch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).sessionId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).sessionId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "sessionId";
			jsonPropertyInfoValues.JsonPropertyName = "session_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("sessionId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues8 = jsonPropertyInfoValues;
			array[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues8);
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = false;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((FeedbackData)obj).isModded;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<FeedbackData>(obj).isModded = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "isModded";
			jsonPropertyInfoValues9.JsonPropertyName = "is_modded";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(FeedbackData).GetField("isModded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues10);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = false;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((FeedbackData)obj).isFullConsole;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<FeedbackData>(obj).isFullConsole = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "isFullConsole";
			jsonPropertyInfoValues9.JsonPropertyName = "is_full_console";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(FeedbackData).GetField("isFullConsole", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues11 = jsonPropertyInfoValues9;
			array[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FeedbackData);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FeedbackData)obj).lang;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<FeedbackData>(obj).lang = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "lang";
			jsonPropertyInfoValues.JsonPropertyName = "lang";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FeedbackData).GetField("lang", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues12 = jsonPropertyInfoValues;
			array[9] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues12);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001BBA8 File Offset: 0x00019DA8
		public JsonTypeInfo<NullLeaderboard> NullLeaderboard
		{
			get
			{
				JsonTypeInfo<NullLeaderboard> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullLeaderboard) == null)
				{
					jsonTypeInfo = (this._NullLeaderboard = (JsonTypeInfo<NullLeaderboard>)base.Options.GetTypeInfo(typeof(NullLeaderboard)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001BBE4 File Offset: 0x00019DE4
		[NullableContext(1)]
		private JsonTypeInfo<NullLeaderboard> Create_NullLeaderboard(JsonSerializerOptions options)
		{
			JsonTypeInfo<NullLeaderboard> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<NullLeaderboard>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<NullLeaderboard> jsonObjectInfoValues = new JsonObjectInfoValues<NullLeaderboard>();
				jsonObjectInfoValues.ObjectCreator = () => new NullLeaderboard();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.NullLeaderboardPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(NullLeaderboard).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<NullLeaderboard> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<NullLeaderboard>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001BCAC File Offset: 0x00019EAC
		[NullableContext(1)]
		private static JsonPropertyInfo[] NullLeaderboardPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NullLeaderboard);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NullLeaderboard)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((NullLeaderboard)obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NullLeaderboard).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<NullLeaderboardFileEntry>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<NullLeaderboardFileEntry>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(NullLeaderboard);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((NullLeaderboard)obj).entries;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<NullLeaderboardFileEntry> value)
			{
				((NullLeaderboard)obj).entries = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "entries";
			jsonPropertyInfoValues3.JsonPropertyName = "entries";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(NullLeaderboard).GetField("entries", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<NullLeaderboardFileEntry>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<NullLeaderboardFileEntry>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public JsonTypeInfo<NullLeaderboardFile> NullLeaderboardFile
		{
			get
			{
				JsonTypeInfo<NullLeaderboardFile> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullLeaderboardFile) == null)
				{
					jsonTypeInfo = (this._NullLeaderboardFile = (JsonTypeInfo<NullLeaderboardFile>)base.Options.GetTypeInfo(typeof(NullLeaderboardFile)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001BF14 File Offset: 0x0001A114
		[NullableContext(1)]
		private JsonTypeInfo<NullLeaderboardFile> Create_NullLeaderboardFile(JsonSerializerOptions options)
		{
			JsonTypeInfo<NullLeaderboardFile> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<NullLeaderboardFile>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<NullLeaderboardFile> jsonObjectInfoValues = new JsonObjectInfoValues<NullLeaderboardFile>();
				jsonObjectInfoValues.ObjectCreator = () => new NullLeaderboardFile();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.NullLeaderboardFilePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(NullLeaderboardFile).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<NullLeaderboardFile> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<NullLeaderboardFile>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001BFDC File Offset: 0x0001A1DC
		[NullableContext(1)]
		private static JsonPropertyInfo[] NullLeaderboardFilePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NullLeaderboardFile);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NullLeaderboardFile)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((NullLeaderboardFile)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NullLeaderboardFile).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<NullLeaderboard>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<NullLeaderboard>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(NullLeaderboardFile);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((NullLeaderboardFile)obj).leaderboards;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<NullLeaderboard> value)
			{
				((NullLeaderboardFile)obj).leaderboards = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "leaderboards";
			jsonPropertyInfoValues3.JsonPropertyName = "leaderboards";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(NullLeaderboardFile).GetField("leaderboards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<NullLeaderboard>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<NullLeaderboard>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001C1F8 File Offset: 0x0001A3F8
		public JsonTypeInfo<NullLeaderboardFileEntry> NullLeaderboardFileEntry
		{
			get
			{
				JsonTypeInfo<NullLeaderboardFileEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullLeaderboardFileEntry) == null)
				{
					jsonTypeInfo = (this._NullLeaderboardFileEntry = (JsonTypeInfo<NullLeaderboardFileEntry>)base.Options.GetTypeInfo(typeof(NullLeaderboardFileEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001C234 File Offset: 0x0001A434
		[NullableContext(1)]
		private JsonTypeInfo<NullLeaderboardFileEntry> Create_NullLeaderboardFileEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<NullLeaderboardFileEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<NullLeaderboardFileEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<NullLeaderboardFileEntry> jsonObjectInfoValues = new JsonObjectInfoValues<NullLeaderboardFileEntry>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new NullLeaderboardFileEntry
				{
					name = (string)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.NullLeaderboardFileEntryPropInit(options);
				JsonObjectInfoValues<NullLeaderboardFileEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<3>__NullLeaderboardFileEntryCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<3>__NullLeaderboardFileEntryCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.NullLeaderboardFileEntryCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(NullLeaderboardFileEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<NullLeaderboardFileEntry> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<NullLeaderboardFileEntry>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001C318 File Offset: 0x0001A518
		[NullableContext(1)]
		private static JsonPropertyInfo[] NullLeaderboardFileEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NullLeaderboardFileEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NullLeaderboardFileEntry)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((NullLeaderboardFileEntry)obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = "name";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NullLeaderboardFileEntry).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(NullLeaderboardFileEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((NullLeaderboardFileEntry)obj).score;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((NullLeaderboardFileEntry)obj).score = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "score";
			jsonPropertyInfoValues3.JsonPropertyName = "score";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(NullLeaderboardFileEntry).GetField("score", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues5.IsProperty = false;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(NullLeaderboardFileEntry);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((NullLeaderboardFileEntry)obj).id;
			jsonPropertyInfoValues5.Setter = delegate(object obj, ulong value)
			{
				((NullLeaderboardFileEntry)obj).id = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "id";
			jsonPropertyInfoValues5.JsonPropertyName = "id";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(NullLeaderboardFileEntry).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<List<ulong>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<List<ulong>>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(NullLeaderboardFileEntry);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((NullLeaderboardFileEntry)obj).userIds;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(2)] List<ulong> value)
			{
				((NullLeaderboardFileEntry)obj).userIds = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "userIds";
			jsonPropertyInfoValues7.JsonPropertyName = "other_ids";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(NullLeaderboardFileEntry).GetField("userIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<ulong>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<List<ulong>>(options, jsonPropertyInfoValues8);
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			return array;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001C760 File Offset: 0x0001A960
		[NullableContext(1)]
		private static JsonParameterInfoValues[] NullLeaderboardFileEntryCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "name",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001C7B0 File Offset: 0x0001A9B0
		public JsonTypeInfo<NullMultiplayerName> NullMultiplayerName
		{
			get
			{
				JsonTypeInfo<NullMultiplayerName> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullMultiplayerName) == null)
				{
					jsonTypeInfo = (this._NullMultiplayerName = (JsonTypeInfo<NullMultiplayerName>)base.Options.GetTypeInfo(typeof(NullMultiplayerName)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001C7EC File Offset: 0x0001A9EC
		[NullableContext(1)]
		private JsonTypeInfo<NullMultiplayerName> Create_NullMultiplayerName(JsonSerializerOptions options)
		{
			JsonTypeInfo<NullMultiplayerName> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<NullMultiplayerName>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<NullMultiplayerName> jsonObjectInfoValues = new JsonObjectInfoValues<NullMultiplayerName>();
				jsonObjectInfoValues.ObjectCreator = () => new NullMultiplayerName();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.NullMultiplayerNamePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(NullMultiplayerName).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<NullMultiplayerName> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<NullMultiplayerName>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		[NullableContext(1)]
		private static JsonPropertyInfo[] NullMultiplayerNamePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(NullMultiplayerName);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((NullMultiplayerName)obj).netId;
			jsonPropertyInfoValues.Setter = delegate(object obj, ulong value)
			{
				((NullMultiplayerName)obj).netId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "netId";
			jsonPropertyInfoValues.JsonPropertyName = "net_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(NullMultiplayerName).GetField("netId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(NullMultiplayerName);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((NullMultiplayerName)obj).name;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((NullMultiplayerName)obj).name = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "name";
			jsonPropertyInfoValues3.JsonPropertyName = "name";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(NullMultiplayerName).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001CAD0 File Offset: 0x0001ACD0
		public JsonTypeInfo<PlatformType> PlatformType
		{
			get
			{
				JsonTypeInfo<PlatformType> jsonTypeInfo;
				if ((jsonTypeInfo = this._PlatformType) == null)
				{
					jsonTypeInfo = (this._PlatformType = (JsonTypeInfo<PlatformType>)base.Options.GetTypeInfo(typeof(PlatformType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		[NullableContext(1)]
		private JsonTypeInfo<PlatformType> Create_PlatformType(JsonSerializerOptions options)
		{
			JsonTypeInfo<PlatformType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<PlatformType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<PlatformType>(options, JsonMetadataServices.GetEnumConverter<PlatformType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001CB38 File Offset: 0x0001AD38
		public JsonTypeInfo<RewardType> RewardType
		{
			get
			{
				JsonTypeInfo<RewardType> jsonTypeInfo;
				if ((jsonTypeInfo = this._RewardType) == null)
				{
					jsonTypeInfo = (this._RewardType = (JsonTypeInfo<RewardType>)base.Options.GetTypeInfo(typeof(RewardType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001CB74 File Offset: 0x0001AD74
		[NullableContext(1)]
		private JsonTypeInfo<RewardType> Create_RewardType(JsonSerializerOptions options)
		{
			JsonTypeInfo<RewardType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RewardType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<RewardType>(options, JsonMetadataServices.GetEnumConverter<RewardType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001CBA0 File Offset: 0x0001ADA0
		public JsonTypeInfo<RoomType> RoomType
		{
			get
			{
				JsonTypeInfo<RoomType> jsonTypeInfo;
				if ((jsonTypeInfo = this._RoomType) == null)
				{
					jsonTypeInfo = (this._RoomType = (JsonTypeInfo<RoomType>)base.Options.GetTypeInfo(typeof(RoomType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001CBDC File Offset: 0x0001ADDC
		[NullableContext(1)]
		private JsonTypeInfo<RoomType> Create_RoomType(JsonSerializerOptions options)
		{
			JsonTypeInfo<RoomType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RoomType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<RoomType>(options, JsonMetadataServices.GetEnumConverter<RoomType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001CC08 File Offset: 0x0001AE08
		public JsonTypeInfo<CardCreationSource> CardCreationSource
		{
			get
			{
				JsonTypeInfo<CardCreationSource> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardCreationSource) == null)
				{
					jsonTypeInfo = (this._CardCreationSource = (JsonTypeInfo<CardCreationSource>)base.Options.GetTypeInfo(typeof(CardCreationSource)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001CC44 File Offset: 0x0001AE44
		[NullableContext(1)]
		private JsonTypeInfo<CardCreationSource> Create_CardCreationSource(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardCreationSource> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardCreationSource>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<CardCreationSource>(options, JsonMetadataServices.GetEnumConverter<CardCreationSource>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001CC70 File Offset: 0x0001AE70
		public JsonTypeInfo<CardRarityOddsType> CardRarityOddsType
		{
			get
			{
				JsonTypeInfo<CardRarityOddsType> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardRarityOddsType) == null)
				{
					jsonTypeInfo = (this._CardRarityOddsType = (JsonTypeInfo<CardRarityOddsType>)base.Options.GetTypeInfo(typeof(CardRarityOddsType)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001CCAC File Offset: 0x0001AEAC
		[NullableContext(1)]
		private JsonTypeInfo<CardRarityOddsType> Create_CardRarityOddsType(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardRarityOddsType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardRarityOddsType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<CardRarityOddsType>(options, JsonMetadataServices.GetEnumConverter<CardRarityOddsType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001CCD8 File Offset: 0x0001AED8
		public JsonTypeInfo<GameMode> GameMode
		{
			get
			{
				JsonTypeInfo<GameMode> jsonTypeInfo;
				if ((jsonTypeInfo = this._GameMode) == null)
				{
					jsonTypeInfo = (this._GameMode = (JsonTypeInfo<GameMode>)base.Options.GetTypeInfo(typeof(GameMode)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001CD14 File Offset: 0x0001AF14
		[NullableContext(1)]
		private JsonTypeInfo<GameMode> Create_GameMode(JsonSerializerOptions options)
		{
			JsonTypeInfo<GameMode> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<GameMode>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<GameMode>(options, JsonMetadataServices.GetEnumConverter<GameMode>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001CD40 File Offset: 0x0001AF40
		public JsonTypeInfo<AncientChoiceHistoryEntry> AncientChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<AncientChoiceHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._AncientChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._AncientChoiceHistoryEntry = (JsonTypeInfo<AncientChoiceHistoryEntry>)base.Options.GetTypeInfo(typeof(AncientChoiceHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001CD7C File Offset: 0x0001AF7C
		[NullableContext(1)]
		private JsonTypeInfo<AncientChoiceHistoryEntry> Create_AncientChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<AncientChoiceHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AncientChoiceHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AncientChoiceHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<AncientChoiceHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => new AncientChoiceHistoryEntry();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.AncientChoiceHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AncientChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<AncientChoiceHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AncientChoiceHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001CE44 File Offset: 0x0001B044
		[NullableContext(1)]
		private static JsonPropertyInfo[] AncientChoiceHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<LocString> jsonPropertyInfoValues = new JsonPropertyInfoValues<LocString>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientChoiceHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientChoiceHistoryEntry)obj).Title;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] LocString value)
			{
				((AncientChoiceHistoryEntry)obj).Title = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Title";
			jsonPropertyInfoValues.JsonPropertyName = "title";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientChoiceHistoryEntry).GetProperty("Title", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(LocString), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<LocString> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<LocString>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientChoiceHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((AncientChoiceHistoryEntry)obj).WasChosen;
			jsonPropertyInfoValues3.Setter = delegate(object obj, bool value)
			{
				((AncientChoiceHistoryEntry)obj).WasChosen = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "WasChosen";
			jsonPropertyInfoValues3.JsonPropertyName = "was_chosen";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientChoiceHistoryEntry).GetProperty("WasChosen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(AncientChoiceHistoryEntry);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((AncientChoiceHistoryEntry)obj).TextKey;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TextKey";
			jsonPropertyInfoValues5.JsonPropertyName = "TextKey";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(AncientChoiceHistoryEntry).GetProperty("TextKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001D15C File Offset: 0x0001B35C
		public JsonTypeInfo<CardChoiceHistoryEntry> CardChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<CardChoiceHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._CardChoiceHistoryEntry = (JsonTypeInfo<CardChoiceHistoryEntry>)base.Options.GetTypeInfo(typeof(CardChoiceHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001D198 File Offset: 0x0001B398
		[NullableContext(1)]
		private JsonTypeInfo<CardChoiceHistoryEntry> Create_CardChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardChoiceHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardChoiceHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardChoiceHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<CardChoiceHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => default(CardChoiceHistoryEntry);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.CardChoiceHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<CardChoiceHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardChoiceHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001D260 File Offset: 0x0001B460
		[NullableContext(1)]
		private static JsonPropertyInfo[] CardChoiceHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardChoiceHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardChoiceHistoryEntry)obj).Card;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				Unsafe.Unbox<CardChoiceHistoryEntry>(obj).Card = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Card";
			jsonPropertyInfoValues.JsonPropertyName = "card";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardChoiceHistoryEntry).GetProperty("Card", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableCard), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardChoiceHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardChoiceHistoryEntry)obj).wasPicked;
			jsonPropertyInfoValues3.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<CardChoiceHistoryEntry>(obj).wasPicked = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "wasPicked";
			jsonPropertyInfoValues3.JsonPropertyName = "was_picked";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardChoiceHistoryEntry).GetField("wasPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001D47C File Offset: 0x0001B67C
		public JsonTypeInfo<CardEnchantmentHistoryEntry> CardEnchantmentHistoryEntry
		{
			get
			{
				JsonTypeInfo<CardEnchantmentHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardEnchantmentHistoryEntry) == null)
				{
					jsonTypeInfo = (this._CardEnchantmentHistoryEntry = (JsonTypeInfo<CardEnchantmentHistoryEntry>)base.Options.GetTypeInfo(typeof(CardEnchantmentHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001D4B8 File Offset: 0x0001B6B8
		[NullableContext(1)]
		private JsonTypeInfo<CardEnchantmentHistoryEntry> Create_CardEnchantmentHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardEnchantmentHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardEnchantmentHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardEnchantmentHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<CardEnchantmentHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => default(CardEnchantmentHistoryEntry);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.CardEnchantmentHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardEnchantmentHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<CardEnchantmentHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardEnchantmentHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001D580 File Offset: 0x0001B780
		[NullableContext(1)]
		private static JsonPropertyInfo[] CardEnchantmentHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardEnchantmentHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardEnchantmentHistoryEntry)obj).Card;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				Unsafe.Unbox<CardEnchantmentHistoryEntry>(obj).Card = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Card";
			jsonPropertyInfoValues.JsonPropertyName = "card";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardEnchantmentHistoryEntry).GetProperty("Card", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableCard), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardEnchantmentHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardEnchantmentHistoryEntry)obj).Enchantment;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				Unsafe.Unbox<CardEnchantmentHistoryEntry>(obj).Enchantment = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Enchantment";
			jsonPropertyInfoValues3.JsonPropertyName = "enchantment";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardEnchantmentHistoryEntry).GetProperty("Enchantment", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		public JsonTypeInfo<CardTransformationHistoryEntry> CardTransformationHistoryEntry
		{
			get
			{
				JsonTypeInfo<CardTransformationHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardTransformationHistoryEntry) == null)
				{
					jsonTypeInfo = (this._CardTransformationHistoryEntry = (JsonTypeInfo<CardTransformationHistoryEntry>)base.Options.GetTypeInfo(typeof(CardTransformationHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001D7E8 File Offset: 0x0001B9E8
		[NullableContext(1)]
		private JsonTypeInfo<CardTransformationHistoryEntry> Create_CardTransformationHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardTransformationHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardTransformationHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardTransformationHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<CardTransformationHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => default(CardTransformationHistoryEntry);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.CardTransformationHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardTransformationHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<CardTransformationHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardTransformationHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
		[NullableContext(1)]
		private static JsonPropertyInfo[] CardTransformationHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardTransformationHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardTransformationHistoryEntry)obj).OriginalCard;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				Unsafe.Unbox<CardTransformationHistoryEntry>(obj).OriginalCard = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "OriginalCard";
			jsonPropertyInfoValues.JsonPropertyName = "original_card";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardTransformationHistoryEntry).GetProperty("OriginalCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableCard), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardTransformationHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardTransformationHistoryEntry)obj).FinalCard;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				Unsafe.Unbox<CardTransformationHistoryEntry>(obj).FinalCard = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "FinalCard";
			jsonPropertyInfoValues.JsonPropertyName = "final_card";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardTransformationHistoryEntry).GetProperty("FinalCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableCard), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues3);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001DACC File Offset: 0x0001BCCC
		public JsonTypeInfo<EventOptionHistoryEntry> EventOptionHistoryEntry
		{
			get
			{
				JsonTypeInfo<EventOptionHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._EventOptionHistoryEntry) == null)
				{
					jsonTypeInfo = (this._EventOptionHistoryEntry = (JsonTypeInfo<EventOptionHistoryEntry>)base.Options.GetTypeInfo(typeof(EventOptionHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001DB08 File Offset: 0x0001BD08
		[NullableContext(1)]
		private JsonTypeInfo<EventOptionHistoryEntry> Create_EventOptionHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<EventOptionHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EventOptionHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EventOptionHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<EventOptionHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => default(EventOptionHistoryEntry);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.EventOptionHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EventOptionHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<EventOptionHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EventOptionHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001DBD0 File Offset: 0x0001BDD0
		[NullableContext(1)]
		private static JsonPropertyInfo[] EventOptionHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<LocString> jsonPropertyInfoValues = new JsonPropertyInfoValues<LocString>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EventOptionHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EventOptionHistoryEntry)obj).Title;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] LocString value)
			{
				Unsafe.Unbox<EventOptionHistoryEntry>(obj).Title = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Title";
			jsonPropertyInfoValues.JsonPropertyName = "title";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EventOptionHistoryEntry).GetProperty("Title", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(LocString), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<LocString> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<LocString>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<Dictionary<string, object>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<Dictionary<string, object>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(EventOptionHistoryEntry);
			jsonPropertyInfoValues3.Converter = (JsonConverter<Dictionary<string, object>>)MegaCritSerializerContext.ExpandConverter(typeof(Dictionary<string, object>), new LocStringVariablesJsonConverter(), options, true);
			jsonPropertyInfoValues3.Getter = (object obj) => ((EventOptionHistoryEntry)obj).Variables;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<string, object> value)
			{
				Unsafe.Unbox<EventOptionHistoryEntry>(obj).Variables = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = true;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Variables";
			jsonPropertyInfoValues3.JsonPropertyName = "variables";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(EventOptionHistoryEntry).GetProperty("Variables", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<string, object>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<string, object>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, object>>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001DE04 File Offset: 0x0001C004
		public JsonTypeInfo<MapPointHistoryEntry> MapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<MapPointHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._MapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._MapPointHistoryEntry = (JsonTypeInfo<MapPointHistoryEntry>)base.Options.GetTypeInfo(typeof(MapPointHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001DE40 File Offset: 0x0001C040
		[NullableContext(1)]
		private JsonTypeInfo<MapPointHistoryEntry> Create_MapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<MapPointHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MapPointHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<MapPointHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<MapPointHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => new MapPointHistoryEntry();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.MapPointHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(MapPointHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<MapPointHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MapPointHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001DF08 File Offset: 0x0001C108
		[NullableContext(1)]
		private static JsonPropertyInfo[] MapPointHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<MapPointType> jsonPropertyInfoValues = new JsonPropertyInfoValues<MapPointType>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(MapPointHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((MapPointHistoryEntry)obj).MapPointType;
			jsonPropertyInfoValues.Setter = delegate(object obj, MapPointType value)
			{
				((MapPointHistoryEntry)obj).MapPointType = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "MapPointType";
			jsonPropertyInfoValues.JsonPropertyName = "map_point_type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(MapPointHistoryEntry).GetProperty("MapPointType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(MapPointType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<MapPointType> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<MapPointType>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<MapPointRoomHistoryEntry>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<MapPointRoomHistoryEntry>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(MapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((MapPointHistoryEntry)obj).Rooms;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<MapPointRoomHistoryEntry> value)
			{
				((MapPointHistoryEntry)obj).Rooms = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Rooms";
			jsonPropertyInfoValues3.JsonPropertyName = "rooms";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(MapPointHistoryEntry).GetProperty("Rooms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<MapPointRoomHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<MapPointRoomHistoryEntry>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<MapPointRoomHistoryEntry>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<List<PlayerMapPointHistoryEntry>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<List<PlayerMapPointHistoryEntry>>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(MapPointHistoryEntry);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((MapPointHistoryEntry)obj).PlayerStats;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<PlayerMapPointHistoryEntry> value)
			{
				((MapPointHistoryEntry)obj).PlayerStats = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "PlayerStats";
			jsonPropertyInfoValues5.JsonPropertyName = "player_stats";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(MapPointHistoryEntry).GetProperty("PlayerStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<PlayerMapPointHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<PlayerMapPointHistoryEntry>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<PlayerMapPointHistoryEntry>>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001E244 File Offset: 0x0001C444
		public JsonTypeInfo<MapPointRoomHistoryEntry> MapPointRoomHistoryEntry
		{
			get
			{
				JsonTypeInfo<MapPointRoomHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._MapPointRoomHistoryEntry) == null)
				{
					jsonTypeInfo = (this._MapPointRoomHistoryEntry = (JsonTypeInfo<MapPointRoomHistoryEntry>)base.Options.GetTypeInfo(typeof(MapPointRoomHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001E280 File Offset: 0x0001C480
		[NullableContext(1)]
		private JsonTypeInfo<MapPointRoomHistoryEntry> Create_MapPointRoomHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<MapPointRoomHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MapPointRoomHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<MapPointRoomHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<MapPointRoomHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => new MapPointRoomHistoryEntry();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.MapPointRoomHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(MapPointRoomHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<MapPointRoomHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MapPointRoomHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001E348 File Offset: 0x0001C548
		[NullableContext(1)]
		private static JsonPropertyInfo[] MapPointRoomHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<RoomType> jsonPropertyInfoValues = new JsonPropertyInfoValues<RoomType>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(MapPointRoomHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((MapPointRoomHistoryEntry)obj).RoomType;
			jsonPropertyInfoValues.Setter = delegate(object obj, RoomType value)
			{
				((MapPointRoomHistoryEntry)obj).RoomType = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "RoomType";
			jsonPropertyInfoValues.JsonPropertyName = "room_type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(MapPointRoomHistoryEntry).GetProperty("RoomType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(RoomType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<RoomType> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<RoomType>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(MapPointRoomHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((MapPointRoomHistoryEntry)obj).ModelId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((MapPointRoomHistoryEntry)obj).ModelId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "ModelId";
			jsonPropertyInfoValues3.JsonPropertyName = "model_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(MapPointRoomHistoryEntry).GetProperty("ModelId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(MapPointRoomHistoryEntry);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((MapPointRoomHistoryEntry)obj).MonsterIds;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((MapPointRoomHistoryEntry)obj).MonsterIds = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "MonsterIds";
			jsonPropertyInfoValues5.JsonPropertyName = "monster_ids";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(MapPointRoomHistoryEntry).GetProperty("MonsterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(MapPointRoomHistoryEntry);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((MapPointRoomHistoryEntry)obj).TurnsTaken;
			jsonPropertyInfoValues7.Setter = delegate(object obj, int value)
			{
				((MapPointRoomHistoryEntry)obj).TurnsTaken = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "TurnsTaken";
			jsonPropertyInfoValues7.JsonPropertyName = "turns_taken";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(MapPointRoomHistoryEntry).GetProperty("TurnsTaken", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001E778 File Offset: 0x0001C978
		public JsonTypeInfo<ModelChoiceHistoryEntry> ModelChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<ModelChoiceHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._ModelChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ModelChoiceHistoryEntry = (JsonTypeInfo<ModelChoiceHistoryEntry>)base.Options.GetTypeInfo(typeof(ModelChoiceHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		[NullableContext(1)]
		private JsonTypeInfo<ModelChoiceHistoryEntry> Create_ModelChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<ModelChoiceHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ModelChoiceHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ModelChoiceHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<ModelChoiceHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => default(ModelChoiceHistoryEntry);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ModelChoiceHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ModelChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ModelChoiceHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ModelChoiceHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001E87C File Offset: 0x0001CA7C
		[NullableContext(1)]
		private static JsonPropertyInfo[] ModelChoiceHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ModelChoiceHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ModelChoiceHistoryEntry)obj).choice;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				Unsafe.Unbox<ModelChoiceHistoryEntry>(obj).choice = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "choice";
			jsonPropertyInfoValues.JsonPropertyName = "choice";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ModelChoiceHistoryEntry).GetField("choice", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(ModelChoiceHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((ModelChoiceHistoryEntry)obj).wasPicked;
			jsonPropertyInfoValues3.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<ModelChoiceHistoryEntry>(obj).wasPicked = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "wasPicked";
			jsonPropertyInfoValues3.JsonPropertyName = "was_picked";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(ModelChoiceHistoryEntry).GetField("wasPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001EA98 File Offset: 0x0001CC98
		public JsonTypeInfo<PlayerMapPointHistoryEntry> PlayerMapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<PlayerMapPointHistoryEntry> jsonTypeInfo;
				if ((jsonTypeInfo = this._PlayerMapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._PlayerMapPointHistoryEntry = (JsonTypeInfo<PlayerMapPointHistoryEntry>)base.Options.GetTypeInfo(typeof(PlayerMapPointHistoryEntry)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		[NullableContext(1)]
		private JsonTypeInfo<PlayerMapPointHistoryEntry> Create_PlayerMapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<PlayerMapPointHistoryEntry> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<PlayerMapPointHistoryEntry>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<PlayerMapPointHistoryEntry> jsonObjectInfoValues = new JsonObjectInfoValues<PlayerMapPointHistoryEntry>();
				jsonObjectInfoValues.ObjectCreator = () => new PlayerMapPointHistoryEntry();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.PlayerMapPointHistoryEntryPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<PlayerMapPointHistoryEntry> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<PlayerMapPointHistoryEntry>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001EB9C File Offset: 0x0001CD9C
		[NullableContext(1)]
		private static JsonPropertyInfo[] PlayerMapPointHistoryEntryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[34];
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).PlayerId;
			jsonPropertyInfoValues.Setter = delegate(object obj, ulong value)
			{
				((PlayerMapPointHistoryEntry)obj).PlayerId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "PlayerId";
			jsonPropertyInfoValues.JsonPropertyName = "player_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("PlayerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ulong), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).GoldGained;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).GoldGained = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "GoldGained";
			jsonPropertyInfoValues3.JsonPropertyName = "gold_gained";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("GoldGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).GoldSpent;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).GoldSpent = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "GoldSpent";
			jsonPropertyInfoValues3.JsonPropertyName = "gold_spent";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("GoldSpent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).GoldLost;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).GoldLost = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "GoldLost";
			jsonPropertyInfoValues3.JsonPropertyName = "gold_lost";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("GoldLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).GoldStolen;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).GoldStolen = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "GoldStolen";
			jsonPropertyInfoValues3.JsonPropertyName = "gold_stolen";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("GoldStolen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues3;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).StolenLoot;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).StolenLoot = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "StolenLoot";
			jsonPropertyInfoValues3.JsonPropertyName = "stolen_loot";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("StolenLoot", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues3;
			array[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CurrentGold;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).CurrentGold = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CurrentGold";
			jsonPropertyInfoValues3.JsonPropertyName = "current_gold";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CurrentGold", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = jsonPropertyInfoValues3;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues9);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CurrentHp;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).CurrentHp = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CurrentHp";
			jsonPropertyInfoValues3.JsonPropertyName = "current_hp";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CurrentHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues10 = jsonPropertyInfoValues3;
			array[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues10);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).MaxHp;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).MaxHp = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxHp";
			jsonPropertyInfoValues3.JsonPropertyName = "max_hp";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("MaxHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues11 = jsonPropertyInfoValues3;
			array[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).DamageTaken;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).DamageTaken = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "DamageTaken";
			jsonPropertyInfoValues3.JsonPropertyName = "damage_taken";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("DamageTaken", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues12 = jsonPropertyInfoValues3;
			array[9] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).HpHealed;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).HpHealed = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "HpHealed";
			jsonPropertyInfoValues3.JsonPropertyName = "hp_healed";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("HpHealed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues13 = jsonPropertyInfoValues3;
			array[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues13);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).MaxHpLost;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).MaxHpLost = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxHpLost";
			jsonPropertyInfoValues3.JsonPropertyName = "max_hp_lost";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("MaxHpLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues14 = jsonPropertyInfoValues3;
			array[11] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues14);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).MaxHpGained;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((PlayerMapPointHistoryEntry)obj).MaxHpGained = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxHpGained";
			jsonPropertyInfoValues3.JsonPropertyName = "max_hp_gained";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("MaxHpGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues15 = jsonPropertyInfoValues3;
			array[12] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues15);
			JsonPropertyInfoValues<List<AncientChoiceHistoryEntry>> jsonPropertyInfoValues16 = new JsonPropertyInfoValues<List<AncientChoiceHistoryEntry>>();
			jsonPropertyInfoValues16.IsProperty = true;
			jsonPropertyInfoValues16.IsPublic = true;
			jsonPropertyInfoValues16.IsVirtual = false;
			jsonPropertyInfoValues16.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues16.Converter = null;
			jsonPropertyInfoValues16.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).AncientChoices;
			jsonPropertyInfoValues16.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<AncientChoiceHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).AncientChoices = value;
			};
			jsonPropertyInfoValues16.IgnoreCondition = null;
			jsonPropertyInfoValues16.HasJsonInclude = false;
			jsonPropertyInfoValues16.IsExtensionData = false;
			jsonPropertyInfoValues16.NumberHandling = null;
			jsonPropertyInfoValues16.PropertyName = "AncientChoices";
			jsonPropertyInfoValues16.JsonPropertyName = "ancient_choice";
			jsonPropertyInfoValues16.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("AncientChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<AncientChoiceHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<AncientChoiceHistoryEntry>> jsonPropertyInfoValues17 = jsonPropertyInfoValues16;
			array[13] = JsonMetadataServices.CreatePropertyInfo<List<AncientChoiceHistoryEntry>>(options, jsonPropertyInfoValues17);
			array[13].IsGetNullable = false;
			array[13].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializableCard>> jsonPropertyInfoValues18 = new JsonPropertyInfoValues<List<SerializableCard>>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CardsGained;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableCard> value)
			{
				((PlayerMapPointHistoryEntry)obj).CardsGained = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "CardsGained";
			jsonPropertyInfoValues18.JsonPropertyName = "cards_gained";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CardsGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableCard>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableCard>> jsonPropertyInfoValues19 = jsonPropertyInfoValues18;
			array[14] = JsonMetadataServices.CreatePropertyInfo<List<SerializableCard>>(options, jsonPropertyInfoValues19);
			array[14].IsGetNullable = false;
			array[14].IsSetNullable = false;
			JsonPropertyInfoValues<List<CardChoiceHistoryEntry>> jsonPropertyInfoValues20 = new JsonPropertyInfoValues<List<CardChoiceHistoryEntry>>();
			jsonPropertyInfoValues20.IsProperty = true;
			jsonPropertyInfoValues20.IsPublic = true;
			jsonPropertyInfoValues20.IsVirtual = false;
			jsonPropertyInfoValues20.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues20.Converter = null;
			jsonPropertyInfoValues20.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CardChoices;
			jsonPropertyInfoValues20.Setter = delegate(object obj, [Nullable(2)] List<CardChoiceHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).CardChoices = value;
			};
			jsonPropertyInfoValues20.IgnoreCondition = null;
			jsonPropertyInfoValues20.HasJsonInclude = false;
			jsonPropertyInfoValues20.IsExtensionData = false;
			jsonPropertyInfoValues20.NumberHandling = null;
			jsonPropertyInfoValues20.PropertyName = "CardChoices";
			jsonPropertyInfoValues20.JsonPropertyName = "card_choices";
			jsonPropertyInfoValues20.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CardChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CardChoiceHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CardChoiceHistoryEntry>> jsonPropertyInfoValues21 = jsonPropertyInfoValues20;
			array[15] = JsonMetadataServices.CreatePropertyInfo<List<CardChoiceHistoryEntry>>(options, jsonPropertyInfoValues21);
			array[15].IsGetNullable = false;
			array[15].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelChoiceHistoryEntry>> jsonPropertyInfoValues22 = new JsonPropertyInfoValues<List<ModelChoiceHistoryEntry>>();
			jsonPropertyInfoValues22.IsProperty = true;
			jsonPropertyInfoValues22.IsPublic = true;
			jsonPropertyInfoValues22.IsVirtual = false;
			jsonPropertyInfoValues22.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues22.Converter = null;
			jsonPropertyInfoValues22.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).RelicChoices;
			jsonPropertyInfoValues22.Setter = delegate(object obj, [Nullable(2)] List<ModelChoiceHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).RelicChoices = value;
			};
			jsonPropertyInfoValues22.IgnoreCondition = null;
			jsonPropertyInfoValues22.HasJsonInclude = false;
			jsonPropertyInfoValues22.IsExtensionData = false;
			jsonPropertyInfoValues22.NumberHandling = null;
			jsonPropertyInfoValues22.PropertyName = "RelicChoices";
			jsonPropertyInfoValues22.JsonPropertyName = "relic_choices";
			jsonPropertyInfoValues22.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("RelicChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelChoiceHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelChoiceHistoryEntry>> jsonPropertyInfoValues23 = jsonPropertyInfoValues22;
			array[16] = JsonMetadataServices.CreatePropertyInfo<List<ModelChoiceHistoryEntry>>(options, jsonPropertyInfoValues23);
			array[16].IsGetNullable = false;
			array[16].IsSetNullable = false;
			jsonPropertyInfoValues22 = new JsonPropertyInfoValues<List<ModelChoiceHistoryEntry>>();
			jsonPropertyInfoValues22.IsProperty = true;
			jsonPropertyInfoValues22.IsPublic = true;
			jsonPropertyInfoValues22.IsVirtual = false;
			jsonPropertyInfoValues22.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues22.Converter = null;
			jsonPropertyInfoValues22.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).PotionChoices;
			jsonPropertyInfoValues22.Setter = delegate(object obj, [Nullable(2)] List<ModelChoiceHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).PotionChoices = value;
			};
			jsonPropertyInfoValues22.IgnoreCondition = null;
			jsonPropertyInfoValues22.HasJsonInclude = false;
			jsonPropertyInfoValues22.IsExtensionData = false;
			jsonPropertyInfoValues22.NumberHandling = null;
			jsonPropertyInfoValues22.PropertyName = "PotionChoices";
			jsonPropertyInfoValues22.JsonPropertyName = "potion_choices";
			jsonPropertyInfoValues22.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("PotionChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelChoiceHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelChoiceHistoryEntry>> jsonPropertyInfoValues24 = jsonPropertyInfoValues22;
			array[17] = JsonMetadataServices.CreatePropertyInfo<List<ModelChoiceHistoryEntry>>(options, jsonPropertyInfoValues24);
			array[17].IsGetNullable = false;
			array[17].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).PotionDiscarded;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).PotionDiscarded = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "PotionDiscarded";
			jsonPropertyInfoValues25.JsonPropertyName = "potion_discarded";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("PotionDiscarded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues26 = jsonPropertyInfoValues25;
			array[18] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues26);
			array[18].IsGetNullable = false;
			array[18].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).PotionUsed;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).PotionUsed = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "PotionUsed";
			jsonPropertyInfoValues25.JsonPropertyName = "potion_used";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("PotionUsed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues27 = jsonPropertyInfoValues25;
			array[19] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues27);
			array[19].IsGetNullable = false;
			array[19].IsSetNullable = false;
			jsonPropertyInfoValues18 = new JsonPropertyInfoValues<List<SerializableCard>>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CardsRemoved;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableCard> value)
			{
				((PlayerMapPointHistoryEntry)obj).CardsRemoved = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "CardsRemoved";
			jsonPropertyInfoValues18.JsonPropertyName = "cards_removed";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CardsRemoved", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableCard>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableCard>> jsonPropertyInfoValues28 = jsonPropertyInfoValues18;
			array[20] = JsonMetadataServices.CreatePropertyInfo<List<SerializableCard>>(options, jsonPropertyInfoValues28);
			array[20].IsGetNullable = false;
			array[20].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).RelicsRemoved;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).RelicsRemoved = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "RelicsRemoved";
			jsonPropertyInfoValues25.JsonPropertyName = "relics_removed";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("RelicsRemoved", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues29 = jsonPropertyInfoValues25;
			array[21] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues29);
			array[21].IsGetNullable = false;
			array[21].IsSetNullable = false;
			JsonPropertyInfoValues<List<CardEnchantmentHistoryEntry>> jsonPropertyInfoValues30 = new JsonPropertyInfoValues<List<CardEnchantmentHistoryEntry>>();
			jsonPropertyInfoValues30.IsProperty = true;
			jsonPropertyInfoValues30.IsPublic = true;
			jsonPropertyInfoValues30.IsVirtual = false;
			jsonPropertyInfoValues30.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues30.Converter = null;
			jsonPropertyInfoValues30.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CardsEnchanted;
			jsonPropertyInfoValues30.Setter = delegate(object obj, [Nullable(2)] List<CardEnchantmentHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).CardsEnchanted = value;
			};
			jsonPropertyInfoValues30.IgnoreCondition = null;
			jsonPropertyInfoValues30.HasJsonInclude = false;
			jsonPropertyInfoValues30.IsExtensionData = false;
			jsonPropertyInfoValues30.NumberHandling = null;
			jsonPropertyInfoValues30.PropertyName = "CardsEnchanted";
			jsonPropertyInfoValues30.JsonPropertyName = "cards_enchanted";
			jsonPropertyInfoValues30.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CardsEnchanted", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CardEnchantmentHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CardEnchantmentHistoryEntry>> jsonPropertyInfoValues31 = jsonPropertyInfoValues30;
			array[22] = JsonMetadataServices.CreatePropertyInfo<List<CardEnchantmentHistoryEntry>>(options, jsonPropertyInfoValues31);
			array[22].IsGetNullable = false;
			array[22].IsSetNullable = false;
			JsonPropertyInfoValues<List<CardTransformationHistoryEntry>> jsonPropertyInfoValues32 = new JsonPropertyInfoValues<List<CardTransformationHistoryEntry>>();
			jsonPropertyInfoValues32.IsProperty = true;
			jsonPropertyInfoValues32.IsPublic = true;
			jsonPropertyInfoValues32.IsVirtual = false;
			jsonPropertyInfoValues32.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues32.Converter = null;
			jsonPropertyInfoValues32.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CardsTransformed;
			jsonPropertyInfoValues32.Setter = delegate(object obj, [Nullable(2)] List<CardTransformationHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).CardsTransformed = value;
			};
			jsonPropertyInfoValues32.IgnoreCondition = null;
			jsonPropertyInfoValues32.HasJsonInclude = false;
			jsonPropertyInfoValues32.IsExtensionData = false;
			jsonPropertyInfoValues32.NumberHandling = null;
			jsonPropertyInfoValues32.PropertyName = "CardsTransformed";
			jsonPropertyInfoValues32.JsonPropertyName = "cards_transformed";
			jsonPropertyInfoValues32.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CardsTransformed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CardTransformationHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CardTransformationHistoryEntry>> jsonPropertyInfoValues33 = jsonPropertyInfoValues32;
			array[23] = JsonMetadataServices.CreatePropertyInfo<List<CardTransformationHistoryEntry>>(options, jsonPropertyInfoValues33);
			array[23].IsGetNullable = false;
			array[23].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).UpgradedCards;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).UpgradedCards = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "UpgradedCards";
			jsonPropertyInfoValues25.JsonPropertyName = "upgraded_cards";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("UpgradedCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues34 = jsonPropertyInfoValues25;
			array[24] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues34);
			array[24].IsGetNullable = false;
			array[24].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).DowngradedCards;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).DowngradedCards = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "DowngradedCards";
			jsonPropertyInfoValues25.JsonPropertyName = "downgraded_cards";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("DowngradedCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues35 = jsonPropertyInfoValues25;
			array[25] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues35);
			array[25].IsGetNullable = false;
			array[25].IsSetNullable = false;
			JsonPropertyInfoValues<List<EventOptionHistoryEntry>> jsonPropertyInfoValues36 = new JsonPropertyInfoValues<List<EventOptionHistoryEntry>>();
			jsonPropertyInfoValues36.IsProperty = true;
			jsonPropertyInfoValues36.IsPublic = true;
			jsonPropertyInfoValues36.IsVirtual = false;
			jsonPropertyInfoValues36.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues36.Converter = null;
			jsonPropertyInfoValues36.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).EventChoices;
			jsonPropertyInfoValues36.Setter = delegate(object obj, [Nullable(2)] List<EventOptionHistoryEntry> value)
			{
				((PlayerMapPointHistoryEntry)obj).EventChoices = value;
			};
			jsonPropertyInfoValues36.IgnoreCondition = null;
			jsonPropertyInfoValues36.HasJsonInclude = false;
			jsonPropertyInfoValues36.IsExtensionData = false;
			jsonPropertyInfoValues36.NumberHandling = null;
			jsonPropertyInfoValues36.PropertyName = "EventChoices";
			jsonPropertyInfoValues36.JsonPropertyName = "event_choices";
			jsonPropertyInfoValues36.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("EventChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<EventOptionHistoryEntry>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<EventOptionHistoryEntry>> jsonPropertyInfoValues37 = jsonPropertyInfoValues36;
			array[26] = JsonMetadataServices.CreatePropertyInfo<List<EventOptionHistoryEntry>>(options, jsonPropertyInfoValues37);
			array[26].IsGetNullable = false;
			array[26].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues38 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues38.IsProperty = true;
			jsonPropertyInfoValues38.IsPublic = true;
			jsonPropertyInfoValues38.IsVirtual = false;
			jsonPropertyInfoValues38.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues38.Converter = null;
			jsonPropertyInfoValues38.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).RestSiteChoices;
			jsonPropertyInfoValues38.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				((PlayerMapPointHistoryEntry)obj).RestSiteChoices = value;
			};
			jsonPropertyInfoValues38.IgnoreCondition = null;
			jsonPropertyInfoValues38.HasJsonInclude = false;
			jsonPropertyInfoValues38.IsExtensionData = false;
			jsonPropertyInfoValues38.NumberHandling = null;
			jsonPropertyInfoValues38.PropertyName = "RestSiteChoices";
			jsonPropertyInfoValues38.JsonPropertyName = "rest_site_choices";
			jsonPropertyInfoValues38.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("RestSiteChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues39 = jsonPropertyInfoValues38;
			array[27] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues39);
			array[27].IsGetNullable = false;
			array[27].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).BoughtRelics;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).BoughtRelics = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "BoughtRelics";
			jsonPropertyInfoValues25.JsonPropertyName = "bought_relics";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("BoughtRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues40 = jsonPropertyInfoValues25;
			array[28] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues40);
			array[28].IsGetNullable = false;
			array[28].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).BoughtPotions;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).BoughtPotions = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "BoughtPotions";
			jsonPropertyInfoValues25.JsonPropertyName = "bought_potions";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("BoughtPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues41 = jsonPropertyInfoValues25;
			array[29] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues41);
			array[29].IsGetNullable = false;
			array[29].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).BoughtColorless;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).BoughtColorless = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "BoughtColorless";
			jsonPropertyInfoValues25.JsonPropertyName = "bought_colorless";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("BoughtColorless", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues42 = jsonPropertyInfoValues25;
			array[30] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues42);
			array[30].IsGetNullable = false;
			array[30].IsSetNullable = false;
			jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).CompletedQuests;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((PlayerMapPointHistoryEntry)obj).CompletedQuests = value;
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "CompletedQuests";
			jsonPropertyInfoValues25.JsonPropertyName = "completed_quests";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("CompletedQuests", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues43 = jsonPropertyInfoValues25;
			array[31] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues43);
			array[31].IsGetNullable = false;
			array[31].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues44 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues44.IsProperty = true;
			jsonPropertyInfoValues44.IsPublic = true;
			jsonPropertyInfoValues44.IsVirtual = false;
			jsonPropertyInfoValues44.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues44.Converter = null;
			jsonPropertyInfoValues44.Getter = (object obj) => ((PlayerMapPointHistoryEntry)obj).IsAffectedByFurCoat;
			jsonPropertyInfoValues44.Setter = delegate(object obj, bool value)
			{
				((PlayerMapPointHistoryEntry)obj).IsAffectedByFurCoat = value;
			};
			jsonPropertyInfoValues44.IgnoreCondition = null;
			jsonPropertyInfoValues44.HasJsonInclude = false;
			jsonPropertyInfoValues44.IsExtensionData = false;
			jsonPropertyInfoValues44.NumberHandling = null;
			jsonPropertyInfoValues44.PropertyName = "IsAffectedByFurCoat";
			jsonPropertyInfoValues44.JsonPropertyName = "is_affected_by_fur_coat";
			jsonPropertyInfoValues44.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("IsAffectedByFurCoat", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues45 = jsonPropertyInfoValues44;
			array[32] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues45);
			jsonPropertyInfoValues44 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues44.IsProperty = true;
			jsonPropertyInfoValues44.IsPublic = true;
			jsonPropertyInfoValues44.IsVirtual = false;
			jsonPropertyInfoValues44.DeclaringType = typeof(PlayerMapPointHistoryEntry);
			jsonPropertyInfoValues44.Converter = null;
			jsonPropertyInfoValues44.Getter = null;
			jsonPropertyInfoValues44.Setter = null;
			jsonPropertyInfoValues44.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues44.HasJsonInclude = false;
			jsonPropertyInfoValues44.IsExtensionData = false;
			jsonPropertyInfoValues44.NumberHandling = null;
			jsonPropertyInfoValues44.PropertyName = "WasMugged";
			jsonPropertyInfoValues44.JsonPropertyName = null;
			jsonPropertyInfoValues44.AttributeProviderFactory = () => typeof(PlayerMapPointHistoryEntry).GetProperty("WasMugged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues46 = jsonPropertyInfoValues44;
			array[33] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues46);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00020F64 File Offset: 0x0001F164
		public JsonTypeInfo<RunHistory> RunHistory
		{
			get
			{
				JsonTypeInfo<RunHistory> jsonTypeInfo;
				if ((jsonTypeInfo = this._RunHistory) == null)
				{
					jsonTypeInfo = (this._RunHistory = (JsonTypeInfo<RunHistory>)base.Options.GetTypeInfo(typeof(RunHistory)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00020FA0 File Offset: 0x0001F1A0
		[NullableContext(1)]
		private JsonTypeInfo<RunHistory> Create_RunHistory(JsonSerializerOptions options)
		{
			JsonTypeInfo<RunHistory> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RunHistory>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<RunHistory> jsonObjectInfoValues = new JsonObjectInfoValues<RunHistory>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new RunHistory
				{
					PlatformType = (PlatformType)args[0],
					GameMode = (GameMode)args[1],
					Win = (bool)args[2],
					Seed = (string)args[3],
					StartTime = (long)args[4],
					RunTime = (float)args[5],
					Ascension = (int)args[6],
					BuildId = (string)args[7],
					WasAbandoned = (bool)args[8],
					KilledByEncounter = (ModelId)args[9],
					KilledByEvent = (ModelId)args[10],
					Players = (List<RunHistoryPlayer>)args[11],
					Acts = (List<ModelId>)args[12],
					Modifiers = (List<SerializableModifier>)args[13],
					MapPointHistory = (List<List<MapPointHistoryEntry>>)args[14]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.RunHistoryPropInit(options);
				JsonObjectInfoValues<RunHistory> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<4>__RunHistoryCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<4>__RunHistoryCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.RunHistoryCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(RunHistory).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<RunHistory> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<RunHistory>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00021084 File Offset: 0x0001F284
		[NullableContext(1)]
		private static JsonPropertyInfo[] RunHistoryPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[16];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunHistory)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((RunHistory)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<PlatformType> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<PlatformType>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((RunHistory)obj).PlatformType;
			jsonPropertyInfoValues3.Setter = delegate(object obj, PlatformType value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "PlatformType";
			jsonPropertyInfoValues3.JsonPropertyName = "platform_type";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("PlatformType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(PlatformType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<PlatformType> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<PlatformType>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<GameMode> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<GameMode>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((RunHistory)obj).GameMode;
			jsonPropertyInfoValues5.Setter = delegate(object obj, GameMode value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "GameMode";
			jsonPropertyInfoValues5.JsonPropertyName = "game_mode";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("GameMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(GameMode), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<GameMode> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<GameMode>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((RunHistory)obj).Win;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "Win";
			jsonPropertyInfoValues7.JsonPropertyName = "win";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((RunHistory)obj).Seed;
			jsonPropertyInfoValues9.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "Seed";
			jsonPropertyInfoValues9.JsonPropertyName = "seed";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues10);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			JsonPropertyInfoValues<long> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((RunHistory)obj).StartTime;
			jsonPropertyInfoValues11.Setter = delegate(object obj, long value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "StartTime";
			jsonPropertyInfoValues11.JsonPropertyName = "start_time";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("StartTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[5] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues12);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues13.IsProperty = true;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((RunHistory)obj).RunTime;
			jsonPropertyInfoValues13.Setter = delegate(object obj, float value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues13.IgnoreCondition = null;
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "RunTime";
			jsonPropertyInfoValues13.JsonPropertyName = "run_time";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("RunTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[6] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues14);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunHistory)obj).Ascension;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Ascension";
			jsonPropertyInfoValues.JsonPropertyName = "ascension";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues15 = jsonPropertyInfoValues;
			array[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues15);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((RunHistory)obj).BuildId;
			jsonPropertyInfoValues9.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "BuildId";
			jsonPropertyInfoValues9.JsonPropertyName = "build_id";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues16 = jsonPropertyInfoValues9;
			array[8] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues16);
			array[8].IsGetNullable = false;
			array[8].IsSetNullable = false;
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((RunHistory)obj).WasAbandoned;
			jsonPropertyInfoValues7.Setter = delegate(object obj, bool value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "WasAbandoned";
			jsonPropertyInfoValues7.JsonPropertyName = "was_abandoned";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("WasAbandoned", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues17 = jsonPropertyInfoValues7;
			array[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues17);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues18 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((RunHistory)obj).KilledByEncounter;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "KilledByEncounter";
			jsonPropertyInfoValues18.JsonPropertyName = "killed_by_encounter";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("KilledByEncounter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues19 = jsonPropertyInfoValues18;
			array[10] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues19);
			array[10].IsGetNullable = false;
			array[10].IsSetNullable = false;
			jsonPropertyInfoValues18 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((RunHistory)obj).KilledByEvent;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "KilledByEvent";
			jsonPropertyInfoValues18.JsonPropertyName = "killed_by_event";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("KilledByEvent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues20 = jsonPropertyInfoValues18;
			array[11] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues20);
			array[11].IsGetNullable = false;
			array[11].IsSetNullable = false;
			JsonPropertyInfoValues<List<RunHistoryPlayer>> jsonPropertyInfoValues21 = new JsonPropertyInfoValues<List<RunHistoryPlayer>>();
			jsonPropertyInfoValues21.IsProperty = true;
			jsonPropertyInfoValues21.IsPublic = true;
			jsonPropertyInfoValues21.IsVirtual = false;
			jsonPropertyInfoValues21.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues21.Converter = null;
			jsonPropertyInfoValues21.Getter = (object obj) => ((RunHistory)obj).Players;
			jsonPropertyInfoValues21.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<RunHistoryPlayer> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues21.IgnoreCondition = null;
			jsonPropertyInfoValues21.HasJsonInclude = false;
			jsonPropertyInfoValues21.IsExtensionData = false;
			jsonPropertyInfoValues21.NumberHandling = null;
			jsonPropertyInfoValues21.PropertyName = "Players";
			jsonPropertyInfoValues21.JsonPropertyName = "players";
			jsonPropertyInfoValues21.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Players", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<RunHistoryPlayer>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<RunHistoryPlayer>> jsonPropertyInfoValues22 = jsonPropertyInfoValues21;
			array[12] = JsonMetadataServices.CreatePropertyInfo<List<RunHistoryPlayer>>(options, jsonPropertyInfoValues22);
			array[12].IsGetNullable = false;
			array[12].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((RunHistory)obj).Acts;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "Acts";
			jsonPropertyInfoValues23.JsonPropertyName = "acts";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Acts", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues24 = jsonPropertyInfoValues23;
			array[13] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues24);
			array[13].IsGetNullable = false;
			array[13].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializableModifier>> jsonPropertyInfoValues25 = new JsonPropertyInfoValues<List<SerializableModifier>>();
			jsonPropertyInfoValues25.IsProperty = true;
			jsonPropertyInfoValues25.IsPublic = true;
			jsonPropertyInfoValues25.IsVirtual = false;
			jsonPropertyInfoValues25.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues25.Converter = null;
			jsonPropertyInfoValues25.Getter = (object obj) => ((RunHistory)obj).Modifiers;
			jsonPropertyInfoValues25.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableModifier> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues25.IgnoreCondition = null;
			jsonPropertyInfoValues25.HasJsonInclude = false;
			jsonPropertyInfoValues25.IsExtensionData = false;
			jsonPropertyInfoValues25.NumberHandling = null;
			jsonPropertyInfoValues25.PropertyName = "Modifiers";
			jsonPropertyInfoValues25.JsonPropertyName = "modifiers";
			jsonPropertyInfoValues25.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("Modifiers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableModifier>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableModifier>> jsonPropertyInfoValues26 = jsonPropertyInfoValues25;
			array[14] = JsonMetadataServices.CreatePropertyInfo<List<SerializableModifier>>(options, jsonPropertyInfoValues26);
			array[14].IsGetNullable = false;
			array[14].IsSetNullable = false;
			JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>> jsonPropertyInfoValues27 = new JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>>();
			jsonPropertyInfoValues27.IsProperty = true;
			jsonPropertyInfoValues27.IsPublic = true;
			jsonPropertyInfoValues27.IsVirtual = false;
			jsonPropertyInfoValues27.DeclaringType = typeof(RunHistory);
			jsonPropertyInfoValues27.Converter = null;
			jsonPropertyInfoValues27.Getter = (object obj) => ((RunHistory)obj).MapPointHistory;
			jsonPropertyInfoValues27.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] List<List<MapPointHistoryEntry>> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues27.IgnoreCondition = null;
			jsonPropertyInfoValues27.HasJsonInclude = false;
			jsonPropertyInfoValues27.IsExtensionData = false;
			jsonPropertyInfoValues27.NumberHandling = null;
			jsonPropertyInfoValues27.PropertyName = "MapPointHistory";
			jsonPropertyInfoValues27.JsonPropertyName = "map_point_history";
			jsonPropertyInfoValues27.AttributeProviderFactory = () => typeof(RunHistory).GetProperty("MapPointHistory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<List<MapPointHistoryEntry>>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>> jsonPropertyInfoValues28 = jsonPropertyInfoValues27;
			array[15] = JsonMetadataServices.CreatePropertyInfo<List<List<MapPointHistoryEntry>>>(options, jsonPropertyInfoValues28);
			array[15].IsGetNullable = false;
			array[15].IsSetNullable = false;
			return array;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00022168 File Offset: 0x00020368
		[NullableContext(1)]
		private static JsonParameterInfoValues[] RunHistoryCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "PlatformType",
					ParameterType = typeof(PlatformType),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "GameMode",
					ParameterType = typeof(GameMode),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Win",
					ParameterType = typeof(bool),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Seed",
					ParameterType = typeof(string),
					Position = 3,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "StartTime",
					ParameterType = typeof(long),
					Position = 4,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "RunTime",
					ParameterType = typeof(float),
					Position = 5,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Ascension",
					ParameterType = typeof(int),
					Position = 6,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "BuildId",
					ParameterType = typeof(string),
					Position = 7,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "WasAbandoned",
					ParameterType = typeof(bool),
					Position = 8,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "KilledByEncounter",
					ParameterType = typeof(ModelId),
					Position = 9,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "KilledByEvent",
					ParameterType = typeof(ModelId),
					Position = 10,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Players",
					ParameterType = typeof(List<RunHistoryPlayer>),
					Position = 11,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Acts",
					ParameterType = typeof(List<ModelId>),
					Position = 12,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Modifiers",
					ParameterType = typeof(List<SerializableModifier>),
					Position = 13,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "MapPointHistory",
					ParameterType = typeof(List<List<MapPointHistoryEntry>>),
					Position = 14,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x000224F0 File Offset: 0x000206F0
		public JsonTypeInfo<RunHistoryPlayer> RunHistoryPlayer
		{
			get
			{
				JsonTypeInfo<RunHistoryPlayer> jsonTypeInfo;
				if ((jsonTypeInfo = this._RunHistoryPlayer) == null)
				{
					jsonTypeInfo = (this._RunHistoryPlayer = (JsonTypeInfo<RunHistoryPlayer>)base.Options.GetTypeInfo(typeof(RunHistoryPlayer)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0002252C File Offset: 0x0002072C
		[NullableContext(1)]
		private JsonTypeInfo<RunHistoryPlayer> Create_RunHistoryPlayer(JsonSerializerOptions options)
		{
			JsonTypeInfo<RunHistoryPlayer> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<RunHistoryPlayer>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<RunHistoryPlayer> jsonObjectInfoValues = new JsonObjectInfoValues<RunHistoryPlayer>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new RunHistoryPlayer
				{
					Id = (ulong)args[0],
					Character = (ModelId)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.RunHistoryPlayerPropInit(options);
				JsonObjectInfoValues<RunHistoryPlayer> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<5>__RunHistoryPlayerCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<5>__RunHistoryPlayerCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.RunHistoryPlayerCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(RunHistoryPlayer).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<RunHistoryPlayer> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<RunHistoryPlayer>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00022610 File Offset: 0x00020810
		[NullableContext(1)]
		private static JsonPropertyInfo[] RunHistoryPlayerPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[7];
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((RunHistoryPlayer)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, ulong value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ulong), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((RunHistoryPlayer)obj).Character;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Character";
			jsonPropertyInfoValues3.JsonPropertyName = "character";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<IEnumerable<SerializableCard>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<IEnumerable<SerializableCard>>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((RunHistoryPlayer)obj).Deck;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<SerializableCard> value)
			{
				((RunHistoryPlayer)obj).Deck = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Deck";
			jsonPropertyInfoValues5.JsonPropertyName = "deck";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<SerializableCard>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<SerializableCard>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<SerializableCard>>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<IEnumerable<SerializableRelic>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<IEnumerable<SerializableRelic>>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((RunHistoryPlayer)obj).Relics;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<SerializableRelic> value)
			{
				((RunHistoryPlayer)obj).Relics = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "Relics";
			jsonPropertyInfoValues7.JsonPropertyName = "relics";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<SerializableRelic>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<SerializableRelic>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<SerializableRelic>>(options, jsonPropertyInfoValues8);
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			JsonPropertyInfoValues<IEnumerable<SerializablePotion>> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<IEnumerable<SerializablePotion>>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((RunHistoryPlayer)obj).Potions;
			jsonPropertyInfoValues9.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<SerializablePotion> value)
			{
				((RunHistoryPlayer)obj).Potions = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "Potions";
			jsonPropertyInfoValues9.JsonPropertyName = "potions";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Potions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<SerializablePotion>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<SerializablePotion>> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<SerializablePotion>>(options, jsonPropertyInfoValues10);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			JsonPropertyInfoValues<IEnumerable<SerializableBadge>> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<IEnumerable<SerializableBadge>>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((RunHistoryPlayer)obj).Badges;
			jsonPropertyInfoValues11.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] IEnumerable<SerializableBadge> value)
			{
				((RunHistoryPlayer)obj).Badges = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "Badges";
			jsonPropertyInfoValues11.JsonPropertyName = "badges";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("Badges", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(IEnumerable<SerializableBadge>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<IEnumerable<SerializableBadge>> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[5] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<SerializableBadge>>(options, jsonPropertyInfoValues12);
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues13.IsProperty = true;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(RunHistoryPlayer);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((RunHistoryPlayer)obj).MaxPotionSlotCount;
			jsonPropertyInfoValues13.Setter = delegate(object obj, int value)
			{
				((RunHistoryPlayer)obj).MaxPotionSlotCount = value;
			};
			jsonPropertyInfoValues13.IgnoreCondition = null;
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "MaxPotionSlotCount";
			jsonPropertyInfoValues13.JsonPropertyName = "max_potion_slot_count";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(RunHistoryPlayer).GetProperty("MaxPotionSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues14);
			return array;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00022D90 File Offset: 0x00020F90
		[NullableContext(1)]
		private static JsonParameterInfoValues[] RunHistoryPlayerCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ulong),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Character",
					ParameterType = typeof(ModelId),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00022E18 File Offset: 0x00021018
		public JsonTypeInfo<AncientCharacterStats> AncientCharacterStats
		{
			get
			{
				JsonTypeInfo<AncientCharacterStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._AncientCharacterStats) == null)
				{
					jsonTypeInfo = (this._AncientCharacterStats = (JsonTypeInfo<AncientCharacterStats>)base.Options.GetTypeInfo(typeof(AncientCharacterStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00022E54 File Offset: 0x00021054
		[NullableContext(1)]
		private JsonTypeInfo<AncientCharacterStats> Create_AncientCharacterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<AncientCharacterStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AncientCharacterStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AncientCharacterStats> jsonObjectInfoValues = new JsonObjectInfoValues<AncientCharacterStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new AncientCharacterStats
				{
					Character = (ModelId)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.AncientCharacterStatsPropInit(options);
				JsonObjectInfoValues<AncientCharacterStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<6>__AncientCharacterStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<6>__AncientCharacterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.AncientCharacterStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AncientCharacterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<AncientCharacterStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AncientCharacterStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00022F38 File Offset: 0x00021138
		[NullableContext(1)]
		private static JsonPropertyInfo[] AncientCharacterStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientCharacterStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientCharacterStats)obj).Character;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Character";
			jsonPropertyInfoValues.JsonPropertyName = "character";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientCharacterStats).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientCharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((AncientCharacterStats)obj).Wins;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((AncientCharacterStats)obj).Wins = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Wins";
			jsonPropertyInfoValues3.JsonPropertyName = "wins";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientCharacterStats).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientCharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((AncientCharacterStats)obj).Losses;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((AncientCharacterStats)obj).Losses = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Losses";
			jsonPropertyInfoValues3.JsonPropertyName = "losses";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientCharacterStats).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientCharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = null;
			jsonPropertyInfoValues3.Setter = null;
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Visits";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientCharacterStats).GetProperty("Visits", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			return array;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002332C File Offset: 0x0002152C
		[NullableContext(1)]
		private static JsonParameterInfoValues[] AncientCharacterStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Character",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0002337C File Offset: 0x0002157C
		public JsonTypeInfo<AncientStats> AncientStats
		{
			get
			{
				JsonTypeInfo<AncientStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._AncientStats) == null)
				{
					jsonTypeInfo = (this._AncientStats = (JsonTypeInfo<AncientStats>)base.Options.GetTypeInfo(typeof(AncientStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000233B8 File Offset: 0x000215B8
		[NullableContext(1)]
		private JsonTypeInfo<AncientStats> Create_AncientStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<AncientStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AncientStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<AncientStats> jsonObjectInfoValues = new JsonObjectInfoValues<AncientStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new AncientStats
				{
					Id = (ModelId)args[0],
					CharStats = (List<AncientCharacterStats>)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.AncientStatsPropInit(options);
				JsonObjectInfoValues<AncientStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<7>__AncientStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<7>__AncientStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.AncientStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(AncientStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<AncientStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<AncientStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002349C File Offset: 0x0002169C
		[NullableContext(1)]
		private static JsonPropertyInfo[] AncientStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(AncientStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((AncientStats)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "ancient_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(AncientStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<AncientCharacterStats>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<AncientCharacterStats>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(AncientStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((AncientStats)obj).CharStats;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<AncientCharacterStats> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CharStats";
			jsonPropertyInfoValues3.JsonPropertyName = "character_stats";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(AncientStats).GetProperty("CharStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<AncientCharacterStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<AncientCharacterStats>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<AncientCharacterStats>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(AncientStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalVisits";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(AncientStats).GetProperty("TotalVisits", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(AncientStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalWins";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(AncientStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(AncientStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalLosses";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(AncientStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues5;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			return array;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0002391C File Offset: 0x00021B1C
		[NullableContext(1)]
		private static JsonParameterInfoValues[] AncientStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "CharStats",
					ParameterType = typeof(List<AncientCharacterStats>),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x000239A4 File Offset: 0x00021BA4
		public JsonTypeInfo<BadgeStats> BadgeStats
		{
			get
			{
				JsonTypeInfo<BadgeStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._BadgeStats) == null)
				{
					jsonTypeInfo = (this._BadgeStats = (JsonTypeInfo<BadgeStats>)base.Options.GetTypeInfo(typeof(BadgeStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000239E0 File Offset: 0x00021BE0
		[NullableContext(1)]
		private JsonTypeInfo<BadgeStats> Create_BadgeStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<BadgeStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<BadgeStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<BadgeStats> jsonObjectInfoValues = new JsonObjectInfoValues<BadgeStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new BadgeStats
				{
					Id = (string)args[0],
					Count = (int)args[1],
					Rarity = (BadgeRarity)args[2]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.BadgeStatsPropInit(options);
				JsonObjectInfoValues<BadgeStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<8>__BadgeStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<8>__BadgeStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.BadgeStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(BadgeStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<BadgeStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<BadgeStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00023AC4 File Offset: 0x00021CC4
		[NullableContext(1)]
		private static JsonPropertyInfo[] BadgeStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(BadgeStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((BadgeStats)obj).Id;
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
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(BadgeStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(BadgeStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((BadgeStats)obj).Count;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((BadgeStats)obj).Count = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Count";
			jsonPropertyInfoValues3.JsonPropertyName = "count";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(BadgeStats).GetProperty("Count", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			array[1].IsRequired = true;
			JsonPropertyInfoValues<BadgeRarity> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<BadgeRarity>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(BadgeStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((BadgeStats)obj).Rarity;
			jsonPropertyInfoValues5.Setter = delegate(object obj, BadgeRarity value)
			{
				((BadgeStats)obj).Rarity = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Rarity";
			jsonPropertyInfoValues5.JsonPropertyName = "rarity";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(BadgeStats).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(BadgeRarity), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<BadgeRarity> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<BadgeRarity>(options, jsonPropertyInfoValues6);
			array[2].IsRequired = true;
			return array;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00023E0C File Offset: 0x0002200C
		[NullableContext(1)]
		private static JsonParameterInfoValues[] BadgeStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Count",
					ParameterType = typeof(int),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Rarity",
					ParameterType = typeof(BadgeRarity),
					Position = 2,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00023ED0 File Offset: 0x000220D0
		public JsonTypeInfo<CardStats> CardStats
		{
			get
			{
				JsonTypeInfo<CardStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._CardStats) == null)
				{
					jsonTypeInfo = (this._CardStats = (JsonTypeInfo<CardStats>)base.Options.GetTypeInfo(typeof(CardStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00023F0C File Offset: 0x0002210C
		[NullableContext(1)]
		private JsonTypeInfo<CardStats> Create_CardStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<CardStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CardStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CardStats> jsonObjectInfoValues = new JsonObjectInfoValues<CardStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new CardStats
				{
					Id = (ModelId)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.CardStatsPropInit(options);
				JsonObjectInfoValues<CardStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<9>__CardStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<9>__CardStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.CardStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CardStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<CardStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CardStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00023FF0 File Offset: 0x000221F0
		[NullableContext(1)]
		private static JsonPropertyInfo[] CardStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CardStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CardStats)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CardStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardStats)obj).TimesPicked;
			jsonPropertyInfoValues3.Setter = delegate(object obj, long value)
			{
				((CardStats)obj).TimesPicked = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TimesPicked";
			jsonPropertyInfoValues3.JsonPropertyName = "times_picked";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardStats).GetProperty("TimesPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardStats)obj).TimesSkipped;
			jsonPropertyInfoValues3.Setter = delegate(object obj, long value)
			{
				((CardStats)obj).TimesSkipped = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TimesSkipped";
			jsonPropertyInfoValues3.JsonPropertyName = "times_skipped";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardStats).GetProperty("TimesSkipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardStats)obj).TimesWon;
			jsonPropertyInfoValues3.Setter = delegate(object obj, long value)
			{
				((CardStats)obj).TimesWon = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TimesWon";
			jsonPropertyInfoValues3.JsonPropertyName = "times_won";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardStats).GetProperty("TimesWon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CardStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CardStats)obj).TimesLost;
			jsonPropertyInfoValues3.Setter = delegate(object obj, long value)
			{
				((CardStats)obj).TimesLost = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TimesLost";
			jsonPropertyInfoValues3.JsonPropertyName = "times_lost";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CardStats).GetProperty("TimesLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues7 = jsonPropertyInfoValues3;
			array[4] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues7);
			return array;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00024510 File Offset: 0x00022710
		[NullableContext(1)]
		private static JsonParameterInfoValues[] CardStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = true,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00024560 File Offset: 0x00022760
		public JsonTypeInfo<CharacterStats> CharacterStats
		{
			get
			{
				JsonTypeInfo<CharacterStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._CharacterStats) == null)
				{
					jsonTypeInfo = (this._CharacterStats = (JsonTypeInfo<CharacterStats>)base.Options.GetTypeInfo(typeof(CharacterStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0002459C File Offset: 0x0002279C
		[NullableContext(1)]
		private JsonTypeInfo<CharacterStats> Create_CharacterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<CharacterStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<CharacterStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<CharacterStats> jsonObjectInfoValues = new JsonObjectInfoValues<CharacterStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new CharacterStats
				{
					Id = (ModelId)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.CharacterStatsPropInit(options);
				JsonObjectInfoValues<CharacterStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<10>__CharacterStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<10>__CharacterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.CharacterStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(CharacterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<CharacterStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<CharacterStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00024680 File Offset: 0x00022880
		[NullableContext(1)]
		private static JsonPropertyInfo[] CharacterStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[10];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((CharacterStats)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CharacterStats)obj).MaxAscension;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((CharacterStats)obj).MaxAscension = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxAscension";
			jsonPropertyInfoValues3.JsonPropertyName = "max_ascension";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("MaxAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CharacterStats)obj).PreferredAscension;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((CharacterStats)obj).PreferredAscension = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "PreferredAscension";
			jsonPropertyInfoValues3.JsonPropertyName = "preferred_ascension";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("PreferredAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CharacterStats)obj).TotalWins;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((CharacterStats)obj).TotalWins = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TotalWins";
			jsonPropertyInfoValues3.JsonPropertyName = "total_wins";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((CharacterStats)obj).TotalLosses;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((CharacterStats)obj).TotalLosses = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TotalLosses";
			jsonPropertyInfoValues3.JsonPropertyName = "total_losses";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues3;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues8 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((CharacterStats)obj).FastestWinTime;
			jsonPropertyInfoValues8.Setter = delegate(object obj, long value)
			{
				((CharacterStats)obj).FastestWinTime = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "FastestWinTime";
			jsonPropertyInfoValues8.JsonPropertyName = "fastest_win_time";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("FastestWinTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues9 = jsonPropertyInfoValues8;
			array[5] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues9);
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((CharacterStats)obj).BestWinStreak;
			jsonPropertyInfoValues8.Setter = delegate(object obj, long value)
			{
				((CharacterStats)obj).BestWinStreak = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "BestWinStreak";
			jsonPropertyInfoValues8.JsonPropertyName = "best_win_streak";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("BestWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues10 = jsonPropertyInfoValues8;
			array[6] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues10);
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((CharacterStats)obj).CurrentWinStreak;
			jsonPropertyInfoValues8.Setter = delegate(object obj, long value)
			{
				((CharacterStats)obj).CurrentWinStreak = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "CurrentWinStreak";
			jsonPropertyInfoValues8.JsonPropertyName = "current_streak";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("CurrentWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues11 = jsonPropertyInfoValues8;
			array[7] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues8 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((CharacterStats)obj).Playtime;
			jsonPropertyInfoValues8.Setter = delegate(object obj, long value)
			{
				((CharacterStats)obj).Playtime = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "Playtime";
			jsonPropertyInfoValues8.JsonPropertyName = "playtime";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("Playtime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues12 = jsonPropertyInfoValues8;
			array[8] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues12);
			JsonPropertyInfoValues<List<BadgeStats>> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<List<BadgeStats>>();
			jsonPropertyInfoValues13.IsProperty = true;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(CharacterStats);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((CharacterStats)obj).Badges;
			jsonPropertyInfoValues13.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<BadgeStats> value)
			{
				((CharacterStats)obj).Badges = value;
			};
			jsonPropertyInfoValues13.IgnoreCondition = null;
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "Badges";
			jsonPropertyInfoValues13.JsonPropertyName = "badges";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(CharacterStats).GetProperty("Badges", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<BadgeStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<BadgeStats>> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[9] = JsonMetadataServices.CreatePropertyInfo<List<BadgeStats>>(options, jsonPropertyInfoValues14);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			return array;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000250C4 File Offset: 0x000232C4
		[NullableContext(1)]
		private static JsonParameterInfoValues[] CharacterStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = true,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00025114 File Offset: 0x00023314
		public JsonTypeInfo<EncounterStats> EncounterStats
		{
			get
			{
				JsonTypeInfo<EncounterStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._EncounterStats) == null)
				{
					jsonTypeInfo = (this._EncounterStats = (JsonTypeInfo<EncounterStats>)base.Options.GetTypeInfo(typeof(EncounterStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00025150 File Offset: 0x00023350
		[NullableContext(1)]
		private JsonTypeInfo<EncounterStats> Create_EncounterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<EncounterStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EncounterStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EncounterStats> jsonObjectInfoValues = new JsonObjectInfoValues<EncounterStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EncounterStats
				{
					Id = (ModelId)args[0],
					FightStats = (List<FightStats>)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.EncounterStatsPropInit(options);
				JsonObjectInfoValues<EncounterStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<11>__EncounterStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<11>__EncounterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.EncounterStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EncounterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<EncounterStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EncounterStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00025234 File Offset: 0x00023434
		[NullableContext(1)]
		private static JsonPropertyInfo[] EncounterStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EncounterStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EncounterStats)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "encounter_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EncounterStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<FightStats>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<FightStats>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(EncounterStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((EncounterStats)obj).FightStats;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<FightStats> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "FightStats";
			jsonPropertyInfoValues3.JsonPropertyName = "fight_stats";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(EncounterStats).GetProperty("FightStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<FightStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<FightStats>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<FightStats>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EncounterStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalWins";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EncounterStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EncounterStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalLosses";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EncounterStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			return array;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000255F4 File Offset: 0x000237F4
		[NullableContext(1)]
		private static JsonParameterInfoValues[] EncounterStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "FightStats",
					ParameterType = typeof(List<FightStats>),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0002567C File Offset: 0x0002387C
		public JsonTypeInfo<EnemyStats> EnemyStats
		{
			get
			{
				JsonTypeInfo<EnemyStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._EnemyStats) == null)
				{
					jsonTypeInfo = (this._EnemyStats = (JsonTypeInfo<EnemyStats>)base.Options.GetTypeInfo(typeof(EnemyStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000256B8 File Offset: 0x000238B8
		[NullableContext(1)]
		private JsonTypeInfo<EnemyStats> Create_EnemyStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<EnemyStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EnemyStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<EnemyStats> jsonObjectInfoValues = new JsonObjectInfoValues<EnemyStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new EnemyStats
				{
					Id = (ModelId)args[0],
					FightStats = (List<FightStats>)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.EnemyStatsPropInit(options);
				JsonObjectInfoValues<EnemyStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<12>__EnemyStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<12>__EnemyStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.EnemyStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(EnemyStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<EnemyStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<EnemyStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0002579C File Offset: 0x0002399C
		[NullableContext(1)]
		private static JsonPropertyInfo[] EnemyStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(EnemyStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((EnemyStats)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "enemy_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(EnemyStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<FightStats>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<FightStats>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(EnemyStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((EnemyStats)obj).FightStats;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<FightStats> value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "FightStats";
			jsonPropertyInfoValues3.JsonPropertyName = "fight_stats";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(EnemyStats).GetProperty("FightStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<FightStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<FightStats>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<FightStats>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EnemyStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalWins";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EnemyStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(EnemyStats);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = null;
			jsonPropertyInfoValues5.Setter = null;
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TotalLosses";
			jsonPropertyInfoValues5.JsonPropertyName = null;
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(EnemyStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues5;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			return array;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00025B5C File Offset: 0x00023D5C
		[NullableContext(1)]
		private static JsonParameterInfoValues[] EnemyStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "FightStats",
					ParameterType = typeof(List<FightStats>),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00025BE4 File Offset: 0x00023DE4
		public JsonTypeInfo<EpochState> EpochState
		{
			get
			{
				JsonTypeInfo<EpochState> jsonTypeInfo;
				if ((jsonTypeInfo = this._EpochState) == null)
				{
					jsonTypeInfo = (this._EpochState = (JsonTypeInfo<EpochState>)base.Options.GetTypeInfo(typeof(EpochState)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00025C20 File Offset: 0x00023E20
		[NullableContext(1)]
		private JsonTypeInfo<EpochState> Create_EpochState(JsonSerializerOptions options)
		{
			JsonTypeInfo<EpochState> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<EpochState>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<EpochState>(options, JsonMetadataServices.GetEnumConverter<EpochState>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x00025C4C File Offset: 0x00023E4C
		public JsonTypeInfo<FightStats> FightStats
		{
			get
			{
				JsonTypeInfo<FightStats> jsonTypeInfo;
				if ((jsonTypeInfo = this._FightStats) == null)
				{
					jsonTypeInfo = (this._FightStats = (JsonTypeInfo<FightStats>)base.Options.GetTypeInfo(typeof(FightStats)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00025C88 File Offset: 0x00023E88
		[NullableContext(1)]
		private JsonTypeInfo<FightStats> Create_FightStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<FightStats> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<FightStats>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<FightStats> jsonObjectInfoValues = new JsonObjectInfoValues<FightStats>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new FightStats
				{
					Character = (ModelId)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.FightStatsPropInit(options);
				JsonObjectInfoValues<FightStats> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<13>__FightStatsCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<13>__FightStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.FightStatsCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(FightStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<FightStats> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<FightStats>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00025D6C File Offset: 0x00023F6C
		[NullableContext(1)]
		private static JsonPropertyInfo[] FightStatsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(FightStats);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((FightStats)obj).Character;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Character";
			jsonPropertyInfoValues.JsonPropertyName = "character";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(FightStats).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(FightStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((FightStats)obj).Wins;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((FightStats)obj).Wins = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Wins";
			jsonPropertyInfoValues3.JsonPropertyName = "wins";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(FightStats).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(FightStats);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((FightStats)obj).Losses;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((FightStats)obj).Losses = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Losses";
			jsonPropertyInfoValues3.JsonPropertyName = "losses";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(FightStats).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			return array;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000260A0 File Offset: 0x000242A0
		[NullableContext(1)]
		private static JsonParameterInfoValues[] FightStatsCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Character",
					ParameterType = typeof(ModelId),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x000260F0 File Offset: 0x000242F0
		public JsonTypeInfo<SerializableMapDrawingLine> SerializableMapDrawingLine
		{
			get
			{
				JsonTypeInfo<SerializableMapDrawingLine> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableMapDrawingLine) == null)
				{
					jsonTypeInfo = (this._SerializableMapDrawingLine = (JsonTypeInfo<SerializableMapDrawingLine>)base.Options.GetTypeInfo(typeof(SerializableMapDrawingLine)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002612C File Offset: 0x0002432C
		[NullableContext(1)]
		private JsonTypeInfo<SerializableMapDrawingLine> Create_SerializableMapDrawingLine(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableMapDrawingLine> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableMapDrawingLine>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableMapDrawingLine> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableMapDrawingLine>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableMapDrawingLine();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableMapDrawingLinePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableMapDrawingLine).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableMapDrawingLine> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableMapDrawingLine>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000261F4 File Offset: 0x000243F4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableMapDrawingLinePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableMapDrawingLine);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableMapDrawingLine)obj).isEraser;
			jsonPropertyInfoValues.Setter = delegate(object obj, bool value)
			{
				((SerializableMapDrawingLine)obj).isEraser = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "isEraser";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableMapDrawingLine).GetField("isEraser", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<Vector2>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<Vector2>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableMapDrawingLine);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableMapDrawingLine)obj).mapPoints;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] List<Vector2> value)
			{
				((SerializableMapDrawingLine)obj).mapPoints = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "mapPoints";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableMapDrawingLine).GetField("mapPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<Vector2>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<Vector2>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00026408 File Offset: 0x00024608
		public JsonTypeInfo<SerializableMapDrawings> SerializableMapDrawings
		{
			get
			{
				JsonTypeInfo<SerializableMapDrawings> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableMapDrawings) == null)
				{
					jsonTypeInfo = (this._SerializableMapDrawings = (JsonTypeInfo<SerializableMapDrawings>)base.Options.GetTypeInfo(typeof(SerializableMapDrawings)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00026444 File Offset: 0x00024644
		[NullableContext(1)]
		private JsonTypeInfo<SerializableMapDrawings> Create_SerializableMapDrawings(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableMapDrawings> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableMapDrawings>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableMapDrawings> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableMapDrawings>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableMapDrawings();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableMapDrawingsPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableMapDrawings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableMapDrawings> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableMapDrawings>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002650C File Offset: 0x0002470C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableMapDrawingsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[1];
			JsonPropertyInfoValues<List<SerializablePlayerMapDrawings>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<SerializablePlayerMapDrawings>>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableMapDrawings);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableMapDrawings)obj).drawings;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializablePlayerMapDrawings> value)
			{
				((SerializableMapDrawings)obj).drawings = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "drawings";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableMapDrawings).GetField("drawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SerializablePlayerMapDrawings>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<SerializablePlayerMapDrawings>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00026620 File Offset: 0x00024820
		public JsonTypeInfo<SerializablePlayerMapDrawings> SerializablePlayerMapDrawings
		{
			get
			{
				JsonTypeInfo<SerializablePlayerMapDrawings> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializablePlayerMapDrawings) == null)
				{
					jsonTypeInfo = (this._SerializablePlayerMapDrawings = (JsonTypeInfo<SerializablePlayerMapDrawings>)base.Options.GetTypeInfo(typeof(SerializablePlayerMapDrawings)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002665C File Offset: 0x0002485C
		[NullableContext(1)]
		private JsonTypeInfo<SerializablePlayerMapDrawings> Create_SerializablePlayerMapDrawings(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializablePlayerMapDrawings> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializablePlayerMapDrawings>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializablePlayerMapDrawings> jsonObjectInfoValues = new JsonObjectInfoValues<SerializablePlayerMapDrawings>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializablePlayerMapDrawings();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializablePlayerMapDrawingsPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializablePlayerMapDrawings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializablePlayerMapDrawings> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializablePlayerMapDrawings>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00026724 File Offset: 0x00024924
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializablePlayerMapDrawingsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePlayerMapDrawings);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePlayerMapDrawings)obj).playerId;
			jsonPropertyInfoValues.Setter = delegate(object obj, ulong value)
			{
				((SerializablePlayerMapDrawings)obj).playerId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "playerId";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePlayerMapDrawings).GetField("playerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<SerializableMapDrawingLine>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<SerializableMapDrawingLine>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayerMapDrawings);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayerMapDrawings)obj).lines;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableMapDrawingLine> value)
			{
				((SerializablePlayerMapDrawings)obj).lines = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "lines";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayerMapDrawings).GetField("lines", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SerializableMapDrawingLine>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<SerializableMapDrawingLine>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00026938 File Offset: 0x00024B38
		public JsonTypeInfo<MigratingData> MigratingData
		{
			get
			{
				JsonTypeInfo<MigratingData> jsonTypeInfo;
				if ((jsonTypeInfo = this._MigratingData) == null)
				{
					jsonTypeInfo = (this._MigratingData = (JsonTypeInfo<MigratingData>)base.Options.GetTypeInfo(typeof(MigratingData)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00026974 File Offset: 0x00024B74
		[NullableContext(1)]
		private JsonTypeInfo<MigratingData> Create_MigratingData(JsonSerializerOptions options)
		{
			JsonTypeInfo<MigratingData> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MigratingData>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<MigratingData> jsonObjectInfoValues = new JsonObjectInfoValues<MigratingData>
				{
					ObjectCreator = null,
					ObjectWithParameterizedConstructorCreator = null,
					PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.MigratingDataPropInit(options),
					ConstructorParameterMetadataInitializer = null,
					ConstructorAttributeProviderFactory = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MigratingData>(options, jsonObjectInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000269FC File Offset: 0x00024BFC
		[NullableContext(1)]
		private static JsonPropertyInfo[] MigratingDataPropInit(JsonSerializerOptions options)
		{
			return new JsonPropertyInfo[0];
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00026A14 File Offset: 0x00024C14
		public JsonTypeInfo<PrefsSave> PrefsSave
		{
			get
			{
				JsonTypeInfo<PrefsSave> jsonTypeInfo;
				if ((jsonTypeInfo = this._PrefsSave) == null)
				{
					jsonTypeInfo = (this._PrefsSave = (JsonTypeInfo<PrefsSave>)base.Options.GetTypeInfo(typeof(PrefsSave)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00026A50 File Offset: 0x00024C50
		[NullableContext(1)]
		private JsonTypeInfo<PrefsSave> Create_PrefsSave(JsonSerializerOptions options)
		{
			JsonTypeInfo<PrefsSave> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<PrefsSave>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<PrefsSave> jsonObjectInfoValues = new JsonObjectInfoValues<PrefsSave>();
				jsonObjectInfoValues.ObjectCreator = () => new PrefsSave();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.PrefsSavePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(PrefsSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<PrefsSave> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<PrefsSave>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00026B18 File Offset: 0x00024D18
		[NullableContext(1)]
		private static JsonPropertyInfo[] PrefsSavePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[12];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PrefsSave)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((PrefsSave)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<FastModeType> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<FastModeType>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((PrefsSave)obj).FastMode;
			jsonPropertyInfoValues3.Setter = delegate(object obj, FastModeType value)
			{
				((PrefsSave)obj).FastMode = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "FastMode";
			jsonPropertyInfoValues3.JsonPropertyName = "fast_mode";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("FastMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(FastModeType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<FastModeType> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<FastModeType>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).PhobiaMode;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).PhobiaMode = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "PhobiaMode";
			jsonPropertyInfoValues5.JsonPropertyName = "phobia_mode";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("PhobiaMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((PrefsSave)obj).ScreenShakeOptionIndex;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((PrefsSave)obj).ScreenShakeOptionIndex = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "ScreenShakeOptionIndex";
			jsonPropertyInfoValues.JsonPropertyName = "screenshake";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("ScreenShakeOptionIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).ShowRunTimer;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).ShowRunTimer = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "ShowRunTimer";
			jsonPropertyInfoValues5.JsonPropertyName = "show_run_timer";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("ShowRunTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues8 = jsonPropertyInfoValues5;
			array[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).ShowCardIndices;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).ShowCardIndices = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "ShowCardIndices";
			jsonPropertyInfoValues5.JsonPropertyName = "show_card_indices";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("ShowCardIndices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues9 = jsonPropertyInfoValues5;
			array[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues9);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).UploadData;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).UploadData = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "UploadData";
			jsonPropertyInfoValues5.JsonPropertyName = "upload_data";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("UploadData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues10 = jsonPropertyInfoValues5;
			array[6] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues10);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).MuteInBackground;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).MuteInBackground = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "MuteInBackground";
			jsonPropertyInfoValues5.JsonPropertyName = "mute_in_background";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("MuteInBackground", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues11 = jsonPropertyInfoValues5;
			array[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).IsLongPressEnabled;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).IsLongPressEnabled = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "IsLongPressEnabled";
			jsonPropertyInfoValues5.JsonPropertyName = "long_press";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("IsLongPressEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues12 = jsonPropertyInfoValues5;
			array[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).TextEffectsEnabled;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).TextEffectsEnabled = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "TextEffectsEnabled";
			jsonPropertyInfoValues5.JsonPropertyName = "text_effects_enabled";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("TextEffectsEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues13 = jsonPropertyInfoValues5;
			array[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues13);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).ShowMultiplayerDrawings;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).ShowMultiplayerDrawings = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "ShowMultiplayerDrawings";
			jsonPropertyInfoValues5.JsonPropertyName = "show_mp_drawings";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("ShowMultiplayerDrawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues14 = jsonPropertyInfoValues5;
			array[10] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues14);
			jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(PrefsSave);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((PrefsSave)obj).IsBestiaryActionsPreferred;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((PrefsSave)obj).IsBestiaryActionsPreferred = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "IsBestiaryActionsPreferred";
			jsonPropertyInfoValues5.JsonPropertyName = "bestiary_actions_preferred";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(PrefsSave).GetProperty("IsBestiaryActionsPreferred", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues15 = jsonPropertyInfoValues5;
			array[11] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues15);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00027750 File Offset: 0x00025950
		public JsonTypeInfo<ProfileSave> ProfileSave
		{
			get
			{
				JsonTypeInfo<ProfileSave> jsonTypeInfo;
				if ((jsonTypeInfo = this._ProfileSave) == null)
				{
					jsonTypeInfo = (this._ProfileSave = (JsonTypeInfo<ProfileSave>)base.Options.GetTypeInfo(typeof(ProfileSave)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002778C File Offset: 0x0002598C
		[NullableContext(1)]
		private JsonTypeInfo<ProfileSave> Create_ProfileSave(JsonSerializerOptions options)
		{
			JsonTypeInfo<ProfileSave> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ProfileSave>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<ProfileSave> jsonObjectInfoValues = new JsonObjectInfoValues<ProfileSave>();
				jsonObjectInfoValues.ObjectCreator = () => new ProfileSave();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.ProfileSavePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(ProfileSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<ProfileSave> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<ProfileSave>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00027854 File Offset: 0x00025A54
		[NullableContext(1)]
		private static JsonPropertyInfo[] ProfileSavePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ProfileSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ProfileSave)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((ProfileSave)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ProfileSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(ProfileSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((ProfileSave)obj).LastProfileId;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((ProfileSave)obj).LastProfileId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "LastProfileId";
			jsonPropertyInfoValues.JsonPropertyName = "last_profile_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(ProfileSave).GetProperty("LastProfileId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues3);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00027A4C File Offset: 0x00025C4C
		public JsonTypeInfo<SavedProperties> SavedProperties
		{
			get
			{
				JsonTypeInfo<SavedProperties> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedProperties) == null)
				{
					jsonTypeInfo = (this._SavedProperties = (JsonTypeInfo<SavedProperties>)base.Options.GetTypeInfo(typeof(SavedProperties)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00027A88 File Offset: 0x00025C88
		[NullableContext(1)]
		private JsonTypeInfo<SavedProperties> Create_SavedProperties(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties>();
				jsonObjectInfoValues.ObjectCreator = () => new SavedProperties();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertiesPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00027B50 File Offset: 0x00025D50
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertiesPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[7];
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int>>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int>>>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties)obj).ints;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0 })] List<SavedProperties.SavedProperty<int>> value)
			{
				((SavedProperties)obj).ints = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "ints";
			jsonPropertyInfoValues.JsonPropertyName = "ints";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties).GetField("ints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int>>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<int>>>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<bool>>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<bool>>>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties)obj).bools;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0 })] List<SavedProperties.SavedProperty<bool>> value)
			{
				((SavedProperties)obj).bools = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "bools";
			jsonPropertyInfoValues3.JsonPropertyName = "bools";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties).GetField("bools", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<bool>>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<bool>>>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<string>>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<string>>>();
			jsonPropertyInfoValues5.IsProperty = false;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SavedProperties)obj).strings;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0, 1 })] List<SavedProperties.SavedProperty<string>> value)
			{
				((SavedProperties)obj).strings = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "strings";
			jsonPropertyInfoValues5.JsonPropertyName = "strings";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SavedProperties).GetField("strings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<string>>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<string>>>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int[]>>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int[]>>>();
			jsonPropertyInfoValues7.IsProperty = false;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SavedProperties)obj).intArrays;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0, 1 })] List<SavedProperties.SavedProperty<int[]>> value)
			{
				((SavedProperties)obj).intArrays = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "intArrays";
			jsonPropertyInfoValues7.JsonPropertyName = "int_arrays";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SavedProperties).GetField("intArrays", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<int[]>>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<int[]>>>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<ModelId>>> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<ModelId>>>();
			jsonPropertyInfoValues9.IsProperty = false;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SavedProperties)obj).modelIds;
			jsonPropertyInfoValues9.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0, 1 })] List<SavedProperties.SavedProperty<ModelId>> value)
			{
				((SavedProperties)obj).modelIds = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "modelIds";
			jsonPropertyInfoValues9.JsonPropertyName = "model_ids";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SavedProperties).GetField("modelIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<ModelId>>> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<ModelId>>>(options, jsonPropertyInfoValues10);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>>();
			jsonPropertyInfoValues11.IsProperty = false;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SavedProperties)obj).cards;
			jsonPropertyInfoValues11.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0, 1 })] List<SavedProperties.SavedProperty<SerializableCard>> value)
			{
				((SavedProperties)obj).cards = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "cards";
			jsonPropertyInfoValues11.JsonPropertyName = "cards";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SavedProperties).GetField("cards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[5] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<SerializableCard>>>(options, jsonPropertyInfoValues12);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>>();
			jsonPropertyInfoValues13.IsProperty = false;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(SavedProperties);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((SavedProperties)obj).cardArrays;
			jsonPropertyInfoValues13.Setter = delegate(object obj, [Nullable(new byte[] { 2, 0, 1, 1 })] List<SavedProperties.SavedProperty<SerializableCard[]>> value)
			{
				((SavedProperties)obj).cardArrays = value;
			};
			jsonPropertyInfoValues13.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "cardArrays";
			jsonPropertyInfoValues13.JsonPropertyName = "card_arrays";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(SavedProperties).GetField("cardArrays", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[6] = JsonMetadataServices.CreatePropertyInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>>(options, jsonPropertyInfoValues14);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00028258 File Offset: 0x00026458
		public JsonTypeInfo<SavedProperties.SavedProperty<bool>> SavedPropertyBoolean
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<bool>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertyBoolean) == null)
				{
					jsonTypeInfo = (this._SavedPropertyBoolean = (JsonTypeInfo<SavedProperties.SavedProperty<bool>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<bool>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00028294 File Offset: 0x00026494
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<bool>> Create_SavedPropertyBoolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<bool>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<bool>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<bool>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<bool>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<bool>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertyBooleanPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<bool>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<bool>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<bool>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002835C File Offset: 0x0002655C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertyBooleanPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<bool>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<bool>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<bool>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<bool>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<bool>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<bool>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, bool value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<bool>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<bool>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00028570 File Offset: 0x00026770
		public JsonTypeInfo<SavedProperties.SavedProperty<ModelId>> SavedPropertyModelId
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<ModelId>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertyModelId) == null)
				{
					jsonTypeInfo = (this._SavedPropertyModelId = (JsonTypeInfo<SavedProperties.SavedProperty<ModelId>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<ModelId>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000285AC File Offset: 0x000267AC
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<ModelId>> Create_SavedPropertyModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<ModelId>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<ModelId>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<ModelId>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<ModelId>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<ModelId>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertyModelIdPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<ModelId>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<ModelId>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<ModelId>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00028674 File Offset: 0x00026874
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertyModelIdPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<ModelId>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<ModelId>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<ModelId>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<ModelId>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<ModelId>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<ModelId>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<ModelId>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<ModelId>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00028888 File Offset: 0x00026A88
		public JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>> SavedPropertySerializableCardArray
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertySerializableCardArray) == null)
				{
					jsonTypeInfo = (this._SavedPropertySerializableCardArray = (JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<SerializableCard[]>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000288C4 File Offset: 0x00026AC4
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0, 1, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>> Create_SavedPropertySerializableCardArray(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<SerializableCard[]>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard[]>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard[]>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<SerializableCard[]>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertySerializableCardArrayPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard[]>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard[]>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<SerializableCard[]>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002898C File Offset: 0x00026B8C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertySerializableCardArrayPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<SerializableCard[]>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<SerializableCard[]>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<SerializableCard[]>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard[]>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableCard[]> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableCard[]>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<SerializableCard[]>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<SerializableCard[]>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] SerializableCard[] value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<SerializableCard[]>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard[]>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<SerializableCard[]> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SerializableCard[]>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00028BA0 File Offset: 0x00026DA0
		public JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>> SavedPropertySerializableCard
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertySerializableCard) == null)
				{
					jsonTypeInfo = (this._SavedPropertySerializableCard = (JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<SerializableCard>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00028BDC File Offset: 0x00026DDC
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>> Create_SavedPropertySerializableCard(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<SerializableCard>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<SerializableCard>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertySerializableCardPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<SerializableCard>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<SerializableCard>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00028CA4 File Offset: 0x00026EA4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertySerializableCardPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<SerializableCard>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<SerializableCard>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<SerializableCard>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<SerializableCard>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<SerializableCard>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<SerializableCard>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<SerializableCard>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00028EB8 File Offset: 0x000270B8
		public JsonTypeInfo<SavedProperties.SavedProperty<int[]>> SavedPropertyInt32Array
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<int[]>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertyInt32Array) == null)
				{
					jsonTypeInfo = (this._SavedPropertyInt32Array = (JsonTypeInfo<SavedProperties.SavedProperty<int[]>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<int[]>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00028EF4 File Offset: 0x000270F4
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<int[]>> Create_SavedPropertyInt32Array(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<int[]>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<int[]>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<int[]>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<int[]>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<int[]>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertyInt32ArrayPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int[]>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<int[]>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<int[]>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00028FBC File Offset: 0x000271BC
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertyInt32ArrayPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<int[]>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<int[]>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<int[]>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int[]>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int[]> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int[]>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<int[]>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<int[]>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] int[] value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<int[]>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int[]>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int[]> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int[]>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x000291D0 File Offset: 0x000273D0
		public JsonTypeInfo<SavedProperties.SavedProperty<int>> SavedPropertyInt32
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<int>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertyInt32) == null)
				{
					jsonTypeInfo = (this._SavedPropertyInt32 = (JsonTypeInfo<SavedProperties.SavedProperty<int>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<int>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0002920C File Offset: 0x0002740C
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<int>> Create_SavedPropertyInt32(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<int>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<int>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<int>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<int>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<int>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertyInt32PropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<int>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<int>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000292D4 File Offset: 0x000274D4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertyInt32PropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<int>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<int>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<int>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SavedProperties.SavedProperty<int>);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SavedProperties.SavedProperty<int>)obj).value;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<int>>(obj).value = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "value";
			jsonPropertyInfoValues3.JsonPropertyName = null;
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<int>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x000294E8 File Offset: 0x000276E8
		public JsonTypeInfo<SavedProperties.SavedProperty<string>> SavedPropertyString
		{
			get
			{
				JsonTypeInfo<SavedProperties.SavedProperty<string>> jsonTypeInfo;
				if ((jsonTypeInfo = this._SavedPropertyString) == null)
				{
					jsonTypeInfo = (this._SavedPropertyString = (JsonTypeInfo<SavedProperties.SavedProperty<string>>)base.Options.GetTypeInfo(typeof(SavedProperties.SavedProperty<string>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00029524 File Offset: 0x00027724
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<string>> Create_SavedPropertyString(JsonSerializerOptions options)
		{
			JsonTypeInfo<SavedProperties.SavedProperty<string>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SavedProperties.SavedProperty<string>>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SavedProperties.SavedProperty<string>> jsonObjectInfoValues = new JsonObjectInfoValues<SavedProperties.SavedProperty<string>>();
				jsonObjectInfoValues.ObjectCreator = () => default(SavedProperties.SavedProperty<string>);
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SavedPropertyStringPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<string>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SavedProperties.SavedProperty<string>> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SavedProperties.SavedProperty<string>>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000295EC File Offset: 0x000277EC
		[NullableContext(1)]
		private static JsonPropertyInfo[] SavedPropertyStringPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<string>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<string>)obj).name;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<string>>(obj).name = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "name";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<string>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SavedProperties.SavedProperty<string>);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SavedProperties.SavedProperty<string>)obj).value;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				Unsafe.Unbox<SavedProperties.SavedProperty<string>>(obj).value = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "value";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SavedProperties.SavedProperty<string>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues3);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000297F0 File Offset: 0x000279F0
		public JsonTypeInfo<SerializableActMap> SerializableActMap
		{
			get
			{
				JsonTypeInfo<SerializableActMap> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableActMap) == null)
				{
					jsonTypeInfo = (this._SerializableActMap = (JsonTypeInfo<SerializableActMap>)base.Options.GetTypeInfo(typeof(SerializableActMap)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0002982C File Offset: 0x00027A2C
		[NullableContext(1)]
		private JsonTypeInfo<SerializableActMap> Create_SerializableActMap(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableActMap> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableActMap>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableActMap> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableActMap>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableActMap();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableActMapPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableActMap).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableActMap> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableActMap>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000298F4 File Offset: 0x00027AF4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableActMapPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[7];
			JsonPropertyInfoValues<List<SerializableMapPoint>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<SerializableMapPoint>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableActMap)obj).Points;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableMapPoint> value)
			{
				((SerializableActMap)obj).Points = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Points";
			jsonPropertyInfoValues.JsonPropertyName = "points";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("Points", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableMapPoint>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableMapPoint>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<SerializableMapPoint>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableMapPoint> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableMapPoint>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableActMap)obj).BossPoint;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SerializableMapPoint value)
			{
				((SerializableActMap)obj).BossPoint = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "BossPoint";
			jsonPropertyInfoValues3.JsonPropertyName = "boss";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("BossPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableMapPoint), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableMapPoint> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SerializableMapPoint>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableMapPoint>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableActMap)obj).SecondBossPoint;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SerializableMapPoint value)
			{
				((SerializableActMap)obj).SecondBossPoint = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "SecondBossPoint";
			jsonPropertyInfoValues3.JsonPropertyName = "second_boss";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("SecondBossPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableMapPoint), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableMapPoint> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<SerializableMapPoint>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableMapPoint>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableActMap)obj).StartingPoint;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SerializableMapPoint value)
			{
				((SerializableActMap)obj).StartingPoint = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "StartingPoint";
			jsonPropertyInfoValues3.JsonPropertyName = "start";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("StartingPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableMapPoint), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableMapPoint> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<SerializableMapPoint>(options, jsonPropertyInfoValues6);
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<List<MapCoord>>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableActMap)obj).StartMapPointCoords;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(2)] List<MapCoord> value)
			{
				((SerializableActMap)obj).StartMapPointCoords = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "StartMapPointCoords";
			jsonPropertyInfoValues7.JsonPropertyName = "start_coords";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("StartMapPointCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<MapCoord>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[4] = JsonMetadataServices.CreatePropertyInfo<List<MapCoord>>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SerializableActMap)obj).GridWidth;
			jsonPropertyInfoValues9.Setter = delegate(object obj, int value)
			{
				((SerializableActMap)obj).GridWidth = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "GridWidth";
			jsonPropertyInfoValues9.JsonPropertyName = "width";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("GridWidth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues10);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SerializableActMap);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SerializableActMap)obj).GridHeight;
			jsonPropertyInfoValues9.Setter = delegate(object obj, int value)
			{
				((SerializableActMap)obj).GridHeight = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "GridHeight";
			jsonPropertyInfoValues9.JsonPropertyName = "height";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SerializableActMap).GetProperty("GridHeight", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues11 = jsonPropertyInfoValues9;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues11);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0002A048 File Offset: 0x00028248
		public JsonTypeInfo<SerializableActModel> SerializableActModel
		{
			get
			{
				JsonTypeInfo<SerializableActModel> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableActModel) == null)
				{
					jsonTypeInfo = (this._SerializableActModel = (JsonTypeInfo<SerializableActModel>)base.Options.GetTypeInfo(typeof(SerializableActModel)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002A084 File Offset: 0x00028284
		[NullableContext(1)]
		private JsonTypeInfo<SerializableActModel> Create_SerializableActModel(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableActModel> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableActModel>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableActModel> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableActModel>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableActModel();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableActModelPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableActModel).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableActModel> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableActModel>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0002A14C File Offset: 0x0002834C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableActModelPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableActModel);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableActModel)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableActModel)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableActModel).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<SerializableRoomSet> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SerializableRoomSet>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableActModel);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableActModel)obj).SerializableRooms;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SerializableRoomSet value)
			{
				((SerializableActModel)obj).SerializableRooms = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "SerializableRooms";
			jsonPropertyInfoValues3.JsonPropertyName = "rooms";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableActModel).GetProperty("SerializableRooms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRoomSet), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRoomSet> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SerializableRoomSet>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableActMap> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<SerializableActMap>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableActModel);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableActModel)obj).SavedMap;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] SerializableActMap value)
			{
				((SerializableActModel)obj).SavedMap = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "SavedMap";
			jsonPropertyInfoValues5.JsonPropertyName = "saved_map";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableActModel).GetProperty("SavedMap", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableActMap), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableActMap> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<SerializableActMap>(options, jsonPropertyInfoValues6);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0002A474 File Offset: 0x00028674
		public JsonTypeInfo<SerializableBadge> SerializableBadge
		{
			get
			{
				JsonTypeInfo<SerializableBadge> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableBadge) == null)
				{
					jsonTypeInfo = (this._SerializableBadge = (JsonTypeInfo<SerializableBadge>)base.Options.GetTypeInfo(typeof(SerializableBadge)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002A4B0 File Offset: 0x000286B0
		[NullableContext(1)]
		private JsonTypeInfo<SerializableBadge> Create_SerializableBadge(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableBadge> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableBadge>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableBadge> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableBadge>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new SerializableBadge
				{
					Id = (string)args[0],
					Rarity = (BadgeRarity)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableBadgePropInit(options);
				JsonObjectInfoValues<SerializableBadge> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<14>__SerializableBadgeCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<14>__SerializableBadgeCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableBadgeCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableBadge).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableBadge> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableBadge>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002A594 File Offset: 0x00028794
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableBadgePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableBadge);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableBadge)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((SerializableBadge)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableBadge).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsRequired = true;
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<BadgeRarity> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<BadgeRarity>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableBadge);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableBadge)obj).Rarity;
			jsonPropertyInfoValues3.Setter = delegate(object obj, BadgeRarity value)
			{
				((SerializableBadge)obj).Rarity = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Rarity";
			jsonPropertyInfoValues3.JsonPropertyName = "rarity";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableBadge).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(BadgeRarity), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<BadgeRarity> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<BadgeRarity>(options, jsonPropertyInfoValues4);
			array[1].IsRequired = true;
			return array;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002A7C0 File Offset: 0x000289C0
		[NullableContext(1)]
		private static JsonParameterInfoValues[] SerializableBadgeCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Id",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "Rarity",
					ParameterType = typeof(BadgeRarity),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0002A848 File Offset: 0x00028A48
		public JsonTypeInfo<SerializableCard> SerializableCard
		{
			get
			{
				JsonTypeInfo<SerializableCard> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableCard) == null)
				{
					jsonTypeInfo = (this._SerializableCard = (JsonTypeInfo<SerializableCard>)base.Options.GetTypeInfo(typeof(SerializableCard)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002A884 File Offset: 0x00028A84
		[NullableContext(1)]
		private JsonTypeInfo<SerializableCard> Create_SerializableCard(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableCard> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableCard>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableCard> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableCard>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableCard();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableCardPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableCard).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableCard> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableCard>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002A94C File Offset: 0x00028B4C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableCardPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableCard);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableCard)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableCard)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableCard).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableCard);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableCard)obj).CurrentUpgradeLevel;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableCard)obj).CurrentUpgradeLevel = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CurrentUpgradeLevel";
			jsonPropertyInfoValues3.JsonPropertyName = "current_upgrade_level";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableCard).GetProperty("CurrentUpgradeLevel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<SerializableEnchantment> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<SerializableEnchantment>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableCard);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableCard)obj).Enchantment;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] SerializableEnchantment value)
			{
				((SerializableCard)obj).Enchantment = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Enchantment";
			jsonPropertyInfoValues5.JsonPropertyName = "enchantment";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableCard).GetProperty("Enchantment", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableEnchantment), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableEnchantment> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<SerializableEnchantment>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<SavedProperties>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableCard);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableCard)obj).Props;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(2)] SavedProperties value)
			{
				((SerializableCard)obj).Props = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "Props";
			jsonPropertyInfoValues7.JsonPropertyName = "props";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableCard).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SavedProperties), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<SavedProperties>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<int?> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<int?>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SerializableCard);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SerializableCard)obj).FloorAddedToDeck;
			jsonPropertyInfoValues9.Setter = delegate(object obj, int? value)
			{
				((SerializableCard)obj).FloorAddedToDeck = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "FloorAddedToDeck";
			jsonPropertyInfoValues9.JsonPropertyName = "floor_added_to_deck";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SerializableCard).GetProperty("FloorAddedToDeck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int?), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int?> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int?>(options, jsonPropertyInfoValues10);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x0002AE5C File Offset: 0x0002905C
		public JsonTypeInfo<SerializableCard[]> SerializableCardArray
		{
			get
			{
				JsonTypeInfo<SerializableCard[]> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableCardArray) == null)
				{
					jsonTypeInfo = (this._SerializableCardArray = (JsonTypeInfo<SerializableCard[]>)base.Options.GetTypeInfo(typeof(SerializableCard[])));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002AE98 File Offset: 0x00029098
		[NullableContext(1)]
		private JsonTypeInfo<SerializableCard[]> Create_SerializableCardArray(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableCard[]> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableCard[]>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<SerializableCard[]> jsonCollectionInfoValues = new JsonCollectionInfoValues<SerializableCard[]>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateArrayInfo<SerializableCard>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0002AEE4 File Offset: 0x000290E4
		public JsonTypeInfo<SerializableEnchantment> SerializableEnchantment
		{
			get
			{
				JsonTypeInfo<SerializableEnchantment> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableEnchantment) == null)
				{
					jsonTypeInfo = (this._SerializableEnchantment = (JsonTypeInfo<SerializableEnchantment>)base.Options.GetTypeInfo(typeof(SerializableEnchantment)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0002AF20 File Offset: 0x00029120
		[NullableContext(1)]
		private JsonTypeInfo<SerializableEnchantment> Create_SerializableEnchantment(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableEnchantment> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableEnchantment>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableEnchantment> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableEnchantment>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableEnchantment();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableEnchantmentPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableEnchantment).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableEnchantment> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableEnchantment>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002AFE8 File Offset: 0x000291E8
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableEnchantmentPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableEnchantment);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableEnchantment)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableEnchantment)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableEnchantment).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableEnchantment);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableEnchantment)obj).Amount;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableEnchantment)obj).Amount = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Amount";
			jsonPropertyInfoValues3.JsonPropertyName = "amount";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableEnchantment).GetProperty("Amount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<SavedProperties>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableEnchantment);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableEnchantment)obj).Props;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] SavedProperties value)
			{
				((SerializableEnchantment)obj).Props = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Props";
			jsonPropertyInfoValues5.JsonPropertyName = "props";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableEnchantment).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SavedProperties), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<SavedProperties>(options, jsonPropertyInfoValues6);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0002B2FC File Offset: 0x000294FC
		public JsonTypeInfo<SerializableExtraRunFields> SerializableExtraRunFields
		{
			get
			{
				JsonTypeInfo<SerializableExtraRunFields> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableExtraRunFields) == null)
				{
					jsonTypeInfo = (this._SerializableExtraRunFields = (JsonTypeInfo<SerializableExtraRunFields>)base.Options.GetTypeInfo(typeof(SerializableExtraRunFields)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002B338 File Offset: 0x00029538
		[NullableContext(1)]
		private JsonTypeInfo<SerializableExtraRunFields> Create_SerializableExtraRunFields(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableExtraRunFields> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableExtraRunFields>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableExtraRunFields> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableExtraRunFields>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableExtraRunFields();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableExtraRunFieldsPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableExtraRunFields).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableExtraRunFields> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableExtraRunFields>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002B400 File Offset: 0x00029600
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableExtraRunFieldsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraRunFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraRunFields)obj).StartedWithNeow;
			jsonPropertyInfoValues.Setter = delegate(object obj, bool value)
			{
				((SerializableExtraRunFields)obj).StartedWithNeow = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "StartedWithNeow";
			jsonPropertyInfoValues.JsonPropertyName = "started_with_neow";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraRunFields).GetProperty("StartedWithNeow", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableExtraRunFields);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableExtraRunFields)obj).TestSubjectKills;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableExtraRunFields)obj).TestSubjectKills = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "TestSubjectKills";
			jsonPropertyInfoValues3.JsonPropertyName = "test_subject_kills";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableExtraRunFields).GetProperty("TestSubjectKills", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraRunFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraRunFields)obj).FreedRepy;
			jsonPropertyInfoValues.Setter = delegate(object obj, bool value)
			{
				((SerializableExtraRunFields)obj).FreedRepy = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "FreedRepy";
			jsonPropertyInfoValues.JsonPropertyName = "freed_repy";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraRunFields).GetProperty("FreedRepy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues5);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0002B70C File Offset: 0x0002990C
		public JsonTypeInfo<SerializableMapPoint> SerializableMapPoint
		{
			get
			{
				JsonTypeInfo<SerializableMapPoint> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableMapPoint) == null)
				{
					jsonTypeInfo = (this._SerializableMapPoint = (JsonTypeInfo<SerializableMapPoint>)base.Options.GetTypeInfo(typeof(SerializableMapPoint)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002B748 File Offset: 0x00029948
		[NullableContext(1)]
		private JsonTypeInfo<SerializableMapPoint> Create_SerializableMapPoint(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableMapPoint> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableMapPoint>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableMapPoint> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableMapPoint>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableMapPoint();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableMapPointPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableMapPoint).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableMapPoint> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableMapPoint>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002B810 File Offset: 0x00029A10
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableMapPointPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<MapCoord> jsonPropertyInfoValues = new JsonPropertyInfoValues<MapCoord>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableMapPoint);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableMapPoint)obj).Coord;
			jsonPropertyInfoValues.Setter = delegate(object obj, MapCoord value)
			{
				((SerializableMapPoint)obj).Coord = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Coord";
			jsonPropertyInfoValues.JsonPropertyName = "coord";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableMapPoint).GetProperty("Coord", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(MapCoord), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<MapCoord> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<MapCoord>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<MapPointType> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<MapPointType>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableMapPoint);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableMapPoint)obj).PointType;
			jsonPropertyInfoValues3.Setter = delegate(object obj, MapPointType value)
			{
				((SerializableMapPoint)obj).PointType = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "PointType";
			jsonPropertyInfoValues3.JsonPropertyName = "type";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableMapPoint).GetProperty("PointType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(MapPointType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<MapPointType> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<MapPointType>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableMapPoint);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableMapPoint)obj).CanBeModified;
			jsonPropertyInfoValues5.Setter = delegate(object obj, bool value)
			{
				((SerializableMapPoint)obj).CanBeModified = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "CanBeModified";
			jsonPropertyInfoValues5.JsonPropertyName = "can_modify";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableMapPoint).GetProperty("CanBeModified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<List<MapCoord>>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableMapPoint);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableMapPoint)obj).ChildCoords;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(2)] List<MapCoord> value)
			{
				((SerializableMapPoint)obj).ChildCoords = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "ChildCoords";
			jsonPropertyInfoValues7.JsonPropertyName = "children";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableMapPoint).GetProperty("ChildCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<MapCoord>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<List<MapCoord>>(options, jsonPropertyInfoValues8);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0002BC24 File Offset: 0x00029E24
		public JsonTypeInfo<SerializableModifier> SerializableModifier
		{
			get
			{
				JsonTypeInfo<SerializableModifier> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableModifier) == null)
				{
					jsonTypeInfo = (this._SerializableModifier = (JsonTypeInfo<SerializableModifier>)base.Options.GetTypeInfo(typeof(SerializableModifier)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002BC60 File Offset: 0x00029E60
		[NullableContext(1)]
		private JsonTypeInfo<SerializableModifier> Create_SerializableModifier(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableModifier> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableModifier>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableModifier> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableModifier>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableModifier();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableModifierPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableModifier).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableModifier> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableModifier>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0002BD28 File Offset: 0x00029F28
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableModifierPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableModifier);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableModifier)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableModifier)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableModifier).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SavedProperties>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableModifier);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableModifier)obj).Props;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SavedProperties value)
			{
				((SerializableModifier)obj).Props = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Props";
			jsonPropertyInfoValues3.JsonPropertyName = "props";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableModifier).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SavedProperties), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SavedProperties>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0002BF2C File Offset: 0x0002A12C
		public JsonTypeInfo<SerializablePlayer> SerializablePlayer
		{
			get
			{
				JsonTypeInfo<SerializablePlayer> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializablePlayer) == null)
				{
					jsonTypeInfo = (this._SerializablePlayer = (JsonTypeInfo<SerializablePlayer>)base.Options.GetTypeInfo(typeof(SerializablePlayer)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0002BF68 File Offset: 0x0002A168
		[NullableContext(1)]
		private JsonTypeInfo<SerializablePlayer> Create_SerializablePlayer(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializablePlayer> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializablePlayer>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializablePlayer> jsonObjectInfoValues = new JsonObjectInfoValues<SerializablePlayer>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializablePlayer();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializablePlayerPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializablePlayer).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializablePlayer> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializablePlayer>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0002C030 File Offset: 0x0002A230
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializablePlayerPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[21];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePlayer)obj).CharacterId;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializablePlayer)obj).CharacterId = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "CharacterId";
			jsonPropertyInfoValues.JsonPropertyName = "character_id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("CharacterId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).CurrentHp;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).CurrentHp = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CurrentHp";
			jsonPropertyInfoValues3.JsonPropertyName = "current_hp";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("CurrentHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).MaxHp;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).MaxHp = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxHp";
			jsonPropertyInfoValues3.JsonPropertyName = "max_hp";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("MaxHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).MaxEnergy;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).MaxEnergy = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxEnergy";
			jsonPropertyInfoValues3.JsonPropertyName = "max_energy";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("MaxEnergy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).MaxPotionSlotCount;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).MaxPotionSlotCount = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "MaxPotionSlotCount";
			jsonPropertyInfoValues3.JsonPropertyName = "max_potion_slot_count";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("MaxPotionSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues3;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).Gold;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).Gold = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Gold";
			jsonPropertyInfoValues3.JsonPropertyName = "gold";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Gold", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues3;
			array[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayer)obj).BaseOrbSlotCount;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePlayer)obj).BaseOrbSlotCount = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "BaseOrbSlotCount";
			jsonPropertyInfoValues3.JsonPropertyName = "base_orb_slot_count";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("BaseOrbSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = jsonPropertyInfoValues3;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues9);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializablePlayer)obj).NetId;
			jsonPropertyInfoValues10.Setter = delegate(object obj, ulong value)
			{
				((SerializablePlayer)obj).NetId = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "NetId";
			jsonPropertyInfoValues10.JsonPropertyName = "net_id";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("NetId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ulong), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[7] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues11);
			JsonPropertyInfoValues<List<SerializableCard>> jsonPropertyInfoValues12 = new JsonPropertyInfoValues<List<SerializableCard>>();
			jsonPropertyInfoValues12.IsProperty = true;
			jsonPropertyInfoValues12.IsPublic = true;
			jsonPropertyInfoValues12.IsVirtual = false;
			jsonPropertyInfoValues12.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues12.Converter = null;
			jsonPropertyInfoValues12.Getter = (object obj) => ((SerializablePlayer)obj).Deck;
			jsonPropertyInfoValues12.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableCard> value)
			{
				((SerializablePlayer)obj).Deck = value;
			};
			jsonPropertyInfoValues12.IgnoreCondition = null;
			jsonPropertyInfoValues12.HasJsonInclude = false;
			jsonPropertyInfoValues12.IsExtensionData = false;
			jsonPropertyInfoValues12.NumberHandling = null;
			jsonPropertyInfoValues12.PropertyName = "Deck";
			jsonPropertyInfoValues12.JsonPropertyName = "deck";
			jsonPropertyInfoValues12.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableCard>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableCard>> jsonPropertyInfoValues13 = jsonPropertyInfoValues12;
			array[8] = JsonMetadataServices.CreatePropertyInfo<List<SerializableCard>>(options, jsonPropertyInfoValues13);
			array[8].IsGetNullable = false;
			array[8].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializableRelic>> jsonPropertyInfoValues14 = new JsonPropertyInfoValues<List<SerializableRelic>>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((SerializablePlayer)obj).Relics;
			jsonPropertyInfoValues14.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableRelic> value)
			{
				((SerializablePlayer)obj).Relics = value;
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "Relics";
			jsonPropertyInfoValues14.JsonPropertyName = "relics";
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableRelic>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableRelic>> jsonPropertyInfoValues15 = jsonPropertyInfoValues14;
			array[9] = JsonMetadataServices.CreatePropertyInfo<List<SerializableRelic>>(options, jsonPropertyInfoValues15);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializablePotion>> jsonPropertyInfoValues16 = new JsonPropertyInfoValues<List<SerializablePotion>>();
			jsonPropertyInfoValues16.IsProperty = true;
			jsonPropertyInfoValues16.IsPublic = true;
			jsonPropertyInfoValues16.IsVirtual = false;
			jsonPropertyInfoValues16.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues16.Converter = null;
			jsonPropertyInfoValues16.Getter = (object obj) => ((SerializablePlayer)obj).Potions;
			jsonPropertyInfoValues16.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializablePotion> value)
			{
				((SerializablePlayer)obj).Potions = value;
			};
			jsonPropertyInfoValues16.IgnoreCondition = null;
			jsonPropertyInfoValues16.HasJsonInclude = false;
			jsonPropertyInfoValues16.IsExtensionData = false;
			jsonPropertyInfoValues16.NumberHandling = null;
			jsonPropertyInfoValues16.PropertyName = "Potions";
			jsonPropertyInfoValues16.JsonPropertyName = "potions";
			jsonPropertyInfoValues16.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Potions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializablePotion>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializablePotion>> jsonPropertyInfoValues17 = jsonPropertyInfoValues16;
			array[10] = JsonMetadataServices.CreatePropertyInfo<List<SerializablePotion>>(options, jsonPropertyInfoValues17);
			array[10].IsGetNullable = false;
			array[10].IsSetNullable = false;
			JsonPropertyInfoValues<SerializablePlayerRngSet> jsonPropertyInfoValues18 = new JsonPropertyInfoValues<SerializablePlayerRngSet>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SerializablePlayer)obj).Rng;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(2)] SerializablePlayerRngSet value)
			{
				((SerializablePlayer)obj).Rng = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "Rng";
			jsonPropertyInfoValues18.JsonPropertyName = "rng";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Rng", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializablePlayerRngSet), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializablePlayerRngSet> jsonPropertyInfoValues19 = jsonPropertyInfoValues18;
			array[11] = JsonMetadataServices.CreatePropertyInfo<SerializablePlayerRngSet>(options, jsonPropertyInfoValues19);
			array[11].IsGetNullable = false;
			array[11].IsSetNullable = false;
			JsonPropertyInfoValues<SerializablePlayerOddsSet> jsonPropertyInfoValues20 = new JsonPropertyInfoValues<SerializablePlayerOddsSet>();
			jsonPropertyInfoValues20.IsProperty = true;
			jsonPropertyInfoValues20.IsPublic = true;
			jsonPropertyInfoValues20.IsVirtual = false;
			jsonPropertyInfoValues20.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues20.Converter = null;
			jsonPropertyInfoValues20.Getter = (object obj) => ((SerializablePlayer)obj).Odds;
			jsonPropertyInfoValues20.Setter = delegate(object obj, [Nullable(2)] SerializablePlayerOddsSet value)
			{
				((SerializablePlayer)obj).Odds = value;
			};
			jsonPropertyInfoValues20.IgnoreCondition = null;
			jsonPropertyInfoValues20.HasJsonInclude = false;
			jsonPropertyInfoValues20.IsExtensionData = false;
			jsonPropertyInfoValues20.NumberHandling = null;
			jsonPropertyInfoValues20.PropertyName = "Odds";
			jsonPropertyInfoValues20.JsonPropertyName = "odds";
			jsonPropertyInfoValues20.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("Odds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializablePlayerOddsSet), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializablePlayerOddsSet> jsonPropertyInfoValues21 = jsonPropertyInfoValues20;
			array[12] = JsonMetadataServices.CreatePropertyInfo<SerializablePlayerOddsSet>(options, jsonPropertyInfoValues21);
			array[12].IsGetNullable = false;
			array[12].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableRelicGrabBag> jsonPropertyInfoValues22 = new JsonPropertyInfoValues<SerializableRelicGrabBag>();
			jsonPropertyInfoValues22.IsProperty = true;
			jsonPropertyInfoValues22.IsPublic = true;
			jsonPropertyInfoValues22.IsVirtual = false;
			jsonPropertyInfoValues22.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues22.Converter = null;
			jsonPropertyInfoValues22.Getter = (object obj) => ((SerializablePlayer)obj).RelicGrabBag;
			jsonPropertyInfoValues22.Setter = delegate(object obj, [Nullable(2)] SerializableRelicGrabBag value)
			{
				((SerializablePlayer)obj).RelicGrabBag = value;
			};
			jsonPropertyInfoValues22.IgnoreCondition = null;
			jsonPropertyInfoValues22.HasJsonInclude = false;
			jsonPropertyInfoValues22.IsExtensionData = false;
			jsonPropertyInfoValues22.NumberHandling = null;
			jsonPropertyInfoValues22.PropertyName = "RelicGrabBag";
			jsonPropertyInfoValues22.JsonPropertyName = "relic_grab_bag";
			jsonPropertyInfoValues22.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("RelicGrabBag", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRelicGrabBag), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRelicGrabBag> jsonPropertyInfoValues23 = jsonPropertyInfoValues22;
			array[13] = JsonMetadataServices.CreatePropertyInfo<SerializableRelicGrabBag>(options, jsonPropertyInfoValues23);
			array[13].IsGetNullable = false;
			array[13].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableExtraPlayerFields> jsonPropertyInfoValues24 = new JsonPropertyInfoValues<SerializableExtraPlayerFields>();
			jsonPropertyInfoValues24.IsProperty = true;
			jsonPropertyInfoValues24.IsPublic = true;
			jsonPropertyInfoValues24.IsVirtual = false;
			jsonPropertyInfoValues24.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues24.Converter = null;
			jsonPropertyInfoValues24.Getter = (object obj) => ((SerializablePlayer)obj).ExtraFields;
			jsonPropertyInfoValues24.Setter = delegate(object obj, [Nullable(2)] SerializableExtraPlayerFields value)
			{
				((SerializablePlayer)obj).ExtraFields = value;
			};
			jsonPropertyInfoValues24.IgnoreCondition = null;
			jsonPropertyInfoValues24.HasJsonInclude = false;
			jsonPropertyInfoValues24.IsExtensionData = false;
			jsonPropertyInfoValues24.NumberHandling = null;
			jsonPropertyInfoValues24.PropertyName = "ExtraFields";
			jsonPropertyInfoValues24.JsonPropertyName = "extra_fields";
			jsonPropertyInfoValues24.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("ExtraFields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableExtraPlayerFields), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableExtraPlayerFields> jsonPropertyInfoValues25 = jsonPropertyInfoValues24;
			array[14] = JsonMetadataServices.CreatePropertyInfo<SerializableExtraPlayerFields>(options, jsonPropertyInfoValues25);
			array[14].IsGetNullable = false;
			array[14].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableUnlockState> jsonPropertyInfoValues26 = new JsonPropertyInfoValues<SerializableUnlockState>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SerializablePlayer)obj).UnlockState;
			jsonPropertyInfoValues26.Setter = delegate(object obj, [Nullable(2)] SerializableUnlockState value)
			{
				((SerializablePlayer)obj).UnlockState = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "UnlockState";
			jsonPropertyInfoValues26.JsonPropertyName = "unlock_state";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("UnlockState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableUnlockState), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableUnlockState> jsonPropertyInfoValues27 = jsonPropertyInfoValues26;
			array[15] = JsonMetadataServices.CreatePropertyInfo<SerializableUnlockState>(options, jsonPropertyInfoValues27);
			array[15].IsGetNullable = false;
			array[15].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues28 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues28.IsProperty = true;
			jsonPropertyInfoValues28.IsPublic = true;
			jsonPropertyInfoValues28.IsVirtual = false;
			jsonPropertyInfoValues28.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues28.Converter = null;
			jsonPropertyInfoValues28.Getter = (object obj) => ((SerializablePlayer)obj).DiscoveredCards;
			jsonPropertyInfoValues28.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializablePlayer)obj).DiscoveredCards = value;
			};
			jsonPropertyInfoValues28.IgnoreCondition = null;
			jsonPropertyInfoValues28.HasJsonInclude = false;
			jsonPropertyInfoValues28.IsExtensionData = false;
			jsonPropertyInfoValues28.NumberHandling = null;
			jsonPropertyInfoValues28.PropertyName = "DiscoveredCards";
			jsonPropertyInfoValues28.JsonPropertyName = "discovered_cards";
			jsonPropertyInfoValues28.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("DiscoveredCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues29 = jsonPropertyInfoValues28;
			array[16] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues29);
			array[16].IsGetNullable = false;
			array[16].IsSetNullable = false;
			jsonPropertyInfoValues28 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues28.IsProperty = true;
			jsonPropertyInfoValues28.IsPublic = true;
			jsonPropertyInfoValues28.IsVirtual = false;
			jsonPropertyInfoValues28.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues28.Converter = null;
			jsonPropertyInfoValues28.Getter = (object obj) => ((SerializablePlayer)obj).DiscoveredEnemies;
			jsonPropertyInfoValues28.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializablePlayer)obj).DiscoveredEnemies = value;
			};
			jsonPropertyInfoValues28.IgnoreCondition = null;
			jsonPropertyInfoValues28.HasJsonInclude = false;
			jsonPropertyInfoValues28.IsExtensionData = false;
			jsonPropertyInfoValues28.NumberHandling = null;
			jsonPropertyInfoValues28.PropertyName = "DiscoveredEnemies";
			jsonPropertyInfoValues28.JsonPropertyName = "discovered_enemies";
			jsonPropertyInfoValues28.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("DiscoveredEnemies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues30 = jsonPropertyInfoValues28;
			array[17] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues30);
			array[17].IsGetNullable = false;
			array[17].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues31 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues31.IsProperty = true;
			jsonPropertyInfoValues31.IsPublic = true;
			jsonPropertyInfoValues31.IsVirtual = false;
			jsonPropertyInfoValues31.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues31.Converter = (JsonConverter<List<string>>)MegaCritSerializerContext.ExpandConverter(typeof(List<string>), new EpochIdListConverter(), options, true);
			jsonPropertyInfoValues31.Getter = (object obj) => ((SerializablePlayer)obj).DiscoveredEpochs;
			jsonPropertyInfoValues31.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				((SerializablePlayer)obj).DiscoveredEpochs = value;
			};
			jsonPropertyInfoValues31.IgnoreCondition = null;
			jsonPropertyInfoValues31.HasJsonInclude = false;
			jsonPropertyInfoValues31.IsExtensionData = false;
			jsonPropertyInfoValues31.NumberHandling = null;
			jsonPropertyInfoValues31.PropertyName = "DiscoveredEpochs";
			jsonPropertyInfoValues31.JsonPropertyName = "discovered_epochs";
			jsonPropertyInfoValues31.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("DiscoveredEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues32 = jsonPropertyInfoValues31;
			array[18] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues32);
			array[18].IsGetNullable = false;
			array[18].IsSetNullable = false;
			jsonPropertyInfoValues28 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues28.IsProperty = true;
			jsonPropertyInfoValues28.IsPublic = true;
			jsonPropertyInfoValues28.IsVirtual = false;
			jsonPropertyInfoValues28.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues28.Converter = null;
			jsonPropertyInfoValues28.Getter = (object obj) => ((SerializablePlayer)obj).DiscoveredPotions;
			jsonPropertyInfoValues28.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializablePlayer)obj).DiscoveredPotions = value;
			};
			jsonPropertyInfoValues28.IgnoreCondition = null;
			jsonPropertyInfoValues28.HasJsonInclude = false;
			jsonPropertyInfoValues28.IsExtensionData = false;
			jsonPropertyInfoValues28.NumberHandling = null;
			jsonPropertyInfoValues28.PropertyName = "DiscoveredPotions";
			jsonPropertyInfoValues28.JsonPropertyName = "discovered_potions";
			jsonPropertyInfoValues28.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("DiscoveredPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues33 = jsonPropertyInfoValues28;
			array[19] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues33);
			array[19].IsGetNullable = false;
			array[19].IsSetNullable = false;
			jsonPropertyInfoValues28 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues28.IsProperty = true;
			jsonPropertyInfoValues28.IsPublic = true;
			jsonPropertyInfoValues28.IsVirtual = false;
			jsonPropertyInfoValues28.DeclaringType = typeof(SerializablePlayer);
			jsonPropertyInfoValues28.Converter = null;
			jsonPropertyInfoValues28.Getter = (object obj) => ((SerializablePlayer)obj).DiscoveredRelics;
			jsonPropertyInfoValues28.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializablePlayer)obj).DiscoveredRelics = value;
			};
			jsonPropertyInfoValues28.IgnoreCondition = null;
			jsonPropertyInfoValues28.HasJsonInclude = false;
			jsonPropertyInfoValues28.IsExtensionData = false;
			jsonPropertyInfoValues28.NumberHandling = null;
			jsonPropertyInfoValues28.PropertyName = "DiscoveredRelics";
			jsonPropertyInfoValues28.JsonPropertyName = "discovered_relics";
			jsonPropertyInfoValues28.AttributeProviderFactory = () => typeof(SerializablePlayer).GetProperty("DiscoveredRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues34 = jsonPropertyInfoValues28;
			array[20] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues34);
			array[20].IsGetNullable = false;
			array[20].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
		public JsonTypeInfo<SerializablePlayerOddsSet> SerializablePlayerOddsSet
		{
			get
			{
				JsonTypeInfo<SerializablePlayerOddsSet> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializablePlayerOddsSet) == null)
				{
					jsonTypeInfo = (this._SerializablePlayerOddsSet = (JsonTypeInfo<SerializablePlayerOddsSet>)base.Options.GetTypeInfo(typeof(SerializablePlayerOddsSet)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0002D6E4 File Offset: 0x0002B8E4
		[NullableContext(1)]
		private JsonTypeInfo<SerializablePlayerOddsSet> Create_SerializablePlayerOddsSet(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializablePlayerOddsSet> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializablePlayerOddsSet>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializablePlayerOddsSet> jsonObjectInfoValues = new JsonObjectInfoValues<SerializablePlayerOddsSet>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializablePlayerOddsSet();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializablePlayerOddsSetPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializablePlayerOddsSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializablePlayerOddsSet> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializablePlayerOddsSet>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002D7AC File Offset: 0x0002B9AC
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializablePlayerOddsSetPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<float> jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePlayerOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePlayerOddsSet)obj).CardRarityOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializablePlayerOddsSet)obj).CardRarityOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "CardRarityOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "card_rarity_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePlayerOddsSet).GetProperty("CardRarityOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePlayerOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePlayerOddsSet)obj).PotionRewardOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializablePlayerOddsSet)obj).PotionRewardOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "PotionRewardOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "potion_reward_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePlayerOddsSet).GetProperty("PotionRewardOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues3);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
		public JsonTypeInfo<SerializablePotion> SerializablePotion
		{
			get
			{
				JsonTypeInfo<SerializablePotion> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializablePotion) == null)
				{
					jsonTypeInfo = (this._SerializablePotion = (JsonTypeInfo<SerializablePotion>)base.Options.GetTypeInfo(typeof(SerializablePotion)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0002D9E0 File Offset: 0x0002BBE0
		[NullableContext(1)]
		private JsonTypeInfo<SerializablePotion> Create_SerializablePotion(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializablePotion> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializablePotion>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializablePotion> jsonObjectInfoValues = new JsonObjectInfoValues<SerializablePotion>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializablePotion();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializablePotionPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializablePotion).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializablePotion> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializablePotion>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0002DAA8 File Offset: 0x0002BCA8
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializablePotionPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePotion);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePotion)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializablePotion)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePotion).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePotion);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePotion)obj).SlotIndex;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializablePotion)obj).SlotIndex = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "SlotIndex";
			jsonPropertyInfoValues3.JsonPropertyName = "slot_index";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePotion).GetProperty("SlotIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
		public JsonTypeInfo<SerializableRelic> SerializableRelic
		{
			get
			{
				JsonTypeInfo<SerializableRelic> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRelic) == null)
				{
					jsonTypeInfo = (this._SerializableRelic = (JsonTypeInfo<SerializableRelic>)base.Options.GetTypeInfo(typeof(SerializableRelic)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002DCEC File Offset: 0x0002BEEC
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRelic> Create_SerializableRelic(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRelic> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRelic>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRelic> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRelic>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRelic();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRelicPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRelic).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRelic> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRelic>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0002DDB4 File Offset: 0x0002BFB4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRelicPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRelic);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRelic)obj).Id;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRelic)obj).Id = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRelic).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<SavedProperties>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRelic);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRelic)obj).Props;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] SavedProperties value)
			{
				((SerializableRelic)obj).Props = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Props";
			jsonPropertyInfoValues3.JsonPropertyName = "props";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRelic).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SavedProperties), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SavedProperties> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<SavedProperties>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<int?> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int?>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableRelic);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableRelic)obj).FloorAddedToDeck;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int? value)
			{
				((SerializableRelic)obj).FloorAddedToDeck = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingNull);
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "FloorAddedToDeck";
			jsonPropertyInfoValues5.JsonPropertyName = "floor_added_to_deck";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableRelic).GetProperty("FloorAddedToDeck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int?), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int?> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int?>(options, jsonPropertyInfoValues6);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0002E0C4 File Offset: 0x0002C2C4
		public JsonTypeInfo<SerializableRelicGrabBag> SerializableRelicGrabBag
		{
			get
			{
				JsonTypeInfo<SerializableRelicGrabBag> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRelicGrabBag) == null)
				{
					jsonTypeInfo = (this._SerializableRelicGrabBag = (JsonTypeInfo<SerializableRelicGrabBag>)base.Options.GetTypeInfo(typeof(SerializableRelicGrabBag)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002E100 File Offset: 0x0002C300
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRelicGrabBag> Create_SerializableRelicGrabBag(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRelicGrabBag> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRelicGrabBag>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRelicGrabBag> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRelicGrabBag>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRelicGrabBag();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRelicGrabBagPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRelicGrabBag).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRelicGrabBag> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRelicGrabBag>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002E1C8 File Offset: 0x0002C3C8
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRelicGrabBagPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[1];
			JsonPropertyInfoValues<Dictionary<RelicRarity, List<ModelId>>> jsonPropertyInfoValues = new JsonPropertyInfoValues<Dictionary<RelicRarity, List<ModelId>>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRelicGrabBag);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRelicGrabBag)obj).RelicIdLists;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<RelicRarity, List<ModelId>> value)
			{
				((SerializableRelicGrabBag)obj).RelicIdLists = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "RelicIdLists";
			jsonPropertyInfoValues.JsonPropertyName = "relic_id_lists";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRelicGrabBag).GetProperty("RelicIdLists", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<RelicRarity, List<ModelId>>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<RelicRarity, List<ModelId>>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<Dictionary<RelicRarity, List<ModelId>>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0002E2E0 File Offset: 0x0002C4E0
		public JsonTypeInfo<SerializableReward> SerializableReward
		{
			get
			{
				JsonTypeInfo<SerializableReward> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableReward) == null)
				{
					jsonTypeInfo = (this._SerializableReward = (JsonTypeInfo<SerializableReward>)base.Options.GetTypeInfo(typeof(SerializableReward)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0002E31C File Offset: 0x0002C51C
		[NullableContext(1)]
		private JsonTypeInfo<SerializableReward> Create_SerializableReward(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableReward> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableReward>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableReward> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableReward>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableReward();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRewardPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableReward).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableReward> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableReward>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002E3E4 File Offset: 0x0002C5E4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRewardPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[10];
			JsonPropertyInfoValues<RewardType> jsonPropertyInfoValues = new JsonPropertyInfoValues<RewardType>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableReward)obj).RewardType;
			jsonPropertyInfoValues.Setter = delegate(object obj, RewardType value)
			{
				((SerializableReward)obj).RewardType = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "RewardType";
			jsonPropertyInfoValues.JsonPropertyName = "reward_type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("RewardType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(RewardType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<RewardType> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<RewardType>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableReward)obj).PredeterminedModelId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableReward)obj).PredeterminedModelId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "PredeterminedModelId";
			jsonPropertyInfoValues3.JsonPropertyName = "predetermined_model_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("PredeterminedModelId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<SerializableCard>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableReward)obj).SpecialCard;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(2)] SerializableCard value)
			{
				((SerializableReward)obj).SpecialCard = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "SpecialCard";
			jsonPropertyInfoValues5.JsonPropertyName = "special_card";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("SpecialCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableCard), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableCard> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<SerializableCard>(options, jsonPropertyInfoValues6);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableReward)obj).GoldAmount;
			jsonPropertyInfoValues7.Setter = delegate(object obj, int value)
			{
				((SerializableReward)obj).GoldAmount = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "GoldAmount";
			jsonPropertyInfoValues7.JsonPropertyName = "gold_amount";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("GoldAmount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SerializableReward)obj).WasGoldStolenBack;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SerializableReward)obj).WasGoldStolenBack = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "WasGoldStolenBack";
			jsonPropertyInfoValues9.JsonPropertyName = "was_gold_stolen_back";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("WasGoldStolenBack", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues10);
			JsonPropertyInfoValues<CardCreationSource> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<CardCreationSource>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SerializableReward)obj).Source;
			jsonPropertyInfoValues11.Setter = delegate(object obj, CardCreationSource value)
			{
				((SerializableReward)obj).Source = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "Source";
			jsonPropertyInfoValues11.JsonPropertyName = "source";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(CardCreationSource), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<CardCreationSource> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[5] = JsonMetadataServices.CreatePropertyInfo<CardCreationSource>(options, jsonPropertyInfoValues12);
			JsonPropertyInfoValues<CardRarityOddsType> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<CardRarityOddsType>();
			jsonPropertyInfoValues13.IsProperty = true;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((SerializableReward)obj).RarityOdds;
			jsonPropertyInfoValues13.Setter = delegate(object obj, CardRarityOddsType value)
			{
				((SerializableReward)obj).RarityOdds = value;
			};
			jsonPropertyInfoValues13.IgnoreCondition = null;
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "RarityOdds";
			jsonPropertyInfoValues13.JsonPropertyName = "rarity_odds";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("RarityOdds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(CardRarityOddsType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<CardRarityOddsType> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[6] = JsonMetadataServices.CreatePropertyInfo<CardRarityOddsType>(options, jsonPropertyInfoValues14);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues15 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues15.IsProperty = true;
			jsonPropertyInfoValues15.IsPublic = true;
			jsonPropertyInfoValues15.IsVirtual = false;
			jsonPropertyInfoValues15.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues15.Converter = null;
			jsonPropertyInfoValues15.Getter = (object obj) => ((SerializableReward)obj).CardPoolIds;
			jsonPropertyInfoValues15.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableReward)obj).CardPoolIds = value;
			};
			jsonPropertyInfoValues15.IgnoreCondition = null;
			jsonPropertyInfoValues15.HasJsonInclude = false;
			jsonPropertyInfoValues15.IsExtensionData = false;
			jsonPropertyInfoValues15.NumberHandling = null;
			jsonPropertyInfoValues15.PropertyName = "CardPoolIds";
			jsonPropertyInfoValues15.JsonPropertyName = "card_pools";
			jsonPropertyInfoValues15.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("CardPoolIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues16 = jsonPropertyInfoValues15;
			array[7] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues16);
			array[7].IsGetNullable = false;
			array[7].IsSetNullable = false;
			jsonPropertyInfoValues7 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableReward)obj).OptionCount;
			jsonPropertyInfoValues7.Setter = delegate(object obj, int value)
			{
				((SerializableReward)obj).OptionCount = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "OptionCount";
			jsonPropertyInfoValues7.JsonPropertyName = "option_count";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("OptionCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues17 = jsonPropertyInfoValues7;
			array[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues17);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableReward);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableReward)obj).CustomDescriptionEncounterSourceId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableReward)obj).CustomDescriptionEncounterSourceId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "CustomDescriptionEncounterSourceId";
			jsonPropertyInfoValues3.JsonPropertyName = "custom_description_encounter_source_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableReward).GetProperty("CustomDescriptionEncounterSourceId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues18 = jsonPropertyInfoValues3;
			array[9] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues18);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0002EE4C File Offset: 0x0002D04C
		public JsonTypeInfo<SerializableRoom> SerializableRoom
		{
			get
			{
				JsonTypeInfo<SerializableRoom> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRoom) == null)
				{
					jsonTypeInfo = (this._SerializableRoom = (JsonTypeInfo<SerializableRoom>)base.Options.GetTypeInfo(typeof(SerializableRoom)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002EE88 File Offset: 0x0002D088
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRoom> Create_SerializableRoom(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRoom> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRoom>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRoom> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRoom>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRoom();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRoomPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRoom).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRoom> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRoom>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002EF50 File Offset: 0x0002D150
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRoomPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[9];
			JsonPropertyInfoValues<RoomType> jsonPropertyInfoValues = new JsonPropertyInfoValues<RoomType>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRoom)obj).RoomType;
			jsonPropertyInfoValues.Setter = delegate(object obj, RoomType value)
			{
				((SerializableRoom)obj).RoomType = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "RoomType";
			jsonPropertyInfoValues.JsonPropertyName = "room_type";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("RoomType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(RoomType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<RoomType> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<RoomType>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoom)obj).EncounterId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoom)obj).EncounterId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "EncounterId";
			jsonPropertyInfoValues3.JsonPropertyName = "encounter_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("EncounterId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoom)obj).EventId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoom)obj).EventId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "EventId";
			jsonPropertyInfoValues3.JsonPropertyName = "event_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("EventId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues5);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((SerializableRoom)obj).IsPreFinished;
			jsonPropertyInfoValues6.Setter = delegate(object obj, bool value)
			{
				((SerializableRoom)obj).IsPreFinished = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "IsPreFinished";
			jsonPropertyInfoValues6.JsonPropertyName = "is_pre_finished";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("IsPreFinished", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues7);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues8 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues8.IsProperty = true;
			jsonPropertyInfoValues8.IsPublic = true;
			jsonPropertyInfoValues8.IsVirtual = false;
			jsonPropertyInfoValues8.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues8.Converter = null;
			jsonPropertyInfoValues8.Getter = (object obj) => ((SerializableRoom)obj).GoldProportion;
			jsonPropertyInfoValues8.Setter = delegate(object obj, float value)
			{
				((SerializableRoom)obj).GoldProportion = value;
			};
			jsonPropertyInfoValues8.IgnoreCondition = null;
			jsonPropertyInfoValues8.HasJsonInclude = false;
			jsonPropertyInfoValues8.IsExtensionData = false;
			jsonPropertyInfoValues8.NumberHandling = null;
			jsonPropertyInfoValues8.PropertyName = "GoldProportion";
			jsonPropertyInfoValues8.JsonPropertyName = "reward_proportion";
			jsonPropertyInfoValues8.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("GoldProportion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues9 = jsonPropertyInfoValues8;
			array[4] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues9);
			JsonPropertyInfoValues<Dictionary<ulong, List<SerializableReward>>> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<Dictionary<ulong, List<SerializableReward>>>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializableRoom)obj).ExtraRewards;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<ulong, List<SerializableReward>> value)
			{
				((SerializableRoom)obj).ExtraRewards = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "ExtraRewards";
			jsonPropertyInfoValues10.JsonPropertyName = "extra_rewards";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("ExtraRewards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<ulong, List<SerializableReward>>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<ulong, List<SerializableReward>>> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[5] = JsonMetadataServices.CreatePropertyInfo<Dictionary<ulong, List<SerializableReward>>>(options, jsonPropertyInfoValues11);
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoom)obj).ParentEventId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoom)obj).ParentEventId = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "ParentEventId";
			jsonPropertyInfoValues3.JsonPropertyName = "parent_event_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("ParentEventId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues12 = jsonPropertyInfoValues3;
			array[6] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues6 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((SerializableRoom)obj).ShouldResumeParentEvent;
			jsonPropertyInfoValues6.Setter = delegate(object obj, bool value)
			{
				((SerializableRoom)obj).ShouldResumeParentEvent = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "ShouldResumeParentEvent";
			jsonPropertyInfoValues6.JsonPropertyName = "should_resume_parent_event";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("ShouldResumeParentEvent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues13 = jsonPropertyInfoValues6;
			array[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues13);
			JsonPropertyInfoValues<Dictionary<string, string>> jsonPropertyInfoValues14 = new JsonPropertyInfoValues<Dictionary<string, string>>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(SerializableRoom);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((SerializableRoom)obj).EncounterState;
			jsonPropertyInfoValues14.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<string, string> value)
			{
				((SerializableRoom)obj).EncounterState = value;
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "EncounterState";
			jsonPropertyInfoValues14.JsonPropertyName = "encounter_state";
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(SerializableRoom).GetProperty("EncounterState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<string, string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<string, string>> jsonPropertyInfoValues15 = jsonPropertyInfoValues14;
			array[8] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, jsonPropertyInfoValues15);
			array[8].IsGetNullable = false;
			array[8].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
		public JsonTypeInfo<SerializableRoomSet> SerializableRoomSet
		{
			get
			{
				JsonTypeInfo<SerializableRoomSet> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRoomSet) == null)
				{
					jsonTypeInfo = (this._SerializableRoomSet = (JsonTypeInfo<SerializableRoomSet>)base.Options.GetTypeInfo(typeof(SerializableRoomSet)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002F8DC File Offset: 0x0002DADC
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRoomSet> Create_SerializableRoomSet(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRoomSet> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRoomSet>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRoomSet> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRoomSet>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRoomSet();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRoomSetPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRoomSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRoomSet> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRoomSet>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002F9A4 File Offset: 0x0002DBA4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRoomSetPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[10];
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRoomSet)obj).EventIds;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableRoomSet)obj).EventIds = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "EventIds";
			jsonPropertyInfoValues.JsonPropertyName = "event_ids";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("EventIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoomSet)obj).EventsVisited;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableRoomSet)obj).EventsVisited = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "EventsVisited";
			jsonPropertyInfoValues3.JsonPropertyName = "events_visited";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("EventsVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRoomSet)obj).NormalEncounterIds;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableRoomSet)obj).NormalEncounterIds = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "NormalEncounterIds";
			jsonPropertyInfoValues.JsonPropertyName = "normal_encounter_ids";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("NormalEncounterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues5);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoomSet)obj).NormalEncountersVisited;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableRoomSet)obj).NormalEncountersVisited = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "NormalEncountersVisited";
			jsonPropertyInfoValues3.JsonPropertyName = "normal_encounters_visited";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("NormalEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRoomSet)obj).EliteEncounterIds;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableRoomSet)obj).EliteEncounterIds = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "EliteEncounterIds";
			jsonPropertyInfoValues.JsonPropertyName = "elite_encounter_ids";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("EliteEncounterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues7);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoomSet)obj).EliteEncountersVisited;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableRoomSet)obj).EliteEncountersVisited = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "EliteEncountersVisited";
			jsonPropertyInfoValues3.JsonPropertyName = "elite_encounters_visited";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("EliteEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues8 = jsonPropertyInfoValues3;
			array[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRoomSet)obj).BossEncountersVisited;
			jsonPropertyInfoValues3.Setter = delegate(object obj, int value)
			{
				((SerializableRoomSet)obj).BossEncountersVisited = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "BossEncountersVisited";
			jsonPropertyInfoValues3.JsonPropertyName = "boss_encounters_visited";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("BossEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = jsonPropertyInfoValues3;
			array[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues9);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializableRoomSet)obj).BossId;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoomSet)obj).BossId = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "BossId";
			jsonPropertyInfoValues10.JsonPropertyName = "boss_id";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("BossId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[7] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues11);
			jsonPropertyInfoValues10 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializableRoomSet)obj).SecondBossId;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoomSet)obj).SecondBossId = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "SecondBossId";
			jsonPropertyInfoValues10.JsonPropertyName = "second_boss_id";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("SecondBossId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues12 = jsonPropertyInfoValues10;
			array[8] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues10 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializableRoomSet);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializableRoomSet)obj).AncientId;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableRoomSet)obj).AncientId = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "AncientId";
			jsonPropertyInfoValues10.JsonPropertyName = "ancient_id";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializableRoomSet).GetProperty("AncientId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues13 = jsonPropertyInfoValues10;
			array[9] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues13);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0003040C File Offset: 0x0002E60C
		public JsonTypeInfo<SerializableRunOddsSet> SerializableRunOddsSet
		{
			get
			{
				JsonTypeInfo<SerializableRunOddsSet> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRunOddsSet) == null)
				{
					jsonTypeInfo = (this._SerializableRunOddsSet = (JsonTypeInfo<SerializableRunOddsSet>)base.Options.GetTypeInfo(typeof(SerializableRunOddsSet)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00030448 File Offset: 0x0002E648
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRunOddsSet> Create_SerializableRunOddsSet(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRunOddsSet> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRunOddsSet>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRunOddsSet> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRunOddsSet>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRunOddsSet();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRunOddsSetPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRunOddsSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRunOddsSet> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRunOddsSet>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00030510 File Offset: 0x0002E710
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRunOddsSetPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[4];
			JsonPropertyInfoValues<float> jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRunOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRunOddsSet)obj).UnknownMapPointMonsterOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializableRunOddsSet)obj).UnknownMapPointMonsterOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "UnknownMapPointMonsterOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "unknown_map_point_monster_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRunOddsSet).GetProperty("UnknownMapPointMonsterOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRunOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRunOddsSet)obj).UnknownMapPointEliteOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializableRunOddsSet)obj).UnknownMapPointEliteOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "UnknownMapPointEliteOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "unknown_map_point_elite_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRunOddsSet).GetProperty("UnknownMapPointEliteOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues3);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRunOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRunOddsSet)obj).UnknownMapPointTreasureOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializableRunOddsSet)obj).UnknownMapPointTreasureOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "UnknownMapPointTreasureOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "unknown_map_point_treasure_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRunOddsSet).GetProperty("UnknownMapPointTreasureOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues4 = jsonPropertyInfoValues;
			array[2] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRunOddsSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRunOddsSet)obj).UnknownMapPointShopOddsValue;
			jsonPropertyInfoValues.Setter = delegate(object obj, float value)
			{
				((SerializableRunOddsSet)obj).UnknownMapPointShopOddsValue = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "UnknownMapPointShopOddsValue";
			jsonPropertyInfoValues.JsonPropertyName = "unknown_map_point_shop_odds_value";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRunOddsSet).GetProperty("UnknownMapPointShopOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues5 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues5);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0003092C File Offset: 0x0002EB2C
		public JsonTypeInfo<SerializableRunRngSet> SerializableRunRngSet
		{
			get
			{
				JsonTypeInfo<SerializableRunRngSet> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRunRngSet) == null)
				{
					jsonTypeInfo = (this._SerializableRunRngSet = (JsonTypeInfo<SerializableRunRngSet>)base.Options.GetTypeInfo(typeof(SerializableRunRngSet)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00030968 File Offset: 0x0002EB68
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRunRngSet> Create_SerializableRunRngSet(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRunRngSet> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRunRngSet>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRunRngSet> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRunRngSet>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRunRngSet();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRunRngSetPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRunRngSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRunRngSet> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRunRngSet>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00030A30 File Offset: 0x0002EC30
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRunRngSetPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRunRngSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRunRngSet)obj).Seed;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((SerializableRunRngSet)obj).Seed = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Seed";
			jsonPropertyInfoValues.JsonPropertyName = "seed";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRunRngSet).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<Dictionary<RunRngType, SerializableRng>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<Dictionary<RunRngType, SerializableRng>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRunRngSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRunRngSet)obj).Rngs;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] Dictionary<RunRngType, SerializableRng> value)
			{
				((SerializableRunRngSet)obj).Rngs = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Rngs";
			jsonPropertyInfoValues3.JsonPropertyName = "rngs";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRunRngSet).GetProperty("Rngs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<RunRngType, SerializableRng>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<RunRngType, SerializableRng>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<RunRngType, SerializableRng>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00030C4C File Offset: 0x0002EE4C
		public JsonTypeInfo<SerializableEpoch> SerializableEpoch
		{
			get
			{
				JsonTypeInfo<SerializableEpoch> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableEpoch) == null)
				{
					jsonTypeInfo = (this._SerializableEpoch = (JsonTypeInfo<SerializableEpoch>)base.Options.GetTypeInfo(typeof(SerializableEpoch)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00030C88 File Offset: 0x0002EE88
		[NullableContext(1)]
		private JsonTypeInfo<SerializableEpoch> Create_SerializableEpoch(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableEpoch> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableEpoch>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableEpoch> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableEpoch>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new SerializableEpoch((string)args[0], (EpochState)args[1]);
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableEpochPropInit(options);
				JsonObjectInfoValues<SerializableEpoch> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<15>__SerializableEpochCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<15>__SerializableEpochCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableEpochCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableEpoch).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(string),
					typeof(EpochState)
				}, null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableEpoch> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableEpoch>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00030D6C File Offset: 0x0002EF6C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableEpochPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableEpoch);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableEpoch)obj).Id;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Id";
			jsonPropertyInfoValues.JsonPropertyName = "id";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableEpoch).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			JsonPropertyInfoValues<EpochState> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<EpochState>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableEpoch);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableEpoch)obj).State;
			jsonPropertyInfoValues3.Setter = delegate(object obj, EpochState value)
			{
				((SerializableEpoch)obj).State = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "State";
			jsonPropertyInfoValues3.JsonPropertyName = "state";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableEpoch).GetProperty("State", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(EpochState), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<EpochState> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<EpochState>(options, jsonPropertyInfoValues4);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableEpoch);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableEpoch)obj).ObtainDate;
			jsonPropertyInfoValues5.Setter = delegate(object obj, long value)
			{
				((SerializableEpoch)obj).ObtainDate = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "ObtainDate";
			jsonPropertyInfoValues5.JsonPropertyName = "obtain_date";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableEpoch).GetProperty("ObtainDate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues6);
			return array;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00031070 File Offset: 0x0002F270
		[NullableContext(1)]
		private static JsonParameterInfoValues[] SerializableEpochCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "id",
					ParameterType = typeof(string),
					Position = 0,
					HasDefaultValue = false,
					DefaultValue = null,
					IsNullable = false
				},
				new JsonParameterInfoValues
				{
					Name = "state",
					ParameterType = typeof(EpochState),
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
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00031108 File Offset: 0x0002F308
		public JsonTypeInfo<SerializableExtraPlayerFields> SerializableExtraPlayerFields
		{
			get
			{
				JsonTypeInfo<SerializableExtraPlayerFields> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableExtraPlayerFields) == null)
				{
					jsonTypeInfo = (this._SerializableExtraPlayerFields = (JsonTypeInfo<SerializableExtraPlayerFields>)base.Options.GetTypeInfo(typeof(SerializableExtraPlayerFields)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00031144 File Offset: 0x0002F344
		[NullableContext(1)]
		private JsonTypeInfo<SerializableExtraPlayerFields> Create_SerializableExtraPlayerFields(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableExtraPlayerFields> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableExtraPlayerFields>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableExtraPlayerFields> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableExtraPlayerFields>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableExtraPlayerFields();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableExtraPlayerFieldsPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableExtraPlayerFields> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableExtraPlayerFields>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0003120C File Offset: 0x0002F40C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableExtraPlayerFieldsPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraPlayerFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraPlayerFields)obj).CardShopRemovalsUsed;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableExtraPlayerFields)obj).CardShopRemovalsUsed = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "CardShopRemovalsUsed";
			jsonPropertyInfoValues.JsonPropertyName = "card_shop_removals_used";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetProperty("CardShopRemovalsUsed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraPlayerFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraPlayerFields)obj).WongoPoints;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableExtraPlayerFields)obj).WongoPoints = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "WongoPoints";
			jsonPropertyInfoValues.JsonPropertyName = "wongo_points";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetProperty("WongoPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues3);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(SerializableExtraPlayerFields);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((SerializableExtraPlayerFields)obj).CccomboBadgeUnlocked;
			jsonPropertyInfoValues4.Setter = delegate(object obj, bool value)
			{
				((SerializableExtraPlayerFields)obj).CccomboBadgeUnlocked = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "CccomboBadgeUnlocked";
			jsonPropertyInfoValues4.JsonPropertyName = "ccccombo_badge_unlocked";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetProperty("CccomboBadgeUnlocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraPlayerFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraPlayerFields)obj).DamageDealt;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableExtraPlayerFields)obj).DamageDealt = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "DamageDealt";
			jsonPropertyInfoValues.JsonPropertyName = "damage_dealt";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetProperty("DamageDealt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues;
			array[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableExtraPlayerFields);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableExtraPlayerFields)obj).DebuffsApplied;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableExtraPlayerFields)obj).DebuffsApplied = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "DebuffsApplied";
			jsonPropertyInfoValues.JsonPropertyName = "debuffs_applied";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableExtraPlayerFields).GetProperty("DebuffsApplied", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues7 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues7);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x00031718 File Offset: 0x0002F918
		public JsonTypeInfo<SerializablePlayerRngSet> SerializablePlayerRngSet
		{
			get
			{
				JsonTypeInfo<SerializablePlayerRngSet> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializablePlayerRngSet) == null)
				{
					jsonTypeInfo = (this._SerializablePlayerRngSet = (JsonTypeInfo<SerializablePlayerRngSet>)base.Options.GetTypeInfo(typeof(SerializablePlayerRngSet)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00031754 File Offset: 0x0002F954
		[NullableContext(1)]
		private JsonTypeInfo<SerializablePlayerRngSet> Create_SerializablePlayerRngSet(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializablePlayerRngSet> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializablePlayerRngSet>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializablePlayerRngSet> jsonObjectInfoValues = new JsonObjectInfoValues<SerializablePlayerRngSet>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializablePlayerRngSet();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializablePlayerRngSetPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializablePlayerRngSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializablePlayerRngSet> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializablePlayerRngSet>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0003181C File Offset: 0x0002FA1C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializablePlayerRngSetPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializablePlayerRngSet);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializablePlayerRngSet)obj).Seed;
			jsonPropertyInfoValues.Setter = delegate(object obj, ulong value)
			{
				((SerializablePlayerRngSet)obj).Seed = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Seed";
			jsonPropertyInfoValues.JsonPropertyName = "seed";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializablePlayerRngSet).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ulong), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<Dictionary<PlayerRngType, SerializableRng>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<Dictionary<PlayerRngType, SerializableRng>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializablePlayerRngSet);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializablePlayerRngSet)obj).Rngs;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] Dictionary<PlayerRngType, SerializableRng> value)
			{
				((SerializablePlayerRngSet)obj).Rngs = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Rngs";
			jsonPropertyInfoValues3.JsonPropertyName = "rngs";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializablePlayerRngSet).GetProperty("Rngs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<PlayerRngType, SerializableRng>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<PlayerRngType, SerializableRng>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<PlayerRngType, SerializableRng>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00031A38 File Offset: 0x0002FC38
		public JsonTypeInfo<SerializableProgress> SerializableProgress
		{
			get
			{
				JsonTypeInfo<SerializableProgress> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableProgress) == null)
				{
					jsonTypeInfo = (this._SerializableProgress = (JsonTypeInfo<SerializableProgress>)base.Options.GetTypeInfo(typeof(SerializableProgress)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00031A74 File Offset: 0x0002FC74
		[NullableContext(1)]
		private JsonTypeInfo<SerializableProgress> Create_SerializableProgress(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableProgress> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableProgress>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableProgress> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableProgress>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new SerializableProgress
				{
					UniqueId = (string)args[0]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableProgressPropInit(options);
				JsonObjectInfoValues<SerializableProgress> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<16>__SerializableProgressCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<16>__SerializableProgressCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableProgressCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableProgress).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableProgress> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableProgress>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00031B58 File Offset: 0x0002FD58
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableProgressPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[31];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableProgress)obj).UniqueId;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "UniqueId";
			jsonPropertyInfoValues3.JsonPropertyName = "unique_id";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("UniqueId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<List<CharacterStats>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<List<CharacterStats>>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableProgress)obj).CharStats;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<CharacterStats> value)
			{
				((SerializableProgress)obj).CharStats = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "CharStats";
			jsonPropertyInfoValues5.JsonPropertyName = "character_stats";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("CharStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CharacterStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CharacterStats>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<CharacterStats>>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<List<CardStats>> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<List<CardStats>>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableProgress)obj).CardStats;
			jsonPropertyInfoValues7.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<CardStats> value)
			{
				((SerializableProgress)obj).CardStats = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "CardStats";
			jsonPropertyInfoValues7.JsonPropertyName = "card_stats";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("CardStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<CardStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<CardStats>> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<List<CardStats>>(options, jsonPropertyInfoValues8);
			array[3].IsGetNullable = false;
			array[3].IsSetNullable = false;
			JsonPropertyInfoValues<List<EncounterStats>> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<List<EncounterStats>>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SerializableProgress)obj).EncounterStats;
			jsonPropertyInfoValues9.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<EncounterStats> value)
			{
				((SerializableProgress)obj).EncounterStats = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "EncounterStats";
			jsonPropertyInfoValues9.JsonPropertyName = "encounter_stats";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("EncounterStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<EncounterStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<EncounterStats>> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[4] = JsonMetadataServices.CreatePropertyInfo<List<EncounterStats>>(options, jsonPropertyInfoValues10);
			array[4].IsGetNullable = false;
			array[4].IsSetNullable = false;
			JsonPropertyInfoValues<List<EnemyStats>> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<List<EnemyStats>>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SerializableProgress)obj).EnemyStats;
			jsonPropertyInfoValues11.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<EnemyStats> value)
			{
				((SerializableProgress)obj).EnemyStats = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "EnemyStats";
			jsonPropertyInfoValues11.JsonPropertyName = "enemy_stats";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("EnemyStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<EnemyStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<EnemyStats>> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[5] = JsonMetadataServices.CreatePropertyInfo<List<EnemyStats>>(options, jsonPropertyInfoValues12);
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			JsonPropertyInfoValues<List<AncientStats>> jsonPropertyInfoValues13 = new JsonPropertyInfoValues<List<AncientStats>>();
			jsonPropertyInfoValues13.IsProperty = true;
			jsonPropertyInfoValues13.IsPublic = true;
			jsonPropertyInfoValues13.IsVirtual = false;
			jsonPropertyInfoValues13.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues13.Converter = null;
			jsonPropertyInfoValues13.Getter = (object obj) => ((SerializableProgress)obj).AncientStats;
			jsonPropertyInfoValues13.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<AncientStats> value)
			{
				((SerializableProgress)obj).AncientStats = value;
			};
			jsonPropertyInfoValues13.IgnoreCondition = null;
			jsonPropertyInfoValues13.HasJsonInclude = false;
			jsonPropertyInfoValues13.IsExtensionData = false;
			jsonPropertyInfoValues13.NumberHandling = null;
			jsonPropertyInfoValues13.PropertyName = "AncientStats";
			jsonPropertyInfoValues13.JsonPropertyName = "ancient_stats";
			jsonPropertyInfoValues13.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("AncientStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<AncientStats>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<AncientStats>> jsonPropertyInfoValues14 = jsonPropertyInfoValues13;
			array[6] = JsonMetadataServices.CreatePropertyInfo<List<AncientStats>>(options, jsonPropertyInfoValues14);
			array[6].IsGetNullable = false;
			array[6].IsSetNullable = false;
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues15 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues15.IsProperty = true;
			jsonPropertyInfoValues15.IsPublic = true;
			jsonPropertyInfoValues15.IsVirtual = false;
			jsonPropertyInfoValues15.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues15.Converter = null;
			jsonPropertyInfoValues15.Getter = (object obj) => ((SerializableProgress)obj).EnableFtues;
			jsonPropertyInfoValues15.Setter = delegate(object obj, bool value)
			{
				((SerializableProgress)obj).EnableFtues = value;
			};
			jsonPropertyInfoValues15.IgnoreCondition = null;
			jsonPropertyInfoValues15.HasJsonInclude = false;
			jsonPropertyInfoValues15.IsExtensionData = false;
			jsonPropertyInfoValues15.NumberHandling = null;
			jsonPropertyInfoValues15.PropertyName = "EnableFtues";
			jsonPropertyInfoValues15.JsonPropertyName = "enable_ftues";
			jsonPropertyInfoValues15.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("EnableFtues", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues16 = jsonPropertyInfoValues15;
			array[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues16);
			JsonPropertyInfoValues<List<SerializableEpoch>> jsonPropertyInfoValues17 = new JsonPropertyInfoValues<List<SerializableEpoch>>();
			jsonPropertyInfoValues17.IsProperty = true;
			jsonPropertyInfoValues17.IsPublic = true;
			jsonPropertyInfoValues17.IsVirtual = false;
			jsonPropertyInfoValues17.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues17.Converter = null;
			jsonPropertyInfoValues17.Getter = (object obj) => ((SerializableProgress)obj).Epochs;
			jsonPropertyInfoValues17.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableEpoch> value)
			{
				((SerializableProgress)obj).Epochs = value;
			};
			jsonPropertyInfoValues17.IgnoreCondition = null;
			jsonPropertyInfoValues17.HasJsonInclude = false;
			jsonPropertyInfoValues17.IsExtensionData = false;
			jsonPropertyInfoValues17.NumberHandling = null;
			jsonPropertyInfoValues17.PropertyName = "Epochs";
			jsonPropertyInfoValues17.JsonPropertyName = "epochs";
			jsonPropertyInfoValues17.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("Epochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableEpoch>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableEpoch>> jsonPropertyInfoValues18 = jsonPropertyInfoValues17;
			array[8] = JsonMetadataServices.CreatePropertyInfo<List<SerializableEpoch>>(options, jsonPropertyInfoValues18);
			array[8].IsGetNullable = false;
			array[8].IsSetNullable = false;
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues19 = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues19.IsProperty = true;
			jsonPropertyInfoValues19.IsPublic = true;
			jsonPropertyInfoValues19.IsVirtual = false;
			jsonPropertyInfoValues19.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues19.Converter = null;
			jsonPropertyInfoValues19.Getter = (object obj) => ((SerializableProgress)obj).FtueCompleted;
			jsonPropertyInfoValues19.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				((SerializableProgress)obj).FtueCompleted = value;
			};
			jsonPropertyInfoValues19.IgnoreCondition = null;
			jsonPropertyInfoValues19.HasJsonInclude = false;
			jsonPropertyInfoValues19.IsExtensionData = false;
			jsonPropertyInfoValues19.NumberHandling = null;
			jsonPropertyInfoValues19.PropertyName = "FtueCompleted";
			jsonPropertyInfoValues19.JsonPropertyName = "ftue_completed";
			jsonPropertyInfoValues19.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("FtueCompleted", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues20 = jsonPropertyInfoValues19;
			array[9] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues20);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializableUnlockedAchievement>> jsonPropertyInfoValues21 = new JsonPropertyInfoValues<List<SerializableUnlockedAchievement>>();
			jsonPropertyInfoValues21.IsProperty = true;
			jsonPropertyInfoValues21.IsPublic = true;
			jsonPropertyInfoValues21.IsVirtual = false;
			jsonPropertyInfoValues21.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues21.Converter = null;
			jsonPropertyInfoValues21.Getter = (object obj) => ((SerializableProgress)obj).UnlockedAchievements;
			jsonPropertyInfoValues21.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableUnlockedAchievement> value)
			{
				((SerializableProgress)obj).UnlockedAchievements = value;
			};
			jsonPropertyInfoValues21.IgnoreCondition = null;
			jsonPropertyInfoValues21.HasJsonInclude = false;
			jsonPropertyInfoValues21.IsExtensionData = false;
			jsonPropertyInfoValues21.NumberHandling = null;
			jsonPropertyInfoValues21.PropertyName = "UnlockedAchievements";
			jsonPropertyInfoValues21.JsonPropertyName = "unlocked_achievements";
			jsonPropertyInfoValues21.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("UnlockedAchievements", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableUnlockedAchievement>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableUnlockedAchievement>> jsonPropertyInfoValues22 = jsonPropertyInfoValues21;
			array[10] = JsonMetadataServices.CreatePropertyInfo<List<SerializableUnlockedAchievement>>(options, jsonPropertyInfoValues22);
			array[10].IsGetNullable = false;
			array[10].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SerializableProgress)obj).DiscoveredCards;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableProgress)obj).DiscoveredCards = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "DiscoveredCards";
			jsonPropertyInfoValues23.JsonPropertyName = "discovered_cards";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("DiscoveredCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues24 = jsonPropertyInfoValues23;
			array[11] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues24);
			array[11].IsGetNullable = false;
			array[11].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SerializableProgress)obj).DiscoveredRelics;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableProgress)obj).DiscoveredRelics = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "DiscoveredRelics";
			jsonPropertyInfoValues23.JsonPropertyName = "discovered_relics";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("DiscoveredRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues25 = jsonPropertyInfoValues23;
			array[12] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues25);
			array[12].IsGetNullable = false;
			array[12].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SerializableProgress)obj).DiscoveredEvents;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableProgress)obj).DiscoveredEvents = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "DiscoveredEvents";
			jsonPropertyInfoValues23.JsonPropertyName = "discovered_events";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("DiscoveredEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues26 = jsonPropertyInfoValues23;
			array[13] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues26);
			array[13].IsGetNullable = false;
			array[13].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SerializableProgress)obj).DiscoveredPotions;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableProgress)obj).DiscoveredPotions = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "DiscoveredPotions";
			jsonPropertyInfoValues23.JsonPropertyName = "discovered_potions";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("DiscoveredPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues27 = jsonPropertyInfoValues23;
			array[14] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues27);
			array[14].IsGetNullable = false;
			array[14].IsSetNullable = false;
			jsonPropertyInfoValues23 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SerializableProgress)obj).DiscoveredActs;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableProgress)obj).DiscoveredActs = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "DiscoveredActs";
			jsonPropertyInfoValues23.JsonPropertyName = "discovered_acts";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("DiscoveredActs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues28 = jsonPropertyInfoValues23;
			array[15] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues28);
			array[15].IsGetNullable = false;
			array[15].IsSetNullable = false;
			JsonPropertyInfoValues<long> jsonPropertyInfoValues29 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues29.IsProperty = true;
			jsonPropertyInfoValues29.IsPublic = true;
			jsonPropertyInfoValues29.IsVirtual = false;
			jsonPropertyInfoValues29.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues29.Converter = null;
			jsonPropertyInfoValues29.Getter = (object obj) => ((SerializableProgress)obj).TotalPlaytime;
			jsonPropertyInfoValues29.Setter = delegate(object obj, long value)
			{
				((SerializableProgress)obj).TotalPlaytime = value;
			};
			jsonPropertyInfoValues29.IgnoreCondition = null;
			jsonPropertyInfoValues29.HasJsonInclude = false;
			jsonPropertyInfoValues29.IsExtensionData = false;
			jsonPropertyInfoValues29.NumberHandling = null;
			jsonPropertyInfoValues29.PropertyName = "TotalPlaytime";
			jsonPropertyInfoValues29.JsonPropertyName = "total_playtime";
			jsonPropertyInfoValues29.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues30 = jsonPropertyInfoValues29;
			array[16] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues30);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).TotalUnlocks;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).TotalUnlocks = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "TotalUnlocks";
			jsonPropertyInfoValues.JsonPropertyName = "total_unlocks";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("TotalUnlocks", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues31 = jsonPropertyInfoValues;
			array[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues31);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).CurrentScore;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).CurrentScore = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "CurrentScore";
			jsonPropertyInfoValues.JsonPropertyName = "current_score";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("CurrentScore", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues32 = jsonPropertyInfoValues;
			array[18] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues32);
			jsonPropertyInfoValues29 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues29.IsProperty = true;
			jsonPropertyInfoValues29.IsPublic = true;
			jsonPropertyInfoValues29.IsVirtual = false;
			jsonPropertyInfoValues29.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues29.Converter = null;
			jsonPropertyInfoValues29.Getter = (object obj) => ((SerializableProgress)obj).FloorsClimbed;
			jsonPropertyInfoValues29.Setter = delegate(object obj, long value)
			{
				((SerializableProgress)obj).FloorsClimbed = value;
			};
			jsonPropertyInfoValues29.IgnoreCondition = null;
			jsonPropertyInfoValues29.HasJsonInclude = false;
			jsonPropertyInfoValues29.IsExtensionData = false;
			jsonPropertyInfoValues29.NumberHandling = null;
			jsonPropertyInfoValues29.PropertyName = "FloorsClimbed";
			jsonPropertyInfoValues29.JsonPropertyName = "floors_climbed";
			jsonPropertyInfoValues29.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("FloorsClimbed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues33 = jsonPropertyInfoValues29;
			array[19] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues33);
			jsonPropertyInfoValues29 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues29.IsProperty = true;
			jsonPropertyInfoValues29.IsPublic = true;
			jsonPropertyInfoValues29.IsVirtual = false;
			jsonPropertyInfoValues29.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues29.Converter = null;
			jsonPropertyInfoValues29.Getter = (object obj) => ((SerializableProgress)obj).ArchitectDamage;
			jsonPropertyInfoValues29.Setter = delegate(object obj, long value)
			{
				((SerializableProgress)obj).ArchitectDamage = value;
			};
			jsonPropertyInfoValues29.IgnoreCondition = null;
			jsonPropertyInfoValues29.HasJsonInclude = false;
			jsonPropertyInfoValues29.IsExtensionData = false;
			jsonPropertyInfoValues29.NumberHandling = null;
			jsonPropertyInfoValues29.PropertyName = "ArchitectDamage";
			jsonPropertyInfoValues29.JsonPropertyName = "architect_damage";
			jsonPropertyInfoValues29.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("ArchitectDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues34 = jsonPropertyInfoValues29;
			array[20] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues34);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).WongoPoints;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).WongoPoints = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "WongoPoints";
			jsonPropertyInfoValues.JsonPropertyName = "wongo_points";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("WongoPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues35 = jsonPropertyInfoValues;
			array[21] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues35);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).PreferredMultiplayerAscension;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).PreferredMultiplayerAscension = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "PreferredMultiplayerAscension";
			jsonPropertyInfoValues.JsonPropertyName = "preferred_multiplayer_ascension";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("PreferredMultiplayerAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues36 = jsonPropertyInfoValues;
			array[22] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues36);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).MaxMultiplayerAscension;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).MaxMultiplayerAscension = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "MaxMultiplayerAscension";
			jsonPropertyInfoValues.JsonPropertyName = "max_multiplayer_ascension";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("MaxMultiplayerAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues37 = jsonPropertyInfoValues;
			array[23] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues37);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableProgress)obj).TestSubjectKills;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableProgress)obj).TestSubjectKills = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "TestSubjectKills";
			jsonPropertyInfoValues.JsonPropertyName = "test_subject_kills";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("TestSubjectKills", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues38 = jsonPropertyInfoValues;
			array[24] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues38);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues39 = new JsonPropertyInfoValues<ModelId>();
			jsonPropertyInfoValues39.IsProperty = true;
			jsonPropertyInfoValues39.IsPublic = true;
			jsonPropertyInfoValues39.IsVirtual = false;
			jsonPropertyInfoValues39.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues39.Converter = null;
			jsonPropertyInfoValues39.Getter = (object obj) => ((SerializableProgress)obj).PendingCharacterUnlock;
			jsonPropertyInfoValues39.Setter = delegate(object obj, [Nullable(2)] ModelId value)
			{
				((SerializableProgress)obj).PendingCharacterUnlock = value;
			};
			jsonPropertyInfoValues39.IgnoreCondition = null;
			jsonPropertyInfoValues39.HasJsonInclude = false;
			jsonPropertyInfoValues39.IsExtensionData = false;
			jsonPropertyInfoValues39.NumberHandling = null;
			jsonPropertyInfoValues39.PropertyName = "PendingCharacterUnlock";
			jsonPropertyInfoValues39.JsonPropertyName = "pending_character_unlock";
			jsonPropertyInfoValues39.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("PendingCharacterUnlock", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModelId), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModelId> jsonPropertyInfoValues40 = jsonPropertyInfoValues39;
			array[25] = JsonMetadataServices.CreatePropertyInfo<ModelId>(options, jsonPropertyInfoValues40);
			array[25].IsGetNullable = false;
			array[25].IsSetNullable = false;
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = null;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Wins";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues41 = jsonPropertyInfoValues;
			array[26] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues41);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = null;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Losses";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues42 = jsonPropertyInfoValues;
			array[27] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues42);
			jsonPropertyInfoValues29 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues29.IsProperty = true;
			jsonPropertyInfoValues29.IsPublic = true;
			jsonPropertyInfoValues29.IsVirtual = false;
			jsonPropertyInfoValues29.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues29.Converter = null;
			jsonPropertyInfoValues29.Getter = null;
			jsonPropertyInfoValues29.Setter = null;
			jsonPropertyInfoValues29.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues29.HasJsonInclude = false;
			jsonPropertyInfoValues29.IsExtensionData = false;
			jsonPropertyInfoValues29.NumberHandling = null;
			jsonPropertyInfoValues29.PropertyName = "FastestVictory";
			jsonPropertyInfoValues29.JsonPropertyName = null;
			jsonPropertyInfoValues29.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("FastestVictory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues43 = jsonPropertyInfoValues29;
			array[28] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues43);
			jsonPropertyInfoValues29 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues29.IsProperty = true;
			jsonPropertyInfoValues29.IsPublic = true;
			jsonPropertyInfoValues29.IsVirtual = false;
			jsonPropertyInfoValues29.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues29.Converter = null;
			jsonPropertyInfoValues29.Getter = null;
			jsonPropertyInfoValues29.Setter = null;
			jsonPropertyInfoValues29.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues29.HasJsonInclude = false;
			jsonPropertyInfoValues29.IsExtensionData = false;
			jsonPropertyInfoValues29.NumberHandling = null;
			jsonPropertyInfoValues29.PropertyName = "BestWinStreak";
			jsonPropertyInfoValues29.JsonPropertyName = null;
			jsonPropertyInfoValues29.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("BestWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues44 = jsonPropertyInfoValues29;
			array[29] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues44);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableProgress);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = null;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "NumberOfRuns";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableProgress).GetProperty("NumberOfRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues45 = jsonPropertyInfoValues;
			array[30] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues45);
			return array;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00033AA8 File Offset: 0x00031CA8
		[NullableContext(1)]
		private static JsonParameterInfoValues[] SerializableProgressCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "UniqueId",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00033AF8 File Offset: 0x00031CF8
		public JsonTypeInfo<SerializableRng> SerializableRng
		{
			get
			{
				JsonTypeInfo<SerializableRng> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRng) == null)
				{
					jsonTypeInfo = (this._SerializableRng = (JsonTypeInfo<SerializableRng>)base.Options.GetTypeInfo(typeof(SerializableRng)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00033B34 File Offset: 0x00031D34
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRng> Create_SerializableRng(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRng> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRng>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRng> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRng>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRng();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRngPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRng).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRng> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRng>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00033BFC File Offset: 0x00031DFC
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRngPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[5];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = false;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRng);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRng)obj).counter;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableRng)obj).counter = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "counter";
			jsonPropertyInfoValues.JsonPropertyName = "counter";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRng).GetField("counter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRng);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRng)obj).state0;
			jsonPropertyInfoValues3.Setter = delegate(object obj, ulong value)
			{
				((SerializableRng)obj).state0 = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "state0";
			jsonPropertyInfoValues3.JsonPropertyName = "s0";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRng).GetField("state0", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues4);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRng);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRng)obj).state1;
			jsonPropertyInfoValues3.Setter = delegate(object obj, ulong value)
			{
				((SerializableRng)obj).state1 = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "state1";
			jsonPropertyInfoValues3.JsonPropertyName = "s1";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRng).GetField("state1", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues5 = jsonPropertyInfoValues3;
			array[2] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues5);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRng);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRng)obj).state2;
			jsonPropertyInfoValues3.Setter = delegate(object obj, ulong value)
			{
				((SerializableRng)obj).state2 = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "state2";
			jsonPropertyInfoValues3.JsonPropertyName = "s2";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRng).GetField("state2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues6 = jsonPropertyInfoValues3;
			array[3] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues6);
			jsonPropertyInfoValues3 = new JsonPropertyInfoValues<ulong>();
			jsonPropertyInfoValues3.IsProperty = false;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRng);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRng)obj).state3;
			jsonPropertyInfoValues3.Setter = delegate(object obj, ulong value)
			{
				((SerializableRng)obj).state3 = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "state3";
			jsonPropertyInfoValues3.JsonPropertyName = "s3";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRng).GetField("state3", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			JsonPropertyInfoValues<ulong> jsonPropertyInfoValues7 = jsonPropertyInfoValues3;
			array[4] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, jsonPropertyInfoValues7);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0003411C File Offset: 0x0003231C
		public JsonTypeInfo<SerializableRun> SerializableRun
		{
			get
			{
				JsonTypeInfo<SerializableRun> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableRun) == null)
				{
					jsonTypeInfo = (this._SerializableRun = (JsonTypeInfo<SerializableRun>)base.Options.GetTypeInfo(typeof(SerializableRun)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00034158 File Offset: 0x00032358
		[NullableContext(1)]
		private JsonTypeInfo<SerializableRun> Create_SerializableRun(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableRun> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableRun>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableRun> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableRun>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableRun();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableRunPropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableRun).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableRun> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableRun>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00034220 File Offset: 0x00032420
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableRunPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[24];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRun)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableRun)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			JsonPropertyInfoValues<List<SerializableActModel>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<SerializableActModel>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableRun)obj).Acts;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableActModel> value)
			{
				((SerializableRun)obj).Acts = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "Acts";
			jsonPropertyInfoValues3.JsonPropertyName = "acts";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("Acts", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableActModel>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableActModel>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<SerializableActModel>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializableModifier>> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<List<SerializableModifier>>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableRun)obj).Modifiers;
			jsonPropertyInfoValues5.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializableModifier> value)
			{
				((SerializableRun)obj).Modifiers = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "Modifiers";
			jsonPropertyInfoValues5.JsonPropertyName = "modifiers";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("Modifiers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializableModifier>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializableModifier>> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<List<SerializableModifier>>(options, jsonPropertyInfoValues6);
			array[2].IsGetNullable = false;
			array[2].IsSetNullable = false;
			JsonPropertyInfoValues<DateTimeOffset?> jsonPropertyInfoValues7 = new JsonPropertyInfoValues<DateTimeOffset?>();
			jsonPropertyInfoValues7.IsProperty = true;
			jsonPropertyInfoValues7.IsPublic = true;
			jsonPropertyInfoValues7.IsVirtual = false;
			jsonPropertyInfoValues7.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues7.Converter = null;
			jsonPropertyInfoValues7.Getter = (object obj) => ((SerializableRun)obj).DailyTime;
			jsonPropertyInfoValues7.Setter = delegate(object obj, DateTimeOffset? value)
			{
				((SerializableRun)obj).DailyTime = value;
			};
			jsonPropertyInfoValues7.IgnoreCondition = null;
			jsonPropertyInfoValues7.HasJsonInclude = false;
			jsonPropertyInfoValues7.IsExtensionData = false;
			jsonPropertyInfoValues7.NumberHandling = null;
			jsonPropertyInfoValues7.PropertyName = "DailyTime";
			jsonPropertyInfoValues7.JsonPropertyName = "dailyTime";
			jsonPropertyInfoValues7.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("DailyTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(DateTimeOffset?), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<DateTimeOffset?> jsonPropertyInfoValues8 = jsonPropertyInfoValues7;
			array[3] = JsonMetadataServices.CreatePropertyInfo<DateTimeOffset?>(options, jsonPropertyInfoValues8);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRun)obj).CurrentActIndex;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableRun)obj).CurrentActIndex = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "CurrentActIndex";
			jsonPropertyInfoValues.JsonPropertyName = "current_act_index";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("CurrentActIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues9 = jsonPropertyInfoValues;
			array[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues9);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues10 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues10.IsProperty = true;
			jsonPropertyInfoValues10.IsPublic = true;
			jsonPropertyInfoValues10.IsVirtual = false;
			jsonPropertyInfoValues10.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues10.Converter = null;
			jsonPropertyInfoValues10.Getter = (object obj) => ((SerializableRun)obj).EventsSeen;
			jsonPropertyInfoValues10.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableRun)obj).EventsSeen = value;
			};
			jsonPropertyInfoValues10.IgnoreCondition = null;
			jsonPropertyInfoValues10.HasJsonInclude = false;
			jsonPropertyInfoValues10.IsExtensionData = false;
			jsonPropertyInfoValues10.NumberHandling = null;
			jsonPropertyInfoValues10.PropertyName = "EventsSeen";
			jsonPropertyInfoValues10.JsonPropertyName = "events_seen";
			jsonPropertyInfoValues10.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("EventsSeen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues11 = jsonPropertyInfoValues10;
			array[5] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues11);
			array[5].IsGetNullable = false;
			array[5].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableRoom> jsonPropertyInfoValues12 = new JsonPropertyInfoValues<SerializableRoom>();
			jsonPropertyInfoValues12.IsProperty = true;
			jsonPropertyInfoValues12.IsPublic = true;
			jsonPropertyInfoValues12.IsVirtual = false;
			jsonPropertyInfoValues12.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues12.Converter = null;
			jsonPropertyInfoValues12.Getter = (object obj) => ((SerializableRun)obj).PreFinishedRoom;
			jsonPropertyInfoValues12.Setter = delegate(object obj, [Nullable(2)] SerializableRoom value)
			{
				((SerializableRun)obj).PreFinishedRoom = value;
			};
			jsonPropertyInfoValues12.IgnoreCondition = null;
			jsonPropertyInfoValues12.HasJsonInclude = false;
			jsonPropertyInfoValues12.IsExtensionData = false;
			jsonPropertyInfoValues12.NumberHandling = null;
			jsonPropertyInfoValues12.PropertyName = "PreFinishedRoom";
			jsonPropertyInfoValues12.JsonPropertyName = "pre_finished_room";
			jsonPropertyInfoValues12.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("PreFinishedRoom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRoom), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRoom> jsonPropertyInfoValues13 = jsonPropertyInfoValues12;
			array[6] = JsonMetadataServices.CreatePropertyInfo<SerializableRoom>(options, jsonPropertyInfoValues13);
			JsonPropertyInfoValues<SerializableRunOddsSet> jsonPropertyInfoValues14 = new JsonPropertyInfoValues<SerializableRunOddsSet>();
			jsonPropertyInfoValues14.IsProperty = true;
			jsonPropertyInfoValues14.IsPublic = true;
			jsonPropertyInfoValues14.IsVirtual = false;
			jsonPropertyInfoValues14.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues14.Converter = null;
			jsonPropertyInfoValues14.Getter = (object obj) => ((SerializableRun)obj).SerializableOdds;
			jsonPropertyInfoValues14.Setter = delegate(object obj, [Nullable(2)] SerializableRunOddsSet value)
			{
				((SerializableRun)obj).SerializableOdds = value;
			};
			jsonPropertyInfoValues14.IgnoreCondition = null;
			jsonPropertyInfoValues14.HasJsonInclude = false;
			jsonPropertyInfoValues14.IsExtensionData = false;
			jsonPropertyInfoValues14.NumberHandling = null;
			jsonPropertyInfoValues14.PropertyName = "SerializableOdds";
			jsonPropertyInfoValues14.JsonPropertyName = "odds";
			jsonPropertyInfoValues14.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("SerializableOdds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRunOddsSet), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRunOddsSet> jsonPropertyInfoValues15 = jsonPropertyInfoValues14;
			array[7] = JsonMetadataServices.CreatePropertyInfo<SerializableRunOddsSet>(options, jsonPropertyInfoValues15);
			array[7].IsGetNullable = false;
			array[7].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableRelicGrabBag> jsonPropertyInfoValues16 = new JsonPropertyInfoValues<SerializableRelicGrabBag>();
			jsonPropertyInfoValues16.IsProperty = true;
			jsonPropertyInfoValues16.IsPublic = true;
			jsonPropertyInfoValues16.IsVirtual = false;
			jsonPropertyInfoValues16.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues16.Converter = null;
			jsonPropertyInfoValues16.Getter = (object obj) => ((SerializableRun)obj).SerializableSharedRelicGrabBag;
			jsonPropertyInfoValues16.Setter = delegate(object obj, [Nullable(2)] SerializableRelicGrabBag value)
			{
				((SerializableRun)obj).SerializableSharedRelicGrabBag = value;
			};
			jsonPropertyInfoValues16.IgnoreCondition = null;
			jsonPropertyInfoValues16.HasJsonInclude = false;
			jsonPropertyInfoValues16.IsExtensionData = false;
			jsonPropertyInfoValues16.NumberHandling = null;
			jsonPropertyInfoValues16.PropertyName = "SerializableSharedRelicGrabBag";
			jsonPropertyInfoValues16.JsonPropertyName = "shared_relic_grab_bag";
			jsonPropertyInfoValues16.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("SerializableSharedRelicGrabBag", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRelicGrabBag), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRelicGrabBag> jsonPropertyInfoValues17 = jsonPropertyInfoValues16;
			array[8] = JsonMetadataServices.CreatePropertyInfo<SerializableRelicGrabBag>(options, jsonPropertyInfoValues17);
			array[8].IsGetNullable = false;
			array[8].IsSetNullable = false;
			JsonPropertyInfoValues<List<SerializablePlayer>> jsonPropertyInfoValues18 = new JsonPropertyInfoValues<List<SerializablePlayer>>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SerializableRun)obj).Players;
			jsonPropertyInfoValues18.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<SerializablePlayer> value)
			{
				((SerializableRun)obj).Players = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "Players";
			jsonPropertyInfoValues18.JsonPropertyName = "players";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("Players", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<SerializablePlayer>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<SerializablePlayer>> jsonPropertyInfoValues19 = jsonPropertyInfoValues18;
			array[9] = JsonMetadataServices.CreatePropertyInfo<List<SerializablePlayer>>(options, jsonPropertyInfoValues19);
			array[9].IsGetNullable = false;
			array[9].IsSetNullable = false;
			JsonPropertyInfoValues<SerializableRunRngSet> jsonPropertyInfoValues20 = new JsonPropertyInfoValues<SerializableRunRngSet>();
			jsonPropertyInfoValues20.IsProperty = true;
			jsonPropertyInfoValues20.IsPublic = true;
			jsonPropertyInfoValues20.IsVirtual = false;
			jsonPropertyInfoValues20.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues20.Converter = null;
			jsonPropertyInfoValues20.Getter = (object obj) => ((SerializableRun)obj).SerializableRng;
			jsonPropertyInfoValues20.Setter = delegate(object obj, [Nullable(2)] SerializableRunRngSet value)
			{
				((SerializableRun)obj).SerializableRng = value;
			};
			jsonPropertyInfoValues20.IgnoreCondition = null;
			jsonPropertyInfoValues20.HasJsonInclude = false;
			jsonPropertyInfoValues20.IsExtensionData = false;
			jsonPropertyInfoValues20.NumberHandling = null;
			jsonPropertyInfoValues20.PropertyName = "SerializableRng";
			jsonPropertyInfoValues20.JsonPropertyName = "rng";
			jsonPropertyInfoValues20.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("SerializableRng", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableRunRngSet), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableRunRngSet> jsonPropertyInfoValues21 = jsonPropertyInfoValues20;
			array[10] = JsonMetadataServices.CreatePropertyInfo<SerializableRunRngSet>(options, jsonPropertyInfoValues21);
			array[10].IsGetNullable = false;
			array[10].IsSetNullable = false;
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues22 = new JsonPropertyInfoValues<List<MapCoord>>();
			jsonPropertyInfoValues22.IsProperty = true;
			jsonPropertyInfoValues22.IsPublic = true;
			jsonPropertyInfoValues22.IsVirtual = false;
			jsonPropertyInfoValues22.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues22.Converter = null;
			jsonPropertyInfoValues22.Getter = (object obj) => ((SerializableRun)obj).VisitedMapCoords;
			jsonPropertyInfoValues22.Setter = delegate(object obj, [Nullable(2)] List<MapCoord> value)
			{
				((SerializableRun)obj).VisitedMapCoords = value;
			};
			jsonPropertyInfoValues22.IgnoreCondition = null;
			jsonPropertyInfoValues22.HasJsonInclude = false;
			jsonPropertyInfoValues22.IsExtensionData = false;
			jsonPropertyInfoValues22.NumberHandling = null;
			jsonPropertyInfoValues22.PropertyName = "VisitedMapCoords";
			jsonPropertyInfoValues22.JsonPropertyName = "visited_map_coords";
			jsonPropertyInfoValues22.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("VisitedMapCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<MapCoord>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<MapCoord>> jsonPropertyInfoValues23 = jsonPropertyInfoValues22;
			array[11] = JsonMetadataServices.CreatePropertyInfo<List<MapCoord>>(options, jsonPropertyInfoValues23);
			array[11].IsGetNullable = false;
			array[11].IsSetNullable = false;
			JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>> jsonPropertyInfoValues24 = new JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>>();
			jsonPropertyInfoValues24.IsProperty = true;
			jsonPropertyInfoValues24.IsPublic = true;
			jsonPropertyInfoValues24.IsVirtual = false;
			jsonPropertyInfoValues24.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues24.Converter = null;
			jsonPropertyInfoValues24.Getter = (object obj) => ((SerializableRun)obj).MapPointHistory;
			jsonPropertyInfoValues24.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] List<List<MapPointHistoryEntry>> value)
			{
				((SerializableRun)obj).MapPointHistory = value;
			};
			jsonPropertyInfoValues24.IgnoreCondition = null;
			jsonPropertyInfoValues24.HasJsonInclude = false;
			jsonPropertyInfoValues24.IsExtensionData = false;
			jsonPropertyInfoValues24.NumberHandling = null;
			jsonPropertyInfoValues24.PropertyName = "MapPointHistory";
			jsonPropertyInfoValues24.JsonPropertyName = "map_point_history";
			jsonPropertyInfoValues24.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("MapPointHistory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<List<MapPointHistoryEntry>>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<List<MapPointHistoryEntry>>> jsonPropertyInfoValues25 = jsonPropertyInfoValues24;
			array[12] = JsonMetadataServices.CreatePropertyInfo<List<List<MapPointHistoryEntry>>>(options, jsonPropertyInfoValues25);
			array[12].IsGetNullable = false;
			array[12].IsSetNullable = false;
			JsonPropertyInfoValues<long> jsonPropertyInfoValues26 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SerializableRun)obj).SaveTime;
			jsonPropertyInfoValues26.Setter = delegate(object obj, long value)
			{
				((SerializableRun)obj).SaveTime = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "SaveTime";
			jsonPropertyInfoValues26.JsonPropertyName = "save_time";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("SaveTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues27 = jsonPropertyInfoValues26;
			array[13] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues27);
			jsonPropertyInfoValues26 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SerializableRun)obj).StartTime;
			jsonPropertyInfoValues26.Setter = delegate(object obj, long value)
			{
				((SerializableRun)obj).StartTime = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "StartTime";
			jsonPropertyInfoValues26.JsonPropertyName = "start_time";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("StartTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues28 = jsonPropertyInfoValues26;
			array[14] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues28);
			jsonPropertyInfoValues26 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SerializableRun)obj).RunTime;
			jsonPropertyInfoValues26.Setter = delegate(object obj, long value)
			{
				((SerializableRun)obj).RunTime = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "RunTime";
			jsonPropertyInfoValues26.JsonPropertyName = "run_time";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("RunTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues29 = jsonPropertyInfoValues26;
			array[15] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues29);
			jsonPropertyInfoValues26 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SerializableRun)obj).WinTime;
			jsonPropertyInfoValues26.Setter = delegate(object obj, long value)
			{
				((SerializableRun)obj).WinTime = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "WinTime";
			jsonPropertyInfoValues26.JsonPropertyName = "win_time";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("WinTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues30 = jsonPropertyInfoValues26;
			array[16] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues30);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRun)obj).Ascension;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableRun)obj).Ascension = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Ascension";
			jsonPropertyInfoValues.JsonPropertyName = "ascension";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues31 = jsonPropertyInfoValues;
			array[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues31);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableRun)obj).NumReloads;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SerializableRun)obj).NumReloads = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "NumReloads";
			jsonPropertyInfoValues.JsonPropertyName = "num_reloads";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("NumReloads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues32 = jsonPropertyInfoValues;
			array[18] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues32);
			JsonPropertyInfoValues<PlatformType> jsonPropertyInfoValues33 = new JsonPropertyInfoValues<PlatformType>();
			jsonPropertyInfoValues33.IsProperty = true;
			jsonPropertyInfoValues33.IsPublic = true;
			jsonPropertyInfoValues33.IsVirtual = false;
			jsonPropertyInfoValues33.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues33.Converter = null;
			jsonPropertyInfoValues33.Getter = (object obj) => ((SerializableRun)obj).PlatformType;
			jsonPropertyInfoValues33.Setter = delegate(object obj, PlatformType value)
			{
				((SerializableRun)obj).PlatformType = value;
			};
			jsonPropertyInfoValues33.IgnoreCondition = null;
			jsonPropertyInfoValues33.HasJsonInclude = false;
			jsonPropertyInfoValues33.IsExtensionData = false;
			jsonPropertyInfoValues33.NumberHandling = null;
			jsonPropertyInfoValues33.PropertyName = "PlatformType";
			jsonPropertyInfoValues33.JsonPropertyName = "platform_type";
			jsonPropertyInfoValues33.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("PlatformType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(PlatformType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<PlatformType> jsonPropertyInfoValues34 = jsonPropertyInfoValues33;
			array[19] = JsonMetadataServices.CreatePropertyInfo<PlatformType>(options, jsonPropertyInfoValues34);
			JsonPropertyInfoValues<SerializableMapDrawings> jsonPropertyInfoValues35 = new JsonPropertyInfoValues<SerializableMapDrawings>();
			jsonPropertyInfoValues35.IsProperty = true;
			jsonPropertyInfoValues35.IsPublic = true;
			jsonPropertyInfoValues35.IsVirtual = false;
			jsonPropertyInfoValues35.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues35.Converter = (JsonConverter<SerializableMapDrawings>)MegaCritSerializerContext.ExpandConverter(typeof(SerializableMapDrawings), new SerializableMapDrawingsJsonConverter(), options, true);
			jsonPropertyInfoValues35.Getter = (object obj) => ((SerializableRun)obj).MapDrawings;
			jsonPropertyInfoValues35.Setter = delegate(object obj, [Nullable(2)] SerializableMapDrawings value)
			{
				((SerializableRun)obj).MapDrawings = value;
			};
			jsonPropertyInfoValues35.IgnoreCondition = null;
			jsonPropertyInfoValues35.HasJsonInclude = false;
			jsonPropertyInfoValues35.IsExtensionData = false;
			jsonPropertyInfoValues35.NumberHandling = null;
			jsonPropertyInfoValues35.PropertyName = "MapDrawings";
			jsonPropertyInfoValues35.JsonPropertyName = "map_drawings";
			jsonPropertyInfoValues35.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("MapDrawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableMapDrawings), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableMapDrawings> jsonPropertyInfoValues36 = jsonPropertyInfoValues35;
			array[20] = JsonMetadataServices.CreatePropertyInfo<SerializableMapDrawings>(options, jsonPropertyInfoValues36);
			JsonPropertyInfoValues<SerializableExtraRunFields> jsonPropertyInfoValues37 = new JsonPropertyInfoValues<SerializableExtraRunFields>();
			jsonPropertyInfoValues37.IsProperty = true;
			jsonPropertyInfoValues37.IsPublic = true;
			jsonPropertyInfoValues37.IsVirtual = false;
			jsonPropertyInfoValues37.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues37.Converter = null;
			jsonPropertyInfoValues37.Getter = (object obj) => ((SerializableRun)obj).ExtraFields;
			jsonPropertyInfoValues37.Setter = delegate(object obj, [Nullable(2)] SerializableExtraRunFields value)
			{
				((SerializableRun)obj).ExtraFields = value;
			};
			jsonPropertyInfoValues37.IgnoreCondition = null;
			jsonPropertyInfoValues37.HasJsonInclude = false;
			jsonPropertyInfoValues37.IsExtensionData = false;
			jsonPropertyInfoValues37.NumberHandling = null;
			jsonPropertyInfoValues37.PropertyName = "ExtraFields";
			jsonPropertyInfoValues37.JsonPropertyName = "extra_fields";
			jsonPropertyInfoValues37.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("ExtraFields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(SerializableExtraRunFields), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<SerializableExtraRunFields> jsonPropertyInfoValues38 = jsonPropertyInfoValues37;
			array[21] = JsonMetadataServices.CreatePropertyInfo<SerializableExtraRunFields>(options, jsonPropertyInfoValues38);
			array[21].IsGetNullable = false;
			array[21].IsSetNullable = false;
			JsonPropertyInfoValues<GameMode> jsonPropertyInfoValues39 = new JsonPropertyInfoValues<GameMode>();
			jsonPropertyInfoValues39.IsProperty = true;
			jsonPropertyInfoValues39.IsPublic = true;
			jsonPropertyInfoValues39.IsVirtual = false;
			jsonPropertyInfoValues39.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues39.Converter = null;
			jsonPropertyInfoValues39.Getter = (object obj) => ((SerializableRun)obj).GameMode;
			jsonPropertyInfoValues39.Setter = delegate(object obj, GameMode value)
			{
				((SerializableRun)obj).GameMode = value;
			};
			jsonPropertyInfoValues39.IgnoreCondition = null;
			jsonPropertyInfoValues39.HasJsonInclude = false;
			jsonPropertyInfoValues39.IsExtensionData = false;
			jsonPropertyInfoValues39.NumberHandling = null;
			jsonPropertyInfoValues39.PropertyName = "GameMode";
			jsonPropertyInfoValues39.JsonPropertyName = "game_mode";
			jsonPropertyInfoValues39.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("GameMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(GameMode), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<GameMode> jsonPropertyInfoValues40 = jsonPropertyInfoValues39;
			array[22] = JsonMetadataServices.CreatePropertyInfo<GameMode>(options, jsonPropertyInfoValues40);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableRun);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = null;
			jsonPropertyInfoValues.Setter = null;
			jsonPropertyInfoValues.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.Always);
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "FloorReached";
			jsonPropertyInfoValues.JsonPropertyName = null;
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableRun).GetProperty("FloorReached", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues41 = jsonPropertyInfoValues;
			array[23] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues41);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00035B1C File Offset: 0x00033D1C
		public JsonTypeInfo<SerializableUnlockedAchievement> SerializableUnlockedAchievement
		{
			get
			{
				JsonTypeInfo<SerializableUnlockedAchievement> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableUnlockedAchievement) == null)
				{
					jsonTypeInfo = (this._SerializableUnlockedAchievement = (JsonTypeInfo<SerializableUnlockedAchievement>)base.Options.GetTypeInfo(typeof(SerializableUnlockedAchievement)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00035B58 File Offset: 0x00033D58
		[NullableContext(1)]
		private JsonTypeInfo<SerializableUnlockedAchievement> Create_SerializableUnlockedAchievement(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableUnlockedAchievement> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableUnlockedAchievement>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableUnlockedAchievement> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableUnlockedAchievement>();
				jsonObjectInfoValues.ObjectCreator = null;
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = (object[] args) => new SerializableUnlockedAchievement
				{
					Achievement = (string)args[0],
					UnlockTime = (long)args[1]
				};
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableUnlockedAchievementPropInit(options);
				JsonObjectInfoValues<SerializableUnlockedAchievement> jsonObjectInfoValues2 = jsonObjectInfoValues;
				Func<JsonParameterInfoValues[]> func;
				if ((func = MegaCritSerializerContext.<>O.<17>__SerializableUnlockedAchievementCtorParamInit) == null)
				{
					func = (MegaCritSerializerContext.<>O.<17>__SerializableUnlockedAchievementCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableUnlockedAchievementCtorParamInit));
				}
				jsonObjectInfoValues2.ConstructorParameterMetadataInitializer = func;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableUnlockedAchievement).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableUnlockedAchievement> jsonObjectInfoValues3 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableUnlockedAchievement>(options, jsonObjectInfoValues3);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00035C3C File Offset: 0x00033E3C
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableUnlockedAchievementPropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[2];
			JsonPropertyInfoValues<string> jsonPropertyInfoValues = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableUnlockedAchievement);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableUnlockedAchievement)obj).Achievement;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Achievement";
			jsonPropertyInfoValues.JsonPropertyName = "achievement";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableUnlockedAchievement).GetProperty("Achievement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<long> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<long>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableUnlockedAchievement);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableUnlockedAchievement)obj).UnlockTime;
			jsonPropertyInfoValues3.Setter = delegate(object obj, long value)
			{
				throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "UnlockTime";
			jsonPropertyInfoValues3.JsonPropertyName = "unlock_time";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableUnlockedAchievement).GetProperty("UnlockTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(long), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<long> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<long>(options, jsonPropertyInfoValues4);
			return array;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00035E58 File Offset: 0x00034058
		[NullableContext(1)]
		private static JsonParameterInfoValues[] SerializableUnlockedAchievementCtorParamInit()
		{
			return new JsonParameterInfoValues[]
			{
				new JsonParameterInfoValues
				{
					Name = "Achievement",
					ParameterType = typeof(string),
					Position = 0,
					IsNullable = false,
					IsMemberInitializer = true
				},
				new JsonParameterInfoValues
				{
					Name = "UnlockTime",
					ParameterType = typeof(long),
					Position = 1,
					IsNullable = false,
					IsMemberInitializer = true
				}
			};
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00035EE0 File Offset: 0x000340E0
		public JsonTypeInfo<SettingsSave> SettingsSave
		{
			get
			{
				JsonTypeInfo<SettingsSave> jsonTypeInfo;
				if ((jsonTypeInfo = this._SettingsSave) == null)
				{
					jsonTypeInfo = (this._SettingsSave = (JsonTypeInfo<SettingsSave>)base.Options.GetTypeInfo(typeof(SettingsSave)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00035F1C File Offset: 0x0003411C
		[NullableContext(1)]
		private JsonTypeInfo<SettingsSave> Create_SettingsSave(JsonSerializerOptions options)
		{
			JsonTypeInfo<SettingsSave> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SettingsSave>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SettingsSave> jsonObjectInfoValues = new JsonObjectInfoValues<SettingsSave>();
				jsonObjectInfoValues.ObjectCreator = () => new SettingsSave();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SettingsSavePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SettingsSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SettingsSave> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SettingsSave>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00035FE4 File Offset: 0x000341E4
		[NullableContext(1)]
		private static JsonPropertyInfo[] SettingsSavePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[23];
			JsonPropertyInfoValues<int> jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsSave)obj).SchemaVersion;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SettingsSave)obj).SchemaVersion = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "SchemaVersion";
			jsonPropertyInfoValues.JsonPropertyName = "schema_version";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues2);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsSave)obj).FpsLimit;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SettingsSave)obj).FpsLimit = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "FpsLimit";
			jsonPropertyInfoValues.JsonPropertyName = "fps_limit";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("FpsLimit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues3 = jsonPropertyInfoValues;
			array[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues3);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues4 = new JsonPropertyInfoValues<string>();
			jsonPropertyInfoValues4.IsProperty = true;
			jsonPropertyInfoValues4.IsPublic = true;
			jsonPropertyInfoValues4.IsVirtual = false;
			jsonPropertyInfoValues4.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues4.Converter = null;
			jsonPropertyInfoValues4.Getter = (object obj) => ((SettingsSave)obj).Language;
			jsonPropertyInfoValues4.Setter = delegate(object obj, [Nullable(2)] string value)
			{
				((SettingsSave)obj).Language = value;
			};
			jsonPropertyInfoValues4.IgnoreCondition = null;
			jsonPropertyInfoValues4.HasJsonInclude = false;
			jsonPropertyInfoValues4.IsExtensionData = false;
			jsonPropertyInfoValues4.NumberHandling = null;
			jsonPropertyInfoValues4.PropertyName = "Language";
			jsonPropertyInfoValues4.JsonPropertyName = "language";
			jsonPropertyInfoValues4.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("Language", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(string), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<string> jsonPropertyInfoValues5 = jsonPropertyInfoValues4;
			array[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, jsonPropertyInfoValues5);
			JsonPropertyInfoValues<Vector2I> jsonPropertyInfoValues6 = new JsonPropertyInfoValues<Vector2I>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((SettingsSave)obj).WindowPosition;
			jsonPropertyInfoValues6.Setter = delegate(object obj, Vector2I value)
			{
				((SettingsSave)obj).WindowPosition = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "WindowPosition";
			jsonPropertyInfoValues6.JsonPropertyName = "window_position";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("WindowPosition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Vector2I), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Vector2I> jsonPropertyInfoValues7 = jsonPropertyInfoValues6;
			array[3] = JsonMetadataServices.CreatePropertyInfo<Vector2I>(options, jsonPropertyInfoValues7);
			jsonPropertyInfoValues6 = new JsonPropertyInfoValues<Vector2I>();
			jsonPropertyInfoValues6.IsProperty = true;
			jsonPropertyInfoValues6.IsPublic = true;
			jsonPropertyInfoValues6.IsVirtual = false;
			jsonPropertyInfoValues6.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues6.Converter = null;
			jsonPropertyInfoValues6.Getter = (object obj) => ((SettingsSave)obj).WindowSize;
			jsonPropertyInfoValues6.Setter = delegate(object obj, Vector2I value)
			{
				((SettingsSave)obj).WindowSize = value;
			};
			jsonPropertyInfoValues6.IgnoreCondition = null;
			jsonPropertyInfoValues6.HasJsonInclude = false;
			jsonPropertyInfoValues6.IsExtensionData = false;
			jsonPropertyInfoValues6.NumberHandling = null;
			jsonPropertyInfoValues6.PropertyName = "WindowSize";
			jsonPropertyInfoValues6.JsonPropertyName = "window_size";
			jsonPropertyInfoValues6.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("WindowSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Vector2I), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Vector2I> jsonPropertyInfoValues8 = jsonPropertyInfoValues6;
			array[4] = JsonMetadataServices.CreatePropertyInfo<Vector2I>(options, jsonPropertyInfoValues8);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).Fullscreen;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).Fullscreen = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "Fullscreen";
			jsonPropertyInfoValues9.JsonPropertyName = "fullscreen";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("Fullscreen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues10 = jsonPropertyInfoValues9;
			array[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues10);
			JsonPropertyInfoValues<AspectRatioSetting> jsonPropertyInfoValues11 = new JsonPropertyInfoValues<AspectRatioSetting>();
			jsonPropertyInfoValues11.IsProperty = true;
			jsonPropertyInfoValues11.IsPublic = true;
			jsonPropertyInfoValues11.IsVirtual = false;
			jsonPropertyInfoValues11.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues11.Converter = null;
			jsonPropertyInfoValues11.Getter = (object obj) => ((SettingsSave)obj).AspectRatioSetting;
			jsonPropertyInfoValues11.Setter = delegate(object obj, AspectRatioSetting value)
			{
				((SettingsSave)obj).AspectRatioSetting = value;
			};
			jsonPropertyInfoValues11.IgnoreCondition = null;
			jsonPropertyInfoValues11.HasJsonInclude = false;
			jsonPropertyInfoValues11.IsExtensionData = false;
			jsonPropertyInfoValues11.NumberHandling = null;
			jsonPropertyInfoValues11.PropertyName = "AspectRatioSetting";
			jsonPropertyInfoValues11.JsonPropertyName = "aspect_ratio";
			jsonPropertyInfoValues11.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("AspectRatioSetting", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(AspectRatioSetting), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<AspectRatioSetting> jsonPropertyInfoValues12 = jsonPropertyInfoValues11;
			array[6] = JsonMetadataServices.CreatePropertyInfo<AspectRatioSetting>(options, jsonPropertyInfoValues12);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsSave)obj).TargetDisplay;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SettingsSave)obj).TargetDisplay = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "TargetDisplay";
			jsonPropertyInfoValues.JsonPropertyName = "target_display";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("TargetDisplay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues13 = jsonPropertyInfoValues;
			array[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues13);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).ResizeWindows;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).ResizeWindows = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "ResizeWindows";
			jsonPropertyInfoValues9.JsonPropertyName = "resize_windows";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("ResizeWindows", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues14 = jsonPropertyInfoValues9;
			array[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues14);
			JsonPropertyInfoValues<VSyncType> jsonPropertyInfoValues15 = new JsonPropertyInfoValues<VSyncType>();
			jsonPropertyInfoValues15.IsProperty = true;
			jsonPropertyInfoValues15.IsPublic = true;
			jsonPropertyInfoValues15.IsVirtual = false;
			jsonPropertyInfoValues15.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues15.Converter = null;
			jsonPropertyInfoValues15.Getter = (object obj) => ((SettingsSave)obj).VSync;
			jsonPropertyInfoValues15.Setter = delegate(object obj, VSyncType value)
			{
				((SettingsSave)obj).VSync = value;
			};
			jsonPropertyInfoValues15.IgnoreCondition = null;
			jsonPropertyInfoValues15.HasJsonInclude = false;
			jsonPropertyInfoValues15.IsExtensionData = false;
			jsonPropertyInfoValues15.NumberHandling = null;
			jsonPropertyInfoValues15.PropertyName = "VSync";
			jsonPropertyInfoValues15.JsonPropertyName = "vsync";
			jsonPropertyInfoValues15.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("VSync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(VSyncType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<VSyncType> jsonPropertyInfoValues16 = jsonPropertyInfoValues15;
			array[9] = JsonMetadataServices.CreatePropertyInfo<VSyncType>(options, jsonPropertyInfoValues16);
			jsonPropertyInfoValues = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SettingsSave)obj).Msaa;
			jsonPropertyInfoValues.Setter = delegate(object obj, int value)
			{
				((SettingsSave)obj).Msaa = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "Msaa";
			jsonPropertyInfoValues.JsonPropertyName = "msaa";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("Msaa", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues17 = jsonPropertyInfoValues;
			array[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues17);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues18 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SettingsSave)obj).VolumeBgm;
			jsonPropertyInfoValues18.Setter = delegate(object obj, float value)
			{
				((SettingsSave)obj).VolumeBgm = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "VolumeBgm";
			jsonPropertyInfoValues18.JsonPropertyName = "volume_bgm";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("VolumeBgm", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues19 = jsonPropertyInfoValues18;
			array[11] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues19);
			jsonPropertyInfoValues18 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SettingsSave)obj).VolumeMaster;
			jsonPropertyInfoValues18.Setter = delegate(object obj, float value)
			{
				((SettingsSave)obj).VolumeMaster = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "VolumeMaster";
			jsonPropertyInfoValues18.JsonPropertyName = "volume_master";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("VolumeMaster", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues20 = jsonPropertyInfoValues18;
			array[12] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues20);
			jsonPropertyInfoValues18 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SettingsSave)obj).VolumeSfx;
			jsonPropertyInfoValues18.Setter = delegate(object obj, float value)
			{
				((SettingsSave)obj).VolumeSfx = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "VolumeSfx";
			jsonPropertyInfoValues18.JsonPropertyName = "volume_sfx";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("VolumeSfx", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues21 = jsonPropertyInfoValues18;
			array[13] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues21);
			jsonPropertyInfoValues18 = new JsonPropertyInfoValues<float>();
			jsonPropertyInfoValues18.IsProperty = true;
			jsonPropertyInfoValues18.IsPublic = true;
			jsonPropertyInfoValues18.IsVirtual = false;
			jsonPropertyInfoValues18.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues18.Converter = null;
			jsonPropertyInfoValues18.Getter = (object obj) => ((SettingsSave)obj).VolumeAmbience;
			jsonPropertyInfoValues18.Setter = delegate(object obj, float value)
			{
				((SettingsSave)obj).VolumeAmbience = value;
			};
			jsonPropertyInfoValues18.IgnoreCondition = null;
			jsonPropertyInfoValues18.HasJsonInclude = false;
			jsonPropertyInfoValues18.IsExtensionData = false;
			jsonPropertyInfoValues18.NumberHandling = null;
			jsonPropertyInfoValues18.PropertyName = "VolumeAmbience";
			jsonPropertyInfoValues18.JsonPropertyName = "volume_ambience";
			jsonPropertyInfoValues18.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("VolumeAmbience", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(float), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<float> jsonPropertyInfoValues22 = jsonPropertyInfoValues18;
			array[14] = JsonMetadataServices.CreatePropertyInfo<float>(options, jsonPropertyInfoValues22);
			JsonPropertyInfoValues<ModSettings> jsonPropertyInfoValues23 = new JsonPropertyInfoValues<ModSettings>();
			jsonPropertyInfoValues23.IsProperty = true;
			jsonPropertyInfoValues23.IsPublic = true;
			jsonPropertyInfoValues23.IsVirtual = false;
			jsonPropertyInfoValues23.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues23.Converter = null;
			jsonPropertyInfoValues23.Getter = (object obj) => ((SettingsSave)obj).ModSettings;
			jsonPropertyInfoValues23.Setter = delegate(object obj, [Nullable(2)] ModSettings value)
			{
				((SettingsSave)obj).ModSettings = value;
			};
			jsonPropertyInfoValues23.IgnoreCondition = null;
			jsonPropertyInfoValues23.HasJsonInclude = false;
			jsonPropertyInfoValues23.IsExtensionData = false;
			jsonPropertyInfoValues23.NumberHandling = null;
			jsonPropertyInfoValues23.PropertyName = "ModSettings";
			jsonPropertyInfoValues23.JsonPropertyName = "mod_settings";
			jsonPropertyInfoValues23.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("ModSettings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ModSettings), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ModSettings> jsonPropertyInfoValues24 = jsonPropertyInfoValues23;
			array[15] = JsonMetadataServices.CreatePropertyInfo<ModSettings>(options, jsonPropertyInfoValues24);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).SkipIntroLogo;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).SkipIntroLogo = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "SkipIntroLogo";
			jsonPropertyInfoValues9.JsonPropertyName = "skip_intro_logo";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("SkipIntroLogo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues25 = jsonPropertyInfoValues9;
			array[16] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues25);
			JsonPropertyInfoValues<Dictionary<string, string>> jsonPropertyInfoValues26 = new JsonPropertyInfoValues<Dictionary<string, string>>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SettingsSave)obj).KeyboardMapping;
			jsonPropertyInfoValues26.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<string, string> value)
			{
				((SettingsSave)obj).KeyboardMapping = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "KeyboardMapping";
			jsonPropertyInfoValues26.JsonPropertyName = "keyboard_mapping";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("KeyboardMapping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<string, string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<string, string>> jsonPropertyInfoValues27 = jsonPropertyInfoValues26;
			array[17] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, jsonPropertyInfoValues27);
			array[17].IsGetNullable = false;
			array[17].IsSetNullable = false;
			JsonPropertyInfoValues<ControllerMappingType> jsonPropertyInfoValues28 = new JsonPropertyInfoValues<ControllerMappingType>();
			jsonPropertyInfoValues28.IsProperty = true;
			jsonPropertyInfoValues28.IsPublic = true;
			jsonPropertyInfoValues28.IsVirtual = false;
			jsonPropertyInfoValues28.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues28.Converter = null;
			jsonPropertyInfoValues28.Getter = (object obj) => ((SettingsSave)obj).ControllerMappingType;
			jsonPropertyInfoValues28.Setter = delegate(object obj, ControllerMappingType value)
			{
				((SettingsSave)obj).ControllerMappingType = value;
			};
			jsonPropertyInfoValues28.IgnoreCondition = null;
			jsonPropertyInfoValues28.HasJsonInclude = false;
			jsonPropertyInfoValues28.IsExtensionData = false;
			jsonPropertyInfoValues28.NumberHandling = null;
			jsonPropertyInfoValues28.PropertyName = "ControllerMappingType";
			jsonPropertyInfoValues28.JsonPropertyName = "controller_mapping_type";
			jsonPropertyInfoValues28.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("ControllerMappingType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(ControllerMappingType), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<ControllerMappingType> jsonPropertyInfoValues29 = jsonPropertyInfoValues28;
			array[18] = JsonMetadataServices.CreatePropertyInfo<ControllerMappingType>(options, jsonPropertyInfoValues29);
			jsonPropertyInfoValues26 = new JsonPropertyInfoValues<Dictionary<string, string>>();
			jsonPropertyInfoValues26.IsProperty = true;
			jsonPropertyInfoValues26.IsPublic = true;
			jsonPropertyInfoValues26.IsVirtual = false;
			jsonPropertyInfoValues26.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues26.Converter = null;
			jsonPropertyInfoValues26.Getter = (object obj) => ((SettingsSave)obj).ControllerMapping;
			jsonPropertyInfoValues26.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1, 1 })] Dictionary<string, string> value)
			{
				((SettingsSave)obj).ControllerMapping = value;
			};
			jsonPropertyInfoValues26.IgnoreCondition = null;
			jsonPropertyInfoValues26.HasJsonInclude = false;
			jsonPropertyInfoValues26.IsExtensionData = false;
			jsonPropertyInfoValues26.NumberHandling = null;
			jsonPropertyInfoValues26.PropertyName = "ControllerMapping";
			jsonPropertyInfoValues26.JsonPropertyName = "controller_mapping";
			jsonPropertyInfoValues26.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("ControllerMapping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(Dictionary<string, string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<Dictionary<string, string>> jsonPropertyInfoValues30 = jsonPropertyInfoValues26;
			array[19] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, jsonPropertyInfoValues30);
			array[19].IsGetNullable = false;
			array[19].IsSetNullable = false;
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).LimitFpsInBackground;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).LimitFpsInBackground = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "LimitFpsInBackground";
			jsonPropertyInfoValues9.JsonPropertyName = "limit_fps_in_background";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("LimitFpsInBackground", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues31 = jsonPropertyInfoValues9;
			array[20] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues31);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).FullConsole;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).FullConsole = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = new JsonIgnoreCondition?(JsonIgnoreCondition.WhenWritingDefault);
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "FullConsole";
			jsonPropertyInfoValues9.JsonPropertyName = "full_console";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("FullConsole", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues32 = jsonPropertyInfoValues9;
			array[21] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues32);
			jsonPropertyInfoValues9 = new JsonPropertyInfoValues<bool>();
			jsonPropertyInfoValues9.IsProperty = true;
			jsonPropertyInfoValues9.IsPublic = true;
			jsonPropertyInfoValues9.IsVirtual = false;
			jsonPropertyInfoValues9.DeclaringType = typeof(SettingsSave);
			jsonPropertyInfoValues9.Converter = null;
			jsonPropertyInfoValues9.Getter = (object obj) => ((SettingsSave)obj).SeenEaDisclaimer;
			jsonPropertyInfoValues9.Setter = delegate(object obj, bool value)
			{
				((SettingsSave)obj).SeenEaDisclaimer = value;
			};
			jsonPropertyInfoValues9.IgnoreCondition = null;
			jsonPropertyInfoValues9.HasJsonInclude = false;
			jsonPropertyInfoValues9.IsExtensionData = false;
			jsonPropertyInfoValues9.NumberHandling = null;
			jsonPropertyInfoValues9.PropertyName = "SeenEaDisclaimer";
			jsonPropertyInfoValues9.JsonPropertyName = "seen_ea_disclaimer";
			jsonPropertyInfoValues9.AttributeProviderFactory = () => typeof(SettingsSave).GetProperty("SeenEaDisclaimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(bool), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<bool> jsonPropertyInfoValues33 = jsonPropertyInfoValues9;
			array[22] = JsonMetadataServices.CreatePropertyInfo<bool>(options, jsonPropertyInfoValues33);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0003776C File Offset: 0x0003596C
		public JsonTypeInfo<AspectRatioSetting> AspectRatioSetting
		{
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

		// Token: 0x06000659 RID: 1625 RVA: 0x000377A8 File Offset: 0x000359A8
		[NullableContext(1)]
		private JsonTypeInfo<AspectRatioSetting> Create_AspectRatioSetting(JsonSerializerOptions options)
		{
			JsonTypeInfo<AspectRatioSetting> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<AspectRatioSetting>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<AspectRatioSetting>(options, JsonMetadataServices.GetEnumConverter<AspectRatioSetting>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x000377D4 File Offset: 0x000359D4
		public JsonTypeInfo<FastModeType> FastModeType
		{
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

		// Token: 0x0600065B RID: 1627 RVA: 0x00037810 File Offset: 0x00035A10
		[NullableContext(1)]
		private JsonTypeInfo<FastModeType> Create_FastModeType(JsonSerializerOptions options)
		{
			JsonTypeInfo<FastModeType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<FastModeType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<FastModeType>(options, JsonMetadataServices.GetEnumConverter<FastModeType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0003783C File Offset: 0x00035A3C
		public JsonTypeInfo<VSyncType> VSyncType
		{
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

		// Token: 0x0600065D RID: 1629 RVA: 0x00037878 File Offset: 0x00035A78
		[NullableContext(1)]
		private JsonTypeInfo<VSyncType> Create_VSyncType(JsonSerializerOptions options)
		{
			JsonTypeInfo<VSyncType> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<VSyncType>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<VSyncType>(options, JsonMetadataServices.GetEnumConverter<VSyncType>(options));
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x000378A4 File Offset: 0x00035AA4
		public JsonTypeInfo<SerializableUnlockState> SerializableUnlockState
		{
			get
			{
				JsonTypeInfo<SerializableUnlockState> jsonTypeInfo;
				if ((jsonTypeInfo = this._SerializableUnlockState) == null)
				{
					jsonTypeInfo = (this._SerializableUnlockState = (JsonTypeInfo<SerializableUnlockState>)base.Options.GetTypeInfo(typeof(SerializableUnlockState)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000378E0 File Offset: 0x00035AE0
		[NullableContext(1)]
		private JsonTypeInfo<SerializableUnlockState> Create_SerializableUnlockState(JsonSerializerOptions options)
		{
			JsonTypeInfo<SerializableUnlockState> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<SerializableUnlockState>(options, out jsonTypeInfo))
			{
				JsonObjectInfoValues<SerializableUnlockState> jsonObjectInfoValues = new JsonObjectInfoValues<SerializableUnlockState>();
				jsonObjectInfoValues.ObjectCreator = () => new SerializableUnlockState();
				jsonObjectInfoValues.ObjectWithParameterizedConstructorCreator = null;
				jsonObjectInfoValues.PropertyMetadataInitializer = (JsonSerializerContext _) => MegaCritSerializerContext.SerializableUnlockStatePropInit(options);
				jsonObjectInfoValues.ConstructorParameterMetadataInitializer = null;
				jsonObjectInfoValues.ConstructorAttributeProviderFactory = () => typeof(SerializableUnlockState).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
				jsonObjectInfoValues.SerializeHandler = null;
				JsonObjectInfoValues<SerializableUnlockState> jsonObjectInfoValues2 = jsonObjectInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<SerializableUnlockState>(options, jsonObjectInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000379A8 File Offset: 0x00035BA8
		[NullableContext(1)]
		private static JsonPropertyInfo[] SerializableUnlockStatePropInit(JsonSerializerOptions options)
		{
			JsonPropertyInfo[] array = new JsonPropertyInfo[3];
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues = new JsonPropertyInfoValues<List<string>>();
			jsonPropertyInfoValues.IsProperty = true;
			jsonPropertyInfoValues.IsPublic = true;
			jsonPropertyInfoValues.IsVirtual = false;
			jsonPropertyInfoValues.DeclaringType = typeof(SerializableUnlockState);
			jsonPropertyInfoValues.Converter = null;
			jsonPropertyInfoValues.Getter = (object obj) => ((SerializableUnlockState)obj).UnlockedEpochs;
			jsonPropertyInfoValues.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<string> value)
			{
				((SerializableUnlockState)obj).UnlockedEpochs = value;
			};
			jsonPropertyInfoValues.IgnoreCondition = null;
			jsonPropertyInfoValues.HasJsonInclude = false;
			jsonPropertyInfoValues.IsExtensionData = false;
			jsonPropertyInfoValues.NumberHandling = null;
			jsonPropertyInfoValues.PropertyName = "UnlockedEpochs";
			jsonPropertyInfoValues.JsonPropertyName = "unlocked_epochs";
			jsonPropertyInfoValues.AttributeProviderFactory = () => typeof(SerializableUnlockState).GetProperty("UnlockedEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<string>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<string>> jsonPropertyInfoValues2 = jsonPropertyInfoValues;
			array[0] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, jsonPropertyInfoValues2);
			array[0].IsGetNullable = false;
			array[0].IsSetNullable = false;
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues3 = new JsonPropertyInfoValues<List<ModelId>>();
			jsonPropertyInfoValues3.IsProperty = true;
			jsonPropertyInfoValues3.IsPublic = true;
			jsonPropertyInfoValues3.IsVirtual = false;
			jsonPropertyInfoValues3.DeclaringType = typeof(SerializableUnlockState);
			jsonPropertyInfoValues3.Converter = null;
			jsonPropertyInfoValues3.Getter = (object obj) => ((SerializableUnlockState)obj).EncountersSeen;
			jsonPropertyInfoValues3.Setter = delegate(object obj, [Nullable(new byte[] { 2, 1 })] List<ModelId> value)
			{
				((SerializableUnlockState)obj).EncountersSeen = value;
			};
			jsonPropertyInfoValues3.IgnoreCondition = null;
			jsonPropertyInfoValues3.HasJsonInclude = false;
			jsonPropertyInfoValues3.IsExtensionData = false;
			jsonPropertyInfoValues3.NumberHandling = null;
			jsonPropertyInfoValues3.PropertyName = "EncountersSeen";
			jsonPropertyInfoValues3.JsonPropertyName = "encounters_seen";
			jsonPropertyInfoValues3.AttributeProviderFactory = () => typeof(SerializableUnlockState).GetProperty("EncountersSeen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(List<ModelId>), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<List<ModelId>> jsonPropertyInfoValues4 = jsonPropertyInfoValues3;
			array[1] = JsonMetadataServices.CreatePropertyInfo<List<ModelId>>(options, jsonPropertyInfoValues4);
			array[1].IsGetNullable = false;
			array[1].IsSetNullable = false;
			JsonPropertyInfoValues<int> jsonPropertyInfoValues5 = new JsonPropertyInfoValues<int>();
			jsonPropertyInfoValues5.IsProperty = true;
			jsonPropertyInfoValues5.IsPublic = true;
			jsonPropertyInfoValues5.IsVirtual = false;
			jsonPropertyInfoValues5.DeclaringType = typeof(SerializableUnlockState);
			jsonPropertyInfoValues5.Converter = null;
			jsonPropertyInfoValues5.Getter = (object obj) => ((SerializableUnlockState)obj).NumberOfRuns;
			jsonPropertyInfoValues5.Setter = delegate(object obj, int value)
			{
				((SerializableUnlockState)obj).NumberOfRuns = value;
			};
			jsonPropertyInfoValues5.IgnoreCondition = null;
			jsonPropertyInfoValues5.HasJsonInclude = false;
			jsonPropertyInfoValues5.IsExtensionData = false;
			jsonPropertyInfoValues5.NumberHandling = null;
			jsonPropertyInfoValues5.PropertyName = "NumberOfRuns";
			jsonPropertyInfoValues5.JsonPropertyName = "number_of_runs";
			jsonPropertyInfoValues5.AttributeProviderFactory = () => typeof(SerializableUnlockState).GetProperty("NumberOfRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null);
			JsonPropertyInfoValues<int> jsonPropertyInfoValues6 = jsonPropertyInfoValues5;
			array[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, jsonPropertyInfoValues6);
			return array;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00037CE4 File Offset: 0x00035EE4
		public JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>> DictionaryRelicRarityListModelId
		{
			get
			{
				JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryRelicRarityListModelId) == null)
				{
					jsonTypeInfo = (this._DictionaryRelicRarityListModelId = (JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>>)base.Options.GetTypeInfo(typeof(Dictionary<RelicRarity, List<ModelId>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00037D20 File Offset: 0x00035F20
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>> Create_DictionaryRelicRarityListModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<RelicRarity, List<ModelId>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<RelicRarity, List<ModelId>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<RelicRarity, List<ModelId>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<RelicRarity, List<ModelId>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<RelicRarity, List<ModelId>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<RelicRarity, List<ModelId>>, RelicRarity, List<ModelId>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00037D88 File Offset: 0x00035F88
		public JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>> DictionaryPlayerRngTypeSerializableRng
		{
			get
			{
				JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryPlayerRngTypeSerializableRng) == null)
				{
					jsonTypeInfo = (this._DictionaryPlayerRngTypeSerializableRng = (JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>>)base.Options.GetTypeInfo(typeof(Dictionary<PlayerRngType, SerializableRng>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00037DC4 File Offset: 0x00035FC4
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>> Create_DictionaryPlayerRngTypeSerializableRng(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<PlayerRngType, SerializableRng>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<PlayerRngType, SerializableRng>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<PlayerRngType, SerializableRng>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<PlayerRngType, SerializableRng>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<PlayerRngType, SerializableRng>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<PlayerRngType, SerializableRng>, PlayerRngType, SerializableRng>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00037E2C File Offset: 0x0003602C
		public JsonTypeInfo<Dictionary<RunRngType, SerializableRng>> DictionaryRunRngTypeSerializableRng
		{
			get
			{
				JsonTypeInfo<Dictionary<RunRngType, SerializableRng>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryRunRngTypeSerializableRng) == null)
				{
					jsonTypeInfo = (this._DictionaryRunRngTypeSerializableRng = (JsonTypeInfo<Dictionary<RunRngType, SerializableRng>>)base.Options.GetTypeInfo(typeof(Dictionary<RunRngType, SerializableRng>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00037E68 File Offset: 0x00036068
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<RunRngType, SerializableRng>> Create_DictionaryRunRngTypeSerializableRng(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<RunRngType, SerializableRng>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<RunRngType, SerializableRng>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<RunRngType, SerializableRng>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<RunRngType, SerializableRng>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<RunRngType, SerializableRng>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<RunRngType, SerializableRng>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<RunRngType, SerializableRng>, RunRngType, SerializableRng>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00037ED0 File Offset: 0x000360D0
		public JsonTypeInfo<Dictionary<string, object>> DictionaryStringObject
		{
			get
			{
				JsonTypeInfo<Dictionary<string, object>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryStringObject) == null)
				{
					jsonTypeInfo = (this._DictionaryStringObject = (JsonTypeInfo<Dictionary<string, object>>)base.Options.GetTypeInfo(typeof(Dictionary<string, object>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00037F0C File Offset: 0x0003610C
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<string, object>> Create_DictionaryStringObject(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<string, object>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, object>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<string, object>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, object>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<string, object>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<string, object>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, object>, string, object>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00037F74 File Offset: 0x00036174
		public JsonTypeInfo<Dictionary<string, string>> DictionaryStringString
		{
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

		// Token: 0x0600066A RID: 1642 RVA: 0x00037FB0 File Offset: 0x000361B0
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<string, string>> Create_DictionaryStringString(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<string, string>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, string>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<string, string>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, string>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<string, string>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<string, string>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, string>, string, string>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00038018 File Offset: 0x00036218
		public JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>> DictionaryUInt64ListSerializableReward
		{
			get
			{
				JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._DictionaryUInt64ListSerializableReward) == null)
				{
					jsonTypeInfo = (this._DictionaryUInt64ListSerializableReward = (JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>>)base.Options.GetTypeInfo(typeof(Dictionary<ulong, List<SerializableReward>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00038054 File Offset: 0x00036254
		[NullableContext(1)]
		private JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>> Create_DictionaryUInt64ListSerializableReward(JsonSerializerOptions options)
		{
			JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<ulong, List<SerializableReward>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<Dictionary<ulong, List<SerializableReward>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<Dictionary<ulong, List<SerializableReward>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new Dictionary<ulong, List<SerializableReward>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<Dictionary<ulong, List<SerializableReward>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<ulong, List<SerializableReward>>, ulong, List<SerializableReward>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000380BC File Offset: 0x000362BC
		public JsonTypeInfo<IEnumerable<SerializableBadge>> IEnumerableSerializableBadge
		{
			get
			{
				JsonTypeInfo<IEnumerable<SerializableBadge>> jsonTypeInfo;
				if ((jsonTypeInfo = this._IEnumerableSerializableBadge) == null)
				{
					jsonTypeInfo = (this._IEnumerableSerializableBadge = (JsonTypeInfo<IEnumerable<SerializableBadge>>)base.Options.GetTypeInfo(typeof(IEnumerable<SerializableBadge>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000380F8 File Offset: 0x000362F8
		[NullableContext(1)]
		private JsonTypeInfo<IEnumerable<SerializableBadge>> Create_IEnumerableSerializableBadge(JsonSerializerOptions options)
		{
			JsonTypeInfo<IEnumerable<SerializableBadge>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<SerializableBadge>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<IEnumerable<SerializableBadge>> jsonCollectionInfoValues = new JsonCollectionInfoValues<IEnumerable<SerializableBadge>>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<SerializableBadge>, SerializableBadge>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00038144 File Offset: 0x00036344
		public JsonTypeInfo<IEnumerable<SerializableCard>> IEnumerableSerializableCard
		{
			get
			{
				JsonTypeInfo<IEnumerable<SerializableCard>> jsonTypeInfo;
				if ((jsonTypeInfo = this._IEnumerableSerializableCard) == null)
				{
					jsonTypeInfo = (this._IEnumerableSerializableCard = (JsonTypeInfo<IEnumerable<SerializableCard>>)base.Options.GetTypeInfo(typeof(IEnumerable<SerializableCard>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00038180 File Offset: 0x00036380
		[NullableContext(1)]
		private JsonTypeInfo<IEnumerable<SerializableCard>> Create_IEnumerableSerializableCard(JsonSerializerOptions options)
		{
			JsonTypeInfo<IEnumerable<SerializableCard>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<SerializableCard>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<IEnumerable<SerializableCard>> jsonCollectionInfoValues = new JsonCollectionInfoValues<IEnumerable<SerializableCard>>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<SerializableCard>, SerializableCard>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x000381CC File Offset: 0x000363CC
		public JsonTypeInfo<IEnumerable<SerializablePotion>> IEnumerableSerializablePotion
		{
			get
			{
				JsonTypeInfo<IEnumerable<SerializablePotion>> jsonTypeInfo;
				if ((jsonTypeInfo = this._IEnumerableSerializablePotion) == null)
				{
					jsonTypeInfo = (this._IEnumerableSerializablePotion = (JsonTypeInfo<IEnumerable<SerializablePotion>>)base.Options.GetTypeInfo(typeof(IEnumerable<SerializablePotion>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00038208 File Offset: 0x00036408
		[NullableContext(1)]
		private JsonTypeInfo<IEnumerable<SerializablePotion>> Create_IEnumerableSerializablePotion(JsonSerializerOptions options)
		{
			JsonTypeInfo<IEnumerable<SerializablePotion>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<SerializablePotion>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<IEnumerable<SerializablePotion>> jsonCollectionInfoValues = new JsonCollectionInfoValues<IEnumerable<SerializablePotion>>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<SerializablePotion>, SerializablePotion>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00038254 File Offset: 0x00036454
		public JsonTypeInfo<IEnumerable<SerializableRelic>> IEnumerableSerializableRelic
		{
			get
			{
				JsonTypeInfo<IEnumerable<SerializableRelic>> jsonTypeInfo;
				if ((jsonTypeInfo = this._IEnumerableSerializableRelic) == null)
				{
					jsonTypeInfo = (this._IEnumerableSerializableRelic = (JsonTypeInfo<IEnumerable<SerializableRelic>>)base.Options.GetTypeInfo(typeof(IEnumerable<SerializableRelic>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00038290 File Offset: 0x00036490
		[NullableContext(1)]
		private JsonTypeInfo<IEnumerable<SerializableRelic>> Create_IEnumerableSerializableRelic(JsonSerializerOptions options)
		{
			JsonTypeInfo<IEnumerable<SerializableRelic>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<SerializableRelic>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<IEnumerable<SerializableRelic>> jsonCollectionInfoValues = new JsonCollectionInfoValues<IEnumerable<SerializableRelic>>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<SerializableRelic>, SerializableRelic>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000382DC File Offset: 0x000364DC
		public JsonTypeInfo<List<Vector2>> ListVector2
		{
			get
			{
				JsonTypeInfo<List<Vector2>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListVector2) == null)
				{
					jsonTypeInfo = (this._ListVector2 = (JsonTypeInfo<List<Vector2>>)base.Options.GetTypeInfo(typeof(List<Vector2>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00038318 File Offset: 0x00036518
		[NullableContext(1)]
		private JsonTypeInfo<List<Vector2>> Create_ListVector2(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<Vector2>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<Vector2>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<Vector2>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<Vector2>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<Vector2>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<Vector2>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<Vector2>, Vector2>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00038380 File Offset: 0x00036580
		public JsonTypeInfo<List<MapCoord>> ListMapCoord
		{
			get
			{
				JsonTypeInfo<List<MapCoord>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListMapCoord) == null)
				{
					jsonTypeInfo = (this._ListMapCoord = (JsonTypeInfo<List<MapCoord>>)base.Options.GetTypeInfo(typeof(List<MapCoord>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000383BC File Offset: 0x000365BC
		[NullableContext(1)]
		private JsonTypeInfo<List<MapCoord>> Create_ListMapCoord(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<MapCoord>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MapCoord>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<MapCoord>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<MapCoord>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<MapCoord>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<MapCoord>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MapCoord>, MapCoord>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00038424 File Offset: 0x00036624
		public JsonTypeInfo<List<ModDependency>> ListModDependency
		{
			get
			{
				JsonTypeInfo<List<ModDependency>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListModDependency) == null)
				{
					jsonTypeInfo = (this._ListModDependency = (JsonTypeInfo<List<ModDependency>>)base.Options.GetTypeInfo(typeof(List<ModDependency>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00038460 File Offset: 0x00036660
		[NullableContext(1)]
		private JsonTypeInfo<List<ModDependency>> Create_ListModDependency(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ModDependency>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ModDependency>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ModDependency>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ModDependency>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ModDependency>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<ModDependency>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ModDependency>, ModDependency>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x000384C8 File Offset: 0x000366C8
		public JsonTypeInfo<List<SettingsSaveMod>> ListSettingsSaveMod
		{
			get
			{
				JsonTypeInfo<List<SettingsSaveMod>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSettingsSaveMod) == null)
				{
					jsonTypeInfo = (this._ListSettingsSaveMod = (JsonTypeInfo<List<SettingsSaveMod>>)base.Options.GetTypeInfo(typeof(List<SettingsSaveMod>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00038504 File Offset: 0x00036704
		[NullableContext(1)]
		private JsonTypeInfo<List<SettingsSaveMod>> Create_ListSettingsSaveMod(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SettingsSaveMod>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SettingsSaveMod>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SettingsSaveMod>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SettingsSaveMod>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SettingsSaveMod>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SettingsSaveMod>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SettingsSaveMod>, SettingsSaveMod>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0003856C File Offset: 0x0003676C
		public JsonTypeInfo<List<ModelId>> ListModelId
		{
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

		// Token: 0x0600067E RID: 1662 RVA: 0x000385A8 File Offset: 0x000367A8
		[NullableContext(1)]
		private JsonTypeInfo<List<ModelId>> Create_ListModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ModelId>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ModelId>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ModelId>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ModelId>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ModelId>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<ModelId>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ModelId>, ModelId>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00038610 File Offset: 0x00036810
		public JsonTypeInfo<List<NullLeaderboard>> ListNullLeaderboard
		{
			get
			{
				JsonTypeInfo<List<NullLeaderboard>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListNullLeaderboard) == null)
				{
					jsonTypeInfo = (this._ListNullLeaderboard = (JsonTypeInfo<List<NullLeaderboard>>)base.Options.GetTypeInfo(typeof(List<NullLeaderboard>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0003864C File Offset: 0x0003684C
		[NullableContext(1)]
		private JsonTypeInfo<List<NullLeaderboard>> Create_ListNullLeaderboard(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<NullLeaderboard>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<NullLeaderboard>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<NullLeaderboard>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<NullLeaderboard>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<NullLeaderboard>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<NullLeaderboard>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<NullLeaderboard>, NullLeaderboard>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x000386B4 File Offset: 0x000368B4
		public JsonTypeInfo<List<NullLeaderboardFileEntry>> ListNullLeaderboardFileEntry
		{
			get
			{
				JsonTypeInfo<List<NullLeaderboardFileEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListNullLeaderboardFileEntry) == null)
				{
					jsonTypeInfo = (this._ListNullLeaderboardFileEntry = (JsonTypeInfo<List<NullLeaderboardFileEntry>>)base.Options.GetTypeInfo(typeof(List<NullLeaderboardFileEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000386F0 File Offset: 0x000368F0
		[NullableContext(1)]
		private JsonTypeInfo<List<NullLeaderboardFileEntry>> Create_ListNullLeaderboardFileEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<NullLeaderboardFileEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<NullLeaderboardFileEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<NullLeaderboardFileEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<NullLeaderboardFileEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<NullLeaderboardFileEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<NullLeaderboardFileEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<NullLeaderboardFileEntry>, NullLeaderboardFileEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00038758 File Offset: 0x00036958
		public JsonTypeInfo<List<NullMultiplayerName>> ListNullMultiplayerName
		{
			get
			{
				JsonTypeInfo<List<NullMultiplayerName>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListNullMultiplayerName) == null)
				{
					jsonTypeInfo = (this._ListNullMultiplayerName = (JsonTypeInfo<List<NullMultiplayerName>>)base.Options.GetTypeInfo(typeof(List<NullMultiplayerName>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00038794 File Offset: 0x00036994
		[NullableContext(1)]
		private JsonTypeInfo<List<NullMultiplayerName>> Create_ListNullMultiplayerName(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<NullMultiplayerName>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<NullMultiplayerName>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<NullMultiplayerName>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<NullMultiplayerName>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<NullMultiplayerName>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<NullMultiplayerName>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<NullMultiplayerName>, NullMultiplayerName>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x000387FC File Offset: 0x000369FC
		public JsonTypeInfo<List<AncientChoiceHistoryEntry>> ListAncientChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<AncientChoiceHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListAncientChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListAncientChoiceHistoryEntry = (JsonTypeInfo<List<AncientChoiceHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<AncientChoiceHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00038838 File Offset: 0x00036A38
		[NullableContext(1)]
		private JsonTypeInfo<List<AncientChoiceHistoryEntry>> Create_ListAncientChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<AncientChoiceHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<AncientChoiceHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<AncientChoiceHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<AncientChoiceHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<AncientChoiceHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<AncientChoiceHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<AncientChoiceHistoryEntry>, AncientChoiceHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000388A0 File Offset: 0x00036AA0
		public JsonTypeInfo<List<CardChoiceHistoryEntry>> ListCardChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<CardChoiceHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCardChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListCardChoiceHistoryEntry = (JsonTypeInfo<List<CardChoiceHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<CardChoiceHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000388DC File Offset: 0x00036ADC
		[NullableContext(1)]
		private JsonTypeInfo<List<CardChoiceHistoryEntry>> Create_ListCardChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CardChoiceHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CardChoiceHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CardChoiceHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CardChoiceHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CardChoiceHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<CardChoiceHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CardChoiceHistoryEntry>, CardChoiceHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00038944 File Offset: 0x00036B44
		public JsonTypeInfo<List<CardEnchantmentHistoryEntry>> ListCardEnchantmentHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<CardEnchantmentHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCardEnchantmentHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListCardEnchantmentHistoryEntry = (JsonTypeInfo<List<CardEnchantmentHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<CardEnchantmentHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00038980 File Offset: 0x00036B80
		[NullableContext(1)]
		private JsonTypeInfo<List<CardEnchantmentHistoryEntry>> Create_ListCardEnchantmentHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CardEnchantmentHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CardEnchantmentHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CardEnchantmentHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CardEnchantmentHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CardEnchantmentHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<CardEnchantmentHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CardEnchantmentHistoryEntry>, CardEnchantmentHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x000389E8 File Offset: 0x00036BE8
		public JsonTypeInfo<List<CardTransformationHistoryEntry>> ListCardTransformationHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<CardTransformationHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCardTransformationHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListCardTransformationHistoryEntry = (JsonTypeInfo<List<CardTransformationHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<CardTransformationHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00038A24 File Offset: 0x00036C24
		[NullableContext(1)]
		private JsonTypeInfo<List<CardTransformationHistoryEntry>> Create_ListCardTransformationHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CardTransformationHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CardTransformationHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CardTransformationHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CardTransformationHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CardTransformationHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<CardTransformationHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CardTransformationHistoryEntry>, CardTransformationHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00038A8C File Offset: 0x00036C8C
		public JsonTypeInfo<List<EventOptionHistoryEntry>> ListEventOptionHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<EventOptionHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListEventOptionHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListEventOptionHistoryEntry = (JsonTypeInfo<List<EventOptionHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<EventOptionHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00038AC8 File Offset: 0x00036CC8
		[NullableContext(1)]
		private JsonTypeInfo<List<EventOptionHistoryEntry>> Create_ListEventOptionHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<EventOptionHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<EventOptionHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<EventOptionHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<EventOptionHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<EventOptionHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<EventOptionHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<EventOptionHistoryEntry>, EventOptionHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00038B30 File Offset: 0x00036D30
		public JsonTypeInfo<List<MapPointHistoryEntry>> ListMapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<MapPointHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListMapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListMapPointHistoryEntry = (JsonTypeInfo<List<MapPointHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<MapPointHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00038B6C File Offset: 0x00036D6C
		[NullableContext(1)]
		private JsonTypeInfo<List<MapPointHistoryEntry>> Create_ListMapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<MapPointHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MapPointHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<MapPointHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<MapPointHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<MapPointHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<MapPointHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MapPointHistoryEntry>, MapPointHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00038BD4 File Offset: 0x00036DD4
		public JsonTypeInfo<List<MapPointRoomHistoryEntry>> ListMapPointRoomHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<MapPointRoomHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListMapPointRoomHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListMapPointRoomHistoryEntry = (JsonTypeInfo<List<MapPointRoomHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<MapPointRoomHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00038C10 File Offset: 0x00036E10
		[NullableContext(1)]
		private JsonTypeInfo<List<MapPointRoomHistoryEntry>> Create_ListMapPointRoomHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<MapPointRoomHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MapPointRoomHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<MapPointRoomHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<MapPointRoomHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<MapPointRoomHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<MapPointRoomHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MapPointRoomHistoryEntry>, MapPointRoomHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00038C78 File Offset: 0x00036E78
		public JsonTypeInfo<List<ModelChoiceHistoryEntry>> ListModelChoiceHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<ModelChoiceHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListModelChoiceHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListModelChoiceHistoryEntry = (JsonTypeInfo<List<ModelChoiceHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<ModelChoiceHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00038CB4 File Offset: 0x00036EB4
		[NullableContext(1)]
		private JsonTypeInfo<List<ModelChoiceHistoryEntry>> Create_ListModelChoiceHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ModelChoiceHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ModelChoiceHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ModelChoiceHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ModelChoiceHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ModelChoiceHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<ModelChoiceHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ModelChoiceHistoryEntry>, ModelChoiceHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00038D1C File Offset: 0x00036F1C
		public JsonTypeInfo<List<PlayerMapPointHistoryEntry>> ListPlayerMapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<PlayerMapPointHistoryEntry>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListPlayerMapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListPlayerMapPointHistoryEntry = (JsonTypeInfo<List<PlayerMapPointHistoryEntry>>)base.Options.GetTypeInfo(typeof(List<PlayerMapPointHistoryEntry>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00038D58 File Offset: 0x00036F58
		[NullableContext(1)]
		private JsonTypeInfo<List<PlayerMapPointHistoryEntry>> Create_ListPlayerMapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<PlayerMapPointHistoryEntry>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<PlayerMapPointHistoryEntry>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<PlayerMapPointHistoryEntry>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<PlayerMapPointHistoryEntry>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<PlayerMapPointHistoryEntry>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<PlayerMapPointHistoryEntry>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<PlayerMapPointHistoryEntry>, PlayerMapPointHistoryEntry>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00038DC0 File Offset: 0x00036FC0
		public JsonTypeInfo<List<RunHistoryPlayer>> ListRunHistoryPlayer
		{
			get
			{
				JsonTypeInfo<List<RunHistoryPlayer>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListRunHistoryPlayer) == null)
				{
					jsonTypeInfo = (this._ListRunHistoryPlayer = (JsonTypeInfo<List<RunHistoryPlayer>>)base.Options.GetTypeInfo(typeof(List<RunHistoryPlayer>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00038DFC File Offset: 0x00036FFC
		[NullableContext(1)]
		private JsonTypeInfo<List<RunHistoryPlayer>> Create_ListRunHistoryPlayer(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<RunHistoryPlayer>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<RunHistoryPlayer>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<RunHistoryPlayer>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<RunHistoryPlayer>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<RunHistoryPlayer>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<RunHistoryPlayer>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<RunHistoryPlayer>, RunHistoryPlayer>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00038E64 File Offset: 0x00037064
		public JsonTypeInfo<List<AncientCharacterStats>> ListAncientCharacterStats
		{
			get
			{
				JsonTypeInfo<List<AncientCharacterStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListAncientCharacterStats) == null)
				{
					jsonTypeInfo = (this._ListAncientCharacterStats = (JsonTypeInfo<List<AncientCharacterStats>>)base.Options.GetTypeInfo(typeof(List<AncientCharacterStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00038EA0 File Offset: 0x000370A0
		[NullableContext(1)]
		private JsonTypeInfo<List<AncientCharacterStats>> Create_ListAncientCharacterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<AncientCharacterStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<AncientCharacterStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<AncientCharacterStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<AncientCharacterStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<AncientCharacterStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<AncientCharacterStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<AncientCharacterStats>, AncientCharacterStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00038F08 File Offset: 0x00037108
		public JsonTypeInfo<List<AncientStats>> ListAncientStats
		{
			get
			{
				JsonTypeInfo<List<AncientStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListAncientStats) == null)
				{
					jsonTypeInfo = (this._ListAncientStats = (JsonTypeInfo<List<AncientStats>>)base.Options.GetTypeInfo(typeof(List<AncientStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00038F44 File Offset: 0x00037144
		[NullableContext(1)]
		private JsonTypeInfo<List<AncientStats>> Create_ListAncientStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<AncientStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<AncientStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<AncientStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<AncientStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<AncientStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<AncientStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<AncientStats>, AncientStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00038FAC File Offset: 0x000371AC
		public JsonTypeInfo<List<BadgeStats>> ListBadgeStats
		{
			get
			{
				JsonTypeInfo<List<BadgeStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListBadgeStats) == null)
				{
					jsonTypeInfo = (this._ListBadgeStats = (JsonTypeInfo<List<BadgeStats>>)base.Options.GetTypeInfo(typeof(List<BadgeStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00038FE8 File Offset: 0x000371E8
		[NullableContext(1)]
		private JsonTypeInfo<List<BadgeStats>> Create_ListBadgeStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<BadgeStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<BadgeStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<BadgeStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<BadgeStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<BadgeStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<BadgeStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<BadgeStats>, BadgeStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00039050 File Offset: 0x00037250
		public JsonTypeInfo<List<CardStats>> ListCardStats
		{
			get
			{
				JsonTypeInfo<List<CardStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCardStats) == null)
				{
					jsonTypeInfo = (this._ListCardStats = (JsonTypeInfo<List<CardStats>>)base.Options.GetTypeInfo(typeof(List<CardStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0003908C File Offset: 0x0003728C
		[NullableContext(1)]
		private JsonTypeInfo<List<CardStats>> Create_ListCardStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CardStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CardStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CardStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CardStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CardStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<CardStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CardStats>, CardStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000390F4 File Offset: 0x000372F4
		public JsonTypeInfo<List<CharacterStats>> ListCharacterStats
		{
			get
			{
				JsonTypeInfo<List<CharacterStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListCharacterStats) == null)
				{
					jsonTypeInfo = (this._ListCharacterStats = (JsonTypeInfo<List<CharacterStats>>)base.Options.GetTypeInfo(typeof(List<CharacterStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00039130 File Offset: 0x00037330
		[NullableContext(1)]
		private JsonTypeInfo<List<CharacterStats>> Create_ListCharacterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<CharacterStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<CharacterStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<CharacterStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<CharacterStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<CharacterStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<CharacterStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<CharacterStats>, CharacterStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00039198 File Offset: 0x00037398
		public JsonTypeInfo<List<EncounterStats>> ListEncounterStats
		{
			get
			{
				JsonTypeInfo<List<EncounterStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListEncounterStats) == null)
				{
					jsonTypeInfo = (this._ListEncounterStats = (JsonTypeInfo<List<EncounterStats>>)base.Options.GetTypeInfo(typeof(List<EncounterStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000391D4 File Offset: 0x000373D4
		[NullableContext(1)]
		private JsonTypeInfo<List<EncounterStats>> Create_ListEncounterStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<EncounterStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<EncounterStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<EncounterStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<EncounterStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<EncounterStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<EncounterStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<EncounterStats>, EncounterStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0003923C File Offset: 0x0003743C
		public JsonTypeInfo<List<EnemyStats>> ListEnemyStats
		{
			get
			{
				JsonTypeInfo<List<EnemyStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListEnemyStats) == null)
				{
					jsonTypeInfo = (this._ListEnemyStats = (JsonTypeInfo<List<EnemyStats>>)base.Options.GetTypeInfo(typeof(List<EnemyStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00039278 File Offset: 0x00037478
		[NullableContext(1)]
		private JsonTypeInfo<List<EnemyStats>> Create_ListEnemyStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<EnemyStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<EnemyStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<EnemyStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<EnemyStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<EnemyStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<EnemyStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<EnemyStats>, EnemyStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000392E0 File Offset: 0x000374E0
		public JsonTypeInfo<List<FightStats>> ListFightStats
		{
			get
			{
				JsonTypeInfo<List<FightStats>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListFightStats) == null)
				{
					jsonTypeInfo = (this._ListFightStats = (JsonTypeInfo<List<FightStats>>)base.Options.GetTypeInfo(typeof(List<FightStats>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0003931C File Offset: 0x0003751C
		[NullableContext(1)]
		private JsonTypeInfo<List<FightStats>> Create_ListFightStats(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<FightStats>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<FightStats>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<FightStats>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<FightStats>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<FightStats>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<FightStats>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<FightStats>, FightStats>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00039384 File Offset: 0x00037584
		public JsonTypeInfo<List<SerializableMapDrawingLine>> ListSerializableMapDrawingLine
		{
			get
			{
				JsonTypeInfo<List<SerializableMapDrawingLine>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableMapDrawingLine) == null)
				{
					jsonTypeInfo = (this._ListSerializableMapDrawingLine = (JsonTypeInfo<List<SerializableMapDrawingLine>>)base.Options.GetTypeInfo(typeof(List<SerializableMapDrawingLine>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000393C0 File Offset: 0x000375C0
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableMapDrawingLine>> Create_ListSerializableMapDrawingLine(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableMapDrawingLine>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableMapDrawingLine>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableMapDrawingLine>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableMapDrawingLine>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableMapDrawingLine>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableMapDrawingLine>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableMapDrawingLine>, SerializableMapDrawingLine>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00039428 File Offset: 0x00037628
		public JsonTypeInfo<List<SerializablePlayerMapDrawings>> ListSerializablePlayerMapDrawings
		{
			get
			{
				JsonTypeInfo<List<SerializablePlayerMapDrawings>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializablePlayerMapDrawings) == null)
				{
					jsonTypeInfo = (this._ListSerializablePlayerMapDrawings = (JsonTypeInfo<List<SerializablePlayerMapDrawings>>)base.Options.GetTypeInfo(typeof(List<SerializablePlayerMapDrawings>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00039464 File Offset: 0x00037664
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializablePlayerMapDrawings>> Create_ListSerializablePlayerMapDrawings(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializablePlayerMapDrawings>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializablePlayerMapDrawings>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializablePlayerMapDrawings>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializablePlayerMapDrawings>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializablePlayerMapDrawings>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializablePlayerMapDrawings>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializablePlayerMapDrawings>, SerializablePlayerMapDrawings>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x000394CC File Offset: 0x000376CC
		public JsonTypeInfo<List<MigratingData>> ListMigratingData
		{
			get
			{
				JsonTypeInfo<List<MigratingData>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListMigratingData) == null)
				{
					jsonTypeInfo = (this._ListMigratingData = (JsonTypeInfo<List<MigratingData>>)base.Options.GetTypeInfo(typeof(List<MigratingData>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00039508 File Offset: 0x00037708
		[NullableContext(1)]
		private JsonTypeInfo<List<MigratingData>> Create_ListMigratingData(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<MigratingData>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MigratingData>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<MigratingData>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<MigratingData>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<MigratingData>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<MigratingData>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MigratingData>, MigratingData>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00039570 File Offset: 0x00037770
		public JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>> ListSavedPropertyBoolean
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertyBoolean) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertyBoolean = (JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<bool>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000395AC File Offset: 0x000377AC
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>> Create_ListSavedPropertyBoolean(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<bool>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<bool>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<bool>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<bool>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<bool>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<bool>>, SavedProperties.SavedProperty<bool>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00039614 File Offset: 0x00037814
		public JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>> ListSavedPropertyModelId
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertyModelId) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertyModelId = (JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<ModelId>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00039650 File Offset: 0x00037850
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>> Create_ListSavedPropertyModelId(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<ModelId>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<ModelId>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<ModelId>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<ModelId>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<ModelId>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<ModelId>>, SavedProperties.SavedProperty<ModelId>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x000396B8 File Offset: 0x000378B8
		public JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>> ListSavedPropertySerializableCardArray
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertySerializableCardArray) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertySerializableCardArray = (JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<SerializableCard[]>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000396F4 File Offset: 0x000378F4
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0, 1, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>> Create_ListSavedPropertySerializableCardArray(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<SerializableCard[]>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<SerializableCard[]>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard[]>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>, SavedProperties.SavedProperty<SerializableCard[]>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0003975C File Offset: 0x0003795C
		public JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>> ListSavedPropertySerializableCard
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertySerializableCard) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertySerializableCard = (JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<SerializableCard>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00039798 File Offset: 0x00037998
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>> Create_ListSavedPropertySerializableCard(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<SerializableCard>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<SerializableCard>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<SerializableCard>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<SerializableCard>>, SavedProperties.SavedProperty<SerializableCard>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00039800 File Offset: 0x00037A00
		public JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>> ListSavedPropertyInt32Array
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertyInt32Array) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertyInt32Array = (JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<int[]>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0003983C File Offset: 0x00037A3C
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>> Create_ListSavedPropertyInt32Array(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<int[]>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int[]>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int[]>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<int[]>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int[]>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<int[]>>, SavedProperties.SavedProperty<int[]>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000398A4 File Offset: 0x00037AA4
		public JsonTypeInfo<List<SavedProperties.SavedProperty<int>>> ListSavedPropertyInt32
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<int>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertyInt32) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertyInt32 = (JsonTypeInfo<List<SavedProperties.SavedProperty<int>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<int>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000398E0 File Offset: 0x00037AE0
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<int>>> Create_ListSavedPropertyInt32(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<int>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<int>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<int>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<int>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<int>>, SavedProperties.SavedProperty<int>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00039948 File Offset: 0x00037B48
		public JsonTypeInfo<List<SavedProperties.SavedProperty<string>>> ListSavedPropertyString
		{
			get
			{
				JsonTypeInfo<List<SavedProperties.SavedProperty<string>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSavedPropertyString) == null)
				{
					jsonTypeInfo = (this._ListSavedPropertyString = (JsonTypeInfo<List<SavedProperties.SavedProperty<string>>>)base.Options.GetTypeInfo(typeof(List<SavedProperties.SavedProperty<string>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00039984 File Offset: 0x00037B84
		[NullableContext(1)]
		[return: Nullable(new byte[] { 1, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<string>>> Create_ListSavedPropertyString(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SavedProperties.SavedProperty<string>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SavedProperties.SavedProperty<string>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<string>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SavedProperties.SavedProperty<string>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SavedProperties.SavedProperty<string>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SavedProperties.SavedProperty<string>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SavedProperties.SavedProperty<string>>, SavedProperties.SavedProperty<string>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x000399EC File Offset: 0x00037BEC
		public JsonTypeInfo<List<SerializableActModel>> ListSerializableActModel
		{
			get
			{
				JsonTypeInfo<List<SerializableActModel>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableActModel) == null)
				{
					jsonTypeInfo = (this._ListSerializableActModel = (JsonTypeInfo<List<SerializableActModel>>)base.Options.GetTypeInfo(typeof(List<SerializableActModel>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00039A28 File Offset: 0x00037C28
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableActModel>> Create_ListSerializableActModel(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableActModel>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableActModel>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableActModel>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableActModel>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableActModel>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableActModel>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableActModel>, SerializableActModel>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00039A90 File Offset: 0x00037C90
		public JsonTypeInfo<List<SerializableCard>> ListSerializableCard
		{
			get
			{
				JsonTypeInfo<List<SerializableCard>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableCard) == null)
				{
					jsonTypeInfo = (this._ListSerializableCard = (JsonTypeInfo<List<SerializableCard>>)base.Options.GetTypeInfo(typeof(List<SerializableCard>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00039ACC File Offset: 0x00037CCC
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableCard>> Create_ListSerializableCard(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableCard>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableCard>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableCard>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableCard>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableCard>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableCard>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableCard>, SerializableCard>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00039B34 File Offset: 0x00037D34
		public JsonTypeInfo<List<SerializableMapPoint>> ListSerializableMapPoint
		{
			get
			{
				JsonTypeInfo<List<SerializableMapPoint>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableMapPoint) == null)
				{
					jsonTypeInfo = (this._ListSerializableMapPoint = (JsonTypeInfo<List<SerializableMapPoint>>)base.Options.GetTypeInfo(typeof(List<SerializableMapPoint>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00039B70 File Offset: 0x00037D70
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableMapPoint>> Create_ListSerializableMapPoint(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableMapPoint>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableMapPoint>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableMapPoint>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableMapPoint>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableMapPoint>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableMapPoint>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableMapPoint>, SerializableMapPoint>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00039BD8 File Offset: 0x00037DD8
		public JsonTypeInfo<List<SerializableModifier>> ListSerializableModifier
		{
			get
			{
				JsonTypeInfo<List<SerializableModifier>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableModifier) == null)
				{
					jsonTypeInfo = (this._ListSerializableModifier = (JsonTypeInfo<List<SerializableModifier>>)base.Options.GetTypeInfo(typeof(List<SerializableModifier>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00039C14 File Offset: 0x00037E14
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableModifier>> Create_ListSerializableModifier(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableModifier>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableModifier>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableModifier>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableModifier>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableModifier>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableModifier>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableModifier>, SerializableModifier>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00039C7C File Offset: 0x00037E7C
		public JsonTypeInfo<List<SerializablePlayer>> ListSerializablePlayer
		{
			get
			{
				JsonTypeInfo<List<SerializablePlayer>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializablePlayer) == null)
				{
					jsonTypeInfo = (this._ListSerializablePlayer = (JsonTypeInfo<List<SerializablePlayer>>)base.Options.GetTypeInfo(typeof(List<SerializablePlayer>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00039CB8 File Offset: 0x00037EB8
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializablePlayer>> Create_ListSerializablePlayer(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializablePlayer>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializablePlayer>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializablePlayer>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializablePlayer>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializablePlayer>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializablePlayer>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializablePlayer>, SerializablePlayer>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00039D20 File Offset: 0x00037F20
		public JsonTypeInfo<List<SerializablePotion>> ListSerializablePotion
		{
			get
			{
				JsonTypeInfo<List<SerializablePotion>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializablePotion) == null)
				{
					jsonTypeInfo = (this._ListSerializablePotion = (JsonTypeInfo<List<SerializablePotion>>)base.Options.GetTypeInfo(typeof(List<SerializablePotion>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00039D5C File Offset: 0x00037F5C
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializablePotion>> Create_ListSerializablePotion(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializablePotion>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializablePotion>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializablePotion>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializablePotion>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializablePotion>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializablePotion>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializablePotion>, SerializablePotion>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00039DC4 File Offset: 0x00037FC4
		public JsonTypeInfo<List<SerializableRelic>> ListSerializableRelic
		{
			get
			{
				JsonTypeInfo<List<SerializableRelic>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableRelic) == null)
				{
					jsonTypeInfo = (this._ListSerializableRelic = (JsonTypeInfo<List<SerializableRelic>>)base.Options.GetTypeInfo(typeof(List<SerializableRelic>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00039E00 File Offset: 0x00038000
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableRelic>> Create_ListSerializableRelic(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableRelic>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableRelic>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableRelic>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableRelic>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableRelic>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableRelic>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableRelic>, SerializableRelic>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00039E68 File Offset: 0x00038068
		public JsonTypeInfo<List<SerializableReward>> ListSerializableReward
		{
			get
			{
				JsonTypeInfo<List<SerializableReward>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableReward) == null)
				{
					jsonTypeInfo = (this._ListSerializableReward = (JsonTypeInfo<List<SerializableReward>>)base.Options.GetTypeInfo(typeof(List<SerializableReward>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00039EA4 File Offset: 0x000380A4
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableReward>> Create_ListSerializableReward(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableReward>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableReward>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableReward>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableReward>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableReward>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableReward>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableReward>, SerializableReward>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00039F0C File Offset: 0x0003810C
		public JsonTypeInfo<List<SerializableEpoch>> ListSerializableEpoch
		{
			get
			{
				JsonTypeInfo<List<SerializableEpoch>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableEpoch) == null)
				{
					jsonTypeInfo = (this._ListSerializableEpoch = (JsonTypeInfo<List<SerializableEpoch>>)base.Options.GetTypeInfo(typeof(List<SerializableEpoch>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00039F48 File Offset: 0x00038148
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableEpoch>> Create_ListSerializableEpoch(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableEpoch>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableEpoch>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableEpoch>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableEpoch>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableEpoch>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableEpoch>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableEpoch>, SerializableEpoch>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00039FB0 File Offset: 0x000381B0
		public JsonTypeInfo<List<SerializableUnlockedAchievement>> ListSerializableUnlockedAchievement
		{
			get
			{
				JsonTypeInfo<List<SerializableUnlockedAchievement>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListSerializableUnlockedAchievement) == null)
				{
					jsonTypeInfo = (this._ListSerializableUnlockedAchievement = (JsonTypeInfo<List<SerializableUnlockedAchievement>>)base.Options.GetTypeInfo(typeof(List<SerializableUnlockedAchievement>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00039FEC File Offset: 0x000381EC
		[NullableContext(1)]
		private JsonTypeInfo<List<SerializableUnlockedAchievement>> Create_ListSerializableUnlockedAchievement(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<SerializableUnlockedAchievement>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<SerializableUnlockedAchievement>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<SerializableUnlockedAchievement>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<SerializableUnlockedAchievement>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<SerializableUnlockedAchievement>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<SerializableUnlockedAchievement>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<SerializableUnlockedAchievement>, SerializableUnlockedAchievement>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0003A054 File Offset: 0x00038254
		public JsonTypeInfo<List<Dictionary<string, object>>> ListDictionaryStringObject
		{
			get
			{
				JsonTypeInfo<List<Dictionary<string, object>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListDictionaryStringObject) == null)
				{
					jsonTypeInfo = (this._ListDictionaryStringObject = (JsonTypeInfo<List<Dictionary<string, object>>>)base.Options.GetTypeInfo(typeof(List<Dictionary<string, object>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0003A090 File Offset: 0x00038290
		[NullableContext(1)]
		private JsonTypeInfo<List<Dictionary<string, object>>> Create_ListDictionaryStringObject(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<Dictionary<string, object>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<Dictionary<string, object>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<Dictionary<string, object>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<Dictionary<string, object>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<Dictionary<string, object>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<Dictionary<string, object>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<Dictionary<string, object>>, Dictionary<string, object>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0003A0F8 File Offset: 0x000382F8
		public JsonTypeInfo<List<List<MapPointHistoryEntry>>> ListListMapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<List<MapPointHistoryEntry>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListListMapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListListMapPointHistoryEntry = (JsonTypeInfo<List<List<MapPointHistoryEntry>>>)base.Options.GetTypeInfo(typeof(List<List<MapPointHistoryEntry>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0003A134 File Offset: 0x00038334
		[NullableContext(1)]
		private JsonTypeInfo<List<List<MapPointHistoryEntry>>> Create_ListListMapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<List<MapPointHistoryEntry>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<List<MapPointHistoryEntry>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<List<MapPointHistoryEntry>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<List<MapPointHistoryEntry>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<List<MapPointHistoryEntry>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<List<MapPointHistoryEntry>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<List<MapPointHistoryEntry>>, List<MapPointHistoryEntry>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0003A19C File Offset: 0x0003839C
		public JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>> ListListPlayerMapPointHistoryEntry
		{
			get
			{
				JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListListPlayerMapPointHistoryEntry) == null)
				{
					jsonTypeInfo = (this._ListListPlayerMapPointHistoryEntry = (JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>>)base.Options.GetTypeInfo(typeof(List<List<PlayerMapPointHistoryEntry>>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0003A1D8 File Offset: 0x000383D8
		[NullableContext(1)]
		private JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>> Create_ListListPlayerMapPointHistoryEntry(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<List<PlayerMapPointHistoryEntry>>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<List<PlayerMapPointHistoryEntry>>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<List<PlayerMapPointHistoryEntry>>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<List<PlayerMapPointHistoryEntry>>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<List<PlayerMapPointHistoryEntry>>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<List<PlayerMapPointHistoryEntry>>, List<PlayerMapPointHistoryEntry>>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0003A240 File Offset: 0x00038440
		public JsonTypeInfo<List<JsonNode>> ListJsonNode
		{
			get
			{
				JsonTypeInfo<List<JsonNode>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListJsonNode) == null)
				{
					jsonTypeInfo = (this._ListJsonNode = (JsonTypeInfo<List<JsonNode>>)base.Options.GetTypeInfo(typeof(List<JsonNode>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0003A27C File Offset: 0x0003847C
		[NullableContext(1)]
		private JsonTypeInfo<List<JsonNode>> Create_ListJsonNode(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<JsonNode>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<JsonNode>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<JsonNode>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<JsonNode>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<JsonNode>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<JsonNode>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<JsonNode>, JsonNode>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0003A2E4 File Offset: 0x000384E4
		public JsonTypeInfo<List<string>> ListString
		{
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

		// Token: 0x060006DA RID: 1754 RVA: 0x0003A320 File Offset: 0x00038520
		[NullableContext(1)]
		private JsonTypeInfo<List<string>> Create_ListString(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<string>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<string>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<string>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<string>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<string>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<string>, string>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0003A388 File Offset: 0x00038588
		public JsonTypeInfo<List<ulong>> ListUInt64
		{
			get
			{
				JsonTypeInfo<List<ulong>> jsonTypeInfo;
				if ((jsonTypeInfo = this._ListUInt64) == null)
				{
					jsonTypeInfo = (this._ListUInt64 = (JsonTypeInfo<List<ulong>>)base.Options.GetTypeInfo(typeof(List<ulong>)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0003A3C4 File Offset: 0x000385C4
		[NullableContext(1)]
		private JsonTypeInfo<List<ulong>> Create_ListUInt64(JsonSerializerOptions options)
		{
			JsonTypeInfo<List<ulong>> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ulong>>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<List<ulong>> jsonCollectionInfoValues = new JsonCollectionInfoValues<List<ulong>>();
				jsonCollectionInfoValues.ObjectCreator = () => new List<ulong>();
				jsonCollectionInfoValues.SerializeHandler = null;
				JsonCollectionInfoValues<List<ulong>> jsonCollectionInfoValues2 = jsonCollectionInfoValues;
				jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ulong>, ulong>(options, jsonCollectionInfoValues2);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0003A42C File Offset: 0x0003862C
		public JsonTypeInfo<DateTimeOffset> DateTimeOffset
		{
			get
			{
				JsonTypeInfo<DateTimeOffset> jsonTypeInfo;
				if ((jsonTypeInfo = this._DateTimeOffset) == null)
				{
					jsonTypeInfo = (this._DateTimeOffset = (JsonTypeInfo<DateTimeOffset>)base.Options.GetTypeInfo(typeof(DateTimeOffset)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0003A468 File Offset: 0x00038668
		[NullableContext(1)]
		private JsonTypeInfo<DateTimeOffset> Create_DateTimeOffset(JsonSerializerOptions options)
		{
			JsonTypeInfo<DateTimeOffset> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<DateTimeOffset>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<DateTimeOffset>(options, JsonMetadataServices.DateTimeOffsetConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0003A494 File Offset: 0x00038694
		public JsonTypeInfo<DateTimeOffset?> NullableDateTimeOffset
		{
			get
			{
				JsonTypeInfo<DateTimeOffset?> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullableDateTimeOffset) == null)
				{
					jsonTypeInfo = (this._NullableDateTimeOffset = (JsonTypeInfo<DateTimeOffset?>)base.Options.GetTypeInfo(typeof(DateTimeOffset?)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0003A4D0 File Offset: 0x000386D0
		[NullableContext(1)]
		private JsonTypeInfo<DateTimeOffset?> Create_NullableDateTimeOffset(JsonSerializerOptions options)
		{
			JsonTypeInfo<DateTimeOffset?> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<DateTimeOffset?>(options, out jsonTypeInfo))
			{
				JsonConverter nullableConverter = JsonMetadataServices.GetNullableConverter<DateTimeOffset>(options);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<DateTimeOffset?>(options, nullableConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0003A500 File Offset: 0x00038700
		public JsonTypeInfo<JsonNode> JsonNode
		{
			get
			{
				JsonTypeInfo<JsonNode> jsonTypeInfo;
				if ((jsonTypeInfo = this._JsonNode) == null)
				{
					jsonTypeInfo = (this._JsonNode = (JsonTypeInfo<JsonNode>)base.Options.GetTypeInfo(typeof(JsonNode)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0003A53C File Offset: 0x0003873C
		[NullableContext(1)]
		private JsonTypeInfo<JsonNode> Create_JsonNode(JsonSerializerOptions options)
		{
			JsonTypeInfo<JsonNode> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<JsonNode>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<JsonNode>(options, JsonMetadataServices.JsonNodeConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0003A568 File Offset: 0x00038768
		public JsonTypeInfo<JsonObject> JsonObject
		{
			get
			{
				JsonTypeInfo<JsonObject> jsonTypeInfo;
				if ((jsonTypeInfo = this._JsonObject) == null)
				{
					jsonTypeInfo = (this._JsonObject = (JsonTypeInfo<JsonObject>)base.Options.GetTypeInfo(typeof(JsonObject)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0003A5A4 File Offset: 0x000387A4
		[NullableContext(1)]
		private JsonTypeInfo<JsonObject> Create_JsonObject(JsonSerializerOptions options)
		{
			JsonTypeInfo<JsonObject> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<JsonObject>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<JsonObject>(options, JsonMetadataServices.JsonObjectConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0003A5D0 File Offset: 0x000387D0
		public JsonTypeInfo<int> Int32
		{
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

		// Token: 0x060006E6 RID: 1766 RVA: 0x0003A60C File Offset: 0x0003880C
		[NullableContext(1)]
		private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0003A638 File Offset: 0x00038838
		public JsonTypeInfo<int?> NullableInt32
		{
			get
			{
				JsonTypeInfo<int?> jsonTypeInfo;
				if ((jsonTypeInfo = this._NullableInt32) == null)
				{
					jsonTypeInfo = (this._NullableInt32 = (JsonTypeInfo<int?>)base.Options.GetTypeInfo(typeof(int?)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0003A674 File Offset: 0x00038874
		[NullableContext(1)]
		private JsonTypeInfo<int?> Create_NullableInt32(JsonSerializerOptions options)
		{
			JsonTypeInfo<int?> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int?>(options, out jsonTypeInfo))
			{
				JsonConverter nullableConverter = JsonMetadataServices.GetNullableConverter<int>(options);
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int?>(options, nullableConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0003A6A4 File Offset: 0x000388A4
		public JsonTypeInfo<int[]> Int32Array
		{
			get
			{
				JsonTypeInfo<int[]> jsonTypeInfo;
				if ((jsonTypeInfo = this._Int32Array) == null)
				{
					jsonTypeInfo = (this._Int32Array = (JsonTypeInfo<int[]>)base.Options.GetTypeInfo(typeof(int[])));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0003A6E0 File Offset: 0x000388E0
		[NullableContext(1)]
		private JsonTypeInfo<int[]> Create_Int32Array(JsonSerializerOptions options)
		{
			JsonTypeInfo<int[]> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int[]>(options, out jsonTypeInfo))
			{
				JsonCollectionInfoValues<int[]> jsonCollectionInfoValues = new JsonCollectionInfoValues<int[]>
				{
					ObjectCreator = null,
					SerializeHandler = null
				};
				jsonTypeInfo = JsonMetadataServices.CreateArrayInfo<int>(options, jsonCollectionInfoValues);
				jsonTypeInfo.NumberHandling = null;
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0003A72C File Offset: 0x0003892C
		public JsonTypeInfo<long> Int64
		{
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

		// Token: 0x060006EC RID: 1772 RVA: 0x0003A768 File Offset: 0x00038968
		[NullableContext(1)]
		private JsonTypeInfo<long> Create_Int64(JsonSerializerOptions options)
		{
			JsonTypeInfo<long> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<long>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<long>(options, JsonMetadataServices.Int64Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0003A794 File Offset: 0x00038994
		public JsonTypeInfo<object> Object
		{
			get
			{
				JsonTypeInfo<object> jsonTypeInfo;
				if ((jsonTypeInfo = this._Object) == null)
				{
					jsonTypeInfo = (this._Object = (JsonTypeInfo<object>)base.Options.GetTypeInfo(typeof(object)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0003A7D0 File Offset: 0x000389D0
		[NullableContext(1)]
		private JsonTypeInfo<object> Create_Object(JsonSerializerOptions options)
		{
			JsonTypeInfo<object> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<object>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<object>(options, JsonMetadataServices.ObjectConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0003A7FC File Offset: 0x000389FC
		public JsonTypeInfo<string> String
		{
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

		// Token: 0x060006F0 RID: 1776 RVA: 0x0003A838 File Offset: 0x00038A38
		[NullableContext(1)]
		private JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
		{
			JsonTypeInfo<string> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, JsonMetadataServices.StringConverter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// Defines the source generated JSON serialization contract metadata for a given type.
		/// </summary>
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0003A864 File Offset: 0x00038A64
		public JsonTypeInfo<ulong> UInt64
		{
			get
			{
				JsonTypeInfo<ulong> jsonTypeInfo;
				if ((jsonTypeInfo = this._UInt64) == null)
				{
					jsonTypeInfo = (this._UInt64 = (JsonTypeInfo<ulong>)base.Options.GetTypeInfo(typeof(ulong)));
				}
				return jsonTypeInfo;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0003A8A0 File Offset: 0x00038AA0
		[NullableContext(1)]
		private JsonTypeInfo<ulong> Create_UInt64(JsonSerializerOptions options)
		{
			JsonTypeInfo<ulong> jsonTypeInfo;
			if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ulong>(options, out jsonTypeInfo))
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<ulong>(options, JsonMetadataServices.UInt64Converter);
			}
			jsonTypeInfo.OriginatingResolver = this;
			return jsonTypeInfo;
		}

		/// <summary>
		/// The default <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> associated with a default <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.
		/// </summary>
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0003A8CB File Offset: 0x00038ACB
		[Nullable(1)]
		public static MegaCritSerializerContext Default
		{
			[NullableContext(1)]
			get;
		} = new MegaCritSerializerContext(new JsonSerializerOptions(MegaCritSerializerContext.s_defaultOptions));

		/// <summary>
		/// The source-generated options associated with this context.
		/// </summary>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0003A8D2 File Offset: 0x00038AD2
		[Nullable(2)]
		protected override JsonSerializerOptions GeneratedSerializerOptions
		{
			[NullableContext(2)]
			get;
		} = MegaCritSerializerContext.s_defaultOptions;

		/// <inheritdoc />
		// Token: 0x060006F5 RID: 1781 RVA: 0x0003A8DA File Offset: 0x00038ADA
		public MegaCritSerializerContext()
			: base(null)
		{
		}

		/// <inheritdoc />
		// Token: 0x060006F6 RID: 1782 RVA: 0x0003A8EE File Offset: 0x00038AEE
		[NullableContext(1)]
		public MegaCritSerializerContext(JsonSerializerOptions options)
			: base(options)
		{
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0003A904 File Offset: 0x00038B04
		[NullableContext(1)]
		private static bool TryGetTypeInfoForRuntimeCustomConverter<[Nullable(2)] TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
		{
			JsonConverter runtimeConverterForType = MegaCritSerializerContext.GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
			if (runtimeConverterForType != null)
			{
				jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
				return true;
			}
			jsonTypeInfo = null;
			return false;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0003A934 File Offset: 0x00038B34
		[NullableContext(1)]
		[return: Nullable(2)]
		private static JsonConverter GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
		{
			for (int i = 0; i < options.Converters.Count; i++)
			{
				JsonConverter jsonConverter = options.Converters[i];
				if (jsonConverter != null && jsonConverter.CanConvert(type))
				{
					return MegaCritSerializerContext.ExpandConverter(type, jsonConverter, options, false);
				}
			}
			return null;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0003A97C File Offset: 0x00038B7C
		[NullableContext(1)]
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
		// Token: 0x060006FA RID: 1786 RVA: 0x0003A9E4 File Offset: 0x00038BE4
		[NullableContext(1)]
		[return: Nullable(2)]
		public override JsonTypeInfo GetTypeInfo(Type type)
		{
			JsonTypeInfo jsonTypeInfo;
			base.Options.TryGetTypeInfo(type, out jsonTypeInfo);
			return jsonTypeInfo;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0003AA04 File Offset: 0x00038C04
		[NullableContext(1)]
		[return: Nullable(2)]
		JsonTypeInfo IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (type == typeof(bool))
			{
				return this.Create_Boolean(options);
			}
			if (type == typeof(double))
			{
				return this.Create_Double(options);
			}
			if (type == typeof(float))
			{
				return this.Create_Single(options);
			}
			if (type == typeof(Vector2))
			{
				return this.Create_Vector2(options);
			}
			if (type == typeof(Vector2I))
			{
				return this.Create_Vector2I(options);
			}
			if (type == typeof(ControllerMappingType))
			{
				return this.Create_ControllerMappingType(options);
			}
			if (type == typeof(RelicRarity))
			{
				return this.Create_RelicRarity(options);
			}
			if (type == typeof(PlayerRngType))
			{
				return this.Create_PlayerRngType(options);
			}
			if (type == typeof(RunRngType))
			{
				return this.Create_RunRngType(options);
			}
			if (type == typeof(LocString))
			{
				return this.Create_LocString(options);
			}
			if (type == typeof(MapCoord))
			{
				return this.Create_MapCoord(options);
			}
			if (type == typeof(MapPointType))
			{
				return this.Create_MapPointType(options);
			}
			if (type == typeof(ModDependency))
			{
				return this.Create_ModDependency(options);
			}
			if (type == typeof(ModManifest))
			{
				return this.Create_ModManifest(options);
			}
			if (type == typeof(ModSettings))
			{
				return this.Create_ModSettings(options);
			}
			if (type == typeof(ModSource))
			{
				return this.Create_ModSource(options);
			}
			if (type == typeof(SettingsSaveMod))
			{
				return this.Create_SettingsSaveMod(options);
			}
			if (type == typeof(BadgeRarity))
			{
				return this.Create_BadgeRarity(options);
			}
			if (type == typeof(ModelId))
			{
				return this.Create_ModelId(options);
			}
			if (type == typeof(FeedbackData))
			{
				return this.Create_FeedbackData(options);
			}
			if (type == typeof(NullLeaderboard))
			{
				return this.Create_NullLeaderboard(options);
			}
			if (type == typeof(NullLeaderboardFile))
			{
				return this.Create_NullLeaderboardFile(options);
			}
			if (type == typeof(NullLeaderboardFileEntry))
			{
				return this.Create_NullLeaderboardFileEntry(options);
			}
			if (type == typeof(NullMultiplayerName))
			{
				return this.Create_NullMultiplayerName(options);
			}
			if (type == typeof(PlatformType))
			{
				return this.Create_PlatformType(options);
			}
			if (type == typeof(RewardType))
			{
				return this.Create_RewardType(options);
			}
			if (type == typeof(RoomType))
			{
				return this.Create_RoomType(options);
			}
			if (type == typeof(CardCreationSource))
			{
				return this.Create_CardCreationSource(options);
			}
			if (type == typeof(CardRarityOddsType))
			{
				return this.Create_CardRarityOddsType(options);
			}
			if (type == typeof(GameMode))
			{
				return this.Create_GameMode(options);
			}
			if (type == typeof(AncientChoiceHistoryEntry))
			{
				return this.Create_AncientChoiceHistoryEntry(options);
			}
			if (type == typeof(CardChoiceHistoryEntry))
			{
				return this.Create_CardChoiceHistoryEntry(options);
			}
			if (type == typeof(CardEnchantmentHistoryEntry))
			{
				return this.Create_CardEnchantmentHistoryEntry(options);
			}
			if (type == typeof(CardTransformationHistoryEntry))
			{
				return this.Create_CardTransformationHistoryEntry(options);
			}
			if (type == typeof(EventOptionHistoryEntry))
			{
				return this.Create_EventOptionHistoryEntry(options);
			}
			if (type == typeof(MapPointHistoryEntry))
			{
				return this.Create_MapPointHistoryEntry(options);
			}
			if (type == typeof(MapPointRoomHistoryEntry))
			{
				return this.Create_MapPointRoomHistoryEntry(options);
			}
			if (type == typeof(ModelChoiceHistoryEntry))
			{
				return this.Create_ModelChoiceHistoryEntry(options);
			}
			if (type == typeof(PlayerMapPointHistoryEntry))
			{
				return this.Create_PlayerMapPointHistoryEntry(options);
			}
			if (type == typeof(RunHistory))
			{
				return this.Create_RunHistory(options);
			}
			if (type == typeof(RunHistoryPlayer))
			{
				return this.Create_RunHistoryPlayer(options);
			}
			if (type == typeof(AncientCharacterStats))
			{
				return this.Create_AncientCharacterStats(options);
			}
			if (type == typeof(AncientStats))
			{
				return this.Create_AncientStats(options);
			}
			if (type == typeof(BadgeStats))
			{
				return this.Create_BadgeStats(options);
			}
			if (type == typeof(CardStats))
			{
				return this.Create_CardStats(options);
			}
			if (type == typeof(CharacterStats))
			{
				return this.Create_CharacterStats(options);
			}
			if (type == typeof(EncounterStats))
			{
				return this.Create_EncounterStats(options);
			}
			if (type == typeof(EnemyStats))
			{
				return this.Create_EnemyStats(options);
			}
			if (type == typeof(EpochState))
			{
				return this.Create_EpochState(options);
			}
			if (type == typeof(FightStats))
			{
				return this.Create_FightStats(options);
			}
			if (type == typeof(SerializableMapDrawingLine))
			{
				return this.Create_SerializableMapDrawingLine(options);
			}
			if (type == typeof(SerializableMapDrawings))
			{
				return this.Create_SerializableMapDrawings(options);
			}
			if (type == typeof(SerializablePlayerMapDrawings))
			{
				return this.Create_SerializablePlayerMapDrawings(options);
			}
			if (type == typeof(MigratingData))
			{
				return this.Create_MigratingData(options);
			}
			if (type == typeof(PrefsSave))
			{
				return this.Create_PrefsSave(options);
			}
			if (type == typeof(ProfileSave))
			{
				return this.Create_ProfileSave(options);
			}
			if (type == typeof(SavedProperties))
			{
				return this.Create_SavedProperties(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<bool>))
			{
				return this.Create_SavedPropertyBoolean(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<ModelId>))
			{
				return this.Create_SavedPropertyModelId(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<SerializableCard[]>))
			{
				return this.Create_SavedPropertySerializableCardArray(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<SerializableCard>))
			{
				return this.Create_SavedPropertySerializableCard(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<int[]>))
			{
				return this.Create_SavedPropertyInt32Array(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<int>))
			{
				return this.Create_SavedPropertyInt32(options);
			}
			if (type == typeof(SavedProperties.SavedProperty<string>))
			{
				return this.Create_SavedPropertyString(options);
			}
			if (type == typeof(SerializableActMap))
			{
				return this.Create_SerializableActMap(options);
			}
			if (type == typeof(SerializableActModel))
			{
				return this.Create_SerializableActModel(options);
			}
			if (type == typeof(SerializableBadge))
			{
				return this.Create_SerializableBadge(options);
			}
			if (type == typeof(SerializableCard))
			{
				return this.Create_SerializableCard(options);
			}
			if (type == typeof(SerializableCard[]))
			{
				return this.Create_SerializableCardArray(options);
			}
			if (type == typeof(SerializableEnchantment))
			{
				return this.Create_SerializableEnchantment(options);
			}
			if (type == typeof(SerializableExtraRunFields))
			{
				return this.Create_SerializableExtraRunFields(options);
			}
			if (type == typeof(SerializableMapPoint))
			{
				return this.Create_SerializableMapPoint(options);
			}
			if (type == typeof(SerializableModifier))
			{
				return this.Create_SerializableModifier(options);
			}
			if (type == typeof(SerializablePlayer))
			{
				return this.Create_SerializablePlayer(options);
			}
			if (type == typeof(SerializablePlayerOddsSet))
			{
				return this.Create_SerializablePlayerOddsSet(options);
			}
			if (type == typeof(SerializablePotion))
			{
				return this.Create_SerializablePotion(options);
			}
			if (type == typeof(SerializableRelic))
			{
				return this.Create_SerializableRelic(options);
			}
			if (type == typeof(SerializableRelicGrabBag))
			{
				return this.Create_SerializableRelicGrabBag(options);
			}
			if (type == typeof(SerializableReward))
			{
				return this.Create_SerializableReward(options);
			}
			if (type == typeof(SerializableRoom))
			{
				return this.Create_SerializableRoom(options);
			}
			if (type == typeof(SerializableRoomSet))
			{
				return this.Create_SerializableRoomSet(options);
			}
			if (type == typeof(SerializableRunOddsSet))
			{
				return this.Create_SerializableRunOddsSet(options);
			}
			if (type == typeof(SerializableRunRngSet))
			{
				return this.Create_SerializableRunRngSet(options);
			}
			if (type == typeof(SerializableEpoch))
			{
				return this.Create_SerializableEpoch(options);
			}
			if (type == typeof(SerializableExtraPlayerFields))
			{
				return this.Create_SerializableExtraPlayerFields(options);
			}
			if (type == typeof(SerializablePlayerRngSet))
			{
				return this.Create_SerializablePlayerRngSet(options);
			}
			if (type == typeof(SerializableProgress))
			{
				return this.Create_SerializableProgress(options);
			}
			if (type == typeof(SerializableRng))
			{
				return this.Create_SerializableRng(options);
			}
			if (type == typeof(SerializableRun))
			{
				return this.Create_SerializableRun(options);
			}
			if (type == typeof(SerializableUnlockedAchievement))
			{
				return this.Create_SerializableUnlockedAchievement(options);
			}
			if (type == typeof(SettingsSave))
			{
				return this.Create_SettingsSave(options);
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
			if (type == typeof(SerializableUnlockState))
			{
				return this.Create_SerializableUnlockState(options);
			}
			if (type == typeof(Dictionary<RelicRarity, List<ModelId>>))
			{
				return this.Create_DictionaryRelicRarityListModelId(options);
			}
			if (type == typeof(Dictionary<PlayerRngType, SerializableRng>))
			{
				return this.Create_DictionaryPlayerRngTypeSerializableRng(options);
			}
			if (type == typeof(Dictionary<RunRngType, SerializableRng>))
			{
				return this.Create_DictionaryRunRngTypeSerializableRng(options);
			}
			if (type == typeof(Dictionary<string, object>))
			{
				return this.Create_DictionaryStringObject(options);
			}
			if (type == typeof(Dictionary<string, string>))
			{
				return this.Create_DictionaryStringString(options);
			}
			if (type == typeof(Dictionary<ulong, List<SerializableReward>>))
			{
				return this.Create_DictionaryUInt64ListSerializableReward(options);
			}
			if (type == typeof(IEnumerable<SerializableBadge>))
			{
				return this.Create_IEnumerableSerializableBadge(options);
			}
			if (type == typeof(IEnumerable<SerializableCard>))
			{
				return this.Create_IEnumerableSerializableCard(options);
			}
			if (type == typeof(IEnumerable<SerializablePotion>))
			{
				return this.Create_IEnumerableSerializablePotion(options);
			}
			if (type == typeof(IEnumerable<SerializableRelic>))
			{
				return this.Create_IEnumerableSerializableRelic(options);
			}
			if (type == typeof(List<Vector2>))
			{
				return this.Create_ListVector2(options);
			}
			if (type == typeof(List<MapCoord>))
			{
				return this.Create_ListMapCoord(options);
			}
			if (type == typeof(List<ModDependency>))
			{
				return this.Create_ListModDependency(options);
			}
			if (type == typeof(List<SettingsSaveMod>))
			{
				return this.Create_ListSettingsSaveMod(options);
			}
			if (type == typeof(List<ModelId>))
			{
				return this.Create_ListModelId(options);
			}
			if (type == typeof(List<NullLeaderboard>))
			{
				return this.Create_ListNullLeaderboard(options);
			}
			if (type == typeof(List<NullLeaderboardFileEntry>))
			{
				return this.Create_ListNullLeaderboardFileEntry(options);
			}
			if (type == typeof(List<NullMultiplayerName>))
			{
				return this.Create_ListNullMultiplayerName(options);
			}
			if (type == typeof(List<AncientChoiceHistoryEntry>))
			{
				return this.Create_ListAncientChoiceHistoryEntry(options);
			}
			if (type == typeof(List<CardChoiceHistoryEntry>))
			{
				return this.Create_ListCardChoiceHistoryEntry(options);
			}
			if (type == typeof(List<CardEnchantmentHistoryEntry>))
			{
				return this.Create_ListCardEnchantmentHistoryEntry(options);
			}
			if (type == typeof(List<CardTransformationHistoryEntry>))
			{
				return this.Create_ListCardTransformationHistoryEntry(options);
			}
			if (type == typeof(List<EventOptionHistoryEntry>))
			{
				return this.Create_ListEventOptionHistoryEntry(options);
			}
			if (type == typeof(List<MapPointHistoryEntry>))
			{
				return this.Create_ListMapPointHistoryEntry(options);
			}
			if (type == typeof(List<MapPointRoomHistoryEntry>))
			{
				return this.Create_ListMapPointRoomHistoryEntry(options);
			}
			if (type == typeof(List<ModelChoiceHistoryEntry>))
			{
				return this.Create_ListModelChoiceHistoryEntry(options);
			}
			if (type == typeof(List<PlayerMapPointHistoryEntry>))
			{
				return this.Create_ListPlayerMapPointHistoryEntry(options);
			}
			if (type == typeof(List<RunHistoryPlayer>))
			{
				return this.Create_ListRunHistoryPlayer(options);
			}
			if (type == typeof(List<AncientCharacterStats>))
			{
				return this.Create_ListAncientCharacterStats(options);
			}
			if (type == typeof(List<AncientStats>))
			{
				return this.Create_ListAncientStats(options);
			}
			if (type == typeof(List<BadgeStats>))
			{
				return this.Create_ListBadgeStats(options);
			}
			if (type == typeof(List<CardStats>))
			{
				return this.Create_ListCardStats(options);
			}
			if (type == typeof(List<CharacterStats>))
			{
				return this.Create_ListCharacterStats(options);
			}
			if (type == typeof(List<EncounterStats>))
			{
				return this.Create_ListEncounterStats(options);
			}
			if (type == typeof(List<EnemyStats>))
			{
				return this.Create_ListEnemyStats(options);
			}
			if (type == typeof(List<FightStats>))
			{
				return this.Create_ListFightStats(options);
			}
			if (type == typeof(List<SerializableMapDrawingLine>))
			{
				return this.Create_ListSerializableMapDrawingLine(options);
			}
			if (type == typeof(List<SerializablePlayerMapDrawings>))
			{
				return this.Create_ListSerializablePlayerMapDrawings(options);
			}
			if (type == typeof(List<MigratingData>))
			{
				return this.Create_ListMigratingData(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<bool>>))
			{
				return this.Create_ListSavedPropertyBoolean(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<ModelId>>))
			{
				return this.Create_ListSavedPropertyModelId(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<SerializableCard[]>>))
			{
				return this.Create_ListSavedPropertySerializableCardArray(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<SerializableCard>>))
			{
				return this.Create_ListSavedPropertySerializableCard(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<int[]>>))
			{
				return this.Create_ListSavedPropertyInt32Array(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<int>>))
			{
				return this.Create_ListSavedPropertyInt32(options);
			}
			if (type == typeof(List<SavedProperties.SavedProperty<string>>))
			{
				return this.Create_ListSavedPropertyString(options);
			}
			if (type == typeof(List<SerializableActModel>))
			{
				return this.Create_ListSerializableActModel(options);
			}
			if (type == typeof(List<SerializableCard>))
			{
				return this.Create_ListSerializableCard(options);
			}
			if (type == typeof(List<SerializableMapPoint>))
			{
				return this.Create_ListSerializableMapPoint(options);
			}
			if (type == typeof(List<SerializableModifier>))
			{
				return this.Create_ListSerializableModifier(options);
			}
			if (type == typeof(List<SerializablePlayer>))
			{
				return this.Create_ListSerializablePlayer(options);
			}
			if (type == typeof(List<SerializablePotion>))
			{
				return this.Create_ListSerializablePotion(options);
			}
			if (type == typeof(List<SerializableRelic>))
			{
				return this.Create_ListSerializableRelic(options);
			}
			if (type == typeof(List<SerializableReward>))
			{
				return this.Create_ListSerializableReward(options);
			}
			if (type == typeof(List<SerializableEpoch>))
			{
				return this.Create_ListSerializableEpoch(options);
			}
			if (type == typeof(List<SerializableUnlockedAchievement>))
			{
				return this.Create_ListSerializableUnlockedAchievement(options);
			}
			if (type == typeof(List<Dictionary<string, object>>))
			{
				return this.Create_ListDictionaryStringObject(options);
			}
			if (type == typeof(List<List<MapPointHistoryEntry>>))
			{
				return this.Create_ListListMapPointHistoryEntry(options);
			}
			if (type == typeof(List<List<PlayerMapPointHistoryEntry>>))
			{
				return this.Create_ListListPlayerMapPointHistoryEntry(options);
			}
			if (type == typeof(List<JsonNode>))
			{
				return this.Create_ListJsonNode(options);
			}
			if (type == typeof(List<string>))
			{
				return this.Create_ListString(options);
			}
			if (type == typeof(List<ulong>))
			{
				return this.Create_ListUInt64(options);
			}
			if (type == typeof(DateTimeOffset))
			{
				return this.Create_DateTimeOffset(options);
			}
			if (type == typeof(DateTimeOffset?))
			{
				return this.Create_NullableDateTimeOffset(options);
			}
			if (type == typeof(JsonNode))
			{
				return this.Create_JsonNode(options);
			}
			if (type == typeof(JsonObject))
			{
				return this.Create_JsonObject(options);
			}
			if (type == typeof(int))
			{
				return this.Create_Int32(options);
			}
			if (type == typeof(int?))
			{
				return this.Create_NullableInt32(options);
			}
			if (type == typeof(int[]))
			{
				return this.Create_Int32Array(options);
			}
			if (type == typeof(long))
			{
				return this.Create_Int64(options);
			}
			if (type == typeof(object))
			{
				return this.Create_Object(options);
			}
			if (type == typeof(string))
			{
				return this.Create_String(options);
			}
			if (type == typeof(ulong))
			{
				return this.Create_UInt64(options);
			}
			return null;
		}

		// Token: 0x040001AC RID: 428
		[Nullable(2)]
		private JsonTypeInfo<bool> _Boolean;

		// Token: 0x040001AD RID: 429
		[Nullable(2)]
		private JsonTypeInfo<double> _Double;

		// Token: 0x040001AE RID: 430
		[Nullable(2)]
		private JsonTypeInfo<float> _Single;

		// Token: 0x040001AF RID: 431
		[Nullable(2)]
		private JsonTypeInfo<Vector2> _Vector2;

		// Token: 0x040001B0 RID: 432
		[Nullable(2)]
		private JsonTypeInfo<Vector2I> _Vector2I;

		// Token: 0x040001B1 RID: 433
		[Nullable(2)]
		private JsonTypeInfo<ControllerMappingType> _ControllerMappingType;

		// Token: 0x040001B2 RID: 434
		[Nullable(2)]
		private JsonTypeInfo<RelicRarity> _RelicRarity;

		// Token: 0x040001B3 RID: 435
		[Nullable(2)]
		private JsonTypeInfo<PlayerRngType> _PlayerRngType;

		// Token: 0x040001B4 RID: 436
		[Nullable(2)]
		private JsonTypeInfo<RunRngType> _RunRngType;

		// Token: 0x040001B5 RID: 437
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<LocString> _LocString;

		// Token: 0x040001B6 RID: 438
		[Nullable(2)]
		private JsonTypeInfo<MapCoord> _MapCoord;

		// Token: 0x040001B7 RID: 439
		[Nullable(2)]
		private JsonTypeInfo<MapPointType> _MapPointType;

		// Token: 0x040001B8 RID: 440
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModDependency> _ModDependency;

		// Token: 0x040001B9 RID: 441
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModManifest> _ModManifest;

		// Token: 0x040001BA RID: 442
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModSettings> _ModSettings;

		// Token: 0x040001BB RID: 443
		[Nullable(2)]
		private JsonTypeInfo<ModSource> _ModSource;

		// Token: 0x040001BC RID: 444
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SettingsSaveMod> _SettingsSaveMod;

		// Token: 0x040001BD RID: 445
		[Nullable(2)]
		private JsonTypeInfo<BadgeRarity> _BadgeRarity;

		// Token: 0x040001BE RID: 446
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ModelId> _ModelId;

		// Token: 0x040001BF RID: 447
		[Nullable(2)]
		private JsonTypeInfo<FeedbackData> _FeedbackData;

		// Token: 0x040001C0 RID: 448
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<NullLeaderboard> _NullLeaderboard;

		// Token: 0x040001C1 RID: 449
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<NullLeaderboardFile> _NullLeaderboardFile;

		// Token: 0x040001C2 RID: 450
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<NullLeaderboardFileEntry> _NullLeaderboardFileEntry;

		// Token: 0x040001C3 RID: 451
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<NullMultiplayerName> _NullMultiplayerName;

		// Token: 0x040001C4 RID: 452
		[Nullable(2)]
		private JsonTypeInfo<PlatformType> _PlatformType;

		// Token: 0x040001C5 RID: 453
		[Nullable(2)]
		private JsonTypeInfo<RewardType> _RewardType;

		// Token: 0x040001C6 RID: 454
		[Nullable(2)]
		private JsonTypeInfo<RoomType> _RoomType;

		// Token: 0x040001C7 RID: 455
		[Nullable(2)]
		private JsonTypeInfo<CardCreationSource> _CardCreationSource;

		// Token: 0x040001C8 RID: 456
		[Nullable(2)]
		private JsonTypeInfo<CardRarityOddsType> _CardRarityOddsType;

		// Token: 0x040001C9 RID: 457
		[Nullable(2)]
		private JsonTypeInfo<GameMode> _GameMode;

		// Token: 0x040001CA RID: 458
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<AncientChoiceHistoryEntry> _AncientChoiceHistoryEntry;

		// Token: 0x040001CB RID: 459
		[Nullable(2)]
		private JsonTypeInfo<CardChoiceHistoryEntry> _CardChoiceHistoryEntry;

		// Token: 0x040001CC RID: 460
		[Nullable(2)]
		private JsonTypeInfo<CardEnchantmentHistoryEntry> _CardEnchantmentHistoryEntry;

		// Token: 0x040001CD RID: 461
		[Nullable(2)]
		private JsonTypeInfo<CardTransformationHistoryEntry> _CardTransformationHistoryEntry;

		// Token: 0x040001CE RID: 462
		[Nullable(2)]
		private JsonTypeInfo<EventOptionHistoryEntry> _EventOptionHistoryEntry;

		// Token: 0x040001CF RID: 463
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<MapPointHistoryEntry> _MapPointHistoryEntry;

		// Token: 0x040001D0 RID: 464
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<MapPointRoomHistoryEntry> _MapPointRoomHistoryEntry;

		// Token: 0x040001D1 RID: 465
		[Nullable(2)]
		private JsonTypeInfo<ModelChoiceHistoryEntry> _ModelChoiceHistoryEntry;

		// Token: 0x040001D2 RID: 466
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<PlayerMapPointHistoryEntry> _PlayerMapPointHistoryEntry;

		// Token: 0x040001D3 RID: 467
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<RunHistory> _RunHistory;

		// Token: 0x040001D4 RID: 468
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<RunHistoryPlayer> _RunHistoryPlayer;

		// Token: 0x040001D5 RID: 469
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<AncientCharacterStats> _AncientCharacterStats;

		// Token: 0x040001D6 RID: 470
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<AncientStats> _AncientStats;

		// Token: 0x040001D7 RID: 471
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<BadgeStats> _BadgeStats;

		// Token: 0x040001D8 RID: 472
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<CardStats> _CardStats;

		// Token: 0x040001D9 RID: 473
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<CharacterStats> _CharacterStats;

		// Token: 0x040001DA RID: 474
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EncounterStats> _EncounterStats;

		// Token: 0x040001DB RID: 475
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<EnemyStats> _EnemyStats;

		// Token: 0x040001DC RID: 476
		[Nullable(2)]
		private JsonTypeInfo<EpochState> _EpochState;

		// Token: 0x040001DD RID: 477
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<FightStats> _FightStats;

		// Token: 0x040001DE RID: 478
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableMapDrawingLine> _SerializableMapDrawingLine;

		// Token: 0x040001DF RID: 479
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableMapDrawings> _SerializableMapDrawings;

		// Token: 0x040001E0 RID: 480
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializablePlayerMapDrawings> _SerializablePlayerMapDrawings;

		// Token: 0x040001E1 RID: 481
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<MigratingData> _MigratingData;

		// Token: 0x040001E2 RID: 482
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<PrefsSave> _PrefsSave;

		// Token: 0x040001E3 RID: 483
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<ProfileSave> _ProfileSave;

		// Token: 0x040001E4 RID: 484
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SavedProperties> _SavedProperties;

		// Token: 0x040001E5 RID: 485
		[Nullable(new byte[] { 2, 0 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<bool>> _SavedPropertyBoolean;

		// Token: 0x040001E6 RID: 486
		[Nullable(new byte[] { 2, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<ModelId>> _SavedPropertyModelId;

		// Token: 0x040001E7 RID: 487
		[Nullable(new byte[] { 2, 0, 1, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard[]>> _SavedPropertySerializableCardArray;

		// Token: 0x040001E8 RID: 488
		[Nullable(new byte[] { 2, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<SerializableCard>> _SavedPropertySerializableCard;

		// Token: 0x040001E9 RID: 489
		[Nullable(new byte[] { 2, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<int[]>> _SavedPropertyInt32Array;

		// Token: 0x040001EA RID: 490
		[Nullable(new byte[] { 2, 0 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<int>> _SavedPropertyInt32;

		// Token: 0x040001EB RID: 491
		[Nullable(new byte[] { 2, 0, 1 })]
		private JsonTypeInfo<SavedProperties.SavedProperty<string>> _SavedPropertyString;

		// Token: 0x040001EC RID: 492
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableActMap> _SerializableActMap;

		// Token: 0x040001ED RID: 493
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableActModel> _SerializableActModel;

		// Token: 0x040001EE RID: 494
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableBadge> _SerializableBadge;

		// Token: 0x040001EF RID: 495
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableCard> _SerializableCard;

		// Token: 0x040001F0 RID: 496
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<SerializableCard[]> _SerializableCardArray;

		// Token: 0x040001F1 RID: 497
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableEnchantment> _SerializableEnchantment;

		// Token: 0x040001F2 RID: 498
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableExtraRunFields> _SerializableExtraRunFields;

		// Token: 0x040001F3 RID: 499
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableMapPoint> _SerializableMapPoint;

		// Token: 0x040001F4 RID: 500
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableModifier> _SerializableModifier;

		// Token: 0x040001F5 RID: 501
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializablePlayer> _SerializablePlayer;

		// Token: 0x040001F6 RID: 502
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializablePlayerOddsSet> _SerializablePlayerOddsSet;

		// Token: 0x040001F7 RID: 503
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializablePotion> _SerializablePotion;

		// Token: 0x040001F8 RID: 504
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRelic> _SerializableRelic;

		// Token: 0x040001F9 RID: 505
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRelicGrabBag> _SerializableRelicGrabBag;

		// Token: 0x040001FA RID: 506
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableReward> _SerializableReward;

		// Token: 0x040001FB RID: 507
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRoom> _SerializableRoom;

		// Token: 0x040001FC RID: 508
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRoomSet> _SerializableRoomSet;

		// Token: 0x040001FD RID: 509
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRunOddsSet> _SerializableRunOddsSet;

		// Token: 0x040001FE RID: 510
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRunRngSet> _SerializableRunRngSet;

		// Token: 0x040001FF RID: 511
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableEpoch> _SerializableEpoch;

		// Token: 0x04000200 RID: 512
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableExtraPlayerFields> _SerializableExtraPlayerFields;

		// Token: 0x04000201 RID: 513
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializablePlayerRngSet> _SerializablePlayerRngSet;

		// Token: 0x04000202 RID: 514
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableProgress> _SerializableProgress;

		// Token: 0x04000203 RID: 515
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRng> _SerializableRng;

		// Token: 0x04000204 RID: 516
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableRun> _SerializableRun;

		// Token: 0x04000205 RID: 517
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableUnlockedAchievement> _SerializableUnlockedAchievement;

		// Token: 0x04000206 RID: 518
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SettingsSave> _SettingsSave;

		// Token: 0x04000207 RID: 519
		[Nullable(2)]
		private JsonTypeInfo<AspectRatioSetting> _AspectRatioSetting;

		// Token: 0x04000208 RID: 520
		[Nullable(2)]
		private JsonTypeInfo<FastModeType> _FastModeType;

		// Token: 0x04000209 RID: 521
		[Nullable(2)]
		private JsonTypeInfo<VSyncType> _VSyncType;

		// Token: 0x0400020A RID: 522
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<SerializableUnlockState> _SerializableUnlockState;

		// Token: 0x0400020B RID: 523
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<Dictionary<RelicRarity, List<ModelId>>> _DictionaryRelicRarityListModelId;

		// Token: 0x0400020C RID: 524
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<Dictionary<PlayerRngType, SerializableRng>> _DictionaryPlayerRngTypeSerializableRng;

		// Token: 0x0400020D RID: 525
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<Dictionary<RunRngType, SerializableRng>> _DictionaryRunRngTypeSerializableRng;

		// Token: 0x0400020E RID: 526
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<Dictionary<string, object>> _DictionaryStringObject;

		// Token: 0x0400020F RID: 527
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<Dictionary<string, string>> _DictionaryStringString;

		// Token: 0x04000210 RID: 528
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<Dictionary<ulong, List<SerializableReward>>> _DictionaryUInt64ListSerializableReward;

		// Token: 0x04000211 RID: 529
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<IEnumerable<SerializableBadge>> _IEnumerableSerializableBadge;

		// Token: 0x04000212 RID: 530
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<IEnumerable<SerializableCard>> _IEnumerableSerializableCard;

		// Token: 0x04000213 RID: 531
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<IEnumerable<SerializablePotion>> _IEnumerableSerializablePotion;

		// Token: 0x04000214 RID: 532
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<IEnumerable<SerializableRelic>> _IEnumerableSerializableRelic;

		// Token: 0x04000215 RID: 533
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<Vector2>> _ListVector2;

		// Token: 0x04000216 RID: 534
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<MapCoord>> _ListMapCoord;

		// Token: 0x04000217 RID: 535
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<ModDependency>> _ListModDependency;

		// Token: 0x04000218 RID: 536
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SettingsSaveMod>> _ListSettingsSaveMod;

		// Token: 0x04000219 RID: 537
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<ModelId>> _ListModelId;

		// Token: 0x0400021A RID: 538
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<NullLeaderboard>> _ListNullLeaderboard;

		// Token: 0x0400021B RID: 539
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<NullLeaderboardFileEntry>> _ListNullLeaderboardFileEntry;

		// Token: 0x0400021C RID: 540
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<NullMultiplayerName>> _ListNullMultiplayerName;

		// Token: 0x0400021D RID: 541
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<AncientChoiceHistoryEntry>> _ListAncientChoiceHistoryEntry;

		// Token: 0x0400021E RID: 542
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<CardChoiceHistoryEntry>> _ListCardChoiceHistoryEntry;

		// Token: 0x0400021F RID: 543
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<CardEnchantmentHistoryEntry>> _ListCardEnchantmentHistoryEntry;

		// Token: 0x04000220 RID: 544
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<CardTransformationHistoryEntry>> _ListCardTransformationHistoryEntry;

		// Token: 0x04000221 RID: 545
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<EventOptionHistoryEntry>> _ListEventOptionHistoryEntry;

		// Token: 0x04000222 RID: 546
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<MapPointHistoryEntry>> _ListMapPointHistoryEntry;

		// Token: 0x04000223 RID: 547
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<MapPointRoomHistoryEntry>> _ListMapPointRoomHistoryEntry;

		// Token: 0x04000224 RID: 548
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<ModelChoiceHistoryEntry>> _ListModelChoiceHistoryEntry;

		// Token: 0x04000225 RID: 549
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<PlayerMapPointHistoryEntry>> _ListPlayerMapPointHistoryEntry;

		// Token: 0x04000226 RID: 550
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<RunHistoryPlayer>> _ListRunHistoryPlayer;

		// Token: 0x04000227 RID: 551
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<AncientCharacterStats>> _ListAncientCharacterStats;

		// Token: 0x04000228 RID: 552
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<AncientStats>> _ListAncientStats;

		// Token: 0x04000229 RID: 553
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<BadgeStats>> _ListBadgeStats;

		// Token: 0x0400022A RID: 554
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<CardStats>> _ListCardStats;

		// Token: 0x0400022B RID: 555
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<CharacterStats>> _ListCharacterStats;

		// Token: 0x0400022C RID: 556
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<EncounterStats>> _ListEncounterStats;

		// Token: 0x0400022D RID: 557
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<EnemyStats>> _ListEnemyStats;

		// Token: 0x0400022E RID: 558
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<FightStats>> _ListFightStats;

		// Token: 0x0400022F RID: 559
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableMapDrawingLine>> _ListSerializableMapDrawingLine;

		// Token: 0x04000230 RID: 560
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializablePlayerMapDrawings>> _ListSerializablePlayerMapDrawings;

		// Token: 0x04000231 RID: 561
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<MigratingData>> _ListMigratingData;

		// Token: 0x04000232 RID: 562
		[Nullable(new byte[] { 2, 1, 0 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<bool>>> _ListSavedPropertyBoolean;

		// Token: 0x04000233 RID: 563
		[Nullable(new byte[] { 2, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<ModelId>>> _ListSavedPropertyModelId;

		// Token: 0x04000234 RID: 564
		[Nullable(new byte[] { 2, 1, 0, 1, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard[]>>> _ListSavedPropertySerializableCardArray;

		// Token: 0x04000235 RID: 565
		[Nullable(new byte[] { 2, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<SerializableCard>>> _ListSavedPropertySerializableCard;

		// Token: 0x04000236 RID: 566
		[Nullable(new byte[] { 2, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<int[]>>> _ListSavedPropertyInt32Array;

		// Token: 0x04000237 RID: 567
		[Nullable(new byte[] { 2, 1, 0 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<int>>> _ListSavedPropertyInt32;

		// Token: 0x04000238 RID: 568
		[Nullable(new byte[] { 2, 1, 0, 1 })]
		private JsonTypeInfo<List<SavedProperties.SavedProperty<string>>> _ListSavedPropertyString;

		// Token: 0x04000239 RID: 569
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableActModel>> _ListSerializableActModel;

		// Token: 0x0400023A RID: 570
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableCard>> _ListSerializableCard;

		// Token: 0x0400023B RID: 571
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableMapPoint>> _ListSerializableMapPoint;

		// Token: 0x0400023C RID: 572
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableModifier>> _ListSerializableModifier;

		// Token: 0x0400023D RID: 573
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializablePlayer>> _ListSerializablePlayer;

		// Token: 0x0400023E RID: 574
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializablePotion>> _ListSerializablePotion;

		// Token: 0x0400023F RID: 575
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableRelic>> _ListSerializableRelic;

		// Token: 0x04000240 RID: 576
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableReward>> _ListSerializableReward;

		// Token: 0x04000241 RID: 577
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableEpoch>> _ListSerializableEpoch;

		// Token: 0x04000242 RID: 578
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<SerializableUnlockedAchievement>> _ListSerializableUnlockedAchievement;

		// Token: 0x04000243 RID: 579
		[Nullable(new byte[] { 2, 1, 1, 1, 1 })]
		private JsonTypeInfo<List<Dictionary<string, object>>> _ListDictionaryStringObject;

		// Token: 0x04000244 RID: 580
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<List<List<MapPointHistoryEntry>>> _ListListMapPointHistoryEntry;

		// Token: 0x04000245 RID: 581
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private JsonTypeInfo<List<List<PlayerMapPointHistoryEntry>>> _ListListPlayerMapPointHistoryEntry;

		// Token: 0x04000246 RID: 582
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<JsonNode>> _ListJsonNode;

		// Token: 0x04000247 RID: 583
		[Nullable(new byte[] { 2, 1, 1 })]
		private JsonTypeInfo<List<string>> _ListString;

		// Token: 0x04000248 RID: 584
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<List<ulong>> _ListUInt64;

		// Token: 0x04000249 RID: 585
		[Nullable(2)]
		private JsonTypeInfo<DateTimeOffset> _DateTimeOffset;

		// Token: 0x0400024A RID: 586
		[Nullable(2)]
		private JsonTypeInfo<DateTimeOffset?> _NullableDateTimeOffset;

		// Token: 0x0400024B RID: 587
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<JsonNode> _JsonNode;

		// Token: 0x0400024C RID: 588
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<JsonObject> _JsonObject;

		// Token: 0x0400024D RID: 589
		[Nullable(2)]
		private JsonTypeInfo<int> _Int32;

		// Token: 0x0400024E RID: 590
		[Nullable(2)]
		private JsonTypeInfo<int?> _NullableInt32;

		// Token: 0x0400024F RID: 591
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<int[]> _Int32Array;

		// Token: 0x04000250 RID: 592
		[Nullable(2)]
		private JsonTypeInfo<long> _Int64;

		// Token: 0x04000251 RID: 593
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<object> _Object;

		// Token: 0x04000252 RID: 594
		[Nullable(new byte[] { 2, 1 })]
		private JsonTypeInfo<string> _String;

		// Token: 0x04000253 RID: 595
		[Nullable(2)]
		private JsonTypeInfo<ulong> _UInt64;

		// Token: 0x04000254 RID: 596
		[Nullable(1)]
		private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions
		{
			Converters = 
			{
				new ModelIdRunSaveConverter(),
				new SnakeCaseJsonStringEnumConverter<Achievement>(),
				new SnakeCaseJsonStringEnumConverter<AspectRatioSetting>(),
				new SnakeCaseJsonStringEnumConverter<VSyncType>(),
				new SnakeCaseJsonStringEnumConverter<GameMode>(),
				new SnakeCaseJsonStringEnumConverter<RelicRarity>(),
				new SnakeCaseJsonStringEnumConverter<RunRngType>(),
				new SnakeCaseJsonStringEnumConverter<PlayerRngType>(),
				new SnakeCaseJsonStringEnumConverter<MapPointType>(),
				new SnakeCaseJsonStringEnumConverter<ModSource>(),
				new SnakeCaseJsonStringEnumConverter<PlatformType>(),
				new SnakeCaseJsonStringEnumConverter<RewardType>(),
				new SnakeCaseJsonStringEnumConverter<RoomType>(),
				new SnakeCaseJsonStringEnumConverter<EpochState>(),
				new SnakeCaseJsonStringEnumConverter<FastModeType>(),
				new SnakeCaseJsonStringEnumConverter<CardCreationSource>(),
				new SnakeCaseJsonStringEnumConverter<CardRarityOddsType>(),
				new SnakeCaseJsonStringEnumConverter<ControllerMappingType>(),
				new SnakeCaseJsonStringEnumConverter<BadgeRarity>()
			},
			IncludeFields = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
			WriteIndented = true
		};

		// Token: 0x04000255 RID: 597
		private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x02000E2B RID: 3627
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003379 RID: 13177
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <0>__LocStringCtorParamInit;

			// Token: 0x0400337A RID: 13178
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <1>__ModDependencyCtorParamInit;

			// Token: 0x0400337B RID: 13179
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <2>__ModelIdCtorParamInit;

			// Token: 0x0400337C RID: 13180
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <3>__NullLeaderboardFileEntryCtorParamInit;

			// Token: 0x0400337D RID: 13181
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <4>__RunHistoryCtorParamInit;

			// Token: 0x0400337E RID: 13182
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <5>__RunHistoryPlayerCtorParamInit;

			// Token: 0x0400337F RID: 13183
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <6>__AncientCharacterStatsCtorParamInit;

			// Token: 0x04003380 RID: 13184
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <7>__AncientStatsCtorParamInit;

			// Token: 0x04003381 RID: 13185
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <8>__BadgeStatsCtorParamInit;

			// Token: 0x04003382 RID: 13186
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <9>__CardStatsCtorParamInit;

			// Token: 0x04003383 RID: 13187
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <10>__CharacterStatsCtorParamInit;

			// Token: 0x04003384 RID: 13188
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <11>__EncounterStatsCtorParamInit;

			// Token: 0x04003385 RID: 13189
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <12>__EnemyStatsCtorParamInit;

			// Token: 0x04003386 RID: 13190
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <13>__FightStatsCtorParamInit;

			// Token: 0x04003387 RID: 13191
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <14>__SerializableBadgeCtorParamInit;

			// Token: 0x04003388 RID: 13192
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <15>__SerializableEpochCtorParamInit;

			// Token: 0x04003389 RID: 13193
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <16>__SerializableProgressCtorParamInit;

			// Token: 0x0400338A RID: 13194
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<JsonParameterInfoValues[]> <17>__SerializableUnlockedAchievementCtorParamInit;
		}
	}
}
