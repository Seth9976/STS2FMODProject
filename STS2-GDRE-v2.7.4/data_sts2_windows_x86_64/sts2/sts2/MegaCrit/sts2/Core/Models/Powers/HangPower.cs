using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200068B RID: 1675
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HangPower : PowerModel
	{
		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x060058B9 RID: 22713 RVA: 0x0024F2CF File Offset: 0x0024D4CF
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x060058BA RID: 22714 RVA: 0x0024F2D2 File Offset: 0x0024D4D2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x060058BB RID: 22715 RVA: 0x0024F2D5 File Offset: 0x0024D4D5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Hang>(false));
			}
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x0024F2E2 File Offset: 0x0024D4E2
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!(cardSource is Hang))
			{
				return 1m;
			}
			return base.Amount;
		}
	}
}
