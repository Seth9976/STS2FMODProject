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
	// Token: 0x02000BDA RID: 3034
	[NullableContext(1)]
	[Nullable(0)]
	public class CalculatedBlockVar : CalculatedVar
	{
		// Token: 0x170022C4 RID: 8900
		// (get) Token: 0x06007DEC RID: 32236 RVA: 0x002AD297 File Offset: 0x002AB497
		public ValueProp Props { get; }

		/// <summary>
		/// Create a new <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedBlockVar" />.
		/// This will only work if the owner is a <see cref="T:MegaCrit.Sts2.Core.Models.CardModel" /> whose <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVarSet" /> also has a
		/// <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationBaseVar" /> and a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationExtraVar" />.
		/// Note: For cards whose values are entirely dynamic and have no base value (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Mirage" />), you
		/// should use a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationBaseVar" /> of 0 and a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationExtraVar" /> of 1.
		/// </summary>
		// Token: 0x06007DED RID: 32237 RVA: 0x002AD29F File Offset: 0x002AB49F
		public CalculatedBlockVar(ValueProp props)
			: base("CalculatedBlock")
		{
			this.Props = props;
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x002AD2B4 File Offset: 0x002AB4B4
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			EnchantmentModel enchantment = card.Enchantment;
			if (enchantment != null)
			{
				decimal num = this.GetBaseVar().BaseValue;
				num += enchantment.EnchantBlockAdditive(num);
				num *= enchantment.EnchantBlockMultiplicative(num);
				if (card.IsEnchantmentPreview)
				{
					base.PreviewValue = num;
				}
				else
				{
					base.EnchantedValue = num;
				}
			}
			decimal num2 = base.Calculate(target);
			if (runGlobalHooks)
			{
				IEnumerable<AbstractModel> enumerable;
				base.PreviewValue = Hook.ModifyBlock(card.CombatState, card.Owner.Creature, base.Calculate(target), this.Props, card, null, out enumerable);
				return;
			}
			if (!card.IsEnchantmentPreview)
			{
				if (enchantment != null)
				{
					num2 += enchantment.EnchantBlockAdditive(num2);
					num2 *= enchantment.EnchantBlockMultiplicative(num2);
				}
				base.PreviewValue = num2;
			}
		}

		// Token: 0x04002A20 RID: 10784
		public const string defaultName = "CalculatedBlock";
	}
}
