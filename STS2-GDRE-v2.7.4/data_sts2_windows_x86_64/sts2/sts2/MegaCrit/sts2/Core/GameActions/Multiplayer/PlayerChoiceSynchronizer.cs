using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// Synchronizes player choices that are gathered in the middle of a model's execution. Examples:
	/// * The card picked for discarding when playing Survivor
	/// * The card chosen to add to hand when playing Discovery
	/// * The card chosen to add to hand at the beginning of combat from Toolbox
	///
	/// The basic scheme here is:
	/// * All players (local and remote) generate an ID for the selection.
	/// * The owning player brings up the card selection UI on their side.
	/// * Once the owning player has made a choice, they send a message to all other players with the selection ID indicating
	///   what they chose.
	/// </summary>
	// Token: 0x02000C40 RID: 3136
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerChoiceSynchronizer : IDisposable
	{
		// Token: 0x17002357 RID: 9047
		// (get) Token: 0x0600815D RID: 33117 RVA: 0x002BDDF3 File Offset: 0x002BBFF3
		public IReadOnlyList<uint> ChoiceIds
		{
			get
			{
				return this._choiceIds;
			}
		}

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x0600815E RID: 33118 RVA: 0x002BDDFC File Offset: 0x002BBFFC
		// (remove) Token: 0x0600815F RID: 33119 RVA: 0x002BDE34 File Offset: 0x002BC034
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player, uint, NetPlayerChoiceResult> PlayerChoiceReceived;

		// Token: 0x06008160 RID: 33120 RVA: 0x002BDE6C File Offset: 0x002BC06C
		public PlayerChoiceSynchronizer(INetGameService netService, IPlayerCollection players)
		{
			this._players = players;
			this._netService = netService;
			netService.RegisterMessageHandler<PlayerChoiceMessage>(new MessageHandlerDelegate<PlayerChoiceMessage>(this.OnPlayerChoiceMessageReceived));
		}

		// Token: 0x06008161 RID: 33121 RVA: 0x002BDEC6 File Offset: 0x002BC0C6
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<PlayerChoiceMessage>(new MessageHandlerDelegate<PlayerChoiceMessage>(this.OnPlayerChoiceMessageReceived));
		}

		/// <summary>
		/// Reserves a choice ID to be passed to SyncLocalChoice or WaitForLocalChoice.
		/// If called during combat, this must be done in a deterministic context (i.e. before a GameAction is paused) so
		/// that checksums match up after action execution. SyncLocalChoice and WaitForRemoteChoice can be called outside of
		/// deterministic contexts (i.e. while a GameAction is paused and executing locally).
		/// </summary>
		/// <param name="player">The player for which the choice ID will be reserved.</param>
		// Token: 0x06008162 RID: 33122 RVA: 0x002BDEE0 File Offset: 0x002BC0E0
		public uint ReserveChoiceId(Player player)
		{
			int playerSlotIndex = this._players.GetPlayerSlotIndex(player);
			while (this._choiceIds.Count <= playerSlotIndex)
			{
				this._choiceIds.Add(0U);
			}
			uint num = this._choiceIds[playerSlotIndex];
			this._choiceIds[playerSlotIndex] = num + 1U;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Reserved choice id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(", next is ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._choiceIds[playerSlotIndex]);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			return num;
		}

		/// <summary>
		/// Sync a locally-chosen player choice with other peers.
		/// This should be called whenever a player choice is made by the local player. In tandem, WaitForRemoteChoice
		/// should be awaited on all remote peers.
		/// </summary>
		/// <param name="player">The player that chose the player choice. Should always be the local player.</param>
		/// <param name="choiceId">The ID of the choice, reserved through ReserveChoiceId.</param>
		/// <param name="result">The result of the player choice.</param>
		// Token: 0x06008163 RID: 33123 RVA: 0x002BDF9C File Offset: 0x002BC19C
		public void SyncLocalChoice(Player player, uint choiceId, PlayerChoiceResult result)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!this.ValidateChoiceId(player, choiceId))
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync local choice with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but player's choice ID is ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this.GetChoiceId(player));
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PlayerChoiceMessage playerChoiceMessage = new PlayerChoiceMessage
			{
				result = result.ToNetData(),
				choiceId = choiceId
			};
			Logger logger = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Sending player choice id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(", result ");
			defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceResult>(result);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			Action<Player, uint, NetPlayerChoiceResult> playerChoiceReceived = this.PlayerChoiceReceived;
			if (playerChoiceReceived != null)
			{
				playerChoiceReceived(player, choiceId, playerChoiceMessage.result);
			}
			this._netService.SendMessage<PlayerChoiceMessage>(playerChoiceMessage);
		}

		/// <summary>
		/// Waits for a choice to be received from a remote peer.
		/// This should be called on all remote peers whenever we expect a player choice to be made.
		/// On the machine of the player who is making a choice, SyncLocalChoice should be called.
		/// </summary>
		/// <param name="player">The player for whom we are awaiting a choice.</param>
		/// <param name="choiceId">The choice ID to await, obtained through ReserveChoiceId.</param>
		/// <returns>The result of the player choice that was made.</returns>
		// Token: 0x06008164 RID: 33124 RVA: 0x002BE0C0 File Offset: 0x002BC2C0
		public async Task<PlayerChoiceResult> WaitForRemoteChoice(Player player, uint choiceId)
		{
			if (this._netService.Type == NetGameType.Singleplayer)
			{
				throw new InvalidOperationException("Cannot wait for remote choice in singleplayer!");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!this.ValidateChoiceId(player, choiceId))
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to wait for remote choice with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", but player's choice ID is ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this.GetChoiceId(player));
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num = this._receivedChoices.FindIndex((PlayerChoiceSynchronizer.ReceivedChoice c) => c.choiceId == choiceId && c.senderId == player.NetId);
			PlayerChoiceSynchronizer.ReceivedChoice receivedChoice;
			if (num >= 0)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Was going to wait for remote choice ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" but we've already received it");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				receivedChoice = this._receivedChoices[num];
				this._receivedChoices.RemoveAt(num);
			}
			else
			{
				receivedChoice = new PlayerChoiceSynchronizer.ReceivedChoice
				{
					choiceId = choiceId,
					senderId = player.NetId,
					completionSource = new TaskCompletionSource<NetPlayerChoiceResult>()
				};
				Logger logger2 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Awaiting remote choice ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._receivedChoices.Add(receivedChoice);
			}
			NetPlayerChoiceResult netPlayerChoiceResult = await receivedChoice.completionSource.Task;
			NetPlayerChoiceResult netPlayerChoiceResult2 = netPlayerChoiceResult;
			PlayerChoiceResult playerChoiceResult = PlayerChoiceResult.FromNetData(player, this._players, netPlayerChoiceResult2);
			Logger logger3 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Finished waiting for remote choice ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(choiceId);
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceResult>(playerChoiceResult);
			logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			return playerChoiceResult;
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x002BE114 File Offset: 0x002BC314
		private void OnPlayerChoiceMessageReceived(PlayerChoiceMessage message, ulong senderId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Received choice from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" for choice ID ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(message.choiceId);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<NetPlayerChoiceResult>(message.result);
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			Player player = this._players.GetPlayer(senderId);
			this.OnReceivePlayerChoice(player, message.choiceId, message.result);
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x002BE1A4 File Offset: 0x002BC3A4
		public void FastForwardChoiceIds(List<uint> choiceIds)
		{
			this._logger.Debug("Fast-forwarded choice IDs to: " + string.Join<uint>(",", choiceIds), 1);
			this._choiceIds.Clear();
			this._choiceIds.AddRange(choiceIds);
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x002BE1DE File Offset: 0x002BC3DE
		public void ReceiveReplayChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
		{
			this.OnReceivePlayerChoice(player, choiceId, result);
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x002BE1EC File Offset: 0x002BC3EC
		private void OnReceivePlayerChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
		{
			Action<Player, uint, NetPlayerChoiceResult> playerChoiceReceived = this.PlayerChoiceReceived;
			if (playerChoiceReceived != null)
			{
				playerChoiceReceived(player, choiceId, result);
			}
			int num = this._receivedChoices.FindIndex((PlayerChoiceSynchronizer.ReceivedChoice c) => c.choiceId == choiceId && c.senderId == player.NetId);
			PlayerChoiceSynchronizer.ReceivedChoice receivedChoice;
			if (num >= 0)
			{
				this._logger.Debug("We are already waiting for the choice, fulfilling the task", 1);
				receivedChoice = this._receivedChoices[num];
				this._receivedChoices.RemoveAt(num);
			}
			else
			{
				receivedChoice = new PlayerChoiceSynchronizer.ReceivedChoice
				{
					choiceId = choiceId,
					senderId = player.NetId,
					completionSource = new TaskCompletionSource<NetPlayerChoiceResult>()
				};
				this._logger.Debug("We are not yet waiting for the choice, creating a new received choice", 1);
				this._receivedChoices.Add(receivedChoice);
			}
			receivedChoice.completionSource.SetResult(result);
		}

		// Token: 0x06008169 RID: 33129 RVA: 0x002BE2D1 File Offset: 0x002BC4D1
		private bool ValidateChoiceId(Player player, uint choiceId)
		{
			return choiceId < this.GetChoiceId(player);
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x002BE2E0 File Offset: 0x002BC4E0
		private uint GetChoiceId(Player player)
		{
			int playerSlotIndex = this._players.GetPlayerSlotIndex(player);
			if (playerSlotIndex >= this._choiceIds.Count)
			{
				return 0U;
			}
			return this._choiceIds[playerSlotIndex];
		}

		// Token: 0x04002B53 RID: 11091
		private readonly List<uint> _choiceIds = new List<uint>();

		// Token: 0x04002B54 RID: 11092
		private readonly List<PlayerChoiceSynchronizer.ReceivedChoice> _receivedChoices = new List<PlayerChoiceSynchronizer.ReceivedChoice>();

		// Token: 0x04002B55 RID: 11093
		private readonly Logger _logger = new Logger("PlayerChoiceSynchronizer", LogType.Actions);

		// Token: 0x04002B56 RID: 11094
		private readonly INetGameService _netService;

		// Token: 0x04002B57 RID: 11095
		private readonly IPlayerCollection _players;

		// Token: 0x02002377 RID: 9079
		[NullableContext(0)]
		private struct ReceivedChoice
		{
			// Token: 0x0400964D RID: 38477
			public ulong senderId;

			// Token: 0x0400964E RID: 38478
			public uint choiceId;

			// Token: 0x0400964F RID: 38479
			[Nullable(1)]
			public TaskCompletionSource<NetPlayerChoiceResult> completionSource;
		}
	}
}
