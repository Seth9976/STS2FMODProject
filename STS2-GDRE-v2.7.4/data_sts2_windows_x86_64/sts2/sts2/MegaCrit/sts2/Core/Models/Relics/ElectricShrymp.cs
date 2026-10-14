using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000539 RID: 1337
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ElectricShrymp : RelicModel
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06005063 RID: 20579 RVA: 0x0023F0CE File Offset: 0x0023D2CE
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005064 RID: 20580 RVA: 0x0023F0D1 File Offset: 0x0023D2D1
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005065 RID: 20581 RVA: 0x0023F0D4 File Offset: 0x0023D2D4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromEnchantment<Imbued>(1);
			}
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x0023F0DC File Offset: 0x0023D2DC
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
			Imbued canonicalMomentum = ModelDb.Enchantment<Imbued>();
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForEnchantment(base.Owner, canonicalMomentum, 1, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Enchant(canonicalMomentum.ToMutable(), cardModel, 1m);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}
	}
}
