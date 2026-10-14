using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000522 RID: 1314
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Claws : RelicModel
	{
		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06004FF7 RID: 20471 RVA: 0x0023E53D File Offset: 0x0023C73D
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x0023E540 File Offset: 0x0023C740
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(6));
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06004FF9 RID: 20473 RVA: 0x0023E54D File Offset: 0x0023C74D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<Maul>(false);
			}
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x0023E558 File Offset: 0x0023C758
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0, base.DynamicVars.Cards.IntValue)
			{
				Cancelable = false,
				RequireManualConfirmation = true
			};
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, (CardModel c) => new CardTransformation(c, this.CreateMaulFromOriginal(c, true)));
			IEnumerable<CardModel> enumerable2 = enumerable;
			List<CardTransformation> list = enumerable2.Select<CardModel, CardTransformation>((CardModel original) => new CardTransformation(original, this.CreateMaulFromOriginal(original, false))).ToList<CardTransformation>();
			await CardCmd.Transform(list, base.Owner.PlayerRng.Transformations, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x0023E59C File Offset: 0x0023C79C
		private CardModel CreateMaulFromOriginal(CardModel original, bool forPreview)
		{
			CardModel cardModel = (forPreview ? ModelDb.Card<Maul>().ToMutable() : base.Owner.RunState.CreateCard<Maul>(base.Owner));
			if (original.IsUpgraded && cardModel.IsUpgradable)
			{
				if (forPreview)
				{
					cardModel.UpgradeInternal();
				}
				else
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			if (original.Enchantment != null)
			{
				EnchantmentModel enchantmentModel = (EnchantmentModel)original.Enchantment.MutableClone();
				if (enchantmentModel.CanEnchant(cardModel))
				{
					if (forPreview)
					{
						cardModel.EnchantInternal(enchantmentModel, enchantmentModel.Amount);
						enchantmentModel.ModifyCard();
					}
					else
					{
						CardCmd.Enchant(enchantmentModel, cardModel, enchantmentModel.Amount);
					}
				}
			}
			return cardModel;
		}
	}
}
