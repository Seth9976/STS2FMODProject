using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000580 RID: 1408
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LuckyFysh : RelicModel
	{
		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600524B RID: 21067 RVA: 0x002429F3 File Offset: 0x00240BF3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x002429F6 File Offset: 0x00240BF6
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x0600524D RID: 21069 RVA: 0x002429FE File Offset: 0x00240BFE
		public override bool IsAllowedInShops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x0600524E RID: 21070 RVA: 0x00242A01 File Offset: 0x00240C01
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(15));
			}
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00242A10 File Offset: 0x00240C10
		public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			CardPile pile = card.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				if (card.Owner == base.Owner)
				{
					base.Flash();
					await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
				}
			}
		}
	}
}
