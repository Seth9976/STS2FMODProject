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
	// Token: 0x020005C5 RID: 1477
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PunchDagger : RelicModel
	{
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x0024611A File Offset: 0x0024431A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x0600542B RID: 21547 RVA: 0x0024611D File Offset: 0x0024431D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Momentum", 5m));
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x0600542C RID: 21548 RVA: 0x00246134 File Offset: 0x00244334
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x0600542D RID: 21549 RVA: 0x00246137 File Offset: 0x00244337
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Momentum>(base.DynamicVars["Momentum"].IntValue);
			}
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x00246154 File Offset: 0x00244354
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			Momentum canonicalMomentum = ModelDb.Enchantment<Momentum>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, canonicalMomentum, base.DynamicVars["Momentum"].IntValue, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant(canonicalMomentum.ToMutable(), cardModel, base.DynamicVars["Momentum"].IntValue);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x040024AC RID: 9388
		private const string _momentumKey = "Momentum";
	}
}
