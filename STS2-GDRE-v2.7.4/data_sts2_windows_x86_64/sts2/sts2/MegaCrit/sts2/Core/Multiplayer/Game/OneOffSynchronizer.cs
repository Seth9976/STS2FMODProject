using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// A synchronizer for a few one-off scenarios that don't fit in any other synchronizer.
	/// This should be used pretty rarely, and we should be on the lookout for patterns that emerge with new content.
	/// </summary>
	// Token: 0x020004A2 RID: 1186
	[NullableContext(1)]
	[Nullable(0)]
	public class OneOffSynchronizer : IDisposable
	{
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06004786 RID: 18310 RVA: 0x0021C773 File Offset: 0x0021A973
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0021C788 File Offset: 0x0021A988
		public OneOffSynchronizer(RunLocationTargetedMessageBuffer messageBuffer, INetGameService gameService, IPlayerCollection playerCollection, ulong localPlayerId)
		{
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._gameService = gameService;
			this._messageBuffer = messageBuffer;
			messageBuffer.RegisterMessageHandler<MerchantCardRemovalMessage>(new MessageHandlerDelegate<MerchantCardRemovalMessage>(this.HandleMerchantCardRemoval));
			messageBuffer.RegisterMessageHandler<TreasureChestOpenedMessage>(new MessageHandlerDelegate<TreasureChestOpenedMessage>(this.HandleTreasureChestOpenedMessage));
			messageBuffer.RegisterMessageHandler<CrystalSphereRewardsMessage>(new MessageHandlerDelegate<CrystalSphereRewardsMessage>(this.HandleCrystalSphereRewardsMessage));
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0021C7F0 File Offset: 0x0021A9F0
		public void Dispose()
		{
			this._messageBuffer.UnregisterMessageHandler<MerchantCardRemovalMessage>(new MessageHandlerDelegate<MerchantCardRemovalMessage>(this.HandleMerchantCardRemoval));
			this._messageBuffer.UnregisterMessageHandler<TreasureChestOpenedMessage>(new MessageHandlerDelegate<TreasureChestOpenedMessage>(this.HandleTreasureChestOpenedMessage));
			this._messageBuffer.UnregisterMessageHandler<CrystalSphereRewardsMessage>(new MessageHandlerDelegate<CrystalSphereRewardsMessage>(this.HandleCrystalSphereRewardsMessage));
		}

		/// <summary>
		/// Does merchant card removal for the local player.
		/// </summary>
		/// <param name="goldCost">The cost, in gold, that we should deduct from the player's gold count.</param>
		/// <param name="cancelable">If false, you are required to remove a card</param>
		/// <returns>A task that completes when the player finishes the removal flow. Result is true if the player removed
		/// a card, false otherwise.</returns>
		// Token: 0x06004789 RID: 18313 RVA: 0x0021C844 File Offset: 0x0021AA44
		public Task<bool> DoLocalMerchantCardRemoval(int goldCost, bool cancelable = true)
		{
			MerchantCardRemovalMessage merchantCardRemovalMessage = new MerchantCardRemovalMessage
			{
				goldCost = goldCost,
				Location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<MerchantCardRemovalMessage>(merchantCardRemovalMessage);
			return this.DoMerchantCardRemoval(this.LocalPlayer, goldCost, cancelable);
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0021C88C File Offset: 0x0021AA8C
		private void HandleMerchantCardRemoval(MerchantCardRemovalMessage message, ulong senderId)
		{
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == this.LocalPlayer)
			{
				throw new InvalidOperationException("MerchantCardRemovalMessage should not be sent to the player removing the card!");
			}
			TaskHelper.RunSafely(this.DoMerchantCardRemoval(player, message.goldCost, true));
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0021C8D0 File Offset: 0x0021AAD0
		private async Task<bool> DoMerchantCardRemoval(Player player, int goldCost, bool cancelable = true)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1)
			{
				Cancelable = cancelable,
				RequireManualConfirmation = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(player, cardSelectorPrefs, null);
			CardModel card = enumerable.FirstOrDefault<CardModel>();
			if (card != null)
			{
				await PlayerCmd.LoseGold(goldCost, player, GoldLossType.Spent);
				await CardPileCmd.RemoveFromDeck(card, true);
				player.ExtraFields.CardShopRemovalsUsed++;
			}
			return card != null;
		}

		/// <summary>
		/// Should be called after a player opens the chest in the treasure room to synchronize gold gain and other effects.
		/// </summary>
		/// <returns>The amount of gold given to the player.</returns>
		// Token: 0x0600478C RID: 18316 RVA: 0x0021C924 File Offset: 0x0021AB24
		public Task<int> DoLocalTreasureRoomRewards()
		{
			TreasureChestOpenedMessage treasureChestOpenedMessage = new TreasureChestOpenedMessage
			{
				Location = this._messageBuffer.CurrentLocation
			};
			this._gameService.SendMessage<TreasureChestOpenedMessage>(treasureChestOpenedMessage);
			return this.DoTreasureRoomRewards(this.LocalPlayer);
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x0021C960 File Offset: 0x0021AB60
		private void HandleTreasureChestOpenedMessage(TreasureChestOpenedMessage message, ulong senderId)
		{
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == this.LocalPlayer)
			{
				throw new InvalidOperationException("TreasureChestOpenedMessage should not be sent to the player who opened the treasure chest!");
			}
			TaskHelper.RunSafely(this.DoTreasureRoomRewards(player));
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0021C99C File Offset: 0x0021AB9C
		private async Task<int> DoTreasureRoomRewards(Player player)
		{
			int num;
			if (!Hook.ShouldGenerateTreasure(player.RunState, player))
			{
				num = 0;
			}
			else
			{
				double gold = (double)player.PlayerRng.Rewards.NextInt(42, 53);
				if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
				{
					gold *= AscensionHelper.PovertyAscensionGoldMultiplier;
				}
				await PlayerCmd.GainGold((int)gold, player, false);
				double num2 = gold;
				gold = num2 + (double)(await this.TryHandleSpoilsMap(player));
				num = (int)gold;
			}
			return num;
		}

		/// <summary>
		/// Special one-off logic for Spoils Map. If more stuff ends up going here, make this a hook
		/// </summary>
		/// <returns>The amount of gold given by spoils map, if any.</returns>
		// Token: 0x0600478F RID: 18319 RVA: 0x0021C9E8 File Offset: 0x0021ABE8
		private async Task<int> TryHandleSpoilsMap(Player player)
		{
			MapPoint mapPoint = ((player.RunState.CurrentMapCoord != null) ? player.RunState.Map.GetPoint(player.RunState.CurrentMapCoord.Value) : null);
			int num;
			if (mapPoint == null)
			{
				num = 0;
			}
			else if (!mapPoint.Quests.Any<AbstractModel>((AbstractModel q) => q is SpoilsMap))
			{
				num = 0;
			}
			else
			{
				List<SpoilsMap> list = player.Deck.Cards.OfType<SpoilsMap>().ToList<SpoilsMap>();
				int num2 = 0;
				foreach (SpoilsMap spoilsMap in list)
				{
					int num3 = num2;
					int num4 = await spoilsMap.OnQuestComplete();
					num2 = num3 + num4;
				}
				List<SpoilsMap>.Enumerator enumerator = default(List<SpoilsMap>.Enumerator);
				num = num2;
			}
			return num;
		}

		/// <summary>
		/// Special one-off logic for presenting crystal sphere rewards to the player, and notifying other players that
		/// crystal sphere rewards are beginning for the local player.
		/// </summary>
		// Token: 0x06004790 RID: 18320 RVA: 0x0021CA2C File Offset: 0x0021AC2C
		public async Task DoLocalCrystalSphereRewards(Player owner, Rng rng, List<CrystalSphereItem> revealed)
		{
			if (owner != LocalContext.GetMe(this._playerCollection))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Trying to sync crystal sphere rewards for non-local player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(owner.NetId);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<SerializableCrystalSphereItem> list = revealed.Select<CrystalSphereItem, SerializableCrystalSphereItem>((CrystalSphereItem r) => r.ToSerializable()).ToList<SerializableCrystalSphereItem>();
			CrystalSphereRewardsMessage crystalSphereRewardsMessage = new CrystalSphereRewardsMessage
			{
				Location = this._messageBuffer.CurrentLocation,
				rewards = list
			};
			this._gameService.SendMessage<CrystalSphereRewardsMessage>(crystalSphereRewardsMessage);
			await this.OfferCrystalSphereRewards(owner, revealed, rng);
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0021CA88 File Offset: 0x0021AC88
		private void HandleCrystalSphereRewardsMessage(CrystalSphereRewardsMessage message, ulong senderId)
		{
			Player player = this._playerCollection.GetPlayer(senderId);
			if (player == this.LocalPlayer)
			{
				throw new InvalidOperationException("CrystalSphereRewardsMessage should not be sent to the player who completed the event!");
			}
			EventModel eventForPlayer = RunManager.Instance.EventSynchronizer.GetEventForPlayer(player);
			CrystalSphere crystalSphere = eventForPlayer as CrystalSphere;
			if (crystalSphere == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Received ");
				defaultInterpolatedStringHandler.AppendFormatted("CrystalSphereRewardsMessage");
				defaultInterpolatedStringHandler.AppendLiteral(" for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" while the player was in event ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(eventForPlayer.Id);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<CrystalSphereItem> list = message.rewards.Select<SerializableCrystalSphereItem, CrystalSphereItem>((SerializableCrystalSphereItem r) => CrystalSphereItem.FromSerializable(r, player)).ToList<CrystalSphereItem>();
			TaskHelper.RunSafely(this.OfferCrystalSphereRewards(player, list, crystalSphere.Rng));
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0021CB90 File Offset: 0x0021AD90
		private async Task OfferCrystalSphereRewards(Player owner, List<CrystalSphereItem> revealed, Rng rng)
		{
			List<Reward> list = revealed.Select<CrystalSphereItem, Reward>((CrystalSphereItem r) => r.ToReward(owner, rng)).OfType<Reward>().ToList<Reward>();
			await RewardsCmd.OfferCustom(owner, list);
		}

		// Token: 0x04001C0D RID: 7181
		private readonly RunLocationTargetedMessageBuffer _messageBuffer;

		// Token: 0x04001C0E RID: 7182
		private readonly INetGameService _gameService;

		// Token: 0x04001C0F RID: 7183
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001C10 RID: 7184
		private readonly ulong _localPlayerId;
	}
}
