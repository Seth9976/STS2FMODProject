using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200051E RID: 1310
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ChemicalX : RelicModel
	{
		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x0023E3B3 File Offset: 0x0023C5B3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x0023E3B6 File Offset: 0x0023C5B6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Increase", 2m));
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x0023E3D0 File Offset: 0x0023C5D0
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!cardPlay.Card.EnergyCost.CostsX && !cardPlay.Card.HasStarCostX)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x0023E426 File Offset: 0x0023C626
		public override int ModifyXValue(CardModel card, int originalValue)
		{
			if (base.Owner != card.Owner)
			{
				return originalValue;
			}
			return originalValue + base.DynamicVars["Increase"].IntValue;
		}

		// Token: 0x04002439 RID: 9273
		private const string _increaseKey = "Increase";
	}
}
