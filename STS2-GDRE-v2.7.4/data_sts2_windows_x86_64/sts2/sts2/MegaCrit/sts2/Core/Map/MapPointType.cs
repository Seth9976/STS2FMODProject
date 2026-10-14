using System;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// Represents a type of point that can appear on the map. This is subtly different from RoomType.
	/// Please see <see cref="T:MegaCrit.Sts2.Core.Rooms.RoomType" /> for more details.
	/// </summary>
	// Token: 0x02000BA4 RID: 2980
	public enum MapPointType
	{
		// Token: 0x04002965 RID: 10597
		Unassigned,
		// Token: 0x04002966 RID: 10598
		Unknown,
		// Token: 0x04002967 RID: 10599
		Shop,
		// Token: 0x04002968 RID: 10600
		Treasure,
		// Token: 0x04002969 RID: 10601
		RestSite,
		// Token: 0x0400296A RID: 10602
		Monster,
		// Token: 0x0400296B RID: 10603
		Elite,
		// Token: 0x0400296C RID: 10604
		Boss,
		// Token: 0x0400296D RID: 10605
		Ancient
	}
}
