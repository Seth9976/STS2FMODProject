using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200055C RID: 1372
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoldPlatedCables : RelicModel
	{
		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x00240BD3 File Offset: 0x0023EDD3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00240BD6 File Offset: 0x0023EDD6
		public override int ModifyOrbPassiveTriggerCounts(OrbModel orb, int triggerCount)
		{
			if (orb.Owner != base.Owner)
			{
				return triggerCount;
			}
			if (orb != base.Owner.PlayerCombatState.OrbQueue.Orbs[0])
			{
				return triggerCount;
			}
			return triggerCount + 1;
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x00240C0B File Offset: 0x0023EE0B
		public override Task AfterModifyingOrbPassiveTriggerCount(OrbModel orb)
		{
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
