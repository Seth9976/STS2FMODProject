using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000558 RID: 1368
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glitter : RelicModel
	{
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06005138 RID: 20792 RVA: 0x00240977 File Offset: 0x0023EB77
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005139 RID: 20793 RVA: 0x0024097A File Offset: 0x0023EB7A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Glam>(1);
			}
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x00240984 File Offset: 0x0023EB84
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
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
	}
}
