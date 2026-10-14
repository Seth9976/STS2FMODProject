using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// A GameAction is a thin wrapper around an async task that should be run in response to player input.
	/// THIS IS DIFFERENT from GameActions in STS1 (and the original Unity version of STS2).
	///
	/// In STS1, a GameAction represented a small unit of game logic, like dealing damage, gaining block, etc.
	///
	/// In STS2, these small units of logic are handled by Commands (see the MegaCrit.Sts2.Core.Commands namespace).
	/// A GameAction WRAPS these commands, and should ONLY be used for player input.
	///
	/// Examples of things that SHOULD be wrapped in a game action:
	/// * Playing a card.
	/// * Drinking a potion.
	/// * Clicking the "end player turn" button.
	///
	/// Examples of things that should NOT be wrapped in a game action:
	/// * Dealing damage (this can happen as PART of a game action, but never directly from player input).
	/// * Gaining block (same reason).
	/// * Applying a power to a creature (same reason).
	/// * A monster making a move (once the player's turn ends, monster moves are run in sequence. They wait on each other
	///   to execute, but never on player input, so no GameActions are necessary).
	/// </summary>
	// Token: 0x02000C21 RID: 3105
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class GameAction
	{
		// Token: 0x17002314 RID: 8980
		// (get) Token: 0x0600802B RID: 32811 RVA: 0x002B7F82 File Offset: 0x002B6182
		// (set) Token: 0x0600802C RID: 32812 RVA: 0x002B7F8A File Offset: 0x002B618A
		public GameActionState State { get; private set; }

		// Token: 0x17002315 RID: 8981
		// (get) Token: 0x0600802D RID: 32813
		public abstract ulong OwnerId { get; }

		/// <summary>
		/// The type of game action, which dictates cancellation and enqueuing timings. See comments in the enum for
		/// details.
		/// </summary>
		// Token: 0x17002316 RID: 8982
		// (get) Token: 0x0600802E RID: 32814
		public abstract GameActionType ActionType { get; }

		// Token: 0x17002317 RID: 8983
		// (get) Token: 0x0600802F RID: 32815 RVA: 0x002B7F93 File Offset: 0x002B6193
		// (set) Token: 0x06008030 RID: 32816 RVA: 0x002B7F9B File Offset: 0x002B619B
		public uint? Id { get; private set; }

		/// <summary>
		/// Called when the GameAction is fully run to completion, but just before we complete _completionSource.
		/// This timing is important for checksum generation.
		/// </summary>
		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06008031 RID: 32817 RVA: 0x002B7FA4 File Offset: 0x002B61A4
		// (remove) Token: 0x06008032 RID: 32818 RVA: 0x002B7FDC File Offset: 0x002B61DC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> JustBeforeFinished;

		/// <summary>
		/// Called when the GameAction is fully run to completion.
		/// </summary>
		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06008033 RID: 32819 RVA: 0x002B8014 File Offset: 0x002B6214
		// (remove) Token: 0x06008034 RID: 32820 RVA: 0x002B804C File Offset: 0x002B624C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> AfterFinished;

		/// <summary>
		/// Called just before the GameAction begins execution for the first time.
		/// Not called when the GameAction is resumed.
		/// </summary>
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06008035 RID: 32821 RVA: 0x002B8084 File Offset: 0x002B6284
		// (remove) Token: 0x06008036 RID: 32822 RVA: 0x002B80BC File Offset: 0x002B62BC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforeExecuted;

		/// <summary>
		/// Called when the GameAction is cancelled.
		/// </summary>
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06008037 RID: 32823 RVA: 0x002B80F4 File Offset: 0x002B62F4
		// (remove) Token: 0x06008038 RID: 32824 RVA: 0x002B812C File Offset: 0x002B632C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforeCancelled;

		/// <summary>
		/// Called when the GameAction is paused for player choice.
		/// </summary>
		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06008039 RID: 32825 RVA: 0x002B8164 File Offset: 0x002B6364
		// (remove) Token: 0x0600803A RID: 32826 RVA: 0x002B819C File Offset: 0x002B639C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforePausedForPlayerChoice;

		/// <summary>
		/// Called when the GameAction is ready to resume after player choice.
		/// </summary>
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x0600803B RID: 32827 RVA: 0x002B81D4 File Offset: 0x002B63D4
		// (remove) Token: 0x0600803C RID: 32828 RVA: 0x002B820C File Offset: 0x002B640C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforeReadyToResumeAfterPlayerChoice;

		/// <summary>
		/// Called when the GameAction resumes execution after player choice.
		/// </summary>
		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x0600803D RID: 32829 RVA: 0x002B8244 File Offset: 0x002B6444
		// (remove) Token: 0x0600803E RID: 32830 RVA: 0x002B827C File Offset: 0x002B647C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> BeforeResumedAfterPlayerChoice;

		/// <summary>
		/// Use this to wait for the GameAction to be fully completed.
		/// </summary>
		// Token: 0x17002318 RID: 8984
		// (get) Token: 0x0600803F RID: 32831 RVA: 0x002B82B1 File Offset: 0x002B64B1
		public Task CompletionTask
		{
			get
			{
				return this._completionSource.Task;
			}
		}

		// Token: 0x17002319 RID: 8985
		// (get) Token: 0x06008040 RID: 32832 RVA: 0x002B82BE File Offset: 0x002B64BE
		[Nullable(2)]
		public Exception Exception
		{
			[NullableContext(2)]
			get
			{
				Task executionTask = this._executionTask;
				if (executionTask == null)
				{
					return null;
				}
				return executionTask.Exception;
			}
		}

		// Token: 0x1700231A RID: 8986
		// (get) Token: 0x06008041 RID: 32833 RVA: 0x002B82D1 File Offset: 0x002B64D1
		public virtual bool RecordableToReplay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x002B82D4 File Offset: 0x002B64D4
		public void OnEnqueued(Action<GameAction> afterFinished, uint id)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.State != GameActionState.None)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" was enqueued to the queue twice!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
			defaultInterpolatedStringHandler.AppendLiteral(" enqueued with id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
			Log.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this.Id = new uint?(id);
			this.AfterFinished += afterFinished;
			this.State = GameActionState.WaitingForExecution;
		}

		// Token: 0x06008043 RID: 32835 RVA: 0x002B837C File Offset: 0x002B657C
		public async Task Execute()
		{
			this._pauseForPlayerChoiceTaskSource = new TaskCompletionSource();
			GameActionState state = this.State;
			if (state != GameActionState.WaitingForExecution)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (state != GameActionState.ReadyToResumeExecuting)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(108, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Attempted to execute GameAction ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" from invalid state ");
					defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(this.State);
					defaultInterpolatedStringHandler.AppendLiteral("! Expected WaitingForExecution or ReadyToResumeExecuting");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Logger logger = GameAction._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" resumed execution");
				logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this.State = GameActionState.Executing;
				this._executeAfterResumptionTaskSource.SetResult();
			}
			else
			{
				Logger logger2 = GameAction._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" began executing");
				logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this.State = GameActionState.Executing;
				Action<GameAction> beforeExecuted = this.BeforeExecuted;
				if (beforeExecuted != null)
				{
					beforeExecuted(this);
				}
				this._executionTask = TaskHelper.RunSafely(this.ExecuteAction());
			}
			try
			{
				await TaskHelper.WhenAny(new Task[]
				{
					this._executionTask,
					this._pauseForPlayerChoiceTaskSource.Task
				});
			}
			finally
			{
				if (this._executionTask.IsCompleted)
				{
					Logger logger3 = GameAction._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Action ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" finished execution");
					logger3.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.State = GameActionState.Finished;
					Action<GameAction> justBeforeFinished = this.JustBeforeFinished;
					if (justBeforeFinished != null)
					{
						justBeforeFinished(this);
					}
					this._completionSource.TrySetResult();
					Action<GameAction> afterFinished = this.AfterFinished;
					if (afterFinished != null)
					{
						afterFinished(this);
					}
				}
				else
				{
					Logger logger4 = GameAction._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Action ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" paused execution");
					logger4.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}
		}

		// Token: 0x06008044 RID: 32836 RVA: 0x002B83C0 File Offset: 0x002B65C0
		public void ResumeAfterGatheringPlayerChoice(uint newId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.State != GameActionState.GatheringPlayerChoice)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried setting GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" ready from invalid state ");
				defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(this.State);
				defaultInterpolatedStringHandler.AppendLiteral("! Expected GatheringPlayerChoice");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = GameAction._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
			defaultInterpolatedStringHandler.AppendLiteral(" finished gathering player choice, and is assigned new id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(newId);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.Id = new uint?(newId);
			Action<GameAction> beforeReadyToResumeAfterPlayerChoice = this.BeforeReadyToResumeAfterPlayerChoice;
			if (beforeReadyToResumeAfterPlayerChoice != null)
			{
				beforeReadyToResumeAfterPlayerChoice(this);
			}
			this.State = GameActionState.ReadyToResumeExecuting;
		}

		// Token: 0x06008045 RID: 32837 RVA: 0x002B8490 File Offset: 0x002B6690
		public async Task WaitForActionToResumeExecutingAfterPlayerChoice()
		{
			Logger logger = GameAction._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
			defaultInterpolatedStringHandler.AppendLiteral(" waiting to resume execution after player choice");
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			await this._executeAfterResumptionTaskSource.Task;
			this._executeAfterResumptionTaskSource = null;
			Action<GameAction> beforeResumedAfterPlayerChoice = this.BeforeResumedAfterPlayerChoice;
			if (beforeResumedAfterPlayerChoice != null)
			{
				beforeResumedAfterPlayerChoice(this);
			}
		}

		// Token: 0x06008046 RID: 32838 RVA: 0x002B84D4 File Offset: 0x002B66D4
		public void PauseForPlayerChoice()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.State != GameActionState.Executing)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to pause GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" from invalid state ");
				defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(this.State);
				defaultInterpolatedStringHandler.AppendLiteral("! Expected Executing");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = GameAction._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this);
			defaultInterpolatedStringHandler.AppendLiteral(" gathering player choice");
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._executeAfterResumptionTaskSource = new TaskCompletionSource();
			Action<GameAction> beforePausedForPlayerChoice = this.BeforePausedForPlayerChoice;
			if (beforePausedForPlayerChoice != null)
			{
				beforePausedForPlayerChoice(this);
			}
			this.State = GameActionState.GatheringPlayerChoice;
			this._pauseForPlayerChoiceTaskSource.SetResult();
		}

		// Token: 0x06008047 RID: 32839
		protected abstract Task ExecuteAction();

		// Token: 0x06008048 RID: 32840 RVA: 0x002B85A8 File Offset: 0x002B67A8
		public void Cancel()
		{
			this.State = GameActionState.Canceled;
			Action<GameAction> beforeCancelled = this.BeforeCancelled;
			if (beforeCancelled != null)
			{
				beforeCancelled(this);
			}
			this.CancelAction();
			Callable.From<bool>(() => this._completionSource.TrySetCanceled()).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06008049 RID: 32841 RVA: 0x002B85F2 File Offset: 0x002B67F2
		protected virtual void CancelAction()
		{
		}

		// Token: 0x0600804A RID: 32842
		public abstract INetAction ToNetAction();

		// Token: 0x04002AE0 RID: 10976
		private static readonly Logger _logger = new Logger("GameAction", LogType.Actions);

		// Token: 0x04002AE3 RID: 10979
		[Nullable(2)]
		private TaskCompletionSource _pauseForPlayerChoiceTaskSource;

		// Token: 0x04002AE4 RID: 10980
		[Nullable(2)]
		private TaskCompletionSource _executeAfterResumptionTaskSource;

		// Token: 0x04002AE5 RID: 10981
		private TaskCompletionSource _completionSource = new TaskCompletionSource();

		// Token: 0x04002AE6 RID: 10982
		[Nullable(2)]
		private Task _executionTask;
	}
}
