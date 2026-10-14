using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when the player applies a large amount of Strength to Osty.
	/// </summary>
	// Token: 0x02000B89 RID: 2953
	public class SkillNecrobinder2Achievement : AchievementModel
	{
		// Token: 0x06007B9B RID: 31643 RVA: 0x002A0FA0 File Offset: 0x0029F1A0
		[NullableContext(1)]
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!LocalContext.IsMe(applier))
			{
				return Task.CompletedTask;
			}
			if (power is StrengthPower && power.Owner.Monster is Osty && power.Amount >= 50)
			{
				AchievementsUtil.Unlock(Achievement.CharacterSkillNecrobinder2, applier.Player);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0400290D RID: 10509
		private const int _strengthThreshold = 50;
	}
}
