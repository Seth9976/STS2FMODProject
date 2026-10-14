using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when the player obtains a large number of Stars.
	/// </summary>
	// Token: 0x02000B8B RID: 2955
	public class SkillRegent2Achievement : AchievementModel
	{
		// Token: 0x06007B9F RID: 31647 RVA: 0x002A10A0 File Offset: 0x0029F2A0
		[NullableContext(1)]
		public override Task AfterStarsGained(int amount, Player gainer)
		{
			if (!LocalContext.IsMe(gainer))
			{
				return Task.CompletedTask;
			}
			PlayerCombatState playerCombatState = gainer.PlayerCombatState;
			if (playerCombatState != null && playerCombatState.Stars < 20)
			{
				return Task.CompletedTask;
			}
			AchievementsUtil.Unlock(Achievement.CharacterSkillRegent2, gainer);
			return Task.CompletedTask;
		}

		// Token: 0x0400290F RID: 10511
		private const int _starThreshold = 20;
	}
}
