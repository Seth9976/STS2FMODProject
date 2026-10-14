using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// An action enqueued at the rewards screen when the player is ready to move to the next act.
	/// Once a player receives actions from all other players indicating that they're ready to move to the next act, then
	/// the player should begin transitioning to the next act.
	/// </summary>
	// Token: 0x02000C32 RID: 3122
	[NullableContext(1)]
	[Nullable(0)]
	public class VoteToMoveToNextActAction : GameAction
	{
		/// <summary>
		/// The current act index.
		/// If the current act is not this or we are already transitioning from this act, then this action is ignored.
		/// </summary>
		// Token: 0x1700233D RID: 9021
		// (get) Token: 0x060080D2 RID: 32978 RVA: 0x002BA697 File Offset: 0x002B8897
		public int CurrentActIndex { get; }

		// Token: 0x1700233E RID: 9022
		// (get) Token: 0x060080D3 RID: 32979 RVA: 0x002BA69F File Offset: 0x002B889F
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x1700233F RID: 9023
		// (get) Token: 0x060080D4 RID: 32980 RVA: 0x002BA6AC File Offset: 0x002B88AC
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.NonCombat;
			}
		}

		// Token: 0x060080D5 RID: 32981 RVA: 0x002BA6AF File Offset: 0x002B88AF
		public VoteToMoveToNextActAction(Player player, int currentActIndex)
		{
			this._player = player;
			this.CurrentActIndex = currentActIndex;
		}

		// Token: 0x060080D6 RID: 32982 RVA: 0x002BA6C5 File Offset: 0x002B88C5
		protected override Task ExecuteAction()
		{
			RunManager.Instance.ActChangeSynchronizer.OnPlayerReady(this._player, this.CurrentActIndex);
			return Task.CompletedTask;
		}

		// Token: 0x060080D7 RID: 32983 RVA: 0x002BA6E8 File Offset: 0x002B88E8
		public override INetAction ToNetAction()
		{
			return new NetVoteToMoveToNextActAction
			{
				currentActIndex = this.CurrentActIndex
			};
		}

		// Token: 0x060080D8 RID: 32984 RVA: 0x002BA710 File Offset: 0x002B8910
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
			defaultInterpolatedStringHandler.AppendFormatted("VoteToMoveToNextActAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" act ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentActIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// The player who is voting.
		/// </summary>
		// Token: 0x04002B20 RID: 11040
		private readonly Player _player;
	}
}
