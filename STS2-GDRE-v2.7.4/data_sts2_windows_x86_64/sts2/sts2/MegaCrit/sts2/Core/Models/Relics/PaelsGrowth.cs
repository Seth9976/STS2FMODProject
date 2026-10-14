using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A6 RID: 1446
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsGrowth : RelicModel
	{
		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600533E RID: 21310 RVA: 0x00244666 File Offset: 0x00242866
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x0600533F RID: 21311 RVA: 0x00244669 File Offset: 0x00242869
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("EnchantmentName", ModelDb.Enchantment<Clone>().Title.GetFormattedText()));
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06005340 RID: 21312 RVA: 0x00244689 File Offset: 0x00242889
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Clone>(1);
			}
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x00244694 File Offset: 0x00242894
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, ModelDb.Enchantment<Clone>(), 1, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant<Clone>(cardModel, 4m);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x002446D7 File Offset: 0x002428D7
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			options.Add(new CloneRestSiteOption(player));
			return true;
		}
	}
}
