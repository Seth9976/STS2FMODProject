using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Pooling;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C08 RID: 3080
	[NullableContext(1)]
	[Nullable(0)]
	public static class GodotTreeExtensions
	{
		// Token: 0x06007FA8 RID: 32680 RVA: 0x002B5318 File Offset: 0x002B3518
		public static void AddChildSafely(this Node parent, [Nullable(2)] Node child)
		{
			if (child == null)
			{
				return;
			}
			if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
			{
				parent.AddChild(child, false, 0L);
				return;
			}
			parent.CallDeferred(Node.MethodName.AddChild, new Variant[] { child });
		}

		// Token: 0x06007FA9 RID: 32681 RVA: 0x002B536C File Offset: 0x002B356C
		public static void AddSiblingSafely(this Node sibling, [Nullable(2)] Node child)
		{
			if (child == null)
			{
				return;
			}
			Node parent = sibling.GetParent();
			if (parent == null)
			{
				return;
			}
			if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
			{
				sibling.AddSibling(child, false);
				return;
			}
			sibling.CallDeferred(Node.MethodName.AddSibling, new Variant[] { child });
		}

		// Token: 0x06007FAA RID: 32682 RVA: 0x002B53C8 File Offset: 0x002B35C8
		public static void MoveChildSafely(this Node parent, [Nullable(2)] Node child, int index)
		{
			if (child == null)
			{
				return;
			}
			if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
			{
				parent.MoveChild(child, index);
				return;
			}
			parent.CallDeferred(Node.MethodName.MoveChild, new Variant[] { child, index });
		}

		// Token: 0x06007FAB RID: 32683 RVA: 0x002B5428 File Offset: 0x002B3628
		public static void MoveToFrontSafely(this CanvasItem node)
		{
			Node parent = node.GetParent();
			if (parent == null)
			{
				return;
			}
			if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
			{
				node.MoveToFront();
				return;
			}
			parent.CallDeferred(Node.MethodName.MoveChild, new Variant[] { node, -1 });
		}

		// Token: 0x06007FAC RID: 32684 RVA: 0x002B548C File Offset: 0x002B368C
		public static void RemoveChildSafely(this Node parent, [Nullable(2)] Node child)
		{
			if (child == null)
			{
				return;
			}
			if (NGame.IsMainThread() && (parent.IsNodeReady() || !parent.IsInsideTree()))
			{
				parent.RemoveChild(child);
				return;
			}
			parent.CallDeferred(Node.MethodName.RemoveChild, new Variant[] { child });
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x002B54DC File Offset: 0x002B36DC
		public static void QueueFreeSafely(this Node node)
		{
			if (!GodotObject.IsInstanceValid(node))
			{
				return;
			}
			IPoolable poolable = node as IPoolable;
			if (poolable != null)
			{
				Node parent = node.GetParent();
				if (parent != null)
				{
					parent.RemoveChildSafely(node);
				}
				Callable.From(delegate
				{
					NodePool.Free(poolable);
				}).CallDeferred(Array.Empty<Variant>());
				return;
			}
			node.QueueFreeSafelyNoPool();
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x002B5543 File Offset: 0x002B3743
		public static void QueueFreeSafelyNoPool(this Node node)
		{
			if (NGame.IsMainThread())
			{
				node.QueueFree();
				return;
			}
			node.CallDeferred(Node.MethodName.QueueFree, Array.Empty<Variant>());
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x002B5564 File Offset: 0x002B3764
		public static void FreeChildren(this Node node)
		{
			foreach (Node node2 in node.GetChildren(false))
			{
				node2.QueueFreeSafely();
			}
		}
	}
}
