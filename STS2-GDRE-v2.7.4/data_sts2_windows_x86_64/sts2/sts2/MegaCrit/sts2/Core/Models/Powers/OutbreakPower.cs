using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C0 RID: 1728
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OutbreakPower : PowerModel
	{
		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x060059EB RID: 23019 RVA: 0x002514EF File Offset: 0x0024F6EF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x060059EC RID: 23020 RVA: 0x002514F2 File Offset: 0x0024F6F2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x060059ED RID: 23021 RVA: 0x002514F8 File Offset: 0x0024F6F8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x00251518 File Offset: 0x0024F718
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (applier == base.Owner)
			{
				if (!(amount <= 0m))
				{
					if (power is PoisonPower)
					{
						base.Flash();
						await CreatureCmd.Damage(choiceContext, base.Owner.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}
	}
}
