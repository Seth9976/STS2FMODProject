using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D3 RID: 1491
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoyalStamp : RelicModel
	{
		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x00246AF3 File Offset: 0x00244CF3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x00246AF6 File Offset: 0x00244CF6
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06005481 RID: 21633 RVA: 0x00246AF9 File Offset: 0x00244CF9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(1),
					new StringVar("Enchantment", ModelDb.Enchantment<RoyallyApproved>().Title.GetFormattedText())
				});
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06005482 RID: 21634 RVA: 0x00246B2B File Offset: 0x00244D2B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<RoyallyApproved>(1);
			}
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x00246B34 File Offset: 0x00244D34
		public override async Task AfterObtained()
		{
			EnchantmentModel royalStamp = ModelDb.Enchantment<RoyallyApproved>();
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => royalStamp.CanEnchant(c)).ToList<CardModel>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(list.UnstableShuffle<CardModel>(base.Owner.RunState.Rng.Niche).ToList<CardModel>(), royalStamp, 1, cardSelectorPrefs);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Enchant<RoyallyApproved>(cardModel, 1m);
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
	}
}
