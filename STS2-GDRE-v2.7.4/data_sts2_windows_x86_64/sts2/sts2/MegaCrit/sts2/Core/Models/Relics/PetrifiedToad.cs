using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005B5 RID: 1461
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PetrifiedToad : RelicModel
	{
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x0024558F File Offset: 0x0024378F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x00245592 File Offset: 0x00243792
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPotion<PotionShapedRock>());
			}
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x002455A0 File Offset: 0x002437A0
		public override async Task BeforeCombatStartLate()
		{
			base.Flash();
			await PotionCmd.TryToProcure<PotionShapedRock>(base.Owner);
		}
	}
}
