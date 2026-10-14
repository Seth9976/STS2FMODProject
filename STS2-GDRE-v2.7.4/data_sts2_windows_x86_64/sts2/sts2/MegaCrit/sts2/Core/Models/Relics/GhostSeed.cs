using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000555 RID: 1365
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GhostSeed : RelicModel
	{
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x00240753 File Offset: 0x0023E953
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x00240756 File Offset: 0x0023E956
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x00240763 File Offset: 0x0023E963
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (!GhostSeed.CanAffect(card))
			{
				return Task.CompletedTask;
			}
			if (card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			CardCmd.ApplyKeyword(card, new CardKeyword[] { CardKeyword.Ethereal });
			return Task.CompletedTask;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x0024079C File Offset: 0x0023E99C
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			IEnumerable<CardModel> allCards = base.Owner.PlayerCombatState.AllCards;
			foreach (CardModel cardModel in allCards)
			{
				if (GhostSeed.CanAffect(cardModel))
				{
					CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Ethereal });
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x0024081C File Offset: 0x0023EA1C
		private static bool CanAffect(CardModel card)
		{
			return card.Rarity == CardRarity.Basic && (card.Tags.Contains(CardTag.Strike) || card.Tags.Contains(CardTag.Defend)) && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Ethereal);
		}
	}
}
