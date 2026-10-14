using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// A player choice context for use when a separate GameAction is required upon player choice.
	///
	/// When player choice is required, a new game action is created and enqueued directly to the player's action queue.
	/// From there, flow proceeds like a normal game action. This is necessary to synchronize execution, so that the
	/// post-player-choice effects are deterministically executed across all peers. No new game action is created if player
	/// choice is not requested.
	///
	/// Note that this is also used outside of hooks, in the <see cref="T:MegaCrit.Sts2.Core.GameActions.Multiplayer.BranchingPlayerChoiceContext" />. It's likely that
	/// we should change the name of this class, but I'm not really sure what the new name should be yet.
	/// </summary>
	// Token: 0x02000C3A RID: 3130
	[NullableContext(2)]
	[Nullable(0)]
	public class HookPlayerChoiceContext : PlayerChoiceContext
	{
		// Token: 0x1700234B RID: 9035
		// (get) Token: 0x0600812C RID: 33068 RVA: 0x002BD3CE File Offset: 0x002BB5CE
		[Nullable(1)]
		private ActionQueueSynchronizer ActionQueueSynchronizer
		{
			[NullableContext(1)]
			get
			{
				return this._actionQueueSynchronizer ?? RunManager.Instance.ActionQueueSynchronizer;
			}
		}

		// Token: 0x1700234C RID: 9036
		// (get) Token: 0x0600812D RID: 33069 RVA: 0x002BD3E4 File Offset: 0x002BB5E4
		[Nullable(1)]
		private ActionQueueSet ActionQueueSet
		{
			[NullableContext(1)]
			get
			{
				return this._actionQueueSet ?? RunManager.Instance.ActionQueueSet;
			}
		}

		// Token: 0x1700234D RID: 9037
		// (get) Token: 0x0600812E RID: 33070 RVA: 0x002BD3FA File Offset: 0x002BB5FA
		[Nullable(1)]
		private ActionExecutor ActionExecutor
		{
			[NullableContext(1)]
			get
			{
				return this._actionExecutor ?? RunManager.Instance.ActionExecutor;
			}
		}

		// Token: 0x1700234E RID: 9038
		// (get) Token: 0x0600812F RID: 33071 RVA: 0x002BD410 File Offset: 0x002BB610
		public AbstractModel Source { get; }

		// Token: 0x1700234F RID: 9039
		// (get) Token: 0x06008130 RID: 33072 RVA: 0x002BD418 File Offset: 0x002BB618
		// (set) Token: 0x06008131 RID: 33073 RVA: 0x002BD420 File Offset: 0x002BB620
		public Task Task { get; private set; }

		// Token: 0x17002350 RID: 9040
		// (get) Token: 0x06008132 RID: 33074 RVA: 0x002BD429 File Offset: 0x002BB629
		public Player Owner { get; }

		// Token: 0x17002351 RID: 9041
		// (get) Token: 0x06008133 RID: 33075 RVA: 0x002BD434 File Offset: 0x002BB634
		public override ulong? OwnerId
		{
			get
			{
				Player owner = this.Owner;
				if (owner == null)
				{
					return null;
				}
				return new ulong?(owner.NetId);
			}
		}

		// Token: 0x17002352 RID: 9042
		// (get) Token: 0x06008134 RID: 33076 RVA: 0x002BD45F File Offset: 0x002BB65F
		public GenericHookGameAction GameAction
		{
			get
			{
				return this._gameAction;
			}
		}

		// Token: 0x06008135 RID: 33077 RVA: 0x002BD467 File Offset: 0x002BB667
		[NullableContext(1)]
		public HookPlayerChoiceContext(Player owner, ulong localPlayerId, GameActionType gameActionType)
		{
			this._gameActionType = gameActionType;
			this._localPlayerId = localPlayerId;
			this.Owner = owner;
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x002BD4A8 File Offset: 0x002BB6A8
		[NullableContext(1)]
		public HookPlayerChoiceContext(AbstractModel source, ulong localPlayerId, [Nullable(2)] ICombatState combatState, GameActionType gameActionType)
		{
			this._localPlayerId = localPlayerId;
			this.Source = source;
			this.Owner = HookPlayerChoiceContext.GetOwner(source, combatState);
			base.PushModel(this.Source);
			this._gameActionType = gameActionType;
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x002BD50C File Offset: 0x002BB70C
		public static Player GetOwner([Nullable(1)] AbstractModel source, ICombatState combatState)
		{
			CardModel cardModel = source as CardModel;
			Player player;
			if (cardModel == null)
			{
				RelicModel relicModel = source as RelicModel;
				if (relicModel == null)
				{
					PotionModel potionModel = source as PotionModel;
					if (potionModel == null)
					{
						AfflictionModel afflictionModel = source as AfflictionModel;
						if (afflictionModel == null)
						{
							EnchantmentModel enchantmentModel = source as EnchantmentModel;
							if (enchantmentModel == null)
							{
								PowerModel powerModel = source as PowerModel;
								if (powerModel == null)
								{
									player = null;
								}
								else
								{
									player = powerModel.Owner.Player;
								}
							}
							else
							{
								player = enchantmentModel.Card.Owner;
							}
						}
						else
						{
							player = afflictionModel.Card.Owner;
						}
					}
					else
					{
						player = potionModel.Owner;
					}
				}
				else
				{
					player = relicModel.Owner;
				}
			}
			else
			{
				player = cardModel.Owner;
			}
			Player player2 = player;
			if (player2 == null)
			{
				player2 = ((combatState != null) ? combatState.Players[0] : null);
			}
			return player2;
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x002BD5C8 File Offset: 0x002BB7C8
		public void MockDependenciesForTest(ActionQueueSynchronizer actionQueueSynchronizer, ActionQueueSet actionQueueSet, ActionExecutor actionExecutor)
		{
			this._actionQueueSet = actionQueueSet;
			this._actionQueueSynchronizer = actionQueueSynchronizer;
			this._actionExecutor = actionExecutor;
		}

		/// <summary>
		/// Most hooks take a PlayerChoiceContext as an argument and return a Task. However, this player choice context must
		/// know about the task in SignalPlayerChoiceBegun, when it is time to create a GameAction. There is no way to pass
		/// the Task to this method in the constructor, as the Task is not created yet. Thus, this method should be called
		/// after the hook returns the Task, for two reasons:
		/// - To assign the Task to this context
		/// - To wait for the synchronous parts of the hook to complete
		/// </summary>
		/// <returns>True if the task was completed, false if it was paused.</returns>
		// Token: 0x06008139 RID: 33081 RVA: 0x002BD5E0 File Offset: 0x002BB7E0
		[NullableContext(1)]
		public async Task<bool> AssignTaskAndWaitForPauseOrCompletion(Task task)
		{
			if (this.Source != null)
			{
				this.Task = this.ExecuteTaskThenInvokeExecutionFinished(task);
			}
			else
			{
				this.Task = task;
			}
			this._taskAssignedCompletionSource.SetResult();
			await TaskHelper.WhenAny(new Task[]
			{
				task,
				this._pausedCompletionSource.Task
			});
			return task.IsCompleted;
		}

		/// <summary>
		/// ONLY USE THIS IF YOU KNOW WHAT YOU'RE DOING!
		/// This waits for the task to complete, _or_ for some content in the task to call SignalPlayerChoiceBegun.
		/// In most cases, AssignTaskAndWaitForPauseOrCompletion should be used. This should only be used when we don't yet
		/// have the full task to assign to the GameAction that would be generated during SignalPlayerChoiceBegun.
		/// </summary>
		/// <returns>True if the task was completed, false if it was paused.</returns>
		// Token: 0x0600813A RID: 33082 RVA: 0x002BD62C File Offset: 0x002BB82C
		[NullableContext(1)]
		public async Task<bool> WaitForPauseOrCompletionWithoutAssigningTask(Task task)
		{
			await TaskHelper.WhenAny(new Task[]
			{
				task,
				this._pausedBeforeTaskAssignedCompletionSource.Task
			});
			return task.IsCompleted;
		}

		/// <summary>
		/// At the end of all hooks that involve models, the ExecutionFinished event must be executed to update some visuals
		/// for things that are listening to changes to the model. This task wraps the hook and calls ExecutionFinished at
		/// the end of it, if a model is involved with the hook.
		/// </summary>
		// Token: 0x0600813B RID: 33083 RVA: 0x002BD678 File Offset: 0x002BB878
		[NullableContext(1)]
		private async Task ExecuteTaskThenInvokeExecutionFinished(Task task)
		{
			await task;
			AbstractModel source = this.Source;
			if (source != null)
			{
				source.InvokeExecutionFinished();
			}
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x002BD6C4 File Offset: 0x002BB8C4
		[NullableContext(1)]
		public override async Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
		{
			ulong netId = chooser.NetId;
			ulong? ownerId = this.OwnerId;
			if (!((netId == ownerId.GetValueOrDefault()) & (ownerId != null)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(180, 4);
				defaultInterpolatedStringHandler.AppendFormatted("HookPlayerChoiceContext");
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
			if (this._gameAction != null)
			{
				if (this.ActionExecutor != null && this.ActionExecutor.CurrentlyRunningAction != this._gameAction)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to interrupt action ");
					defaultInterpolatedStringHandler.AppendFormatted<GenericHookGameAction>(this._gameAction);
					defaultInterpolatedStringHandler.AppendLiteral(" but the currently running action is ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(this.ActionExecutor.CurrentlyRunningAction);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return;
				}
			}
			else
			{
				if (this.Owner == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(126, 1);
					defaultInterpolatedStringHandler.AppendLiteral("HookPlayerChoiceContext is assigned a model ");
					defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(this.Source);
					defaultInterpolatedStringHandler.AppendLiteral(" with no owner, but the model has requested a player choice! This is not supported");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._gameAction = this.ActionQueueSynchronizer.GenerateHookAction(chooser.NetId, this._gameActionType);
				this._pausedBeforeTaskAssignedCompletionSource.SetResult();
				if (this.Task == null)
				{
					await this._taskAssignedCompletionSource.Task;
					if (this.Task == null)
					{
						throw new InvalidOperationException("HookPlayerChoiceContext was never passed a task to await!");
					}
				}
				this._gameAction.SetChoiceContext(this);
				if (this._gameAction.OwnerId == this._localPlayerId)
				{
					this.ActionQueueSynchronizer.RequestEnqueueHookAction(this._gameAction);
				}
				this._pausedCompletionSource.SetResult();
				await this._gameAction.ExecutionStartedTask;
			}
			this.ActionQueueSet.PauseActionForPlayerChoice(this._gameAction, options);
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x002BD718 File Offset: 0x002BB918
		[NullableContext(1)]
		public override async Task SignalPlayerChoiceEnded()
		{
			if (this._gameAction.OwnerId == this._localPlayerId)
			{
				this.ActionQueueSynchronizer.RequestResumeActionAfterPlayerChoice(this._gameAction);
			}
			await this._gameAction.WaitForActionToResumeExecutingAfterPlayerChoice();
		}

		// Token: 0x0600813E RID: 33086 RVA: 0x002BD75C File Offset: 0x002BB95C
		[NullableContext(1)]
		public async Task WaitForCompletion()
		{
			if (this.Task == null)
			{
				throw new InvalidOperationException("Tried to call WaitForCompletion before AssignTaskAndWaitForPauseOrCompletion was called!");
			}
			await this.Task;
			if (this.GameAction != null)
			{
				await this.GameAction.CompletionTask;
			}
		}

		// Token: 0x04002B41 RID: 11073
		private readonly ulong _localPlayerId;

		// Token: 0x04002B42 RID: 11074
		private GenericHookGameAction _gameAction;

		// Token: 0x04002B43 RID: 11075
		[Nullable(1)]
		private readonly TaskCompletionSource _taskAssignedCompletionSource = new TaskCompletionSource();

		// Token: 0x04002B44 RID: 11076
		[Nullable(1)]
		private readonly TaskCompletionSource _pausedBeforeTaskAssignedCompletionSource = new TaskCompletionSource();

		// Token: 0x04002B45 RID: 11077
		[Nullable(1)]
		private readonly TaskCompletionSource _pausedCompletionSource = new TaskCompletionSource();

		// Token: 0x04002B46 RID: 11078
		private ActionQueueSynchronizer _actionQueueSynchronizer;

		// Token: 0x04002B47 RID: 11079
		private ActionQueueSet _actionQueueSet;

		// Token: 0x04002B48 RID: 11080
		private ActionExecutor _actionExecutor;

		/// <summary>
		/// The GameActionType to assign to the GameAction generated by this context, if any. Currently, only Combat and
		/// CombatPlayPhaseOnly are supported.
		/// Many hooks can execute outside of the player turn. For example, we allow the following:
		///  - Centennial Puzzle triggers on the enemy turn
		///  - Hellraiser triggers Seeker Strike
		///  - Seeker Strike causes a player choice
		/// However, for actions that start just before the player turn, we must wait until the player turn before executing
		/// them. Otherwise, we may run into ordering issues on remote players. For full context, see the
		/// HookPlayerChoiceContextTest.
		/// </summary>
		// Token: 0x04002B49 RID: 11081
		private GameActionType _gameActionType;
	}
}
