using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x02000144 RID: 324
	[NullableContext(1)]
	[Nullable(0)]
	public class CardRemovalReward : Reward
	{
		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0005F819 File Offset: 0x0005DA19
		private static string RewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card_removal.png");
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0005F825 File Offset: 0x0005DA25
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.RemoveCard;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0005F828 File Offset: 0x0005DA28
		public override int RewardsSetIndex
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0005F82B File Offset: 0x0005DA2B
		protected override string IconPath
		{
			get
			{
				return CardRemovalReward.RewardIcon;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0005F832 File Offset: 0x0005DA32
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(CardRemovalReward.RewardIcon);
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0005F83E File Offset: 0x0005DA3E
		public override bool IsPopulated
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0005F841 File Offset: 0x0005DA41
		public override LocString Description
		{
			get
			{
				return new LocString("gameplay_ui", "COMBAT_REWARD_CARD_REMOVAL");
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005F852 File Offset: 0x0005DA52
		public CardRemovalReward(Player player)
			: base(player)
		{
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005F85B File Offset: 0x0005DA5B
		public override void Populate()
		{
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0005F860 File Offset: 0x0005DA60
		protected override async Task<bool> OnSelect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" obtained card removal from reward");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return await RunManager.Instance.RewardSynchronizer.DoUnsyncedCardRemoval(base.Player);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005F8A3 File Offset: 0x0005DAA3
		public override void MarkContentAsSeen()
		{
		}
	}
}
