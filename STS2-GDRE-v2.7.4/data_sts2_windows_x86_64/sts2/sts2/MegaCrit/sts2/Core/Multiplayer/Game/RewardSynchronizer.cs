using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Temporary synchronizer that tells other players about rewards that a player receives in non-deterministic contexts
	/// Currently only used at the merchant.
	/// </summary>
	// Token: 0x020004A6 RID: 1190
	[NullableContext(1)]
	[Nullable(0)]
	public class RewardSynchronizer : IDisposable
	{
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x060047CA RID: 18378 RVA: 0x0021E2B5 File Offset: 0x0021C4B5
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x0021E2C8 File Offset: 0x0021C4C8
		public RewardSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, IPlayerCollection playerCollection, ulong localPlayerId)
		{
			this._gameService = gameService;
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._messageBuffer = messageBuffer;
			this._messageBuffer.RegisterMessageHandler<RewardObtainedMessage>(new MessageHandlerDelegate<RewardObtainedMessage>(this.HandleRewardObtainedMessage));
			this._messageBuffer.RegisterMessageHandler<GoldLostMessage>(new MessageHandlerDelegate<GoldLostMessage>(this.HandleGoldLostMessage));
			this._messageBuffer.RegisterMessageHandler<CardRemovedMessage>(new MessageHandlerDelegate<CardRemovedMessage>(this.HandleCardRemovedMessage));
			CombatManager.Instance.CombatEnded += this.OnCombatEnded;
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x0021E360 File Offset: 0x0021C560
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<RewardObtainedMessage>(new MessageHandlerDelegate<RewardObtainedMessage>(this.HandleRewardObtainedMessage));
			this._messageBuffer.UnregisterMessageHandler<GoldLostMessage>(new MessageHandlerDelegate<GoldLostMessage>(this.HandleGoldLostMessage));
			this._messageBuffer.UnregisterMessageHandler<CardRemovedMessage>(new MessageHandlerDelegate<CardRemovedMessage>(this.HandleCardRemovedMessage));
			CombatManager.Instance.CombatEnded -= this.OnCombatEnded;
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x0021E3C8 File Offset: 0x0021C5C8
		public void SyncLocalObtainedCard(CardModel card)
		{
			this.SyncLocalCardEvent(card, false);
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x0021E3D2 File Offset: 0x0021C5D2
		public void SyncLocalSkippedCard(CardModel card)
		{
			this.SyncLocalCardEvent(card, true);
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x0021E3DC File Offset: 0x0021C5DC
		private void SyncLocalCardEvent(CardModel card, bool skipped)
		{
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync card event ");
				defaultInterpolatedStringHandler.AppendFormatted<CardModel>(card);
				defaultInterpolatedStringHandler.AppendLiteral(" during combat! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardObtainedMessage rewardObtainedMessage = new RewardObtainedMessage
			{
				rewardType = RewardType.Card,
				cardModel = card,
				wasSkipped = skipped,
				location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<RewardObtainedMessage>(rewardObtainedMessage);
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x0021E478 File Offset: 0x0021C678
		public void SyncLocalObtainedRelic(RelicModel relic)
		{
			this.SyncLocalRelicEvent(relic, false);
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x0021E482 File Offset: 0x0021C682
		public void SyncLocalSkippedRelic(RelicModel relic)
		{
			this.SyncLocalRelicEvent(relic, true);
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x0021E48C File Offset: 0x0021C68C
		private void SyncLocalRelicEvent(RelicModel relic, bool skipped)
		{
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync relic event ");
				defaultInterpolatedStringHandler.AppendFormatted<RelicModel>(relic);
				defaultInterpolatedStringHandler.AppendLiteral(" during combat! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardObtainedMessage rewardObtainedMessage = new RewardObtainedMessage
			{
				rewardType = RewardType.Relic,
				relicModel = relic,
				wasSkipped = skipped,
				location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<RewardObtainedMessage>(rewardObtainedMessage);
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x0021E528 File Offset: 0x0021C728
		public void SyncLocalObtainedPotion(PotionModel potion)
		{
			this.SyncLocalPotionEvent(potion, false);
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0021E532 File Offset: 0x0021C732
		public void SyncLocalSkippedPotion(PotionModel potion)
		{
			this.SyncLocalPotionEvent(potion, true);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x0021E53C File Offset: 0x0021C73C
		private void SyncLocalPotionEvent(PotionModel potion, bool skipped)
		{
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync potion event ");
				defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
				defaultInterpolatedStringHandler.AppendLiteral(" during combat! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardObtainedMessage rewardObtainedMessage = new RewardObtainedMessage
			{
				rewardType = RewardType.Potion,
				potionModel = potion,
				wasSkipped = skipped,
				location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<RewardObtainedMessage>(rewardObtainedMessage);
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0021E5D8 File Offset: 0x0021C7D8
		public void SyncLocalObtainedGold(int goldAmount)
		{
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync obtaining ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(goldAmount);
				defaultInterpolatedStringHandler.AppendLiteral(" gold during combat! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			RewardObtainedMessage rewardObtainedMessage = new RewardObtainedMessage
			{
				rewardType = RewardType.Gold,
				goldAmount = new int?(goldAmount),
				wasSkipped = false,
				location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<RewardObtainedMessage>(rewardObtainedMessage);
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0021E67C File Offset: 0x0021C87C
		public void SyncLocalGoldLost(int goldLost)
		{
			if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync losing ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(goldLost);
				defaultInterpolatedStringHandler.AppendLiteral(" gold during combat! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			GoldLostMessage goldLostMessage = new GoldLostMessage
			{
				goldLost = goldLost,
				location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<GoldLostMessage>(goldLostMessage);
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0021E708 File Offset: 0x0021C908
		public async Task<bool> DoLocalCardRemoval()
		{
			CardRemovedMessage cardRemovedMessage = new CardRemovedMessage
			{
				Location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<CardRemovedMessage>(cardRemovedMessage);
			return await this.DoUnsyncedCardRemoval(this.LocalPlayer);
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x0021E74C File Offset: 0x0021C94C
		private void HandleRewardObtainedMessage(RewardObtainedMessage message, ulong senderId)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				RewardSynchronizer.BufferedMessage bufferedMessage = new RewardSynchronizer.BufferedMessage
				{
					senderId = senderId,
					rewardMessage = new RewardObtainedMessage?(message)
				};
				this._bufferedMessages.Add(bufferedMessage);
				return;
			}
			Player player = this._playerCollection.GetPlayer(senderId);
			MapPointHistoryEntry historyEntryFor = player.RunState.GetHistoryEntryFor(message.location.mapLocation);
			PlayerMapPointHistoryEntry playerMapPointHistoryEntry = null;
			if (historyEntryFor != null)
			{
				playerMapPointHistoryEntry = historyEntryFor.GetEntry(player.NetId);
			}
			switch (message.rewardType)
			{
			case RewardType.Card:
			{
				CardModel cardModel = message.cardModel;
				if (!message.wasSkipped)
				{
					player.RunState.AddCard(cardModel, player);
					TaskHelper.RunSafely(CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false));
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(cardModel.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" from card reward");
					Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" skipped ");
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>(cardModel.Id);
					defaultInterpolatedStringHandler.AppendLiteral(" from card reward");
					Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				if (playerMapPointHistoryEntry != null)
				{
					playerMapPointHistoryEntry.CardChoices.Add(new CardChoiceHistoryEntry(message.cardModel, !message.wasSkipped));
					return;
				}
				break;
			}
			case RewardType.Gold:
			{
				if (message.wasSkipped)
				{
					throw new NotImplementedException("Cannot handle skip gold reward message!");
				}
				TaskHelper.RunSafely(PlayerCmd.GainGold(message.goldAmount.Value, player, false));
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(message.goldAmount);
				defaultInterpolatedStringHandler.AppendLiteral(" gold from gold reward");
				Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			case RewardType.Potion:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!message.wasSkipped)
				{
					TaskHelper.RunSafely(PotionCmd.TryToProcure(message.potionModel.ToMutable(), player, -1));
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
					PotionModel potionModel = message.potionModel;
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>((potionModel != null) ? potionModel.Id : null);
					defaultInterpolatedStringHandler.AppendLiteral(" from potion reward");
					Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return;
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" skipped ");
				PotionModel potionModel2 = message.potionModel;
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>((potionModel2 != null) ? potionModel2.Id : null);
				defaultInterpolatedStringHandler.AppendLiteral(" from potion reward");
				Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (playerMapPointHistoryEntry != null)
				{
					playerMapPointHistoryEntry.PotionChoices.Add(new ModelChoiceHistoryEntry(message.potionModel.Id, !message.wasSkipped));
					return;
				}
				break;
			}
			case RewardType.Relic:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!message.wasSkipped)
				{
					TaskHelper.RunSafely(RelicCmd.Obtain(message.relicModel, player, -1));
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
					RelicModel relicModel = message.relicModel;
					defaultInterpolatedStringHandler.AppendFormatted<ModelId>((relicModel != null) ? relicModel.Id : null);
					defaultInterpolatedStringHandler.AppendLiteral(" from relic reward");
					Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					return;
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" skipped ");
				RelicModel relicModel2 = message.relicModel;
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>((relicModel2 != null) ? relicModel2.Id : null);
				defaultInterpolatedStringHandler.AppendLiteral(" from relic reward");
				Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (playerMapPointHistoryEntry != null)
				{
					playerMapPointHistoryEntry.RelicChoices.Add(new ModelChoiceHistoryEntry(message.relicModel.Id, !message.wasSkipped));
					return;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("rewardType", message.rewardType, null);
			}
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x0021EBA4 File Offset: 0x0021CDA4
		private void HandleGoldLostMessage(GoldLostMessage message, ulong senderId)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				RewardSynchronizer.BufferedMessage bufferedMessage = new RewardSynchronizer.BufferedMessage
				{
					senderId = senderId,
					goldLostMessage = new GoldLostMessage?(message)
				};
				this._bufferedMessages.Add(bufferedMessage);
				return;
			}
			Player player = this._playerCollection.GetPlayer(senderId);
			TaskHelper.RunSafely(PlayerCmd.LoseGold(message.goldLost, player, GoldLossType.Lost));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" lost ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(message.goldLost);
			defaultInterpolatedStringHandler.AppendLiteral(" gold");
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0021EC64 File Offset: 0x0021CE64
		private void HandleCardRemovedMessage(CardRemovedMessage message, ulong senderId)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				RewardSynchronizer.BufferedMessage bufferedMessage = new RewardSynchronizer.BufferedMessage
				{
					senderId = senderId,
					cardRemovedMessage = message
				};
				this._bufferedMessages.Add(bufferedMessage);
				return;
			}
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == this.LocalPlayer)
			{
				throw new InvalidOperationException("CardRemovedMessage should not be sent to the player removing the card!");
			}
			TaskHelper.RunSafely(this.DoUnsyncedCardRemoval(player));
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0021ECD4 File Offset: 0x0021CED4
		private void OnCombatEnded(CombatRoom _)
		{
			foreach (RewardSynchronizer.BufferedMessage bufferedMessage in this._bufferedMessages)
			{
				if (bufferedMessage.rewardMessage != null)
				{
					this.HandleRewardObtainedMessage(bufferedMessage.rewardMessage.Value, bufferedMessage.senderId);
				}
				else if (bufferedMessage.goldLostMessage != null)
				{
					this.HandleGoldLostMessage(bufferedMessage.goldLostMessage.Value, bufferedMessage.senderId);
				}
				else if (bufferedMessage.cardRemovedMessage != null)
				{
					this.HandleCardRemovedMessage(bufferedMessage.cardRemovedMessage, bufferedMessage.senderId);
				}
			}
			this._bufferedMessages.Clear();
		}

		/// <summary>
		/// Does card removal for the given player WITHOUT syncing it.
		/// This is a temporary bridge method for things that already execute deterministically (card removal rewards) and
		/// things that don't (merchant card removal).
		/// </summary>
		// Token: 0x060047DD RID: 18397 RVA: 0x0021ED98 File Offset: 0x0021CF98
		public async Task<bool> DoUnsyncedCardRemoval(Player player)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(new LocString("gameplay_ui", "COMBAT_REWARD_CARD_REMOVAL.selectionScreenPrompt"), 1)
			{
				Cancelable = true,
				RequireManualConfirmation = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(player, cardSelectorPrefs, null);
			CardModel card = enumerable.FirstOrDefault<CardModel>();
			bool flag;
			if (card != null)
			{
				await CardPileCmd.RemoveFromDeck(card, true);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(card.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" from deck");
				Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04001C28 RID: 7208
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04001C29 RID: 7209
		private readonly INetGameService _gameService;

		// Token: 0x04001C2A RID: 7210
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C2B RID: 7211
		private readonly ulong _localPlayerId;

		// Token: 0x04001C2C RID: 7212
		private readonly List<RewardSynchronizer.BufferedMessage> _bufferedMessages = new List<RewardSynchronizer.BufferedMessage>();

		// Token: 0x020019D1 RID: 6609
		[NullableContext(0)]
		private struct BufferedMessage
		{
			// Token: 0x040065FC RID: 26108
			public ulong senderId;

			// Token: 0x040065FD RID: 26109
			public RewardObtainedMessage? rewardMessage;

			// Token: 0x040065FE RID: 26110
			public GoldLostMessage? goldLostMessage;

			// Token: 0x040065FF RID: 26111
			[Nullable(2)]
			public CardRemovedMessage cardRemovedMessage;
		}
	}
}
