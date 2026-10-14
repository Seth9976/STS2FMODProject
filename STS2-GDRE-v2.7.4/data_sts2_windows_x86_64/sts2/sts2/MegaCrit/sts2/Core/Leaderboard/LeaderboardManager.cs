using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Platform.Steam;

namespace MegaCrit.Sts2.Core.Leaderboard
{
	/// <summary>
	/// Facade for platform-specific leaderboard operations. Currently only Steam is supported in production;
	/// NullLeaderboardStrategy is used for dev/offline. The current implementation uses a sentinel score hack for
	/// multiplayer dailies (see ScoreUtility.clientScore) which is Steam-specific and will need revisiting when
	/// integrating other platforms' leaderboard APIs.
	/// </summary>
	// Token: 0x02000BF5 RID: 3061
	[NullableContext(1)]
	[Nullable(0)]
	public static class LeaderboardManager
	{
		/// <summary>
		/// This should be called after all platform initialization is completed.
		/// </summary>
		// Token: 0x06007E88 RID: 32392 RVA: 0x002AE38B File Offset: 0x002AC58B
		public static void Initialize()
		{
			if (SteamInitializer.Initialized)
			{
				LeaderboardManager._strategy = new SteamLeaderboardStrategy();
				return;
			}
			LeaderboardManager._strategy = new NullLeaderboardStrategy();
		}

		// Token: 0x170022EF RID: 8943
		// (get) Token: 0x06007E89 RID: 32393 RVA: 0x002AE3A9 File Offset: 0x002AC5A9
		public static PlatformType CurrentPlatform
		{
			get
			{
				return LeaderboardManager._strategy.Platform;
			}
		}

		/// <summary>
		/// Obtains the leaderboard with the specified name. If it doesn't exist, it is created first.
		/// </summary>
		// Token: 0x06007E8A RID: 32394 RVA: 0x002AE3B5 File Offset: 0x002AC5B5
		public static Task<ILeaderboardHandle> GetOrCreateLeaderboard(string name, CancellationToken cancelToken = default(CancellationToken))
		{
			return LeaderboardManager._strategy.GetOrCreateLeaderboard(name, cancelToken);
		}

		/// <summary>
		/// Obtains the leaderboard with the specified name. If it doesn't exist, null is returned.
		/// </summary>
		// Token: 0x06007E8B RID: 32395 RVA: 0x002AE3C3 File Offset: 0x002AC5C3
		[return: Nullable(new byte[] { 1, 2 })]
		public static Task<ILeaderboardHandle> GetLeaderboard(string name, CancellationToken cancelToken = default(CancellationToken))
		{
			return LeaderboardManager._strategy.GetLeaderboard(name, cancelToken);
		}

		/// <summary>
		/// Uploads a score associated with the local player.
		/// </summary>
		/// <param name="handle">The leaderboard to upload to, obtained from GetLeaderboard.</param>
		/// <param name="score">The score to upload.</param>
		/// <param name="userIds">User IDs to associate with the score. These will be returned with the leaderboard entry in
		/// future queries, but note that you can only query for the score based on the submitting player.</param>
		// Token: 0x06007E8C RID: 32396 RVA: 0x002AE3D1 File Offset: 0x002AC5D1
		public static Task UploadLocalScore(ILeaderboardHandle handle, int score, IReadOnlyList<ulong> userIds)
		{
			return LeaderboardManager._strategy.UploadLocalScore(handle, score, userIds);
		}

		/// <summary>
		/// Queries the given leaderboard.
		/// </summary>
		/// <param name="handle">The leaderboard to query, obtained from GetLeaderboard.</param>
		/// <param name="type">The type of query.</param>
		/// <param name="startIndex">The index at which to start the query.</param>
		/// <param name="resultCount">The amount of results from the start index to return.</param>
		/// <param name="cancelToken">The token to use when requesting cancellation.</param>
		/// <returns>A task which will result in the entries queried.</returns>
		// Token: 0x06007E8D RID: 32397 RVA: 0x002AE3E0 File Offset: 0x002AC5E0
		public static Task<List<LeaderboardEntry>> QueryLeaderboard(ILeaderboardHandle handle, LeaderboardQueryType type, int startIndex, int resultCount, CancellationToken cancelToken = default(CancellationToken))
		{
			return LeaderboardManager._strategy.QueryLeaderboard(handle, type, startIndex, resultCount, cancelToken);
		}

		/// <summary>
		/// Queries the given leaderboard, only returning entries for the passed users (if they exist).
		/// </summary>
		/// <param name="handle">The leaderboard to query, obtained from GetLeaderboard.</param>
		/// <param name="userIds">List of platform identifiers to query the leaderboard for.</param>
		/// <param name="cancelToken">The token to use when requesting cancellation.</param>
		/// <returns>A task which will result in the entries queried.</returns>
		// Token: 0x06007E8E RID: 32398 RVA: 0x002AE3F2 File Offset: 0x002AC5F2
		public static Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(ILeaderboardHandle handle, IReadOnlyList<ulong> userIds, CancellationToken cancelToken = default(CancellationToken))
		{
			return LeaderboardManager._strategy.QueryLeaderboardForUsers(handle, userIds, cancelToken);
		}

		/// <summary>
		/// Obtains the number of entries in the given leaderboard.
		/// </summary>
		// Token: 0x06007E8F RID: 32399 RVA: 0x002AE401 File Offset: 0x002AC601
		public static int GetLeaderboardEntryCount(ILeaderboardHandle handle)
		{
			return LeaderboardManager._strategy.GetLeaderboardEntryCount(handle);
		}

		/// <summary>
		/// A method used in debugging. This will throw if the leaderboard is pointing to an actual backend.
		/// Rank is ignored. Score will be used to sort the entries.
		/// </summary>
		// Token: 0x06007E90 RID: 32400 RVA: 0x002AE410 File Offset: 0x002AC610
		public static void DebugAddEntry(ILeaderboardHandle handle, LeaderboardEntry entry)
		{
			NullLeaderboardStrategy nullLeaderboardStrategy = LeaderboardManager._strategy as NullLeaderboardStrategy;
			if (nullLeaderboardStrategy == null)
			{
				throw new NotImplementedException();
			}
			nullLeaderboardStrategy.DebugAddEntry(handle, entry);
		}

		// Token: 0x04002A48 RID: 10824
		private static ILeaderboardStrategy _strategy;
	}
}
