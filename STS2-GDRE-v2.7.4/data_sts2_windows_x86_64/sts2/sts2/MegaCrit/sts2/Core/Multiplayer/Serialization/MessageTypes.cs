using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x0200044B RID: 1099
	public static class MessageTypes
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x002138B1 File Offset: 0x00211AB1
		public static int Count
		{
			get
			{
				NetTypeCache<INetMessage> cache = MessageTypes._cache;
				if (cache == null)
				{
					throw new InvalidOperationException();
				}
				return cache.Count;
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x002138C7 File Offset: 0x00211AC7
		public static void Initialize()
		{
			List<Type> list = new List<Type>();
			list.AddRange(INetMessageSubtypes.All);
			list.AddRange(ReflectionHelper.GetSubtypesInMods<INetMessage>());
			MessageTypes._cache = new NetTypeCache<INetMessage>(list);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x002138EE File Offset: 0x00211AEE
		public static int TypeToId<T>() where T : INetMessage
		{
			NetTypeCache<INetMessage> cache = MessageTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId<T>();
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00213904 File Offset: 0x00211B04
		[NullableContext(1)]
		private static int TypeToId(Type type)
		{
			NetTypeCache<INetMessage> cache = MessageTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId(type);
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x0021391B File Offset: 0x00211B1B
		[NullableContext(1)]
		public static int ToId(this INetMessage message)
		{
			NetTypeCache<INetMessage> cache = MessageTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId(message.GetType());
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00213937 File Offset: 0x00211B37
		[NullableContext(2)]
		public static bool TryGetMessageType(int id, out Type type)
		{
			NetTypeCache<INetMessage> cache = MessageTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TryGetTypeFromId(id, out type);
		}

		// Token: 0x04001AC4 RID: 6852
		[Nullable(new byte[] { 2, 1 })]
		private static NetTypeCache<INetMessage> _cache;
	}
}
