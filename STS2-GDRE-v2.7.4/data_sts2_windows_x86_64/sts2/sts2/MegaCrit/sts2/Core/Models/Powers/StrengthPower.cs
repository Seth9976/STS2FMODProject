using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000707 RID: 1799
	public sealed class StrengthPower : PowerModel
	{
		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06005B9E RID: 23454 RVA: 0x0025488F File Offset: 0x00252A8F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06005B9F RID: 23455 RVA: 0x00254892 File Offset: 0x00252A92
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x00254895 File Offset: 0x00252A95
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x00254898 File Offset: 0x00252A98
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != dealer)
			{
				return 0m;
			}
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
