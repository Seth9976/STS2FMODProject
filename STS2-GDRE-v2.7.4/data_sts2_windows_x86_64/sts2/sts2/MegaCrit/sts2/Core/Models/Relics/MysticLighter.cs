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
	// Token: 0x02000592 RID: 1426
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MysticLighter : RelicModel
	{
		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x060052B4 RID: 21172 RVA: 0x0024360A File Offset: 0x0024180A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x060052B5 RID: 21173 RVA: 0x0024360D File Offset: 0x0024180D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(9m, ValueProp.Unpowered));
			}
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x00243624 File Offset: 0x00241824
		[NullableContext(2)]
		public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			if (((cardSource != null) ? cardSource.Enchantment : null) == null)
			{
				return 0m;
			}
			if (cardSource.Owner != base.Owner)
			{
				return 0m;
			}
			return base.DynamicVars.Damage.IntValue;
		}
	}
}
