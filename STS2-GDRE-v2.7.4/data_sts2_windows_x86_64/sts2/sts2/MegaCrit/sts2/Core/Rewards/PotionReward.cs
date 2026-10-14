using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000148 RID: 328
	[NullableContext(2)]
	[Nullable(0)]
	public class PotionReward : Reward
	{
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0006031D File Offset: 0x0005E51D
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.Potion;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00060320 File Offset: 0x0005E520
		public override int RewardsSetIndex
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00060323 File Offset: 0x0005E523
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0006032B File Offset: 0x0005E52B
		public PotionModel Potion { get; private set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00060334 File Offset: 0x0005E534
		public override Vector2 IconPosition
		{
			get
			{
				return new Vector2(0f, -2f);
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00060345 File Offset: 0x0005E545
		[Nullable(1)]
		public override LocString Description
		{
			[NullableContext(1)]
			get
			{
				return this.Potion.Title;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00060352 File Offset: 0x0005E552
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0006035A File Offset: 0x0005E55A
		public PotionModel ClaimedPotion { get; private set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00060363 File Offset: 0x0005E563
		[Nullable(1)]
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			[NullableContext(1)]
			get
			{
				return this.Potion.HoverTips;
			}
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00060370 File Offset: 0x0005E570
		[NullableContext(1)]
		public PotionReward(Player player)
			: base(player)
		{
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00060379 File Offset: 0x0005E579
		[NullableContext(1)]
		public PotionReward(PotionModel potion, Player player)
			: base(player)
		{
			potion.AssertMutable();
			this.Potion = potion;
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0006038F File Offset: 0x0005E58F
		public override bool IsPopulated
		{
			get
			{
				return this.Potion != null;
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0006039C File Offset: 0x0005E59C
		public override void Populate()
		{
			Rng rng = this._rngOverride ?? base.Player.PlayerRng.Rewards;
			if (this.Potion == null)
			{
				this.Potion = PotionFactory.CreateRandomPotionOutOfCombat(base.Player, rng, null).ToMutable();
			}
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000603E6 File Offset: 0x0005E5E6
		public override Control CreateIcon()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (this._icon == null)
			{
				this._icon = NPotion.Create(this.Potion);
			}
			return this._icon;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00060410 File Offset: 0x0005E610
		[NullableContext(1)]
		protected override async Task<bool> OnSelect()
		{
			PotionProcureResult potionProcureResult = await PotionCmd.TryToProcure(this.Potion, base.Player, -1);
			PotionProcureResult potionProcureResult2 = potionProcureResult;
			bool flag;
			if (potionProcureResult2.success)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(potionProcureResult2.potion.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" from potion reward");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.ClaimedPotion = this.Potion;
				this._wasTaken = true;
				flag = true;
			}
			else if (potionProcureResult2.failureReason == PotionProcureFailureReason.TooFull)
			{
				flag = false;
			}
			else
			{
				this.ClaimedPotion = this.Potion;
				this._wasTaken = true;
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00060454 File Offset: 0x0005E654
		public override void OnSkipped()
		{
			if (this._wasTaken)
			{
				return;
			}
			base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).PotionChoices.Add(new ModelChoiceHistoryEntry(this.Potion.Id, false));
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000604A5 File Offset: 0x0005E6A5
		public override void MarkContentAsSeen()
		{
			SaveManager.Instance.MarkPotionAsSeen(this.Potion);
		}

		// Token: 0x04000597 RID: 1431
		private bool _wasTaken;

		// Token: 0x04000599 RID: 1433
		private Control _icon;
	}
}
