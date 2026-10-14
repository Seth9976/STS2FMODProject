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
	// Token: 0x020005B6 RID: 1462
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhialHolster : RelicModel
	{
		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x060053C5 RID: 21445 RVA: 0x002455EB File Offset: 0x002437EB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x060053C6 RID: 21446 RVA: 0x002455EE File Offset: 0x002437EE
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x002455F1 File Offset: 0x002437F1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("PotionSlots", 1m),
					new DynamicVar("Potions", 2m)
				});
			}
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x00245624 File Offset: 0x00243824
		public override async Task AfterObtained()
		{
			await PlayerCmd.GainMaxPotionCount(base.DynamicVars["PotionSlots"].IntValue, base.Owner);
			IEnumerable<PotionModel> enumerable = PotionFactory.CreateRandomPotionsOutOfCombat(base.Owner, base.DynamicVars["Potions"].IntValue, base.Owner.RunState.Rng.CombatPotionGeneration, null);
			foreach (PotionModel potionModel in enumerable)
			{
				await PotionCmd.TryToProcure(potionModel.ToMutable(), base.Owner, -1);
			}
			IEnumerator<PotionModel> enumerator = null;
		}

		// Token: 0x0400249E RID: 9374
		private const string _potionSlotsKey = "PotionSlots";

		// Token: 0x0400249F RID: 9375
		private const string _potionCountKey = "Potions";
	}
}
