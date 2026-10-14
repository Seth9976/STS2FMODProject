using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000584 RID: 1412
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MawBank : RelicModel
	{
		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600525E RID: 21086 RVA: 0x00242B8B File Offset: 0x00240D8B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x00242B8E File Offset: 0x00240D8E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(12));
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x00242B9C File Offset: 0x00240D9C
		public override bool ShouldFlashOnPlayer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x00242B9F File Offset: 0x00240D9F
		public override bool IsUsedUp
		{
			get
			{
				return this.HasItemBeenBought;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x00242BA7 File Offset: 0x00240DA7
		// (set) Token: 0x06005263 RID: 21091 RVA: 0x00242BAF File Offset: 0x00240DAF
		[SavedProperty]
		public bool HasItemBeenBought
		{
			get
			{
				return this._hasItemBeenBought;
			}
			set
			{
				base.AssertMutable();
				this._hasItemBeenBought = value;
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x00242BD0 File Offset: 0x00240DD0
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (base.Owner.RunState.BaseRoom == room)
			{
				if (!this.HasItemBeenBought)
				{
					base.Flash();
					await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, base.Owner, false);
				}
			}
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x00242C1B File Offset: 0x00240E1B
		public override Task AfterItemPurchased(Player player, MerchantEntry itemPurchased, int goldSpent)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (this.HasItemBeenBought)
			{
				return Task.CompletedTask;
			}
			if (goldSpent <= 0)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			this.HasItemBeenBought = true;
			return Task.CompletedTask;
		}

		// Token: 0x04002473 RID: 9331
		private bool _hasItemBeenBought;
	}
}
