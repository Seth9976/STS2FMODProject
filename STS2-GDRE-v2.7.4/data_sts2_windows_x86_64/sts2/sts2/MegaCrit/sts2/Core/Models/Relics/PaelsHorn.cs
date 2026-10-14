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
	// Token: 0x020005A7 RID: 1447
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsHorn : RelicModel
	{
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06005344 RID: 21316 RVA: 0x002446F9 File Offset: 0x002428F9
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06005345 RID: 21317 RVA: 0x002446FC File Offset: 0x002428FC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Relax>(false);
			}
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x00244704 File Offset: 0x00242904
		public override async Task AfterObtained()
		{
			List<CardPileAddResult> results = new List<CardPileAddResult>();
			for (int i = 0; i < 2; i++)
			{
				CardModel cardModel = base.Owner.RunState.CreateCard<Relax>(base.Owner);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				results.Add(cardPileAddResult2);
			}
			CardCmd.PreviewCardPileAdd(results, 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
