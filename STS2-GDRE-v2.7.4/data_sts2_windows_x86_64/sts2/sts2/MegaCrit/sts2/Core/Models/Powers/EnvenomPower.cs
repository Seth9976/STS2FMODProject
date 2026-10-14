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
	// Token: 0x0200066E RID: 1646
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EnvenomPower : PowerModel
	{
		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06005821 RID: 22561 RVA: 0x0024E1D7 File Offset: 0x0024C3D7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06005822 RID: 22562 RVA: 0x0024E1DA File Offset: 0x0024C3DA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06005823 RID: 22563 RVA: 0x0024E1E0 File Offset: 0x0024C3E0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x0024E200 File Offset: 0x0024C400
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer == base.Owner)
			{
				if (props.IsPoweredAttack())
				{
					if (result.UnblockedDamage > 0)
					{
						await PowerCmd.Apply<PoisonPower>(choiceContext, target, base.Amount, base.Owner, null, false);
					}
				}
			}
		}
	}
}
