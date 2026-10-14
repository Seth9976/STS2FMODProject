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
	// Token: 0x02000533 RID: 1331
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DowsingRod : RelicModel
	{
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x0023ED19 File Offset: 0x0023CF19
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005044 RID: 20548 RVA: 0x0023ED1C File Offset: 0x0023CF1C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Dowsing>(false);
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06005045 RID: 20549 RVA: 0x0023ED24 File Offset: 0x0023CF24
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x0023ED28 File Offset: 0x0023CF28
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<Dowsing>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult2), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
