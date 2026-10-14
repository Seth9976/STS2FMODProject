using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000627 RID: 1575
	public sealed class AccuracyPower : PowerModel
	{
		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600568E RID: 22158 RVA: 0x0024B6C8 File Offset: 0x002498C8
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600568F RID: 22159 RVA: 0x0024B6CB File Offset: 0x002498CB
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0024B6D0 File Offset: 0x002498D0
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel card, CardPlay cardPlay)
		{
			if (base.Owner != dealer)
			{
				return 0m;
			}
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			if (card == null)
			{
				return 0m;
			}
			if (!card.Tags.Contains(CardTag.Shiv))
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
