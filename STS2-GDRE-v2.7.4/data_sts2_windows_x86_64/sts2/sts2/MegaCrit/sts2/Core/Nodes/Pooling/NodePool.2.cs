using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Nodes.Pooling
{
	// Token: 0x02000387 RID: 903
	[NullableContext(1)]
	[Nullable(0)]
	public class NodePool<[Nullable(0)] T> : INodePool where T : Node, IPoolable
	{
		/// <summary>
		/// WARNING: ONLY USE THIS PROPERTY FOR DEBUGGING!
		/// All of this pool's free objects.
		/// </summary>
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x0018FD68 File Offset: 0x0018DF68
		public IReadOnlyList<T> DebugFreeObjects
		{
			get
			{
				return this._freeObjects;
			}
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0018FD70 File Offset: 0x0018DF70
		public NodePool(string scenePath, int prewarmCount = 0)
		{
			this._scenePath = scenePath;
			for (int i = 0; i < prewarmCount; i++)
			{
				this._freeObjects.Add(this.Instantiate());
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x0018FDBD File Offset: 0x0018DFBD
		IPoolable INodePool.Get()
		{
			return this.Get();
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x0018FDCA File Offset: 0x0018DFCA
		void INodePool.Free(IPoolable poolable)
		{
			this.Free((T)((object)poolable));
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0018FDD8 File Offset: 0x0018DFD8
		public T Get()
		{
			T t;
			if (this._freeObjects.Count > 0)
			{
				List<T> freeObjects = this._freeObjects;
				t = freeObjects[freeObjects.Count - 1];
				this._freeObjects.RemoveAt(this._freeObjects.Count - 1);
			}
			else
			{
				t = this.Instantiate();
			}
			this._usedObjects.Add(t);
			t.OnReturnedFromPool();
			return t;
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x0018FE44 File Offset: 0x0018E044
		public void Free(T obj)
		{
			if (this._usedObjects.Contains(obj))
			{
				this.DisconnectIncomingAndOutgoingSignals(obj);
				this._usedObjects.Remove(obj);
				this._freeObjects.Add(obj);
				obj.OnFreedToPool();
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._freeObjects.Contains(obj))
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to free object ");
				defaultInterpolatedStringHandler.AppendFormatted<T>(obj);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(obj.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(") back to pool ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(NodePool<T>));
				defaultInterpolatedStringHandler.AppendLiteral(" but it's already been freed!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to free object ");
			defaultInterpolatedStringHandler.AppendFormatted<T>(obj);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(obj.GetType());
			defaultInterpolatedStringHandler.AppendLiteral(") back to pool ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(NodePool<T>));
			defaultInterpolatedStringHandler.AppendLiteral(" but it's not part of the pool!");
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			obj.QueueFreeSafelyNoPool();
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x0018FF94 File Offset: 0x0018E194
		private T Instantiate()
		{
			T t = PreloadManager.Cache.GetScene(this._scenePath).Instantiate<T>(0L);
			t.OnInstantiated();
			return t;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0018FFC8 File Offset: 0x0018E1C8
		private void DisconnectIncomingAndOutgoingSignals(Node obj)
		{
			foreach (Dictionary dictionary in obj.GetSignalList())
			{
				StringName stringName = dictionary[NodePool<T>._nameStr].AsStringName();
				foreach (Dictionary dictionary2 in obj.GetSignalConnectionList(stringName))
				{
					Callable callable = dictionary2[NodePool<T>._callableStr].AsCallable();
					Signal signal = dictionary2[NodePool<T>._signalStr].AsSignal();
					this.DisconnectSignal(callable, signal);
				}
			}
			foreach (Dictionary dictionary3 in obj.GetIncomingConnections())
			{
				Callable callable2 = dictionary3[NodePool<T>._callableStr].AsCallable();
				Signal signal2 = dictionary3[NodePool<T>._signalStr].AsSignal();
				this.DisconnectSignal(callable2, signal2);
			}
			for (int i = 0; i < obj.GetChildCount(false); i++)
			{
				this.DisconnectIncomingAndOutgoingSignals(obj.GetChild(i, false));
			}
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x00190130 File Offset: 0x0018E330
		private void DisconnectSignal(Callable callable, Signal signal)
		{
			GodotObject target = callable.Target;
			if (target == null && callable.Method == null)
			{
				return;
			}
			if (target != null && !GodotObject.IsInstanceValid(target))
			{
				return;
			}
			StringName name = signal.Name;
			Node node = target as Node;
			if (node != null && !node.IsInsideTree())
			{
				return;
			}
			GodotObject owner = signal.Owner;
			if (!GodotObject.IsInstanceValid(owner))
			{
				return;
			}
			Node node2 = owner as Node;
			if (node != null && node.HasSignal(name) && node.IsConnected(name, callable))
			{
				node.Disconnect(name, callable);
				return;
			}
			if (node2 != null && node2.HasSignal(name) && node2.IsConnected(name, callable))
			{
				node2.Disconnect(name, callable);
			}
		}

		// Token: 0x040014BE RID: 5310
		private static Variant _nameStr = Variant.CreateFrom("name");

		// Token: 0x040014BF RID: 5311
		private static Variant _callableStr = Variant.CreateFrom("callable");

		// Token: 0x040014C0 RID: 5312
		private static Variant _signalStr = Variant.CreateFrom("signal");

		// Token: 0x040014C1 RID: 5313
		private string _scenePath;

		// Token: 0x040014C2 RID: 5314
		private readonly List<T> _freeObjects = new List<T>();

		// Token: 0x040014C3 RID: 5315
		private readonly HashSet<T> _usedObjects = new HashSet<T>();
	}
}
