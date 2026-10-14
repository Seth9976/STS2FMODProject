using System;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x02000159 RID: 345
	public static class StatsManager
	{
		// Token: 0x06001091 RID: 4241 RVA: 0x0006246A File Offset: 0x0006066A
		public static void RefreshGlobalStats()
		{
			TaskHelper.RunSafely(SteamStatsManager.RefreshGlobalStats());
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00062477 File Offset: 0x00060677
		public static void IncrementArchitectDamage(int score)
		{
			SteamStatsManager.IncrementArchitectDamage(score);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0006247F File Offset: 0x0006067F
		public static long GetPersonalArchitectDamage()
		{
			return SaveManager.Instance.Progress.ArchitectDamage;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00062490 File Offset: 0x00060690
		public static long? GetGlobalArchitectDamage()
		{
			if (SteamStatsManager.IsGlobalStatsReady)
			{
				long globalArchitectDamage = SteamStatsManager.GetGlobalArchitectDamage();
				if (globalArchitectDamage > 0L)
				{
					return new long?(globalArchitectDamage);
				}
			}
			return null;
		}
	}
}
