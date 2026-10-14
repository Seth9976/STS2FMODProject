using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// An action enqueued at the map screen when the player picks/votes for a map coord to move to.
	/// Once all players have selected a map coord, the host decides which map coord to move to based on the votes. It then
	/// enqueues MoveToMapCoordAction which triggers entering the room.
	/// This action may be enqueued multiple times with the same origin location, and should overwrite which coord the owning
	/// player is voting for. If it is enqueued after MoveToMapCoordAction has already been executed, then it should be
	/// ignored.
	/// </summary>
	// Token: 0x02000C30 RID: 3120
	[NullableContext(1)]
	[Nullable(0)]
	public class VoteForMapCoordAction : GameAction
	{
		// Token: 0x1700233B RID: 9019
		// (get) Token: 0x060080C8 RID: 32968 RVA: 0x002BA4BA File Offset: 0x002B86BA
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x1700233C RID: 9020
		// (get) Token: 0x060080C9 RID: 32969 RVA: 0x002BA4C7 File Offset: 0x002B86C7
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.Any;
			}
		}

		// Token: 0x060080CA RID: 32970 RVA: 0x002BA4CA File Offset: 0x002B86CA
		public VoteForMapCoordAction(Player player, MapLocation source, MapVote? destination)
		{
			this._player = player;
			this._source = source;
			this._destination = destination;
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x002BA4E7 File Offset: 0x002B86E7
		protected override Task ExecuteAction()
		{
			RunManager.Instance.MapSelectionSynchronizer.PlayerVotedForMapCoord(this._player, this._source, this._destination);
			return Task.CompletedTask;
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x002BA510 File Offset: 0x002B8710
		public override INetAction ToNetAction()
		{
			return new NetVoteForMapCoordAction
			{
				source = this._source,
				destination = this._destination
			};
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x002BA548 File Offset: 0x002B8748
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 4);
			defaultInterpolatedStringHandler.AppendFormatted("VoteForMapCoordAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<MapLocation>(this._source);
			defaultInterpolatedStringHandler.AppendLiteral("->");
			defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(this._destination);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// The player who is voting.
		/// </summary>
		// Token: 0x04002B1B RID: 11035
		private readonly Player _player;

		/// <summary>
		/// The location that the player is currently at while casting the vote.
		/// </summary>
		// Token: 0x04002B1C RID: 11036
		private readonly MapLocation _source;

		/// <summary>
		/// The player's vote. If null, any existing votes for the player should be cancelled.
		/// </summary>
		// Token: 0x04002B1D RID: 11037
		private readonly MapVote? _destination;
	}
}
