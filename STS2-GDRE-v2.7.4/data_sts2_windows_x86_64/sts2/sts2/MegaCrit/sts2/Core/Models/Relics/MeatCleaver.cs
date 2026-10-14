using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000586 RID: 1414
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeatCleaver : RelicModel
	{
		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x00242CCF File Offset: 0x00240ECF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x00242CD2 File Offset: 0x00240ED2
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Cook, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00242CE5 File Offset: 0x00240EE5
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			options.Add(new CookRestSiteOption(player));
			return true;
		}
	}
}
