using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A5 RID: 1701
	public sealed class LeadershipPower : PowerModel
	{
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x0600595A RID: 22874 RVA: 0x002505D7 File Offset: 0x0024E7D7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x0600595B RID: 22875 RVA: 0x002505DA File Offset: 0x0024E7DA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x002505E0 File Offset: 0x0024E7E0
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner == dealer)
			{
				return 0m;
			}
			if (base.Owner.Side != dealer.Side)
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
