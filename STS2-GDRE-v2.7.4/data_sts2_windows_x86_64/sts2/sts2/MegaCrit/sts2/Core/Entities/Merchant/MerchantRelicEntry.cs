using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Entities.Merchant
{
	/// <summary>
	/// Manages a purchasable relic in the merchant shop
	/// </summary>
	// Token: 0x02000C9A RID: 3226
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MerchantRelicEntry : MerchantEntry
	{
		// Token: 0x17002405 RID: 9221
		// (get) Token: 0x060083EA RID: 33770 RVA: 0x002C84D0 File Offset: 0x002C66D0
		// (set) Token: 0x060083EB RID: 33771 RVA: 0x002C84D8 File Offset: 0x002C66D8
		public RelicModel Model { get; private set; }

		// Token: 0x17002406 RID: 9222
		// (get) Token: 0x060083EC RID: 33772 RVA: 0x002C84E1 File Offset: 0x002C66E1
		public override bool IsStocked
		{
			get
			{
				return this.Model != null;
			}
		}

		// Token: 0x060083ED RID: 33773 RVA: 0x002C84EC File Offset: 0x002C66EC
		[NullableContext(1)]
		public MerchantRelicEntry(RelicRarity rarity, Player player)
			: base(player)
		{
			this.FillSlot(rarity, null);
		}

		// Token: 0x060083EE RID: 33774 RVA: 0x002C84FD File Offset: 0x002C66FD
		[NullableContext(1)]
		public MerchantRelicEntry(RelicModel relic, Player player)
			: base(player)
		{
			this.SetModel(relic);
		}

		// Token: 0x060083EF RID: 33775 RVA: 0x002C8510 File Offset: 0x002C6710
		private void FillSlot(RelicRarity rarity, [Nullable(new byte[] { 2, 1 })] IEnumerable<RelicModel> blacklist = null)
		{
			this.SetModel(RelicFactory.PullNextRelicFromBack(this._player, rarity, (RelicModel r) => (blacklist == null || !blacklist.Contains(r)) && r.IsAllowedInShops).ToMutable());
		}

		// Token: 0x060083F0 RID: 33776 RVA: 0x002C854D File Offset: 0x002C674D
		public override void CalcCost()
		{
			this._cost = (int)Math.Round((double)((float)this.Model.MerchantCost * this._player.PlayerRng.Shops.NextFloat(0.85f, 1.15f)));
		}

		// Token: 0x060083F1 RID: 33777 RVA: 0x002C8588 File Offset: 0x002C6788
		[NullableContext(1)]
		private void SetModel(RelicModel model)
		{
			model.AssertMutable();
			this.Model = model;
			this.CalcCost();
			SaveManager.Instance.MarkRelicAsSeen(this.Model);
		}

		// Token: 0x060083F2 RID: 33778 RVA: 0x002C85B0 File Offset: 0x002C67B0
		[return: Nullable(new byte[] { 1, 0 })]
		protected override async Task<ValueTuple<bool, int>> OnTryPurchase(MerchantInventory inventory, bool ignoreCost)
		{
			if (!ignoreCost)
			{
				await PlayerCmd.LoseGold(base.Cost, this._player, GoldLossType.Spent);
			}
			MapPointHistoryEntry currentMapPointHistoryEntry = this._player.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(this._player.NetId).BoughtRelics.Add(this.Model.Id);
			}
			await RelicCmd.Obtain(this.Model, this._player, -1);
			RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(base.Cost);
			RunManager.Instance.RewardSynchronizer.SyncLocalObtainedRelic(this.Model);
			return new ValueTuple<bool, int>(true, ignoreCost ? 0 : base.Cost);
		}

		// Token: 0x060083F3 RID: 33779 RVA: 0x002C85FB File Offset: 0x002C67FB
		protected override void ClearAfterPurchase()
		{
			this.Model = null;
		}

		// Token: 0x060083F4 RID: 33780 RVA: 0x002C8604 File Offset: 0x002C6804
		protected override void RestockAfterPurchase(MerchantInventory inventory)
		{
			HashSet<RelicModel> hashSet;
			if (inventory == null)
			{
				hashSet = null;
			}
			else
			{
				hashSet = inventory.RelicEntries.Select<MerchantRelicEntry, RelicModel>(delegate(MerchantRelicEntry e)
				{
					RelicModel model = e.Model;
					if (model == null)
					{
						return null;
					}
					return model.CanonicalInstance;
				}).OfType<RelicModel>().ToHashSet<RelicModel>();
			}
			HashSet<RelicModel> hashSet2 = hashSet ?? new HashSet<RelicModel>();
			this.FillSlot(RelicFactory.RollRarity(this._player), hashSet2);
		}
	}
}
