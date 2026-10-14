using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000640 RID: 1600
	public sealed class CalcifyPower : PowerModel
	{
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x0600571E RID: 22302 RVA: 0x0024C42B File Offset: 0x0024A62B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x0024C42E File Offset: 0x0024A62E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0024C434 File Offset: 0x0024A634
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!(((dealer != null) ? dealer.Monster : null) is Osty))
			{
				return 0m;
			}
			Creature owner = base.Owner;
			Player petOwner = dealer.PetOwner;
			if (owner != ((petOwner != null) ? petOwner.Creature : null))
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
