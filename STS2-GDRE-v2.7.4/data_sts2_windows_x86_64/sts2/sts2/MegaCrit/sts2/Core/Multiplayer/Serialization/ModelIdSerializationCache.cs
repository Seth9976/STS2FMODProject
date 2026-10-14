using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	/// <summary>
	/// Database of string to integer mappings, for use in network serialization.
	/// </summary>
	// Token: 0x0200044C RID: 1100
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModelIdSerializationCache
	{
		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060044E6 RID: 17638 RVA: 0x0021394F File Offset: 0x00211B4F
		// (set) Token: 0x060044E7 RID: 17639 RVA: 0x00213956 File Offset: 0x00211B56
		public static int CategoryIdBitSize { get; private set; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060044E8 RID: 17640 RVA: 0x0021395E File Offset: 0x00211B5E
		// (set) Token: 0x060044E9 RID: 17641 RVA: 0x00213965 File Offset: 0x00211B65
		public static int EntryIdBitSize { get; private set; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x0021396D File Offset: 0x00211B6D
		// (set) Token: 0x060044EB RID: 17643 RVA: 0x00213974 File Offset: 0x00211B74
		public static int EpochIdBitSize { get; private set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x0021397C File Offset: 0x00211B7C
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x00213983 File Offset: 0x00211B83
		public static int PropertyIdBitSize { get; private set; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x0021398B File Offset: 0x00211B8B
		public static int MaxCategoryId
		{
			get
			{
				return ModelIdSerializationCache._netIdToCategoryNameMap.Count - 1;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x00213999 File Offset: 0x00211B99
		public static int MaxEntryId
		{
			get
			{
				return ModelIdSerializationCache._netIdToEntryNameMap.Count - 1;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x002139A7 File Offset: 0x00211BA7
		public static int MaxEpochId
		{
			get
			{
				return ModelIdSerializationCache._netIdToEpochNameMap.Count - 1;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060044F1 RID: 17649 RVA: 0x002139B5 File Offset: 0x00211BB5
		public static int MaxPropertyId
		{
			get
			{
				return ModelIdSerializationCache._netIdToPropertyNameMap.Count - 1;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x002139C3 File Offset: 0x00211BC3
		// (set) Token: 0x060044F3 RID: 17651 RVA: 0x002139CA File Offset: 0x00211BCA
		public static uint Hash { get; private set; }

		/// <summary>
		/// Initializes the serialization cache.
		/// Note that the alternative is to initialize in a static initializer, but if we do this we don't control the time
		/// at which it gets initialized, and it usually happens at a bad time during gameplay.
		/// </summary>
		// Token: 0x060044F4 RID: 17652 RVA: 0x002139D4 File Offset: 0x00211BD4
		public static void Init()
		{
			byte[] array = new byte[512];
			XxHash32 xxHash = new XxHash32();
			IEnumerable<Type> enumerable = ModelDb.All.Select<AbstractModel, Type>((AbstractModel m) => m.GetType());
			Func<Type, ModelId> func;
			if ((func = ModelIdSerializationCache.<>O.<0>__GetId) == null)
			{
				func = (ModelIdSerializationCache.<>O.<0>__GetId = new Func<Type, ModelId>(ModelDb.GetId));
			}
			List<ContentSorter<ModelId>.Item> list = ContentSorter<ModelId>.Sort(enumerable, func, true);
			foreach (ContentSorter<ModelId>.Item item in list)
			{
				ModelId id = item.id;
				if (!ModelIdSerializationCache._categoryNameToNetIdMap.ContainsKey(id.Category))
				{
					int count = ModelIdSerializationCache._netIdToCategoryNameMap.Count;
					ModelIdSerializationCache._categoryNameToNetIdMap[id.Category] = count;
					ModelIdSerializationCache._netIdToCategoryNameMap.Add(id.Category);
				}
				if (!ModelIdSerializationCache._entryNameToNetIdMap.ContainsKey(id.Entry))
				{
					int count2 = ModelIdSerializationCache._netIdToEntryNameMap.Count;
					ModelIdSerializationCache._entryNameToNetIdMap[id.Entry] = count2;
					ModelIdSerializationCache._netIdToEntryNameMap.Add(id.Entry);
				}
				Mod mod = item.mod;
				bool? flag;
				if (mod == null)
				{
					flag = null;
				}
				else
				{
					ModManifest manifest = mod.manifest;
					flag = ((manifest != null) ? new bool?(manifest.affectsGameplay) : null);
				}
				bool? flag2 = flag;
				bool valueOrDefault = flag2.GetValueOrDefault(true);
				if (valueOrDefault)
				{
					int num = Encoding.UTF8.GetBytes(id.Category, 0, id.Category.Length, array, 0);
					xxHash.Append(array.AsSpan<byte>(0, num));
					num = Encoding.UTF8.GetBytes(id.Entry, 0, id.Entry.Length, array, 0);
					xxHash.Append(array.AsSpan<byte>(0, num));
				}
			}
			foreach (ContentSorter<ModelId>.Item item2 in list)
			{
				Mod mod2 = item2.mod;
				bool? flag3;
				if (mod2 == null)
				{
					flag3 = null;
				}
				else
				{
					ModManifest manifest2 = mod2.manifest;
					flag3 = ((manifest2 != null) ? new bool?(manifest2.affectsGameplay) : null);
				}
				bool? flag2 = flag3;
				bool valueOrDefault2 = flag2.GetValueOrDefault(true);
				if (valueOrDefault2)
				{
					ModelIdSerializationCache.CachePropertiesForType(item2.type, xxHash, array);
				}
				else
				{
					ModelIdSerializationCache.CachePropertiesForType(item2.type, null, null);
				}
			}
			IEnumerable<Type> allEpochs = EpochModel.AllEpochs;
			Func<Type, string> func2;
			if ((func2 = ModelIdSerializationCache.<>O.<1>__GetId) == null)
			{
				func2 = (ModelIdSerializationCache.<>O.<1>__GetId = new Func<Type, string>(EpochModel.GetId));
			}
			IEnumerable<ContentSorter<string>.Item> enumerable2 = ContentSorter<string>.Sort(allEpochs, func2, true);
			foreach (ContentSorter<string>.Item item3 in enumerable2)
			{
				string id2 = item3.id;
				if (!ModelIdSerializationCache._epochNameToNetIdMap.ContainsKey(id2))
				{
					int count3 = ModelIdSerializationCache._netIdToEpochNameMap.Count;
					ModelIdSerializationCache._epochNameToNetIdMap[id2] = count3;
					ModelIdSerializationCache._netIdToEpochNameMap.Add(id2);
				}
				int bytes = Encoding.UTF8.GetBytes(id2, 0, id2.Length, array, 0);
				xxHash.Append(array.AsSpan<byte>(0, bytes));
			}
			ModelIdSerializationCache._initialized = true;
			ModelIdSerializationCache.CategoryIdBitSize = Mathf.CeilToInt(Math.Log2((double)ModelIdSerializationCache._netIdToCategoryNameMap.Count));
			ModelIdSerializationCache.EntryIdBitSize = Mathf.CeilToInt(Math.Log2((double)ModelIdSerializationCache._netIdToEntryNameMap.Count));
			ModelIdSerializationCache.PropertyIdBitSize = Mathf.CeilToInt(Math.Log2((double)ModelIdSerializationCache._netIdToPropertyNameMap.Count));
			ModelIdSerializationCache.EpochIdBitSize = Mathf.CeilToInt(Math.Log2((double)ModelIdSerializationCache._netIdToEpochNameMap.Count));
			ModelIdSerializationCache.Hash = xxHash.GetCurrentHashAsUInt32();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(90, 5);
			defaultInterpolatedStringHandler.AppendLiteral("ModelIdSerializationCache initialized. Categories: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToCategoryNameMap.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Entries: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToEntryNameMap.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Epochs: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToEpochNameMap.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Properties: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToPropertyNameMap.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" Hash: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(ModelIdSerializationCache.Hash);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (ModManager.State == ModManagerState.Initialized)
			{
				if (ModManager.Mods.Any<Mod>(delegate(Mod m)
				{
					if (m.state == ModLoadState.Loaded)
					{
						ModManifest manifest3 = m.manifest;
						return manifest3 != null && !manifest3.affectsGameplay;
					}
					return false;
				}))
				{
					Log.Info("  There are mods included that do not affect gameplay. Hash may not include all IDs.", 2);
				}
			}
			ModelIdSerializationCache._initialized = true;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x00213EA8 File Offset: 0x002120A8
		public static void ResetForTest()
		{
			ModelIdSerializationCache._savedPropertyCache.Clear();
			ModelIdSerializationCache._categoryNameToNetIdMap.Clear();
			ModelIdSerializationCache._netIdToCategoryNameMap.Clear();
			ModelIdSerializationCache._entryNameToNetIdMap.Clear();
			ModelIdSerializationCache._netIdToEntryNameMap.Clear();
			ModelIdSerializationCache._categoryNameToNetIdMap[ModelId.none.Category] = 0;
			ModelIdSerializationCache._netIdToCategoryNameMap.Add(ModelId.none.Category);
			ModelIdSerializationCache._entryNameToNetIdMap[ModelId.none.Entry] = 0;
			ModelIdSerializationCache._netIdToEntryNameMap.Add(ModelId.none.Entry);
			ModelIdSerializationCache._epochNameToNetIdMap.Clear();
			ModelIdSerializationCache._netIdToEpochNameMap.Clear();
			ModelIdSerializationCache._propertyNameToNetIdMap.Clear();
			ModelIdSerializationCache._netIdToPropertyNameMap.Clear();
			ModelIdSerializationCache.CategoryIdBitSize = 0;
			ModelIdSerializationCache.EntryIdBitSize = 0;
			ModelIdSerializationCache.EpochIdBitSize = 0;
			ModelIdSerializationCache.PropertyIdBitSize = 0;
			ModelIdSerializationCache.Hash = 0U;
			ModelIdSerializationCache._initialized = false;
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x00213F88 File Offset: 0x00212188
		[NullableContext(2)]
		private static void CachePropertiesForType([Nullable(1)] [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type, XxHash32 hasher, byte[] byteBuffer)
		{
			List<PropertyInfo> list = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where p.GetCustomAttribute<SavedPropertyAttribute>() != null
				select p).ToList<PropertyInfo>();
			List<PropertyInfo> list2 = list;
			Comparison<PropertyInfo> comparison;
			if ((comparison = ModelIdSerializationCache.<>O.<2>__CompareProperties) == null)
			{
				comparison = (ModelIdSerializationCache.<>O.<2>__CompareProperties = new Comparison<PropertyInfo>(ModelIdSerializationCache.CompareProperties));
			}
			list2.Sort(comparison);
			if (list.Count > 0)
			{
				ModelIdSerializationCache._savedPropertyCache[type] = list;
			}
			foreach (PropertyInfo propertyInfo in list)
			{
				if (!ModelIdSerializationCache._propertyNameToNetIdMap.ContainsKey(propertyInfo.Name))
				{
					int count = ModelIdSerializationCache._netIdToPropertyNameMap.Count;
					ModelIdSerializationCache._propertyNameToNetIdMap[propertyInfo.Name] = count;
					ModelIdSerializationCache._netIdToPropertyNameMap.Add(propertyInfo.Name);
					if (hasher != null && byteBuffer != null)
					{
						int bytes = Encoding.UTF8.GetBytes(propertyInfo.Name, 0, propertyInfo.Name.Length, byteBuffer, 0);
						hasher.Append(byteBuffer.AsSpan<byte>(0, bytes));
					}
				}
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x002140B8 File Offset: 0x002122B8
		private static int CompareProperties(PropertyInfo p1, PropertyInfo p2)
		{
			SavedPropertyAttribute customAttribute = p1.GetCustomAttribute<SavedPropertyAttribute>();
			SavedPropertyAttribute customAttribute2 = p2.GetCustomAttribute<SavedPropertyAttribute>();
			if (customAttribute.order != customAttribute2.order)
			{
				return customAttribute.order.CompareTo(customAttribute2.order);
			}
			return string.CompareOrdinal(p1.Name, p2.Name);
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00214104 File Offset: 0x00212304
		public static int GetNetIdForCategory(string category)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			int num;
			if (!ModelIdSerializationCache._categoryNameToNetIdMap.TryGetValue(category, out num))
			{
				throw new ArgumentException("ModelId category " + category + " could not be mapped to any net ID!");
			}
			return num;
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00214149 File Offset: 0x00212349
		public static bool TryGetNetIdForCategory(string category, out int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			return ModelIdSerializationCache._categoryNameToNetIdMap.TryGetValue(category, out netId);
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x0021416C File Offset: 0x0021236C
		public static string GetCategoryForNetId(int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			if (netId < 0 || netId >= ModelIdSerializationCache._netIdToCategoryNameMap.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("ModelId category ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(netId);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range! We have ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToCategoryNameMap.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" categories");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ModelIdSerializationCache._netIdToCategoryNameMap[netId];
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x002141FC File Offset: 0x002123FC
		public static int GetNetIdForEntry(string entry)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			int num;
			if (!ModelIdSerializationCache._entryNameToNetIdMap.TryGetValue(entry, out num))
			{
				throw new ArgumentException("ModelId entry " + entry + " could not be mapped to any net ID!");
			}
			return num;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00214241 File Offset: 0x00212441
		public static bool TryGetNetIdForEntry(string entry, out int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			return ModelIdSerializationCache._entryNameToNetIdMap.TryGetValue(entry, out netId);
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00214264 File Offset: 0x00212464
		public static string GetEntryForNetId(int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			if (netId < 0 || netId >= ModelIdSerializationCache._netIdToEntryNameMap.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("ModelId entry ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(netId);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range! We have ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToEntryNameMap.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" entries");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ModelIdSerializationCache._netIdToEntryNameMap[netId];
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x002142F4 File Offset: 0x002124F4
		public static int GetNetIdForEpochId(string epochId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			int num;
			if (!ModelIdSerializationCache._epochNameToNetIdMap.TryGetValue(epochId, out num))
			{
				throw new ArgumentException("Epoch ID " + epochId + " could not be mapped to any net ID!");
			}
			return num;
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0021433C File Offset: 0x0021253C
		public static string GetEpochIdForNetId(int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			if (netId < 0 || netId >= ModelIdSerializationCache._netIdToEpochNameMap.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Epoch ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(netId);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range! We have ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToEpochNameMap.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" entries");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ModelIdSerializationCache._netIdToEpochNameMap[netId];
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x002143CC File Offset: 0x002125CC
		public static int GetNetIdForPropertyName(string propertyName)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			int num;
			if (!ModelIdSerializationCache._propertyNameToNetIdMap.TryGetValue(propertyName, out num))
			{
				throw new ArgumentException("SavedProperty name " + propertyName + " could not be mapped to any net ID!");
			}
			return num;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00214414 File Offset: 0x00212614
		public static string GetPropertyNameForNetId(int netId)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			if (netId < 0 || netId >= ModelIdSerializationCache._netIdToPropertyNameMap.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
				defaultInterpolatedStringHandler.AppendLiteral("SavedProperty net ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(netId);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range! We have ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ModelIdSerializationCache._netIdToPropertyNameMap.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" property names");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return ModelIdSerializationCache._netIdToPropertyNameMap[netId];
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x002144A4 File Offset: 0x002126A4
		[return: Nullable(new byte[] { 2, 1 })]
		public static List<PropertyInfo> GetJsonPropertiesForType(Type t)
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			List<PropertyInfo> list;
			if (ModelIdSerializationCache._savedPropertyCache.TryGetValue(t, out list))
			{
				return list;
			}
			return null;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x002144D5 File Offset: 0x002126D5
		public static void CacheSavedPropertiesForTypeDebug([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type)
		{
			ModelIdSerializationCache.CachePropertiesForType(type, null, null);
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x002144E0 File Offset: 0x002126E0
		public static string Dump()
		{
			if (!ModelIdSerializationCache._initialized)
			{
				throw new InvalidOperationException("ModelIdSerializationCache used before it was initialized!");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("CATEGORIES");
			StringBuilder stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			for (int i = 0; i < ModelIdSerializationCache._netIdToCategoryNameMap.Count; i++)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(i.ToString().PadRight(3));
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted(ModelIdSerializationCache._netIdToCategoryNameMap[i]);
				stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("ENTRIES");
			for (int j = 0; j < ModelIdSerializationCache._netIdToEntryNameMap.Count; j++)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(j.ToString().PadRight(3));
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted(ModelIdSerializationCache._netIdToEntryNameMap[j]);
				stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("PROPERTIES");
			for (int k = 0; k < ModelIdSerializationCache._netIdToPropertyNameMap.Count; k++)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(k.ToString().PadRight(3));
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted(ModelIdSerializationCache._netIdToPropertyNameMap[k]);
				stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("EPOCHS");
			for (int l = 0; l < ModelIdSerializationCache._netIdToEpochNameMap.Count; l++)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder6 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(l.ToString().PadRight(3));
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted(ModelIdSerializationCache._netIdToEpochNameMap[l]);
				stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine();
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder7 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Hash: ");
			appendInterpolatedStringHandler.AppendFormatted<uint>(ModelIdSerializationCache.Hash);
			stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
			return stringBuilder.ToString();
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00214704 File Offset: 0x00212904
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static ModelIdSerializationCache()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			string text = ModelId.none.Category;
			dictionary[text] = 0;
			ModelIdSerializationCache._categoryNameToNetIdMap = dictionary;
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = ModelId.none.Category;
			ModelIdSerializationCache._netIdToCategoryNameMap = list;
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			text = ModelId.none.Entry;
			dictionary2[text] = 0;
			ModelIdSerializationCache._entryNameToNetIdMap = dictionary2;
			num2 = 1;
			List<string> list2 = new List<string>(num2);
			CollectionsMarshal.SetCount<string>(list2, num2);
			span = CollectionsMarshal.AsSpan<string>(list2);
			num = 0;
			*span[num] = ModelId.none.Entry;
			ModelIdSerializationCache._netIdToEntryNameMap = list2;
			ModelIdSerializationCache._epochNameToNetIdMap = new Dictionary<string, int>();
			ModelIdSerializationCache._netIdToEpochNameMap = new List<string>();
			ModelIdSerializationCache._propertyNameToNetIdMap = new Dictionary<string, int>();
			ModelIdSerializationCache._netIdToPropertyNameMap = new List<string>();
		}

		// Token: 0x04001AC5 RID: 6853
		private static readonly Dictionary<Type, List<PropertyInfo>> _savedPropertyCache = new Dictionary<Type, List<PropertyInfo>>();

		/// For serializing over the network, to save space, we map the string names statically to integers...
		// Token: 0x04001AC6 RID: 6854
		private static readonly Dictionary<string, int> _categoryNameToNetIdMap;

		/// ...and vice-versa.
		// Token: 0x04001AC7 RID: 6855
		private static readonly List<string> _netIdToCategoryNameMap;

		// Token: 0x04001AC8 RID: 6856
		private static readonly Dictionary<string, int> _entryNameToNetIdMap;

		// Token: 0x04001AC9 RID: 6857
		private static readonly List<string> _netIdToEntryNameMap;

		// Token: 0x04001ACA RID: 6858
		private static readonly Dictionary<string, int> _epochNameToNetIdMap;

		// Token: 0x04001ACB RID: 6859
		private static readonly List<string> _netIdToEpochNameMap;

		// Token: 0x04001ACC RID: 6860
		private static readonly Dictionary<string, int> _propertyNameToNetIdMap;

		// Token: 0x04001ACD RID: 6861
		private static readonly List<string> _netIdToPropertyNameMap;

		// Token: 0x04001AD3 RID: 6867
		private static bool _initialized;

		// Token: 0x020019A6 RID: 6566
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04006542 RID: 25922
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<Type, ModelId> <0>__GetId;

			// Token: 0x04006543 RID: 25923
			[Nullable(new byte[] { 0, 1, 1 })]
			public static Func<Type, string> <1>__GetId;

			// Token: 0x04006544 RID: 25924
			[Nullable(new byte[] { 0, 1 })]
			public static Comparison<PropertyInfo> <2>__CompareProperties;
		}
	}
}
