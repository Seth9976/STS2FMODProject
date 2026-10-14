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
	// Token: 0x02000BD8 RID: 3032
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockVar : DynamicVar
	{
		// Token: 0x170022C2 RID: 8898
		// (get) Token: 0x06007DE4 RID: 32228 RVA: 0x002AD1B4 File Offset: 0x002AB3B4
		public ValueProp Props { get; }

		// Token: 0x06007DE5 RID: 32229 RVA: 0x002AD1BC File Offset: 0x002AB3BC
		public BlockVar(decimal block, ValueProp props)
			: base("Block", block)
		{
			this.Props = props;
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x002AD1D1 File Offset: 0x002AB3D1
		public BlockVar(string name, decimal block, ValueProp props)
			: base(name, block)
		{
			this.Props = props;
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x002AD1E4 File Offset: 0x002AB3E4
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			decimal num = base.BaseValue;
			EnchantmentModel enchantment = card.Enchantment;
			if (enchantment != null)
			{
				num += enchantment.EnchantBlockAdditive(num);
				num *= enchantment.EnchantBlockMultiplicative(num);
				if (!card.IsEnchantmentPreview)
				{
					base.EnchantedValue = num;
				}
			}
			if (runGlobalHooks)
			{
				IEnumerable<AbstractModel> enumerable;
				num = Hook.ModifyBlock(card.CombatState, card.Owner.Creature, base.BaseValue, this.Props, card, null, out enumerable);
			}
			base.PreviewValue = num;
		}

		// Token: 0x04002A1E RID: 10782
		public const string defaultName = "Block";
	}
}
