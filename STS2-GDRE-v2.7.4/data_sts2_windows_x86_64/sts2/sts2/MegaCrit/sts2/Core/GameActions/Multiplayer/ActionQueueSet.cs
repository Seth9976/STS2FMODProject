using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// Contains one action queue per player.
	/// We use multiple action queues to resolve the following scenario:
	/// - One player queues up three or more card plays, the second of which requires player choice
	/// - Their queue blocks on the player choice. The third action awaits completion of the player choice
	/// - Other players may freely queue card plays to their queues and have them executed
	/// </summary>
	// Token: 0x02000C34 RID: 3124
	[NullableContext(1)]
	[Nullable(0)]
	public class ActionQueueSet
	{
		// Token: 0x17002340 RID: 9024
		// (get) Token: 0x060080DD RID: 32989 RVA: 0x002BA7E2 File Offset: 0x002B89E2
		public bool IsEmpty
		{
			get
			{
				return this._queuesEmptyCompletionSource == null || this._queuesEmptyCompletionSource.Task.IsCompleted;
			}
		}

		// Token: 0x17002341 RID: 9025
		// (get) Token: 0x060080DE RID: 32990 RVA: 0x002BA7FE File Offset: 0x002B89FE
		public uint NextActionId
		{
			get
			{
				return this._nextId;
			}
		}

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060080DF RID: 32991 RVA: 0x002BA808 File Offset: 0x002B8A08
		// (remove) Token: 0x060080E0 RID: 32992 RVA: 0x002BA840 File Offset: 0x002B8A40
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action ActionQueueChanged;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060080E1 RID: 32993 RVA: 0x002BA878 File Offset: 0x002B8A78
		// (remove) Token: 0x060080E2 RID: 32994 RVA: 0x002BA8B0 File Offset: 0x002B8AB0
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<GameAction> ActionEnqueued;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x060080E3 RID: 32995 RVA: 0x002BA8E8 File Offset: 0x002B8AE8
		// (remove) Token: 0x060080E4 RID: 32996 RVA: 0x002BA920 File Offset: 0x002B8B20
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<uint> ActionResumed;

		// Token: 0x060080E5 RID: 32997 RVA: 0x002BA958 File Offset: 0x002B8B58
		public ActionQueueSet(IReadOnlyList<Player> players)
		{
			this._logger = new Logger("ActionQueueSet", LogType.Actions);
			foreach (Player player in players)
			{
				this._actionQueues.Add(new ActionQueueSet.ActionQueue
				{
					actions = new List<GameAction>(),
					ownerId = player.NetId
				});
			}
		}

		/// <summary>
		/// Only use this if you really know what you're doing! Improper use can lead to state divergence.
		/// Enqueues an action directly to the owner's action queue.
		/// </summary>
		/// <param name="gameAction">The action to enqueue.</param>
		// Token: 0x060080E6 RID: 32998 RVA: 0x002BA9F0 File Offset: 0x002B8BF0
		public void EnqueueWithoutSynchronizing(GameAction gameAction)
		{
			if (this._queuesEmptyCompletionSource == null || this._queuesEmptyCompletionSource.Task.IsCompleted)
			{
				this._queuesEmptyCompletionSource = new TaskCompletionSource();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (gameAction.Id != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(111, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempting to enqueue GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" which already has an ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint?>(gameAction.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", indicating it was previously enqueued to the queue!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			gameAction.OnEnqueued(new Action<GameAction>(this.PopAction), this.GetAndIncrementActionId());
			ActionQueueSet.ActionQueue queue = this.GetQueue(gameAction.OwnerId);
			try
			{
				Action<GameAction> actionEnqueued = this.ActionEnqueued;
				if (actionEnqueued != null)
				{
					actionEnqueued(gameAction);
				}
			}
			catch (Exception ex)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception encountered in ActionEnqueued for action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
			}
			if (queue.isCancellingPlayCardActions && gameAction is PlayCardAction)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(136, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to enqueue PlayCardAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" to player queue owned by ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(gameAction.OwnerId);
				defaultInterpolatedStringHandler.AppendLiteral(", but it's currently cancelling all play card actions due to player choice");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				gameAction.Cancel();
				return;
			}
			if (queue.isCancellingPlayerDrivenCombatActions && ActionQueueSet.IsGameActionPlayerDriven(gameAction) && gameAction.ActionType != GameActionType.NonCombat && gameAction.ActionType != GameActionType.Any)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(129, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to enqueue GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" to player queue owned by ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(gameAction.OwnerId);
				defaultInterpolatedStringHandler.AppendLiteral(", but it's currently cancelling all non-hook actions due to end of turn");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				gameAction.Cancel();
				return;
			}
			bool isCancellingCombatActions = queue.isCancellingCombatActions;
			bool flag = isCancellingCombatActions;
			if (flag)
			{
				GameActionType actionType = gameAction.ActionType;
				bool flag2 = actionType - GameActionType.Combat <= 1;
				flag = flag2;
			}
			if (flag)
			{
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(108, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to enqueue GameAction ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" to player queue owned by ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(gameAction.OwnerId);
				defaultInterpolatedStringHandler.AppendLiteral(", but it's currently cancelling all combat actions");
				logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				gameAction.Cancel();
				return;
			}
			Logger logger4 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Enqueueing action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
			defaultInterpolatedStringHandler.AppendLiteral(" to player queue owned by ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(gameAction.OwnerId);
			logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			queue.actions.Add(gameAction);
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Returns true if a GameAction is player driven, false if it is emitted automatically.
		/// If this ends up being used in a few different places, it can be moved somewhere else; for now, it's obscure enough
		/// that it should only be used here.
		/// </summary>
		/// <returns></returns>
		// Token: 0x060080E7 RID: 32999 RVA: 0x002BACF8 File Offset: 0x002B8EF8
		public static bool IsGameActionPlayerDriven(GameAction gameAction)
		{
			return !(gameAction is GenericHookGameAction) && !(gameAction is ReadyToBeginEnemyTurnAction);
		}

		/// <summary>
		/// Returns the next action on top of a player's queue that should be executed.
		/// Selection works like this:
		/// - If any player's queue is awaiting a player choice, then that queue is skipped.
		/// - Otherwise, we return the action with the minimum ID (the first queued by the host) at the top of any player's queue.
		/// This method should not be called when an action is already being executed.
		/// </summary>
		// Token: 0x060080E8 RID: 33000 RVA: 0x002BAD10 File Offset: 0x002B8F10
		[NullableContext(2)]
		public GameAction GetReadyAction()
		{
			GameAction gameAction = null;
			this._logger.VeryDebug("Attempting to find ready action", 1);
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				if (actionQueue.actions.Count <= 0)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Queue for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
					defaultInterpolatedStringHandler.AppendLiteral(" is empty");
					logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
				else
				{
					while (actionQueue.actions.Count > 0 && actionQueue.actions[0].State == GameActionState.Canceled)
					{
						Logger logger2 = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Removing canceled action ");
						defaultInterpolatedStringHandler.AppendFormatted<GameAction>(actionQueue.actions[0]);
						defaultInterpolatedStringHandler.AppendLiteral(" from front of player queue ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						actionQueue.actions.RemoveAt(0);
					}
					if (actionQueue.actions.Count > 0)
					{
						GameAction gameAction2 = actionQueue.actions[0];
						if (this._isInCombat && gameAction2.ActionType == GameActionType.NonCombat)
						{
							Logger logger3 = this._logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
							defaultInterpolatedStringHandler.AppendLiteral("We are currently in combat and candidate action ");
							defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
							defaultInterpolatedStringHandler.AppendLiteral(" has type ");
							defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(gameAction2.ActionType);
							logger3.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						}
						else
						{
							bool flag = !this._isInCombat;
							bool flag2 = flag;
							if (flag2)
							{
								GameActionType actionType = gameAction2.ActionType;
								bool flag3 = actionType - GameActionType.Combat <= 1;
								flag2 = flag3;
							}
							if (flag2)
							{
								Logger logger4 = this._logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
								defaultInterpolatedStringHandler.AppendLiteral("We are currently not in combat and candidate action ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
								defaultInterpolatedStringHandler.AppendLiteral(" has type ");
								defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(gameAction2.ActionType);
								logger4.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							}
							else if (actionQueue.isPaused && gameAction2.ActionType == GameActionType.CombatPlayPhaseOnly)
							{
								Logger logger5 = this._logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Queue for player ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
								defaultInterpolatedStringHandler.AppendLiteral(" is paused and candidate action ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
								defaultInterpolatedStringHandler.AppendLiteral(" has type ");
								defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(gameAction2.ActionType);
								logger5.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							}
							else if (gameAction2.State == GameActionState.GatheringPlayerChoice)
							{
								Logger logger6 = this._logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Action ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
								defaultInterpolatedStringHandler.AppendLiteral(" at front of player queue ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
								defaultInterpolatedStringHandler.AppendLiteral(" is waiting for player choice");
								logger6.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							}
							else
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
								if (gameAction2.State != GameActionState.WaitingForExecution && gameAction2.State != GameActionState.ReadyToResumeExecuting)
								{
									defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
									defaultInterpolatedStringHandler.AppendLiteral("GameAction ");
									defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
									defaultInterpolatedStringHandler.AppendLiteral(" at the front of player action queue ");
									defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
									defaultInterpolatedStringHandler.AppendLiteral(" is in invalid state ");
									defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(gameAction2.State);
									defaultInterpolatedStringHandler.AppendLiteral("!");
									throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
								}
								if (gameAction != null)
								{
									uint? id = gameAction2.Id;
									uint? id2 = gameAction.Id;
									if (!((id.GetValueOrDefault() < id2.GetValueOrDefault()) & ((id != null) & (id2 != null))))
									{
										Logger logger7 = this._logger;
										defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
										defaultInterpolatedStringHandler.AppendLiteral("Action ");
										defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
										defaultInterpolatedStringHandler.AppendLiteral(" has id ");
										defaultInterpolatedStringHandler.AppendFormatted<uint>(gameAction2.Id.Value);
										defaultInterpolatedStringHandler.AppendLiteral(" greater than current ready action ");
										defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
										defaultInterpolatedStringHandler.AppendLiteral(" with ID ");
										defaultInterpolatedStringHandler.AppendFormatted<uint>(gameAction.Id.Value);
										logger7.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
										continue;
									}
								}
								Logger logger8 = this._logger;
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Action ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction2);
								defaultInterpolatedStringHandler.AppendLiteral(" with id ");
								defaultInterpolatedStringHandler.AppendFormatted<uint>(gameAction2.Id.Value);
								defaultInterpolatedStringHandler.AppendLiteral(" belonging to ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
								defaultInterpolatedStringHandler.AppendLiteral(" becomes new ready action");
								logger8.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
								gameAction = gameAction2;
							}
						}
					}
				}
			}
			if (gameAction != null)
			{
				Logger logger9 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Got ready action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<uint?>(gameAction.Id);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				logger9.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			else
			{
				this._logger.VeryDebug("No action is ready", 1);
			}
			return gameAction;
		}

		/// <summary>
		/// Pauses an action for player choice. The action will be removed from the ActionExecutor and a new action will be
		/// allowed to execute, if there are any actions that are ready.
		/// To resume the action after this is called, call RequestResumeActionAfterPlayerChoice.
		/// </summary>
		/// <param name="action">The GameAction to pause.</param>
		/// <param name="options">Whether to cancel card play actions that enter the queue while obtaining player choice.</param>
		/// <exception cref="T:System.InvalidOperationException">Thrown if the action is not at the front of the owner's action queue.</exception>
		// Token: 0x060080E9 RID: 33001 RVA: 0x002BB260 File Offset: 0x002B9460
		public void PauseActionForPlayerChoice(GameAction action, PlayerChoiceOptions options)
		{
			ActionQueueSet.ActionQueue queue = this.GetQueue(action.OwnerId);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (action != queue.actions[0])
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempting to pause action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" that is not at the front of the owner ");
				defaultInterpolatedStringHandler.AppendFormatted<uint?>(action.Id);
				defaultInterpolatedStringHandler.AppendLiteral("'s queue!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Pausing action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
			defaultInterpolatedStringHandler.AppendLiteral(" for player choice");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			action.PauseForPlayerChoice();
			ActionQueueSet.ActionWaitingForResumption? actionWaitingForResumption = null;
			for (int i = 0; i < this._actionsWaitingForResumption.Count; i++)
			{
				uint oldId = this._actionsWaitingForResumption[i].oldId;
				uint? id = action.Id;
				if ((oldId == id.GetValueOrDefault()) & (id != null))
				{
					actionWaitingForResumption = new ActionQueueSet.ActionWaitingForResumption?(this._actionsWaitingForResumption[i]);
					this._actionsWaitingForResumption.RemoveAt(i);
					break;
				}
			}
			if (options.HasFlag(PlayerChoiceOptions.CancelPlayCardActions))
			{
				this.CancelNonExecutingActionsOfType<PlayCardAction>(action.OwnerId, (actionWaitingForResumption != null) ? new uint?(actionWaitingForResumption.GetValueOrDefault().newId) : null);
				queue.isCancellingPlayCardActions = true;
			}
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged != null)
			{
				actionQueueChanged();
			}
			if (actionWaitingForResumption != null)
			{
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Immediately resuming action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" - already had resumption waiting");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				action.ResumeAfterGatheringPlayerChoice(actionWaitingForResumption.Value.newId);
				queue.isCancellingPlayCardActions = false;
				Action actionQueueChanged2 = this.ActionQueueChanged;
				if (actionQueueChanged2 == null)
				{
					return;
				}
				actionQueueChanged2();
			}
		}

		/// <summary>
		/// Returns a task which completes when all player queues are empty.
		/// Be careful of running this in tests that involve player choice! If actions are not resumed, the task will never complete.
		/// </summary>
		// Token: 0x060080EA RID: 33002 RVA: 0x002BB453 File Offset: 0x002B9653
		public Task BecameEmpty()
		{
			if (this._queuesEmptyCompletionSource == null)
			{
				return Task.CompletedTask;
			}
			return this._queuesEmptyCompletionSource.Task;
		}

		/// <summary>
		/// Pauses execution of all actions on all player queues, including those that are queued after this is called.
		/// Note that this does not pause execution of the currently executing action - only those that come after it.
		/// </summary>
		// Token: 0x060080EB RID: 33003 RVA: 0x002BB470 File Offset: 0x002B9670
		public void PauseAllPlayerQueues()
		{
			this._logger.Debug("Pausing all player queues", 1);
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				actionQueue.isPaused = true;
				actionQueue.isCancellingPlayerDrivenCombatActions = false;
			}
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Cancels all manual combat actions that are enqueued to all queues.
		/// Manual combat actions are any GameActions that have ActionType != GameActionType.NonCombat and ActionType !=
		/// GameActionType.Any, and are manually played by the player (i.e. everything except for GenericHookGameAction and
		/// ReadyToSwitchToEnemyTurnAction). The flag becomes unset when either PauseAllPlayerQueues or UnpauseAllPlayerQueues
		/// is called.
		/// See the comments in EnqueueWithoutSynchronizing for why this is necessary.
		/// </summary>
		// Token: 0x060080EC RID: 33004 RVA: 0x002BB4EC File Offset: 0x002B96EC
		public void StartCancellingAllPlayerDrivenCombatActions()
		{
			this._logger.Debug("Setting all player queues to cancel all non-hook actions", 1);
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				actionQueue.isCancellingPlayerDrivenCombatActions = true;
				for (int i = 0; i < actionQueue.actions.Count; i++)
				{
					GameAction gameAction = actionQueue.actions[i];
					if (ActionQueueSet.IsGameActionPlayerDriven(gameAction) && gameAction.ActionType != GameActionType.NonCombat && gameAction.ActionType != GameActionType.Any && gameAction.State == GameActionState.WaitingForExecution)
					{
						Logger logger = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Cancelling non-hook action ");
						defaultInterpolatedStringHandler.AppendFormatted<GameAction>(actionQueue.actions[i]);
						logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						gameAction.Cancel();
						actionQueue.actions.RemoveAt(i);
						i--;
					}
				}
			}
		}

		/// <returns>Returns true if the action queue for the given player is paused.</returns>
		// Token: 0x060080ED RID: 33005 RVA: 0x002BB5F4 File Offset: 0x002B97F4
		public bool ActionQueueIsPaused(ulong playerId)
		{
			return this.GetQueue(playerId).isPaused;
		}

		/// <summary>
		/// Resumes execution of all player queues without synchronization.
		/// It's fine to call this at the end of combat, but call this very carefully during combat, as incorrect usage
		/// can lead to state divergences.
		/// </summary>
		// Token: 0x060080EE RID: 33006 RVA: 0x002BB604 File Offset: 0x002B9804
		public void UnpauseAllPlayerQueues()
		{
			this._logger.Debug("Unpausing all player queues", 1);
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				actionQueue.isPaused = false;
				actionQueue.isCancellingPlayerDrivenCombatActions = false;
			}
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Cancels all combat actions on the queue and continues cancelling them until CombatStarted is called.
		/// </summary>
		// Token: 0x060080EF RID: 33007 RVA: 0x002BB680 File Offset: 0x002B9880
		public void CombatEnded()
		{
			this._logger.Debug("Combat ended. Cancelling all non-executing combat actions in all queues", 1);
			this._isInCombat = false;
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				for (int i = 0; i < actionQueue.actions.Count; i++)
				{
					GameAction gameAction = actionQueue.actions[i];
					GameActionType actionType = gameAction.ActionType;
					bool flag = actionType - GameActionType.Combat <= 1;
					if (flag && gameAction.State != GameActionState.Executing)
					{
						Logger logger = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Cancelling action ");
						defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
						logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
						gameAction.Cancel();
						actionQueue.actions.RemoveAt(i);
						i--;
					}
					else
					{
						Logger logger2 = this._logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Not cancelling action ");
						defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
						defaultInterpolatedStringHandler.AppendLiteral(", type: ");
						defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(gameAction.ActionType);
						defaultInterpolatedStringHandler.AppendLiteral(", state: ");
						defaultInterpolatedStringHandler.AppendFormatted<GameActionState>(gameAction.State);
						logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					}
				}
				actionQueue.isCancellingPlayCardActions = false;
				actionQueue.isCancellingPlayerDrivenCombatActions = false;
				actionQueue.isCancellingCombatActions = true;
			}
			this.CheckIfQueuesEmpty();
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Allows combat actions to be added to player queues, but not be executed.
		/// </summary>
		// Token: 0x060080F0 RID: 33008 RVA: 0x002BB81C File Offset: 0x002B9A1C
		public void SetUpForCombat()
		{
			this._logger.Debug("Setting up for combat.", 1);
			this._wasReset = false;
			this._isInCombat = false;
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				actionQueue.isCancellingCombatActions = false;
			}
		}

		/// <summary>
		/// Allows combat actions to start executing.
		/// </summary>
		// Token: 0x060080F1 RID: 33009 RVA: 0x002BB890 File Offset: 0x002B9A90
		public void CombatStarted()
		{
			this._logger.Debug("Combat started.", 1);
			this._isInCombat = true;
			this._wasReset = false;
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				actionQueue.isCancellingCombatActions = false;
			}
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x002BB904 File Offset: 0x002B9B04
		public void Reset()
		{
			this._wasReset = true;
			this._actionQueues.Clear();
			this.CheckIfQueuesEmpty();
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Cancels all non-executing actions for a specific player's queue.
		/// </summary>
		// Token: 0x060080F3 RID: 33011 RVA: 0x002BB930 File Offset: 0x002B9B30
		public void CancelNonExecutingActionsForPlayer(ulong playerId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cancelling all non-executing actions owned by ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			ActionQueueSet.ActionQueue queue = this.GetQueue(playerId);
			for (int i = 0; i < queue.actions.Count; i++)
			{
				if (queue.actions[i].State == GameActionState.WaitingForExecution)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cancelling action ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(queue.actions[i]);
					logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					queue.actions[i].Cancel();
					queue.actions.RemoveAt(i);
					i--;
				}
			}
			this.CheckIfQueuesEmpty();
		}

		/// <summary>
		/// Cancel all queued actions of the specified type owned by a specific player.
		/// Used when we are selecting a card, e.g. for Survivor, but cards have been queued up for play.
		/// Note that this does not cancel the action at the front of the queue if it is executing.
		/// This is private because the timing must be synchronized with action enqueues across peers.
		/// </summary>
		/// <typeparam name="T">Type of actions to cancel.</typeparam>
		/// <param name="ownerId">The owner of the actions to cancel.</param>
		/// <param name="maxActionId">If non-null, only actions with ID less than this value will be cancelled.</param>
		// Token: 0x060080F4 RID: 33012 RVA: 0x002BBA08 File Offset: 0x002B9C08
		[NullableContext(0)]
		private void CancelNonExecutingActionsOfType<T>(ulong ownerId, uint? maxActionId) where T : GameAction
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Cancelling non-executing actions of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
			defaultInterpolatedStringHandler.AppendLiteral(" owned by ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(ownerId);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				if (actionQueue.ownerId == ownerId)
				{
					for (int i = 0; i < actionQueue.actions.Count; i++)
					{
						GameAction gameAction = actionQueue.actions[i];
						if (gameAction is T && gameAction.State == GameActionState.WaitingForExecution)
						{
							if (maxActionId != null)
							{
								uint value = gameAction.Id.Value;
								uint? num = maxActionId;
								if ((value >= num.GetValueOrDefault()) & (num != null))
								{
									goto IL_0125;
								}
							}
							Logger logger2 = this._logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Cancelling action ");
							defaultInterpolatedStringHandler.AppendFormatted<GameAction>(actionQueue.actions[i]);
							logger2.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
							gameAction.Cancel();
							actionQueue.actions.RemoveAt(i);
							i--;
						}
						IL_0125:;
					}
				}
			}
			this.CheckIfQueuesEmpty();
		}

		/// <summary>
		/// Resumes a GameAction after a player choice. This should never be called anywhere other than ActionQueueSynchronizer.
		/// </summary>
		// Token: 0x060080F5 RID: 33013 RVA: 0x002BBB84 File Offset: 0x002B9D84
		public void ResumeActionWithoutSynchronizing(uint id)
		{
			Action<uint> actionResumed = this.ActionResumed;
			if (actionResumed != null)
			{
				actionResumed(id);
			}
			uint andIncrementActionId = this.GetAndIncrementActionId();
			GameAction gameAction;
			ActionQueueSet.ActionQueue actionQueue;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!this.TryGetAction(id, out gameAction, out actionQueue) || gameAction.State != GameActionState.GatheringPlayerChoice)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Action with id ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
				defaultInterpolatedStringHandler.AppendLiteral(" is not ready to resume, enqueueing resumption");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				ActionQueueSet.ActionWaitingForResumption actionWaitingForResumption = new ActionQueueSet.ActionWaitingForResumption
				{
					oldId = id,
					newId = andIncrementActionId
				};
				this._actionsWaitingForResumption.Add(actionWaitingForResumption);
				return;
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Resuming action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
			defaultInterpolatedStringHandler.AppendLiteral(" after player choice");
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			actionQueue.isCancellingPlayCardActions = false;
			gameAction.ResumeAfterGatheringPlayerChoice(andIncrementActionId);
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged == null)
			{
				return;
			}
			actionQueueChanged();
		}

		/// <summary>
		/// Obtains an action by its ID.
		/// </summary>
		// Token: 0x060080F6 RID: 33014 RVA: 0x002BBC84 File Offset: 0x002B9E84
		[NullableContext(2)]
		private bool TryGetAction(uint id, out GameAction gameAction, out ActionQueueSet.ActionQueue queue)
		{
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				foreach (GameAction gameAction2 in actionQueue.actions)
				{
					uint? id2 = gameAction2.Id;
					if ((id2.GetValueOrDefault() == id) & (id2 != null))
					{
						queue = actionQueue;
						gameAction = gameAction2;
						return true;
					}
				}
			}
			queue = null;
			gameAction = null;
			return false;
		}

		/// <summary>
		/// Obtains an action queue by its owner's ID.
		/// </summary>
		// Token: 0x060080F7 RID: 33015 RVA: 0x002BBD40 File Offset: 0x002B9F40
		private ActionQueueSet.ActionQueue GetQueue(ulong playerId)
		{
			ActionQueueSet.ActionQueue actionQueue = this._actionQueues.FirstOrDefault<ActionQueueSet.ActionQueue>((ActionQueueSet.ActionQueue q) => q.ownerId == playerId);
			if (actionQueue == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to get local action queue for nonexistent player with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return actionQueue;
		}

		/// <summary>
		/// Removes an action from the front of its owner's queue. Should be called when an action finishes execution.
		/// </summary>
		// Token: 0x060080F8 RID: 33016 RVA: 0x002BBDB4 File Offset: 0x002B9FB4
		private void PopAction(GameAction action)
		{
			if (this._wasReset)
			{
				return;
			}
			bool flag = false;
			foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
			{
				if (actionQueue.actions.Count != 0)
				{
					if (actionQueue.actions[0] == action)
					{
						flag = true;
						actionQueue.actions.RemoveAt(0);
					}
					else
					{
						foreach (GameAction gameAction in actionQueue.actions)
						{
							if (gameAction == action)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Tried to pop action ");
								defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
								defaultInterpolatedStringHandler.AppendLiteral(", but it is not the top-most action for player ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
								defaultInterpolatedStringHandler.AppendLiteral("!");
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
			}
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to pop action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(", but we didn't find it in any queue!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Action actionQueueChanged = this.ActionQueueChanged;
			if (actionQueueChanged != null)
			{
				actionQueueChanged();
			}
			if (action.Exception == null)
			{
				this.CheckIfQueuesEmpty();
				return;
			}
			TaskCompletionSource queuesEmptyCompletionSource = this._queuesEmptyCompletionSource;
			if (queuesEmptyCompletionSource == null)
			{
				return;
			}
			queuesEmptyCompletionSource.SetException(action.Exception);
		}

		/// <summary>
		/// Used in replays so that we start with the correct action ID when replaying actions.
		/// </summary>
		/// <param name="nextId">The ID to assign to the next action.</param>
		// Token: 0x060080F9 RID: 33017 RVA: 0x002BBF48 File Offset: 0x002BA148
		public void FastForwardNextActionId(uint nextId)
		{
			this._nextId = nextId;
		}

		/// <summary>
		/// Checks if all action queues are empty and sets the completion source for tests that wait.
		/// </summary>
		// Token: 0x060080FA RID: 33018 RVA: 0x002BBF54 File Offset: 0x002BA154
		private void CheckIfQueuesEmpty()
		{
			if (this._actionQueues.All<ActionQueueSet.ActionQueue>((ActionQueueSet.ActionQueue q) => q.actions.Count == 0))
			{
				TaskCompletionSource queuesEmptyCompletionSource = this._queuesEmptyCompletionSource;
				if (queuesEmptyCompletionSource != null)
				{
					Task task = queuesEmptyCompletionSource.Task;
					if (task != null && !task.IsCompleted)
					{
						TaskCompletionSource queuesEmptyCompletionSource2 = this._queuesEmptyCompletionSource;
						if (queuesEmptyCompletionSource2 == null)
						{
							return;
						}
						queuesEmptyCompletionSource2.SetResult();
					}
				}
			}
		}

		/// <summary>
		/// Called when an action is enqueued or resumed.
		/// It is extremely important that this message is called deterministically across all peers. Action IDs are not
		/// synchronized across the network; since all action messages (enqueue/resume) are received in the same order that
		/// they are sent, we can trust that all peers will generate the same action IDs.
		///
		/// Note that we re-assign an ID to an action when it becomes ready. The reason we do this is to avoid subtle timing
		/// bugs with message ready timings. Re-assigning the ID to an ID that is greater than all actions currently in the
		/// queue ensures that the existing actions in the queue will execute in a deterministic fashion, and the newly ready
		/// action will execute after them.
		/// </summary>
		// Token: 0x060080FB RID: 33019 RVA: 0x002BBFBC File Offset: 0x002BA1BC
		private uint GetAndIncrementActionId()
		{
			uint nextId = this._nextId;
			this._nextId += 1U;
			return nextId;
		}

		// Token: 0x04002B23 RID: 11043
		private readonly Logger _logger;

		// Token: 0x04002B24 RID: 11044
		private readonly List<ActionQueueSet.ActionQueue> _actionQueues = new List<ActionQueueSet.ActionQueue>();

		// Token: 0x04002B25 RID: 11045
		private readonly List<ActionQueueSet.ActionWaitingForResumption> _actionsWaitingForResumption = new List<ActionQueueSet.ActionWaitingForResumption>();

		// Token: 0x04002B26 RID: 11046
		[Nullable(2)]
		private TaskCompletionSource _queuesEmptyCompletionSource;

		// Token: 0x04002B27 RID: 11047
		private uint _nextId;

		// Token: 0x04002B28 RID: 11048
		private bool _isInCombat;

		/// <summary>
		/// Set to true by <see cref="M:MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet.Reset" /> and back to false by <see cref="M:MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet.CombatStarted" />.
		/// This lets <see cref="M:MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet.PopAction(MegaCrit.Sts2.Core.GameActions.GameAction)" /> no-op for actions that were orphaned mid-execution when their queue was
		/// cleared out from under them.
		/// </summary>
		// Token: 0x04002B29 RID: 11049
		private bool _wasReset;

		// Token: 0x02002366 RID: 9062
		[NullableContext(0)]
		private class ActionQueue
		{
			// Token: 0x0400960C RID: 38412
			[Nullable(1)]
			public List<GameAction> actions = new List<GameAction>();

			// Token: 0x0400960D RID: 38413
			public ulong ownerId;

			// Token: 0x0400960E RID: 38414
			public bool isCancellingPlayCardActions;

			// Token: 0x0400960F RID: 38415
			public bool isCancellingPlayerDrivenCombatActions;

			// Token: 0x04009610 RID: 38416
			public bool isCancellingCombatActions = true;

			// Token: 0x04009611 RID: 38417
			public bool isPaused;
		}

		// Token: 0x02002367 RID: 9063
		[NullableContext(0)]
		private struct ActionWaitingForResumption
		{
			// Token: 0x04009612 RID: 38418
			public uint oldId;

			// Token: 0x04009613 RID: 38419
			public uint newId;
		}
	}
}
