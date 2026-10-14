using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000637 RID: 1591
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlackHolePower : PowerModel
	{
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x0024BE6F File Offset: 0x0024A06F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0024BE72 File Offset: 0x0024A072
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		/// <remarks>
		/// We do this in AfterCardPlayed instead of AfterStarsSpent because stars are spent at the beginning of the card
		/// play, but Black Hole should trigger after the card is played.
		/// </remarks>
		// Token: 0x060056E9 RID: 22249 RVA: 0x0024BE78 File Offset: 0x0024A078
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Resources.StarsSpent > 0)
			{
				if (cardPlay.Card.Owner == base.Owner.Player)
				{
					if (cardPlay.IsLastInSeries)
					{
						await this.DealDamageToAllEnemies();
					}
				}
			}
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x0024BEC4 File Offset: 0x0024A0C4
		public override async Task AfterStarsGained(int amount, Player gainer)
		{
			if (amount > 0)
			{
				if (gainer == base.Owner.Player)
				{
					await this.DealDamageToAllEnemies();
				}
			}
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x0024BF18 File Offset: 0x0024A118
		private async Task DealDamageToAllEnemies()
		{
			base.Flash();
			await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
		}
	}
}
