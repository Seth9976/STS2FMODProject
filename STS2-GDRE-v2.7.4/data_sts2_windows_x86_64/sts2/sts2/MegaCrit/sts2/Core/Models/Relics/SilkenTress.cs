using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E2 RID: 1506
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilkenTress : RelicModel
	{
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x00247733 File Offset: 0x00245933
		public override bool IsUsedUp
		{
			get
			{
				return this.IsUsed;
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x060054DA RID: 21722 RVA: 0x0024773B File Offset: 0x0024593B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x060054DB RID: 21723 RVA: 0x0024773E File Offset: 0x0024593E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Glam>(1);
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x00247746 File Offset: 0x00245946
		// (set) Token: 0x060054DD RID: 21725 RVA: 0x0024774E File Offset: 0x0024594E
		[SavedProperty]
		private bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				base.AssertMutable();
				this._isUsed = value;
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x0024776C File Offset: 0x0024596C
		public override async Task AfterObtained()
		{
			await PlayerCmd.LoseGold(base.Owner.Gold, base.Owner, GoldLossType.Lost);
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x002477B0 File Offset: 0x002459B0
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
			{
				return false;
			}
			if (this.IsUsed)
			{
				return false;
			}
			Glam glam = ModelDb.Enchantment<Glam>();
			foreach (CardCreationResult cardCreationResult in cardRewards)
			{
				CardModel card = cardCreationResult.Card;
				if (glam.CanEnchant(card))
				{
					CardModel cardModel = base.Owner.RunState.CloneCard(card);
					CardCmd.Enchant<Glam>(cardModel, 1m);
					cardCreationResult.ModifyCard(cardModel, this);
				}
			}
			return true;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x0024786C File Offset: 0x00245A6C
		public override Task AfterModifyingCardRewardOptions()
		{
			if (this.IsUsed)
			{
				return Task.CompletedTask;
			}
			this.IsUsed = true;
			return Task.CompletedTask;
		}

		// Token: 0x040024BD RID: 9405
		private bool _isUsed;
	}
}
