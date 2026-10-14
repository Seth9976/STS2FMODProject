using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Leaderboard;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x0200016A RID: 362
	[RequiredMember]
	public class NullLeaderboardHandle : ILeaderboardHandle
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x00063FDE File Offset: 0x000621DE
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public NullLeaderboardHandle()
		{
		}

		// Token: 0x0400060E RID: 1550
		[Nullable(1)]
		[RequiredMember]
		public NullLeaderboard leaderboard;
	}
}
