using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	/// <summary>
	/// Works with the PeerInputSynchronizer to synchronize the intent shown over the player's head during combat.
	/// Note that this doesn't do any message sending itself - PeerInputSynchronizer does the message sending and handling
	/// to reduce bandwidth. This is here only to separate out some of the state tracking from PeerInputSynchronizer.
	/// </summary>
	// Token: 0x020004AB RID: 1195
	[NullableContext(2)]
	[Nullable(0)]
	public class HoveredModelTracker
	{
		// Token: 0x14000074 RID: 116
		// (add) Token: 0x060047FD RID: 18429 RVA: 0x002202D8 File Offset: 0x0021E4D8
		// (remove) Token: 0x060047FE RID: 18430 RVA: 0x00220310 File Offset: 0x0021E510
		public event Action<ulong> HoverChanged;

		// Token: 0x060047FF RID: 18431 RVA: 0x00220348 File Offset: 0x0021E548
		[NullableContext(1)]
		public HoveredModelTracker(PeerInputSynchronizer inputSynchronizer, IPlayerCollection playerCollection)
		{
			this._inputSynchronizer = inputSynchronizer;
			this._playerCollection = playerCollection;
			foreach (Player player in playerCollection.Players)
			{
				this._hoveredModels.Add(null);
			}
			this._inputSynchronizer.StateChanged += this.OnPlayerStateChanged;
			this._inputSynchronizer.StateRemoved += this.OnPlayerStateRemoved;
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x002203E8 File Offset: 0x0021E5E8
		private void OnPlayerStateChanged(ulong playerId)
		{
			Player player = this._playerCollection.GetPlayer(playerId);
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			if (playerSlotIndex >= this._hoveredModels.Count)
			{
				return;
			}
			HoveredModelData hoveredModelData = this._inputSynchronizer.GetHoveredModelData(playerId);
			AbstractModel abstractModel;
			switch (hoveredModelData.type)
			{
			case HoveredModelType.None:
				abstractModel = null;
				break;
			case HoveredModelType.Card:
				abstractModel = ((hoveredModelData.hoveredCombatCard != null) ? hoveredModelData.hoveredCombatCard.GetValueOrDefault().ToCardModelOrNull() : null);
				break;
			case HoveredModelType.Relic:
			{
				int? num = hoveredModelData.hoveredRelicIndex;
				int num2 = player.Relics.Count;
				abstractModel = (((num.GetValueOrDefault() < num2) & (num != null)) ? player.Relics[hoveredModelData.hoveredRelicIndex.Value] : null);
				break;
			}
			case HoveredModelType.Potion:
			{
				int? num = hoveredModelData.hoveredPotionIndex;
				int num2 = player.PotionSlots.Count;
				AbstractModel abstractModel2;
				if ((num.GetValueOrDefault() < num2) & (num != null))
				{
					num = hoveredModelData.hoveredPotionIndex;
					num2 = 0;
					if ((num.GetValueOrDefault() >= num2) & (num != null))
					{
						abstractModel2 = player.GetPotionAtSlotIndex(hoveredModelData.hoveredPotionIndex.Value);
						goto IL_0134;
					}
				}
				abstractModel2 = null;
				IL_0134:
				abstractModel = abstractModel2;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported hover type ");
				defaultInterpolatedStringHandler.AppendFormatted<HoveredModelType>(hoveredModelData.type);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (abstractModel == null && hoveredModelData.type != HoveredModelType.None)
			{
				abstractModel = ModelDb.GetById<AbstractModel>(hoveredModelData.hoveredModelId);
			}
			AbstractModel abstractModel3 = this._hoveredModels[playerSlotIndex];
			if (abstractModel3 != abstractModel)
			{
				this._hoveredModels[playerSlotIndex] = abstractModel;
				Action<ulong> hoverChanged = this.HoverChanged;
				if (hoverChanged == null)
				{
					return;
				}
				hoverChanged(playerId);
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x002205A4 File Offset: 0x0021E7A4
		private void OnPlayerStateRemoved(ulong playerId)
		{
			Player player = this._playerCollection.GetPlayer(playerId);
			int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
			if (this._hoveredModels[playerSlotIndex] != null)
			{
				this._hoveredModels[playerSlotIndex] = null;
				Action<ulong> hoverChanged = this.HoverChanged;
				if (hoverChanged == null)
				{
					return;
				}
				hoverChanged(playerId);
			}
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x002205F7 File Offset: 0x0021E7F7
		public AbstractModel GetHoveredModel(ulong playerId)
		{
			return this._hoveredModels[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId))];
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0022061B File Offset: 0x0021E81B
		[NullableContext(1)]
		public void OnLocalCardSelected(CardModel cardModel)
		{
			this._localSelectedCard = cardModel;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0022062A File Offset: 0x0021E82A
		public void OnLocalCardDeselected()
		{
			this._localSelectedCard = null;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x00220639 File Offset: 0x0021E839
		[NullableContext(1)]
		public void OnLocalPotionSelected(PotionModel potionModel)
		{
			this._localSelectedPotion = potionModel;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x00220648 File Offset: 0x0021E848
		public void OnLocalPotionDeselected()
		{
			this._localSelectedPotion = null;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x00220657 File Offset: 0x0021E857
		[NullableContext(1)]
		public void OnLocalCardHovered(CardModel cardModel)
		{
			this._localHoveredCard = cardModel;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00220666 File Offset: 0x0021E866
		public void OnLocalCardUnhovered()
		{
			this._localHoveredCard = null;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x00220675 File Offset: 0x0021E875
		[NullableContext(1)]
		public void OnLocalRelicHovered(RelicModel relicModel)
		{
			this._localHoveredRelic = relicModel;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x00220684 File Offset: 0x0021E884
		public void OnLocalRelicUnhovered()
		{
			this._localHoveredRelic = null;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x00220693 File Offset: 0x0021E893
		[NullableContext(1)]
		public void OnLocalPotionHovered(PotionModel potionModel)
		{
			this._localHoveredPotion = potionModel;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x002206A2 File Offset: 0x0021E8A2
		public void OnLocalPotionUnhovered()
		{
			this._localHoveredPotion = null;
			this.SynchronizeLocalHoveredModel();
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x002206B4 File Offset: 0x0021E8B4
		private void SynchronizeLocalHoveredModel()
		{
			CardModel cardModel;
			if ((cardModel = this._localSelectedCard) == null && (cardModel = this._localSelectedPotion) == null && (cardModel = this._localHoveredCard) == null)
			{
				cardModel = this._localHoveredPotion ?? this._localHoveredRelic;
			}
			AbstractModel abstractModel = cardModel;
			this._inputSynchronizer.SyncLocalHoveredModel(abstractModel);
		}

		// Token: 0x04001C44 RID: 7236
		[Nullable(1)]
		private readonly PeerInputSynchronizer _inputSynchronizer;

		// Token: 0x04001C45 RID: 7237
		private CardModel _localSelectedCard;

		// Token: 0x04001C46 RID: 7238
		private PotionModel _localSelectedPotion;

		// Token: 0x04001C47 RID: 7239
		private CardModel _localHoveredCard;

		// Token: 0x04001C48 RID: 7240
		private RelicModel _localHoveredRelic;

		// Token: 0x04001C49 RID: 7241
		private PotionModel _localHoveredPotion;

		// Token: 0x04001C4A RID: 7242
		[Nullable(new byte[] { 1, 2 })]
		private readonly List<AbstractModel> _hoveredModels = new List<AbstractModel>();

		// Token: 0x04001C4B RID: 7243
		[Nullable(1)]
		private readonly IPlayerCollection _playerCollection;
	}
}
