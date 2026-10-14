using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	// Token: 0x020003A1 RID: 929
	[NullableContext(1)]
	[Nullable(0)]
	public static class NodeUtil
	{
		/// <summary>
		/// Awaits the next process frame and returns the delta time.
		/// Throws <see cref="T:System.OperationCanceledException" /> if the token is cancelled or the node has exited the tree,
		/// which <see cref="M:MegaCrit.Sts2.Core.Helpers.TaskHelper.RunSafely(System.Threading.Tasks.Task)" /> silently swallows.
		/// </summary>
		// Token: 0x060037E8 RID: 14312 RVA: 0x001A631C File Offset: 0x001A451C
		public static async Task<float> AwaitProcessFrame(this Node node, CancellationToken ct = default(CancellationToken))
		{
			ct.ThrowIfCancellationRequested();
			SceneTree treeOrNull = node.GetTreeOrNull();
			if (treeOrNull == null)
			{
				throw new TaskCanceledException();
			}
			await treeOrNull.ToSignal(treeOrNull, SceneTree.SignalName.ProcessFrame);
			ct.ThrowIfCancellationRequested();
			if (!node.IsValid() || !node.IsInsideTree())
			{
				throw new TaskCanceledException();
			}
			return (float)node.GetProcessDeltaTime();
		}

		/// <summary>
		/// Awaits the next process frame.
		/// Unlike <see cref="M:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NodeUtil.AwaitProcessFrame(Godot.Node,System.Threading.CancellationToken)" />, this merely returns when the cancellation token is cancelled instead of
		/// throwing. Use this in scenarios where cancellation is frequent and expected, as throwing OperationCancelledException
		/// can be very slow when running in debug mode.
		/// This method itself cancels the cancellation token if the node is removed from the scene tree or becomes invalid.
		/// </summary>
		// Token: 0x060037E9 RID: 14313 RVA: 0x001A6368 File Offset: 0x001A4568
		public static async Task AwaitProcessFrameNonThrowing(this Node node, CancellationTokenSource cts)
		{
			if (!cts.IsCancellationRequested)
			{
				SceneTree treeOrNull = node.GetTreeOrNull();
				if (treeOrNull == null)
				{
					await cts.CancelAsync();
				}
				else
				{
					await treeOrNull.ToSignal(treeOrNull, SceneTree.SignalName.ProcessFrame);
					if (!cts.IsCancellationRequested)
					{
						if (!node.IsValid() || !node.IsInsideTree())
						{
							await cts.CancelAsync();
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the node's <see cref="T:Godot.SceneTree" />, or <c>null</c> if the node is not inside a tree.
		/// Unlike <see cref="M:Godot.Node.GetTree" />, this does not trigger a native error print when the tree is null.
		/// Always prefer this over <see cref="M:Godot.Node.GetTree" /> when the node may not be in the tree.
		/// </summary>
		// Token: 0x060037EA RID: 14314 RVA: 0x001A63B3 File Offset: 0x001A45B3
		[return: Nullable(2)]
		public static SceneTree GetTreeOrNull(this Node node)
		{
			if (!node.IsInsideTree())
			{
				return null;
			}
			return node.GetTree();
		}

		/// <summary>
		/// Returns true if candidate is located within parent's subtree.
		/// </summary>
		// Token: 0x060037EB RID: 14315 RVA: 0x001A63C8 File Offset: 0x001A45C8
		public static bool IsDescendant([Nullable(2)] Node parent, Node candidate)
		{
			if (parent == null)
			{
				return false;
			}
			for (Node node = candidate.GetParent(); node != null; node = node.GetParent())
			{
				if (node == parent)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if the node is not null, is valid, and is not deleting.
		/// </summary>
		// Token: 0x060037EC RID: 14316 RVA: 0x001A63F4 File Offset: 0x001A45F4
		[NullableContext(2)]
		public static bool IsValid(this Node node)
		{
			return node != null && GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion();
		}

		/// <summary>
		/// Checks to see if a controller is detected before we grab the focus of control.
		/// </summary>
		// Token: 0x060037ED RID: 14317 RVA: 0x001A640C File Offset: 0x001A460C
		public static void TryGrabFocus(this Control control)
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				if (control.IsVisibleInTree())
				{
					control.GrabFocus();
					return;
				}
				Callable.From(delegate
				{
					if (control.IsValid() && control.IsInsideTree())
					{
						control.GrabFocus();
					}
				}).CallDeferred(Array.Empty<Variant>());
			}
		}

		/// <summary>
		/// Obtains the nearest ancestor of the given type.
		/// </summary>
		// Token: 0x060037EE RID: 14318 RVA: 0x001A646C File Offset: 0x001A466C
		[NullableContext(2)]
		public static T GetAncestorOfType<T>([Nullable(1)] this Node node)
		{
			for (Node node2 = node.GetParent(); node2 != null; node2 = node2.GetParent())
			{
				if (node2 is T)
				{
					return node2 as T;
				}
			}
			return default(T);
		}

		/// <summary>
		/// Awaits an arbitrary signal on a <see cref="T:Godot.GodotObject" />, automatically cancelling when the owning node
		/// exits the tree.
		/// </summary>
		/// <remarks>
		/// <c>ToSignal</c> connects via <c>CONNECT_ONE_SHOT</c>, but one-shot cleanup only runs during signal
		/// emission. If the source object is freed without emitting the signal, the <c>SignalAwaiterCallable</c>
		/// and its GC handle linger until the source is destroyed by the garbage collector. This method avoids that
		/// by explicitly disconnecting on both the signal-fired and tree-exiting paths.
		/// </remarks>
		// Token: 0x060037EF RID: 14319 RVA: 0x001A64AC File Offset: 0x001A46AC
		public static Task AwaitSignal(this GodotObject source, StringName signal, Node owner)
		{
			NodeUtil.<>c__DisplayClass7_0 CS$<>8__locals1 = new NodeUtil.<>c__DisplayClass7_0();
			CS$<>8__locals1.source = source;
			CS$<>8__locals1.signal = signal;
			CS$<>8__locals1.owner = owner;
			if (!GodotObject.IsInstanceValid(CS$<>8__locals1.source))
			{
				return Task.CompletedTask;
			}
			CS$<>8__locals1.tcs = new TaskCompletionSource();
			CS$<>8__locals1.resolved = false;
			CS$<>8__locals1.callable = default(Callable);
			CS$<>8__locals1.callable = Callable.From(new Action(CS$<>8__locals1.<AwaitSignal>g__OnSignal|0));
			CS$<>8__locals1.source.Connect(CS$<>8__locals1.signal, CS$<>8__locals1.callable, 0U);
			CS$<>8__locals1.owner.TreeExiting += CS$<>8__locals1.<AwaitSignal>g__OnExiting|1;
			return CS$<>8__locals1.tcs.Task;
		}

		/// <summary>
		/// Awaits an arbitrary signal on a <see cref="T:Godot.GodotObject" />, automatically cancelling when the owning node
		/// exits the tree.
		/// </summary>
		/// <remarks>
		/// <c>ToSignal</c> connects via <c>CONNECT_ONE_SHOT</c>, but one-shot cleanup only runs during signal
		/// emission. If the source object is freed without emitting the signal, the <c>SignalAwaiterCallable</c>
		/// and its GC handle linger until the source is destroyed by the garbage collector. This method avoids that
		/// by explicitly disconnecting on both the signal-fired and tree-exiting paths.
		/// </remarks>
		// Token: 0x060037F0 RID: 14320 RVA: 0x001A6558 File Offset: 0x001A4758
		[return: Nullable(new byte[] { 1, 2 })]
		public static Task<T> AwaitSignal<[MustBeVariant] T>(this GodotObject source, StringName signal, Node owner) where T : class
		{
			NodeUtil.<>c__DisplayClass8_0<T> CS$<>8__locals1 = new NodeUtil.<>c__DisplayClass8_0<T>();
			CS$<>8__locals1.source = source;
			CS$<>8__locals1.signal = signal;
			CS$<>8__locals1.owner = owner;
			if (!GodotObject.IsInstanceValid(CS$<>8__locals1.source))
			{
				return Task.FromResult<T>(default(T));
			}
			CS$<>8__locals1.tcs = new TaskCompletionSource<T>();
			CS$<>8__locals1.resolved = false;
			CS$<>8__locals1.callable = default(Callable);
			CS$<>8__locals1.callable = Callable.From<T>(new Action<T>(CS$<>8__locals1.<AwaitSignal>g__OnSignal|0));
			CS$<>8__locals1.source.Connect(CS$<>8__locals1.signal, CS$<>8__locals1.callable, 0U);
			CS$<>8__locals1.owner.TreeExiting += CS$<>8__locals1.<AwaitSignal>g__OnExiting|1;
			return CS$<>8__locals1.tcs.Task;
		}

		/// <summary>
		/// Obtains all children of a certain type, searching recursively.
		/// </summary>
		// Token: 0x060037F1 RID: 14321 RVA: 0x001A660C File Offset: 0x001A480C
		public static IEnumerable<T> GetChildrenRecursive<[Nullable(2)] T>(this Node node)
		{
			NodeUtil.<GetChildrenRecursive>d__9<T> <GetChildrenRecursive>d__ = new NodeUtil.<GetChildrenRecursive>d__9<T>(-2);
			<GetChildrenRecursive>d__.<>3__node = node;
			return <GetChildrenRecursive>d__;
		}
	}
}
