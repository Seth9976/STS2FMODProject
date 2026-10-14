using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000595 RID: 1429
	public sealed class NeowsTalisman : RelicModel
	{
		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060052C3 RID: 21187 RVA: 0x002437EB File Offset: 0x002419EB
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060052C4 RID: 21188 RVA: 0x002437EE File Offset: 0x002419EE
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x002437F4 File Offset: 0x002419F4
		[NullableContext(1)]
		public override Task AfterObtained()
		{
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Rarity == CardRarity.Basic).ToList<CardModel>();
			CardModel[] array = new CardModel[2];
			array[0] = list.LastOrDefault<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Strike));
			array[1] = list.LastOrDefault<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Defend));
			IEnumerable<CardModel> enumerable = new <>z__ReadOnlyArray<CardModel>(array);
			foreach (CardModel cardModel in enumerable)
			{
				if (cardModel != null)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			return Task.CompletedTask;
		}
	}
}
