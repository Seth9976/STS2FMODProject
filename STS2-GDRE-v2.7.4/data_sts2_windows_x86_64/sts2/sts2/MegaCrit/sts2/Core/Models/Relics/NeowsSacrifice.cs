using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000594 RID: 1428
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeowsSacrifice : RelicModel
	{
		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x060052BE RID: 21182 RVA: 0x00243775 File Offset: 0x00241975
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060052BF RID: 21183 RVA: 0x00243778 File Offset: 0x00241978
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.FromPotion<Ambergris>());
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Guilty>(false));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x060052C0 RID: 21184 RVA: 0x0024379B File Offset: 0x0024199B
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x002437A0 File Offset: 0x002419A0
		public override async Task AfterObtained()
		{
			await PotionCmd.TryToProcure<Ambergris>(base.Owner);
			CardModel cardModel = base.Owner.RunState.CreateCard<Guilty>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(new <>z__ReadOnlySingleElementList<CardPileAddResult>(cardPileAddResult), 2f, CardPreviewStyle.HorizontalLayout);
		}
	}
}
