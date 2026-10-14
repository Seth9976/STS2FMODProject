using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F0 RID: 1520
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrikeDummy : RelicModel
	{
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06005539 RID: 21817 RVA: 0x0024832B File Offset: 0x0024652B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x0600553A RID: 21818 RVA: 0x0024832E File Offset: 0x0024652E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraDamage", 3m));
			}
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x00248348 File Offset: 0x00246548
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
			if (!cardSource.Tags.Contains(CardTag.Strike))
			{
				return 0m;
			}
			if (dealer != base.Owner.Creature && cardSource.Owner != base.Owner)
			{
				return 0m;
			}
			return base.DynamicVars["ExtraDamage"].BaseValue;
		}

		// Token: 0x040024C3 RID: 9411
		private const string _extraDamageKey = "ExtraDamage";
	}
}
