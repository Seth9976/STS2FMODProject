using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Keeps track of how many debuffs a player has applied.
	/// </summary>
	// Token: 0x02000B62 RID: 2914
	public class DebufferModel : BadgeModel
	{
		// Token: 0x06007AC9 RID: 31433 RVA: 0x0029F4EC File Offset: 0x0029D6EC
		[NullableContext(1)]
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (power.GetTypeForAmount(amount) == PowerType.Debuff && !(power is ITemporaryPower) && applier != null && applier.Player != null && applier != power.Owner)
			{
				applier.Player.ExtraFields.DebuffsApplied++;
			}
			return Task.CompletedTask;
		}
	}
}
