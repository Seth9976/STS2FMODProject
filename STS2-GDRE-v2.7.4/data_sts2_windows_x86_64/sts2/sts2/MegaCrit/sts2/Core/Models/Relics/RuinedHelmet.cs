using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D4 RID: 1492
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RuinedHelmet : RelicModel
	{
		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06005485 RID: 21637 RVA: 0x00246B7F File Offset: 0x00244D7F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06005486 RID: 21638 RVA: 0x00246B82 File Offset: 0x00244D82
		// (set) Token: 0x06005487 RID: 21639 RVA: 0x00246B8A File Offset: 0x00244D8A
		private bool UsedThisCombat
		{
			get
			{
				return this._usedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._usedThisCombat = value;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06005488 RID: 21640 RVA: 0x00246B9C File Offset: 0x00244D9C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x00246BBC File Offset: 0x00244DBC
		public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, [Nullable(2)] Creature applier, out decimal modifiedAmount)
		{
			modifiedAmount = amount;
			if (!(canonicalPower is StrengthPower))
			{
				return false;
			}
			if (target != base.Owner.Creature)
			{
				return false;
			}
			if (amount <= 0m)
			{
				return false;
			}
			if (this.UsedThisCombat)
			{
				return false;
			}
			modifiedAmount *= 2m;
			return true;
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x00246C1E File Offset: 0x00244E1E
		public override Task AfterModifyingPowerAmountReceived(PowerModel power)
		{
			base.Flash();
			this.UsedThisCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x00246C32 File Offset: 0x00244E32
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.UsedThisCombat = false;
			return Task.CompletedTask;
		}

		// Token: 0x040024B4 RID: 9396
		private bool _usedThisCombat;
	}
}
