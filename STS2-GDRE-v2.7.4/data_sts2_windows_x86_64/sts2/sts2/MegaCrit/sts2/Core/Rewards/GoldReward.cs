using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000146 RID: 326
	[NullableContext(1)]
	[Nullable(0)]
	public class GoldReward : Reward
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0005FF5B File Offset: 0x0005E15B
		private static string RewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_money.png");
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0005FF67 File Offset: 0x0005E167
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(GoldReward.RewardIcon);
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0005FF73 File Offset: 0x0005E173
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.Gold;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0005FF76 File Offset: 0x0005E176
		public override int RewardsSetIndex
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0005FF79 File Offset: 0x0005E179
		protected override string IconPath
		{
			get
			{
				return GoldReward.RewardIcon;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0005FF80 File Offset: 0x0005E180
		public override Vector2 IconPosition
		{
			get
			{
				return new Vector2(0f, -5f);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0005FF91 File Offset: 0x0005E191
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x0005FF99 File Offset: 0x0005E199
		public int Amount { get; private set; } = -1;

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0005FFA4 File Offset: 0x0005E1A4
		public override LocString Description
		{
			get
			{
				LocString locString = new LocString("gameplay_ui", this._wasGoldStolenBack ? "COMBAT_REWARD_GOLD_STOLEN" : "COMBAT_REWARD_GOLD");
				locString.Add("gold", this.Amount);
				return locString;
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0005FFE7 File Offset: 0x0005E1E7
		public GoldReward(int amount, Player player, bool wasGoldStolenBack = false)
			: base(player)
		{
			this._min = amount;
			this._max = amount;
			this.Amount = amount;
			this._wasGoldStolenBack = wasGoldStolenBack;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00060013 File Offset: 0x0005E213
		public GoldReward(int min, int max, Player player, bool wasGoldStolenBack = false)
			: base(player)
		{
			this._min = min;
			this._max = max;
			this._wasGoldStolenBack = wasGoldStolenBack;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00060039 File Offset: 0x0005E239
		public override bool IsPopulated
		{
			get
			{
				return this.Amount >= 0;
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00060048 File Offset: 0x0005E248
		public override void Populate()
		{
			Rng rng = this._rngOverride ?? base.Player.PlayerRng.Rewards;
			this.Amount = rng.NextInt(this._min, this._max + 1);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0006008C File Offset: 0x0005E28C
		protected override async Task<bool> OnSelect()
		{
			await PlayerCmd.GainGold(this.Amount, base.Player, this._wasGoldStolenBack);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Amount);
			defaultInterpolatedStringHandler.AppendLiteral(" gold from reward");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return true;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x000600CF File Offset: 0x0005E2CF
		public override SerializableReward ToSerializable()
		{
			return new SerializableReward
			{
				RewardType = RewardType.Gold,
				GoldAmount = this.Amount,
				WasGoldStolenBack = this._wasGoldStolenBack
			};
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x000600F5 File Offset: 0x0005E2F5
		public override void MarkContentAsSeen()
		{
		}

		// Token: 0x0400058F RID: 1423
		public const int defaultMinGoldAmount = 10;

		// Token: 0x04000590 RID: 1424
		public const int defaultMaxGoldAmount = 20;

		// Token: 0x04000592 RID: 1426
		private readonly bool _wasGoldStolenBack;

		// Token: 0x04000593 RID: 1427
		private readonly int _min;

		// Token: 0x04000594 RID: 1428
		private readonly int _max;
	}
}
