using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D5 RID: 2261
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Goopy : EnchantmentModel
	{
		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x06006C5A RID: 27738 RVA: 0x00281C5A File Offset: 0x0027FE5A
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x06006C5B RID: 27739 RVA: 0x00281C5D File Offset: 0x0027FE5D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06006C5C RID: 27740 RVA: 0x00281C6A File Offset: 0x0027FE6A
		public override bool CanEnchant(CardModel card)
		{
			return base.CanEnchant(card) && card.Tags.Contains(CardTag.Defend);
		}

		// Token: 0x06006C5D RID: 27741 RVA: 0x00281C83 File Offset: 0x0027FE83
		protected override void OnEnchant()
		{
			base.Card.AddKeyword(CardKeyword.Exhaust);
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x00281C94 File Offset: 0x0027FE94
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card != base.Card)
			{
				return Task.CompletedTask;
			}
			int num = base.Amount;
			base.Amount = num + 1;
			if (base.Card.DeckVersion != null)
			{
				EnchantmentModel enchantment = base.Card.DeckVersion.Enchantment;
				num = enchantment.Amount;
				enchantment.Amount = num + 1;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006C5F RID: 27743 RVA: 0x00281CF6 File Offset: 0x0027FEF6
		public override decimal EnchantBlockAdditive(decimal originalBlock)
		{
			return base.Amount - 1;
		}
	}
}
