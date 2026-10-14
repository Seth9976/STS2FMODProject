using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// Responsible for synchronizing actions across all peers in a deterministically-ordered manner.
	/// </summary>
	// Token: 0x02000C35 RID: 3125
	[NullableContext(1)]
	[Nullable(0)]
	public class ActionQueueSynchronizer
	{
		/// <summary>
		/// Next ID assigned to hook actions. Exposed only so the combat replay writer has access to it.
		/// </summary>
		// Token: 0x17002342 RID: 9026
		// (get) Token: 0x060080FC RID: 33020 RVA: 0x002BBFDF File Offset: 0x002BA1DF
		public uint NextHookId
		{
			get
			{
				return this._nextHookId;
			}
		}

		/// <summary>
		/// Current combat state. Dictates the paused state of queues.
		/// </summary>
		// Token: 0x17002343 RID: 9027
		// (get) Token: 0x060080FD RID: 33021 RVA: 0x002BBFE7 File Offset: 0x002BA1E7
		// (set) Token: 0x060080FE RID: 33022 RVA: 0x002BBFEF File Offset: 0x002BA1EF
		public ActionSynchronizerCombatState CombatState { get; private set; }

		// Token: 0x060080FF RID: 33023 RVA: 0x002BBFF8 File Offset: 0x002BA1F8
		public ActionQueueSynchronizer(IPlayerCollection players, ActionQueueSet actionQueueSet, RunLocationTargetedMessageBuffer messageBuffer, INetGameService netService)
		{
			this._playerCollection = players;
			this._actionQueueSet = actionQueueSet;
			this._netService = netService;
			this._messageBuffer = messageBuffer;
			this._messageBuffer.RegisterMessageHandler<RequestEnqueueActionMessage>(new MessageHandlerDelegate<RequestEnqueueActionMessage>(this.HandleRequestEnqueueActionMessage));
			this._messageBuffer.RegisterMessageHandler<ActionEnqueuedMessage>(new MessageHandlerDelegate<ActionEnqueuedMessage>(this.HandleActionEnqueuedMessage));
			this._messageBuffer.RegisterMessageHandler<RequestEnqueueHookActionMessage>(new MessageHandlerDelegate<RequestEnqueueHookActionMessage>(this.HandleRequestEnqueueHookActionMessage));
			this._messageBuffer.RegisterMessageHandler<HookActionEnqueuedMessage>(new MessageHandlerDelegate<HookActionEnqueuedMessage>(this.HandleHookActionEnqueuedMessage));
			this._messageBuffer.RegisterMessageHandler<RequestResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<RequestResumeActionAfterPlayerChoiceMessage>(this.HandleRequestResumeActionAfterPlayerChoiceMessage));
			this._messageBuffer.RegisterMessageHandler<ResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<ResumeActionAfterPlayerChoiceMessage>(this.HandleResumeActionAfterPlayerChoiceMessage));
			this._logger = new Logger("ActionQueueSynchronizer", LogType.Actions);
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x002BC0DC File Offset: 0x002BA2DC
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<RequestEnqueueActionMessage>(new MessageHandlerDelegate<RequestEnqueueActionMessage>(this.HandleRequestEnqueueActionMessage));
			this._messageBuffer.UnregisterMessageHandler<ActionEnqueuedMessage>(new MessageHandlerDelegate<ActionEnqueuedMessage>(this.HandleActionEnqueuedMessage));
			this._messageBuffer.UnregisterMessageHandler<RequestEnqueueHookActionMessage>(new MessageHandlerDelegate<RequestEnqueueHookActionMessage>(this.HandleRequestEnqueueHookActionMessage));
			this._messageBuffer.UnregisterMessageHandler<HookActionEnqueuedMessage>(new MessageHandlerDelegate<HookActionEnqueuedMessage>(this.HandleHookActionEnqueuedMessage));
			this._messageBuffer.UnregisterMessageHandler<RequestResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<RequestResumeActionAfterPlayerChoiceMessage>(this.HandleRequestResumeActionAfterPlayerChoiceMessage));
			this._messageBuffer.UnregisterMessageHandler<ResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<ResumeActionAfterPlayerChoiceMessage>(this.HandleResumeActionAfterPlayerChoiceMessage));
		}

		/// <summary>
		/// Call this when combat begins, ends, or the turn changes within combat.
		/// Certain actions, like card plays or potion usages, should always occur after the hooks that execute at the start
		/// of the player turn. However, the player is able to queue these actions before their turn actually starts. The
		/// synchronizer holds onto those actions until the start of the player turn, and then it sends all of them out.
		/// </summary>
		/// <param name="combatState">The new combat state.</param>
		// Token: 0x06008101 RID: 33025 RVA: 0x002BC174 File Offset: 0x002BA374
		public void SetCombatState(ActionSynchronizerCombatState combatState)
		{
			if (this.CombatState == combatState)
			{
				return;
			}
			ActionSynchronizerCombatState combatState2 = this.CombatState;
			this.CombatState = combatState;
			bool flag = combatState2 <= ActionSynchronizerCombatState.PreCombatSetup;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = combatState - ActionSynchronizerCombatState.PlayPhase <= 2;
				flag3 = flag4;
			}
			if (flag3)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Combat becomes ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState2);
				defaultInterpolatedStringHandler.AppendLiteral(". Signaling queues that combat has begun");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._actionQueueSet.CombatStarted();
			}
			switch (combatState)
			{
			case ActionSynchronizerCombatState.NotInCombat:
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(94, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Combat state becomes ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
				defaultInterpolatedStringHandler.AppendLiteral(". Cancelling deferred actions and cancelling all remaining combat actions");
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._actionQueueSet.CombatEnded();
				this._actionQueueSet.UnpauseAllPlayerQueues();
				foreach (GameAction gameAction in this._requestedActionsWaitingForPlayerTurn)
				{
					gameAction.Cancel();
				}
				this._requestedActionsWaitingForPlayerTurn.Clear();
				return;
			}
			case ActionSynchronizerCombatState.PreCombatSetup:
				if (combatState2 == ActionSynchronizerCombatState.NotInCombat)
				{
					Logger logger3 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Combat state becomes ");
					defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
					defaultInterpolatedStringHandler.AppendLiteral(". Allowing non-combat actions to execute, but not combat actions.");
					logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this._actionQueueSet.SetUpForCombat();
					return;
				}
				break;
			case ActionSynchronizerCombatState.PlayPhase:
			{
				Logger logger4 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Combat state becomes ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
				defaultInterpolatedStringHandler.AppendLiteral(". Requesting ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._requestedActionsWaitingForPlayerTurn.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" actions and unpausing action queues");
				logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				foreach (GameAction gameAction2 in this._requestedActionsWaitingForPlayerTurn)
				{
					this.RequestEnqueue(gameAction2);
				}
				this._requestedActionsWaitingForPlayerTurn.Clear();
				this._actionQueueSet.UnpauseAllPlayerQueues();
				return;
			}
			case ActionSynchronizerCombatState.EndTurnPhaseOne:
			{
				Logger logger5 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Combat state becomes ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
				defaultInterpolatedStringHandler.AppendLiteral(" (from ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState2);
				defaultInterpolatedStringHandler.AppendLiteral("). Starting to cancel all player-driven actions");
				logger5.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._actionQueueSet.StartCancellingAllPlayerDrivenCombatActions();
				return;
			}
			case ActionSynchronizerCombatState.NotPlayPhase:
			{
				Logger logger6 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Combat state becomes ");
				defaultInterpolatedStringHandler.AppendFormatted<ActionSynchronizerCombatState>(combatState);
				defaultInterpolatedStringHandler.AppendLiteral(". Pausing all player queues");
				logger6.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._actionQueueSet.PauseAllPlayerQueues();
				break;
			}
			default:
				return;
			}
		}

		/// <summary>
		/// Main entry point for enqueueing a GameAction.
		/// If you are the host, the GameAction is directly enqueued, and a message is sent to notify clients about the action.
		/// If you are the client, the GameAction is not enqueued. Instead, a message is sent to the host to request that
		/// the action be enqueued. The host sends back a confirmation, preserving ordering of the action queues.
		/// </summary>
		// Token: 0x06008102 RID: 33026 RVA: 0x002BC488 File Offset: 0x002BA688
		public void RequestEnqueue(GameAction action)
		{
			if (action.ActionType == GameActionType.CombatPlayPhaseOnly && this.CombatState == ActionSynchronizerCombatState.NotPlayPhase)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to request enqueue of action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" during the enemy turn. Deferring the action until the player turn");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._requestedActionsWaitingForPlayerTurn.Add(action);
				return;
			}
			NetGameType type = this._netService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				if (type == NetGameType.Client)
				{
					Logger logger2 = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Sending message to host requesting to enqueue action ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
					logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					RequestEnqueueActionMessage requestEnqueueActionMessage = default(RequestEnqueueActionMessage);
					requestEnqueueActionMessage.action = action.ToNetAction();
					requestEnqueueActionMessage.location = this._messageBuffer.CurrentLocation;
					this._netService.SendMessage<RequestEnqueueActionMessage>(requestEnqueueActionMessage);
					return;
				}
			}
			else
			{
				this.EnqueueAction(action, this._netService.NetId);
			}
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x002BC59C File Offset: 0x002BA79C
		public GenericHookGameAction GenerateHookAction(ulong ownerId, GameActionType gameActionType)
		{
			GenericHookGameAction hookActionForId = this.GetHookActionForId(this._nextHookId, ownerId, gameActionType);
			this._nextHookId += 1U;
			return hookActionForId;
		}

		/// <summary>
		/// Requests that a GenericHookGameAction be enqueued.
		/// Hook game actions are a little different than normal game actions. They contain arbitrary logic that is executed
		/// as part of a hook. Since hooks are executed on all peers, we can identify the actions by an incrementing ID.
		/// If on host, the action is immediately enqueued and a message is sent to clients.
		/// If on client, a message is sent to the host, and the host sends a message back to confirm that the message is
		/// enqueued.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06008104 RID: 33028 RVA: 0x002BC5C8 File Offset: 0x002BA7C8
		public void RequestEnqueueHookAction(GenericHookGameAction action)
		{
			NetGameType type = this._netService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				if (type == NetGameType.Client)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Sending message to host requesting to enqueue HOOK action with id ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(action.HookId);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
					logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					RequestEnqueueHookActionMessage requestEnqueueHookActionMessage = default(RequestEnqueueHookActionMessage);
					requestEnqueueHookActionMessage.hookActionId = action.HookId;
					requestEnqueueHookActionMessage.location = this._messageBuffer.CurrentLocation;
					requestEnqueueHookActionMessage.gameActionType = action.ActionType;
					this._netService.SendMessage<RequestEnqueueHookActionMessage>(requestEnqueueHookActionMessage);
					return;
				}
			}
			else
			{
				this.EnqueueHookAction(action);
			}
		}

		/// <summary>
		/// Requests that an action, paused for player choice, be resumed.
		/// If on the host, the action is immediately resumed and a message is sent to clients indicating this.
		/// If on a client, a message is sent to the host to ensure correct ordering, and the host sends a message back
		/// to confirm resumption of the action.
		/// </summary>
		/// <param name="action">The GameAction to resume.</param>
		// Token: 0x06008105 RID: 33029 RVA: 0x002BC688 File Offset: 0x002BA888
		public void RequestResumeActionAfterPlayerChoice(GameAction action)
		{
			NetGameType type = this._netService.Type;
			if (type - NetGameType.Singleplayer > 1)
			{
				if (type == NetGameType.Client)
				{
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Sending message to host requesting resumption of action ");
					defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
					defaultInterpolatedStringHandler.AppendLiteral(", id ");
					defaultInterpolatedStringHandler.AppendFormatted<uint?>(action.Id);
					defaultInterpolatedStringHandler.AppendLiteral(", at ");
					defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
					logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					RequestResumeActionAfterPlayerChoiceMessage requestResumeActionAfterPlayerChoiceMessage = default(RequestResumeActionAfterPlayerChoiceMessage);
					requestResumeActionAfterPlayerChoiceMessage.actionId = action.Id.Value;
					requestResumeActionAfterPlayerChoiceMessage.location = this._messageBuffer.CurrentLocation;
					this._netService.SendMessage<RequestResumeActionAfterPlayerChoiceMessage>(requestResumeActionAfterPlayerChoiceMessage);
					return;
				}
			}
			else
			{
				this.ResumeActionAfterPlayerChoice(action.Id.Value);
			}
		}

		// Token: 0x06008106 RID: 33030 RVA: 0x002BC764 File Offset: 0x002BA964
		private void EnqueueAction(GameAction action, ulong actionOwnerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._netService.Type == NetGameType.Host)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Sending message to clients to enqueue action ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				ActionEnqueuedMessage actionEnqueuedMessage = default(ActionEnqueuedMessage);
				actionEnqueuedMessage.playerId = actionOwnerId;
				actionEnqueuedMessage.location = this._messageBuffer.CurrentLocation;
				actionEnqueuedMessage.action = action.ToNetAction();
				this._netService.SendMessage<ActionEnqueuedMessage>(actionEnqueuedMessage);
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Enqueueing action ");
			defaultInterpolatedStringHandler.AppendFormatted<GameAction>(action);
			defaultInterpolatedStringHandler.AppendLiteral(" from owner ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionOwnerId);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._actionQueueSet.EnqueueWithoutSynchronizing(action);
		}

		// Token: 0x06008107 RID: 33031 RVA: 0x002BC858 File Offset: 0x002BAA58
		private void EnqueueHookAction(GenericHookGameAction gameAction)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._netService.Type == NetGameType.Host)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Sending message to clients to enqueue hook action ");
				defaultInterpolatedStringHandler.AppendFormatted<GenericHookGameAction>(gameAction);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				HookActionEnqueuedMessage hookActionEnqueuedMessage = default(HookActionEnqueuedMessage);
				hookActionEnqueuedMessage.hookActionId = gameAction.HookId;
				hookActionEnqueuedMessage.ownerId = gameAction.OwnerId;
				hookActionEnqueuedMessage.location = this._messageBuffer.CurrentLocation;
				hookActionEnqueuedMessage.gameActionType = gameAction.ActionType;
				this._netService.SendMessage<HookActionEnqueuedMessage>(hookActionEnqueuedMessage);
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Enqueueing HOOK action with id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(gameAction.HookId);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._actionQueueSet.EnqueueWithoutSynchronizing(gameAction);
		}

		/// <summary>
		/// Resumes a GameAction after a player choice. This call is synchronized, so it should only be called from the host
		/// or in response to messages.
		/// </summary>
		// Token: 0x06008108 RID: 33032 RVA: 0x002BC950 File Offset: 0x002BAB50
		private void ResumeActionAfterPlayerChoice(uint id)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._netService.Type == NetGameType.Host)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Sending message to clients to resume action id ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				ResumeActionAfterPlayerChoiceMessage resumeActionAfterPlayerChoiceMessage = default(ResumeActionAfterPlayerChoiceMessage);
				resumeActionAfterPlayerChoiceMessage.actionId = id;
				resumeActionAfterPlayerChoiceMessage.location = this._messageBuffer.CurrentLocation;
				this._netService.SendMessage<ResumeActionAfterPlayerChoiceMessage>(resumeActionAfterPlayerChoiceMessage);
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Resuming action with ID ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._actionQueueSet.ResumeActionWithoutSynchronizing(id);
		}

		// Token: 0x06008109 RID: 33033 RVA: 0x002BCA20 File Offset: 0x002BAC20
		private void HandleRequestEnqueueActionMessage(RequestEnqueueActionMessage message, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received action enqueue request while not host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received request enqueue action for action ");
			defaultInterpolatedStringHandler.AppendFormatted<INetAction>(message.action);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(") at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			GameAction gameAction = this.NetActionToGameAction(message.action, senderId);
			this.EnqueueAction(gameAction, senderId);
		}

		// Token: 0x0600810A RID: 33034 RVA: 0x002BCAC0 File Offset: 0x002BACC0
		private void HandleActionEnqueuedMessage(ActionEnqueuedMessage message, ulong _)
		{
			if (this._netService.Type != NetGameType.Client)
			{
				throw new InvalidOperationException("Received action enqueued message while not client!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received handle action enqueued message ");
			defaultInterpolatedStringHandler.AppendFormatted<ActionEnqueuedMessage>(message);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			GameAction gameAction = this.NetActionToGameAction(message.action, message.playerId);
			this.EnqueueAction(gameAction, message.playerId);
		}

		// Token: 0x0600810B RID: 33035 RVA: 0x002BCB50 File Offset: 0x002BAD50
		private void HandleRequestEnqueueHookActionMessage(RequestEnqueueHookActionMessage message, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received hook action enqueue request while not host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received HOOK request enqueue action from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(", hook id: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.hookActionId);
			defaultInterpolatedStringHandler.AppendLiteral(", at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			GenericHookGameAction hookActionForId = this.GetHookActionForId(message.hookActionId, senderId, message.gameActionType);
			this.EnqueueHookAction(hookActionForId);
		}

		// Token: 0x0600810C RID: 33036 RVA: 0x002BCBF4 File Offset: 0x002BADF4
		private void HandleHookActionEnqueuedMessage(HookActionEnqueuedMessage message, ulong _)
		{
			if (this._netService.Type != NetGameType.Client)
			{
				throw new InvalidOperationException("Received hook action enqueued message while not host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received HOOK request enqueue action, hook id: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.hookActionId);
			defaultInterpolatedStringHandler.AppendLiteral(", at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			GenericHookGameAction hookActionForId = this.GetHookActionForId(message.hookActionId, message.ownerId, message.gameActionType);
			this.EnqueueHookAction(hookActionForId);
		}

		/// <summary>
		/// Called on the host when a client wishes to resume an action after a player choice is made.
		/// </summary>
		// Token: 0x0600810D RID: 33037 RVA: 0x002BCC88 File Offset: 0x002BAE88
		private void HandleRequestResumeActionAfterPlayerChoiceMessage(RequestResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage, ulong senderId)
		{
			if (this._netService.Type != NetGameType.Host)
			{
				throw new InvalidOperationException("Received action ready request message while not host!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received action ready request message for action ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(afterPlayerChoiceMessage.actionId);
			defaultInterpolatedStringHandler.AppendLiteral(", sender ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(", at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ResumeActionAfterPlayerChoice(afterPlayerChoiceMessage.actionId);
		}

		/// <summary>
		/// Called on the client when a host resumes an action.
		/// </summary>
		// Token: 0x0600810E RID: 33038 RVA: 0x002BCD1C File Offset: 0x002BAF1C
		private void HandleResumeActionAfterPlayerChoiceMessage(ResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage, ulong _)
		{
			if (this._netService.Type != NetGameType.Client)
			{
				throw new InvalidOperationException("Received action ready message while not client!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received action ready message for action ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(afterPlayerChoiceMessage.actionId);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted(Log.Timestamp);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ResumeActionAfterPlayerChoice(afterPlayerChoiceMessage.actionId);
		}

		// Token: 0x0600810F RID: 33039 RVA: 0x002BCD9C File Offset: 0x002BAF9C
		private GameAction NetActionToGameAction(INetAction action, ulong actionOwnerId)
		{
			Player player = this._playerCollection.GetPlayer(actionOwnerId);
			if (player == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Action owner ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(actionOwnerId);
				defaultInterpolatedStringHandler.AppendLiteral(" for action ");
				defaultInterpolatedStringHandler.AppendFormatted<INetAction>(action);
				defaultInterpolatedStringHandler.AppendLiteral(" could not be mapped to a Player!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return action.ToGameAction(player);
		}

		/// <summary>
		/// This should only ever be called outside this class in replay playback!
		/// </summary>
		// Token: 0x06008110 RID: 33040 RVA: 0x002BCE10 File Offset: 0x002BB010
		public GenericHookGameAction GetHookActionForId(uint id, ulong ownerId, GameActionType gameActionType)
		{
			GenericHookGameAction action = this._hookActions.Find((GenericHookGameAction a) => a.HookId == id);
			if (action == null)
			{
				action = new GenericHookGameAction(id, ownerId, gameActionType);
				TaskHelper.RunSafely(action.ExecutionStartedTask.ContinueWith(delegate(Task _)
				{
					this.HookActionStarted(action);
				}));
				this._hookActions.Add(action);
			}
			else
			{
				if (action.OwnerId != ownerId)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Attempted to get hook for owner ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(ownerId);
					defaultInterpolatedStringHandler.AppendLiteral(" with hook ID ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(id);
					defaultInterpolatedStringHandler.AppendLiteral(", but the hook already existed and had owner ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(action.OwnerId);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (action.ActionType != gameActionType)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Generating GenericHookGameAction with type ");
					defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(gameActionType);
					defaultInterpolatedStringHandler.AppendLiteral(". Found one already enqueued, but with a mismatching game action type ");
					defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(action.ActionType);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			return action;
		}

		// Token: 0x06008111 RID: 33041 RVA: 0x002BCF86 File Offset: 0x002BB186
		private void HookActionStarted(GenericHookGameAction action)
		{
			this._hookActions.Remove(action);
		}

		// Token: 0x06008112 RID: 33042 RVA: 0x002BCF95 File Offset: 0x002BB195
		public void FastForwardHookId(uint hookId)
		{
			this._nextHookId = hookId;
		}

		// Token: 0x04002B2D RID: 11053
		private readonly ActionQueueSet _actionQueueSet;

		// Token: 0x04002B2E RID: 11054
		private readonly INetGameService _netService;

		// Token: 0x04002B2F RID: 11055
		private readonly Logger _logger;

		// Token: 0x04002B30 RID: 11056
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04002B31 RID: 11057
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04002B32 RID: 11058
		private readonly List<GenericHookGameAction> _hookActions = new List<GenericHookGameAction>();

		// Token: 0x04002B33 RID: 11059
		private uint _nextHookId;

		/// <summary>
		/// This list contains actions that were queued during the enemy turn or draw phase, but can only be executed during
		/// the player's play phase.
		/// </summary>
		// Token: 0x04002B34 RID: 11060
		private readonly List<GameAction> _requestedActionsWaitingForPlayerTurn = new List<GameAction>();
	}
}
