using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x0200044F RID: 1103
	[NullableContext(1)]
	[Nullable(0)]
	public static class PacketReaderExtensions
	{
		// Token: 0x06004522 RID: 17698 RVA: 0x00214EF7 File Offset: 0x002130F7
		public static T ReadModel<[Nullable(0)] T>(this PacketReader reader) where T : AbstractModel
		{
			if (typeof(T) == typeof(AbstractModel))
			{
				throw new ArgumentException("T must not be AbstractModel!");
			}
			return ModelDb.GetById<T>(reader.ReadModelIdAssumingType<T>());
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00214F2C File Offset: 0x0021312C
		public static ModelId ReadModelIdAssumingType<[Nullable(0)] T>(this PacketReader reader) where T : AbstractModel
		{
			if (typeof(T) == typeof(AbstractModel))
			{
				throw new ArgumentException("T must not be AbstractModel!");
			}
			string entryForNetId = ModelIdSerializationCache.GetEntryForNetId(reader.ReadInt(ModelIdSerializationCache.EntryIdBitSize));
			if (entryForNetId == ModelId.none.Entry)
			{
				return ModelId.none;
			}
			return new ModelId(ModelId.SlugifyCategory(ModelDb.GetCategoryType(typeof(T)).Name), entryForNetId);
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00214FA8 File Offset: 0x002131A8
		public static ModelId ReadFullModelId(this PacketReader reader)
		{
			string categoryForNetId = ModelIdSerializationCache.GetCategoryForNetId(reader.ReadInt(ModelIdSerializationCache.CategoryIdBitSize));
			string entryForNetId = ModelIdSerializationCache.GetEntryForNetId(reader.ReadInt(ModelIdSerializationCache.EntryIdBitSize));
			if (entryForNetId == ModelId.none.Entry || categoryForNetId == ModelId.none.Category)
			{
				return ModelId.none;
			}
			return new ModelId(categoryForNetId, entryForNetId);
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00215008 File Offset: 0x00213208
		public static EpochModel ReadEpoch(this PacketReader reader)
		{
			return EpochModel.Get(reader.ReadEpochId());
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00215015 File Offset: 0x00213215
		public static string ReadEpochId(this PacketReader reader)
		{
			return ModelIdSerializationCache.GetEpochIdForNetId(reader.ReadInt(ModelIdSerializationCache.EpochIdBitSize));
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00215028 File Offset: 0x00213228
		public static List<ModelId> ReadFullModelIdList(this PacketReader reader)
		{
			List<ModelId> list = new List<ModelId>();
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.ReadFullModelId());
			}
			return list;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00215060 File Offset: 0x00213260
		public static List<T> ReadModelList<[Nullable(0)] T>(this PacketReader reader) where T : AbstractModel
		{
			List<T> list = new List<T>();
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.ReadModel<T>());
			}
			return list;
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x00215098 File Offset: 0x00213298
		public static List<ModelId> ReadModelIdListAssumingType<[Nullable(0)] T>(this PacketReader reader) where T : AbstractModel
		{
			List<ModelId> list = new List<ModelId>();
			int num = reader.ReadInt(32);
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.ReadModelIdAssumingType<T>());
			}
			return list;
		}
	}
}
