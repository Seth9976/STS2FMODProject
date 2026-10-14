using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F5 RID: 1525
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TanxsWhistle : RelicModel
	{
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06005555 RID: 21845 RVA: 0x0024860B File Offset: 0x0024680B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06005556 RID: 21846 RVA: 0x0024860E File Offset: 0x0024680E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Whistle>(false);
			}
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x00248618 File Offset: 0x00246818
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<Whistle>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult2), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
