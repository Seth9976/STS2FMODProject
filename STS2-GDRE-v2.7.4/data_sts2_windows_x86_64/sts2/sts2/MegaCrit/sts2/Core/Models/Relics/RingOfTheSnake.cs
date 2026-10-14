using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D0 RID: 1488
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RingOfTheSnake : RelicModel
	{
		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x0600546E RID: 21614 RVA: 0x002468F5 File Offset: 0x00244AF5
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x0600546F RID: 21615 RVA: 0x002468F8 File Offset: 0x00244AF8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x00246905 File Offset: 0x00244B05
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
