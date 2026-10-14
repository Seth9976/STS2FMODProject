using System;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000104 RID: 260
	public static class GameModeExtension
	{
		/// <summary>
		/// Helper method to check if epochs and achievements are allowed this run.
		/// Update once more game modes are created.
		/// </summary>
		// Token: 0x06000B3C RID: 2876 RVA: 0x0004C514 File Offset: 0x0004A714
		public static bool AreAchievementsAndEpochsLocked(this GameMode gameMode)
		{
			return gameMode != GameMode.Standard;
		}
	}
}
