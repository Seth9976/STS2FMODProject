using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Retries a file write that fails due to transient external interference on Windows
	/// (antivirus on-access scans, cloud sync locks, file indexers). Sentry data (PRG-7042) shows
	/// save writes intermittently fail with FileCantWrite in bursts across the whole save directory,
	/// then recover moments later. This mirrors the retry already applied to
	/// <see cref="M:MegaCrit.Sts2.Core.Saves.GodotFileIo.RenameFile(System.String,System.String)" /> in PRG-6241.
	///
	/// The write attempt MUST fully reopen the target file on each call. Godot's FileAccess reports
	/// write errors via the sticky C stdio ferror() flag (drivers/windows/file_access_windows.cpp),
	/// which is only cleared by reopening the handle, so retrying on the same handle could keep
	/// reporting a stale error even after a later write succeeds.
	///
	/// On the final attempt the failure exception propagates unchanged, preserving the original Sentry
	/// grouping and deferring error reporting until every retry is exhausted.
	/// </summary>
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public static class FileWriteRetry
	{
		/// <summary>
		/// Runs <paramref name="writeAttempt" />, retrying on transient write failures. The
		/// <paramref name="sleep" /> parameter is injectable for tests; production passes Thread.Sleep.
		/// </summary>
		// Token: 0x06000503 RID: 1283 RVA: 0x00017384 File Offset: 0x00015584
		public static void Run(string path, Action writeAttempt, [Nullable(2)] Action<int> sleep = null)
		{
			if (sleep == null)
			{
				Action<int> action;
				if ((action = FileWriteRetry.<>O.<0>__Sleep) == null)
				{
					action = (FileWriteRetry.<>O.<0>__Sleep = new Action<int>(Thread.Sleep));
				}
				sleep = action;
			}
			for (int i = 1; i <= 4; i++)
			{
				try
				{
					writeAttempt();
					break;
				}
				catch (Exception ex) when (FileWriteRetry.IsTransient(ex) && i < 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 4);
					defaultInterpolatedStringHandler.AppendLiteral("File write failed (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(4);
					defaultInterpolatedStringHandler.AppendLiteral("), retrying. path=");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(" error=");
					defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					sleep(50);
				}
			}
		}

		/// <summary>
		/// Async counterpart to <see cref="M:MegaCrit.Sts2.Core.Saves.FileWriteRetry.Run(System.String,System.Action,System.Action{System.Int32})" />. The <paramref name="delay" /> parameter is injectable
		/// for tests; production passes Task.Delay.
		/// </summary>
		// Token: 0x06000504 RID: 1284 RVA: 0x00017478 File Offset: 0x00015678
		public static async Task RunAsync(string path, Func<Task> writeAttempt, [Nullable(new byte[] { 2, 1 })] Func<int, Task> delay = null)
		{
			if (delay == null)
			{
				delay = (int ms) => Task.Delay(ms);
			}
			for (int attempt = 1; attempt <= 4; attempt++)
			{
				int num = 0;
				object obj;
				Exception ex;
				bool flag;
				object obj2;
				Exception ex2;
				try
				{
					await writeAttempt();
					break;
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					ex = obj as Exception;
					if (ex == null)
					{
						flag = false;
					}
					else
					{
						obj2 = ex;
						ex2 = (Exception)obj2;
						flag = (FileWriteRetry.IsTransient(ex2) && attempt < 4) > false;
					}
					endfilter(flag);
				})
				{
					num = 1;
				}
				if (num == 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 4);
					defaultInterpolatedStringHandler.AppendLiteral("File write failed (attempt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(attempt);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(4);
					defaultInterpolatedStringHandler.AppendLiteral("), retrying. path=");
					defaultInterpolatedStringHandler.AppendFormatted(path);
					defaultInterpolatedStringHandler.AppendLiteral(" error=");
					defaultInterpolatedStringHandler.AppendFormatted(ex2.Message);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					await delay(50);
				}
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000174CC File Offset: 0x000156CC
		private static bool IsTransient(Exception e)
		{
			return e is SaveException || e is IOException;
		}

		// Token: 0x040001A7 RID: 423
		public const int maxAttempts = 4;

		// Token: 0x040001A8 RID: 424
		public const int retryDelayMs = 50;

		// Token: 0x02000E22 RID: 3618
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400334D RID: 13133
			[Nullable(0)]
			public static Action<int> <0>__Sleep;
		}
	}
}
