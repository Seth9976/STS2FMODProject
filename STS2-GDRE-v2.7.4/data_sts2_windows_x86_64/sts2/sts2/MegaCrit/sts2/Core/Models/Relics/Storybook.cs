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
	// Token: 0x020005EE RID: 1518
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Storybook : RelicModel
	{
		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06005530 RID: 21808 RVA: 0x00248270 File Offset: 0x00246470
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06005531 RID: 21809 RVA: 0x00248273 File Offset: 0x00246473
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<BrightestFlame>(false);
			}
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x0024827C File Offset: 0x0024647C
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<BrightestFlame>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult2), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
