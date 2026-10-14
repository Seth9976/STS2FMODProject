using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE0 RID: 3040
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageVar : DynamicVar
	{
		// Token: 0x170022C7 RID: 8903
		// (get) Token: 0x06007E05 RID: 32261 RVA: 0x002AD6E6 File Offset: 0x002AB8E6
		// (set) Token: 0x06007E06 RID: 32262 RVA: 0x002AD6EE File Offset: 0x002AB8EE
		public ValueProp Props { get; set; }

		// Token: 0x06007E07 RID: 32263 RVA: 0x002AD6F7 File Offset: 0x002AB8F7
		public DamageVar(decimal damage, ValueProp props)
			: base("Damage", damage)
		{
			this.Props = props;
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x002AD70C File Offset: 0x002AB90C
		public DamageVar(string name, decimal damage, ValueProp props)
			: base(name, damage)
		{
			this.Props = props;
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x002AD720 File Offset: 0x002AB920
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			decimal num = base.BaseValue;
			EnchantmentModel enchantment = card.Enchantment;
			if (enchantment != null)
			{
				num += enchantment.EnchantDamageAdditive(num, this.Props);
				num *= enchantment.EnchantDamageMultiplicative(num, this.Props);
				if (!card.IsEnchantmentPreview)
				{
					base.EnchantedValue = num;
				}
			}
			if (runGlobalHooks)
			{
				IEnumerable<AbstractModel> enumerable;
				num = Hook.ModifyDamage(card.Owner.RunState, card.CombatState, target, card.Owner.Creature, base.BaseValue, this.Props, card, null, ModifyDamageHookType.All, previewMode, out enumerable);
			}
			base.PreviewValue = num;
		}

		// Token: 0x04002A29 RID: 10793
		public const string defaultName = "Damage";
	}
}
