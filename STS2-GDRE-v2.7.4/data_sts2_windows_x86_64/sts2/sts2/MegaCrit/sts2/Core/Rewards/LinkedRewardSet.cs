using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000147 RID: 327
	[NullableContext(1)]
	[Nullable(0)]
	public class LinkedRewardSet : Reward
	{
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x000600F7 File Offset: 0x0005E2F7
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.None;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x000600FA File Offset: 0x0005E2FA
		public override int RewardsSetIndex
		{
			get
			{
				return this.Rewards.Max<Reward>((Reward r) => r.RewardsSetIndex);
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00060126 File Offset: 0x0005E326
		private static LocString HoverTipTitle
		{
			get
			{
				return new LocString("static_hover_tips", "LINKED_REWARDS.title");
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00060137 File Offset: 0x0005E337
		private static LocString HoverTipDesc
		{
			get
			{
				return new LocString("static_hover_tips", "LINKED_REWARDS.description");
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00060148 File Offset: 0x0005E348
		public static HoverTip HoverTip
		{
			get
			{
				return new HoverTip(LinkedRewardSet.HoverTipTitle, LinkedRewardSet.HoverTipDesc, null);
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0006015A File Offset: 0x0005E35A
		public IReadOnlyList<Reward> Rewards
		{
			get
			{
				return this._rewards.ToList<Reward>();
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00060168 File Offset: 0x0005E368
		public LinkedRewardSet(List<Reward> rewards, Player player)
			: base(player)
		{
			this._rewards = rewards;
			foreach (Reward reward in this._rewards)
			{
				reward.ParentRewardSet = this;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x000601CC File Offset: 0x0005E3CC
		public override bool IsPopulated
		{
			get
			{
				return this._rewards.All<Reward>((Reward r) => r.IsPopulated);
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000601F8 File Offset: 0x0005E3F8
		public override void Populate()
		{
			foreach (Reward reward in this._rewards)
			{
				reward.Populate();
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0006024C File Offset: 0x0005E44C
		public void RemoveReward(Reward reward)
		{
			this._rewards.Remove(reward);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0006025B File Offset: 0x0005E45B
		protected override Task<bool> OnSelect()
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00060264 File Offset: 0x0005E464
		public override void OnSkipped()
		{
			foreach (Reward reward in this._rewards)
			{
				reward.OnSkipped();
			}
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000602B8 File Offset: 0x0005E4B8
		public override void MarkContentAsSeen()
		{
			foreach (Reward reward in this._rewards)
			{
				reward.MarkContentAsSeen();
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0006030C File Offset: 0x0005E50C
		public override LocString Description
		{
			get
			{
				return new LocString("gameplay_ui", "COMBAT_REWARD_LINKED");
			}
		}

		// Token: 0x04000595 RID: 1429
		private readonly List<Reward> _rewards;
	}
}
