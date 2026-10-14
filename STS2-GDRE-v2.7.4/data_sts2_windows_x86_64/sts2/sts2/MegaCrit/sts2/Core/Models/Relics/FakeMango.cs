using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000542 RID: 1346
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeMango : RelicModel
	{
		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060050A1 RID: 20641 RVA: 0x0023F6D3 File Offset: 0x0023D8D3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060050A2 RID: 20642 RVA: 0x0023F6D6 File Offset: 0x0023D8D6
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x0023F6D9 File Offset: 0x0023D8D9
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060050A4 RID: 20644 RVA: 0x0023F6DD File Offset: 0x0023D8DD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(3m));
			}
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0023F6F0 File Offset: 0x0023D8F0
		public override async Task AfterObtained()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
		}
	}
}
