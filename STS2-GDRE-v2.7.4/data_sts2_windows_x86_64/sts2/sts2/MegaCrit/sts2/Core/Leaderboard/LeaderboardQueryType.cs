using System;

namespace MegaCrit.Sts2.Core.Leaderboard
{
	// Token: 0x02000BF6 RID: 3062
	public enum LeaderboardQueryType
	{
		// Token: 0x04002A4A RID: 10826
		None,
		/// <summary>
		/// Query globally. Indexes control the range of the query.
		/// </summary>
		// Token: 0x04002A4B RID: 10827
		Global,
		/// <summary>
		/// Query around the current user. Indexes are relative to the user.
		/// </summary>
		// Token: 0x04002A4C RID: 10828
		AroundUser,
		/// <summary>
		/// Query the user's friends. Indexes are ignored.
		/// </summary>
		// Token: 0x04002A4D RID: 10829
		FriendsOnly
	}
}
