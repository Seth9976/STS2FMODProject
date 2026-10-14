using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Null;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Platform
{
	/// <summary>
	/// Controls achievement unlocking.
	/// </summary>
	// Token: 0x02000152 RID: 338
	[NullableContext(2)]
	[Nullable(0)]
	public static class AchievementsUtil
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001060 RID: 4192 RVA: 0x000620FC File Offset: 0x000602FC
		// (remove) Token: 0x06001061 RID: 4193 RVA: 0x00062130 File Offset: 0x00060330
		public static event Action AchievementsChanged;

		/// <summary>
		/// Unlocks an achievement on the current platform and profile.
		/// Remember that the player can have multiple profiles per platform account. This unlocks the achievement on the
		/// profile's progress.save, as well as on the platform account if the account does not yet have it.
		/// </summary>
		/// <param name="achievement">Achievement to unlock.</param>
		/// <param name="localPlayer">
		/// Optional local player from when the achievement was unlocked.
		/// Will be null when unlocked outside a run (like on the main menu or after game over).
		/// </param>
		// Token: 0x06001062 RID: 4194 RVA: 0x00062163 File Offset: 0x00060363
		public static void Unlock(Achievement achievement, Player localPlayer)
		{
		}

		/// <summary>
		/// Revokes an achievement from the current player profile.
		/// Should only be used for debugging.
		/// </summary>
		// Token: 0x06001063 RID: 4195 RVA: 0x00062168 File Offset: 0x00060368
		public static void Revoke(Achievement achievement)
		{
			if (!AchievementsUtil.IsUnlocked(achievement))
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Revoking achievement: ");
			defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement);
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			AchievementsUtil._platform.Revoke(achievement);
			if (SaveManager.Instance.Progress.RemoveUnlockedAchievement(achievement))
			{
				SaveManager.Instance.SaveProgressFile();
			}
			Action achievementsChanged = AchievementsUtil.AchievementsChanged;
			if (achievementsChanged == null)
			{
				return;
			}
			achievementsChanged();
		}

		/// <summary>
		/// Returns whether an achievement is unlocked on the current player profile.
		/// Note that this does _not_ tell you whether the achievement is unlocked on the player's platform account. There
		/// is likely no scenario in which we need that, but if we do, add a new API.
		/// </summary>
		// Token: 0x06001064 RID: 4196 RVA: 0x000621DF File Offset: 0x000603DF
		public static bool IsUnlocked(Achievement achievement)
		{
			return SaveManager.Instance.Progress.IsAchievementUnlocked(achievement);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000621F1 File Offset: 0x000603F1
		public static int TotalAchievementCount()
		{
			return Enum.GetValues<Achievement>().Length;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000621FA File Offset: 0x000603FA
		public static int UnlockedAchievementCount()
		{
			IEnumerable<Achievement> values = Enum.GetValues<Achievement>();
			Func<Achievement, bool> func;
			if ((func = AchievementsUtil.<>O.<0>__IsUnlocked) == null)
			{
				func = (AchievementsUtil.<>O.<0>__IsUnlocked = new Func<Achievement, bool>(AchievementsUtil.IsUnlocked));
			}
			return values.Count<Achievement>(func);
		}

		// Token: 0x040005C3 RID: 1475
		[Nullable(1)]
		private static readonly IAchievementStrategy _platform = new NullAchievementStrategy();

		// Token: 0x02000F3A RID: 3898
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003C78 RID: 15480
			[Nullable(0)]
			public static Func<Achievement, bool> <0>__IsUnlocked;
		}
	}
}
