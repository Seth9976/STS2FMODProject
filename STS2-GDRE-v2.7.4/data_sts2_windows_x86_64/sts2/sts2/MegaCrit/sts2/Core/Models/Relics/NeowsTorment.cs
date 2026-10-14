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
	// Token: 0x02000596 RID: 1430
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeowsTorment : RelicModel
	{
		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060052C7 RID: 21191 RVA: 0x002438E4 File Offset: 0x00241AE4
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x060052C8 RID: 21192 RVA: 0x002438E7 File Offset: 0x00241AE7
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<NeowsFury>(false);
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x060052C9 RID: 21193 RVA: 0x002438EF File Offset: 0x00241AEF
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x002438F4 File Offset: 0x00241AF4
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<NeowsFury>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult2), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
