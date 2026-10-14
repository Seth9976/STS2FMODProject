using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F5 RID: 2549
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Havoc : CardModel
	{
		// Token: 0x060072C9 RID: 29385 RVA: 0x0028E017 File Offset: 0x0028C217
		public Havoc()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x0028E024 File Offset: 0x0028C224
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner, 1, CardPilePosition.Top, true);
		}

		// Token: 0x17001E7A RID: 7802
		// (get) Token: 0x060072CB RID: 29387 RVA: 0x0028E06F File Offset: 0x0028C26F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x060072CC RID: 29388 RVA: 0x0028E07C File Offset: 0x0028C27C
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
