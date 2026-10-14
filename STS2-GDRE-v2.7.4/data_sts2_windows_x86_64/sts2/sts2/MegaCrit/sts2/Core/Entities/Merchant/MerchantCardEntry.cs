using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	/// <summary>
	/// Manages a buy-able card in the merchant shop.
	/// </summary>
	// Token: 0x02000C94 RID: 3220
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MerchantCardEntry : MerchantEntry
	{
		// Token: 0x06008397 RID: 33687 RVA: 0x002C7548 File Offset: 0x002C5748
		[NullableContext(1)]
		private static int GetCost(CardModel card)
		{
			CardRarity rarity = card.Rarity;
			int num;
			if (rarity != CardRarity.Uncommon)
			{
				if (rarity == CardRarity.Rare)
				{
					num = 150;
				}
				else
				{
					num = 50;
				}
			}
			else
			{
				num = 75;
			}
			int num2 = num;
			if (card.Pool is ColorlessCardPool)
			{
				num2 = Mathf.RoundToInt((float)num2 * 1.15f);
			}
			return num2;
		}

		// Token: 0x170023ED RID: 9197
		// (get) Token: 0x06008398 RID: 33688 RVA: 0x002C7592 File Offset: 0x002C5792
		// (set) Token: 0x06008399 RID: 33689 RVA: 0x002C759A File Offset: 0x002C579A
		public CardCreationResult CreationResult { get; private set; }

		// Token: 0x170023EE RID: 9198
		// (get) Token: 0x0600839A RID: 33690 RVA: 0x002C75A3 File Offset: 0x002C57A3
		// (set) Token: 0x0600839B RID: 33691 RVA: 0x002C75AB File Offset: 0x002C57AB
		public bool IsOnSale { get; private set; }

		// Token: 0x0600839C RID: 33692 RVA: 0x002C75B4 File Offset: 0x002C57B4
		[NullableContext(1)]
		public MerchantCardEntry(Player player, [Nullable(2)] MerchantInventory inventory, IEnumerable<CardModel> cardPool, CardType cardType)
			: base(player)
		{
			this._inventory = inventory;
			this._cardPool = cardPool;
			this._cardType = new CardType?(cardType);
		}

		// Token: 0x0600839D RID: 33693 RVA: 0x002C75D8 File Offset: 0x002C57D8
		[NullableContext(1)]
		public MerchantCardEntry(Player player, [Nullable(2)] MerchantInventory inventory, IEnumerable<CardModel> cardPool, CardRarity cardRarity)
			: base(player)
		{
			this._inventory = inventory;
			this._cardPool = cardPool;
			this._cardRarity = new CardRarity?(cardRarity);
		}

		// Token: 0x0600839E RID: 33694 RVA: 0x002C75FC File Offset: 0x002C57FC
		public unsafe void Populate()
		{
			MerchantInventory inventory = this._inventory;
			HashSet<CardModel> hashSet;
			if (inventory == null)
			{
				hashSet = null;
			}
			else
			{
				hashSet = inventory.CardEntries.Select<MerchantCardEntry, CardModel>(delegate(MerchantCardEntry e)
				{
					CardCreationResult creationResult = e.CreationResult;
					if (creationResult == null)
					{
						return null;
					}
					return creationResult.Card.CanonicalInstance;
				}).OfType<CardModel>().ToHashSet<CardModel>();
			}
			HashSet<CardModel> hashSet2 = hashSet ?? new HashSet<CardModel>();
			if (this._cardType != null)
			{
				this.CreationResult = CardFactory.CreateForMerchant(this._player, this._cardPool.Except<CardModel>(hashSet2), this._cardType.Value);
			}
			else
			{
				if (this._cardRarity == null)
				{
					throw new InvalidOperationException();
				}
				this.CreationResult = CardFactory.CreateForMerchant(this._player, this._cardPool.Except<CardModel>(hashSet2), this._cardRarity.Value);
			}
			IRunState runState = this._player.RunState;
			Player player = this._player;
			int num = 1;
			List<CardCreationResult> list = new List<CardCreationResult>(num);
			CollectionsMarshal.SetCount<CardCreationResult>(list, num);
			Span<CardCreationResult> span = CollectionsMarshal.AsSpan<CardCreationResult>(list);
			int num2 = 0;
			*span[num2] = this.CreationResult;
			Hook.ModifyMerchantCardCreationResults(runState, player, list);
			this.CalcCost();
		}

		// Token: 0x0600839F RID: 33695 RVA: 0x002C770C File Offset: 0x002C590C
		protected unsafe override void UpdateEntry()
		{
			if (this.CreationResult != null)
			{
				IRunState runState = this._player.RunState;
				Player player = this._player;
				int num = 1;
				List<CardCreationResult> list = new List<CardCreationResult>(num);
				CollectionsMarshal.SetCount<CardCreationResult>(list, num);
				Span<CardCreationResult> span = CollectionsMarshal.AsSpan<CardCreationResult>(list);
				int num2 = 0;
				*span[num2] = this.CreationResult;
				Hook.ModifyMerchantCardCreationResults(runState, player, list);
			}
		}

		// Token: 0x060083A0 RID: 33696 RVA: 0x002C775E File Offset: 0x002C595E
		public void SetOnSale()
		{
			this.IsOnSale = true;
			this.CalcCost();
		}

		// Token: 0x170023EF RID: 9199
		// (get) Token: 0x060083A1 RID: 33697 RVA: 0x002C776D File Offset: 0x002C596D
		public override bool IsStocked
		{
			get
			{
				return this.CreationResult != null;
			}
		}

		// Token: 0x060083A2 RID: 33698 RVA: 0x002C7778 File Offset: 0x002C5978
		public override void CalcCost()
		{
			if (this.CreationResult == null)
			{
				throw new InvalidOperationException("There is no item to purchase.");
			}
			this._cost = Mathf.RoundToInt((float)MerchantCardEntry.GetCost(this.CreationResult.Card) * this._player.PlayerRng.Shops.NextFloat(0.95f, 1.05f));
			if (this.IsOnSale)
			{
				this._cost /= 2;
			}
		}

		// Token: 0x060083A3 RID: 33699 RVA: 0x002C77EC File Offset: 0x002C59EC
		[return: Nullable(new byte[] { 1, 0 })]
		protected override async Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost)
		{
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(this.CreationResult.Card, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			ValueTuple<bool, int> valueTuple;
			if (!cardPileAddResult2.success)
			{
				base.InvokePurchaseFailed(PurchaseStatus.FailureSpace);
				valueTuple = new ValueTuple<bool, int>(false, 0);
			}
			else
			{
				if (!ignoreCost)
				{
					await PlayerCmd.LoseGold(base.Cost, this._player, GoldLossType.Spent);
				}
				RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(base.Cost);
				RunManager.Instance.RewardSynchronizer.SyncLocalObtainedCard(this.CreationResult.Card);
				if (this.CreationResult.Card.Pool is ColorlessCardPool)
				{
					MapPointHistoryEntry currentMapPointHistoryEntry = this._player.RunState.CurrentMapPointHistoryEntry;
					if (currentMapPointHistoryEntry != null)
					{
						currentMapPointHistoryEntry.GetEntry(this._player.NetId).BoughtColorless.Add(this.CreationResult.Card.Id);
					}
				}
				valueTuple = new ValueTuple<bool, int>(true, ignoreCost ? 0 : base.Cost);
			}
			return valueTuple;
		}

		// Token: 0x060083A4 RID: 33700 RVA: 0x002C7837 File Offset: 0x002C5A37
		protected override void ClearAfterPurchase()
		{
			this.CreationResult = null;
		}

		// Token: 0x060083A5 RID: 33701 RVA: 0x002C7840 File Offset: 0x002C5A40
		protected override void RestockAfterPurchase(MerchantInventory inventory)
		{
			this.IsOnSale = false;
			this.Populate();
		}

		// Token: 0x04002CF1 RID: 11505
		private readonly MerchantInventory _inventory;

		// Token: 0x04002CF2 RID: 11506
		[Nullable(1)]
		private readonly IEnumerable<CardModel> _cardPool;

		// Token: 0x04002CF3 RID: 11507
		private readonly CardType? _cardType;

		// Token: 0x04002CF4 RID: 11508
		private readonly CardRarity? _cardRarity;
	}
}
