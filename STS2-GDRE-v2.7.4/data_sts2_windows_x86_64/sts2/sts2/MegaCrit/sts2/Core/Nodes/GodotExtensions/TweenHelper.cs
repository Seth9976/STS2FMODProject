using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	// Token: 0x020003A5 RID: 933
	[NullableContext(1)]
	[Nullable(0)]
	public static class TweenHelper
	{
		// Token: 0x06003842 RID: 14402 RVA: 0x001A919E File Offset: 0x001A739E
		public static void FastForwardToCompletion(this Tween t)
		{
			t.CustomStep(999999999.0);
		}

		/// <summary>
		/// Awaits the tween's <see cref="F:Godot.Tween.SignalName.Finished" /> signal, automatically cancelling when the
		/// owning node exits the tree. Unlike <c>ToSignal(tween, Tween.SignalName.Finished)</c>, this will not
		/// hang when the tween is killed (e.g. because the node exited the tree and bound tweens stop processing).
		/// </summary>
		/// <remarks>
		/// <c>Tween.Kill()</c> does not emit the <c>finished</c> signal (see <c>scene/animation/tween.cpp</c>).
		/// <c>ToSignal</c> connects via <c>CONNECT_ONE_SHOT</c>, but one-shot cleanup only runs during signal
		/// emission (<c>object.cpp</c>). Since the signal is never emitted, the <c>SignalAwaiterCallable</c>
		/// remains in the tween's signal map, holding a strong GC handle that pins the <c>SignalAwaiter</c> and
		/// the async state machine. The state machine in turn holds the tween reference, forming a reference cycle
		/// across the managed/native boundary that persists for the lifetime of the process.
		/// This method sidesteps that by cancelling via the node's <c>TreeExiting</c> signal, which fires before
		/// the node is freed.
		/// </remarks>
		/// <returns>
		/// <c>true</c> if the tween finished normally and the owner is still alive when control returns to the caller.
		/// <c>false</c> if the owner exited the tree before the tween completed, OR if the owner was freed between the
		/// tween's <c>Finished</c> signal firing and the caller's continuation resuming.
		/// Callers should bail out early when <c>false</c> is returned.
		/// </returns>
		// Token: 0x06003843 RID: 14403 RVA: 0x001A91B0 File Offset: 0x001A73B0
		public static async Task<bool> AwaitFinished(this Tween tween, Node owner)
		{
			TaskAwaiter<bool> taskAwaiter = TweenHelper.AwaitFinishedInternal(tween, owner).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			bool flag;
			if (!taskAwaiter.GetResult())
			{
				flag = false;
			}
			else
			{
				flag = GodotObject.IsInstanceValid(owner) && owner.IsInsideTree();
			}
			return flag;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x001A91FC File Offset: 0x001A73FC
		private static Task<bool> AwaitFinishedInternal(Tween tween, Node owner)
		{
			TweenHelper.<>c__DisplayClass2_0 CS$<>8__locals1 = new TweenHelper.<>c__DisplayClass2_0();
			CS$<>8__locals1.tween = tween;
			CS$<>8__locals1.owner = owner;
			if (!CS$<>8__locals1.tween.IsValid() || !CS$<>8__locals1.tween.IsRunning())
			{
				return Task.FromResult<bool>(true);
			}
			CS$<>8__locals1.tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			CS$<>8__locals1.resolved = false;
			CS$<>8__locals1.tween.Finished += CS$<>8__locals1.<AwaitFinishedInternal>g__OnFinished|0;
			CS$<>8__locals1.owner.TreeExiting += CS$<>8__locals1.<AwaitFinishedInternal>g__OnExiting|1;
			if (!CS$<>8__locals1.tween.IsValid() || !CS$<>8__locals1.tween.IsRunning())
			{
				CS$<>8__locals1.<AwaitFinishedInternal>g__OnFinished|0();
			}
			else if (!CS$<>8__locals1.owner.IsInsideTree())
			{
				CS$<>8__locals1.<AwaitFinishedInternal>g__OnExiting|1();
			}
			return CS$<>8__locals1.tcs.Task;
		}

		/// <summary>
		/// Awaits the tween's <see cref="F:Godot.Tween.SignalName.Finished" /> signal with cancellation support.
		/// Unlike <c>ToSignal(tween, Tween.SignalName.Finished)</c>, this will not hang when the tween is killed,
		/// provided the caller cancels the token.
		/// </summary>
		/// <remarks>
		/// <c>Tween.Kill()</c> does not emit the <c>finished</c> signal (see <c>scene/animation/tween.cpp</c>).
		/// <c>ToSignal</c> connects via <c>CONNECT_ONE_SHOT</c>, but one-shot cleanup only runs during signal
		/// emission (<c>object.cpp</c>). Since the signal is never emitted, the <c>SignalAwaiterCallable</c>
		/// remains in the tween's signal map, holding a strong GC handle that pins the <c>SignalAwaiter</c> and
		/// the async state machine. The state machine in turn holds the tween reference, forming a reference cycle
		/// across the managed/native boundary that persists for the lifetime of the process.
		/// This method sidesteps that by using a <see cref="T:System.Threading.Tasks.TaskCompletionSource" /> that can be cancelled externally.
		/// </remarks>
		// Token: 0x06003845 RID: 14405 RVA: 0x001A92C0 File Offset: 0x001A74C0
		public static Task AwaitFinished(this Tween tween, CancellationToken ct)
		{
			TweenHelper.<>c__DisplayClass3_0 CS$<>8__locals1 = new TweenHelper.<>c__DisplayClass3_0();
			CS$<>8__locals1.tween = tween;
			CS$<>8__locals1.ct = ct;
			CS$<>8__locals1.tcs = new TaskCompletionSource();
			CS$<>8__locals1.unsubscribed = 0;
			CS$<>8__locals1.ctr = default(CancellationTokenRegistration);
			CS$<>8__locals1.tween.Finished += CS$<>8__locals1.<AwaitFinished>g__OnFinished|1;
			if (CS$<>8__locals1.ct.CanBeCanceled)
			{
				CS$<>8__locals1.ctr = CS$<>8__locals1.ct.Register(delegate
				{
					if (Interlocked.Exchange(ref CS$<>8__locals1.unsubscribed, 1) == 0)
					{
						CS$<>8__locals1.tween.Finished -= base.<AwaitFinished>g__OnFinished|1;
					}
					CS$<>8__locals1.tcs.TrySetCanceled(CS$<>8__locals1.ct);
				});
			}
			return CS$<>8__locals1.tcs.Task;
		}
	}
}
