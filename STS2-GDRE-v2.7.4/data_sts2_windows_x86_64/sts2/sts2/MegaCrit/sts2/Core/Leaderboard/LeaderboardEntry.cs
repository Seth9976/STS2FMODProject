using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Leaderboard
{
	/// <summary>
	/// Abstracted leaderboard entry returned by querying the leaderboard API.
	/// </summary>
	// Token: 0x02000BF4 RID: 3060
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public class LeaderboardEntry
	{
		// Token: 0x06007E87 RID: 32391 RVA: 0x002AE378 File Offset: 0x002AC578
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public LeaderboardEntry()
		{
		}

		/// <summary>
		/// Rank on the leaderboard reported from the API.
		/// </summary>
		// Token: 0x04002A43 RID: 10819
		public int rank;

		/// <summary>
		/// Name of the user who submitted the entry.
		/// </summary>
		// Token: 0x04002A44 RID: 10820
		[RequiredMember]
		public string name;

		/// <summary>
		/// ID of the user who submitted the entry.
		/// </summary>
		// Token: 0x04002A45 RID: 10821
		public ulong id;

		/// <summary>
		/// The score of the submission.
		/// </summary>
		// Token: 0x04002A46 RID: 10822
		public int score;

		/// <summary>
		/// Other player IDs present in the run when this score was submitted.
		/// </summary>
		// Token: 0x04002A47 RID: 10823
		public List<ulong> userIds = new List<ulong>();
	}
}
