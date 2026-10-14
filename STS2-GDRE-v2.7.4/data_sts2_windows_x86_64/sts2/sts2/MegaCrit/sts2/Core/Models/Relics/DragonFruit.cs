using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000534 RID: 1332
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DragonFruit : RelicModel
	{
		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005048 RID: 20552 RVA: 0x0023ED73 File Offset: 0x0023CF73
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x0023ED76 File Offset: 0x0023CF76
		public override bool IsAllowed(IRunState runState)
		{
			return RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x0600504A RID: 20554 RVA: 0x0023ED7E File Offset: 0x0023CF7E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(1m));
			}
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x0023ED90 File Offset: 0x0023CF90
		public override async Task AfterGoldGained(Player player)
		{
			if (player == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			}
		}
	}
}
