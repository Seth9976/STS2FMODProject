using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DB RID: 2267
	public sealed class PerfectFit : EnchantmentModel
	{
		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x06006C7C RID: 27772 RVA: 0x00281EF2 File Offset: 0x002800F2
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x00281EF5 File Offset: 0x002800F5
		[NullableContext(1)]
		public override void ModifyShuffleOrder(Player player, List<CardModel> cards, bool isInitialShuffle)
		{
			if (isInitialShuffle)
			{
				return;
			}
			if (!cards.Contains(base.Card))
			{
				return;
			}
			cards.Remove(base.Card);
			cards.Insert(0, base.Card);
		}
	}
}
