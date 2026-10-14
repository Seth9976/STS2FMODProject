using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200061B RID: 1563
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WongosMysteryTicket : RelicModel
	{
		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06005650 RID: 22096 RVA: 0x0024A4E1 File Offset: 0x002486E1
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06005651 RID: 22097 RVA: 0x0024A4E4 File Offset: 0x002486E4
		public override bool IsUsedUp
		{
			get
			{
				return this.GaveRelic;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x06005652 RID: 22098 RVA: 0x0024A4EC File Offset: 0x002486EC
		public override bool ShowCounter
		{
			get
			{
				return this.DisplayAmount > 0;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x06005653 RID: 22099 RVA: 0x0024A4F7 File Offset: 0x002486F7
		public override int DisplayAmount
		{
			get
			{
				return 5 - this.CombatsFinished;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06005654 RID: 22100 RVA: 0x0024A501 File Offset: 0x00248701
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new RepeatVar(3),
					new DynamicVar("RemainingCombats", 5m)
				});
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06005655 RID: 22101 RVA: 0x0024A52A File Offset: 0x0024872A
		// (set) Token: 0x06005656 RID: 22102 RVA: 0x0024A532 File Offset: 0x00248732
		[SavedProperty]
		public int CombatsFinished
		{
			get
			{
				return this._combatsFinished;
			}
			set
			{
				base.AssertMutable();
				this._combatsFinished = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06005657 RID: 22103 RVA: 0x0024A547 File Offset: 0x00248747
		// (set) Token: 0x06005658 RID: 22104 RVA: 0x0024A54F File Offset: 0x0024874F
		[SavedProperty]
		public bool GaveRelic
		{
			get
			{
				return this._gaveRelic;
			}
			set
			{
				base.AssertMutable();
				this._gaveRelic = value;
				base.InvokeDisplayAmountChanged();
				if (this._gaveRelic)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x0024A574 File Offset: 0x00248774
		public override Task AfterCombatEnd(CombatRoom _)
		{
			int combatsFinished = this.CombatsFinished;
			this.CombatsFinished = combatsFinished + 1;
			int num = 5 - this.CombatsFinished;
			base.DynamicVars["RemainingCombats"].BaseValue = decimal.Max(num, 0m);
			return Task.CompletedTask;
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x0024A5C4 File Offset: 0x002487C4
		public override bool TryModifyRewards(Player player, List<Reward> rewards, [Nullable(2)] AbstractRoom room)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (!(room is CombatRoom))
			{
				return false;
			}
			if (this.GaveRelic)
			{
				return false;
			}
			int num = 5 - this.CombatsFinished;
			if (num > 0)
			{
				return false;
			}
			for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
			{
				rewards.Add(new RelicReward(player));
			}
			return true;
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x0024A627 File Offset: 0x00248827
		public override Task AfterModifyingRewards()
		{
			base.Flash();
			this.GaveRelic = true;
			return Task.CompletedTask;
		}

		// Token: 0x040024E9 RID: 9449
		private const string _remainingCombatsKey = "RemainingCombats";

		// Token: 0x040024EA RID: 9450
		public const int combatsToActivate = 5;

		// Token: 0x040024EB RID: 9451
		public const int relicCount = 3;

		// Token: 0x040024EC RID: 9452
		private int _combatsFinished;

		// Token: 0x040024ED RID: 9453
		private bool _gaveRelic;
	}
}
