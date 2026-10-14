using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D2C RID: 3372
	[NullableContext(1)]
	[Nullable(0)]
	public static class Cmd
	{
		/// <summary>
		/// Waits for a specified amount of time.
		/// This should be used over Task.Delay for all gameplay-related waits because it uses a Godot timer.
		/// That means, unlike Task.Delay, this will respect timescale (e.g. for hitpause) and lag.
		/// </summary>
		/// <param name="seconds">The amount of seconds to wait.</param>
		/// <param name="ignoreCombatEnd">If combat is about to end and this is false, the wait will be skipped.</param>
		// Token: 0x0600881A RID: 34842 RVA: 0x002DD670 File Offset: 0x002DB870
		public static Task Wait(float seconds, bool ignoreCombatEnd = false)
		{
			return Cmd.Wait(seconds, default(CancellationToken), ignoreCombatEnd);
		}

		/// <summary>
		/// Waits for a specified amount of time.
		/// This should be used over Task.Delay for all gameplay-related waits because it uses a Godot timer.
		/// That means, unlike Task.Delay, this will respect timescale (e.g. for hitpause) and lag.
		/// </summary>
		/// <param name="seconds">The amount of seconds to wait.</param>
		/// <param name="cancelToken">If the cancel token requests cancellation, the wait will immediately end.</param>
		/// <param name="ignoreCombatEnd">If combat is about to end and this is false, the wait will be skipped.</param>
		// Token: 0x0600881B RID: 34843 RVA: 0x002DD690 File Offset: 0x002DB890
		public static async Task Wait(float seconds, CancellationToken cancelToken, bool ignoreCombatEnd = false)
		{
			if (!NonInteractiveMode.IsActive)
			{
				if (seconds > 0f)
				{
					if (NGame.Instance != null)
					{
						if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
						{
							return;
						}
						if (!ignoreCombatEnd && CombatManager.Instance.IsEnding)
						{
							return;
						}
					}
					SceneTree sceneTree = (SceneTree)Engine.GetMainLoop();
					SceneTreeTimer sceneTreeTimer = sceneTree.CreateTimer((double)seconds, true, false, false);
					await Cmd.WaitInternal(sceneTreeTimer, cancelToken);
				}
			}
		}

		// Token: 0x0600881C RID: 34844 RVA: 0x002DD6E4 File Offset: 0x002DB8E4
		private static async Task WaitInternal(SceneTreeTimer timer, CancellationToken cancellationToken)
		{
			await timer.ToSignal(timer, SceneTreeTimer.SignalName.Timeout).ToTask().WaitAsync(cancellationToken);
		}

		/// <summary>
		/// Wait cmd which can specify the exact timings for each FastModeType.
		/// </summary>
		// Token: 0x0600881D RID: 34845 RVA: 0x002DD730 File Offset: 0x002DB930
		public static async Task CustomScaledWait(float fastSeconds, float standardSeconds, bool ignoreCombatEnd = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!NonInteractiveMode.IsActive)
			{
				if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
				{
					if (ignoreCombatEnd || !CombatManager.Instance.IsEnding)
					{
						switch (SaveManager.Instance.PrefsSave.FastMode)
						{
						case FastModeType.Normal:
							await Cmd.Wait(standardSeconds, cancellationToken, ignoreCombatEnd);
							goto IL_015A;
						case FastModeType.Fast:
							await Cmd.Wait(fastSeconds, cancellationToken, ignoreCombatEnd);
							goto IL_015A;
						case FastModeType.Instant:
							goto IL_015A;
						}
						throw new ArgumentOutOfRangeException();
						IL_015A:;
					}
				}
			}
		}
	}
}
