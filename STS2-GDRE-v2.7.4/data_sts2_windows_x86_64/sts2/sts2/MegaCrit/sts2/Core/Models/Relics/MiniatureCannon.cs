using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200058B RID: 1419
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MiniatureCannon : RelicModel
	{
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x0600528D RID: 21133 RVA: 0x0024310B File Offset: 0x0024130B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x0600528E RID: 21134 RVA: 0x0024310E File Offset: 0x0024130E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraDamage", 3m));
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x00243128 File Offset: 0x00241328
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
			if (!cardSource.IsUpgraded)
			{
				return 0m;
			}
			if (dealer != base.Owner.Creature && cardSource.Owner != base.Owner)
			{
				return 0m;
			}
			return base.DynamicVars["ExtraDamage"].BaseValue;
		}

		// Token: 0x04002479 RID: 9337
		private const string _extraDamageKey = "ExtraDamage";
	}
}
