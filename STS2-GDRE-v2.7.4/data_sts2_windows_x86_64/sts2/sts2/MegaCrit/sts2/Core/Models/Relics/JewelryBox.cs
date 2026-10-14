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
	// Token: 0x0200056B RID: 1387
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JewelryBox : RelicModel
	{
		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x060051A6 RID: 20902 RVA: 0x00241537 File Offset: 0x0023F737
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x060051A7 RID: 20903 RVA: 0x0024153A File Offset: 0x0023F73A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Apotheosis>(false);
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x060051A8 RID: 20904 RVA: 0x00241542 File Offset: 0x0023F742
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x00241548 File Offset: 0x0023F748
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<Apotheosis>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult2), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
