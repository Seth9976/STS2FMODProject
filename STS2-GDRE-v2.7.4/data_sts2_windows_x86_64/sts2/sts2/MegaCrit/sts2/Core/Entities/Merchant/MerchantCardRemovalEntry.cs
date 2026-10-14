using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	/// <summary>
	/// Manages the card removal option of a merchant shop
	/// </summary>
	// Token: 0x02000C95 RID: 3221
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MerchantCardRemovalEntry : MerchantEntry
	{
		// Token: 0x170023F0 RID: 9200
		// (get) Token: 0x060083A6 RID: 33702 RVA: 0x002C784F File Offset: 0x002C5A4F
		// (set) Token: 0x060083A7 RID: 33703 RVA: 0x002C7857 File Offset: 0x002C5A57
		public bool Used { get; private set; }

		// Token: 0x170023F1 RID: 9201
		// (get) Token: 0x060083A8 RID: 33704 RVA: 0x002C7860 File Offset: 0x002C5A60
		public override bool IsStocked
		{
			get
			{
				return !this.Used;
			}
		}

		// Token: 0x170023F2 RID: 9202
		// (get) Token: 0x060083A9 RID: 33705 RVA: 0x002C786B File Offset: 0x002C5A6B
		private static int BaseCost
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Inflation, 100, 75);
			}
		}

		// Token: 0x170023F3 RID: 9203
		// (get) Token: 0x060083AA RID: 33706 RVA: 0x002C7877 File Offset: 0x002C5A77
		public static int PriceIncrease
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.Inflation, 50, 25);
			}
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x002C7883 File Offset: 0x002C5A83
		[NullableContext(1)]
		public MerchantCardRemovalEntry(Player player)
			: base(player)
		{
			this.CalcCost();
		}

		// Token: 0x060083AC RID: 33708 RVA: 0x002C7892 File Offset: 0x002C5A92
		public override void CalcCost()
		{
			this._cost = MerchantCardRemovalEntry.BaseCost + MerchantCardRemovalEntry.PriceIncrease * this._player.ExtraFields.CardShopRemovalsUsed;
		}

		// Token: 0x060083AD RID: 33709 RVA: 0x002C78B8 File Offset: 0x002C5AB8
		[NullableContext(1)]
		public async Task<bool> OnTryPurchaseWrapper([Nullable(2)] MerchantInventory inventory, bool ignoreCost = false, bool cancelable = true)
		{
			bool flag;
			if (!RunManager.Instance.IsInProgress)
			{
				flag = false;
			}
			else if (!base.EnoughGold && !ignoreCost)
			{
				base.InvokePurchaseFailed(PurchaseStatus.FailureGold);
				flag = false;
			}
			else
			{
				ValueTuple<bool, int> valueTuple = await this.OnTryPurchase(inventory, ignoreCost, cancelable);
				ValueTuple<bool, int> valueTuple2 = valueTuple;
				bool success = valueTuple2.Item1;
				int item = valueTuple2.Item2;
				if (success)
				{
					await Hook.AfterItemPurchased(this._player.RunState, this._player, this, item);
					base.InvokePurchaseCompleted(this);
				}
				flag = success;
			}
			return flag;
		}

		// Token: 0x060083AE RID: 33710 RVA: 0x002C7914 File Offset: 0x002C5B14
		[return: Nullable(new byte[] { 1, 0 })]
		protected override async Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost)
		{
			return await this.OnTryPurchase(inventory, ignoreCost, true);
		}

		// Token: 0x060083AF RID: 33711 RVA: 0x002C7968 File Offset: 0x002C5B68
		[return: Nullable(new byte[] { 1, 0 })]
		private async Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost, bool cancelable)
		{
			ValueTuple<bool, int> valueTuple;
			if (this.Used)
			{
				valueTuple = new ValueTuple<bool, int>(false, 0);
			}
			else
			{
				int goldToSpend = (ignoreCost ? 0 : base.Cost);
				bool flag = await RunManager.Instance.OneOffSynchronizer.DoLocalMerchantCardRemoval(goldToSpend, cancelable);
				bool flag2 = flag;
				if (flag2)
				{
					NRun instance = NRun.Instance;
					if (instance != null)
					{
						NMerchantRoom merchantRoom = instance.MerchantRoom;
						if (merchantRoom != null)
						{
							merchantRoom.Inventory.OnCardRemovalUsed();
						}
					}
				}
				valueTuple = new ValueTuple<bool, int>(flag2, goldToSpend);
			}
			return valueTuple;
		}

		// Token: 0x060083B0 RID: 33712 RVA: 0x002C79BB File Offset: 0x002C5BBB
		protected override void ClearAfterPurchase()
		{
		}

		// Token: 0x060083B1 RID: 33713 RVA: 0x002C79BD File Offset: 0x002C5BBD
		protected override void RestockAfterPurchase(MerchantInventory inventory)
		{
		}

		// Token: 0x060083B2 RID: 33714 RVA: 0x002C79BF File Offset: 0x002C5BBF
		public void SetUsed()
		{
			this.Used = true;
		}
	}
}
