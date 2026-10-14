using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x02000495 RID: 1173
	[NullableContext(1)]
	[Nullable(0)]
	public class EventSynchronizer : IDisposable
	{
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600471F RID: 18207 RVA: 0x0021A707 File Offset: 0x00218907
		public IReadOnlyList<EventModel> Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06004720 RID: 18208 RVA: 0x0021A70F File Offset: 0x0021890F
		public bool IsShared
		{
			get
			{
				EventModel canonicalEvent = this._canonicalEvent;
				if (canonicalEvent == null)
				{
					throw new InvalidOperationException("Event is not in progress!");
				}
				return canonicalEvent.IsShared;
			}
		}

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06004721 RID: 18209 RVA: 0x0021A72C File Offset: 0x0021892C
		// (remove) Token: 0x06004722 RID: 18210 RVA: 0x0021A764 File Offset: 0x00218964
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player> PlayerVoteChanged;

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06004723 RID: 18211 RVA: 0x0021A799 File Offset: 0x00218999
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0021A7AC File Offset: 0x002189AC
		public EventSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService netService, IPlayerCollection playerCollection, IRunState runState, ulong localPlayerId, ulong seed)
		{
			this._netService = netService;
			this._messageBuffer = messageBuffer;
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._multiplayerOptionSelectionRng = new Rng(seed, "event_synchronizer");
			this._combatSynchronizer = new EventCombatSynchronizer(playerCollection, runState);
			this._messageBuffer.RegisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleEventOptionChosenMessage));
			this._messageBuffer.RegisterMessageHandler<VotedForSharedEventOptionMessage>(new MessageHandlerDelegate<VotedForSharedEventOptionMessage>(this.HandleVotedForSharedEventOptionMessage));
			this._messageBuffer.RegisterMessageHandler<SharedEventOptionChosenMessage>(new MessageHandlerDelegate<SharedEventOptionChosenMessage>(this.HandleSharedEventOptionChosenMessage));
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x0021A874 File Offset: 0x00218A74
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<OptionIndexChosenMessage>(new MessageHandlerDelegate<OptionIndexChosenMessage>(this.HandleEventOptionChosenMessage));
			this._messageBuffer.UnregisterMessageHandler<VotedForSharedEventOptionMessage>(new MessageHandlerDelegate<VotedForSharedEventOptionMessage>(this.HandleVotedForSharedEventOptionMessage));
			this._messageBuffer.UnregisterMessageHandler<SharedEventOptionChosenMessage>(new MessageHandlerDelegate<SharedEventOptionChosenMessage>(this.HandleSharedEventOptionChosenMessage));
		}

		/// <summary>
		/// Called when an event room is entered.
		/// </summary>
		/// <param name="canonicalEvent">The canonical version of the event that will be started.</param>
		/// <param name="isPrefinished">If the event is already pre-finished</param>
		/// <param name="debugOnStart">A method to call to inject some debug code right after the model is cloned. The mutable
		/// model instances will be passed to the method.</param>
		// Token: 0x06004726 RID: 18214 RVA: 0x0021A8C8 File Offset: 0x00218AC8
		public void BeginEvent(EventModel canonicalEvent, bool isPrefinished = false, [Nullable(new byte[] { 2, 1 })] Action<EventModel> debugOnStart = null)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Beginning event ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(canonicalEvent.Id);
			defaultInterpolatedStringHandler.AppendLiteral(", shared: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(canonicalEvent.IsShared);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			for (int i = this._playerVotes.Count; i < this._playerCollection.Players.Count; i++)
			{
				this._playerVotes.Add(null);
			}
			foreach (EventModel eventModel in this._events)
			{
				if (!eventModel.IsFinished)
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Beginning new event ");
					defaultInterpolatedStringHandler.AppendFormatted<EventModel>(canonicalEvent);
					defaultInterpolatedStringHandler.AppendLiteral(", but event ");
					defaultInterpolatedStringHandler.AppendFormatted<EventModel>(eventModel);
					defaultInterpolatedStringHandler.AppendLiteral(" for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(eventModel.Owner.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" is not yet finished!");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					eventModel.EnsureCleanup();
				}
			}
			this._events.Clear();
			this._pendingOptionTasks.Clear();
			this.ClearPlayerVotes();
			this._pageIndex = 0U;
			this._canonicalEvent = canonicalEvent;
			foreach (Player player in this._playerCollection.Players)
			{
				EventModel eventModel2 = canonicalEvent.ToMutable();
				if (debugOnStart != null)
				{
					debugOnStart(eventModel2);
				}
				this._events.Add(eventModel2);
				TaskHelper.RunSafely(eventModel2.BeginEvent(player, this._combatSynchronizer, isPrefinished));
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Event ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(eventModel2.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" began for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" with options: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<EventOption>(",", eventModel2.CurrentOptions));
				logger3.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
		}

		/// <summary>Received during shared events. Records a player's vote for a given event option. </summary>
		// Token: 0x06004727 RID: 18215 RVA: 0x0021AB40 File Offset: 0x00218D40
		private void HandleVotedForSharedEventOptionMessage(VotedForSharedEventOptionMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("VotedForSharedEventOptionMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" from player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" for option ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" on page ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.pageIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (!this.IsShared)
			{
				throw new InvalidOperationException("Received VotedForSharedEventOptionMessage during a non-shared event!");
			}
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("VotedForSharedEventOptionMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" that doesn't exist!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.PlayerVotedForSharedOptionIndex(player, message.optionIndex, message.pageIndex);
		}

		/// <summary> Records a player's vote. Called in response to a remote player's message, or immediately when the
		/// local player votes for a shared event option. </summary>
		// Token: 0x06004728 RID: 18216 RVA: 0x0021AC50 File Offset: 0x00218E50
		private void PlayerVotedForSharedOptionIndex(Player player, uint optionIndex, uint pageIndex)
		{
			if (pageIndex < this._pageIndex)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(101, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Received message from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" voting for option ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(optionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" on page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but we are on greater page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this._pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral(". Ignoring vote");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			if (pageIndex > this._pageIndex)
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(101, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Received message from player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" voting for option ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(optionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" on page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but we are on lesser page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this._pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral(". This is a bug!");
				logger2.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				return;
			}
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			this._playerVotes[playerSlotIndex] = new uint?(optionIndex);
			Action<Player> playerVoteChanged = this.PlayerVoteChanged;
			if (playerVoteChanged != null)
			{
				playerVoteChanged(player);
			}
			if (this._playerVotes.All<uint?>((uint? p) => p != null) && this._netService.Type != NetGameType.Client)
			{
				this._logger.Debug("All votes received and we are host. Choosing shared event option", 1);
				this.ChooseSharedEventOption();
			}
		}

		/// <summary> Called on the host when all votes for a shared event page are received. Decides which event option
		/// should be chosen and notifies all clients of the option chosen. </summary>
		// Token: 0x06004729 RID: 18217 RVA: 0x0021AE00 File Offset: 0x00219000
		private void ChooseSharedEventOption()
		{
			if (this._netService.Type == NetGameType.Client)
			{
				throw new InvalidOperationException("Only host should be choosing shared event option!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
			defaultInterpolatedStringHandler.AppendLiteral("All votes received on host for shared event ");
			defaultInterpolatedStringHandler.AppendFormatted<EventModel>(this._canonicalEvent);
			defaultInterpolatedStringHandler.AppendLiteral(", choosing option");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			uint value = this._multiplayerOptionSelectionRng.NextItem<uint?>(this._playerVotes).Value;
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Shared event option ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(value);
			defaultInterpolatedStringHandler.AppendLiteral(" was chosen");
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			SharedEventOptionChosenMessage sharedEventOptionChosenMessage = new SharedEventOptionChosenMessage
			{
				optionIndex = value,
				pageIndex = this._pageIndex,
				location = this._messageBuffer.CurrentLocation
			};
			this._netService.SendMessage<SharedEventOptionChosenMessage>(sharedEventOptionChosenMessage);
			this.ChooseOptionForSharedEvent(value);
		}

		/// <summary> Called on clients when the host receives all player votes and decides on an event option to choose. </summary>
		// Token: 0x0600472A RID: 18218 RVA: 0x0021AF04 File Offset: 0x00219104
		private void HandleSharedEventOptionChosenMessage(SharedEventOptionChosenMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("SharedEventOptionChosenMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" for option ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" on page ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.pageIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this._netService.Type != NetGameType.Client)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("SharedEventOptionChosenMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" on non-client! ");
				defaultInterpolatedStringHandler.AppendFormatted<NetGameType>(this._netService.Type);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this._pageIndex != message.pageIndex)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("SharedEventOptionChosenMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" for page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(message.pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" while we were on page ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this._pageIndex);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.ChooseOptionForSharedEvent(message.optionIndex);
		}

		/// <summary> Called on all peers during non-shared events when one peer selects an event option. </summary>
		// Token: 0x0600472B RID: 18219 RVA: 0x0021B05C File Offset: 0x0021925C
		private void HandleEventOptionChosenMessage(OptionIndexChosenMessage message, ulong senderId)
		{
			if (message.type != OptionIndexType.Event)
			{
				return;
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received ");
			defaultInterpolatedStringHandler.AppendFormatted("OptionIndexChosenMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" from player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" for event option index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.optionIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this.IsShared)
			{
				throw new InvalidOperationException("Received OptionIndexChosenMessage during a shared event!");
			}
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received EventOptionChosenMessage for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" that doesn't exist!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.ChooseOptionForEvent(player, (int)message.optionIndex);
		}

		/// <summary>
		/// Called when the local player selects an event option button.
		/// During shared events, this votes for the event option.
		/// During non-shared events, this immediately executes the option and sends the event option to all other peers.
		/// </summary>
		/// <param name="index">The index of the event option that was chosen.</param>
		// Token: 0x0600472C RID: 18220 RVA: 0x0021B13C File Offset: 0x0021933C
		public void ChooseLocalOption(int index)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.IsShared)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Local player voted for shared event option index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this.PlayerVotedForSharedOptionIndex(this.LocalPlayer, (uint)index, this._pageIndex);
				VotedForSharedEventOptionMessage votedForSharedEventOptionMessage = default(VotedForSharedEventOptionMessage);
				votedForSharedEventOptionMessage.optionIndex = (uint)index;
				votedForSharedEventOptionMessage.pageIndex = this._pageIndex;
				votedForSharedEventOptionMessage.location = this._messageBuffer.CurrentLocation;
				this._netService.SendMessage<VotedForSharedEventOptionMessage>(votedForSharedEventOptionMessage);
				return;
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Local player chose event option index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ChooseOptionForEvent(this.LocalPlayer, index);
			OptionIndexChosenMessage optionIndexChosenMessage = default(OptionIndexChosenMessage);
			optionIndexChosenMessage.type = OptionIndexType.Event;
			optionIndexChosenMessage.optionIndex = (uint)index;
			optionIndexChosenMessage.location = this._messageBuffer.CurrentLocation;
			this._netService.SendMessage<OptionIndexChosenMessage>(optionIndexChosenMessage);
		}

		/// <summary> Executes the shared event option on all players' event model instances and clears player votes. </summary>
		// Token: 0x0600472D RID: 18221 RVA: 0x0021B248 File Offset: 0x00219448
		private void ChooseOptionForSharedEvent(uint optionIndex)
		{
			if (!this.IsShared)
			{
				throw new InvalidOperationException("ChooseOptionForSharedEvent called during non-shared event!");
			}
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Choosing option index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" for shared event on page ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._pageIndex);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.ClearPlayerVotes();
			this._pageIndex += 1U;
			foreach (Player player in this._playerCollection.Players)
			{
				this.ChooseOptionForEvent(player, (int)optionIndex);
			}
		}

		/// <summary>
		/// Executes the given event option for a given player.
		/// </summary>
		// Token: 0x0600472E RID: 18222 RVA: 0x0021B30C File Offset: 0x0021950C
		private void ChooseOptionForEvent(Player player, int optionIndex)
		{
			EventModel eventForPlayer = this.GetEventForPlayer(player);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (eventForPlayer.IsFinished)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Option chosen for player ");
				defaultInterpolatedStringHandler.AppendFormatted<Player>(player);
				defaultInterpolatedStringHandler.AppendLiteral(" on ");
				defaultInterpolatedStringHandler.AppendFormatted<EventModel>(eventForPlayer);
				defaultInterpolatedStringHandler.AppendLiteral(", but it is already finished!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (optionIndex >= eventForPlayer.CurrentOptions.Count)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" attempted to choose option index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(optionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" in event ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(eventForPlayer.Id);
				defaultInterpolatedStringHandler.AppendLiteral(", but there were only ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(eventForPlayer.CurrentOptions.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" options available!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Option index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" chosen for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" in event ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(eventForPlayer.Id);
			defaultInterpolatedStringHandler.AppendLiteral(". Choice key: ");
			defaultInterpolatedStringHandler.AppendFormatted(eventForPlayer.CurrentOptions[optionIndex].TextKey);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			EventOption eventOption = eventForPlayer.CurrentOptions[optionIndex];
			this._pendingOptionTasks.Add(TaskHelper.RunSafely(eventOption.Chosen()));
			this.SaveEventOptionToHistory(player, eventOption);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0021B4C0 File Offset: 0x002196C0
		private void SaveEventOptionToHistory(Player player, EventOption option)
		{
			if (!option.ShouldSaveChoiceToHistory)
			{
				return;
			}
			EventOptionHistoryEntry eventOptionHistoryEntry = new EventOptionHistoryEntry
			{
				Title = option.HistoryName,
				Variables = new Dictionary<string, object>()
			};
			if (option.ShouldSaveVariablesToHistory)
			{
				foreach (KeyValuePair<string, object> keyValuePair in option.HistoryName.Variables)
				{
					eventOptionHistoryEntry.Variables[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			MapPointHistoryEntry currentMapPointHistoryEntry = player.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry == null)
			{
				return;
			}
			currentMapPointHistoryEntry.GetEntry(player.NetId).EventChoices.Add(eventOptionHistoryEntry);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0021B580 File Offset: 0x00219780
		private void ClearPlayerVotes()
		{
			for (int i = 0; i < this._playerVotes.Count; i++)
			{
				this._playerVotes[i] = null;
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0021B5B8 File Offset: 0x002197B8
		public uint? GetPlayerVote(Player player)
		{
			return this._playerVotes[this._playerCollection.GetPlayerSlotIndex(player)];
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0021B5D1 File Offset: 0x002197D1
		public EventModel GetLocalEvent()
		{
			return this.GetEventForPlayer(this.LocalPlayer);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x0021B5E0 File Offset: 0x002197E0
		public EventModel GetEventForPlayer(Player player)
		{
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			return this._events[playerSlotIndex];
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x0021B608 File Offset: 0x00219808
		public void ResumeEvents(AbstractRoom exitedRoom)
		{
			foreach (EventModel eventModel in this._events)
			{
				TaskHelper.RunSafely(eventModel.Resume(exitedRoom));
			}
		}

		/// <summary>
		/// Called when the room is entered to ensure that state necessary for the event layout is generated.
		/// </summary>
		/// <param name="localEvent"></param>
		// Token: 0x06004735 RID: 18229 RVA: 0x0021B664 File Offset: 0x00219864
		public void GenerateInternalCombatStateIfNecessary(EventModel localEvent)
		{
			this._combatSynchronizer.InitializeForEvent(localEvent);
		}

		/// <summary>
		/// Called before the room is exited to ensure combat synchronizer state is reset.
		/// If we end up supporting multiple event combats in a single event, this will likely have to be refactored.
		/// </summary>
		// Token: 0x06004736 RID: 18230 RVA: 0x0021B672 File Offset: 0x00219872
		public void BeforeExitingRoom()
		{
			this._combatSynchronizer.ResetState();
		}

		/// <summary>
		/// Awaits all in-flight event option tasks. Called before the room exit checksum to ensure that all
		/// players' event handlers have finished mutating state. Without this, a peer whose local event
		/// finished quickly (e.g. Touch a Mirror) can exit the room while a remote player's slow handler
		/// (e.g. Shatter's per-card deck duplication loop) is still running, causing the checksum to
		/// capture incomplete state.
		/// </summary>
		// Token: 0x06004737 RID: 18231 RVA: 0x0021B680 File Offset: 0x00219880
		public async Task AwaitPendingOptionTasks()
		{
			try
			{
				await Task.WhenAll(this._pendingOptionTasks);
			}
			catch (Exception)
			{
			}
			this._pendingOptionTasks.Clear();
		}

		// Token: 0x04001BD6 RID: 7126
		private readonly INetGameService _netService;

		// Token: 0x04001BD7 RID: 7127
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04001BD8 RID: 7128
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001BD9 RID: 7129
		private readonly ulong _localPlayerId;

		// Token: 0x04001BDA RID: 7130
		private readonly List<EventModel> _events = new List<EventModel>();

		/// <summary>
		/// The canonical event we are currently in the middle of.
		/// </summary>
		// Token: 0x04001BDB RID: 7131
		[Nullable(2)]
		private EventModel _canonicalEvent;

		/// <summary>
		/// Votes received from players on which option to choose. Only set during shared events.
		/// </summary>
		// Token: 0x04001BDC RID: 7132
		private readonly List<uint?> _playerVotes = new List<uint?>();

		/// <summary>
		/// The "page" of options we are on. Increments every time an option is chosen.
		/// </summary>
		// Token: 0x04001BDD RID: 7133
		private uint _pageIndex;

		/// <summary>
		/// Tracks in-flight event option tasks so we can await them before room exit.
		/// Option handlers run as fire-and-forget tasks via TaskHelper.RunSafely. Without tracking,
		/// the room can exit (and checksum) before a slow handler (e.g. Shatter's per-card loop)
		/// finishes on the peer whose local event completed first, causing state divergence.
		/// </summary>
		// Token: 0x04001BDE RID: 7134
		private readonly List<Task> _pendingOptionTasks = new List<Task>();

		/// <summary>
		/// Determines what event option is chosen in multiplayer when multiple options have the same number of votes.
		/// This is only rolled on the host. Do not depend on the RNG being deterministic across peers.
		/// </summary>
		// Token: 0x04001BDF RID: 7135
		private readonly Rng _multiplayerOptionSelectionRng;

		// Token: 0x04001BE0 RID: 7136
		private readonly EventCombatSynchronizer _combatSynchronizer;

		// Token: 0x04001BE1 RID: 7137
		private readonly Logger _logger = new Logger("EventSynchronizer", LogType.GameSync);
	}
}
