using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008DE RID: 2270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Slither : EnchantmentModel
	{
		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x06006C87 RID: 27783 RVA: 0x00281FB3 File Offset: 0x002801B3
		// (set) Token: 0x06006C88 RID: 27784 RVA: 0x00281FBB File Offset: 0x002801BB
		public int TestEnergyCostOverride
		{
			get
			{
				return this._testEnergyCostOverride;
			}
			set
			{
				if (TestMode.IsOff)
				{
					throw new InvalidOperationException("Only set this value in test mode.");
				}
				base.AssertMutable();
				this._testEnergyCostOverride = value;
			}
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x00281FDC File Offset: 0x002801DC
		public override bool CanEnchant(CardModel card)
		{
			return base.CanEnchant(card) && !card.Keywords.Contains(CardKeyword.Unplayable) && !card.EnergyCost.CostsX;
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x00282008 File Offset: 0x00280208
		public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card != base.Card)
			{
				return Task.CompletedTask;
			}
			if (base.Card.Pile.Type != PileType.Hand)
			{
				return Task.CompletedTask;
			}
			base.Card.EnergyCost.SetThisCombat(this.NextEnergyCost(), false);
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard != null)
			{
				ncard.PlayRandomizeCostAnim();
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x00282073 File Offset: 0x00280273
		private int NextEnergyCost()
		{
			if (this.TestEnergyCostOverride >= 0)
			{
				return this.TestEnergyCostOverride;
			}
			return base.Card.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
		}

		// Token: 0x04002836 RID: 10294
		private int _testEnergyCostOverride = -1;
	}
}
