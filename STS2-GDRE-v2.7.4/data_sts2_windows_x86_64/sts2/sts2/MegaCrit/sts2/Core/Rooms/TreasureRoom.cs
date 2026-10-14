using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x02000132 RID: 306
	[NullableContext(1)]
	[Nullable(0)]
	public class TreasureRoom : AbstractRoom
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0005CB1A File Offset: 0x0005AD1A
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Treasure;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0005CB1D File Offset: 0x0005AD1D
		[Nullable(2)]
		public override ModelId ModelId
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005CB20 File Offset: 0x0005AD20
		public TreasureRoom(int actIndex)
		{
			bool flag = actIndex < 0 || actIndex > 2;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("actIndex", "must be between 0 and 2");
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0005CB54 File Offset: 0x0005AD54
		public override async Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			if (isRestoringRoomStackBase)
			{
				throw new InvalidOperationException("TreasureRoom does not support room stack reconstruction.");
			}
			if (runState != null)
			{
				await PreloadManager.LoadRoomTreasureAssets(runState.Act);
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.SetCurrentRoom(NTreasureRoom.Create(this, runState));
				}
				await Hook.AfterRoomEntered(runState, this);
				this._runState = runState;
			}
			RunManager.Instance.TreasureRoomRelicSynchronizer.BeginRelicPicking();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0005CBA7 File Offset: 0x0005ADA7
		public override Task Exit([Nullable(2)] IRunState runState)
		{
			RunManager.Instance.TreasureRoomRelicSynchronizer.OnRoomExited();
			return Task.CompletedTask;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0005CBBD File Offset: 0x0005ADBD
		public override Task Resume(AbstractRoom _, [Nullable(2)] IRunState runState)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Offer the normal treasure room rewards (gold, relics, etc.)
		/// </summary>
		/// <returns>The amount of gold given to the player.</returns>
		// Token: 0x06000EC8 RID: 3784 RVA: 0x0005CBC4 File Offset: 0x0005ADC4
		public Task<int> DoNormalRewards()
		{
			return RunManager.Instance.OneOffSynchronizer.DoLocalTreasureRoomRewards();
		}

		/// <summary>
		/// Offer extra rewards.
		/// Normally this does nothing but if extra rewards were added by a relic then this will show the rewards screen.
		/// Note that this only awaits the local player's reward task, and not everyone's.
		/// </summary>
		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005CBD8 File Offset: 0x0005ADD8
		public async Task DoExtraRewardsIfNeeded()
		{
			Task localTask = null;
			List<RewardsSet> rewards = new List<RewardsSet>();
			foreach (Player player in this._runState.Players)
			{
				List<RewardsSet> list = rewards;
				RewardsSet rewardsSet = await RewardsCmd.GenerateForRoomEnd(player, this);
				list.Add(rewardsSet);
				list = null;
			}
			IEnumerator<Player> enumerator = null;
			foreach (RewardsSet rewardsSet2 in rewards)
			{
				Task task = TaskHelper.RunSafely(rewardsSet2.Offer());
				if (LocalContext.IsMe(rewardsSet2.Player))
				{
					localTask = task;
				}
			}
			if (localTask == null)
			{
				throw new InvalidOperationException("Tried to do extra rewards, but the local player is not in the run state!");
			}
			await localTask;
		}

		// Token: 0x04000560 RID: 1376
		[Nullable(2)]
		private IRunState _runState;
	}
}
