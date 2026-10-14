using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C1 RID: 1729
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PagestormPower : PowerModel
	{
		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x060059F0 RID: 23024 RVA: 0x00251584 File Offset: 0x0024F784
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x060059F1 RID: 23025 RVA: 0x00251587 File Offset: 0x0024F787
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x060059F2 RID: 23026 RVA: 0x0025158A File Offset: 0x0024F78A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x00251598 File Offset: 0x0024F798
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (card.Keywords.Contains(CardKeyword.Ethereal))
				{
					base.Flash();
					await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player, false);
				}
			}
		}
	}
}
