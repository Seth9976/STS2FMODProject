using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000656 RID: 1622
	public sealed class CuriousPower : PowerModel
	{
		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06005798 RID: 22424 RVA: 0x0024D1E2 File Offset: 0x0024B3E2
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06005799 RID: 22425 RVA: 0x0024D1E5 File Offset: 0x0024B3E5
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x0024D1E8 File Offset: 0x0024B3E8
		[NullableContext(1)]
		public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (card.Owner.Creature != base.Owner)
			{
				return false;
			}
			if (card.Type != CardType.Power)
			{
				return false;
			}
			if (originalCost <= 0m)
			{
				return false;
			}
			modifiedCost = originalCost - base.Amount;
			if (modifiedCost < 0m)
			{
				modifiedCost = 0m;
			}
			return true;
		}
	}
}
