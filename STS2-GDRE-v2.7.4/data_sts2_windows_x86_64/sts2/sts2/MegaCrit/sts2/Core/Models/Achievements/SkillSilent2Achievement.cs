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
	/// Grants an achievement when the player applies a large amount of poison to a single creature.
	/// </summary>
	// Token: 0x02000B8D RID: 2957
	public class SkillSilent2Achievement : AchievementModel
	{
		// Token: 0x06007BA6 RID: 31654 RVA: 0x002A11C3 File Offset: 0x0029F3C3
		[NullableContext(1)]
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!LocalContext.IsMe(applier))
			{
				return Task.CompletedTask;
			}
			if (power is PoisonPower && power.Amount >= 99)
			{
				AchievementsUtil.Unlock(Achievement.CharacterSkillSilent2, applier.Player);
			}
			return Task.CompletedTask;
		}

		// Token: 0x04002912 RID: 10514
		private const int _poisonThreshold = 99;
	}
}
