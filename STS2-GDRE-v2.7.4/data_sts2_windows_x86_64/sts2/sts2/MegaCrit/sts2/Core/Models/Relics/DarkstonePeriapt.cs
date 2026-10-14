using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000527 RID: 1319
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DarkstonePeriapt : RelicModel
	{
		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005011 RID: 20497 RVA: 0x0023E837 File Offset: 0x0023CA37
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005012 RID: 20498 RVA: 0x0023E83A File Offset: 0x0023CA3A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(6m));
			}
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x0023E84C File Offset: 0x0023CA4C
		public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			CardPile pile = card.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				if (card.Owner == base.Owner)
				{
					if (card.Type == CardType.Curse)
					{
						base.Flash();
						await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
					}
				}
			}
		}
	}
}
