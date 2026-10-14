using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x02000492 RID: 1170
	[NullableContext(1)]
	[Nullable(0)]
	public class ActChangeSynchronizer
	{
		// Token: 0x060046FC RID: 18172 RVA: 0x00219130 File Offset: 0x00217330
		public ActChangeSynchronizer(RunState runState)
		{
			this._runState = runState;
			for (int i = 0; i < runState.Players.Count; i++)
			{
				this._readyPlayers.Add(false);
			}
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00219190 File Offset: 0x00217390
		public void SetLocalPlayerReady()
		{
			this._logger.Info("Local player ready to move to next act", 1);
			Player me = LocalContext.GetMe(this._runState);
			if (me == null)
			{
				return;
			}
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new VoteToMoveToNextActAction(me, this._runState.CurrentActIndex));
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x002191E0 File Offset: 0x002173E0
		public bool IsWaitingForOtherPlayers()
		{
			int playerSlotIndex = this._runState.GetPlayerSlotIndex(LocalContext.NetId.Value);
			for (int i = 0; i < this._readyPlayers.Count; i++)
			{
				if (!this._readyPlayers[i] && i != playerSlotIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00219234 File Offset: 0x00217434
		public void OnPlayerReady(Player player, int actIndex)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" ready to move to next act from ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(actIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			AbstractRoom currentRoom = this._runState.CurrentRoom;
			if ((currentRoom == null || !currentRoom.IsVictoryRoom) && (actIndex < this._runState.CurrentActIndex || actIndex <= this._lastTransitioningActIndex))
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(120, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to transition to next act from index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(actIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but the current index is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._runState.CurrentActIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" and we last transitioned from ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._lastTransitioningActIndex);
				defaultInterpolatedStringHandler.AppendLiteral(". Ignoring.");
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			int playerSlotIndex = this._runState.GetPlayerSlotIndex(player);
			this._readyPlayers[playerSlotIndex] = true;
			if (this._readyPlayers.All<bool>((bool x) => x))
			{
				this.MoveToNextAct();
			}
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x002193A0 File Offset: 0x002175A0
		private void MoveToNextAct()
		{
			for (int i = 0; i < this._readyPlayers.Count; i++)
			{
				this._readyPlayers[i] = false;
			}
			this._logger.Info("All players ready to move to next act, beginning transition", 1);
			this._lastTransitioningActIndex = this._runState.CurrentActIndex;
			RunState runState = this._runState;
			int actFloor = runState.ActFloor;
			runState.ActFloor = actFloor + 1;
			TaskHelper.RunSafely(RunManager.Instance.EnterNextAct());
			NOverlayStack instance = NOverlayStack.Instance;
			NRewardsScreen nrewardsScreen = ((instance != null) ? instance.Peek() : null) as NRewardsScreen;
			if (nrewardsScreen != null)
			{
				nrewardsScreen.HideWaitingForPlayersScreen();
			}
		}

		// Token: 0x04001BC0 RID: 7104
		private readonly RunState _runState;

		// Token: 0x04001BC1 RID: 7105
		private readonly List<bool> _readyPlayers = new List<bool>();

		// Token: 0x04001BC2 RID: 7106
		private readonly Logger _logger = new Logger("ActChangeSynchronizer", LogType.GameSync);

		/// <summary>
		/// The last act index we transitioned away from.
		/// All attempts to vote to transition away from this act again will be ignored.
		/// </summary>
		// Token: 0x04001BC3 RID: 7107
		private int _lastTransitioningActIndex = -1;
	}
}
