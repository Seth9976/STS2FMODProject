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
	/// <summary>
	/// DamageVar variant used exclusively by Osty. Utilizes buffs, debuffs on Osty like Strength and Weak
	/// </summary>
	// Token: 0x02000BEC RID: 3052
	[NullableContext(1)]
	[Nullable(0)]
	public class OstyDamageVar : DynamicVar
	{
		// Token: 0x170022EC RID: 8940
		// (get) Token: 0x06007E6D RID: 32365 RVA: 0x002AE184 File Offset: 0x002AC384
		// (set) Token: 0x06007E6E RID: 32366 RVA: 0x002AE18C File Offset: 0x002AC38C
		public ValueProp Props { get; set; }

		// Token: 0x06007E6F RID: 32367 RVA: 0x002AE195 File Offset: 0x002AC395
		public OstyDamageVar(decimal damage, ValueProp props)
			: base("OstyDamage", damage)
		{
			this.Props = props;
		}

		// Token: 0x06007E70 RID: 32368 RVA: 0x002AE1AA File Offset: 0x002AC3AA
		public OstyDamageVar(string name, decimal damage, ValueProp props)
			: base(name, damage)
		{
			this.Props = props;
		}

		// Token: 0x06007E71 RID: 32369 RVA: 0x002AE1BC File Offset: 0x002AC3BC
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
				ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
				IEnumerable<AbstractModel> enumerable;
				num = Hook.ModifyDamage(card.Owner.RunState, combatState, target, card.Owner.Osty, base.BaseValue, this.Props, card, null, ModifyDamageHookType.All, previewMode, out enumerable);
			}
			base.PreviewValue = num;
		}

		// Token: 0x04002A3D RID: 10813
		public const string defaultName = "OstyDamage";
	}
}
