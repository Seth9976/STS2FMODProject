using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200054E RID: 1358
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FresnelLens : RelicModel
	{
		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x060050EA RID: 20714 RVA: 0x0023FE53 File Offset: 0x0023E053
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x060050EB RID: 20715 RVA: 0x0023FE56 File Offset: 0x0023E056
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("NimbleAmount", 2m));
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x060050EC RID: 20716 RVA: 0x0023FE6D File Offset: 0x0023E06D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Nimble>(base.DynamicVars["NimbleAmount"].IntValue);
			}
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x0023FE89 File Offset: 0x0023E089
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			this.EnchantValidCards(cardRewards);
			return true;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x0023FE9E File Offset: 0x0023E09E
		public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
		{
			if (player != base.Owner)
			{
				return;
			}
			this.EnchantValidCards(cards);
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x0023FEB1 File Offset: 0x0023E0B1
		public override bool TryModifyCardBeingAddedToDeck(CardModel card, [Nullable(2)] out CardModel newCard)
		{
			newCard = null;
			if (card.Owner != base.Owner)
			{
				return false;
			}
			if (!ModelDb.Enchantment<Nimble>().CanEnchant(card))
			{
				return false;
			}
			newCard = this.EnchantCard(card);
			return true;
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x0023FEE0 File Offset: 0x0023E0E0
		private void EnchantValidCards(List<CardCreationResult> options)
		{
			Nimble nimble = ModelDb.Enchantment<Nimble>();
			foreach (CardCreationResult cardCreationResult in options)
			{
				CardModel card = cardCreationResult.Card;
				if (nimble.CanEnchant(card))
				{
					cardCreationResult.ModifyCard(this.EnchantCard(card), this);
				}
			}
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x0023FF4C File Offset: 0x0023E14C
		private CardModel EnchantCard(CardModel card)
		{
			CardModel cardModel = base.Owner.RunState.CloneCard(card);
			CardCmd.Enchant<Nimble>(cardModel, base.DynamicVars["NimbleAmount"].BaseValue);
			return cardModel;
		}

		// Token: 0x0400244C RID: 9292
		private const string _nimbleAmountKey = "NimbleAmount";
	}
}
