using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200051A RID: 1306
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Cauldron : RelicModel
	{
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x0023E10B File Offset: 0x0023C30B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x0023E10E File Offset: 0x0023C30E
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x0023E111 File Offset: 0x0023C311
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Potions", 5m));
			}
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x0023E128 File Offset: 0x0023C328
		public override async Task AfterObtained()
		{
			await RewardsCmd.OfferCustom(base.Owner, this.GenerateRewards());
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x0023E16C File Offset: 0x0023C36C
		private List<Reward> GenerateRewards()
		{
			int intValue = base.DynamicVars["Potions"].IntValue;
			List<Reward> list = new List<Reward>();
			if (TestMode.IsOn)
			{
				for (int i = 0; i < intValue; i++)
				{
					list.Add(new PotionReward(Cauldron._testPotions[i % intValue].ToMutable(), base.Owner));
				}
			}
			else
			{
				for (int j = 0; j < intValue; j++)
				{
					list.Add(new PotionReward(base.Owner));
				}
			}
			return list;
		}

		// Token: 0x04002435 RID: 9269
		private const string _potionsKey = "Potions";

		// Token: 0x04002436 RID: 9270
		private static readonly PotionModel[] _testPotions = new PotionModel[]
		{
			ModelDb.Potion<FlexPotion>(),
			ModelDb.Potion<WeakPotion>(),
			ModelDb.Potion<VulnerablePotion>(),
			ModelDb.Potion<StrengthPotion>(),
			ModelDb.Potion<DexterityPotion>()
		};
	}
}
