using System;
using MegaCrit.Sts2.Core.Achievements;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x02000153 RID: 339
	public interface IAchievementStrategy
	{
		// Token: 0x06001068 RID: 4200
		void Unlock(Achievement achievement);

		// Token: 0x06001069 RID: 4201
		void Revoke(Achievement achievement);

		// Token: 0x0600106A RID: 4202
		bool IsUnlocked(Achievement achievement);
	}
}
