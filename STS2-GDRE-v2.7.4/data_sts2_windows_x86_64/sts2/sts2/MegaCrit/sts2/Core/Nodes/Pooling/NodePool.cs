using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Nodes.Pooling
{
	// Token: 0x02000385 RID: 901
	[NullableContext(1)]
	[Nullable(0)]
	public class NodePool
	{
		// Token: 0x06003596 RID: 13718 RVA: 0x0018FBF8 File Offset: 0x0018DDF8
		public static NodePool<T> Init<[Nullable(0)] T>(string scenePath, int prewarmCount) where T : Node, IPoolable
		{
			Type typeFromHandle = typeof(T);
			INodePool nodePool;
			if (NodePool._pools.TryGetValue(typeFromHandle, out nodePool))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to init NodePool for type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(" but it's already initialized!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NodePool<T> nodePool2 = new NodePool<T>(scenePath, prewarmCount);
			NodePool._pools[typeFromHandle] = nodePool2;
			return nodePool2;
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x0018FC74 File Offset: 0x0018DE74
		public static IPoolable Get(Type type)
		{
			INodePool nodePool;
			if (!NodePool._pools.TryGetValue(type, out nodePool))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get pool for type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" before it was initialized!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return nodePool.Get();
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x0018FCD0 File Offset: 0x0018DED0
		public static void Free(IPoolable poolable)
		{
			Type type = poolable.GetType();
			INodePool nodePool;
			if (!NodePool._pools.TryGetValue(type, out nodePool))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get pool for type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" before it was initialized!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			nodePool.Free(poolable);
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x0018FD31 File Offset: 0x0018DF31
		public static T Get<[Nullable(0)] T>() where T : Node, IPoolable
		{
			return (T)((object)NodePool.Get(typeof(T)));
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0018FD47 File Offset: 0x0018DF47
		public static void Free<[Nullable(0)] T>(T obj) where T : Node, IPoolable
		{
			NodePool.Free(obj);
		}

		// Token: 0x040014BD RID: 5309
		private static Dictionary<Type, INodePool> _pools = new Dictionary<Type, INodePool>();
	}
}
