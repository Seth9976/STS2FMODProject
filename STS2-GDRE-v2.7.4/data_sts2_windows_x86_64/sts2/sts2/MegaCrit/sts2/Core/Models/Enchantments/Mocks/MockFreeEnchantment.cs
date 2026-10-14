using System;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Enchantments.Mocks
{
	// Token: 0x020008E7 RID: 2279
	public sealed class MockFreeEnchantment : EnchantmentModel
	{
		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x06006CAC RID: 27820 RVA: 0x00282393 File Offset: 0x00280593
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x00282396 File Offset: 0x00280596
		protected override void OnEnchant()
		{
			base.Card.EnergyCost.UpgradeBy(-base.Card.EnergyCost.GetWithModifiers(CostModifiers.None));
		}
	}
}
