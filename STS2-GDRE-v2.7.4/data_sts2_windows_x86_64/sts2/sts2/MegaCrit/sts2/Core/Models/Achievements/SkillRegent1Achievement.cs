using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when the player forges a Sovereign Blade which deals a large amount of damage.
	/// </summary>
	// Token: 0x02000B8A RID: 2954
	public class SkillRegent1Achievement : AchievementModel
	{
		// Token: 0x06007B9D RID: 31645 RVA: 0x002A0FFC File Offset: 0x0029F1FC
		[NullableContext(1)]
		public override Task AfterForge(decimal amount, Player forger, [Nullable(2)] AbstractModel source)
		{
			if (!LocalContext.IsMe(forger))
			{
				return Task.CompletedTask;
			}
			if (AchievementsUtil.IsUnlocked(Achievement.CharacterSkillRegent1))
			{
				return Task.CompletedTask;
			}
			foreach (SovereignBlade sovereignBlade in forger.PlayerCombatState.AllCards.OfType<SovereignBlade>())
			{
				if (sovereignBlade.DynamicVars.Damage.BaseValue >= 999m)
				{
					AchievementsUtil.Unlock(Achievement.CharacterSkillRegent1, forger);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x0400290E RID: 10510
		private const int _damageThreshold = 999;
	}
}
