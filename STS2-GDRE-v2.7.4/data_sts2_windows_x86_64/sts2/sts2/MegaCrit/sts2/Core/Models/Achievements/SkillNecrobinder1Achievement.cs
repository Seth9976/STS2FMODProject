using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when a player applies a large amount of Doom to a single creature.
	/// </summary>
	// Token: 0x02000B88 RID: 2952
	public class SkillNecrobinder1Achievement : AchievementModel
	{
		// Token: 0x06007B99 RID: 31641 RVA: 0x002A0F5C File Offset: 0x0029F15C
		[NullableContext(1)]
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!LocalContext.IsMe(applier))
			{
				return Task.CompletedTask;
			}
			if (power is DoomPower && power.Amount >= 999)
			{
				AchievementsUtil.Unlock(Achievement.CharacterSkillNecrobinder1, applier.Player);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0400290C RID: 10508
		private const int _doomThreshold = 999;
	}
}
