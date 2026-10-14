using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C39 RID: 3129
	[NullableContext(1)]
	[Nullable(0)]
	public class GameActionPlayerChoiceContext : PlayerChoiceContext
	{
		// Token: 0x17002346 RID: 9030
		// (get) Token: 0x06008123 RID: 33059 RVA: 0x002BD1AE File Offset: 0x002BB3AE
		public GameAction Action { get; }

		// Token: 0x17002347 RID: 9031
		// (get) Token: 0x06008124 RID: 33060 RVA: 0x002BD1B6 File Offset: 0x002BB3B6
		public override ulong? OwnerId
		{
			get
			{
				return new ulong?(this.Action.OwnerId);
			}
		}

		// Token: 0x17002348 RID: 9032
		// (get) Token: 0x06008125 RID: 33061 RVA: 0x002BD1C8 File Offset: 0x002BB3C8
		private ActionQueueSynchronizer ActionQueueSynchronizer
		{
			get
			{
				return this._actionQueueSynchronizer ?? RunManager.Instance.ActionQueueSynchronizer;
			}
		}

		// Token: 0x17002349 RID: 9033
		// (get) Token: 0x06008126 RID: 33062 RVA: 0x002BD1DE File Offset: 0x002BB3DE
		private ActionQueueSet ActionQueueSet
		{
			get
			{
				return this._actionQueueSet ?? RunManager.Instance.ActionQueueSet;
			}
		}

		// Token: 0x1700234A RID: 9034
		// (get) Token: 0x06008127 RID: 33063 RVA: 0x002BD1F4 File Offset: 0x002BB3F4
		private ActionExecutor ActionExecutor
		{
			get
			{
				return this._actionExecutor ?? RunManager.Instance.ActionExecutor;
			}
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x002BD20A File Offset: 0x002BB40A
		public GameActionPlayerChoiceContext(GameAction action)
		{
			this.Action = action;
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x002BD21C File Offset: 0x002BB41C
		public override Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
		{
			ulong netId = chooser.NetId;
			ulong? ownerId = this.OwnerId;
			if (!((netId == ownerId.GetValueOrDefault()) & (ownerId != null)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(180, 4);
				defaultInterpolatedStringHandler.AppendFormatted("GameActionPlayerChoiceContext");
				defaultInterpolatedStringHandler.AppendLiteral(" is executing player choice owned by ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(chooser.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but ");
				defaultInterpolatedStringHandler.AppendLiteral("the player choice began with owner ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this.OwnerId);
				defaultInterpolatedStringHandler.AppendLiteral("! This will work, but will likely result in some weird-looking ");
				defaultInterpolatedStringHandler.AppendLiteral("user experience. See ");
				defaultInterpolatedStringHandler.AppendFormatted("BlockingPlayerChoiceContext");
				defaultInterpolatedStringHandler.AppendLiteral(" for a resolution.");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			if (this.ActionExecutor != null && this.ActionExecutor.CurrentlyRunningAction != this.Action)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to interrupt shared queue action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this.ActionExecutor.CurrentlyRunningAction);
				defaultInterpolatedStringHandler.AppendLiteral(" with a player choice context with action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this.Action);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return Task.CompletedTask;
			}
			this.ActionQueueSet.PauseActionForPlayerChoice(this.Action, options);
			return Task.CompletedTask;
		}

		// Token: 0x0600812A RID: 33066 RVA: 0x002BD374 File Offset: 0x002BB574
		public override async Task SignalPlayerChoiceEnded()
		{
			ulong ownerId = this.Action.OwnerId;
			ulong? netId = LocalContext.NetId;
			if ((ownerId == netId.GetValueOrDefault()) & (netId != null))
			{
				this.ActionQueueSynchronizer.RequestResumeActionAfterPlayerChoice(this.Action);
			}
			await this.Action.WaitForActionToResumeExecutingAfterPlayerChoice();
		}

		// Token: 0x0600812B RID: 33067 RVA: 0x002BD3B7 File Offset: 0x002BB5B7
		[NullableContext(2)]
		public void MockDependenciesForTest(ActionQueueSynchronizer actionQueueSynchronizer, ActionQueueSet actionQueueSet, ActionExecutor actionExecutor)
		{
			this._actionQueueSet = actionQueueSet;
			this._actionQueueSynchronizer = actionQueueSynchronizer;
			this._actionExecutor = actionExecutor;
		}

		// Token: 0x04002B3E RID: 11070
		[Nullable(2)]
		private ActionQueueSynchronizer _actionQueueSynchronizer;

		// Token: 0x04002B3F RID: 11071
		[Nullable(2)]
		private ActionQueueSet _actionQueueSet;

		// Token: 0x04002B40 RID: 11072
		[Nullable(2)]
		private ActionExecutor _actionExecutor;
	}
}
