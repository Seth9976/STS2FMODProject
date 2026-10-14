using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Daily
{
	/// <summary>
	/// A little helper for uploading the score at the end of a daily run.
	/// </summary>
	// Token: 0x02000D19 RID: 3353
	[NullableContext(1)]
	[Nullable(0)]
	public static class DailyRunUtility
	{
		/// <summary>
		/// Uploads a score to the daily leaderboard for the given time.
		/// </summary>
		// Token: 0x06008754 RID: 34644 RVA: 0x002D8690 File Offset: 0x002D6890
		public static async Task UploadScore(DateTimeOffset time, int score, List<SerializablePlayer> players)
		{
			List<ulong> playerIdsInRun = players.Select<SerializablePlayer, ulong>((SerializablePlayer p) => p.NetId).ToList<ulong>();
			if (playerIdsInRun != null && playerIdsInRun.Count == 1 && playerIdsInRun[0] == 1UL)
			{
				playerIdsInRun[0] = PlatformUtil.GetLocalPlayerId(LeaderboardManager.CurrentPlatform);
			}
			string leaderboardName = DailyRunUtility.GetLeaderboardName(time, players.Count);
			ILeaderboardHandle leaderboardHandle = await LeaderboardManager.GetLeaderboard(leaderboardName, default(CancellationToken));
			ILeaderboardHandle leaderboardHandle2 = leaderboardHandle;
			TaskAwaiter<bool> taskAwaiter = DailyRunUtility.ShouldUploadScore(leaderboardHandle2, playerIdsInRun, default(CancellationToken)).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player already uploaded score for daily ");
				defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset>(time);
				defaultInterpolatedStringHandler.AppendLiteral(", ignoring new score");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				leaderboardHandle2 = await LeaderboardManager.GetOrCreateLeaderboard(leaderboardName, default(CancellationToken));
				await LeaderboardManager.UploadLocalScore(leaderboardHandle2, score, playerIdsInRun);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Uploaded score of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(score);
				defaultInterpolatedStringHandler.AppendLiteral(" for daily ");
				defaultInterpolatedStringHandler.AppendFormatted<DateTimeOffset>(time);
				defaultInterpolatedStringHandler.AppendLiteral(" to leaderboard ");
				defaultInterpolatedStringHandler.AppendFormatted(leaderboardName);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		/// <summary>
		/// Figures out whether we should upload a score to the passed leaderboard.
		/// If any player in the run has already submitted a score, then this returns false.
		/// </summary>
		// Token: 0x06008755 RID: 34645 RVA: 0x002D86E4 File Offset: 0x002D68E4
		public static async Task<bool> ShouldUploadScore([Nullable(2)] ILeaderboardHandle handle, IReadOnlyList<ulong> playerIdsInRun, CancellationToken cancelToken = default(CancellationToken))
		{
			bool flag;
			if (handle == null)
			{
				flag = true;
			}
			else
			{
				List<LeaderboardEntry> list = await LeaderboardManager.QueryLeaderboardForUsers(handle, playerIdsInRun, cancelToken);
				List<LeaderboardEntry> list2 = list;
				flag = list2.Count <= 0;
			}
			return flag;
		}

		/// <param name="dateTime">Date to retrieve the leaderboard for. All fields other than year/month/day are ignored.</param>
		/// <param name="playerCount">Count of players to retrieve the leaderboard for.</param>
		/// <returns>The name of the leaderboard used for the passed date and player count.</returns>
		// Token: 0x06008756 RID: 34646 RVA: 0x002D8738 File Offset: 0x002D6938
		public static string GetLeaderboardName(DateTimeOffset dateTime, int playerCount)
		{
			PlatformBranch platformBranch = PlatformUtil.GetPlatformBranch();
			bool flag = platformBranch - PlatformBranch.PublicBeta <= 2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag || !NGame.IsReleaseGame())
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 4);
				defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Year);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Month, "D2");
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Day, "D2");
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerCount);
				defaultInterpolatedStringHandler.AppendLiteral("p_BETA");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 4);
			defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Year);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Month, "D2");
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(dateTime.Day, "D2");
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(playerCount);
			defaultInterpolatedStringHandler.AppendLiteral("p");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Returns the leaderboard day <paramref name="days" /> away from <paramref name="day" />, or null if that day
		/// falls outside the representable <see cref="T:System.DateTimeOffset" /> range (e.g. the day before
		/// <see cref="F:System.DateTimeOffset.MinValue" />). The daily leaderboard probes its neighbouring days and is paged one
		/// day at a time, so the boundary arithmetic must not throw <see cref="T:System.ArgumentOutOfRangeException" />.
		/// </summary>
		// Token: 0x06008757 RID: 34647 RVA: 0x002D8860 File Offset: 0x002D6A60
		public static DateTimeOffset? AddLeaderboardDays(DateTimeOffset day, int days)
		{
			DateTimeOffset? dateTimeOffset;
			try
			{
				dateTimeOffset = new DateTimeOffset?(day + TimeSpan.FromDays(days));
			}
			catch (ArgumentOutOfRangeException)
			{
				dateTimeOffset = null;
			}
			return dateTimeOffset;
		}
	}
}
