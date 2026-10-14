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
	// Token: 0x02000559 RID: 1369
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GnarledHammer : RelicModel
	{
		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x0600513C RID: 20796 RVA: 0x00240A20 File Offset: 0x0023EC20
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x00240A23 File Offset: 0x0023EC23
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(3),
					new DynamicVar("SharpAmount", 3m)
				});
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x00240A4C File Offset: 0x0023EC4C
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x00240A4F File Offset: 0x0023EC4F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Sharp>(base.DynamicVars["SharpAmount"].IntValue);
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x00240A6C File Offset: 0x0023EC6C
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, base.DynamicVars.Cards.IntValue)
			{
				Cancelable = false,
				RequireManualConfirmation = true
			};
			Sharp canonicalEnchantment = ModelDb.Enchantment<Sharp>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, canonicalEnchantment, base.DynamicVars["SharpAmount"].IntValue, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant(canonicalEnchantment.ToMutable(), cardModel, base.DynamicVars["SharpAmount"].IntValue);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x04002456 RID: 9302
		private const string _sharpAmountKey = "SharpAmount";
	}
}
