using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000602 RID: 1538
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToyBox : RelicModel
	{
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x060055A5 RID: 21925 RVA: 0x0024901D File Offset: 0x0024721D
		public static LocString WaxRelicPrefix
		{
			get
			{
				return new LocString("relics", "TOY_BOX.waxRelicPrefix");
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x060055A6 RID: 21926 RVA: 0x0024902E File Offset: 0x0024722E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x060055A7 RID: 21927 RVA: 0x00249031 File Offset: 0x00247231
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060055A8 RID: 21928 RVA: 0x00249034 File Offset: 0x00247234
		public override bool IsUsedUp
		{
			get
			{
				return this.CombatsSeen >= base.DynamicVars["Combats"].IntValue * base.DynamicVars["Relics"].IntValue;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060055A9 RID: 21929 RVA: 0x0024906C File Offset: 0x0024726C
		public override bool ShowCounter
		{
			get
			{
				return !this.IsUsedUp;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060055AA RID: 21930 RVA: 0x00249077 File Offset: 0x00247277
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.CombatsSeen % base.DynamicVars["Combats"].IntValue;
				}
				return base.DynamicVars["Combats"].IntValue;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060055AB RID: 21931 RVA: 0x002490B3 File Offset: 0x002472B3
		// (set) Token: 0x060055AC RID: 21932 RVA: 0x002490BB File Offset: 0x002472BB
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060055AD RID: 21933 RVA: 0x002490D0 File Offset: 0x002472D0
		// (set) Token: 0x060055AE RID: 21934 RVA: 0x002490D8 File Offset: 0x002472D8
		[SavedProperty]
		public int CombatsSeen
		{
			get
			{
				return this._combatsSeen;
			}
			set
			{
				base.AssertMutable();
				this._combatsSeen = value;
				base.InvokeDisplayAmountChanged();
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x060055AF RID: 21935 RVA: 0x002490FC File Offset: 0x002472FC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Relics", 5m),
					new DynamicVar("Combats", 3m)
				});
			}
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x00249130 File Offset: 0x00247330
		public override async Task AfterObtained()
		{
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < base.DynamicVars["Relics"].IntValue; i++)
			{
				RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
				relicModel.IsWax = true;
				list.Add(new RelicReward(relicModel, base.Owner));
			}
			await RewardsCmd.OfferCustom(base.Owner, list);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x00249174 File Offset: 0x00247374
		public override async Task AfterCombatEnd(CombatRoom __)
		{
			if (!this.IsUsedUp)
			{
				int combatsSeen = this.CombatsSeen;
				this.CombatsSeen = combatsSeen + 1;
				if (this.CombatsSeen % base.DynamicVars["Combats"].IntValue == 0)
				{
					TaskHelper.RunSafely(this.DoActivateVisuals());
					RelicModel relicModel = base.Owner.Relics.FirstOrDefault<RelicModel>((RelicModel r) => r != null && r.IsWax && !r.IsMelted);
					if (relicModel != null)
					{
						await RelicCmd.Melt(relicModel);
						await Cmd.CustomScaledWait(0.5f, 0.75f, false, default(CancellationToken));
					}
				}
			}
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x002491B8 File Offset: 0x002473B8
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x040024D5 RID: 9429
		private const string _relicsKey = "Relics";

		// Token: 0x040024D6 RID: 9430
		private const string _combatsKey = "Combats";

		// Token: 0x040024D7 RID: 9431
		private bool _isActivating;

		// Token: 0x040024D8 RID: 9432
		private int _combatsSeen;
	}
}
