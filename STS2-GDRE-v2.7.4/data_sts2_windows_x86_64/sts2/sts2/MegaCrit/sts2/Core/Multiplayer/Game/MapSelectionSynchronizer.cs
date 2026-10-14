using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x0200049D RID: 1181
	[NullableContext(1)]
	[Nullable(0)]
	public class MapSelectionSynchronizer
	{
		/// <summary>
		/// Each time we generate a map, this counter goes up. It is used to keep track of multiplayer map votes and to
		/// discard map votes that are for old maps.
		/// </summary>
		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x0600476F RID: 18287 RVA: 0x0021BF76 File Offset: 0x0021A176
		// (set) Token: 0x06004770 RID: 18288 RVA: 0x0021BF7E File Offset: 0x0021A17E
		public int MapGenerationCount { get; private set; }

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06004771 RID: 18289 RVA: 0x0021BF88 File Offset: 0x0021A188
		// (remove) Token: 0x06004772 RID: 18290 RVA: 0x0021BFC0 File Offset: 0x0021A1C0
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player, MapVote?, MapVote?> PlayerVoteChanged;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06004773 RID: 18291 RVA: 0x0021BFF8 File Offset: 0x0021A1F8
		// (remove) Token: 0x06004774 RID: 18292 RVA: 0x0021C030 File Offset: 0x0021A230
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player> PlayerVoteCancelled;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06004775 RID: 18293 RVA: 0x0021C068 File Offset: 0x0021A268
		// (remove) Token: 0x06004776 RID: 18294 RVA: 0x0021C0A0 File Offset: 0x0021A2A0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action PlayerVotesCleared;

		// Token: 0x06004777 RID: 18295 RVA: 0x0021C0D8 File Offset: 0x0021A2D8
		public MapSelectionSynchronizer(INetGameService netService, ActionQueueSynchronizer actionQueueSynchronizer, RunState runState)
		{
			this._netService = netService;
			this._actionQueueSynchronizer = actionQueueSynchronizer;
			this._runState = runState;
			this._multiplayerMapPointSelection = new Rng(this._runState.Rng.Seed, "map_point_selection");
			this.OnLocationChanged(this._runState.MapLocation);
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0021C150 File Offset: 0x0021A350
		public void PlayerVotedForMapCoord(Player player, MapLocation source, MapVote? destination)
		{
			if (this._acceptingVotesFromSource != source)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Received map vote from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" for source ");
				defaultInterpolatedStringHandler.AppendFormatted<MapLocation>(source);
				defaultInterpolatedStringHandler.AppendLiteral(", but we're currently accepting votes for ");
				defaultInterpolatedStringHandler.AppendFormatted<MapLocation>(this._acceptingVotesFromSource);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			int? num = ((destination != null) ? new int?(destination.GetValueOrDefault().mapGenerationCount) : null);
			int mapGenerationCount = this.MapGenerationCount;
			if ((num.GetValueOrDefault() < mapGenerationCount) & (num != null))
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Received map vote from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" for destination ");
				defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(destination);
				defaultInterpolatedStringHandler.AppendLiteral(", but the map generation count is lower than our current: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.MapGenerationCount);
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			int playerSlotIndex = this._runState.GetPlayerSlotIndex(player);
			MapVote? mapVote = this._votes[playerSlotIndex];
			this._votes[playerSlotIndex] = destination;
			Action<Player, MapVote?, MapVote?> playerVoteChanged = this.PlayerVoteChanged;
			if (playerVoteChanged != null)
			{
				playerVoteChanged(player, mapVote, destination);
			}
			if (destination != null)
			{
				Logger logger3 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Received vote to move to ");
				defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(destination);
				defaultInterpolatedStringHandler.AppendLiteral(" from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" (slot ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerSlotIndex);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			else
			{
				Logger logger4 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received cancellation of vote from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" (slot ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerSlotIndex);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (this._votes.All<MapVote?>((MapVote? p) => p != null && p.Value.mapGenerationCount == this.MapGenerationCount) && this._netService.Type != NetGameType.Client)
			{
				this._logger.Debug("All votes received and we are host, choosing coordinate", 1);
				this.MoveToMapCoord();
			}
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0021C3BB File Offset: 0x0021A5BB
		public MapVote? GetVote(Player player)
		{
			return this._votes[this._runState.GetPlayerSlotIndex(player)];
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0021C3D4 File Offset: 0x0021A5D4
		private void MoveToMapCoord()
		{
			if (this._netService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Only host should be moving to new map point!");
			}
			MapVote value = this._multiplayerMapPointSelection.NextItem<MapVote?>(this._votes).Value;
			MapCoord coord = value.coord;
			this._acceptingVotesFromSource.coord = new MapCoord?(coord);
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Moving to coordinate ");
			defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(coord);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			MoveToMapCoordAction moveToMapCoordAction = new MoveToMapCoordAction(LocalContext.GetMe(this._runState), coord);
			this._actionQueueSynchronizer.RequestEnqueue(moveToMapCoordAction);
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0021C47C File Offset: 0x0021A67C
		public void OnLocationChanged(MapLocation location)
		{
			this._acceptingVotesFromSource = location;
			this._votes.Clear();
			for (int i = 0; i < this._runState.Players.Count; i++)
			{
				this._votes.Add(null);
			}
			Action playerVotesCleared = this.PlayerVotesCleared;
			if (playerVotesCleared == null)
			{
				return;
			}
			playerVotesCleared();
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0021C4DC File Offset: 0x0021A6DC
		public void BeforeMapGenerated()
		{
			int mapGenerationCount = this.MapGenerationCount;
			this.MapGenerationCount = mapGenerationCount + 1;
			for (int i = 0; i < this._votes.Count; i++)
			{
				MapVote? mapVote = this._votes[i];
				if (mapVote != null && mapVote.Value.mapGenerationCount < this.MapGenerationCount)
				{
					Player player = this._runState.Players[i];
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cancelling map vote for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" because the map has re-generated and their vote is old");
					logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this._votes[i] = null;
					Action<Player> playerVoteCancelled = this.PlayerVoteCancelled;
					if (playerVoteCancelled != null)
					{
						playerVoteCancelled(player);
					}
				}
			}
		}

		// Token: 0x04001BF8 RID: 7160
		private readonly INetGameService _netService;

		// Token: 0x04001BF9 RID: 7161
		private readonly ActionQueueSynchronizer _actionQueueSynchronizer;

		// Token: 0x04001BFA RID: 7162
		private readonly RunState _runState;

		// Token: 0x04001BFB RID: 7163
		private MapLocation _acceptingVotesFromSource;

		// Token: 0x04001BFC RID: 7164
		private readonly List<MapVote?> _votes = new List<MapVote?>();

		// Token: 0x04001BFD RID: 7165
		private readonly Logger _logger = new Logger("MapSelectionSynchronizer", LogType.GameSync);

		// Token: 0x04001BFE RID: 7166
		private readonly Rng _multiplayerMapPointSelection;
	}
}
