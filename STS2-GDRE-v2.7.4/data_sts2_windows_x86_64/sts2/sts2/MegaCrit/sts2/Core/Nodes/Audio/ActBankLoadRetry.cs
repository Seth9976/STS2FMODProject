using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using Sentry;

namespace MegaCrit.Sts2.Core.Nodes.Audio
{
	/// <summary>
	/// Retries an act-music bank load that fails intermittently. The FMOD GDExtension reads bank files
	/// through Godot's FileAccess on a background thread with no retry, collapsing any open or
	/// short-read failure into a hard loadBankFile error (SLAYTHESPIRE2-6GF). The failures look like
	/// transient external interference (antivirus scans, cloud sync, file indexers touching the .pck),
	/// the same class that affects save writes, but the exact FMOD_RESULT is not captured, so the cause
	/// is inferred rather than confirmed. When a load fails the act bank stays unloaded; the production
	/// errors show the global "Progress" parameter then cannot be set, which indicates it is served
	/// from the act bank, so combat music freezes on its last value ("music inertia"). This mirrors the
	/// retry already applied to save writes in <see cref="T:MegaCrit.Sts2.Core.Saves.FileWriteRetry" />.
	///
	/// The load is reported as a success/failure bool rather than via an exception, so this retries on
	/// a false result instead of a caught throw. The bank loader MUST fully unload the failed loader
	/// between attempts (the proxy does this) so a retry issues a fresh loadBankFile rather than reusing
	/// the failed load.
	///
	/// Each non-healthy outcome is reported to Sentry (a recovery that needed more than one attempt, or
	/// an exhausted failure) so we can measure in production whether the retry window is sized right:
	/// the recovered-vs-failed ratio and the attempt distribution tell us if a wider window or a
	/// different approach (keeping banks resident) is warranted. First-try successes, expected to be the
	/// common case, are not reported.
	/// </summary>
	// Token: 0x02000421 RID: 1057
	public static class ActBankLoadRetry
	{
		/// <summary>
		/// Runs <paramref name="loadAttempt" />, retrying while it reports failure (false). Returns true
		/// once a load succeeds, or false if every attempt fails. <paramref name="sleep" /> and
		/// <paramref name="report" /> are injectable for tests; production passes Thread.Sleep and the
		/// Sentry reporter.
		/// </summary>
		// Token: 0x06004387 RID: 17287 RVA: 0x0020C7D4 File Offset: 0x0020A9D4
		[NullableContext(1)]
		public static bool Run(string bankPath, Func<bool> loadAttempt, [Nullable(2)] Action<int> sleep = null, [Nullable(new byte[] { 2, 1 })] Action<string, ActBankLoadRetry.LoadOutcome> report = null)
		{
			if (sleep == null)
			{
				Action<int> action;
				if ((action = ActBankLoadRetry.<>O.<0>__Sleep) == null)
				{
					action = (ActBankLoadRetry.<>O.<0>__Sleep = new Action<int>(Thread.Sleep));
				}
				sleep = action;
			}
			if (report == null)
			{
				Action<string, ActBankLoadRetry.LoadOutcome> action2;
				if ((action2 = ActBankLoadRetry.<>O.<1>__ReportToSentry) == null)
				{
					action2 = (ActBankLoadRetry.<>O.<1>__ReportToSentry = new Action<string, ActBankLoadRetry.LoadOutcome>(ActBankLoadRetry.ReportToSentry));
				}
				report = action2;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			for (int i = 1; i <= 3; i++)
			{
				if (loadAttempt())
				{
					report(bankPath, new ActBankLoadRetry.LoadOutcome(true, i));
					return true;
				}
				if (i < 3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Act music bank load failed (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(3);
					defaultInterpolatedStringHandler.AppendLiteral("), retrying. path=");
					defaultInterpolatedStringHandler.AppendFormatted(bankPath);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					sleep(50);
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Act music bank failed to load after ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(3);
			defaultInterpolatedStringHandler.AppendLiteral(" attempts; music will not follow combat state. path=");
			defaultInterpolatedStringHandler.AppendFormatted(bankPath);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			report(bankPath, new ActBankLoadRetry.LoadOutcome(false, 3));
			return false;
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0020C8F4 File Offset: 0x0020AAF4
		[NullableContext(1)]
		private static void ReportToSentry(string bankPath, ActBankLoadRetry.LoadOutcome outcome)
		{
			if (outcome.Loaded && outcome.Attempts == 1)
			{
				return;
			}
			string result = (outcome.Loaded ? "recovered" : "failed");
			SentryLevel sentryLevel = (outcome.Loaded ? 1 : 2);
			SentryService.CaptureMessage("Act music bank load needed retries or failed", sentryLevel, delegate(Scope scope)
			{
				scope.SetTag("act_bank_load", result);
				scope.SetTag("act_bank_load_attempts", outcome.Attempts.ToString());
				scope.SetTag("act_bank_path", bankPath);
				EventLikeExtensions.SetFingerprint(scope, new string[] { "act-bank-load", result });
			});
		}

		// Token: 0x04001A34 RID: 6708
		public const int maxAttempts = 3;

		// Token: 0x04001A35 RID: 6709
		public const int retryDelayMs = 50;

		/// <summary>The result of a load: whether it ultimately loaded, and how many attempts it took.</summary>
		// Token: 0x02001975 RID: 6517
		public readonly struct LoadOutcome : IEquatable<ActBankLoadRetry.LoadOutcome>
		{
			/// <summary>The result of a load: whether it ultimately loaded, and how many attempts it took.</summary>
			// Token: 0x0600AAD0 RID: 43728 RVA: 0x0035E202 File Offset: 0x0035C402
			public LoadOutcome(bool Loaded, int Attempts)
			{
				this.Loaded = Loaded;
				this.Attempts = Attempts;
			}

			// Token: 0x170026BC RID: 9916
			// (get) Token: 0x0600AAD1 RID: 43729 RVA: 0x0035E212 File Offset: 0x0035C412
			// (set) Token: 0x0600AAD2 RID: 43730 RVA: 0x0035E21A File Offset: 0x0035C41A
			public bool Loaded { get; set; }

			// Token: 0x170026BD RID: 9917
			// (get) Token: 0x0600AAD3 RID: 43731 RVA: 0x0035E223 File Offset: 0x0035C423
			// (set) Token: 0x0600AAD4 RID: 43732 RVA: 0x0035E22B File Offset: 0x0035C42B
			public int Attempts { get; set; }

			// Token: 0x0600AAD5 RID: 43733 RVA: 0x0035E234 File Offset: 0x0035C434
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LoadOutcome");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600AAD6 RID: 43734 RVA: 0x0035E280 File Offset: 0x0035C480
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Loaded = ");
				builder.Append(this.Loaded.ToString());
				builder.Append(", Attempts = ");
				builder.Append(this.Attempts.ToString());
				return true;
			}

			// Token: 0x0600AAD7 RID: 43735 RVA: 0x0035E2DC File Offset: 0x0035C4DC
			[CompilerGenerated]
			public static bool operator !=(ActBankLoadRetry.LoadOutcome left, ActBankLoadRetry.LoadOutcome right)
			{
				return !(left == right);
			}

			// Token: 0x0600AAD8 RID: 43736 RVA: 0x0035E2E8 File Offset: 0x0035C4E8
			[CompilerGenerated]
			public static bool operator ==(ActBankLoadRetry.LoadOutcome left, ActBankLoadRetry.LoadOutcome right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600AAD9 RID: 43737 RVA: 0x0035E2F2 File Offset: 0x0035C4F2
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<bool>.Default.GetHashCode(this.<Loaded>k__BackingField) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<Attempts>k__BackingField);
			}

			// Token: 0x0600AADA RID: 43738 RVA: 0x0035E31B File Offset: 0x0035C51B
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ActBankLoadRetry.LoadOutcome && this.Equals((ActBankLoadRetry.LoadOutcome)obj);
			}

			// Token: 0x0600AADB RID: 43739 RVA: 0x0035E333 File Offset: 0x0035C533
			[CompilerGenerated]
			public bool Equals(ActBankLoadRetry.LoadOutcome other)
			{
				return EqualityComparer<bool>.Default.Equals(this.<Loaded>k__BackingField, other.<Loaded>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<Attempts>k__BackingField, other.<Attempts>k__BackingField);
			}

			// Token: 0x0600AADC RID: 43740 RVA: 0x0035E365 File Offset: 0x0035C565
			[CompilerGenerated]
			public void Deconstruct(out bool Loaded, out int Attempts)
			{
				Loaded = this.Loaded;
				Attempts = this.Attempts;
			}
		}

		// Token: 0x02001976 RID: 6518
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400649D RID: 25757
			public static Action<int> <0>__Sleep;

			// Token: 0x0400649E RID: 25758
			[Nullable(new byte[] { 0, 1 })]
			public static Action<string, ActBankLoadRetry.LoadOutcome> <1>__ReportToSentry;
		}
	}
}
