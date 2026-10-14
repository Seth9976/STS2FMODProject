using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000451 RID: 1105
	[NullableContext(1)]
	[Nullable(0)]
	public static class PacketWriterExtensions
	{
		// Token: 0x06004546 RID: 17734 RVA: 0x002157A8 File Offset: 0x002139A8
		public static void WriteModel<[Nullable(0)] T>(this PacketWriter writer, T model) where T : AbstractModel
		{
			if (model.IsMutable)
			{
				throw new ArgumentException("Cannot serialize mutable models");
			}
			writer.WriteModelEntry(model.Id);
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x002157D4 File Offset: 0x002139D4
		public static void WriteModelEntry(this PacketWriter writer, ModelId id)
		{
			if (string.IsNullOrEmpty(id.Category) || string.IsNullOrEmpty(id.Entry))
			{
				throw new InvalidOperationException("Tried to serialize an empty ModelId!");
			}
			int netIdForEntry;
			if (!ModelIdSerializationCache.TryGetNetIdForEntry(id.Entry, out netIdForEntry))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown ModelId entry '");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
				defaultInterpolatedStringHandler.AppendLiteral("' during serialization, writing NONE");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				netIdForEntry = ModelIdSerializationCache.GetNetIdForEntry(ModelId.none.Entry);
			}
			writer.WriteInt(netIdForEntry, ModelIdSerializationCache.EntryIdBitSize);
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x00215868 File Offset: 0x00213A68
		public static void WriteEpoch<[Nullable(0)] T>(this PacketWriter writer) where T : EpochModel
		{
			writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(EpochModel.GetId<T>()), ModelIdSerializationCache.EpochIdBitSize);
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0021587F File Offset: 0x00213A7F
		public static void WriteEpoch(this PacketWriter writer, EpochModel epochModel)
		{
			writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(epochModel.Id), ModelIdSerializationCache.EpochIdBitSize);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00215897 File Offset: 0x00213A97
		public static void WriteEpochId(this PacketWriter writer, string epochId)
		{
			writer.WriteInt(ModelIdSerializationCache.GetNetIdForEpochId(epochId), ModelIdSerializationCache.EpochIdBitSize);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x002158AC File Offset: 0x00213AAC
		public static void WriteFullModelId(this PacketWriter writer, ModelId id)
		{
			int netIdForCategory;
			bool flag = ModelIdSerializationCache.TryGetNetIdForCategory(id.Category, out netIdForCategory);
			int netIdForEntry;
			bool flag2 = ModelIdSerializationCache.TryGetNetIdForEntry(id.Entry, out netIdForEntry);
			if (!flag || !flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown ModelId '");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(id);
				defaultInterpolatedStringHandler.AppendLiteral("' during serialization, writing NONE");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				netIdForCategory = ModelIdSerializationCache.GetNetIdForCategory(ModelId.none.Category);
				netIdForEntry = ModelIdSerializationCache.GetNetIdForEntry(ModelId.none.Entry);
			}
			writer.WriteInt(netIdForCategory, ModelIdSerializationCache.CategoryIdBitSize);
			writer.WriteInt(netIdForEntry, ModelIdSerializationCache.EntryIdBitSize);
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x0021594C File Offset: 0x00213B4C
		public static void WriteFullModelIdList(this PacketWriter writer, IReadOnlyCollection<ModelId> models)
		{
			writer.WriteInt(models.Count, 32);
			foreach (ModelId modelId in models)
			{
				writer.WriteFullModelId(modelId);
			}
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x002159A4 File Offset: 0x00213BA4
		public static void WriteModelList<[Nullable(0)] T>(this PacketWriter writer, IReadOnlyCollection<T> models) where T : AbstractModel
		{
			writer.WriteInt(models.Count, 32);
			foreach (T t in models)
			{
				writer.WriteModel(t);
			}
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x002159FC File Offset: 0x00213BFC
		public static void WriteModelEntriesInList(this PacketWriter writer, IReadOnlyCollection<ModelId> modelIds)
		{
			writer.WriteInt(modelIds.Count, 32);
			foreach (ModelId modelId in modelIds)
			{
				writer.WriteModelEntry(modelId);
			}
		}
	}
}
