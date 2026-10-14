using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200056F RID: 1391
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Kifuda : RelicModel
	{
		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060051C8 RID: 20936 RVA: 0x002418F7 File Offset: 0x0023FAF7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060051C9 RID: 20937 RVA: 0x002418FA File Offset: 0x0023FAFA
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x002418FD File Offset: 0x0023FAFD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060051CB RID: 20939 RVA: 0x0024190A File Offset: 0x0023FB0A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Adroit>(3);
			}
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00241914 File Offset: 0x0023FB14
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, base.DynamicVars.Cards.IntValue)
			{
				Cancelable = false,
				RequireManualConfirmation = true
			};
			Adroit canonicalEnchantment = ModelDb.Enchantment<Adroit>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, canonicalEnchantment, 3, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant(canonicalEnchantment.ToMutable(), cardModel, 3m);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x04002465 RID: 9317
		private const int _adroitAmount = 3;
	}
}
