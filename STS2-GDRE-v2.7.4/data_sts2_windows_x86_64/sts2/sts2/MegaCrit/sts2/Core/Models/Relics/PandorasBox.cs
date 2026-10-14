using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005AC RID: 1452
	public sealed class PandorasBox : RelicModel
	{
		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06005382 RID: 21378 RVA: 0x00244E83 File Offset: 0x00243083
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06005383 RID: 21379 RVA: 0x00244E86 File Offset: 0x00243086
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x00244E8C File Offset: 0x0024308C
		[NullableContext(1)]
		public override async Task AfterObtained()
		{
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsBasicStrikeOrDefend && c.IsRemovable).ToList<CardModel>();
			IEnumerable<CardTransformation> enumerable = list.Select<CardModel, CardTransformation>((CardModel c) => new CardTransformation(c, CardFactory.CreateRandomCardForTransform(c, false, base.Owner.RunState.Rng.Niche)));
			IEnumerable<CardPileAddResult> enumerable2 = await CardCmd.Transform(enumerable, null, CardPreviewStyle.None);
			List<CardPileAddResult> list2 = enumerable2.ToList<CardPileAddResult>();
			if (list2.Count > 0 && LocalContext.IsMe(base.Owner))
			{
				NSimpleCardsViewScreen.ShowScreen(list2, new LocString("relics", "PANDORAS_BOX.infoText"));
			}
		}
	}
}
