using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	/// <summary>
	/// A special version of <see cref="T:MegaCrit.Sts2.Core.Localization.DynamicVars.DamageVar" /> that is used to represent extra damage done by a calculated damage card.
	/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PerfectedStrike" /> uses this for its 2 extra damage done for each Strike.
	/// </summary>
	// Token: 0x02000BE4 RID: 3044
	[NullableContext(1)]
	[Nullable(0)]
	public class ExtraDamageVar : DynamicVar
	{
		// Token: 0x170022EB RID: 8939
		// (get) Token: 0x06007E5B RID: 32347 RVA: 0x002AE062 File Offset: 0x002AC262
		// (set) Token: 0x06007E5C RID: 32348 RVA: 0x002AE06A File Offset: 0x002AC26A
		public bool IsFromOsty { get; private set; }

		/// <summary>
		/// Set this damage to come from Osty.
		/// </summary>
		// Token: 0x06007E5D RID: 32349 RVA: 0x002AE073 File Offset: 0x002AC273
		public ExtraDamageVar FromOsty()
		{
			this.IsFromOsty = true;
			return this;
		}

		// Token: 0x06007E5E RID: 32350 RVA: 0x002AE07D File Offset: 0x002AC27D
		public ExtraDamageVar(decimal damage)
			: base("ExtraDamage", damage)
		{
		}

		// Token: 0x06007E5F RID: 32351 RVA: 0x002AE08C File Offset: 0x002AC28C
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			decimal num = base.BaseValue;
			EnchantmentModel enchantment = card.Enchantment;
			if (enchantment != null)
			{
				num *= enchantment.EnchantDamageMultiplicative(num, ValueProp.Move);
				if (!card.IsEnchantmentPreview)
				{
					base.EnchantedValue = num;
				}
			}
			base.PreviewValue = num;
		}

		// Token: 0x04002A34 RID: 10804
		public const string defaultName = "ExtraDamage";
	}
}
