using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BED RID: 3053
	[NullableContext(1)]
	[Nullable(0)]
	public class PowerVar<[Nullable(0)] T> : DynamicVar where T : PowerModel
	{
		// Token: 0x06007E72 RID: 32370 RVA: 0x002AE268 File Offset: 0x002AC468
		public PowerVar(decimal powerAmount)
			: base(typeof(T).Name, powerAmount)
		{
		}

		// Token: 0x06007E73 RID: 32371 RVA: 0x002AE280 File Offset: 0x002AC480
		public PowerVar(string name, decimal powerAmount)
			: base(name, powerAmount)
		{
		}

		// Token: 0x06007E74 RID: 32372 RVA: 0x002AE28C File Offset: 0x002AC48C
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			if (!runGlobalHooks)
			{
				return;
			}
			IEnumerable<AbstractModel> enumerable;
			base.PreviewValue = Hook.ModifyPowerAmountGiven(card.CombatState, ModelDb.Power<T>(), card.Owner.Creature, base.BaseValue, target, card, out enumerable);
		}
	}
}
