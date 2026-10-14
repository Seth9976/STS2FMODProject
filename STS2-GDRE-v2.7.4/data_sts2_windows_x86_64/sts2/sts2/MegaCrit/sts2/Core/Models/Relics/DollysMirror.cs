using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000532 RID: 1330
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DollysMirror : RelicModel
	{
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x0023ECB7 File Offset: 0x0023CEB7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x0600503F RID: 20543 RVA: 0x0023ECBA File Offset: 0x0023CEBA
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x0023ECC0 File Offset: 0x0023CEC0
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckGeneric(base.Owner, cardSelectorPrefs, new Func<CardModel, bool>(this.Filter), null);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(base.Owner.RunState.CloneCard(cardModel), PileType.Deck, CardPilePosition.Bottom, null, false), 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0023ED03 File Offset: 0x0023CF03
		private bool Filter(CardModel c)
		{
			return c.Type != CardType.Quest;
		}
	}
}
