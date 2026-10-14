using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C4 RID: 1476
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PumpkinCandle : RelicModel
	{
		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x0600541D RID: 21533 RVA: 0x00246013 File Offset: 0x00244213
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x0600541E RID: 21534 RVA: 0x00246016 File Offset: 0x00244216
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x0600541F RID: 21535 RVA: 0x00246019 File Offset: 0x00244219
		public override int DisplayAmount
		{
			get
			{
				return this.KindleCount;
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x00246021 File Offset: 0x00244221
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("CombatCount", 5m),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06005421 RID: 21537 RVA: 0x0024604A File Offset: 0x0024424A
		// (set) Token: 0x06005422 RID: 21538 RVA: 0x00246052 File Offset: 0x00244252
		[SavedProperty]
		public int KindleCount
		{
			get
			{
				return this._kindleCount;
			}
			set
			{
				base.AssertMutable();
				this._kindleCount = value;
				base.Status = ((this.KindleCount > 0) ? RelicStatus.Normal : RelicStatus.Disabled);
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x0024607A File Offset: 0x0024427A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x00246087 File Offset: 0x00244287
		public override Task AfterObtained()
		{
			this.Rekindle();
			return Task.CompletedTask;
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x00246094 File Offset: 0x00244294
		public override decimal ModifyMaxEnergy(Player player, decimal amount)
		{
			if (player != base.Owner)
			{
				return amount;
			}
			if (this.KindleCount <= 0)
			{
				return amount;
			}
			return amount + base.DynamicVars.Energy.IntValue;
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x002460C7 File Offset: 0x002442C7
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.KindleCount = Math.Max(this.KindleCount - 1, 0);
			return Task.CompletedTask;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x002460E2 File Offset: 0x002442E2
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			options.Add(new KindleRestSiteOption(player));
			return true;
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x002460FC File Offset: 0x002442FC
		public void Rekindle()
		{
			this.KindleCount += 5;
			base.Flash();
		}

		// Token: 0x040024A9 RID: 9385
		private const string _defaultCombatCountKey = "CombatCount";

		// Token: 0x040024AA RID: 9386
		public const int kindleAmount = 5;

		// Token: 0x040024AB RID: 9387
		private int _kindleCount;
	}
}
