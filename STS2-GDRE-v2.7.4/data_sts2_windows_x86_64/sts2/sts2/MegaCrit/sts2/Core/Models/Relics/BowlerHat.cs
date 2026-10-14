using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200050F RID: 1295
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowlerHat : RelicModel
	{
		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06004F80 RID: 20352 RVA: 0x0023D7DF File Offset: 0x0023B9DF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06004F81 RID: 20353 RVA: 0x0023D7E2 File Offset: 0x0023B9E2
		public override bool IsAllowedInShops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x0023D7E5 File Offset: 0x0023B9E5
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x0023D7ED File Offset: 0x0023B9ED
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("GoldIncrease", 1.25m));
			}
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x0023D809 File Offset: 0x0023BA09
		public override decimal ModifyGoldGained(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount * base.DynamicVars["GoldIncrease"].BaseValue;
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x0023D831 File Offset: 0x0023BA31
		public override Task AfterModifyingGoldGained(Player player, decimal amount)
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x0400242B RID: 9259
		private const string _goldIncreaseKey = "GoldIncrease";
	}
}
