using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BDB RID: 3035
	[NullableContext(1)]
	[Nullable(0)]
	public class CalculatedDamageVar : CalculatedVar
	{
		// Token: 0x170022C5 RID: 8901
		// (get) Token: 0x06007DEF RID: 32239 RVA: 0x002AD373 File Offset: 0x002AB573
		public ValueProp Props { get; }

		// Token: 0x170022C6 RID: 8902
		// (get) Token: 0x06007DF0 RID: 32240 RVA: 0x002AD37B File Offset: 0x002AB57B
		// (set) Token: 0x06007DF1 RID: 32241 RVA: 0x002AD383 File Offset: 0x002AB583
		public bool IsFromOsty { get; private set; }

		/// <summary>
		/// Create a new <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedDamageVar" />.
		/// This will only work if the owner is a <see cref="T:MegaCrit.Sts2.Core.Models.CardModel" /> whose <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVarSet" /> also has a
		/// <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationBaseVar" /> and a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.ExtraDamageVar" />.
		/// Note: For cards whose values are entirely dynamic and have no base value (like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.BodySlam" />), you
		/// should use a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.CalculationBaseVar" /> of 0 and a <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.ExtraDamageVar" /> of 1.
		/// </summary>
		// Token: 0x06007DF2 RID: 32242 RVA: 0x002AD38C File Offset: 0x002AB58C
		public CalculatedDamageVar(ValueProp props)
			: base("CalculatedDamage")
		{
			this.Props = props;
		}

		/// <summary>
		/// Set this damage to come from Osty.
		/// </summary>
		// Token: 0x06007DF3 RID: 32243 RVA: 0x002AD3A0 File Offset: 0x002AB5A0
		public CalculatedDamageVar FromOsty()
		{
			this.IsFromOsty = true;
			return this;
		}

		// Token: 0x06007DF4 RID: 32244 RVA: 0x002AD3AC File Offset: 0x002AB5AC
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			EnchantmentModel enchantment = card.Enchantment;
			if (enchantment != null)
			{
				decimal num = this.GetBaseVar().BaseValue;
				num += enchantment.EnchantDamageAdditive(num, this.Props);
				num *= enchantment.EnchantDamageMultiplicative(num, this.Props);
				num = Math.Max(num, 0m);
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
				ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
				IEnumerable<AbstractModel> enumerable;
				base.PreviewValue = Hook.ModifyDamage(card.Owner.RunState, combatState, target, this.IsFromOsty ? card.Owner.Osty : card.Owner.Creature, num2, this.Props, card, null, ModifyDamageHookType.All, previewMode, out enumerable);
			}
			else if (!card.IsEnchantmentPreview)
			{
				if (enchantment != null)
				{
					num2 += enchantment.EnchantDamageAdditive(num2, this.Props);
					num2 *= enchantment.EnchantDamageMultiplicative(num2, this.Props);
				}
				base.PreviewValue = num2;
			}
			base.PreviewValue = Math.Max(base.PreviewValue, 0m);
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x002AD4DA File Offset: 0x002AB6DA
		protected override DynamicVar GetExtraVar()
		{
			return ((CardModel)this._owner).DynamicVars.ExtraDamage;
		}

		// Token: 0x04002A22 RID: 10786
		public const string defaultName = "CalculatedDamage";
	}
}
