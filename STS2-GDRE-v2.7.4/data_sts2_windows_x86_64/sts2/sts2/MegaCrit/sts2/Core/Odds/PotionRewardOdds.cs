using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Odds
{
	// Token: 0x02000171 RID: 369
	[NullableContext(1)]
	[Nullable(0)]
	public class PotionRewardOdds : AbstractOdds
	{
		/// <summary>
		/// For creating at the start of a run.
		/// </summary>
		/// <param name="rng">RNG to use for rolls.</param>
		// Token: 0x06001141 RID: 4417 RVA: 0x00064C91 File Offset: 0x00062E91
		public PotionRewardOdds(Rng rng)
			: base(0.4f, rng)
		{
		}

		/// <summary>
		/// For restoring from save.
		/// </summary>
		/// <param name="initialValue">Restored value at the saved spot in the run.</param>
		/// <param name="rng">RNG to use for rolls.</param>
		// Token: 0x06001142 RID: 4418 RVA: 0x00064C9F File Offset: 0x00062E9F
		public PotionRewardOdds(float initialValue, Rng rng)
			: base(initialValue, rng)
		{
		}

		/// <summary>
		/// Roll for whether or not a potion should be included in a set of rewards.
		/// Using this will modify the odds of future potion rewards.
		/// </summary>
		/// <param name="player">The player who will receive the potential potion reward.</param>
		/// <param name="roomType">Room type for roll, affects bonus chance for potions.</param>
		/// <returns>Whether or not to give a potion reward.</returns>
		/// <remarks>
		/// This method implements a pity system where:
		/// - If a potion is rolled, future potion chances decrease by 10%
		/// - If a potion is not rolled, future potion chances increase by 10%
		/// - Elite rooms provide a 25% bonus chance for potions
		/// - The HookBus allows for external modification of potion reward odds
		/// </remarks>
		// Token: 0x06001143 RID: 4419 RVA: 0x00064CAC File Offset: 0x00062EAC
		public bool Roll(Player player, RoomType roomType)
		{
			float currentValue = base.CurrentValue;
			bool flag = Hook.ShouldForcePotionReward(player.RunState, player, roomType);
			if (flag)
			{
				return true;
			}
			float num;
			if (roomType == RoomType.Elite)
			{
				num = 0.25f;
			}
			else
			{
				num = 0f;
			}
			float num2 = num;
			float num3 = currentValue + num2 * 0.5f;
			float num4 = this._rng.NextFloat(1f);
			if (num4 < num3)
			{
				base.CurrentValue -= 0.1f;
				return true;
			}
			base.CurrentValue += 0.1f;
			return false;
		}

		/// <summary>
		/// The percentage of normal combats that we expect to roll potions over the course of a run.
		/// While our rolling logic is more complex than this (to avoid streakiness and other game-feel reasons), we should
		/// converge on this percentage.
		/// </summary>
		// Token: 0x04000623 RID: 1571
		public const float targetOdds = 0.5f;

		// Token: 0x04000624 RID: 1572
		public const float eliteBonus = 0.25f;

		// Token: 0x04000625 RID: 1573
		private const float _basePotionRewardOdds = 0.4f;
	}
}
