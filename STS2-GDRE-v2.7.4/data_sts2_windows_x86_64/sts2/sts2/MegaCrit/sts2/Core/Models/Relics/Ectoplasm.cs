using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000538 RID: 1336
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ectoplasm : RelicModel
	{
		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600505C RID: 20572 RVA: 0x0023F062 File Offset: 0x0023D262
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600505D RID: 20573 RVA: 0x0023F065 File Offset: 0x0023D265
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0023F072 File Offset: 0x0023D272
		public override decimal ModifyGoldGained(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return 0m;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0023F084 File Offset: 0x0023D284
		public override Task AfterModifyingGoldGained(Player player, decimal amount)
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06005060 RID: 20576 RVA: 0x0023F091 File Offset: 0x0023D291
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0023F09E File Offset: 0x0023D29E
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}
	}
}
