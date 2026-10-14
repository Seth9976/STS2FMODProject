using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CB6 RID: 3254
	public static class CardRarityExtensions
	{
		/// <summary>
		/// Gets the next highest rarity.
		/// Basic -&gt; Common -&gt; Uncommon -&gt; Rare -&gt; Common.
		/// Intended to be used in a loop with a blacklist. Be careful not to get into an infinite loop if the blacklist is
		/// full.
		/// </summary>
		// Token: 0x060084D0 RID: 34000 RVA: 0x002CAC94 File Offset: 0x002C8E94
		public static CardRarity GetNextHighestRarityWithWrapping(this CardRarity cardRarity)
		{
			CardRarity cardRarity2;
			switch (cardRarity)
			{
			case CardRarity.None:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Basic:
				cardRarity2 = CardRarity.Common;
				break;
			case CardRarity.Common:
				cardRarity2 = CardRarity.Uncommon;
				break;
			case CardRarity.Uncommon:
				cardRarity2 = CardRarity.Rare;
				break;
			case CardRarity.Rare:
				cardRarity2 = CardRarity.Common;
				break;
			case CardRarity.Ancient:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Event:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Token:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Status:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Curse:
				cardRarity2 = CardRarity.None;
				break;
			case CardRarity.Quest:
				cardRarity2 = CardRarity.None;
				break;
			default:
				throw new ArgumentOutOfRangeException("cardRarity", cardRarity, null);
			}
			return cardRarity2;
		}

		// Token: 0x060084D1 RID: 34001 RVA: 0x002CAD14 File Offset: 0x002C8F14
		[NullableContext(1)]
		public static LocString ToLocString(this CardRarity cardRarity)
		{
			LocString locString;
			switch (cardRarity)
			{
			case CardRarity.Basic:
				locString = new LocString("gameplay_ui", "CARD_RARITY.BASIC");
				break;
			case CardRarity.Common:
				locString = new LocString("gameplay_ui", "CARD_RARITY.COMMON");
				break;
			case CardRarity.Uncommon:
				locString = new LocString("gameplay_ui", "CARD_RARITY.UNCOMMON");
				break;
			case CardRarity.Rare:
				locString = new LocString("gameplay_ui", "CARD_RARITY.RARE");
				break;
			case CardRarity.Ancient:
				locString = new LocString("gameplay_ui", "CARD_RARITY.ANCIENT");
				break;
			case CardRarity.Event:
				locString = new LocString("gameplay_ui", "CARD_RARITY.EVENT");
				break;
			case CardRarity.Token:
				locString = new LocString("gameplay_ui", "CARD_RARITY.TOKEN");
				break;
			case CardRarity.Status:
				locString = new LocString("gameplay_ui", "CARD_RARITY.STATUS");
				break;
			case CardRarity.Curse:
				locString = new LocString("gameplay_ui", "CARD_RARITY.CURSE");
				break;
			case CardRarity.Quest:
				locString = new LocString("gameplay_ui", "CARD_RARITY.QUEST");
				break;
			default:
				throw new ArgumentOutOfRangeException("cardRarity", cardRarity, null);
			}
			return locString;
		}
	}
}
