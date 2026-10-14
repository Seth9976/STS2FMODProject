using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// Responsible for pulling actions from the PlayerActionQueueSet and executing them.
	/// </summary>
	// Token: 0x02000C1C RID: 3100
	[NullableContext(1)]
	[Nullable(0)]
	public class ActionExecutor
	{
		// Token: 0x1700230C RID: 8972
		// (get) Token: 0x06008001 RID: 32769 RVA: 0x002B7778 File Offset: 0x002B5978
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
		}

		// Token: 0x1700230D RID: 8973
		// (get) Token: 0x06008002 RID: 32770 RVA: 0x002B7780 File Offset: 0x002B5980
		public bool IsRunning
		{
			get
			{
				TaskCompletionSource<bool> queueTaskCompletionSource = this._queueTaskCompletionSource;
				if (queueTaskCompletionSource != null)
				{
					Task<bool> task = queueTaskCompletionSource.Task;
					if (task != null)
					{
						return !task.IsCompleted;
					}
				}
				return false;
			}
		}

		// Token: 0x1700230E RID: 8974
		// (get) Token: 0x06008003 RID: 32771 RVA: 0x002B77AC File Offset: 0x002B59AC
		// (set) Token: 0x06008004 RID: 32772 RVA: 0x002B77B4 File Offset: 0x002B59B4
		[Nullable(2)]
		public GameAction CurrentlyRunningAction
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06008005 RID: 32773 RVA: 0x002B77C0 File Offset: 0x002B59C0
		// (remove) Token: 0x06008006 RID: 32774 RVA: 0x002B77F8 File Offset: 0x002B59F8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforeActionExecuted;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06008007 RID: 32775 RVA: 0x002B7830 File Offset: 0x002B5A30
		// (remove) Token: 0x06008008 RID: 32776 RVA: 0x002B7868 File Offset: 0x002B5A68
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> JustBeforeActionFinishedExecuting;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06008009 RID: 32777 RVA: 0x002B78A0 File Offset: 0x002B5AA0
		// (remove) Token: 0x0600800A RID: 32778 RVA: 0x002B78D8 File Offset: 0x002B5AD8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> AfterActionExecuted;

		// Token: 0x0600800B RID: 32779 RVA: 0x002B790D File Offset: 0x002B5B0D
		public ActionExecutor(ActionQueueSet actionQueueSet)
		{
			actionQueueSet.ActionQueueChanged += this.ActionQueueChanged;
			this._actionQueueSet = actionQueueSet;
			this._logger = new Logger("ActionExecutor", LogType.Actions);
		}

		/// <summary>
		/// WARNING: You probably want to use CombatManager.Pause() instead. Only use this if you want to pause player
		/// actions WITHOUT pausing the rest of combat.
		///
		/// Pause the ActionQueue.
		/// </summary>
		// Token: 0x0600800C RID: 32780 RVA: 0x002B793F File Offset: 0x002B5B3F
		public void Pause()
		{
			if (NonInteractiveMode.AutoSlayerCheck())
			{
				return;
			}
			this._logger.Debug("Pausing queue", 1);
			this._isPaused = true;
		}

		/// <summary>
		/// WARNING: You probably want to use CombatManager.Unpause() instead. Only use this if you want to un-pause player
		/// actions WITHOUT un-pausing the rest of combat.
		///
		/// Unpause the ActionQueue.
		/// </summary>
		// Token: 0x0600800D RID: 32781 RVA: 0x002B7966 File Offset: 0x002B5B66
		public void Unpause()
		{
			this._logger.Debug("Un-pausing queue", 1);
			this._isPaused = false;
		}

		/// <summary>
		/// Returns a task which finishes execution when there are no tasks left to execute.
		/// If there is no currently executing task, then this returns a completed task.
		/// </summary>
		// Token: 0x0600800E RID: 32782 RVA: 0x002B7980 File Offset: 0x002B5B80
		public Task FinishedExecutingActions()
		{
			if (this._queueTaskCompletionSource == null)
			{
				return Task.CompletedTask;
			}
			return this._queueTaskCompletionSource.Task;
		}

		/// <summary>
		/// Cancel the whole queue.
		/// </summary>
		// Token: 0x0600800F RID: 32783 RVA: 0x002B799B File Offset: 0x002B5B9B
		public void Cancel()
		{
			this._logger.Debug("Cancelling queue", 1);
			CancellationTokenSource actionCancelToken = this._actionCancelToken;
			if (actionCancelToken == null)
			{
				return;
			}
			actionCancelToken.Cancel();
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x002B79BE File Offset: 0x002B5BBE
		private void ActionQueueChanged()
		{
			if (!this.IsRunning)
			{
				this._logger.Debug("Action queue changed, beginning ExecuteActions", 1);
				TaskHelper.RunSafely(this.ExecuteActions());
			}
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x002B79E8 File Offset: 0x002B5BE8
		private async Task ExecuteActions()
		{
			this._queueTaskCompletionSource = new TaskCompletionSource<bool>();
			this._actionCancelToken = new CancellationTokenSource();
			try
			{
				GameAction readyAction = this._actionQueueSet.GetReadyAction();
				while (readyAction != null)
				{
					await this.WaitForUnpause();
					if (readyAction.State == GameActionState.Canceled)
					{
						readyAction = this._actionQueueSet.GetReadyAction();
					}
					else
					{
						Logger logger = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Executing action: ");
						defaultInterpolatedStringHandler.AppendFormatted<GameAction>(readyAction);
						logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						Action<GameAction> beforeActionExecuted = this.BeforeActionExecuted;
						if (beforeActionExecuted != null)
						{
							beforeActionExecuted(readyAction);
						}
						if (NonInteractiveMode.IsActive)
						{
							this.CurrentlyRunningAction = readyAction;
							await readyAction.Execute();
							this.AfterActionFinished(readyAction);
						}
						else
						{
							this.CurrentlyRunningAction = readyAction;
							readyAction.JustBeforeFinished += this.JustBeforeActionFinished;
							readyAction.AfterFinished += this.AfterActionFinished;
							Task actionTask = readyAction.Execute();
							while (!actionTask.IsCompleted && !this._actionCancelToken.IsCancellationRequested)
							{
								await Engine.GetMainLoop().ToSignal(Engine.GetMainLoop(), SceneTree.SignalName.ProcessFrame);
							}
							if (actionTask.IsFaulted)
							{
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
								defaultInterpolatedStringHandler.AppendLiteral("GameAction ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(readyAction);
								defaultInterpolatedStringHandler.AppendLiteral(" completed with exception: ");
								defaultInterpolatedStringHandler.AppendFormatted<AggregateException>(actionTask.Exception);
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							}
							actionTask = null;
						}
						bool flag = CombatManager.Instance.IsInProgress;
						if (flag)
						{
							flag = !(readyAction is EndPlayerTurnAction) && !(readyAction is ReadyToBeginEnemyTurnAction);
						}
						if (flag)
						{
							await CombatManager.Instance.CheckWinCondition();
						}
						if (readyAction.State == GameActionState.Finished)
						{
							Logger logger2 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Completed execution of action ");
							defaultInterpolatedStringHandler.AppendFormatted<GameAction>(readyAction);
							defaultInterpolatedStringHandler.AppendLiteral(", attempting to find new action");
							logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						}
						else
						{
							Logger logger3 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Paused execution of action ");
							defaultInterpolatedStringHandler.AppendFormatted<GameAction>(readyAction);
							defaultInterpolatedStringHandler.AppendLiteral(" (state is ");
							defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(readyAction.State);
							defaultInterpolatedStringHandler.AppendLiteral("), attempting to find new action");
							logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						}
						readyAction.JustBeforeFinished -= this.JustBeforeActionFinished;
						readyAction.AfterFinished -= this.AfterActionFinished;
						readyAction = this._actionQueueSet.GetReadyAction();
					}
				}
				TaskCompletionSource<bool> queueTaskCompletionSource = this._queueTaskCompletionSource;
				if (queueTaskCompletionSource != null)
				{
					queueTaskCompletionSource.SetResult(true);
				}
				readyAction = null;
			}
			catch (OperationCanceledException ex)
			{
				InvalidOperationException ex2 = new InvalidOperationException("ActionExecutor.ExecuteActions should never be canceled!", ex);
				TaskCompletionSource<bool> queueTaskCompletionSource2 = this._queueTaskCompletionSource;
				if (queueTaskCompletionSource2 != null)
				{
					queueTaskCompletionSource2.SetException(ex2);
				}
				throw ex2;
			}
			catch (Exception ex3)
			{
				TaskCompletionSource<bool> queueTaskCompletionSource3 = this._queueTaskCompletionSource;
				if (queueTaskCompletionSource3 != null)
				{
					queueTaskCompletionSource3.SetException(ex3);
				}
				throw;
			}
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x002B7A2C File Offset: 0x002B5C2C
		private void JustBeforeActionFinished(GameAction action)
		{
			if (this.CurrentlyRunningAction != action)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Currently running action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this.CurrentlyRunningAction);
				defaultInterpolatedStringHandler.AppendLiteral(" did not match finishing action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			if (action.State == GameActionState.Finished)
			{
				Action<GameAction> justBeforeActionFinishedExecuting = this.JustBeforeActionFinishedExecuting;
				if (justBeforeActionFinishedExecuting == null)
				{
					return;
				}
				justBeforeActionFinishedExecuting(action);
			}
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x002B7AB0 File Offset: 0x002B5CB0
		private void AfterActionFinished(GameAction action)
		{
			if (this.CurrentlyRunningAction != action)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Currently running action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this.CurrentlyRunningAction);
				defaultInterpolatedStringHandler.AppendLiteral(" did not match recently finished action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			if (action.State == GameActionState.Finished)
			{
				Action<GameAction> afterActionExecuted = this.AfterActionExecuted;
				if (afterActionExecuted != null)
				{
					afterActionExecuted(action);
				}
			}
			this.CurrentlyRunningAction = null;
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x002B7B3C File Offset: 0x002B5D3C
		private async Task WaitForUnpause()
		{
			if (!NonInteractiveMode.AutoSlayerCheck())
			{
				while (this._isPaused)
				{
					if (TestMode.IsOn)
					{
						await Task.Delay(50);
					}
					else
					{
						await NGame.Instance.AwaitProcessFrame(default(CancellationToken));
					}
				}
			}
		}

		// Token: 0x04002ACF RID: 10959
		private readonly ActionQueueSet _actionQueueSet;

		// Token: 0x04002AD0 RID: 10960
		private bool _isPaused;

		// Token: 0x04002AD1 RID: 10961
		[Nullable(2)]
		private CancellationTokenSource _actionCancelToken;

		// Token: 0x04002AD2 RID: 10962
		[Nullable(2)]
		private TaskCompletionSource<bool> _queueTaskCompletionSource;

		// Token: 0x04002AD7 RID: 10967
		private readonly Logger _logger;
	}
}
