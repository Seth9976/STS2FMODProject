using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200059D RID: 1437
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OldCoin : RelicModel
	{
		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060052F2 RID: 21234 RVA: 0x00243D67 File Offset: 0x00241F67
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x00243D6A File Offset: 0x00241F6A
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060052F4 RID: 21236 RVA: 0x00243D72 File Offset: 0x00241F72
		public override bool IsAllowedInShops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x00243D75 File Offset: 0x00241F75
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x060052F6 RID: 21238 RVA: 0x00243D78 File Offset: 0x00241F78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(300));
			}
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x00243D8C File Offset: 0x00241F8C
		public override async Task AfterObtained()
		{
			await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
		}
	}
}
