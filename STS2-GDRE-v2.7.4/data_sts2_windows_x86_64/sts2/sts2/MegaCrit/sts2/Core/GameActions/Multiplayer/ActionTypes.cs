using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C36 RID: 3126
	public static class ActionTypes
	{
		// Token: 0x06008113 RID: 33043 RVA: 0x002BCF9E File Offset: 0x002BB19E
		public static void Initialize()
		{
			List<Type> list = new List<Type>();
			list.AddRange(INetActionSubtypes.All);
			list.AddRange(ReflectionHelper.GetSubtypesInMods<INetAction>());
			ActionTypes._cache = new NetTypeCache<INetAction>(list);
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x002BCFC5 File Offset: 0x002BB1C5
		public static int TypeToId<T>() where T : INetAction
		{
			NetTypeCache<INetAction> cache = ActionTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId<T>();
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x002BCFDB File Offset: 0x002BB1DB
		[NullableContext(1)]
		private static int TypeToId(Type type)
		{
			NetTypeCache<INetAction> cache = ActionTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId(type);
		}

		// Token: 0x06008116 RID: 33046 RVA: 0x002BCFF2 File Offset: 0x002BB1F2
		[NullableContext(1)]
		public static int ToId(this INetAction message)
		{
			NetTypeCache<INetAction> cache = ActionTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TypeToId(message.GetType());
		}

		// Token: 0x06008117 RID: 33047 RVA: 0x002BD00E File Offset: 0x002BB20E
		[NullableContext(2)]
		public static bool TryGetActionType(int id, out Type type)
		{
			NetTypeCache<INetAction> cache = ActionTypes._cache;
			if (cache == null)
			{
				throw new InvalidOperationException();
			}
			return cache.TryGetTypeFromId(id, out type);
		}

		// Token: 0x04002B36 RID: 11062
		[Nullable(new byte[] { 2, 1 })]
		private static NetTypeCache<INetAction> _cache;
	}
}
