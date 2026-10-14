using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	/// <summary>
	/// Represents purchasable entry in the merchant shop.
	/// Wrapped in an NMerchant_Node that manages the UI interactions.
	/// </summary>
	// Token: 0x02000C97 RID: 3223
	[NullableContext(2)]
	[Nullable(0)]
	public abstract class MerchantEntry
	{
		// Token: 0x170023F8 RID: 9208
		// (get) Token: 0x060083BA RID: 33722 RVA: 0x002C7C88 File Offset: 0x002C5E88
		public int Cost
		{
			get
			{
				decimal num = this._cost;
				if (this._player.RunState.CurrentRoom is MerchantRoom)
				{
					num = Hook.ModifyMerchantPrice(this._player.RunState, this._player, this, this._cost);
				}
				return (int)num;
			}
		}

		// Token: 0x170023F9 RID: 9209
		// (get) Token: 0x060083BB RID: 33723 RVA: 0x002C7CE1 File Offset: 0x002C5EE1
		public bool EnoughGold
		{
			get
			{
				return this.Cost <= this._player.Gold;
			}
		}

		// Token: 0x170023FA RID: 9210
		// (get) Token: 0x060083BC RID: 33724
		public abstract bool IsStocked { get; }

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x060083BD RID: 33725 RVA: 0x002C7CFC File Offset: 0x002C5EFC
		// (remove) Token: 0x060083BE RID: 33726 RVA: 0x002C7D34 File Offset: 0x002C5F34
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PurchaseStatus, MerchantEntry> PurchaseCompleted;

		// Token: 0x060083BF RID: 33727 RVA: 0x002C7D69 File Offset: 0x002C5F69
		[NullableContext(1)]
		public void InvokePurchaseCompleted(MerchantEntry entry)
		{
			Action<PurchaseStatus, MerchantEntry> purchaseCompleted = this.PurchaseCompleted;
			if (purchaseCompleted == null)
			{
				return;
			}
			purchaseCompleted(PurchaseStatus.Success, entry);
		}

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x060083C0 RID: 33728 RVA: 0x002C7D80 File Offset: 0x002C5F80
		// (remove) Token: 0x060083C1 RID: 33729 RVA: 0x002C7DB8 File Offset: 0x002C5FB8
		public event Action<PurchaseStatus> PurchaseFailed;

		// Token: 0x060083C2 RID: 33730 RVA: 0x002C7DED File Offset: 0x002C5FED
		public void InvokePurchaseFailed(PurchaseStatus status)
		{
			Action<PurchaseStatus> purchaseFailed = this.PurchaseFailed;
			if (purchaseFailed == null)
			{
				return;
			}
			purchaseFailed(status);
		}

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x060083C3 RID: 33731 RVA: 0x002C7E00 File Offset: 0x002C6000
		// (remove) Token: 0x060083C4 RID: 33732 RVA: 0x002C7E38 File Offset: 0x002C6038
		public event Action EntryUpdated;

		// Token: 0x060083C5 RID: 33733 RVA: 0x002C7E6D File Offset: 0x002C606D
		[NullableContext(1)]
		protected MerchantEntry(Player player)
		{
			this._player = player;
		}

		// Token: 0x060083C6 RID: 33734 RVA: 0x002C7E7C File Offset: 0x002C607C
		protected virtual void UpdateEntry()
		{
		}

		// Token: 0x060083C7 RID: 33735 RVA: 0x002C7E7E File Offset: 0x002C607E
		public void OnMerchantInventoryUpdated()
		{
			this.UpdateEntry();
			Action entryUpdated = this.EntryUpdated;
			if (entryUpdated == null)
			{
				return;
			}
			entryUpdated();
		}

		// Token: 0x060083C8 RID: 33736
		public abstract void CalcCost();

		// Token: 0x060083C9 RID: 33737 RVA: 0x002C7E98 File Offset: 0x002C6098
		[NullableContext(1)]
		public async Task<bool> OnTryPurchaseWrapper([Nullable(2)] MerchantInventory inventory, bool ignoreCost = false)
		{
			bool flag;
			if (!this.IsStocked)
			{
				this.InvokePurchaseFailed(PurchaseStatus.FailureOutOfStock);
				flag = false;
			}
			else if (!this.EnoughGold && !ignoreCost)
			{
				this.InvokePurchaseFailed(PurchaseStatus.FailureGold);
				flag = false;
			}
			else
			{
				ValueTuple<bool, int> valueTuple = await this.OnTryPurchase(inventory, ignoreCost);
				ValueTuple<bool, int> valueTuple2 = valueTuple;
				bool success = valueTuple2.Item1;
				int item = valueTuple2.Item2;
				if (success)
				{
					if (this._player.RunState.CurrentRoom is MerchantRoom && Hook.ShouldRefillMerchantEntry(this._player.RunState, this, this._player))
					{
						this.RestockAfterPurchase(inventory);
					}
					else
					{
						this.ClearAfterPurchase();
					}
					await Hook.AfterItemPurchased(this._player.RunState, this._player, this, item);
					this.InvokePurchaseCompleted(this);
				}
				flag = success;
			}
			return flag;
		}

		/// <summary>
		/// Subclass-specific logic for purchasing the item in the entry.
		/// </summary>
		/// <param name="inventory">Inventory that the entry is being purchased in.</param>
		/// <param name="ignoreCost">Whether or not the cost should be ignored</param>
		/// <returns>Tuple containing whether or not the purchase was successful and the amount of gold spent.</returns>
		// Token: 0x060083CA RID: 33738
		[return: Nullable(new byte[] { 1, 0 })]
		protected abstract Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost);

		/// <summary>
		/// Subclass-specific logic for clearing the entry after a purchase.
		/// </summary>
		// Token: 0x060083CB RID: 33739
		protected abstract void ClearAfterPurchase();

		/// <summary>
		/// Subclass-specific logic for restocking the entry after a purchase if the player has an effect like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.TheCourier" />.
		/// </summary>
		// Token: 0x060083CC RID: 33740
		protected abstract void RestockAfterPurchase(MerchantInventory inventory);

		// Token: 0x04002D00 RID: 11520
		[Nullable(1)]
		protected readonly Player _player;

		// Token: 0x04002D01 RID: 11521
		protected int _cost;
	}
}
