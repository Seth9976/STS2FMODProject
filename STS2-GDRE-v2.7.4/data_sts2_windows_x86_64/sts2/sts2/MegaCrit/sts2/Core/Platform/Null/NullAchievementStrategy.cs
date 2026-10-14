using System;
using MegaCrit.Sts2.Core.Achievements;

namespace MegaCrit.Sts2.Core.Platform.Null
{
	// Token: 0x02000166 RID: 358
	public class NullAchievementStrategy : IAchievementStrategy
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x00063F7A File Offset: 0x0006217A
		public void Unlock(Achievement achievement)
		{
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00063F7C File Offset: 0x0006217C
		public void Revoke(Achievement achievement)
		{
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00063F7E File Offset: 0x0006217E
		public bool IsUnlocked(Achievement achievement)
		{
			return false;
		}
	}
}
