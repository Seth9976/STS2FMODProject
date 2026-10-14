using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000506 RID: 1286
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BloodSoakedRose : RelicModel
	{
		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004F43 RID: 20291 RVA: 0x0023D01B File Offset: 0x0023B21B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004F44 RID: 20292 RVA: 0x0023D01E File Offset: 0x0023B21E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004F45 RID: 20293 RVA: 0x0023D021 File Offset: 0x0023B221
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.ForEnergy(this));
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Enthralled>(false));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06004F46 RID: 20294 RVA: 0x0023D045 File Offset: 0x0023B245
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x0023D054 File Offset: 0x0023B254
		public override async Task AfterObtained()
		{
			await CardPileCmd.AddCurseToDeck<Enthralled>(base.Owner);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x0023D097 File Offset: 0x0023B297
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.BaseValue;
		}
	}
}
