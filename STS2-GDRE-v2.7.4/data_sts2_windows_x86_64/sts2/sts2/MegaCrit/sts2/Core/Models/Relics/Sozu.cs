using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E8 RID: 1512
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sozu : RelicModel
	{
		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x0600550A RID: 21770 RVA: 0x00247D92 File Offset: 0x00245F92
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x0600550B RID: 21771 RVA: 0x00247D95 File Offset: 0x00245F95
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x0600550C RID: 21772 RVA: 0x00247DA2 File Offset: 0x00245FA2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x00247DAF File Offset: 0x00245FAF
		public override bool ShouldProcurePotion(PotionModel potion, Player player)
		{
			return player != base.Owner;
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x00247DBD File Offset: 0x00245FBD
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}
	}
}
