using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000722 RID: 1826
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TrackingPower : PowerModel
	{
		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06005C59 RID: 23641 RVA: 0x002561B3 File Offset: 0x002543B3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06005C5A RID: 23642 RVA: 0x002561B6 File Offset: 0x002543B6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06005C5B RID: 23643 RVA: 0x002561BC File Offset: 0x002543BC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x002561DC File Offset: 0x002543DC
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			if (dealer != base.Owner && !base.Owner.Pets.Contains(dealer))
			{
				return 1m;
			}
			if (target == null || !target.HasPower<WeakPower>())
			{
				return 1m;
			}
			return 1m + base.Amount / 100m;
		}
	}
}
