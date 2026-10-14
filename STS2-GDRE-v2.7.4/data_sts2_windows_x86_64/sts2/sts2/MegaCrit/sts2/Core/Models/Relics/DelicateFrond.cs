using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200052A RID: 1322
	public sealed class DelicateFrond : RelicModel
	{
		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x0023E98F File Offset: 0x0023CB8F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x0023E994 File Offset: 0x0023CB94
		[NullableContext(1)]
		public override async Task BeforeCombatStart()
		{
			base.Flash();
			while (base.Owner.HasOpenPotionSlots)
			{
				PotionModel potionModel = PotionFactory.CreateRandomPotionOutOfCombat(base.Owner, base.Owner.RunState.Rng.CombatPotionGeneration, null).ToMutable();
				PotionProcureResult potionProcureResult = await PotionCmd.TryToProcure(potionModel, base.Owner, -1);
				PotionProcureResult potionProcureResult2 = potionProcureResult;
				if (!potionProcureResult2.success)
				{
					break;
				}
			}
		}
	}
}
