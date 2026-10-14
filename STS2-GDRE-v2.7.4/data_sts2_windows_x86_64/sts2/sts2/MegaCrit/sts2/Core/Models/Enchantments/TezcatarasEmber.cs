using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E5 RID: 2277
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TezcatarasEmber : EnchantmentModel
	{
		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x06006CA2 RID: 27810 RVA: 0x002822BF File Offset: 0x002804BF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Eternal));
			}
		}

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x06006CA3 RID: 27811 RVA: 0x002822CC File Offset: 0x002804CC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Move));
			}
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x002822DF File Offset: 0x002804DF
		protected override void OnEnchant()
		{
			base.Card.EnergyCost.UpgradeBy(-base.Card.EnergyCost.GetWithModifiers(CostModifiers.None));
			base.Card.AddKeyword(CardKeyword.Eternal);
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x0028230F File Offset: 0x0028050F
		public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.DynamicVars.Damage.BaseValue;
		}
	}
}
