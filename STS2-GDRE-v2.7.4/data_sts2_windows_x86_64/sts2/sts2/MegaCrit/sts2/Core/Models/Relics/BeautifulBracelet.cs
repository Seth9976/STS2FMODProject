using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FC RID: 1276
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeautifulBracelet : RelicModel
	{
		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004F07 RID: 20231 RVA: 0x0023C8A3 File Offset: 0x0023AAA3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004F08 RID: 20232 RVA: 0x0023C8A6 File Offset: 0x0023AAA6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new DynamicVar("Swift", 3m)
				});
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004F09 RID: 20233 RVA: 0x0023C8CF File Offset: 0x0023AACF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Swift>(base.DynamicVars["Swift"].IntValue);
			}
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x0023C8EC File Offset: 0x0023AAEC
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Swift>(), base.DynamicVars["Swift"].IntValue, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant<Swift>(cardModel, 3m);
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

		// Token: 0x04002424 RID: 9252
		private const string _swiftKey = "Swift";
	}
}
