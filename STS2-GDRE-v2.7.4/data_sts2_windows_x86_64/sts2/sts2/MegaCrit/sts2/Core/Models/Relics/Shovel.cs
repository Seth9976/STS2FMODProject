using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DF RID: 1503
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shovel : RelicModel
	{
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x060054C2 RID: 21698 RVA: 0x002474AF File Offset: 0x002456AF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x002474B2 File Offset: 0x002456B2
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x002474BA File Offset: 0x002456BA
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			options.Add(new DigRestSiteOption(player));
			return true;
		}
	}
}
