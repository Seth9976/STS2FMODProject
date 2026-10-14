using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005AB RID: 1451
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsWing : RelicModel
	{
		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06005376 RID: 21366 RVA: 0x00244D33 File Offset: 0x00242F33
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06005377 RID: 21367 RVA: 0x00244D36 File Offset: 0x00242F36
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06005378 RID: 21368 RVA: 0x00244D39 File Offset: 0x00242F39
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.RewardsSacrificed % base.DynamicVars["Sacrifices"].IntValue;
				}
				return base.DynamicVars["Sacrifices"].IntValue;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06005379 RID: 21369 RVA: 0x00244D75 File Offset: 0x00242F75
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Sacrifices", 2m));
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x0600537A RID: 21370 RVA: 0x00244D8C File Offset: 0x00242F8C
		// (set) Token: 0x0600537B RID: 21371 RVA: 0x00244D94 File Offset: 0x00242F94
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

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x0600537C RID: 21372 RVA: 0x00244DA9 File Offset: 0x00242FA9
		// (set) Token: 0x0600537D RID: 21373 RVA: 0x00244DB1 File Offset: 0x00242FB1
		[SavedProperty]
		public int RewardsSacrificed
		{
			get
			{
				return this._rewardsSacrificed;
			}
			set
			{
				base.AssertMutable();
				this._rewardsSacrificed = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x00244DC6 File Offset: 0x00242FC6
		public override bool TryModifyCardRewardAlternatives(Player player, CardReward cardReward, List<CardRewardAlternative> alternatives)
		{
			if (base.Owner != player)
			{
				return false;
			}
			alternatives.Add(new CardRewardAlternative("SACRIFICE", new Func<Task>(this.OnSacrifice), PostAlternateCardRewardAction.EndSelectionAndCompleteReward));
			return true;
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x00244DF4 File Offset: 0x00242FF4
		public async Task OnSacrifice()
		{
			int rewardsSacrificed = this.RewardsSacrificed;
			this.RewardsSacrificed = rewardsSacrificed + 1;
			base.Flash();
			if (this.RewardsSacrificed % base.DynamicVars["Sacrifices"].IntValue == 0)
			{
				TaskHelper.RunSafely(this.DoActivateVisuals());
				await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable(), base.Owner, -1);
			}
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x00244E38 File Offset: 0x00243038
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x04002492 RID: 9362
		public const string sacrificeAlternativeKey = "SACRIFICE";

		// Token: 0x04002493 RID: 9363
		private const string _sacrificesKey = "Sacrifices";

		// Token: 0x04002494 RID: 9364
		private bool _isActivating;

		// Token: 0x04002495 RID: 9365
		private int _rewardsSacrificed;
	}
}
