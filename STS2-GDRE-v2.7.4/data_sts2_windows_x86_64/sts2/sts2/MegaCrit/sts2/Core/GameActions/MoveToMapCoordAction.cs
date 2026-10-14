using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// An action enqueued by the host after all players have issued a vote at the map screen.
	/// This needs to be enqueued, instead of depending on everyone picking the same map point, because a random map point
	/// may be chosen in the event that two or more map points receive the same number of votes. We do not depend on RNGs
	/// being deterministic outside of combat.
	/// </summary>
	// Token: 0x02000C23 RID: 3107
	[NullableContext(1)]
	[Nullable(0)]
	public class MoveToMapCoordAction : GameAction
	{
		// Token: 0x17002321 RID: 8993
		// (get) Token: 0x0600805A RID: 32858 RVA: 0x002B88A6 File Offset: 0x002B6AA6
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002322 RID: 8994
		// (get) Token: 0x0600805B RID: 32859 RVA: 0x002B88B3 File Offset: 0x002B6AB3
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.NonCombat;
			}
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x002B88B6 File Offset: 0x002B6AB6
		public MoveToMapCoordAction(Player player, MapCoord destination)
		{
			this._player = player;
			this._destination = destination;
		}

		// Token: 0x0600805D RID: 32861 RVA: 0x002B88CC File Offset: 0x002B6ACC
		protected override async Task ExecuteAction()
		{
			if (TestMode.IsOn)
			{
				await RunManager.Instance.FadeOut();
				await RunManager.Instance.EnterMapCoord(this._destination);
				TaskHelper.RunSafely(RunManager.Instance.FadeIn(true));
			}
			else
			{
				await NMapScreen.Instance.TravelToMapCoord(this._destination);
			}
		}

		// Token: 0x0600805E RID: 32862 RVA: 0x002B8910 File Offset: 0x002B6B10
		public override INetAction ToNetAction()
		{
			return new NetMoveToMapCoordAction
			{
				destination = this._destination
			};
		}

		// Token: 0x0600805F RID: 32863 RVA: 0x002B8938 File Offset: 0x002B6B38
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted("MoveToMapCoordAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(this._destination);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AF5 RID: 10997
		private readonly Player _player;

		// Token: 0x04002AF6 RID: 10998
		private readonly MapCoord _destination;
	}
}
