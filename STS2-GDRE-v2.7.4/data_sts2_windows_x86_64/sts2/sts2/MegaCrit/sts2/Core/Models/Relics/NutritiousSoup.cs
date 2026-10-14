using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200059B RID: 1435
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NutritiousSoup : RelicModel
	{
		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x00243C17 File Offset: 0x00241E17
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x00243C1A File Offset: 0x00241E1A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<TezcatarasEmber>(1);
			}
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x00243C24 File Offset: 0x00241E24
		public override Task AfterObtained()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.ToList<CardModel>();
			foreach (CardModel cardModel in enumerable)
			{
				if (cardModel.Rarity == CardRarity.Basic && cardModel.Tags.Contains(CardTag.Strike) && ModelDb.Enchantment<TezcatarasEmber>().CanEnchant(cardModel))
				{
					CardCmd.Enchant<TezcatarasEmber>(cardModel, 1m);
					NCardEnchantVfx ncardEnchantVfx = NCardEnchantVfx.Create(cardModel);
					if (ncardEnchantVfx != null)
					{
						NRun instance = NRun.Instance;
						if (instance != null)
						{
							instance.GlobalUi.CardPreviewContainer.AddChildSafely(ncardEnchantVfx);
						}
					}
				}
			}
			return Task.CompletedTask;
		}
	}
}
