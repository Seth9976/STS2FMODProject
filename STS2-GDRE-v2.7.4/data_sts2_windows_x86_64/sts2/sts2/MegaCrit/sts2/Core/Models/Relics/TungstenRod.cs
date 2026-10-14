using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000604 RID: 1540
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TungstenRod : RelicModel
	{
		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x060055B9 RID: 21945 RVA: 0x00249267 File Offset: 0x00247467
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x060055BA RID: 21946 RVA: 0x0024926A File Offset: 0x0024746A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("HpLossReduction", 1m));
			}
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x00249280 File Offset: 0x00247480
		[NullableContext(2)]
		public override decimal ModifyHpLostAfterOsty([Nullable(1)] Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
		{
			if (target != base.Owner.Creature)
			{
				return amount;
			}
			return Math.Max(0m, amount - base.DynamicVars["HpLossReduction"].BaseValue);
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x002492B7 File Offset: 0x002474B7
		public override Task AfterModifyingHpLostAfterOsty()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x040024D9 RID: 9433
		private const string _hpLossReductionKey = "HpLossReduction";
	}
}
