using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005EA RID: 1514
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpikedGauntlets : RelicModel
	{
		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x06005515 RID: 21781 RVA: 0x00247EA7 File Offset: 0x002460A7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x06005516 RID: 21782 RVA: 0x00247EAA File Offset: 0x002460AA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x06005517 RID: 21783 RVA: 0x00247EB7 File Offset: 0x002460B7
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x00247EC4 File Offset: 0x002460C4
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x00247EEC File Offset: 0x002460EC
		public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (card.Owner.Creature != base.Owner.Creature)
			{
				return false;
			}
			if (card.Type != CardType.Power)
			{
				return false;
			}
			modifiedCost = originalCost + 1m;
			return true;
		}
	}
}
