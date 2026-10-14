using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F5 RID: 1269
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArcaneScroll : RelicModel
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004ECF RID: 20175 RVA: 0x0023C12F File Offset: 0x0023A32F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004ED0 RID: 20176 RVA: 0x0023C132 File Offset: 0x0023A332
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x0023C140 File Offset: 0x0023A340
		public override async Task AfterObtained()
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(base.Owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoUpgradeRoll);
			List<CardModel> list = (from r in CardFactory.CreateForReward(base.Owner, base.DynamicVars.Cards.IntValue, cardCreationOptions)
				select r.Card).ToList<CardModel>();
			if (list.Count > 0)
			{
				CardModel cardModel = list[0];
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}
	}
}
