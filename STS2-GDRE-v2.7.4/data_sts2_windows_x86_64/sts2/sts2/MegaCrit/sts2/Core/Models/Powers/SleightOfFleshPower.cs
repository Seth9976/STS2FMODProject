using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F1 RID: 1777
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SleightOfFleshPower : PowerModel
	{
		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06005B23 RID: 23331 RVA: 0x00253B66 File Offset: 0x00251D66
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06005B24 RID: 23332 RVA: 0x00253B69 File Offset: 0x00251D69
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06005B25 RID: 23333 RVA: 0x00253B6C File Offset: 0x00251D6C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x00253B74 File Offset: 0x00251D74
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount == 0m))
			{
				if (power.GetTypeForAmount(amount) == PowerType.Debuff)
				{
					if (power.Owner.IsEnemy)
					{
						if (applier == base.Owner)
						{
							if (!(power is ITemporaryPower))
							{
								base.Flash();
								await CreatureCmd.Damage(choiceContext, power.Owner, base.Amount, ValueProp.Unpowered, base.Owner);
							}
						}
					}
				}
			}
		}
	}
}
