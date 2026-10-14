using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AB0 RID: 2736
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scrawl : CardModel
	{
		// Token: 0x060076B3 RID: 30387 RVA: 0x00296400 File Offset: 0x00294600
		public Scrawl()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002016 RID: 8214
		// (get) Token: 0x060076B4 RID: 30388 RVA: 0x0029640D File Offset: 0x0029460D
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x00296418 File Offset: 0x00294618
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			int num = CardPile.MaxCardsInHand - base.Owner.PlayerCombatState.Hand.Cards.Count;
			await CardPileCmd.Draw(choiceContext, num, base.Owner, false);
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x00296463 File Offset: 0x00294663
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
