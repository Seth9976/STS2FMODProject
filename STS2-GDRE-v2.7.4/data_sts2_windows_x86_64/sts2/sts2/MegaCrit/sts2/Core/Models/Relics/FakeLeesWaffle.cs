using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000541 RID: 1345
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeLeesWaffle : RelicModel
	{
		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x0600509B RID: 20635 RVA: 0x0023F669 File Offset: 0x0023D869
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x0600509C RID: 20636 RVA: 0x0023F66C File Offset: 0x0023D86C
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x0600509D RID: 20637 RVA: 0x0023F66F File Offset: 0x0023D86F
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x0600509E RID: 20638 RVA: 0x0023F673 File Offset: 0x0023D873
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(10m));
			}
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x0023F688 File Offset: 0x0023D888
		public override async Task AfterObtained()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.Owner.Creature.MaxHp * (base.DynamicVars.Heal.BaseValue / 100m), true);
		}
	}
}
