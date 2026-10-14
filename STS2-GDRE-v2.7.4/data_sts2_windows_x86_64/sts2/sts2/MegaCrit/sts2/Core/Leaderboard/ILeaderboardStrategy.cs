using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Leaderboard
{
	/// <summary>
	/// Interface for platform leaderboard implementations.
	/// Note: This is currently modeled after Steam's leaderboard API. As we add other platforms, I expect the interface to
	/// change heavily or need refactoring.
	/// For documentation on individual methods, look at LeaderboardManager, which replicates these methods exactly.
	/// </summary>
	// Token: 0x02000BF3 RID: 3059
	[NullableContext(1)]
	public interface ILeaderboardStrategy
	{
		// Token: 0x170022EE RID: 8942
		// (get) Token: 0x06007E80 RID: 32384
		PlatformType Platform { get; }

		// Token: 0x06007E81 RID: 32385
		Task<ILeaderboardHandle> GetOrCreateLeaderboard(string name, CancellationToken cancelToken);

		// Token: 0x06007E82 RID: 32386
		[return: Nullable(new byte[] { 1, 2 })]
		Task<ILeaderboardHandle> GetLeaderboard(string name, CancellationToken cancelToken);

		// Token: 0x06007E83 RID: 32387
		Task UploadLocalScore(ILeaderboardHandle handle, int score, IReadOnlyList<ulong> otherIds);

		// Token: 0x06007E84 RID: 32388
		Task<List<LeaderboardEntry>> QueryLeaderboard(ILeaderboardHandle handle, LeaderboardQueryType type, int startIndex, int count, CancellationToken cancelToken = default(CancellationToken));

		// Token: 0x06007E85 RID: 32389
		Task<List<LeaderboardEntry>> QueryLeaderboardForUsers(ILeaderboardHandle handle, IReadOnlyList<ulong> userIds, CancellationToken cancelToken = default(CancellationToken));

		// Token: 0x06007E86 RID: 32390
		int GetLeaderboardEntryCount(ILeaderboardHandle handle);
	}
}
