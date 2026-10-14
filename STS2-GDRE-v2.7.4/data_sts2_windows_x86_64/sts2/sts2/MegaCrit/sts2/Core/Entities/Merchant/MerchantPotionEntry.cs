using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	/// <summary>
	/// Manages a purchasable potion in the merchant shop.
	///
	/// </summary>
	// Token: 0x02000C99 RID: 3225
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MerchantPotionEntry : MerchantEntry
	{
		// Token: 0x17002403 RID: 9219
		// (get) Token: 0x060083DF RID: 33759 RVA: 0x002C82EF File Offset: 0x002C64EF
		// (set) Token: 0x060083E0 RID: 33760 RVA: 0x002C82F7 File Offset: 0x002C64F7
		public PotionModel Model { get; private set; }

		// Token: 0x17002404 RID: 9220
		// (get) Token: 0x060083E1 RID: 33761 RVA: 0x002C8300 File Offset: 0x002C6500
		public override bool IsStocked
		{
			get
			{
				return this.Model != null;
			}
		}

		// Token: 0x060083E2 RID: 33762 RVA: 0x002C830B File Offset: 0x002C650B
		[NullableContext(1)]
		public MerchantPotionEntry(PotionModel potion, Player player)
			: base(player)
		{
			potion.AssertMutable();
			this.Model = potion;
			this.CalcCost();
			SaveManager.Instance.MarkPotionAsSeen(this.Model);
		}

		// Token: 0x060083E3 RID: 33763 RVA: 0x002C8337 File Offset: 0x002C6537
		[NullableContext(1)]
		public MerchantPotionEntry(Player player)
			: base(player)
		{
			this.FillSlot(Array.Empty<PotionModel>());
		}

		// Token: 0x060083E4 RID: 33764 RVA: 0x002C834B File Offset: 0x002C654B
		[NullableContext(1)]
		private void FillSlot(IEnumerable<PotionModel> blacklist)
		{
			this.Model = PotionFactory.CreateRandomPotionOutOfCombat(this._player, this._player.PlayerRng.Shops, blacklist).ToMutable();
			this.CalcCost();
			SaveManager.Instance.MarkPotionAsSeen(this.Model);
		}

		// Token: 0x060083E5 RID: 33765 RVA: 0x002C838C File Offset: 0x002C658C
		private static int GetCost(PotionRarity rarity)
		{
			int num;
			if (rarity != PotionRarity.Uncommon)
			{
				if (rarity == PotionRarity.Rare)
				{
					num = 100;
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
			return num;
		}

		// Token: 0x060083E6 RID: 33766 RVA: 0x002C83B0 File Offset: 0x002C65B0
		public override void CalcCost()
		{
			if (this.Model == null)
			{
				throw new InvalidOperationException("There is no item to purchase.");
			}
			this._cost = MerchantPotionEntry.GetCost(this.Model.Rarity);
			if (TestMode.IsOff)
			{
				this._cost = (int)Mathf.Round((float)this._cost * this._player.PlayerRng.Shops.NextFloat(0.95f, 1.05f));
			}
		}

		// Token: 0x060083E7 RID: 33767 RVA: 0x002C8420 File Offset: 0x002C6620
		[return: Nullable(new byte[] { 1, 0 })]
		protected override async Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost)
		{
			if (this.Model == null)
			{
				throw new InvalidOperationException("There is no item to purchase.");
			}
			PotionProcureResult potionProcureResult = await PotionCmd.TryToProcure(this.Model, this._player, -1);
			PotionProcureResult potionProcureResult2 = potionProcureResult;
			ValueTuple<bool, int> valueTuple;
			if (!potionProcureResult2.success)
			{
				base.InvokePurchaseFailed((potionProcureResult2.failureReason == PotionProcureFailureReason.NotAllowed) ? PurchaseStatus.FailureForbidden : PurchaseStatus.FailureSpace);
				valueTuple = new ValueTuple<bool, int>(false, 0);
			}
			else
			{
				if (!ignoreCost)
				{
					await PlayerCmd.LoseGold(base.Cost, this._player, GoldLossType.Spent);
				}
				MapPointHistoryEntry currentMapPointHistoryEntry = this._player.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(this._player.NetId).BoughtPotions.Add(this.Model.Id);
				}
				RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(base.Cost);
				RunManager.Instance.RewardSynchronizer.SyncLocalObtainedPotion(this.Model);
				valueTuple = new ValueTuple<bool, int>(true, ignoreCost ? 0 : base.Cost);
			}
			return valueTuple;
		}

		// Token: 0x060083E8 RID: 33768 RVA: 0x002C846B File Offset: 0x002C666B
		protected override void ClearAfterPurchase()
		{
			this.Model = null;
		}

		// Token: 0x060083E9 RID: 33769 RVA: 0x002C8474 File Offset: 0x002C6674
		protected override void RestockAfterPurchase(MerchantInventory inventory)
		{
			HashSet<PotionModel> hashSet;
			if (inventory == null)
			{
				hashSet = null;
			}
			else
			{
				hashSet = inventory.PotionEntries.Select<MerchantPotionEntry, PotionModel>(delegate(MerchantPotionEntry e)
				{
					PotionModel model = e.Model;
					if (model == null)
					{
						return null;
					}
					return model.CanonicalInstance;
				}).OfType<PotionModel>().ToHashSet<PotionModel>();
			}
			HashSet<PotionModel> hashSet2 = hashSet ?? new HashSet<PotionModel>();
			this.FillSlot(Array.Empty<PotionModel>());
		}
	}
}
