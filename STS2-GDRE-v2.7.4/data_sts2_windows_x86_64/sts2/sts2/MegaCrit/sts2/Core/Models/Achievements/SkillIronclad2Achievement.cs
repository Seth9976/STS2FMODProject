using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when a player deals a large amount of damage in a single blow.
	/// </summary>
	// Token: 0x02000B87 RID: 2951
	public class SkillIronclad2Achievement : AchievementModel
	{
		// Token: 0x06007B97 RID: 31639 RVA: 0x002A0F1F File Offset: 0x0029F11F
		[NullableContext(1)]
		public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (!LocalContext.IsMe(dealer))
			{
				return Task.CompletedTask;
			}
			if (result.UnblockedDamage < 999)
			{
				return Task.CompletedTask;
			}
			AchievementsUtil.Unlock(Achievement.CharacterSkillIronclad2, dealer.Player);
			return Task.CompletedTask;
		}

		// Token: 0x0400290B RID: 10507
		private const int _damageRequirement = 999;
	}
}
