using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000526 RID: 1318
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CursedPearl : RelicModel
	{
		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600500C RID: 20492 RVA: 0x0023E7CF File Offset: 0x0023C9CF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x0023E7D2 File Offset: 0x0023C9D2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Greed>(false);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x0600500E RID: 20494 RVA: 0x0023E7DA File Offset: 0x0023C9DA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(333));
			}
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x0023E7EC File Offset: 0x0023C9EC
		public override async Task AfterObtained()
		{
			await CardPileCmd.AddCurseToDeck<Greed>(base.Owner);
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
		}
	}
}
