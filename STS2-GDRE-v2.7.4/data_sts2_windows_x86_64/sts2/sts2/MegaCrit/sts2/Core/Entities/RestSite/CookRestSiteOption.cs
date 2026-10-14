using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C66 RID: 3174
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CookRestSiteOption : RestSiteOption
	{
		// Token: 0x17002394 RID: 9108
		// (get) Token: 0x0600824C RID: 33356 RVA: 0x002C0F8F File Offset: 0x002BF18F
		public override string OptionId
		{
			get
			{
				return "COOK";
			}
		}

		// Token: 0x17002395 RID: 9109
		// (get) Token: 0x0600824D RID: 33357 RVA: 0x002C0F98 File Offset: 0x002BF198
		public override LocString Description
		{
			get
			{
				if (this.IsEnabled)
				{
					LocString locString = new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".description");
					locString.Add("Cards", 2m);
					locString.Add("MaxHp", 5m);
					return locString;
				}
				return new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".descriptionDisabled");
			}
		}

		// Token: 0x17002396 RID: 9110
		// (get) Token: 0x0600824E RID: 33358 RVA: 0x002C1010 File Offset: 0x002BF210
		public override bool IsEnabled
		{
			get
			{
				return CookRestSiteOption.GetRemovableCardCount(base.Owner) >= 2;
			}
		}

		// Token: 0x0600824F RID: 33359 RVA: 0x002C1023 File Offset: 0x002BF223
		public CookRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x06008250 RID: 33360 RVA: 0x002C102C File Offset: 0x002BF22C
		public override async Task<bool> OnSelect()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2)
			{
				Cancelable = true,
				RequireManualConfirmation = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, null);
			IEnumerable<CardModel> enumerable2 = enumerable;
			bool flag;
			if (!enumerable2.Any<CardModel>())
			{
				flag = false;
			}
			else
			{
				foreach (CardModel cardModel in enumerable2)
				{
					await CardPileCmd.RemoveFromDeck(cardModel, true);
				}
				IEnumerator<CardModel> enumerator = null;
				await CreatureCmd.GainMaxHp(base.Owner.Creature, 5m);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06008251 RID: 33361 RVA: 0x002C106F File Offset: 0x002BF26F
		private static int GetRemovableCardCount(Player player)
		{
			return PileType.Deck.GetPile(player).Cards.Count<CardModel>((CardModel c) => c.IsRemovable);
		}

		// Token: 0x04002BE3 RID: 11235
		private const int _cardsToRemove = 2;

		// Token: 0x04002BE4 RID: 11236
		private const int _maxHpGain = 5;
	}
}
