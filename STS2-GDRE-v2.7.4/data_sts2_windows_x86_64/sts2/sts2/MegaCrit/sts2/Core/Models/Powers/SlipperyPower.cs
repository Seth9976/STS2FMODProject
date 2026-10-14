using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F2 RID: 1778
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlipperyPower : PowerModel
	{
		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06005B28 RID: 23336 RVA: 0x00253BE0 File Offset: 0x00251DE0
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06005B29 RID: 23337 RVA: 0x00253BE3 File Offset: 0x00251DE3
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06005B2A RID: 23338 RVA: 0x00253BE6 File Offset: 0x00251DE6
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x00253BE9 File Offset: 0x00251DE9
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return amount;
			}
			if (amount < 1m)
			{
				return amount;
			}
			return 1m;
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x00253C0C File Offset: 0x00251E0C
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (result.UnblockedDamage >= 1)
				{
					base.Flash();
					await PowerCmd.Decrement(this);
				}
			}
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x00253C5F File Offset: 0x00251E5F
		public override decimal GetScaledAmountForMultiplayer(ICombatState combatState, [Nullable(2)] Creature applier, decimal amount, Creature target, [Nullable(2)] CardModel cardSource)
		{
			return amount * combatState.Players.Count;
		}
	}
}
