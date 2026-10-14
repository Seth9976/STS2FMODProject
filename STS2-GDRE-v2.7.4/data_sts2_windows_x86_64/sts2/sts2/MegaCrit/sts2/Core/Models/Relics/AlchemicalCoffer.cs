using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F2 RID: 1266
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AlchemicalCoffer : RelicModel
	{
		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x0023BFAB File Offset: 0x0023A1AB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004EBF RID: 20159 RVA: 0x0023BFAE File Offset: 0x0023A1AE
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x0023BFB1 File Offset: 0x0023A1B1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("PotionSlots", 4m));
			}
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x0023BFC8 File Offset: 0x0023A1C8
		public override async Task AfterObtained()
		{
			int originalSlotCount = base.Owner.MaxPotionCount;
			await PlayerCmd.GainMaxPotionCount(base.DynamicVars["PotionSlots"].IntValue, base.Owner);
			IEnumerable<PotionModel> enumerable = PotionFactory.CreateRandomPotionsOutOfCombat(base.Owner, base.DynamicVars["PotionSlots"].IntValue, base.Owner.RunState.Rng.CombatPotionGeneration, null);
			int i = 0;
			foreach (PotionModel potionModel in enumerable)
			{
				await PotionCmd.TryToProcure(potionModel.ToMutable(), base.Owner, originalSlotCount + i);
				i++;
			}
			IEnumerator<PotionModel> enumerator = null;
		}

		// Token: 0x0400241A RID: 9242
		private const string _potionSlotsKey = "PotionSlots";
	}
}
