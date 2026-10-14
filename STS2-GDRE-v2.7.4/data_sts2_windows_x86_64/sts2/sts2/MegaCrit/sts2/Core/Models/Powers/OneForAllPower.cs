using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006BD RID: 1725
	public sealed class OneForAllPower : PowerModel
	{
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x060059D8 RID: 23000 RVA: 0x002512DB File Offset: 0x0024F4DB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x060059D9 RID: 23001 RVA: 0x002512DE File Offset: 0x0024F4DE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x002512E4 File Offset: 0x0024F4E4
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			if (cardSource == null)
			{
				return 0m;
			}
			if (cardSource.Owner.Creature != base.Owner)
			{
				return 0m;
			}
			if ((cardPlay != null) ? cardPlay.Card.EnergyCost.CostsX : cardSource.EnergyCost.CostsX)
			{
				return 0m;
			}
			if (cardPlay != null && cardPlay.Resources.EnergySpent != 0)
			{
				return 0m;
			}
			if (cardPlay == null && cardSource.EnergyCost.GetWithModifiers(CostModifiers.All) != 0)
			{
				return 0m;
			}
			return base.Amount;
		}
	}
}
