using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FA RID: 1274
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BagOfPreparation : RelicModel
	{
		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004EFA RID: 20218 RVA: 0x0023C738 File Offset: 0x0023A938
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004EFB RID: 20219 RVA: 0x0023C73B File Offset: 0x0023A93B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x0023C748 File Offset: 0x0023A948
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			if (base.Owner.PlayerCombatState.TurnNumber > 1)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.BaseValue;
		}
	}
}
