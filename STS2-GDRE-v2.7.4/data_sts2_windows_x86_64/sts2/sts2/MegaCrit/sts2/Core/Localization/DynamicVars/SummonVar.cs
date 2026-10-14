using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BF1 RID: 3057
	[NullableContext(1)]
	[Nullable(0)]
	public class SummonVar : DynamicVar
	{
		// Token: 0x06007E7D RID: 32381 RVA: 0x002AE33B File Offset: 0x002AC53B
		public SummonVar(decimal summonAmount)
			: base("Summon", summonAmount)
		{
		}

		// Token: 0x06007E7E RID: 32382 RVA: 0x002AE349 File Offset: 0x002AC549
		public SummonVar(string name, decimal summonAmount)
			: base(name, summonAmount)
		{
		}

		// Token: 0x06007E7F RID: 32383 RVA: 0x002AE353 File Offset: 0x002AC553
		public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, [Nullable(2)] Creature target, bool runGlobalHooks)
		{
			if (!runGlobalHooks)
			{
				return;
			}
			base.PreviewValue = Hook.ModifySummonAmount(card.CombatState, card.Owner, base.BaseValue, card);
		}

		// Token: 0x04002A42 RID: 10818
		public const string defaultName = "Summon";
	}
}
