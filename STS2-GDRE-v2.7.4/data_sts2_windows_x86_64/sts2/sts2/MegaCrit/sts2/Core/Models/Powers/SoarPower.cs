using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F9 RID: 1785
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoarPower : PowerModel
	{
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06005B56 RID: 23382 RVA: 0x0025416F File Offset: 0x0025236F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06005B57 RID: 23383 RVA: 0x00254172 File Offset: 0x00252372
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06005B58 RID: 23384 RVA: 0x00254175 File Offset: 0x00252375
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 50m));
			}
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x0025418D File Offset: 0x0025238D
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			return base.DynamicVars["DamageDecrease"].BaseValue / 100m;
		}

		// Token: 0x0400251C RID: 9500
		private const string _damageDecreaseKey = "DamageDecrease";
	}
}
