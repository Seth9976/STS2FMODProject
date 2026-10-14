using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C5 RID: 1733
	public sealed class PaperCutsPower : PowerModel
	{
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06005A0F RID: 23055 RVA: 0x0025185B File Offset: 0x0024FA5B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06005A10 RID: 23056 RVA: 0x0025185E File Offset: 0x0024FA5E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x00251864 File Offset: 0x0024FA64
		[NullableContext(1)]
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer == base.Owner)
			{
				if (target.IsPlayer)
				{
					if (props.IsPoweredAttack())
					{
						if (result.UnblockedDamage > 0)
						{
							await CreatureCmd.LoseMaxHp(choiceContext, target, base.Amount, false);
						}
					}
				}
			}
		}
	}
}
